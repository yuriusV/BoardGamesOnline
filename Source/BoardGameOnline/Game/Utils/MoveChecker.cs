using Game.Model.Game.Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessDotNet;
using ChessDotNet.Pieces;

namespace Game.Utils
{
    public static class ChessMoveChecker
    {
        public static ChessGame Game;

        static ChessMoveChecker( ) {
            Game = new ChessGame();
            
        }

    }

    public enum ChessMoveState {
        Unsucess,
        Sucess,
        Check,
        Mate
    }
}
