using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Remote;
using Game.Model.Remote;
using Game.Model.Game.Chess;

namespace Game.Games.Chess
{
    public class DedicatedChessGame : IGame
    {
        public Action<object> DataReceived { get; set; }
        public Action Ended { get; set; }

        private ChessToolValidate _validator;

        private Connection _connection;

        public bool HaveChat
        {
            get
            {
                return true;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Action<List<object>> MessagesReceived { get; set; }
        public Action<object> MoveReceived { get; set; }

        public object Settings { get; set; }

        public bool CanMove( object move )
        {
            return _validator.CanMove(move);
        }

        public void Close( )
        {
            RemoteManager.SendData(new RemoteData {
                State = GameStateQuery.Resign
            });
        }

        public List<object> GetValidMoves( object inputPosition )
        {
            return _validator.GetMoves(inputPosition);
        }

        public async Task<object> MakeMove( object moveData )
        {
            var move = await _validator.MakeMove(moveData);
            var userMoved = (GameState)moveData;

            RemoteManager.SendData(new RemoteData {
                State = GameStateQuery.Move,
                Move = new Tuple<BPosition, BPosition>(userMoved.LastMove.Item1, userMoved.LastMove.Item2)
            },
                _connection);
            return move;
        }

        private void LocalReceive( RemoteData data ) {
            if(data.State == GameStateQuery.Message)
            {
                MessagesReceived?.Invoke(new List<object> { data.MessageText });
            }
            else if(data.State == GameStateQuery.Pause)
            {
                // TODO: now its only ignore
            }
            else if(data.State == GameStateQuery.Start)
            {

            }
            else if(data.State == GameStateQuery.Resign)
            {

            }
            else if(data.State == GameStateQuery.Move) {
                MoveReceived?.Invoke(data.Move as Tuple<BPosition, BPosition>);
            }

        }

        public async Task<bool> Open( )
        {
            return await Task.Run(( ) =>
            {
                try
                {
                    _connection = RemoteManager.RecentConnections[0];
                    RemoteManager.ReceiveData(LocalReceive, _connection);
                    return _connection.Connected;
                }
                catch
                {
                    return false;
                }
            });

        }

        public async Task<bool> Pause( )
        {
            return await Task.Run(( ) =>
            {
                RemoteManager.SendData(new RemoteData
                {
                    State = GameStateQuery.Pause
                }, _connection);
                return true;
            });
        }

        public object ProcessMove( object moveData )
        {
            throw new NotImplementedException();
        }

        public bool QueryCancelMove( )
        {
            return false;
        }

        public async Task<bool> Resign( )
        {
            return await Task.Run(( ) =>
            {
                RemoteManager.SendData(new RemoteData
                {
                    State = GameStateQuery.Resign
                }, _connection);
                return true;
            });
        }

        public async Task<object> SendData( object data )
        {
            return await Task.Run(( ) =>
            {
                RemoteManager.SendData(data as RemoteData, _connection);
                return true;
            });
        }

        public async Task<bool> SendMessage( object data )
        {
            return await Task.Run(( ) =>
            {
                RemoteManager.SendData(new RemoteData
                {
                    MessageText = data as string
                }, _connection);
                return true;
            });
        }
    }
}
