using Android.Views;
using RemoseNetwork.Packets;
using RemoseNetwork.Packets.Specialized;
using ReMouse.Network;
using System;
using System.Collections.Generic;

namespace ReMouse.Utils.Mouse.Working
{
    public abstract class WorkingButtonBase : IWorkingButton
    {
        public static List<WorkingButtonBase> activeButtons = new List<WorkingButtonBase>();

        protected View _view;
        protected bool _isHideable;

        public ButtonType Type { get; }

        protected WorkingButtonBase(View view, ButtonType type, bool isHideable)
        {
            activeButtons.Add(this);
            _view = view;
            Type = type;

            _view.Click += OnClickEvent;
            _view.LongClick += OnLongClickEvent;
            _view.Touch += OnTouchEvent;

            if (isHideable)
            {
                _isHideable = isHideable;
                BoxWorkingButton.OnVisibilityChange += OnVisibilityChangeEvent;
            }
        }

        public virtual void OnClickEvent(object sender, EventArgs e)
        {
            return;
        }

        public virtual void OnLongClickEvent(object sender, View.LongClickEventArgs e)
        {
            e.Handled = false;
            return;
        }

        public virtual void OnTouchEvent(object sender, View.TouchEventArgs e)
        {
            e.Handled = false;
            return;
        }

        public virtual void OnVisibilityChangeEvent(ViewStates state)
        {
            _view.Visibility = state;
        }

        public virtual void Delete()
        {
            _view.Click -= OnClickEvent;
            _view.LongClick -= OnLongClickEvent;
            _view.Touch -= OnTouchEvent;

            if (_isHideable)
                BoxWorkingButton.OnVisibilityChange -= OnVisibilityChangeEvent;

            activeButtons.Remove(this);
        }

        protected void Send(BehaviouralPacket packet)
        {
            ConnectionSocket.Instance.Send(packet.BytesToSend());
        }
    }
}