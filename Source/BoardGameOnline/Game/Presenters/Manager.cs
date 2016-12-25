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

        private static Type HelpType = typeof(IHelp);
        private static Type ChessBoardType = typeof(IChessBoardView);

        public static object ResolveView(Type type) {
            if(type == HelpType)
            {
                return GetHelpView();
            }
            else if(type == ChessBoardType) {
                return GetChessBoardView();
            }

            return null;
        }

        private static object GetHelpView( ) {
            var view = SettingsManager.Value("HelpView");
            if(view == "Default")
                return new Help();
            else
                return new Help();
        }

        private static object GetChessBoardView( ) {
            var view = SettingsManager.Value("ChessBoard");
            if(view == "Default")
                return new ChessBoard();
            else
                return new ChessBoard();
        }
    }
}
