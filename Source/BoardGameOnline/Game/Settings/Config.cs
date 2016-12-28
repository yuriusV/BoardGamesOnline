using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace Game.Settings
{
    public class Config
    {
        private static Config _instance;
        public static Config Get( ) {
            if(_instance == null)
            {
                _instance = SettingsManager.Load();
                if(_instance == null)
                    _instance = new Config();
            }

            return _instance;
        }

        public static void Set( Config newInst ) => _instance = newInst;

        public ChessGameBoard BoardForChess { get; set; }
        public Color BlackPieceColor { get; set; }
        public Color WhitePieceColor { get; set; }
        public bool AutoRotateField { get; set; }
        public string BoardBackImagePath { get; set; }
        public User UserProfile { get; set; }
        public int BotLevel { get; set; }
        public bool AllowRemote { get; set; } 
        public int ListenPort { get; set; }

        public Config( ) {
            BoardBackImagePath = string.Empty;
            BoardForChess = ChessGameBoard.Volume;
            BlackPieceColor = Colors.Black;
            WhitePieceColor = Colors.White;
            AutoRotateField = true;
            BoardBackImagePath = string.Empty;
            UserProfile = new User();
            BotLevel = 1;   
        }
    }

    public class User {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Status { get; set; }

        public User( ) {
            Name = string.Empty;
            ImagePath = string.Empty;
            Status = string.Empty;
        }
    }

    public enum ChessGameBoard {
        [Description("Обычынй")]
        Simple,
        [Description("3D")]
        Volume
    }
}
