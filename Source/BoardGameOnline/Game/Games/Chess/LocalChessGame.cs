using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Model.Game.Chess;

namespace Game.Games.Chess
{
    public class LocalChessGame : IGame
    {

        private List<string> pendingMessages = new List<string>();
        private List<GameState> pendingMoves = new List<GameState>();


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
            return await Task.Run(( ) => {
                return moveData;
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
            throw new NotImplementedException();
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
