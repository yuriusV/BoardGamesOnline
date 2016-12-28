using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model.View
{
    public class StatisticViewData
    {
        public int CountWins { get; set; }
        public int CountLoses {get;set;}
        public int CountDraws { get; set; }

        public int LongestWins { get; set; }

        public int TotalPlayed { get; set; }
    }
}
