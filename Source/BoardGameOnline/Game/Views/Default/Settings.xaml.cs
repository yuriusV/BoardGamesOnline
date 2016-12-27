using Game.Interfaces;
using Game.Settings;
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

namespace Game.Views.Default
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl, ISettingsView
    {

        public Settings( )
        {
            InitializeComponent();
            
            comboBoxChessBoardType.ItemsSource = Enum.GetValues(typeof(ChessGameBoard)).Cast<ChessGameBoard>();
        }


        #region [Dependency props]


        public Config UIData
        {
            get { return (Config)GetValue(UIDataProperty); }
            set { SetValue(UIDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UIData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UIDataProperty =
            DependencyProperty.Register("UIData", typeof(Config), typeof(Settings), new PropertyMetadata(Config.Get()));


        #endregion  


        #region [ISettingsView implemenation]
        public void Start( ) {
            
            UIData = Data;
            saveButton.Click += ( s, e )
                => Save?.Invoke(UIData);
            cancelButton.Click += ( s, e ) => Cancel?.Invoke();
            this.DataContext = this;    
        }
        
        public Config Data { get; set; }

        public Action Cancel { get; set; }
        
        public Action<Config> Save { get; set; }

        public Action<Config> Updated { get; set; }
        

        #endregion
    }
}
