using Game.Database;
using Game.Model.Remote;
using Game.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Game.Remote
{
    public class RemoteManager
    {
        public static List<Connection> RecentConnections { get; set; } = new List<Connection>();

        private static Server _server;
        private static WebClient _cl = new WebClient();

        public static bool ServerStarted { get; set; }

        public static bool StartServer() {
            try
            {
                _server = new Server(Config.Get().ListenPort);
                _server.Start();
                ServerStarted = true;
                return true;
            }
            catch(Exception e) {
                Debug.Write(e.Message);
                ServerStarted = false;
                return false;
            }
        }

        public static void StopServer( ) {
            try
            {
                ServerStarted = false;
                _server.Stop();
            }
            catch(Exception e) {
                Debug.Write("Cannot stop server: " + e.Message);
            }
        }

        public static bool ConnectTo( Connection conData ) {
            try
            {
                var data = _cl.DownloadString($"http://{conData.Address}:{conData.Port}");
                if(data == null)
                {
                    return false;
                }
                else
                {
                    RecentConnections.Add(conData);
                    return true;
                }
            }
            catch {
                return false;
            }
        }

        public static void Disconnect( Connection conData ) {
            _server.RemoveListener($"{conData.Address}:{conData.Port}");
        }

        public static string SendData( RemoteData data, Connection partner = null ) {
            return Encoding.UTF8.GetString(_cl.UploadData($"http://{partner.Address}:{partner.Port}", 
                Encoding.UTF8.GetBytes(Serializer.SaveString(data))));
        }

        public static void ReceiveData( Action<RemoteData> received, Connection parnter = null ) {
            _server.AddListener($"{parnter.Address}:{parnter.Port}", (data) => {
                received?.Invoke(data); return new RemoteData();
            });
        }
    }

    public class Connection {
        public string Address { get; set; }
        public string Port { get; set; }
        public UserData Partner { get; set; }

        public bool Connected { get; set; }
    }
}
