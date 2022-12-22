using Android.Graphics.Drawables;
using Newtonsoft.Json;
using RemoseNetwork.Packets;
using System;

namespace ReMouse.Data.Holders
{
    [Serializable]
    public class ButtonDisplayableProperties : DisplayablePropertiesBase
    {
        public override bool IsEnable { get; set; }
        public override bool IsInBox { get; set; }
        public override bool IsSelected { get; set; }
        public override int PosX { get; set; }
        public override int PosY { get; set; }
        public override int Size { get; set; }
        public override int Progress { get; set; } = 50;
        [JsonIgnore]
        public override Drawable Drawable
        {
            get
            {
                int id;
                try
                {
                    if ((int)Type <= 10)
                        id = (int)typeof(Resource.Drawable).GetField(Type.ToString().ToLower()).GetValue(null);
                    else
                        id = (int)typeof(Resource.Drawable).GetField(Type.ToString().ToLower()).GetValue(null);
                }
                catch (Exception e)
                {
                    id = (int)typeof(Resource.Drawable).GetField("erase").GetValue(null);
                }
                return DataManager.Instance.GetDrawable(id);
            }
            set { }
        }

        public ButtonType Type { get; set; }

        public override string Name => Type.ToString();

        public ButtonDisplayableProperties(string type) : base()
        {
            Type = Enum.Parse<ButtonType>(type);
        }

        public override string SaveAsJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}