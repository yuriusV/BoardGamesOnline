using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Utils;
using Game.Model.Game.Chess;
using Game.Settings;
using System.Threading;

namespace Game.Games.Chess
{
    public class ChessGameWithPC : IGame
    {
        public Action Ended { get; set; }
        private ChessToolValidate _validator;
        private ChessAI _ai;
        private int level;

        public ChessGameWithPC( ) {
            _validator = new ChessToolValidate(new ChessDotNet.ChessGame());
            _ai = new ChessAI();
            level = Config.Get().BotLevel;
        }

        public bool HaveChat
        {
            get
            {
                return false;
            }
            set { throw new NotImplementedException(); }
        }

        public object Settings { get; set; }

        public Action<object> MoveReceived { get; set; }
        public Action<List<object>> MessagesReceived { get; set;}
        public Action<object> DataReceived { get; set; }

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
            var move = await _validator.MakeMove(moveData);
            var userMoved = (GameState)moveData;
            var response = await _ai.GetMoveAsync(userMoved, new ChessAI.Settings
            {
                IsWhite = !(Settings as ChessSettings).IsWhite,
                Miliseconds = 10,
                MinLevel = level
            });
            await _validator.MakeMove(userMoved.GetWithMove(response.Item1, response.Item2));
            await Task.Run(( ) => {
                MoveReceived?.Invoke(response);
            }); 
            
            return move;
        }

        public async Task<bool> Open( )
        {
            return await Task.Run(() => true);
        }

        public async Task<bool> Pause( )
        {
            return await Task.Run(( ) => true);
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

        public async Task<bool> Resign( )
        {
            return await Task.Run(( ) => true);
        }
    }
}
