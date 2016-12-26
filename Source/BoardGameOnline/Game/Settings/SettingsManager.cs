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
        public static Dictionary<string, Dictionary<string, string>> AppSettings;
        private static List<ISettingUser> _users;

        public static Dictionary<string, string> Value( string key ) {
            Dictionary<string,string> result;
            AppSettings.TryGetValue(key, out result);

            return result ?? new Dictionary<string, string>();
        }

        public static void Value( string key, Dictionary<string, string> value ) {
            AppSettings[key] = value;
        }

        private static string divider, divider2, divider3;
        

        static SettingsManager() {
            divider = "!@#$%^";
            divider2 = "*&^";
            divider3 = "_)(";
            Task.Run(( ) =>
           {
               try
               {
                   string lastName;
                   AppSettings = new Dictionary<string, Dictionary<string, string>>();
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

                               var dict = new Dictionary<string, string>();
                               foreach(var blob in data)
                               {
                                   var arr = blob.Split(new string[] { divider3 }, StringSplitOptions.RemoveEmptyEntries);
                                   if(arr.Length < 2) continue;
                                   dict[arr[0]] = arr[1];
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
                    build.Append(string.Join(divider2, module.Value.Select(x => x.Key + divider3 + x.Value)));
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
