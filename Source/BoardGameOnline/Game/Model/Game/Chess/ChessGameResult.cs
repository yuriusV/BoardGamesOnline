using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPoco;

namespace Game.Model.Game.Chess
{
    [PrimaryKey("id")]
    [TableName("chess_results")]
    public class ChessGameResult
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("uid")]
        public string Uid { get; set; }

        [Column("player1uid")]
        public string Player1Uid { get; set; }

        [Column("player2uid")]
        public string Player2Uid { get; set; }

        [Column("winner")]
        public int Winner { get; set; }// 1, 2 or 0

        [Column("aborted")]
        public bool Aborted { get; set; }

        [Column("seconds")]
        public int SecondsPlayed { get; set; }

        [Column("maked_steps")]
        public int MakedSteps { get; set; }

        [Column("win_type")]
        public int WinType { get; set; }
    }

    public enum GameWinner {
        Draw = 0,
        Player1 = 1,
        Player2 = 2
    }

    public enum WinType {
        Mate = 0, // мат
        Resigned = 1 // сдался 
    }
}
