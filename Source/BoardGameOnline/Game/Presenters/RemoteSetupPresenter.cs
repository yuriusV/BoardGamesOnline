using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Interfaces;
using Game.Database;
using Game.Remote;
using System.Windows.Controls;

namespace Game.Presenters
{
    public class RemoteSetupPresenter
    {


        private MainPresenter _main;
        private IRemoteSetupView _view;

        public RemoteSetupPresenter( ) {

        }

        public async void Register( ) {
            _main = MainPresenter.Get();
            _view = ViewPresenterManager.ResolveView(typeof(IRemoteSetupView)) as IRemoteSetupView;

            var users = await DbLocal.FetchUsers();
            _view.Contacts = users?.ToList();
            _view.StartGame = ( addr ) => {
                var gamePres = new ChessBoardPresenter();
                var canPlay = RemoteManager.ConnectTo(new Connection {
                    Address = addr.Split(':')[0],
                    Port = addr.Split(':')[1]
                });
                if(canPlay) {
                    _main.ShowInfo("Невозможно подключиться");
                    _main.ReleaseControl();
                    return;
                }
                gamePres.Register(new Model.Game.Chess.ChessSettings {
                    Type = Model.Game.Chess.GameType.Remote,
                    IsWhite = true,// TODO:
                    SecondLimited = -1
                });
                
            };
            _view.Cancel = ( ) => {
                _main.ReleaseControl();
            };

            _view.Start();
            _main.QueryForSetupView(_view as UserControl);
        }
    }
}
