using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using ReMouse.Network;
using ReMouse.Utils;
using System;
using System.Threading;

namespace ReMouse.Activies
{
    [Activity(Label = "BluetoothConAct", LaunchMode = Android.Content.PM.LaunchMode.SingleInstance)]
    public class BluetoothConAct : Activity
    {
        private Button _button;
        private TextView _connection_state_tv;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            LayoutDisplayTracker.SetLayout(this, Resource.Layout.bluetooth_con, Resource.Id.layout_blue_con);

            ConnectionSocket.Instance.SwitchToBluetooth();

            _button = FindViewById<Button>(Resource.Id.blue_tryconnect);
            _connection_state_tv = FindViewById<TextView>(Resource.Id.connection_status_text);

            ConnectionStateUtil.SetTextView(_connection_state_tv);
            ConnectionStateUtil.Update();

            _button.Click += TryConnect;
            ConnectionSocket.Instance.OnConnect += OnConnectHandler;
        }

        private void OnConnectHandler(bool connected)
        {
            if (connected)
            {
                Thread.Sleep(1000);
                Intent intent = new Intent(this, typeof(MouseAct));
                StartActivity(intent);
            }
        }

        private void TryConnect(object sender, EventArgs e)
        {
            if (ConnectionSocket.Instance.Connected)
                OnConnectHandler(true);
            else
                ConnectionSocket.Instance.Connect();
        }
    }
}