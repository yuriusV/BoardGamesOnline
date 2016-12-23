using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Interfaces;

namespace Game.Presenters
{
    public class MainPresenter
    {
        private IMainView _mainView = null;

        public MainPresenter( ) {

        }
        
        public void SetupView( IMainView view ) {
            _mainView = view;

            
        }
    }
}
 