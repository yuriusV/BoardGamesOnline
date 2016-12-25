using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Model.Game.Chess;

namespace Game.Interfaces
{
    public interface IChessBoardView
    {
        void Start(ChessSettings settings );
        Action IntervalElapsed { get; set; }
        Func<GameState, Task<bool>> StateChanged { get; set; }
        Action GameReady { get; set; }

        void SetupState( GameState state, bool isWhite );
        void SetClock( int seconds );
        void SetText( string text );
        void SetHandleMoveFrom(bool white );
            
        void OnGamePaused( Action paused );
        void OnGameFinished( Action stopped );
    }
}
