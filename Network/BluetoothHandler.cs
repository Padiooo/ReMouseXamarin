using Android.Bluetooth;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReMouse.Network
{
    public class BluetoothHandler : ISocketConnection
    {
        private BluetoothSocket _socket;

        private readonly UUID service = UUID.FromString("00112233-4455-6677-8899-aabbccddeeff");
        private readonly byte[] _buffer = new byte[512];

        private readonly object _mLock = new object();

        private IAsyncResult _read;

        public BluetoothHandler()
        {
            OnConnect += OnConnection;
            OnDisconnect += OnDisconnection;
        }

        public event Action<byte[]> OnReceive;
        public event Action<bool> OnConnect;
        public event Action OnDisconnect;
        public event Action OnTryConnection;

        public Task Connect(string host = "")
        {
            return Task.Run(() =>
            {
                lock (_mLock)
                {
                    try
                    {
                        if (_socket != null && _socket.IsConnected) return;
                    }
                    catch { }

                    OnTryConnection?.Invoke();

                    bool connected = GetDeviceWithServiceUUID();

                    OnConnect?.Invoke(connected);
                }
            });
        }

        private void OnConnection(bool connected)
        {
            if (connected)
            {
                _read = _socket.InputStream.BeginRead(_buffer, 0, _buffer.Length, new AsyncCallback(ReadCallback), null);
            }
        }

        private void OnDisconnection()
        {
            try
            {
                _socket.InputStream.EndRead(_read);
            }
            catch (Exception)
            {

            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {

                int read = _socket.InputStream.EndRead(ar);

                if (read != 0)
                {
                    _socket.InputStream.BeginRead(_buffer, 0, _buffer.Length, new AsyncCallback(ReadCallback), null);
                    byte[] data = new byte[read];
                    Array.Copy(_buffer, data, data.Length);
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
                _socket.OutputStream.Write(data);
            });
        }

        private bool GetDeviceWithServiceUUID()
        {
            ICollection<BluetoothDevice> bonded = BluetoothAdapter.DefaultAdapter.BondedDevices;

            foreach (var device in bonded)
            {
                _socket = device.CreateRfcommSocketToServiceRecord(service);
                try
                {
                    _socket.Connect();
                    if (_socket.IsConnected) return true;
                }
                catch (Exception) { }
            }

            return false;
        }
    }
}