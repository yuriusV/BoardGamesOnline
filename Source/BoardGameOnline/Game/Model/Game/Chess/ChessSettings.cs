using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model.Game.Chess
{
    public class ChessSettings
    {
        public bool IsWhite = true;
        public int SecondLimited = -1;
        public GameType Type = GameType.Single;
        public int SecondsTimer { get; set; } = -1;
    }


    public enum GameType { Single, Remote, AgainstPC }
}
