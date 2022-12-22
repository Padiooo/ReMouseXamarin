using Android.Views;
using Android.Widget;
using RemoseNetwork.Packets;
using ReMouse.Data;
using System;

namespace ReMouse.Utils.Mouse.Working
{
    public class BoxWorkingButton : WorkingButtonBase
    {
        private readonly ImageView _imageView;
        private readonly int closed = Resource.Drawable.closed;
        private readonly int open = Resource.Drawable.open;

        private ViewStates _state = ViewStates.Visible;

        public static event Action<ViewStates> OnVisibilityChange;

        public BoxWorkingButton(ImageView v) : base(v, ButtonType.Settings, true)
        {
            _imageView = v;

            OnVisibilityChange?.Invoke(_state);
        }

        public override void OnClickEvent(object sender, EventArgs e)
        {
            _state = _state == ViewStates.Visible ? ViewStates.Gone : ViewStates.Visible;
            OnVisibilityChange?.Invoke(_state);
        }

        public override void OnVisibilityChangeEvent(ViewStates state)
        {
            int id = state == ViewStates.Visible ? open : closed;
            _imageView.SetImageDrawable(DataManager.Instance.GetDrawable(id));
        }
    }
}