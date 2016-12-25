using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Model.Game.Chess;
using ChessDotNet;
using Game.Utils;
using Game.Model.Game.Chess;

namespace Game.Games.Chess
{
    public class LocalChessGame : IGame
    {

        private List<string> pendingMessages = new List<string>();
        private List<GameState> pendingMoves = new List<GameState>();
        private ChessGame _gameLib = new ChessGame();

        public Action Ended { get; set; }

        public bool HaveChat
        {
            get
            {
                return false;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public object Settings { get; set; }

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

        public List<object> CheckData( object data )
        {
            throw new NotImplementedException();
        }

        public List<object> CheckMessages( )
        {
            throw new NotImplementedException();
        }

        public void Close( )
        {
            
        }

        public async Task<object> MakeMove( object moveData )
        {
            var state = (GameState)moveData;
            _gameLib.ApplyMove(new Move(
                    Converter.ConvertChessPosition(state.LastMove.Item1),
                    Converter.ConvertChessPosition(state.LastMove.Item2), _gameLib.WhoseTurn), 
                alreadyValidated: true);
            return await Task.Run(( ) => {
                return true;
            });
        }

        public Task<bool> Open( )
        {
            return Task.Run(( ) => true);
        }

        public Task<bool> Pause( )
        {
            return Task.Run(( ) => true);
        }

        public object ProcessMove( object moveData )
        {
            throw new NotImplementedException();
        }

        public Task Resign( )
        {
            return Task.Run(() => _gameLib.Resign(Player.None));
        }

        public Task<object> SendData( object data )
        {
            throw new NotImplementedException();
        }

        public void SendMessage( object data )
        {
            throw new NotImplementedException();
        }
    }
}
