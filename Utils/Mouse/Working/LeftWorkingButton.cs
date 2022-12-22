using Android.Views;
using RemoseNetwork.Packets;
using RemoseNetwork.Packets.Specialized;
using System;

namespace ReMouse.Utils.Mouse.Working
{
    public class LeftWorkingButton : WorkingButtonBase
    {
        private readonly BehaviouralPacket click = new BehaviouralPacket(ButtonType.Left, ButtonBehaviour.CLICK);
        private readonly BehaviouralPacket holdOrRelease = new BehaviouralPacket(ButtonType.Left, ButtonBehaviour.HOLDORRELEASE);

        public LeftWorkingButton(View v, bool isHideable) : base(v, ButtonType.Left, isHideable)
        {

        }

        public override void OnClickEvent(object sender, EventArgs e)
        {
            Send(click);
        }

        public override void OnLongClickEvent(object sender, View.LongClickEventArgs e)
        {
            Send(holdOrRelease);
        }
    }
}