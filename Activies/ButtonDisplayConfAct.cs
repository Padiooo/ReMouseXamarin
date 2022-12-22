using Android.App;
using Android.OS;
using Android.Widget;
using ReMouse.Data;
using ReMouse.Data.Holders;
using ReMouse.Utils;
using ReMouse.Utils.Mouse.Display;
using System.Collections.Generic;

namespace ReMouse.Activies
{
    [Activity(Label = "ButtonDisplayConfAct")]
    public class ButtonDisplayConfAct : Activity
    {
        private List<DraggableButton> draggables = new List<DraggableButton>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            LayoutDisplayTracker.SetLayout(this, Resource.Layout.mouse, Resource.Id.mouse);
            // Create your application here

            RelativeLayout mouse = FindViewById<RelativeLayout>(Resource.Id.mouse);

            foreach (var button in DisplayablePropertiesBase.GetEnabled())
            {
                ImageView imageView = new ImageView(this);
                mouse.AddView(imageView);
                imageView.RequestLayout();
                imageView.LayoutParameters.Width = button.Size;
                imageView.LayoutParameters.Height = button.Size;
                draggables.Add(new DraggableButton(button, imageView));

            }
        }

        protected override void OnDestroy()
        {
            DataManager.Instance.SaveAll().Wait();
            draggables.ForEach((draggable) => draggable.Delete());
            base.OnDestroy();
        }
    }
}