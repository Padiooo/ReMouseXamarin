using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ReMouse.Network
{
    public class WifiHandler : ISocketConnection
    {
        private Socket _socket;

        private const int _port = 11000;
        private readonly byte[] _byteBuffer = new byte[512];

        private readonly object _mLock = new object();

        private IAsyncResult _read;

        public WifiHandler()
        {
            OnConnect += OnConnection;
            OnDisconnect += OnDisconnection;
        }

        public event Action<byte[]> OnReceive;
        public event Action<bool> OnConnect;
        public event Action OnDisconnect;
        public event Action OnTryConnection;

        private void OnDisconnection()
        {
            try
            {
                _socket.EndReceive(_read);
            }
            catch (Exception)
            {

            }
        }

        private void OnConnection(bool connected)
        {
            if (connected)
            {
                _read = _socket.BeginReceive(_byteBuffer, 0, _byteBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), null);
            }
        }

        public Task Connect(string host = "")
        {
            return Task.Run(() =>
            {
                lock (_mLock)
                {
                    EndPoint endPoint = new IPEndPoint(IPAddress.Parse(host), _port);
                    try
                    {
                        if (_socket != null && _socket.Connected && _socket.RemoteEndPoint == endPoint) return;
                    }
                    catch { }

                    OnTryConnection?.Invoke();

                    _socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        _socket.Connect(endPoint);
                    }
                    catch (Exception) { }
                    OnConnect?.Invoke(_socket.Connected);
                }
            });
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int read = _socket.EndReceive(ar);

                if (read != 0)
                {
                    _socket.BeginReceive(_byteBuffer, 0, _byteBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), null);
                    byte[] data = new byte[read];
                    Array.Copy(_byteBuffer, data, data.Length);
                    OnReceive?.Invoke(data);
                }
                else
                {
                    Disconnect();
                }
            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        public Task Disconnect()
        {
            return Task.Run(() =>
            {
                OnDisconnect?.Invoke();
                Thread.Sleep(500);
                _socket.Close();
                _socket.Dispose();
            });
        }

        public Task Send(byte[] data)
        {
            return Task.Run(() =>
            {
                _socket.Send(data);
            });
        }
    }
}