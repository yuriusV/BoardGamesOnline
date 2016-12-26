using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Interfaces;
using Game.Settings;
using Game.Views;
using Game.Views.Default;
using System.Reflection;

namespace Game.Presenters
{
    public class ViewPresenterManager
    {

        private static string SettingsUser = "ViewManager";
        private static Type HelpType = typeof(IHelp);
        private static Type ChessBoardType = typeof(IChessBoardView);
        private static Type SettingsType = typeof(ISettingsView);
        
        public static object ResolveView(Type type) {

            if(type == HelpType)
                return GetHelpView();
            else if(type == ChessBoardType)
                return GetChessBoardView();
            else if(type == SettingsType)
                return GetSettingsView();

            return null;
        }
        private static object GetSettingsView( ) {
            var view = SettingsManager.Value(SettingsUser)["SettingsView"];
            if(view == "Default")
                return new Views.Default.Settings();

            return new Views.Default.Settings();
        }

        private static object GetHelpView( ) {
            var view = SettingsManager.Value(SettingsUser)["HelpView"];
            if(view == "Default")
                return new Help();
            
            return new Help();
        }

        private static object GetChessBoardView( ) {
            var view = SettingsManager.Value(SettingsUser)["ChessBoard"];
            if(view == "Default")
                return new ChessBoard();
            else
                return new Views._3D.View3D();
        }
    }
}
