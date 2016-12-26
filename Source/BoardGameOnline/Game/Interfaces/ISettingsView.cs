using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Interfaces
{
    public interface ISettingsView
    {
        Action<Dictionary<string, Dictionary<string, Tuple<string, string>>>> Save { get; set; }
        Action Cancel { get; set; }
        Action<string, string, string> Updated { get; set; }

        void DisplaySettings( Dictionary<string, Dictionary<string, Tuple<string, string>>> data);
    }
}
