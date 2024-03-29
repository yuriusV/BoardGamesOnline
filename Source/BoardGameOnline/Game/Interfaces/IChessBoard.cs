﻿using System;
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
        Func<BPosition, List<BPosition>> GetMovesFrom { get; set; }
        Action Paused { get; set; }
        Action Stoped { get; set; }
        Action Losed { get; set; }
        Func<bool> CancelLastMove { get; set; }

        void SetChange( Tuple<BPosition, BPosition> move );
        void SetupState( GameState state, bool isWhite );
        void SetClock( int seconds );
        void SetText( string text );
        void SetHandleMoveFrom(bool white );
            
    }
}
