using Game.Database;
using Game.Model.Game.Chess;
using Game.Presenters;
using Game.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game.Views._3D
{
    /// <summary>
    /// Interaction logic for View3D.xaml
    /// </summary>
    public partial class View3D : UserControl, Game.Interfaces.IChessBoardView
    {


        #region [Style properties]
        public double CellStrokeSize { get; set; } = 0.05;
        public Color CellStrokeColor { get; set; } = Colors.LightGreen;


        public Brush BlackCellBrush {
            get
            {
                return _cells[0, 0].Fill;
            }
            set
            {
                for(int i = 0; i < 8; i++)
                {
                    for(int j = 0; j < 8; j++)
                    {
                        if((i + j) % 2 == 0)
                            _cells[i, j].Fill = value;
                    }
                }
            }
        }
        public Brush WhiteCellBrush {
            get
            {
                return _cells[0, 1].Fill;
            }
            set
            {
                for(int i = 0; i < 8; i++) {
                    for(int j = 0; j < 8; j++) {
                        if((i + j) %2 == 1)
                            _cells[i, j].Fill = value;
                    }
                }
            }
        }

        public Brush BorderBoardBrush {
            get
            {
                return Border1.Fill;
            }
            set
            {
                Border1.Fill = Border2.Fill = Border3.Fill = Border4.Fill = value;
            }
        }

        public Brush BackgroundBrush {
            get
            {
                return LayoutRoot.Background;
            }
            set
            {
                LayoutRoot.Background = value;
            }
        }
        #endregion


        List<Tuple<Model.Game.Chess.Figure, ChessPiece>> _piecesReg;

        public delegate void PieceCaptureEventHandler( ChessPiece capturingPiece, ChessPiece capturedPiece );
        public event PieceCaptureEventHandler PieceCaptured;
        private event System.EventHandler PiecePicked;
        private event System.EventHandler PieceReleased;
        private Rectangle[,] _cells;
        private Timer _timer;

        private BPosition _selection = null;
        private GameState _state;
        private ChessSettings _settings;
        private int _currentSeconds = -1;
        private bool _watchAtSide = false;

        private Tuple<Vector3D, Point3D> _cameraSide;
        private Tuple<Vector3D, Point3D> _cameraFront;
        
        public View3D( )
        {
            InitializeComponent();

            Loaded += UserControl_Loaded;


            _cameraFront = new Tuple<Vector3D, Point3D>(Camera.LookDirection, Camera.Position);
            _cameraSide = new Tuple<Vector3D, Point3D>(new Vector3D(20.066, 0, -5.976), new Point3D(-17.066, 0, 20.976));

            //viewMover.Click += ( s, e ) =>
            //{
            //    viewMover.Content = _watchAtSide ? "Вид сбоку" : "Вид сзади";
            //    _watchAtSide = !_watchAtSide;
            //    if(_watchAtSide)
            //    {
            //        Camera.Position = _cameraSide.Item2;
            //        Camera.LookDirection = _cameraSide.Item1;
            //    }
            //    else
            //    {
            //        Camera.Position = _cameraFront.Item2;
            //        Camera.LookDirection = _cameraFront.Item1;
            //    }
            //};

            try
            {
                var path = ResourceFiles.GetFile(Config.Get().BoardBackImagePath);
                LayoutRoot.Background = new ImageBrush(new BitmapImage(new Uri(path)));
            }
            catch { }

            cancelButton.Click += async ( s, e ) => {
                var cur = await Task<bool>.Run(() => {
                    if(_state.Moves.Count < 1)
                        return false;
                    var res = CancelLastMove?.Invoke();
                    return res.HasValue && res.Value;
                });

                if(cur) {
                    var move = _state.Moves[_state.Moves.Count - 1];
                    MovePiece(new Point(move.Item2.Column + 1, 8 - move.Item2.Row), new Point(move.Item1.Column + 1, 8 - move.Item1.Row));
                }
            };
            resignButton.Click += ( s, e ) => {
                MainPresenter.Get().ShowQuestion("Хотите сдаться?", (val) => {
                    if(val)
                    {
                        Stoped?.Invoke();
                        GameLose();
                    }      
                });
            };

            Border1.Fill = Border2.Fill = Border3.Fill = Border4.Fill = SystemColors.ControlTextBrush;
            BoardPositions = new ChessPiece[8, 8];
            
            SetupButtons();

            #region [Setup all fields]
            _piecesReg = new List<Tuple<Model.Game.Chess.Figure, ChessPiece>> {
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.BlackBishop, BBishop1),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.BlackBishop, BBishop2),

                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.BlackKnight, BKnight1),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.BlackKnight, BKnight2),

                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.BlackRook, BRook1),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.BlackRook, BRook2),

                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.BlackKing, BKing),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.BlackQueen, BQueen),

                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.WhiteBishop, WBishop1),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.WhiteBishop, WBishop2),

                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.WhiteKnight, WKnight1),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.WhiteKnight, WKnight2),

                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.WhiteRook, WRook1),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.WhiteRook, WRook2),

                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.WhiteKing, WKing),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.WhiteQueen, WQueen),

                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.BlackPawn, BPawn1),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.BlackPawn, BPawn2),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.BlackPawn, BPawn3),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.BlackPawn, BPawn4),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.BlackPawn, BPawn5),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.BlackPawn, BPawn6),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.BlackPawn, BPawn7),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.BlackPawn, BPawn8),


                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.WhitePawn, WPawn1),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.WhitePawn, WPawn2),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.WhitePawn, WPawn3),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.WhitePawn, WPawn4),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.WhitePawn, WPawn5),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.WhitePawn, WPawn6),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.WhitePawn, WPawn7),
                new Tuple<Model.Game.Chess.Figure, ChessPiece> (Model.Game.Chess.Figure.WhitePawn, WPawn8),
            };

            #endregion
        }

        
        /// <summary>
        /// Set the new coordinates for the piece on the matrix
        /// </summary>
        void OnPieceCoordChange( ChessPiece piece, Point prevCoord, Point newCoord )
        {
            ChessPiece targetPiece = GetPiece(newCoord);
            if(targetPiece != null)
            {
                //piece at destination gets captured, report this event
                PieceCaptured?.Invoke(piece, targetPiece);
                targetPiece.AnimateCaptureMove();
            }
            //When a piece is located initially, prevCoord is 0,0 in this case, there's no relocation on the matrix
            if(prevCoord != new Point(0, 0))
                BoardPositions[Convert.ToInt32(prevCoord.X) - 1, Convert.ToInt32(prevCoord.Y) - 1] = null;
            //set the new piece location on the matrix
            BoardPositions[Convert.ToInt32(newCoord.X) - 1, Convert.ToInt32(newCoord.Y) - 1] = piece;
        }
        
        protected ChessPiece[,] BoardPositions { get; set; }


        /// <summary>
        /// locates a chess piece in the specified position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected ChessPiece GetPiece( Point position )
        {
            ChessPiece piece = (ChessPiece)BoardPositions[Convert.ToInt32(position.X) - 1, Convert.ToInt32(position.Y) - 1];
            return piece;
        }

        /// <summary>
        /// Sets up the board to the startup position
        /// </summary>
        public void SetUpBoard( GameState state )
        {
            
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    var currentPoint = new Point(j + 1, i + 1);
                    var val = state.Field[7 - i, j];
                    if(val == Game.Model.Game.Chess.Figure.BlackBishop)
                    {
                        if(BBishop1.Coordinates.X == 0)
                        {
                            BBishop1.Coordinates = currentPoint;
                        }
                        else
                            BBishop2.Coordinates = currentPoint;
                    }
                    else if(val == Game.Model.Game.Chess.Figure.WhiteBishop)
                    {
                        if(WBishop1.Coordinates.X == 0)
                        {
                            WBishop1.Coordinates = currentPoint;
                        }
                        else
                            WBishop2.Coordinates = currentPoint;
                    }
                    else if(val == Game.Model.Game.Chess.Figure.BlackKing)
                    {
                        BKing.Coordinates = currentPoint;
                    }
                    else if(val == Game.Model.Game.Chess.Figure.WhiteKing)
                    {
                        WKing.Coordinates = currentPoint;
                    }
                    else if(val == Game.Model.Game.Chess.Figure.BlackQueen)
                    {
                        BQueen.Coordinates = currentPoint;
                    }
                    else if(val == Game.Model.Game.Chess.Figure.WhiteQueen)
                    {
                        WQueen.Coordinates = currentPoint;
                    }
                    else if(val == Game.Model.Game.Chess.Figure.BlackKnight)
                    {
                        if(BKnight1.Coordinates.X == 0)
                        {
                            BKnight1.Coordinates = currentPoint;
                        }
                        else
                            BKnight2.Coordinates = currentPoint;
                    }
                    else if(val == Game.Model.Game.Chess.Figure.WhiteKnight)
                    {
                        if(WKnight1.Coordinates.X == 0)
                        {
                            WKnight1.Coordinates = currentPoint;
                        }
                        else
                            WKnight2.Coordinates = currentPoint;
                    }
                    else if(val == Game.Model.Game.Chess.Figure.BlackRook)
                    {
                        if(BRook1.Coordinates.X == 0)
                        {
                            BRook1.Coordinates = currentPoint;
                        }
                        else
                            BRook2.Coordinates = currentPoint;
                    }
                    else if(val == Game.Model.Game.Chess.Figure.WhiteRook)
                    {
                        if(WRook1.Coordinates.X == 0)
                        {
                            WRook1.Coordinates = currentPoint;
                        }
                        else
                            WRook2.Coordinates = currentPoint;
                    }
                    else if(val == Game.Model.Game.Chess.Figure.WhitePawn)
                    {
                        if(WPawn1.Coordinates.X == 0)
                            WPawn1.Coordinates = currentPoint;
                        else if(WPawn2.Coordinates.X == 0)
                            WPawn2.Coordinates = currentPoint;
                        else if(WPawn3.Coordinates.X == 0)
                            WPawn3.Coordinates = currentPoint;
                        else if(WPawn4.Coordinates.X == 0)
                            WPawn4.Coordinates = currentPoint;
                        else if(WPawn5.Coordinates.X == 0)
                            WPawn5.Coordinates = currentPoint;
                        else if(WPawn6.Coordinates.X == 0)
                            WPawn6.Coordinates = currentPoint;
                        else if(WPawn7.Coordinates.X == 0)
                            WPawn7.Coordinates = currentPoint;
                        else if(WPawn8.Coordinates.X == 0)
                            WPawn8.Coordinates = currentPoint;
                    }
                    else if(val == Game.Model.Game.Chess.Figure.BlackPawn)
                    {
                        if(BPawn1.Coordinates.X == 0)
                            BPawn1.Coordinates = currentPoint;
                        else if(BPawn2.Coordinates.X == 0)
                            BPawn2.Coordinates = currentPoint;
                        else if(BPawn3.Coordinates.X == 0)
                            BPawn3.Coordinates = currentPoint;
                        else if(BPawn4.Coordinates.X == 0)
                            BPawn4.Coordinates = currentPoint;
                        else if(BPawn5.Coordinates.X == 0)
                            BPawn5.Coordinates = currentPoint;
                        else if(BPawn6.Coordinates.X == 0)
                            BPawn6.Coordinates = currentPoint;
                        else if(BPawn7.Coordinates.X == 0)
                            BPawn7.Coordinates = currentPoint;
                        else if(BPawn8.Coordinates.X == 0)
                            BPawn8.Coordinates = currentPoint;
                    }

                }
            }
            


        }

        public void MovePiece( Point origin, Point destination )
        {
            try
            {
                ChessPiece piece = GetPiece(origin);
                piece.Coordinates = destination;
            }
            catch(Exception ex)
            {
            }
        }
        public void MovePiece( string origin, string destination )
        {
            Point pOrigin = this.ParseCoordinate(origin);
            Point pDestination = this.ParseCoordinate(destination);
            this.MovePiece(pOrigin, pDestination);
        }

        private Point ParseCoordinate( string coord )
        {
            return new Point( (int)coord[0] - (int)'a' + 1, (int)coord[1] - (int)'1' + 1 );
        }

        private void UserControl_Loaded( object sender, RoutedEventArgs e )
        {
            _3DChess.MouseDown += (s, ev1) => _3DChess_MouseDown(s, ev1);
            _3DChess.MouseUp += (s, ev2) => _3DChess_MouseUp(s, ev2);
            PiecePicked +=  (s, ev3) => SetSelectionTo(s as ChessPiece);
            PieceReleased += (s, ev4) => ClearPieceSelection();
        }



        void ClearPieceSelection()
        {
            foreach(ChessPiece piece in BoardPositions)
            {
                if(piece != null)
                {
                    piece.IsGrayedOut = false;
                }
            }
        }

        void SetSelectionTo( ChessPiece sender)
        {
            //when a piece is picked up, gray out the rest of the pieces in the board
            ChessPiece currentPiece = sender as ChessPiece;
            System.Diagnostics.Debug.Assert(currentPiece != null);
            //now get the rest of the pieces other than cuurentPiece and grey them out
            foreach(ChessPiece piece in BoardPositions)
            {
                if(piece != null && piece != currentPiece)
                {
                    piece.IsGrayedOut = true;
                }
            }
        }

        

        ChessPiece GetHitTestResult( Point location )
        {
            
            HitTestResult result = VisualTreeHelper.HitTest(_3DChess, location);
            if(result != null && result.VisualHit is ChessPiece)
            {
                ChessPiece visual = (ChessPiece)result.VisualHit;
                return visual;
            }
            return null;
        }

        void _3DChess_MouseUp( object sender, MouseButtonEventArgs e )
        {
            return;
            Point location = e.GetPosition(_3DChess);
            ChessPiece cp = GetHitTestResult(location);
            if(cp != null)
            {
                //found a chesspiece, send the release event
                PieceReleased?.Invoke(cp, new System.EventArgs());
            }
        } 

        async void _3DChess_MouseDown( object sender, MouseButtonEventArgs e )
        {
            
            Point location = e.GetPosition(_3DChess);
            ChessPiece cp = GetHitTestResult(location);


            if(cp != null)
            {
                if(_selection != null &&
                      cp == GetPieceAt(_selection.Row, _selection.Column))
                {
                    _selection = null;
                    ClearPieceSelection();
                    return;
                }
                else if(_selection != null)
                {
                    await PerformMove(_selection, new BPosition(8 - (int)cp.Coordinates.Y, (int)cp.Coordinates.X - 1));
                }
                else if(_selection == null) {
                    _state.LastMove = new Tuple<BPosition, BPosition>(
                    new BPosition(8 - (int)cp.Coordinates.Y, (int)cp.Coordinates.X),
                    new BPosition(0, 0));

                    
                    _selection = new BPosition( 8 - (int)cp.Coordinates.Y, (int)cp.Coordinates.X - 1 );

                    var poses = GetMovesFrom(_selection);
                    HightLightCells(poses);

                    PiecePicked?.Invoke(cp, new System.EventArgs());
                }
               
            }   
        }

        private async Task<bool> PerformMove( BPosition p1, BPosition p2 ) {
            if(p1 == null || p2 == null)
                return await Task.Run(( ) => false);

            var old = _state;
            _state = _state.GetWithMove(p1, p2);
            var res = await StateChanged?.Invoke(_state);
            if(res)
            {
                var figure = GetPieceAt(p1.Row, p1.Column);
                MovePiece(figure.Coordinates, new Point(p2.Column + 1, 8 - p2.Row));
            }
            else {
                _state = old;
            }


            ClearPieceSelection();
            UnHighlightAllCells();
            _selection = null;
            return res;
        }

        private void HightLightCells( IEnumerable<BPosition> position ) {
            foreach(var pos in position)
            {
                _cells[pos.Row, pos.Column].StrokeThickness = CellStrokeSize;
                _cells[pos.Row, pos.Column].Stroke = new SolidColorBrush(CellStrokeColor);
            }
        }

        private void UnHighlightAllCells( ) {
            for(int i = 0; i < 8; i++) {
                for(int j = 0; j < 8; j++) {
                    _cells[i, j].StrokeThickness = 0;
                }
            }
        }


        private async void PerformClick( int i, int j ) {
            if(_selection == null && HaveFigureOn(i, j)) {
                _selection = new BPosition(i, j);
                var poses = GetMovesFrom(_selection);
                HightLightCells(poses);
            } else
            {
                await PerformMove(_selection, new BPosition(i, j));
                UnHighlightAllCells();
            }
        }

        private bool HaveFigureOn( int i, int j ) {
            return _piecesReg.Select(x => x.Item2).SingleOrDefault(x => x.Coordinates.Y == 8 - i
            && x.Coordinates.X == j + i)  == null? false : true;
        }

        private ChessPiece GetPieceAt( int i, int j ) {
            return _piecesReg.Select(x => x.Item2)
                .FirstOrDefault(x => x.Coordinates.Y == 8 - i
                    && x.Coordinates.X == j + 1);
        }
        


        private void SetupButtons( ) {
            
            var buttons = new Rectangle[8, 8];
            foreach(var i in Enumerable.Range(0, 8)) {
                foreach(var j in Enumerable.Range(0, 8)){
                    buttons[i, j] = new Rectangle();
                    buttons[i, j].SetValue(Grid.ColumnProperty, j + 1);
                    buttons[i, j].SetValue(Grid.RowProperty, i + 1);
                    buttons[i, j].MouseDown += ( s, e ) => PerformClick(i, j);
                    buttons[i, j].Height = 1;
                    buttons[i, j].Width = 1;
                    

                    if((i + j) % 2 == 0) buttons[i, j].Fill = new SolidColorBrush(SystemColors.WindowColor);
                    else buttons[i, j].Fill = new SolidColorBrush(SystemColors.GrayTextColor);

                    boardGrid.Children.Add(buttons[i, j]);
                }
            }

            _cells = buttons;
        }

      
        #region
        public Action IntervalElapsed { get; set; }

        public Func<GameState, System.Threading.Tasks.Task<bool>> StateChanged { get; set; }

        public Action GameReady { get; set; }

        public Func<BPosition, List<BPosition>> GetMovesFrom { get; set; }

        public Func<bool> CancelLastMove { get; set; }
        public Action Paused { get; set; }
        public Action Stoped { get; set; }
        public Action Losed { get; set; }


        public void Start( ChessSettings settings )
        {
            _settings = settings;

            //if(_settings.SecondLimited < 1)
            //{
            //    timer.Visibility = Visibility.Hidden;
            //}

            GameReady?.Invoke();
        }

        public void SetupState( GameState state, bool isWhite )
        {
            _state = state;
            SetUpBoard(state);
        }

        public void SetClock( int seconds )
        {
            _settings.SecondLimited = seconds;
            _currentSeconds = seconds;
            _timer?.Dispose();
            if(seconds == -1)
                return;
            _timer = new Timer(1000);
            _timer.Elapsed += ( s, e ) => UpdateTime();
            _timer.Start();
        }

        private void UpdateTime( )
        {
            _currentSeconds -= 1;
            if(_currentSeconds <= 1) {
                SignalTime();
            }
        }

        private void GameLose( ) {
            Losed?.Invoke();
            SetText("Игра окончена, вы проиграли.");
            MainPresenter.Get().ReleaseControl();
        }

        private void SignalTime( )
        {
            //SetText("Вы потратили слишком много времени.");
            //timer.Background = Brushes.Red;
            //GameLose();    
        }

        public void SetText( string text )
        {
            
        }

        public void SetHandleMoveFrom( bool white )
        {
            
        }

        public void SetChange( Tuple<BPosition, BPosition> move )
        {
            this.Dispatcher.Invoke(( ) => {
                MovePiece(new Point(move.Item1.Column + 1, 8 - move.Item1.Row),
                    new Point(move.Item2.Column + 1, 8 - move.Item2.Row));
                _state = _state.GetWithMove(move.Item1, move.Item2);
            });
        }

        #endregion
    }
}
