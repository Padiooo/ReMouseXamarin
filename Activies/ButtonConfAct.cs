using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using RemoseNetwork.Packets;
using ReMouse.Data;
using ReMouse.Data.Holders;
using ReMouse.Utils;
using ReMouse.Utils.Extensions;
using System;
using System.Collections.Generic;
using Yuku.AmbilWarna;

namespace ReMouse.Activies
{
    [Activity(Label = "ButtonConfAct")]
    public class ButtonConfAct : Activity
    {
        private int ARGB
        {
            set
            {
                _color = new Color(value);
                _colorView.SetBackgroundColor(_color);
                string s = string.Format("#{0:X2}{1:X2}{2:X2}", _color.R, _color.G, _color.B); ;
                _editColor.Text = s;
            }
        }
        private Color _color;
        private View _colorView;
        private EditText _editColor;

        private TextView _title;
        private SeekBar _scaler;
        private ImageView _imageView;
        private CheckBox _enable, _inBox;

        private int _size;

        private List<DisplayablePropertiesBase> selected = new List<DisplayablePropertiesBase>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            LayoutDisplayTracker.SetLayout(this, Resource.Layout.button_config, Resource.Id.linearLayout);

            _colorView = FindViewById(Resource.Id.view1);
            _title = FindViewById<TextView>(Resource.Id.button_config_title);
            _editColor = FindViewById<EditText>(Resource.Id.editText1);
            _enable = FindViewById<CheckBox>(Resource.Id.checkBox1);
            _inBox = FindViewById<CheckBox>(Resource.Id.checkBox2);
            _scaler = FindViewById<SeekBar>(Resource.Id.seekBar1);
            _imageView = FindViewById<ImageView>(Resource.Id.imageView1);


            _scaler.ProgressChanged += OnProgressChanged;
            _colorView.Click += ColorClick;
            _editColor.TextChanged += ColorTextChanged;


            selected = DisplayablePropertiesBase.GetSelected();

            if (selected.Count == 1)
            {
                DisplayablePropertiesBase button = selected[0];
                string name = button.Name;
                try
                {
                    ButtonType buttonType = Enum.Parse<ButtonType>(button.Name);
                    name = buttonType.GetName();
                }
                catch
                {

                }
                _title.Text = name;
                _editColor.Text = button.Color;
                _enable.Checked = button.IsEnable;
                _inBox.Checked = button.IsInBox;
                _colorView.SetBackgroundColor(Color.ParseColor(button.Color));
                _scaler.Progress = button.Progress;
                _imageView.SetImageDrawable(button.Drawable);
            }
            else
            {
                bool enable = true, inBox = true, color = true, progress = true;
                string oldColor = null;
                int oldProgress = 0;
                foreach (var item in selected)
                {
                    enable = item.IsEnable && enable;
                    inBox = item.IsInBox && inBox;
                    color = oldColor == null || (oldColor == item.Color && color);
                    oldColor = item.Color;
                    color = oldProgress == 0 || (oldProgress == item.Progress && progress);
                    oldProgress = item.Progress;
                }
                if (enable) _enable.Checked = true;
                if (inBox) _inBox.Checked = true;
                if (color) ARGB = Color.ParseColor(oldColor).ToArgb();
                else ARGB = 0;
                if (progress) _scaler.Progress = oldProgress;
                else _scaler.Progress = 50;
            }
            MathImageSize(_scaler.Progress);

            FindViewById<Button>(Resource.Id.button1).Click += (sender, args) =>
            {
                Finish();
            };
        }

        private void ColorTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (e.Text.ToString().Length == 7)
            {
                _colorView.SetBackgroundColor(Color.ParseColor(e.Text.ToString()));
            }
        }

        private void ColorClick(object sender, EventArgs e)
        {
            OpenColorPicker(false);
        }

        private void OnProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            MathImageSize(e.Progress);
        }

        private void MathImageSize(int progress)
        {
            double scaling = (0.99 / 50f) * progress - 0.99;
            _size = (int)(150 * (1 + scaling));
            _imageView.LayoutParameters.Width = _size;
            _imageView.LayoutParameters.Height = _size;
            _imageView.RequestLayout();
        }

        private void OpenColorPicker(bool alphaSupport)
        {
            AmbilWarnaDialog dialog = new AmbilWarnaDialog(this, _color.ToArgb(), alphaSupport);
            dialog.Ok += (sender, args) =>
            {
                ARGB = args.Color;
            };
            dialog.Cancel += (sender, args) =>
            {

            };
            dialog.Show();
        }

        protected override void OnDestroy()
        {
            foreach (var item in selected)
            {
                item.IsInBox = _inBox.Checked;
                item.IsEnable = _enable.Checked;
                item.Progress = _scaler.Progress;
                item.Size = _size;
                item.Color = _editColor.Text;
            }

            DataManager.Instance.SaveAll().Wait();
            base.OnDestroy();
        }
    }
}