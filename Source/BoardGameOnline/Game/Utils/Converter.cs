using System;
using ChessDotNet;
using Game.Model.Game.Chess;

namespace Game.Utils
{
    internal class Converter
    {
        internal static string ConvertChessPosition( Tuple<int, int> point )
        {
            return (char)(point.Item2 + (int)'A') + (8 - point.Item1).ToString();
        }
        public static BPosition ConvertChessPosition( string position ) {
            return new BPosition { Column = ((int)position[0] - (int)'A'), Row = 7 - ((int)position[1] - (int)'1') };
        }

        internal static string ConvertChessPosition( BPosition point )
        {
            return (char)(point.Column + (int)'A') + (8 - point.Row).ToString();
        }
    }
}