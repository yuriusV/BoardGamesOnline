using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPoco;


namespace Game.Model.User
{
    [TableName("user")]
    [PrimaryKey("id")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("uid")]
        public string Uid { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("password")]
        public string Password { get; set; }
    }
}
