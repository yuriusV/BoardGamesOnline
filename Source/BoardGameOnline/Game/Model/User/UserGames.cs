using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPoco;

namespace Game.Model.User
{
    [PrimaryKey("id")]
    [TableName("user_games")]
    public class UserGames
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public string UserUid { get; set; }

        [Column("game_uid")]
        public string GameUid { get; set; }
    }
}
