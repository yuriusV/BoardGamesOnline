using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Model.Game.Chess;
using ChessDotNet;
using ChessDotNet.Pieces;

namespace Game.Utils
{
    public class ChessAI
    {
        public GameState State { get; set; }
        public float Coefficient { get; set; }
        private Dictionary<Figure, int> _costs;
        private Settings _current;
        private ChessGame _lib;
        private DateTime _started;
        private TreeNode _root;
        private List<GameState> _lastState;

        public ChessAI( ) {
            _costs = new Dictionary<Figure, int> {
                [Figure.BlackBishop] = 3, [Figure.WhiteBishop] = 3,
                [Figure.BlackKing] = 4, [Figure.WhiteKing] = 4,
                [Figure.BlackKnight] = 3, [Figure.WhiteKnight] = 3,
                [Figure.BlackPawn] = 1, [Figure.WhitePawn] = 1,
                [Figure.WhiteQueen] = 8, [Figure.BlackQueen] = 8,
                [Figure.BlackRook] = 4, [Figure.WhiteRook] = 4,
                [Figure.None] = 0
            };
            _lib = new ChessGame();
        }

        //public async Task<Tuple<BPosition, BPosition>> GetMoveAsync( GameState state, Settings sets) {
        //    _current = sets;

        //    return await Task.Run(( ) => {
        //        _started = DateTime.Now;
        //        GetMove(state, 0);
        //        return null;
        //    });

        //}

        public void GetMove(GameState state,  int level ) {



            var currentLib = new ChessGame(state.Moves.Select(x => new Move(
               Converter.ConvertChessPosition(x.Item1),
                Converter.ConvertChessPosition(x.Item2), _current.IsWhite ? Player.White : Player.Black )), movesAreValidated: true);

            var owner = currentLib.GetValidMoves( ( _current.IsWhite ? Player.White: Player.Black))
                                                .Select(x => State.GetWithMove(
                                                    Converter.ConvertChessPosition(x.OriginalPosition.ToString()), 
                                                    Converter.ConvertChessPosition(x.NewPosition.ToString()))).ToList();

            var next = new List<GameState>();
            foreach(var variant in owner) {
            }
        }



        public int CostPosition( ) {
            var result = 0;
            for(int i = 0; i < 8; i++) {
                for(int j = 0; j < 8; j++) {
                    if(State.Field[i, j] > 0 == _current.IsWhite) {
                        result += _costs[State.Field[i, j]];
                    }
                }
            }

            return result;
        }

        public class TreeNode {
            public Tuple<GameState, ChessGame> State { get; set; }
            public int Level { get; set; }

            public List<TreeNode> Variants;
        }
        
        
        public class Settings {
            public bool IsWhite = true;
            public int MinLevel = 3;
            public int Miliseconds = 1000;
        }    
    }
}
