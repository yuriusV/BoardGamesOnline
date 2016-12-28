using Game.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Interfaces
{
    public interface IRemoteSetupView
    {
        Action<string> StartGame { get; set; }
        Action Cancel { get; set; }

        void Start( );

        List<User> Contacts { get; set; }
    }
}
