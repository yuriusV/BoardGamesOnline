using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Interfaces;
using Game.Settings;
using Game.Views;
using Game.Views.Default;

namespace Game.Presenters
{
    public class ViewPresenterManager
    {
        public static object ResolveView(Type type) {
            if(type == typeof(IHelp))
            {
                var view = SettingsManager.Value("HelpView");
                if(view == "Default")
                    return new Help();
                else
                    return new Help();
            }
            else if(type == typeof(IChessBoardView)) {
                var view = SettingsManager.Value("ChessBoard");
                if(view == "Default")
                    return new ChessBoard();
                else
                    return new ChessBoard();
            }

            return null;
        }
    }
}
