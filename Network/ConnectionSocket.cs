using System;
using System.Threading.Tasks;

namespace ReMouse.Network
{
    public class ConnectionSocket : ISocketConnection
    {
        #region Singleton

        private enum HandlerType
        {
            NONE,
            BLUETOOTH,
            WIFI
        }

        private HandlerType _handlerType;

        private static ConnectionSocket _instance;
        public static ConnectionSocket Instance
        {
            get
            {
                if (_instance == null) _instance = new ConnectionSocket();
                return _instance;
            }
        }

        private ConnectionSocket()
        {
            _handlerType = HandlerType.NONE;
        }

        private ISocketConnection _handler;

        public void SwitchToWifi()
        {
            // If we already use WIFI don't change
            if (_handlerType == HandlerType.WIFI) return;
            else _handlerType = HandlerType.WIFI;

            Connected = false;
            if (_handler != null)
            {
                UnBoundEvents();
                _handler.Disconnect();
            }
            _handler = new WifiHandler();
            BoundEvents();
        }

        public void SwitchToBluetooth()
        {
            // If we already use BLUETOOTH don't change
            if (_handlerType == HandlerType.BLUETOOTH) return;
            else _handlerType = HandlerType.BLUETOOTH;

            Connected = false;
            if (_handler != null)
            {
                UnBoundEvents();
                _handler.Disconnect();
            }
            _handler = new BluetoothHandler();
            BoundEvents();
        }

        private void BoundEvents()
        {
            _handler.OnConnect += HandleOnConnect;
            _handler.OnDisconnect += HandleOnDisconnect;
            _handler.OnReceive += HandleOnReceive;
            _handler.OnTryConnection += HandleOnTryConnection;
        }

        private void UnBoundEvents()
        {
            _handler.OnConnect -= HandleOnConnect;
            _handler.OnDisconnect -= HandleOnDisconnect;
            _handler.OnReceive -= HandleOnReceive;
            _handler.OnTryConnection -= HandleOnTryConnection;
        }

        private void HandleOnTryConnection()
        {
            OnTryConnection?.Invoke();
        }

        private void HandleOnReceive(byte[] data)
        {
            OnReceive?.Invoke(data);
        }

        private void HandleOnDisconnect()
        {
            Connected = false;
            OnDisconnect?.Invoke();
        }

        private void HandleOnConnect(bool connected)
        {
            Connected = connected;
            OnConnect?.Invoke(connected);
        }

        #endregion

        public event Action<bool> OnConnect;
        public event Action OnDisconnect;
        public event Action<byte[]> OnReceive;
        public event Action OnTryConnection;

        public bool Connected { get; private set; }

        public Task Connect(string host = "")
        {
            return _handler.Connect(host);
        }

        public Task Disconnect()
        {
            return _handler.Disconnect();
        }

        public Task Send(byte[] data)
        {
            return _handler.Send(data);
        }
    }
}