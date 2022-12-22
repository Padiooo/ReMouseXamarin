using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using ReMouse.Data;
using ReMouse.Data.Holders;
using ReMouse.Network;
using ReMouse.Utils;
using System;
using System.Threading;

namespace ReMouse.Activies
{
    [Activity(Label = "WifiConAct", LaunchMode = Android.Content.PM.LaunchMode.SingleInstance)]
    public class WifiConAct : Activity
    {
        private TextView _connection_state_tv;
        private EditText _ip_address;
        private Button _button;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            LayoutDisplayTracker.SetLayout(this, Resource.Layout.wifi_config, Resource.Id.layout_wifi_con);

            ConnectionSocket.Instance.SwitchToWifi();

            _button = FindViewById<Button>(Resource.Id.wifi_try_connect);
            _ip_address = FindViewById<EditText>(Resource.Id.ip_address);
            _ip_address.Text = AppData.Get().IPAddress;
            _connection_state_tv = FindViewById<TextView>(Resource.Id.connection_state);

            ConnectionStateUtil.SetTextView(_connection_state_tv);

            _ip_address.TextChanged += IpChanged;
            _button.Click += TryConnect;
            ConnectionSocket.Instance.OnConnect += OnConnectHandler;
        }

        private void IpChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            AppData.Get().IPAddress = e.Text.ToString();
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
            AppData data = AppData.Get();
            if (ConnectionSocket.Instance.Connected)
                OnConnectHandler(true);
            else
                ConnectionSocket.Instance.Connect(data.IPAddress);
            DataManager.Instance.SaveObject(data);
        }
    }
}