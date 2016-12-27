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
        private static XmlSerializer _seri;

        public static Config Load( ) {
            if(_seri == null) {
                _seri = new XmlSerializer(typeof(Config));
            }
            try
            {
                if(File.Exists(_path))
                {
                    using(var sr = new StreamReader(_path))
                    {
                        var data = _seri.Deserialize(sr) as Config;
                        Config.Set(data);
                        return data;
                    }
                }
                else
                {
                    File.WriteAllText(_path, "");
                    Config.Set(new Config());
                    return Config.Get();
                }
            }
            catch(FileNotFoundException e)
            {
                File.WriteAllText(_path, "");
                using(var sw = new StreamWriter(_path))
                {
                    _seri.Serialize(sw, new Config());
                }
                Config.Set(new Config());
                return Config.Get();
            }
            catch(Exception e) {
                Debug.Write(e.Message);
                Config.Set(new Config());
                return Config.Get();
            }
        }

        public static void Save( ) {
            if(_seri == null)
            {
                _seri = new XmlSerializer(typeof(Config));
            }

            try
            {
                using(var sw = new StreamWriter(_path))
                {
                    _seri.Serialize(sw, Config.Get());
                }
            }
            catch(Exception e)
            {
                Debug.Write(e.Message);
            }
        }


    }
}
