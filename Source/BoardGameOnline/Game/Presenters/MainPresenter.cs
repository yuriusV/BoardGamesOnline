using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Interfaces;
using System.Windows.Controls;

namespace Game.Presenters
{
    public class MainPresenter
    {

        private static MainPresenter _instance;
        public static MainPresenter Get( ) {
            if(_instance == null)
                _instance = new MainPresenter();

            return _instance;
        }


        private IMainView _mainView = null;
        
        public MainPresenter( ) {
            _instance = this;
        }
        
        public void SetupView( IMainView view ) {
            _mainView = view;

            view.RegisterMainMenuItem("Новая игра", ( ) => { }, new Dictionary<string, Action>
            {
                ["ПРотив ПК"] = () => StartGameAgainstPC(),
                ["Сам с собой"] = () => StartSingleGame(),
                ["Удаленно"] = () => StartRemoteGame(),
            });
            view.RegisterMainMenuItem("Текущая", ( ) => { }, new Dictionary<string, Action> {
                //["Сделать ход"]
            });                
        }

        private void StartRemoteGame( )
        {

            
        }

        private void StartSingleGame( )
        {
            // TODO : remote 'chess', set 'game'
            var sets = new ChessSettings();

            sets.IsWhite = true;
            sets.Type = GameType.Single;
            sets.SecondLimited = -1;

            var pres = new ChessBoardPresenter();
            pres.Register(sets);

        }

        private void StartGameAgainstPC( ) {
            
        }



        public bool QueryForSetupView( UserControl control ) {
            _mainView.SetupView(control);
            return true;
        }
    }
}
 