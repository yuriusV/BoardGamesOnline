using Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Model.View;
using Game.Database;
using System.Windows.Controls;

namespace Game.Presenters
{
    public class StatisticPresenter
    {
        private MainPresenter _main;
        private IStatisticView _view;

        public StatisticPresenter( ) {
        }

        public async void Register( ) {
            _view = ViewPresenterManager.ResolveView(typeof(IStatisticView)) as IStatisticView;
            _main = MainPresenter.Get();

            var games = await DbLocal.FetchChessResults();

            var data = new StatisticViewData();
            data.TotalPlayed = games.Count();
            data.CountDraws = games.Count(x => x.Winner == 0);
            data.CountLoses = games.Count(x => x.Winner == 2);
            data.CountWins = data.TotalPlayed - data.CountLoses - data.CountDraws;
            int maxLen = 0;
            int currentMax = 0;
            foreach(var game in games) {
                if(game.Winner == 1)
                    currentMax++;
                else {
                    if(maxLen < currentMax)
                        maxLen = currentMax;
                }
            }
            data.LongestWins = maxLen;

            _view.Start(data);
            _main.QueryForSetupView(_view as UserControl);
        }
    }
}
