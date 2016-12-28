using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Game.Remote;
using Game.Settings;

namespace Game
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        public static void Main( string[] args ) {
            
            Task.Run(( ) =>
            {
                if(Config.Get().AllowRemote)
                    RemoteManager.StartServer();
            });
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
