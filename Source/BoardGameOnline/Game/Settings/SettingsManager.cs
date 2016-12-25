using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace Game.Settings
{
    public class SettingsManager
    {
        private static string _path = "settings.xml"; 
        public static Dictionary<string, string> AppSettings;

        public static string Value( string key ) {
            string result;
            AppSettings.TryGetValue(key, out result);

            return result ?? string.Empty;
        }

        public static void Value( string key, string value ) {
            AppSettings[key] = value;
        }

        private static string divider, divider2;

        static SettingsManager() {
            divider = "!@#$%^";
            divider2 = "*&^";

            AppSettings = new Dictionary<string, string>();
            if(File.Exists(_path))
            {
                var options = File.ReadAllText(_path).Split(new string[] { divider }, 
                    StringSplitOptions.RemoveEmptyEntries);
                foreach(var option in options) {
                    var data = option.Split(new string[] { divider2},
                        StringSplitOptions.RemoveEmptyEntries);

                    if(data.Length < 2)
                        continue;
                    AppSettings[data[0]] = data[1];
                }
            }
        }

        public static void Save( ) {
            try
            {
                File.WriteAllText(_path, string.Join(divider,
                    AppSettings.Select(x => x.Key + divider2 + x.Value)));
            }
            catch(Exception e) {
                throw;
            }
        }


    }
}
