using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Games.Chess
{
    public class ChessGameWithPC : IGame
    {
        public Action Ended { get; set; }

        public bool HaveChat
        {
            get
            {
                return true;
            }
            set { throw new NotImplementedException(); }
        }

        public object Settings
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool CanMove( object move )
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public Task<object> MakeMove( object moveData )
        {
            throw new NotImplementedException();
        }

        public Task<bool> Open( )
        {
            throw new NotImplementedException();
        }

        public Task<bool> Pause( )
        {
            throw new NotImplementedException();
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
