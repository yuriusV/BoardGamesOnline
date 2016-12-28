using Game.Model.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Game.Database;

namespace Game.Remote
{
    public class Server
    {
        private TcpListener _listener;
        private int _port = 80;
        private Task _task;
        private Dictionary<string, Func<RemoteData, RemoteData>> _listeners;

        public Server( int port )
        {
            _port = port;
            _listeners = new Dictionary<string, Func<RemoteData, RemoteData>>();
        }

        public void AddListener(string adress, Func<RemoteData, RemoteData> listen ) {
            _listeners[adress] = listen;
        }

        public void RemoveListener( string adress ) {
            try
            {
                _listeners.Remove(adress);
            }
            catch { }
        }

        public void Start( )
        {
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();
            _task = Task.Run(( ) =>
            {
                while(true)
                {
                    ProccessClient(_listener.AcceptTcpClient());
                }
            });

        }

        protected virtual void ProccessClient( TcpClient client )
        {
            var page = GetAnswerForClient(client);
            if(!client.Connected) return;
            var all = "HTTP/1.1 200 OK\nContent-type: text/html\nContent-Length:" +
                page.Length + "\n\n" + page;

            var data = Encoding.UTF8.GetBytes(all);
            client.GetStream().Write(data, 0, data.Length);
            client.Close();
        }

        protected virtual string GetAnswerForClient( TcpClient client ) {
            try
            {
                var data = Serializer.LoadFromStream<RemoteData>(client.GetStream());

                var endPoint = client.Client.RemoteEndPoint.ToString();
                if(_listeners.ContainsKey(endPoint))
                {
                    var answer = _listeners[endPoint](data);
                    return Serializer.SaveString(answer);
                }

                return string.Empty;
            }
            catch {
                return string.Empty;
            }

                
        }

        public void Stop( )
        {
            _task.Dispose();
            _listener.Stop();
        }
    }
}
