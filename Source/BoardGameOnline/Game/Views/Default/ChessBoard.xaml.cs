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
using Game.Utils;
using Game.Presenters;
using System.Threading;

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
        private Tuple<int, int> _clickedCell;
        private bool _isWhite = true;
        private ChessSettings _settings;

        private int _timerMinutes = 0;
        private int _timerSeconds = 0;

        public ChessBoard( )
        {
            InitializeComponent();
            buttons = new Button[8, 8];


            // TODO : make for with correct catching 
            foreach(var i in Enumerable.Range(0, 8))
            {
                foreach(var j in Enumerable.Range(0, 8))
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].SetValue(Grid.RowProperty, i);
                    buttons[i, j].SetValue(Grid.ColumnProperty, j);
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
            
            if(_clickedCell == null)
            {
                // Нельзя ходить с пустой
                if(_state.Field[i, j] == Model.Game.Chess.Figure.None)
                    return;

                ToggleIndicator(i, j);
                _clickedCell = new Tuple<int, int>(i, j);
            }
            else
            {
                if(i == _clickedCell.Item1 && j == _clickedCell.Item2)
                {
                    ToggleIndicator(i, j, false);
                    _clickedCell = null;
                } else
                    HandleMove(i, j);
            }
        }

        private void UnsetIndicators( ) {
            for(int i = 0; i < 8; i++) {
                for(int j = 0; j < 8; j++) {
                    buttons[i, j].BorderBrush = Brushes.Gray;
                }
            }
        }

        private void ToggleIndicator( int i, int j, bool active = true, bool success = true) {
            if(active)
                buttons[i, j].BorderBrush = success ? Brushes.Green : Brushes.Red;
            else
                buttons[i, j].BorderBrush = Brushes.Gray;
        }

        private async void HandleMove( int i, int j ) {
            _state.LastMove = new Tuple<Tuple<int, int>, Tuple<int, int>>(
                _clickedCell, 
                new Tuple<int, int>(i, j)
            );

            var result = await StateChanged?.Invoke(_state);
            if(result)
            {    
                UnsetIndicators();
                _state.Field[i, j] = _state.Field[_clickedCell.Item1, _clickedCell.Item2];
                _state.Field[_clickedCell.Item1, _clickedCell.Item2] = Model.Game.Chess.Figure.None;
                UpdateState();
                _clickedCell = null;
            }
            else {
                //MainPresenter.Get().ShowError("Не возможно обработать ход.");
            }
            
            
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

        public Action IntervalElapsed { get; set; }

        public Func<GameState, Task<bool>> StateChanged { get; set; }

        public void SetupState( GameState state, bool isWhite )
        {
            _state = state;
            _isWhite = isWhite;
            UpdateState();
        }

        private Timer _timer;
        public void SetClock( int seconds )
        {
            if(_timer == null)
                _timer = new Timer(TimerCallback, null, 1000, 1000);
            else {
                _timer.Dispose();
                _timer = new Timer(TimerCallback, null, 1000, 1000);
            }
        }

        private void TimerCallback( object state )
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

        public void Start( ChessSettings settings )
        {
            _settings = settings;
        }

        #endregion
    }
}
