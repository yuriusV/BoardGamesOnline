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

        Action<object> MoveReceived { get; set; }
        Action<List<object>> MessagesReceived { get; set; }
        Action<object> DataReceived { get; set; }

        Action Ended { get; set; }

        bool HaveChat { get; set; }
        bool CanMove( object move );
        List<object> GetValidMoves( object inputPosition );



        Task<bool> Open( );
        Task<bool> Pause( );
        
        Task<object> MakeMove( object moveData );
        bool QueryCancelMove( );
        object ProcessMove( object moveData );

        
        Task<bool> SendMessage( object data);
        Task<object> SendData( object data );
        
        Task<bool> Resign( );
        void Close( );
    }
}
