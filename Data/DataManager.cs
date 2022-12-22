using Android.Content;
using Android.Graphics.Drawables;
using Android.Preferences;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReMouse.Data
{
    public class DataManager
    {
        #region Singleton
        private ISharedPreferences _sharedPreferences;
        private Context _context;

        private static DataManager _instance;
        public static DataManager Instance
        {
            get
            {
                if (_instance == null) _instance = new DataManager();
                return _instance;
            }
        }

        private DataManager()
        {

        }

        public void Init(Context context)
        {
            _context = context;
            _sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(context);
        }

        #endregion 

        public static IList<IData> datas = new List<IData>();
        private readonly static ConcurrentDictionary<string, string> nameToJson = new ConcurrentDictionary<string, string>();
        public static string[] Keys => nameToJson.Keys.ToArray();

        private readonly static Dictionary<int, Drawable> drawables = new Dictionary<int, Drawable>();

        public Task SaveAll()
        {
            return Task.Run(() =>
            {
                var editor = _sharedPreferences.Edit();
                List<string> keys = new List<string>();
                foreach (IData data in datas)
                {
                    string key = data.Name;
                    string value = data.SaveAsJson();
                    editor.PutString(key, value);
                    keys.Add(key);
                }
                string keysJson = JsonConvert.SerializeObject(keys);
                editor.PutString("old_keys", keysJson);

                editor.Commit();
                editor.Apply();
            });
        }

        public Task LoadAll()
        {
            return Task.Run(() =>
            {
                List<string> keys = JsonConvert.DeserializeObject<List<string>>(_sharedPreferences.GetString("old_keys", ""));

                if (keys == null) return;

                foreach (string key in keys)
                {
                    string json = _sharedPreferences.GetString(key, "");
                    nameToJson.TryAdd(key, json);
                }
            });
        }

        public void SaveObject(IData data)
        {
            string name = data.Name;
            string json = data.SaveAsJson();
            if (nameToJson.ContainsKey(name))
            {
                nameToJson.Remove(name, out string _);
            }
            nameToJson.TryAdd(name, json);
        }

        public Task<T> LoadObject<T>(string name)
        {
            return Task.Factory.StartNew(() =>
            {
                nameToJson.TryGetValue(name, out string value);
                return JsonConvert.DeserializeObject<T>(value);
            });
        }

        public Drawable GetDrawable(int id)
        {
            if (!drawables.TryGetValue(id, out Drawable value))
            {
                value = _context.GetDrawable(id);
                drawables.Add(id, value);
            }
            return value;
        }

        /// <summary>
        /// Where value is [int, float, string, long, bool].
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task SaveProperty(string name, object value)
        {
            return Task.Run(() =>
            {
                var editor = _sharedPreferences.Edit();
                if (value is string @string)
                {
                    editor.PutString(name, @string);
                }
                else if (value is int @int)
                {
                    editor.PutInt(name, @int);
                }
                else if (value is long @long)
                {
                    editor.PutLong(name, @long);
                }
                else if (value is float @float)
                {
                    editor.PutFloat(name, @float);
                }
                else if (value is bool @bool)
                {
                    editor.PutBoolean(name, @bool);
                }
            });
        }

        public Task<object> LoadProperty(string name, Type type)
        {
            return Task.Factory.StartNew<object>(() =>
            {
                object result = null;
                if (type == typeof(string))
                {
                    result = _sharedPreferences.GetString(name, "Property not found");
                }
                else if (type == typeof(int))
                {
                    result = _sharedPreferences.GetInt(name, -1);
                }
                else if (type == typeof(float))
                {
                    result = _sharedPreferences.GetFloat(name, -1f);
                }
                else if (type == typeof(long))
                {
                    result = _sharedPreferences.GetLong(name, -1L);
                }
                else if (type == typeof(bool))
                {
                    result = _sharedPreferences.GetBoolean(name, false);
                }
                return result;
            });
        }
    }
}