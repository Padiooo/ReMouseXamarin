using Android.Graphics.Drawables;
using Newtonsoft.Json;

namespace ReMouse.Data.Holders
{
    public class BoxDisplayableProperties : DisplayablePropertiesBase
    {
        public override bool IsEnable { get; set; }
        public override bool IsInBox { get; set; }
        public override bool IsSelected { get; set; }
        public override int PosX { get; set; }
        public override int PosY { get; set; }
        public override int Size { get; set; }
        public override int Progress { get; set; }
        public override Drawable Drawable
        {
            get => DataManager.Instance.GetDrawable(Resource.Drawable.closed);
            set { }
        }

        private readonly string _name = "Box";
        public override string Name => _name;

        public BoxDisplayableProperties(string type)
        {

        }

        public override string SaveAsJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}