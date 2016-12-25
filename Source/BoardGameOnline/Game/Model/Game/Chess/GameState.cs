using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model.Game.Chess
{

    public class BPosition {
        public int Column;
        public int Row;
    }

    public class GameState
    {
        public Figure[,] Field { get; set; }
        public Tuple<Tuple<int, int>, Tuple<int, int>> LastMove { get; set; }
        public List<Tuple<BPosition, BPosition>> Moves { get; set; }

        public static GameState Initial;

        static GameState( ) {
            Initial = new GameState();
            Initial.Field = Parse("BRoo;BKni;BBis;BQue;BKin;BBis;BKni;BRoo\n" +
                "BPaw;BPaw;BPaw;BPaw;BPaw;BPaw;BPaw;BPaw\n" +
                ";;;;;;;\n;;;;;;;\n;;;;;;;\n;;;;;;;\n" +
                "WPaw;WPaw;WPaw;WPaw;WPaw;WPaw;WPaw;WPaw\n" +
                "WRoo;WKni;WBis;WQue;WKin;WBis;WKni;WRoo\n");
            
        }
        private static Figure[,] Parse( string text ) {
            var result = new Figure[8, 8];

            var lines = text.Split(new string[] { "\n" }, StringSplitOptions.None);
            var cells = lines.Select(x => x.Split(new string[] { ";" }, StringSplitOptions.None)).ToArray();

            for(int i = 0; i < 8; i++) {
                for(int j = 0; j < 8; j++) {
                    bool black = cells[i][j].StartsWith("B");
                    int val;
                    if(cells[i][j].EndsWith("Kin")) val = 1;
                    else if(cells[i][j].EndsWith("Que")) val = 2;
                    else if(cells[i][j].EndsWith("Roo")) val = 3;
                    else if(cells[i][j].EndsWith("Bis")) val = 4;
                    else if(cells[i][j].EndsWith("Kni")) val = 5;
                    else if(cells[i][j].EndsWith("Paw")) val = 6;
                    else val = 0;
                    val = black ? -val : val;

                    result[i, j] = (Figure)val;
                }
            }

            return result;
        }

        public GameState( ) {
            Field = new Figure[8, 8];
        }
    
        
        public GameState GetWithMove( BPosition b1, BPosition b2 ) {
            var copy = new Figure[8, 8];
            for(int i = 0; i < 8; i++) {
                for(int j = 0; j < 8; j++) {
                    copy[i, j] = Field[i, j];
                }
            }

            copy[b2.Column, b2.Row] = copy[b1.Column, b1.Row];
            copy[b1.Column, b1.Row] = Figure.None;
            var moves = Moves;moves.Add(new Tuple<BPosition, BPosition>(b1, b2));

            return new GameState() { Field = copy, Moves = moves};
        }
    }

    public enum Figure {
        None = 0,
        WhiteKing = 1,
        WhiteQueen = 2,
        WhiteRook = 3,
        WhiteBishop = 4,
        WhiteKnight = 5,
        WhitePawn = 6,
        BlackKing = -1,
        BlackQueen = -2,
        BlackRook = -3,
        BlackBishop = -4,
        BlackKnight = -5,
        BlackPawn = -6
    }
}
