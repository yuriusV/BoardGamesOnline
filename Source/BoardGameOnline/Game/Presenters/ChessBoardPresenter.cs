using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Interfaces;
using Game.Model.Game.Chess;
using System.Windows.Controls;
using Game.Games;
using ChessDotNet;
using ChessDotNet.Pieces;



namespace Game.Presenters
{
    public class ChessBoardPresenter
    {

        public Action GameEnded { get; set; }

        private IChessBoardView _board;
        private ChessSettings _settings;
        private MainPresenter _main;
        private IGame _game;
        private bool _needMoveWhite = true;
        private ChessGameResult _result;
        private DateTime _start;
        private int _countMoves = 0;
       

        public ChessBoardPresenter( ) {
            _result = new ChessGameResult();
        }

        public void Register( ChessSettings settings ) {
            _settings = settings;
            _board = ViewPresenterManager.ResolveView(typeof(IChessBoardView)) 
                as IChessBoardView;
            _main = MainPresenter.Get();

            _board.Start(settings);
            string gameType;   
            if(_settings.Type == GameType.Single)
                gameType = "single chess";
            else if(_settings.Type == GameType.AgainstPC)
                gameType = "local chess";
            else
                gameType = "remote chess";

            _game = GameFactory.GetGame(gameType);
            _game.Settings = settings;
            _game.Open();
             

            _main.QueryForSetupView((UserControl)_board);

            _board.SetupState(GameState.Initial, true);
            _board.GameReady = GameStarted;
            _board.StateChanged = GameStateChanged;
            _board.SetText("Начало игры");
            _board.GetMovesFrom = ( item ) => _game.GetMoves(item).Select(x=> (BPosition)x).ToList();

            if(_game.HaveChat)
            {
                _main.ShowChat();
            }
            else {
                _main.HideChat();
            }

        }

        private async Task<bool> GameStateChanged( GameState state ) {
            if(_settings.IsWhite != _needMoveWhite && _settings.Type != GameType.Single ) {
                return await Task.Run(( ) => false);
            }


            if(!_game.CanMove(state))
                return await Task.Run(( ) => false);

            var result = await _game.MakeMove(state);
            var val =  (result as bool?) ?? false;
            if(val) {
                _needMoveWhite = !_needMoveWhite;
                _countMoves++;
            }
            return val;
        }


        private void GameStarted( ) {
            _start = DateTime.Now;
        }
    }

    

}
