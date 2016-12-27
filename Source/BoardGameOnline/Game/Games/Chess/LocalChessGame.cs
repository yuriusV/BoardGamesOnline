using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Model.Game.Chess;
using ChessDotNet;
using Game.Utils;

namespace Game.Games.Chess
{
    public class LocalChessGame : IGame
    {

        private List<string> pendingMessages = new List<string>();
        private List<GameState> pendingMoves = new List<GameState>();
        private ChessToolValidate _validator;

        public LocalChessGame( ) {
            _validator = new ChessToolValidate(new ChessGame());
        }

        public Action<object> DataReceived { get; set; }

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

        public Action<List<object>> MessagesReceived { get; set; }

        public Action<object> MoveReceived { get; set; }

        public object Settings { get; set; }

        public bool CanMove( object move )
        {
            return _validator.CanMove(move);
        }

        public void Close( )
        {
            
        }

        public List<object> GetValidMoves( object inputPosition )
        {
            return _validator.GetMoves(inputPosition);
        }

        public async Task<object> MakeMove( object moveData )
        {
            return await _validator.MakeMove(moveData);
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

        public bool QueryCancelMove( )
        {
            return _validator.QueryCancelMove();
        }
        

        public Task<object> SendData( object data )
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendMessage( object data )
        {
            throw new NotImplementedException();
        }

        Task<bool> IGame.Resign( )
        {
            Close();
            return Task.Run(( ) => true);
        }
    }
}
