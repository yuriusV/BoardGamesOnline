using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Settings
{
    public interface ISettingUser
    {
        Func<Dictionary<string, Tuple<string, string>>> QueryForSettings { get; set; }
        string SettingUserName { get; set; }
        Dictionary<string, Type> Setups { get; set; }        
    }
}
