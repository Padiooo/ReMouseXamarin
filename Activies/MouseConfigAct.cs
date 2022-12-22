using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using ReMouse.Data;
using ReMouse.Data.Holders;
using ReMouse.Utils;
using System;
using System.Linq;
using Yuku.AmbilWarna;

namespace ReMouse.Activies
{
    [Activity(Label = "MouseConfigAct")]
    public class MouseConfigAct : Activity
    {
        private EditText _mouse, _foreground, _background;
        private View _foregroundView, _backgroundView;

        private Button _save;

        private AppData _appData;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            LayoutDisplayTracker.SetLayout(this, Resource.Layout.app_config, Resource.Id.layout_app_config);

            _appData = AppData.Get();

            _mouse = FindViewById<EditText>(Resource.Id.mouse_speed);
            _foreground = FindViewById<EditText>(Resource.Id.editText1);
            _foregroundView = FindViewById(Resource.Id.view1);
            _background = FindViewById<EditText>(Resource.Id.editText2);
            _backgroundView = FindViewById(Resource.Id.view2);
            _save = FindViewById<Button>(Resource.Id.save_mouse_config);

            Button _buttonConfig = FindViewById<Button>(Resource.Id.buttons_configuration);
            _buttonConfig.Click += (sender, args) =>
            {
                Intent intent = new Intent(this, typeof(GridConfigAct));
                StartActivity(intent);
            };

            _mouse.TextChanged += OnMouseSpeedChange;
            _foreground.TextChanged += OnForegroundChange;
            _background.TextChanged += OnBackgroundChange;
            _save.Click += OnSave;
            _foregroundView.Click += OnColorPick;
            _backgroundView.Click += OnColorPick;

            _mouse.Text = _appData.MouseSpeed.ToString();
            _background.Text = _appData.BackgroundColor;
            _foreground.Text = _appData.ForegroundColor;
        }

        private void OnColorPick(object sender, EventArgs e)
        {
            View v = (View)sender;
            EditText edit;

            if (v.Id == _foregroundView.Id)
                edit = _foreground;
            else
                edit = _background;

            OpenColorPicker(false, v, edit);
        }

        private void OpenColorPicker(bool alphaSupport, View v, EditText edit)
        {
            AmbilWarnaDialog dialog = new AmbilWarnaDialog(this, 0, alphaSupport);
            dialog.Ok += (sender, args) =>
            {
                int value = args.Color;

                Color color = new Color(value);
                string s = string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B).ToUpper();
                edit.Text = s;
            };
            dialog.Cancel += (sender, args) =>
            {

            };
            dialog.Show();
        }

        private void ChangeViewColor(View v, string color)
        {
            v.SetBackgroundColor(Color.ParseColor(color));
        }

        private void OnSave(object sender, EventArgs e)
        {
            Finish();
        }

        private void OnBackgroundChange(object sender, TextChangedEventArgs e)
        {
            if (e.Text.Count() == 7)
            {
                _appData.BackgroundColor = e.Text.ToString();
                ChangeViewColor(_backgroundView, e.Text.ToString());
            }
        }

        private void OnForegroundChange(object sender, TextChangedEventArgs e)
        {
            if (e.Text.Count() == 7)
            {
                _appData.ForegroundColor = e.Text.ToString();
                ChangeViewColor(_foregroundView, e.Text.ToString());
            }
        }

        private void OnMouseSpeedChange(object sender, TextChangedEventArgs e)
        {
            int value;
            int length = e.Text.ToString().Length;
            if (length == 0)
            {
                value = 0;
            }
            else if (length < 2)
            {
                value = 100;
            }
            else
            {
                value = int.Parse(e.Text.ToString());
            }
            _appData.MouseSpeed = value;
        }

        protected override void OnDestroy()
        {
            DataManager.Instance.SaveAll();
            base.OnDestroy();
        }
    }
}