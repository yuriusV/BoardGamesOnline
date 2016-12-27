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

        public async Task<Tuple<BPosition, BPosition>> GetMoveAsync( GameState state, Settings sets )
        {
            _current = sets;
            _lastState = new List<GameState>();
            return await Task.Run(( ) =>
            {
                _started = DateTime.Now;
                GetMove(state, 0);
                int maxCost = -100;
                GameState max = new GameState();
                foreach(var st in _lastState) {
                    var res = CostPosition(st);
                    if(res > maxCost)
                    {
                        maxCost = res;
                        max = st;
                    }
                }
                return max.Moves[max.Moves.Count - _current.MinLevel];
            });

        }

        public bool GetMove(GameState state,  int level ) {

            if(level == _current.MinLevel) {
                _lastState.Add(state);
                return true;
            }

            var currentLib = new ChessGame(new GameCreationData { Board = Converter.ConvertFigureToPieces(state.Field), WhoseTurn = _current.IsWhite ? Player.White : Player.Black});

            var owner = currentLib.GetValidMoves( ( _current.IsWhite ? Player.White: Player.Black))
                                                .Select(x => state.GetWithMove(
                                                    Converter.ConvertChessPosition(x.OriginalPosition.ToString()), 
                                                    Converter.ConvertChessPosition(x.NewPosition.ToString()))).ToList();

            var next = new List<GameState>();
            bool needEx = false;
            foreach(var variant in owner) {
                if(GetMove(variant, level + 1))
                    needEx = true;
            }
            return needEx;
        }



        public int CostPosition( GameState state ) {
            var result = 0;
            for(int i = 0; i < 8; i++) {
                for(int j = 0; j < 8; j++) {
                    if(state.Field[i, j] > 0 == _current.IsWhite) {
                        result += _costs[state.Field[i, j]];
                    } else if(state.Field[i, j] != 0)
                    {
                        result -= _costs[state.Field[i, j]];
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
