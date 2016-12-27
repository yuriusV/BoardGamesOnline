using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class Program
    {
        static void Main( string[] args )
        {

            
            var ai = new  Game.Utils.ChessAI();
            var state = Game.Model.Game.Chess.GameState.Initial;
            var res = ai.GetMoveAsync(state, new Game.Utils.ChessAI.Settings {
                IsWhite = true,
                MinLevel = 3
            });
            res.Wait();
            var d = res.Result;
            Console.ReadKey(true);
        }
    }
}
