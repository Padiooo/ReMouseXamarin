using Android.Graphics.Drawables;
using RemoseNetwork.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReMouse.Data.Holders
{
    public abstract class DisplayablePropertiesBase : IDisplayable, IData
    {
        private static readonly Dictionary<string, DisplayablePropertiesBase> _displayables = new Dictionary<string, DisplayablePropertiesBase>();

        public abstract bool IsEnable { get; set; }
        public abstract bool IsInBox { get; set; }
        public abstract bool IsSelected { get; set; }
        public abstract int PosX { get; set; }
        public abstract int PosY { get; set; }
        public abstract int Size { get; set; }
        public abstract int Progress { get; set; }
        private string _color = "#000000";
        public string Color
        {
            get => _color;
            set
            {
                _color = value;
                if (Drawable != null)
                    Drawable.SetTint(Android.Graphics.Color.ParseColor(_color));
            }
        }
        public abstract Drawable Drawable { get; set; }

        public abstract string Name { get; }

        protected DisplayablePropertiesBase()
        {

        }

        public static T Get<T>(string name) where T : DisplayablePropertiesBase
        {
            if (!_displayables.TryGetValue(name, out DisplayablePropertiesBase value))
            {
                if (DataManager.Keys.Contains(name))
                {
                    Task<T> t = DataManager.Instance.LoadObject<T>(name);
                    t.Wait();
                    value = t.Result;
                }
                else
                {
                    value = (T)Activator.CreateInstance(typeof(T), name);
                }
                _displayables.Add(name, value);
                DataManager.datas.Add(value);
            }

            return (T)value;
        }

        public static void BuildAll()
        {
            foreach (ButtonType type in Enum.GetValues(typeof(ButtonType)))
            {
                if (type == ButtonType.Settings || type == ButtonType.System_Volume_Mute) continue;
                Get<ButtonDisplayableProperties>(type.ToString());
            }
            Get<BoxDisplayableProperties>("Box");
        }

        public static List<DisplayablePropertiesBase> GetEnabled()
        {
            List<DisplayablePropertiesBase> ts = new List<DisplayablePropertiesBase>();

            foreach (var item in _displayables.Values)
            {
                if (item.IsEnable)
                    ts.Add(item);
            }

            return ts;
        }

        public static List<DisplayablePropertiesBase> GetSelected()
        {
            List<DisplayablePropertiesBase> ts = new List<DisplayablePropertiesBase>();

            foreach (var item in _displayables.Values)
            {
                if (item.IsSelected)
                    ts.Add(item);
            }

            return ts;
        }

        public abstract string SaveAsJson();
    }
}