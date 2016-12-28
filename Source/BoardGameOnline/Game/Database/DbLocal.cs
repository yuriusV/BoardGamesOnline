using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Model;
using Game.Model.Game;
using Game.Model.User;
using NPoco;
using Game.Model.Game.Chess;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace Game.Database
{

    public static class Serializer {
        public static void Save( object item, string path ) {
           
            using(var sw = new StreamWriter(path)) {
                var seri = new XmlSerializer(item.GetType());
                seri.Serialize(sw, item);
                
            }
        }
        
        public static string SaveString( object item ) {
            var sb = new StringBuilder();
            using(var sw = new StringWriter(sb))
            {
                var seri = new XmlSerializer(item.GetType());
                seri.Serialize(sw, item);

            }
            return sb.ToString();
        }
        
        public static T LoadFromStream<T>( Stream stream ) {
            var seri = new XmlSerializer(typeof(T));
            return (T)seri.Deserialize(stream);
        }

        public static T Load<T>( string path ) {
            using(var sr = new StreamReader(path))
            {
                var seri = new XmlSerializer(typeof(T));
                return (T)seri.Deserialize(sr);
            }
        }
    }

    public static class DbLocal
    {

        private static NPoco.Database _db;

        static DbLocal( ) {
            try
            {
                _db = new NPoco.Database(Properties.Settings.Default.DbConnection,
                    Properties.Settings.Default.DbProvider);
            }
            catch(Exception e) {
                Debug.Write("Cannot connect to database.");
                throw;
            }
            
        }

        public static async Task<bool> SetupLocalUser( User user ) {
            return await Task.Run(( ) => {
                Serializer.Save(user, "user.xml");
                return true;
            });
        }
        
        public static async Task<bool> AddOrUpdateUser( User user ) {
            return await Task.Run(( ) => {
                _db.Save(user);
                return true;
            });
        }

        public static async Task<IEnumerable<User>> FetchUsers( ) {
            return await Task.Run(( ) => {
                return _db.Fetch<User>();
            }); 
        }

        public static async Task<User> GetCurrentUser( ) {
            return await Task.Run(( ) => {
                return Serializer.Load<User>("user.xml");
            });
        }

        public static async void InsertChessResult( ChessGameResult result ) {
            await Task.Run(( ) => {
                result.Uid = Guid.NewGuid().ToString();
                _db.InsertAsync<ChessGameResult>(result);
            });
        }

        public static async Task<IEnumerable<ChessGameResult>> FetchChessResults( )
        {
            return await Task.Run(( ) => {
                return _db.Fetch<ChessGameResult>();
            });
        }
    }
}
