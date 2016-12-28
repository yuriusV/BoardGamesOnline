using Game.Model.Game.Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model.Remote
{
    public class RemoteData
    {
        public string MessageText { get; set; }
        public UserData Sender { get; set; }
        public GameStateQuery State { get; set; }
        public Tuple<BPosition, BPosition> Move { get; set; }

        public string SystemData { get; set; }
    }

    public class UserData {
        public string Name { get;set;}
        public string Status { get; set; }
        public string ImageBase64 { get; set; }
    }

    public enum GameStateQuery {
        Nothing,
        Message,
        Move,
        Start,
        Pause,
        Resign
    }
}
