using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using Game.Modules;
using System.Diagnostics;

namespace Game.Settings
{
    public class SettingsManager
    {
        private static string _path = "settings.xml";
        public static Dictionary<string, Dictionary<string, Tuple<string, string>>> AppSettings;
        private static List<ISettingUser> _users;

        public static Dictionary<string, Tuple<string, string>> Value( string key ) {
            Dictionary<string, Tuple<string, string>> result;
            AppSettings.TryGetValue(key, out result);

            return result ?? new Dictionary<string, Tuple<string, string>>();
        }

        public static object GetSettings( string module, string key ) {
            if(AppSettings == null)
                return string.Empty;
            try
            {
                var item = AppSettings[module][key];
                return ConvertFromString(item.Item1, item.Item2);
            }
            catch(Exception e) {
                return string.Empty;
            }
        }

        public static string GetValueFromArray( string module, string key ) {
            if(AppSettings == null)
                return string.Empty;
            try
            {
                var item = AppSettings[module][key];
                var data = ConvertFromString(item.Item1, "list_string");
                if(data is List<string> && (data as List<string>).Count > 1)
                    return (data as List<string>)[0];
                return string.Empty;
            }
            catch(Exception e)
            {
                return string.Empty;
            }
        }

        public static void SetValueOfArray( string module, string key, string value ) {
            try
            {
                if(!AppSettings.ContainsKey(module)) {
                    AppSettings[module] = new Dictionary<string, Tuple<string, string>>();
                }
                if(!AppSettings[module].ContainsKey(key)) {
                    AppSettings[module][key] = new Tuple<string, string>(string.Empty, "list_string");
                }
                var old = AppSettings[module][key].Item1;
                if(old.Contains(value)) {
                    old = value + "," + old.Replace(value, "");
                }
                AppSettings[module][key] = new Tuple<string, string>(old, "list_string");
            }
            catch (Exception e) {

            }
        }


        public static void SetSetting( string module, string key, object value ) {
            try
            {
                if(!AppSettings.ContainsKey(module))
                {
                    AppSettings[module] = new Dictionary<string, Tuple<string, string>>();
                }
                if(!AppSettings[module].ContainsKey(key))
                {
                    AppSettings[module][key] = new Tuple<string, string>(string.Empty, "list_string");
                }
                AppSettings[module][key] = ConvertToString(value);
            }
            catch(Exception e) {
                Debug.Write("Cannot set setting " + key + " from module " + module);
            }
        }
        
        private static string divider, divider2, divider3;

        public static object ConvertFromString( string data, string typeName ) {
            if(typeName == "string")
                return data;
            else if(typeName == "bool")
                return data == "1" || data == "true" ? true : false;
            else if(typeName == "list_string")
                return data.Split(new string[] { ","}, StringSplitOptions.RemoveEmptyEntries).ToList();
            else if(typeName == "int")
                return int.Parse(data);
            else if(typeName == "double")
                return double.Parse(data);

            return data;
        }

        public static Tuple<string, string> ConvertToString( object data ) {
            var type = data.GetType();
            if(type == typeof(string))
                return new Tuple<string, string>(data.ToString(), "string");
            else if(type == typeof(int))
                return new Tuple<string, string>(data.ToString(), "int");
            else if(type == typeof(bool))
                return new Tuple<string, string>((bool)data ? "1" : "0", "bool");
            else if(type == typeof(double))
                return new Tuple<string, string>(data.ToString(), "double");
            else if(type == typeof(List<string>))
                return new Tuple<string, string>(string.Join(",", (data as List<string>)), "list_string");

            return new Tuple<string, string>(data.ToString(), "string");

        }

        static SettingsManager() {
            divider = "!@#$%^";
            divider2 = "*&^";
            divider3 = "_)(";
            Task.Run(( ) =>
           {
               try
               {
                   string lastName;
                   AppSettings = new Dictionary<string, Dictionary<string, Tuple<string, string>>>();
                   if(File.Exists(_path))
                   {
                       var options = File.ReadAllText(_path).Split(new string[] { divider },
                           StringSplitOptions.RemoveEmptyEntries);
                       foreach(var option in options)
                       {
                           if(option.Contains(divider2))
                           {
                               var data = option.Split(new string[] { divider2 },
                               StringSplitOptions.RemoveEmptyEntries);

                               var dict = new Dictionary<string, Tuple<string, string>>();
                               foreach(var blob in data)
                               {
                                   var arr = blob.Split(new string[] { divider3 }, StringSplitOptions.RemoveEmptyEntries);
                                   if(arr.Length < 2) continue;
                                   dict[arr[0]] = new Tuple<string, string> (arr[1], arr[2]);
                               }
                               AppSettings[option] = dict;
                           }
                           else
                           {
                               lastName = option;
                           }


                       }
                   }
               }
               catch(Exception e) {
                   AppSettings = new Dictionary<string, Dictionary<string, Tuple<string, string>>>();
                   Debug.Write("Error while loading settings: " + e.Message);
                   throw;
               } 
           });
        }

        public static void RegisterUser(ISettingUser user) {
            if(user == null)
            {
                Debug.Write("Settings user is null, ignore them.");
                return;
            }
            user.QueryForSettings = ( ) => {
                return AppSettings.FirstOrDefault(x => x.Key == user.SettingUserName).Value;
            };
            _users.Add(user);
        }

        public static void Save( ) {
            try
            {
                var build = new StringBuilder();
                foreach(var module in AppSettings) {
                    build.Append(module.Key + divider);
                    build.Append(string.Join(divider2, module.Value.Select(x => x.Key + divider3 + x.Value.Item1 + divider3 + x.Value.Item2)));
                    build.Append(divider);
                }

                File.WriteAllText(_path, build.ToString());
            }
            catch(Exception e) {
                Debug.Write("Error while saving: " + e.Message);
                throw;
            }
        }


    }
}
