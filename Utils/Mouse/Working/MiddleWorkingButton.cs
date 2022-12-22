using Android.Views;
using RemoseNetwork.Packets;
using RemoseNetwork.Packets.Specialized;
using System;

namespace ReMouse.Utils.Mouse.Working
{
    public class MiddleWorkingButton : WorkingButtonBase
    {
        private readonly BehaviouralPacket packet;

        public MiddleWorkingButton(View v, ButtonType type, bool isHided) : base(v, type, isHided)
        {
            packet = new BehaviouralPacket(type, ButtonBehaviour.DEFAULT);
        }

        public override void OnClickEvent(object sender, EventArgs e)
        {
            Send(packet);
        }
    }
}