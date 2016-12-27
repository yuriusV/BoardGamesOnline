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

        public static Piece[][] ConvertFigureToPieces( Figure[,] figs ) {
            var ps = new Piece[8][];
            for(int i = 0; i < 8; i++) {
                ps[i] = new Piece[8];
                for(int j = 0; j < 8; j++) {
                    var item = figs[i, j];
                    if(item == Figure.BlackBishop)
                        ps[i][j] = new ChessDotNet.Pieces.Bishop(Player.Black);
                    else if(item == Figure.BlackKing)
                        ps[i][j] = new ChessDotNet.Pieces.King(Player.Black);
                    else if(item == Figure.BlackQueen)
                        ps[i][j] = new ChessDotNet.Pieces.Queen(Player.Black);
                    else if(item == Figure.BlackKnight)
                        ps[i][j] = new ChessDotNet.Pieces.Knight(Player.Black);
                    else if(item == Figure.BlackPawn)
                        ps[i][j] = new ChessDotNet.Pieces.Pawn(Player.Black);
                    else if(item == Figure.BlackRook)
                        ps[i][j] = new ChessDotNet.Pieces.Rook(Player.Black);
                    else if(item == Figure.WhiteBishop)
                        ps[i][j] = new ChessDotNet.Pieces.Bishop(Player.White);
                    else if(item == Figure.WhiteKing)
                        ps[i][j] = new ChessDotNet.Pieces.King(Player.White);
                    else if(item == Figure.WhiteQueen)
                        ps[i][j] = new ChessDotNet.Pieces.Queen(Player.White);
                    else if(item == Figure.WhiteKnight)
                        ps[i][j] = new ChessDotNet.Pieces.Knight(Player.White);
                    else if(item == Figure.WhitePawn)
                        ps[i][j] = new ChessDotNet.Pieces.Pawn(Player.White);
                    else if(item == Figure.WhiteRook)
                        ps[i][j] = new ChessDotNet.Pieces.Rook(Player.White);
                    
                }
            }

            return ps;
            

        }
    }
}