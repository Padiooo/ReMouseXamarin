using Android.Views;
using RemoseNetwork.Packets;
using RemoseNetwork.Packets.Specialized;
using System;

namespace ReMouse.Utils.Mouse.Working
{
    public class DefaultWorkingButton : WorkingButtonBase
    {
        private readonly BehaviouralPacket packet;

        public DefaultWorkingButton(View v, ButtonType type, bool isHidable) : base(v, type, isHidable)
        {
            packet = new BehaviouralPacket(type, ButtonBehaviour.DEFAULT);
        }

        public override void OnClickEvent(object sender, EventArgs e)
        {
            Send(packet);
        }
    }
}