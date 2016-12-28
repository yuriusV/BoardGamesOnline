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
        
        private static Type HelpType = typeof(IHelp);
        private static Type ChessBoardType = typeof(IChessBoardView);
        private static Type SettingsType = typeof(ISettingsView);
        private static Type StatisticType = typeof(IStatisticView);
        private static Type RemoteSetupType = typeof(IRemoteSetupView);

        public static object ResolveView(Type type) {

            if(type == HelpType)
                return GetHelpView();
            else if(type == ChessBoardType)
                return GetChessBoardView();
            else if(type == SettingsType)
                return GetSettingsView();
            else if(type == StatisticType)
                return ResolveStatistic();
            else if(type == RemoteSetupType)
                return GetRemoteSetupView();


            return null;
        }
        private static object GetSettingsView( ) {
            return new Views.Default.Settings();
        }

        private static object GetRemoteSetupView( ) {
            return new RemoteSetupView();
        }

        private static object GetHelpView( ) {
            return new Help();
        }

        private static object ResolveStatistic( ) {
            return new StatisticView();
        }

        private static object GetChessBoardView( ) {
            var conf = Config.Get().BoardForChess;
            if(conf == ChessGameBoard.Simple)
                return new ChessBoard();

            return new Views._3D.View3D();
        }
    }
}
