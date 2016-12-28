using Game.Database;
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
using System.Windows.Forms;
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
    public partial class Settings : System.Windows.Controls.UserControl, ISettingsView
    {

        public Settings( )
        {
            InitializeComponent();

            textLevelEnter.TextChanged += ( s, e ) => {
                try {
                    var item = int.Parse(textLevelEnter.Text);
                    if(item < 1) e.Handled = true;
                } catch {
                    e.Handled = true;
                }

            };
                
            comboBoxChessBoardType.ItemsSource = Enum.GetValues(typeof(ChessGameBoard)).Cast<ChessGameBoard>();
            
            buttonBackImageSelect.Click += ( s, e ) => {
                var openD = new OpenFileDialog();
                openD.InitialDirectory 
                    = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                if(openD.ShowDialog() == DialogResult.OK) {
                    var name = ResourceFiles.SaveFile(openD.FileName);
                    UIData.BoardBackImagePath = name;
                }
            };

            buttonColorBlackPiece.Click += ( s, e ) => {
                var colorD = new ColorDialog();
                colorD.Color = System.Drawing.Color.FromArgb(
                    UIData.BlackPieceColor.A, 
                    UIData.BlackPieceColor.R, 
                    UIData.BlackPieceColor.G, 
                    UIData.BlackPieceColor.B
                    );

                if(colorD.ShowDialog() == DialogResult.OK) {
                    UIData.BlackPieceColor = Color.FromArgb(
                        colorD.Color.A, 
                        colorD.Color.R, 
                        colorD.Color.G, 
                        colorD.Color.B);
                    
                }
            };

            buttonColorWhitePiece.Click += ( s, e ) => {
                var colorD = new ColorDialog();
                colorD.Color = System.Drawing.Color.FromArgb(
                    UIData.WhitePieceColor.A,
                    UIData.WhitePieceColor.R,
                    UIData.WhitePieceColor.G,
                    UIData.WhitePieceColor.B);

                if(colorD.ShowDialog() == DialogResult.OK)
                {
                    UIData.WhitePieceColor = Color.FromArgb(
                        colorD.Color.A,
                        colorD.Color.R,
                        colorD.Color.G,
                        colorD.Color.B);
                }
            };

            buttonProfileImageSelect.Click += ( s, e ) => {
                var openD = new OpenFileDialog();
                openD.InitialDirectory
                    = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                if(openD.ShowDialog() == DialogResult.OK)
                {
                    var name = ResourceFiles.SaveFile(openD.FileName);
                    UIData.UserProfile.ImagePath = name;
                }
            };


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

        private bool Validate( ) {
            return UIData.BotLevel > 0;
        }


        #region [ISettingsView implemenation]
        public void Start( ) {
            
            UIData = Data;
            buttonSave.Click += ( s, e ) => {
                if(Validate())
                    Save?.Invoke(UIData);
                
            };
            buttonCancel.Click += ( s, e ) => Cancel?.Invoke();
            this.DataContext = this;    
        }
        
        public Config Data { get; set; }

        public Action Cancel { get; set; }
        
        public Action<Config> Save { get; set; }

        public Action<Config> Updated { get; set; }
        

        #endregion
    }
}
