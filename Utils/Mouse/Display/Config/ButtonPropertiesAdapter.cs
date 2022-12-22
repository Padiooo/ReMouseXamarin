using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using ReMouse.Data.Holders;
using System;
using System.Collections.Generic;

namespace ReMouse.Utils.Mouse.Display.Config
{
    public class ButtonPropertiesAdapter : ArrayAdapter<DisplayablePropertiesBase>
    {
        public static readonly Color Enabled = Color.ParseColor("#3FC380");
        public static readonly Color Disabled = Color.ParseColor("#D91E18");

        public ButtonPropertiesAdapter(
            Context context,
            int resource,
            int textViewResourceId,
            IList<DisplayablePropertiesBase> objects)
            : base(context, resource, textViewResourceId, objects)
        {

        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = LayoutInflater.From(Context);
            ViewHolder viewHolder = null;
            DisplayablePropertiesBase displayProperties = GetItem(position);
            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.gridview_config_item, parent, false);
                ImageView button = convertView.FindViewById<ImageView>(Resource.Id.item_imageview);
                ImageView tick = convertView.FindViewById<ImageView>(Resource.Id.item_tick);
                viewHolder = new ViewHolder
                {
                    Button = button,
                    Tick = tick,
                    Position = position,
                };
                convertView.Click += OnClik;
                convertView.SetTag(convertView.Id, viewHolder);
            }
            else
            {
                viewHolder = (ViewHolder)convertView.GetTag(convertView.Id);
                viewHolder.Position = position;
            }
            viewHolder.Button.SetImageDrawable(displayProperties.Drawable);
            Color color = displayProperties.IsEnable ? Enabled : Disabled;
            viewHolder.Button.SetColorFilter(color);
            viewHolder.Tick.Visibility = displayProperties.IsSelected ? ViewStates.Visible : ViewStates.Invisible;

            return convertView;
        }

        private void OnClik(object sender, EventArgs args)
        {
            View v = (View)sender;
            ViewHolder viewHolder = (ViewHolder)v.GetTag(v.Id);
            int position = viewHolder.Position;
            DisplayablePropertiesBase displayProperties = GetItem(position);
            displayProperties.IsSelected ^= true;
            viewHolder.Tick.Visibility = displayProperties.IsSelected ? ViewStates.Visible : ViewStates.Invisible;
        }
    }
}