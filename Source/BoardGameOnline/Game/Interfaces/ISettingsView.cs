using Game.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Interfaces
{
    public interface ISettingsView
    {
        void Start( );
        Config Data { get; set; }
        Action<Config> Save { get; set; }
        Action Cancel { get; set; }
        Action<Config> Updated { get; set; }
    }
}
