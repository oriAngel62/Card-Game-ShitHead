using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shithead.DatabaseCommunication
{
    public class PlayerData
    {
        public string Name { get; set; }
        public int Win { get; set; }
        public int Lose { get; set; }
        public int Score { get; set; }

       public PlayerData(string name, int win, int lose, int score)
        {
            Name = name;
            Win = win;
            Lose = lose;
            Score = score;
        }
    }
}
