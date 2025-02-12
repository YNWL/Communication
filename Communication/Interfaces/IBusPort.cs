﻿using Communication.Exceptions;

namespace Communication.Interfaces
{
    /// <summary>
    /// 总线接口
    /// </summary>
    public interface IBusPort : IAsyncDisposable
    {
        /// <summary>
        /// 物理口
        /// </summary>
        IPhysicalPort PhysicalPort { get; set; }

        /// <summary>
        /// 发出的数据
        /// </summary>
        event SentDataEventHandler<byte[]> OnSentData;

        /// <summary>
        /// 数据接收
        /// </summary>
        event ReceiveOriginalDataEventHandler OnReceiveOriginalData;

        /// <summary>
        /// 对端掉线
        /// </summary>
        event DisconnectEventHandler OnDisconnect;

        /// <summary>
        /// 对端连接成功
        /// </summary>
        event ConnectEventHandler OnConnect;

        /// <summary>
        /// 打开
        /// </summary>
        /// <exception cref="ConnectFailedException"></exception>
        Task OpenAsync();

        /// <summary>
        /// 关闭
        /// </summary>
        /// <exception cref="TimeoutException"></exception>
        Task CloseAsync();

        /// <summary>
        /// 数据发送
        /// </summary>
        /// <param name="data">要发送的字节数组</param>
        /// <param name="timeInterval">发送后强制间隔时间(单位毫秒)</param>
        Task SendAsync(byte[] data, int timeInterval = 0);
    }
}
