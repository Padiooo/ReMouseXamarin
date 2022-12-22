
using Android.Views;
using System;

namespace ReMouse.Utils.Mouse.Working
{
    public interface IWorkingButton
    {
        void OnTouchEvent(object sender, View.TouchEventArgs e);

        void OnClickEvent(object sender, EventArgs e);

        void OnLongClickEvent(object sender, View.LongClickEventArgs e);

        void OnVisibilityChangeEvent(ViewStates state);

        void Delete();
    }
}