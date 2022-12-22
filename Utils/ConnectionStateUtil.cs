using Android.Graphics;
using Android.Widget;
using ReMouse.Network;

namespace ReMouse.Utils
{
    public static class ConnectionStateUtil
    {
        private static TextView _textView;

        private static Color _disconnectColor = Color.ParseColor("#CF000F");
        private static Color _inprogressColor = Color.ParseColor("#F7CA18");
        private static Color _connectedColor = Color.ParseColor("#26A65B");

        static ConnectionStateUtil()
        {
            ConnectionSocket.Instance.OnConnect += OnConnection;
            ConnectionSocket.Instance.OnDisconnect += OnDisconnection;
            ConnectionSocket.Instance.OnTryConnection += OnTryConnection;
        }

        public static void SetTextView(TextView textView)
        {
            _textView = textView;
        }

        public static void Update()
        {
            OnConnection(ConnectionSocket.Instance.Connected);
        }

        private static void OnTryConnection()
        {
            _textView?.SetText(Resource.String.con_state_inprogress);
            _textView?.SetBackgroundColor(_inprogressColor);
        }

        private static void OnDisconnection()
        {
            _textView?.SetText(Resource.String.con_state_disconnected);
            _textView?.SetBackgroundColor(_disconnectColor);
        }

        private static void OnConnection(bool connected)
        {
            if (connected)
            {
                _textView?.SetText(Resource.String.con_state_connected);
                _textView?.SetBackgroundColor(_connectedColor);
            }
            else
            {
                _textView?.SetText(Resource.String.con_state_disconnected);
                _textView?.SetBackgroundColor(_disconnectColor);
            }
        }
    }
}