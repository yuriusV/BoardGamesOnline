using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Games.Chess;

namespace Game.Games
{
    public class GameFactory
    {
        public static IGame GetGame( string name ) {
            if(name == "single chess")
                return (IGame)new LocalChessGame();
            else if(name == "bot chess")
                return (IGame)new ChessGameWithPC();

            return null;
        }
    }
}
