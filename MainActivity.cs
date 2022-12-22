using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using ReMouse.Activies;
using ReMouse.Data;
using ReMouse.Data.Holders;
using ReMouse.Utils;
using System;

namespace ReMouse
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleInstance)]
    public class MainActivity : AppCompatActivity
    {
        private static InputMethodManager manager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            //Load all last data saved
            DataManager.Instance.Init(this);
            DataManager.Instance.LoadAll().Wait();
            DisplayablePropertiesBase.BuildAll();

            LayoutDisplayTracker.SetLayout(this, Resource.Layout.display_choices, Resource.Id.layout_choices);

            LinearLayout wifi = FindViewById<LinearLayout>(Resource.Id.wifi);
            wifi.Click += ClickedWifi;

            LinearLayout bluetooth = FindViewById<LinearLayout>(Resource.Id.bluetooth);
            bluetooth.Click += ClickedBluetooth;

            LinearLayout config = FindViewById<LinearLayout>(Resource.Id.config);
            config.Click += ClickedConfig;

            manager = (InputMethodManager)ApplicationContext.GetSystemService(InputMethodService);
        }

        private void ClickedWifi(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(WifiConAct));
            StartActivity(intent);
        }

        private void ClickedBluetooth(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(BluetoothConAct));
            StartActivity(intent);
        }

        private void ClickedConfig(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MouseConfigAct));
            StartActivity(intent);
        }

        public static void HideKeyboard(View view)
        {
            manager.HideSoftInputFromWindow(view.WindowToken, HideSoftInputFlags.None);
        }

        public static void ShowKeyboard(View view)
        {
            manager.ShowSoftInput(view, ShowFlags.Forced);
        }

        #region Activity Methods

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnDestroy()
        {
            DataManager.Instance.SaveAll().Wait();

            base.OnDestroy();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            LayoutDisplayTracker.LayoutChanged(FindViewById(Resource.Id.layout_choices));
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        #endregion
    }
}