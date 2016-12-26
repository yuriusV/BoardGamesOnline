using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Game.Interfaces;
using Game.Presenters;
using Game.Model.View;

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView
    {
        public MainWindow( )
        {
            
            InitializeComponent();
            var presenter = new MainPresenter();
            presenter.SetupView(this);

            Loaded += (s,e) => WindowLoaded?.Invoke();
            Closing += ( s, e ) => ClosingWindow?.Invoke();
            StateChanged += ( s, e ) => {
                if(this.WindowState == WindowState.Minimized)
                    Minimized?.Invoke();
                else if(this.WindowState == WindowState.Normal)
                    Unminimized?.Invoke();
            };
            

        }

        public Action Minimized { get; set; }
        public Action ClosingWindow { get; set; }
        public Action WindowLoaded { get; set; }
        public Action Unminimized { get; set; }

        public Func<object, bool> OnChatMessage { get; set; }

        public void RegisterMainMenuItem( string title, Action clicked, Dictionary<string, Action> subitems = null )
        {
            var item = new MenuItem();
            item.Header = title;
            item.Click += ( s, e ) => clicked?.Invoke();

            if(subitems != null) {
                foreach(var sub in subitems) {
                    var subControl = new MenuItem
                    {
                        Header = sub.Key
                    };
                    subControl.Click += ( s, e ) => sub.Value?.Invoke();
                    item.Items.Add(subControl);
                }
            }

            mainMenu.Items.Add(item);
        }

        public void SetDefaultView( UserControl control )
        {
            Field.Children.Clear();
            Field.Children.Add(control);
        }

        public void SetupView( UserControl control )
        {
            Field.Children.Clear();
            Field.Children.Add(control);
        }

        public void ShowError( string error )
        {
            MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowMessage( string message )
        {
            MessageBox.Show(message, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowChat( )
        {
            chatColumn.Width = new GridLength(1, GridUnitType.Star);
        }

        public void HideChat( )
        {
            chatColumn.Width = new GridLength(0);
        }

        public void ClearChat( )
        {
            
        }

        public void AddMessageToChat( object message )
        {
            
        }

        public void SetChatTitle( object title )
        {
            chatTitleText.Text = ((ChatTitle)title).Text;
        }

        public void RegisterMainMenuItem( string v, Action p )
        {
            RegisterMainMenuItem(v, p, null);
        }
    }
}
