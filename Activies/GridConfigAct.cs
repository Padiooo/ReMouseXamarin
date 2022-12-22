using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using RemoseNetwork.Packets;
using ReMouse.Data.Holders;
using ReMouse.Utils;
using ReMouse.Utils.Mouse.Display.Config;
using System;
using System.Collections.Generic;

namespace ReMouse.Activies
{
    [Activity(Label = "GridConfigAct")]
    public class GridConfigAct : Activity
    {
        private List<DisplayablePropertiesBase> objects;
        private GridView _gridView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            LayoutDisplayTracker.SetLayout(this, Resource.Layout.gridview_config, Resource.Id.gridconfig_parent);
            // Create your application here

            _gridView = FindViewById<GridView>(Resource.Id.gridview);

            objects = new List<DisplayablePropertiesBase>();

            foreach (ButtonType type in Enum.GetValues(typeof(ButtonType)))
            {
                if (type == ButtonType.Settings || type == ButtonType.System_Volume_Mute || type == ButtonType.MoveMouse) continue;
                DisplayablePropertiesBase properties = DisplayablePropertiesBase.Get<DisplayablePropertiesBase>(type.ToString());
                objects.Add(properties);
            }
            objects.Add(DisplayablePropertiesBase.Get<DisplayablePropertiesBase>("Box"));

            ButtonPropertiesAdapter adapter = new ButtonPropertiesAdapter(this, Resource.Layout.gridview_config, Resource.Layout.gridview_config, objects);
            _gridView.Adapter = adapter;


            // POSITION
            FindViewById<LinearLayout>(Resource.Id.configure_button_display).Click += (sender, args) =>
            {
                Intent intent = new Intent(this, typeof(ButtonDisplayConfAct));
                StartActivity(intent);
            };
            // PROPERTIES
            FindViewById<LinearLayout>(Resource.Id.configure_button_properties).Click += (sender, args) =>
            {
                Intent intent = new Intent(this, typeof(ButtonConfAct));
                StartActivity(intent);
            };
        }
    }
}