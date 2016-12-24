using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Interfaces;
using Game.Model.Game.Chess;
using System.Windows.Controls;
using Game.Games;

namespace Game.Presenters
{
    public class ChessBoardPresenter
    {
        private IChessBoardView _board;
        private ChessSettings _settings;
        private MainPresenter _main;
        private IGame _game;

        public ChessBoardPresenter( ) { }

        public void Register( ChessSettings settings ) {
            _settings = settings;
            _board = ViewPresenterManager.ResolveView(typeof(IChessBoardView)) 
                as IChessBoardView;
            _main = MainPresenter.Get();

            string gameType;
            if(_settings.Type == GameType.Single)
                gameType = "single chess";
            else if(_settings.Type == GameType.AgainstPC)
                gameType = "local chess";
            else
                gameType = "remote chess";

            _game = GameFactory.GetGame(gameType);
            _game.Open();

            _main.QueryForSetupView((UserControl)_board);
            
            _board.SetupState(GameState.Initial, true);
            _board.GameReady = GameStarted;
            _board.StateChanged = GameStateChanged;
            _board.SetText("Начало игры");


            if(_game.HaveChat)
            {
                _main.ShowChat();
            }
            else {
                _main.HideChat();
            }

        }

        private async Task<bool> GameStateChanged( GameState state ) {
            var result = await _game.MakeMove(state);
            return (result as bool?) ?? false;
        }


        private void GameStarted( ) {

        }
    }

    public class ChessSettings {
        public bool IsWhite = true;
        public int SecondLimited = -1;
        public GameType Type = GameType.Single;

    }

    public enum GameType { Single, Remote, AgainstPC }
}
