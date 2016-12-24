using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Model;
using Game.Model.Game;
using Game.Model.User;
using NPoco;


namespace Game.Database
{
    public static class DbLocal
    {
        public static async Task<bool> SetupLocalUser( User user ) {
            return await Task.Run(( ) => {

                return true;
            });
        }
        
        public static async Task<bool> AddOrUpdateUser( User user ) {
            return await Task.Run(( ) => {

                return true;
            });
        }

        public static async Task<IEnumerable<User>> FetchUsers( ) {
            return await Task.Run(( ) => {

                return new User[1];
            });
        }

        public static async Task<User> GetCurrentUser( ) {
            return await Task.Run(( ) => {

                return new User();
            });
        }


    }
}
