using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessDotNet;
using Game.Model.Game.Chess;
using Game.Utils;

namespace Game.Games.Chess
{
    public class ChessToolValidate
    {
        protected ChessGame _gameLib;
        protected Tuple<BPosition, BPosition> _lastMove;

        public ChessToolValidate( ChessGame game ) {
            _gameLib = game;
        }

        public bool CanMove( object move )
        {
            var state = (GameState)move;
            var pos1 = Converter.ConvertChessPosition(state.LastMove.Item1);
            var pos2 = Converter.ConvertChessPosition(state.LastMove.Item2);
            var canMove = _gameLib.GetValidMoves(
                new Position(pos1)).Contains(
                new Move(
                    pos1,
                    pos2,
                    _gameLib.WhoseTurn));

            return canMove;
        }
        
        public List<object> GetMoves( object inputPosition ) {
            var pos = (BPosition)inputPosition;
            return _gameLib.GetValidMoves(new Position(Converter.ConvertChessPosition(pos)))
                .Select(x => (object)Converter.ConvertChessPosition(x.NewPosition.ToString())).ToList();
        }

        public async Task<object> MakeMove( object moveData )
        {
            var state = (GameState)moveData;
            _gameLib.ApplyMove(new Move(
                    Converter.ConvertChessPosition(state.LastMove.Item1),
                    Converter.ConvertChessPosition(state.LastMove.Item2), _gameLib.WhoseTurn),
                alreadyValidated: true);
            _lastMove = state.LastMove;

            return await Task.Run(( ) => {
                return true;
            });
        }
        
        public bool QueryCancelMove( )
        {
            if(_lastMove == null)
                return false;

            return _gameLib.ApplyMove(new Move(
                    Converter.ConvertChessPosition(_lastMove.Item1),
                    Converter.ConvertChessPosition(_lastMove.Item2),
                    _gameLib.WhoseTurn == Player.Black ? Player.White : Player.Black),
                alreadyValidated: true) != MoveType.Invalid;
        }
        
    }
}
