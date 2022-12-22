using Android.Graphics;
using Android.Views;
using Android.Widget;
using ReMouse.Data.Holders;

namespace ReMouse.Utils.Mouse.Display
{
    public class DraggableButton
    {
        private readonly DisplayablePropertiesBase _properties;
        private readonly ImageView _imageView;

        public DraggableButton(DisplayablePropertiesBase properties, ImageView imageView)
        {
            _properties = properties;
            _imageView = imageView;

            _imageView.SetX(_properties.PosX);
            _imageView.SetY(_properties.PosY);
            _imageView.SetImageDrawable(properties.Drawable);
            _imageView.SetColorFilter(Color.ParseColor(properties.Color));

            _imageView.Touch += OnDragging;
        }

        private void OnDragging(object sender, View.TouchEventArgs e)
        {
            UpdatePosition(e.Event.RawX, e.Event.RawY);
        }

        private void UpdatePosition(float x, float y)
        {
            float delta = _properties.Size / 2;
            x -= delta;
            y -= delta * 2;
            _imageView.SetX(x);
            _imageView.SetY(y);
            _properties.PosX = (int)x;
            _properties.PosY = (int)y;
        }

        public void Delete()
        {
            _imageView.Touch -= OnDragging;
        }
    }
}