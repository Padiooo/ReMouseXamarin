using Android.Views;
using RemoseNetwork.Packets;
using RemoseNetwork.Packets.Specialized;
using ReMouse.Data.Holders;
using System;

namespace ReMouse.Utils.Mouse.Working
{
    public class MouseWorkingButton : WorkingButtonBase
    {
        private VelocityTracker mVelocityTracker;
        private readonly MouseMovePacket packet = new MouseMovePacket();

        public MouseWorkingButton(View view) : base(view, ButtonType.MoveMouse, false)
        {

        }

        public override void OnClickEvent(object sender, EventArgs e)
        {
            return;
        }

        public override void OnLongClickEvent(object sender, View.LongClickEventArgs e)
        {
            e.Handled = false;
            return;
        }

        public override void OnTouchEvent(object sender, View.TouchEventArgs e)
        {
            int index = e.Event.ActionIndex;
            MotionEventActions action = e.Event.ActionMasked;
            int pointerId = e.Event.GetPointerId(index);

            MainActivity.HideKeyboard(_view);
            _view.RequestFocus();

            switch (action)
            {
                case MotionEventActions.Down:
                    if (mVelocityTracker == null)
                    {
                        // Retrieve a new VelocityTracker object to watch the
                        // velocity of a motion.
                        mVelocityTracker = VelocityTracker.Obtain();
                    }
                    else
                    {
                        // Reset the velocity tracker back to its initial state.
                        mVelocityTracker.Clear();
                    }
                    // Add a user's movement to the tracker.
                    mVelocityTracker.AddMovement(e.Event);
                    break;
                case MotionEventActions.Move:
                    mVelocityTracker.AddMovement(e.Event);
                    // When you want to determine the velocity, call
                    // computeCurrentVelocity(). Then call getXVelocity()
                    // and getYVelocity() to retrieve the velocity for each pointer ID.
                    mVelocityTracker.ComputeCurrentVelocity(1000);
                    // Log velocity of pixels per second
                    // Best practice to use VelocityTrackerCompat where possible.
                    float x = mVelocityTracker.GetXVelocity(pointerId) / 1000;
                    float y = mVelocityTracker.GetYVelocity(pointerId) / 1000;
                    // Send the movements
                    packet.X = x * AppData.Get().MouseSpeed;
                    packet.Y = y * AppData.Get().MouseSpeed;
                    Send(packet);
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    // Return a VelocityTracker object back to be re-used by others.
                    try
                    {
                        mVelocityTracker.Recycle();
                    }
                    catch (Exception)
                    {

                    }
                    break;
            }
        }

        public override void OnVisibilityChangeEvent(ViewStates state)
        {
            return;
        }
    }
}