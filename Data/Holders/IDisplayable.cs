using Android.Graphics.Drawables;
using Newtonsoft.Json;

namespace ReMouse.Data.Holders
{
    public interface IDisplayable
    {
        bool IsEnable { get; set; }
        bool IsInBox { get; set; }
        bool IsSelected { get; set; }
        int PosX { get; set; }
        int PosY { get; set; }
        int Size { get; set; }
        int Progress { get; set; }
        string Color { get; set; }
        [JsonIgnore]
        Drawable Drawable { get; set; }
    }
}