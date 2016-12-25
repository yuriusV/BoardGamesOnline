using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Games
{
    public interface IGame
    {

        object Settings { get; set; }

        Action Ended { get; set; }

        bool HaveChat { get; set; }

        Task<bool> Open( );
        Task<bool> Pause( );


        Task<object> MakeMove( object moveData );
        object ProcessMove( object moveData ); //????
        bool CanMove( object move );

        void SendMessage( object data);
        List<object> CheckMessages( );

        Task<object> SendData( object data );
        List<object> CheckData( object data);

        Task Resign( );
        void Close( );
    }
}
