using Game.Interfaces;
using Game.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Game.Presenters
{
    public class SettingsPresenter
    {
        private ISettingsView _view;
        private MainPresenter _main;

        public SettingsPresenter( ) {

        }

        public void Register( ) {
            _main = MainPresenter.Get();
            var view = ViewPresenterManager.ResolveView(typeof(ISettingsView));
            _main.QueryForSetupView(view as UserControl);
            _view = view as ISettingsView;
            _view.DisplaySettings(SettingsManager.AppSettings);
            _view.Cancel = ( ) => {
                _main.ReleaseControl();       
            };
            _view.Save = data =>
            {
                SettingsManager.AppSettings = data;
                SettingsManager.Save();
            };
        }
    }
}
