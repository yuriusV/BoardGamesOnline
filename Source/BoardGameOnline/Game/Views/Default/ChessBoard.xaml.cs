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
using Game.Model.Game.Chess;
using Game.Views.Default;
using Game.Interfaces;

namespace Game.Views.Default
{
    /// <summary>
    /// Interaction logic for ChessBoard.xaml
    /// </summary>
    public partial class ChessBoard : UserControl, IChessBoardView
    {
        private Action<GameState> StateChaned { get; set; }


        private GameState _state;
        private Button[,] buttons;
        private Tuple<int, int> clickedButton;
        private bool _isWhite = true;

        public ChessBoard( )
        {
            InitializeComponent();
            buttons = new Button[8, 8];

            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].SetValue(Grid.ColumnProperty, i);
                    buttons[i, j].SetValue(Grid.RowProperty, j);
                    buttons[i, j].HorizontalAlignment = HorizontalAlignment.Stretch;
                    buttons[i, j].VerticalAlignment = VerticalAlignment.Stretch;

                    buttons[i, j].Click += ( s, e ) => ClickButton(i, j);

                    boardGrid.Children.Add(buttons[i, j]);
                }
            }


            this.Loaded += ( s, e ) => {
                GameReady?.Invoke();
            };
        }

        private void ClickButton( int i, int j ) {
            if(clickedButton == null)
            {
                ToggleIndicator(i, j);
                clickedButton = new Tuple<int, int>(i, j);
            }
            else
            {
                HandleMove(i, j);
                clickedButton = null;
                //UnsetIndicators();
            }
        }

        private void ToggleIndicator( int i, int j, bool active = true, bool success = true) {
            if(active)
                buttons[i, j].BorderBrush = success ? Brushes.Green : Brushes.Red;
            else
                buttons[i, j].BorderBrush = Brushes.Gray;
        }

        private void HandleMove( int i, int j) {


            StateChaned(_state);
        }

        private void UpdateState( ) {
            for(int i = 0; i < 8; i++) {
                for(int j = 0; j < 8; j++) {
                    buttons[i, j].Content = _state.Field[i, j].ToString();
                }
            }
        }


        #region [IChessBoardView implementation]
        public Action GameReady { get; set; }

        public Action IntervalElapsed
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Action<GameState> StateChagned { get; set; }

        public void SetupState( GameState state, bool isWhite )
        {
            _state = state;
            _isWhite = isWhite;
            UpdateState();
        }

        public void SetClock( int seconds )
        {
            
        }

        public void SetText( string text )
        {
            textBoard.Text = text;
        }

        public void SetHandleMoveFrom( bool white )
        {
            _isWhite = white;
        }

        public void OnGamePaused( Action paused )
        {
            throw new NotImplementedException();
        }

        public void OnGameFinished( Action stopped )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
