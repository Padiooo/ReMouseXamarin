using Newtonsoft.Json;
using System;
using System.Linq;

namespace ReMouse.Data.Holders
{
    [Serializable]
    public class AppData : IData
    {
        private static AppData _instance;
        public string Name => "AppData";

        public AppData()
        {
            DataManager.datas.Add(this);
        }

        public static AppData Get()
        {
            if (_instance == null)
            {
                if (DataManager.Keys.Contains("AppData"))
                {
                    _instance = DataManager.Instance.LoadObject<AppData>("AppData").Result;
                }
                else
                {
                    _instance = new AppData();
                }
            }
            return _instance;
        }

        public int MouseSpeed { get; set; }
        private string _foregroundColor = "#000000";
        public string ForegroundColor
        {
            get => _foregroundColor.ToUpper();
            set => _foregroundColor = value;
        }
        private string _backgroundColor = "#FFFFFF";
        public string BackgroundColor
        {
            get => _backgroundColor.ToUpper();
            set => _backgroundColor = value;
        }

        public string IPAddress { get; set; }
        [JsonIgnore]
        private const int _buttonDefaultHeight = 200;
        [JsonIgnore]
        private const double _maxScaling = 0.99;
        public double scaling;
        public int ButtonHeight => (int)(_buttonDefaultHeight * (1 + scaling));

        public void SetScaling(int progress)
        {
            scaling = (_maxScaling / 50f) * progress - _maxScaling;
        }

        public string SaveAsJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}