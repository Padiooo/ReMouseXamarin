using System;
using System.Threading.Tasks;

namespace ReMouse
{
    public interface ISocketConnection
    {
        event Action OnTryConnection;
        event Action<bool> OnConnect;
        event Action OnDisconnect;
        event Action<byte[]> OnReceive;

        Task Connect(string host = "");

        Task Disconnect();

        Task Send(byte[] data);
    }
}