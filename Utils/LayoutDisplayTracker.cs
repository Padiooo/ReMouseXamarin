using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using ReMouse.Data.Holders;
using System;
using System.Collections.Generic;

namespace ReMouse.Utils
{
    public static class LayoutDisplayTracker
    {
        private static AppData _holder = AppData.Get();

        public static void SetLayout(Activity activity, int contentId, int layoutId)
        {
            activity.SetContentView(contentId);
            LayoutChanged(activity.FindViewById(layoutId));
        }

        public static void LayoutChanged(View layout)
        {
            List<View> views = GetViews(layout, null);
            foreach (var view in views)
            {
                SetupView(view);
            }
        }

        public static void SetupView(View v)
        {
            if (v is ViewGroup vg)
            {
                vg.SetBackgroundColor(Color.ParseColor(_holder.BackgroundColor));
            }
            else if (v is Button bt)
            {
                bt.SetTextColor(Color.ParseColor(_holder.BackgroundColor));
                bt.SetBackgroundColor(Color.ParseColor(_holder.ForegroundColor));
            }
            else if (v is ImageView iv)
            {
                iv.SetColorFilter(Color.ParseColor(_holder.ForegroundColor));
                iv.LayoutParameters.Height = _holder.ButtonHeight;
                iv.LayoutParameters.Width = _holder.ButtonHeight;
                iv.RequestLayout();
            }
            else if (v is TextView tv)
            {
                tv.SetTextColor(Color.ParseColor(_holder.ForegroundColor));
                tv.SetBackgroundColor(Color.ParseColor(_holder.BackgroundColor));
            }
            else if (v is EditText ed)
            {
                ed.SetTextColor(Color.ParseColor(_holder.ForegroundColor));
            }

        }

        public static void SetupView(ImageView imageView, int size, Color color)
        {
            imageView.SetColorFilter(color);
            imageView.LayoutParameters.Height = size;
            imageView.LayoutParameters.Width = size;
            try
            {
                imageView.RequestLayout();
            }
            catch
            {

            }
        }

        private static List<View> GetViews(View view, Type viewtypes)
        {
            List<View> result = new List<View>();
            if (view is ViewGroup vg)
            {
                result.Add(view);

                for (int i = 0; i < vg.ChildCount; i++)
                {
                    View child = vg.GetChildAt(i);
                    result.AddRange(GetViews(child, viewtypes));
                }
            }
            else
            {
                if (view.Tag == null)
                {
                    if (viewtypes == null)
                    {
                        result.Add(view);
                    }
                    else
                    {
                        if (view.GetType() == viewtypes)
                        {
                            result.Add(view);
                        }
                    }
                }
                else
                {
                    if (view.Tag.ToString() != "cancel")
                    {
                        if (viewtypes == null)
                        {
                            result.Add(view);
                        }
                        else
                        {
                            if (view.GetType() == viewtypes)
                            {
                                result.Add(view);
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}