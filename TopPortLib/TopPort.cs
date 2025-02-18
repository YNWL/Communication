﻿using Communication;
using Communication.Bus;
using Communication.Interfaces;
using Parser;
using Parser.Interfaces;
using TopPortLib.Interfaces;

namespace TopPortLib
{
    /// <summary>
    /// 顶层通讯口
    /// </summary>
    public class TopPort : ITopPort, IDisposable
    {
        private readonly IBusPort _port;
        private readonly IParser _parser;

        /// <inheritdoc/>
        public IPhysicalPort PhysicalPort { get => _port.PhysicalPort; set => _port.PhysicalPort = value; }
        /// <inheritdoc/>
        public event SentDataEventHandler<byte[]>? OnSentData { add => _port.OnSentData += value; remove => _port.OnSentData -= value; }
        /// <inheritdoc/>
        public event ReceiveParsedDataEventHandler? OnReceiveParsedData { add => _parser.OnReceiveParsedData += value; remove => _parser.OnReceiveParsedData -= value; }
        /// <inheritdoc/>
        public event DisconnectEventHandler? OnDisconnect { add => _port.OnDisconnect += value; remove => _port.OnDisconnect -= value; }
        /// <inheritdoc/>
        public event ConnectEventHandler? OnConnect { add => _port.OnConnect += value; remove => _port.OnConnect -= value; }

        /// <summary>
        /// 顶层通讯口
        /// </summary>
        /// <param name="physicalPort">物理口</param>
        /// <param name="parser">解析器</param>
        /// <param name="isDisconnectedWhenZero">读到字节数为0是否认为断连</param>
        public TopPort(IPhysicalPort physicalPort, IParser parser, bool isDisconnectedWhenZero = true)
        {
            _parser = parser;
            _port = new BusPort(physicalPort, isDisconnectedWhenZero);
            _port.OnReceiveOriginalData += parser.ReceiveOriginalDataAsync;
        }

        /// <inheritdoc/>
        public async Task CloseAsync()
        {
            await _port.CloseAsync();
        }

        /// <inheritdoc/>
        public async Task OpenAsync(bool reconnectAfterInitialFailure = false)
        {
            await _port.OpenAsync(reconnectAfterInitialFailure);
        }

        /// <inheritdoc/>
        public async Task SendAsync(byte[] data, int timeInterval = 0)
        {
            await _port.SendAsync(data, timeInterval);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_parser is IDisposable needDisposingParser)
            {
                needDisposingParser.Dispose();
            }
            var task = this.CloseAsync();
            task.ConfigureAwait(false);
            task.Wait();
            GC.SuppressFinalize(this);
        }
    }
}
