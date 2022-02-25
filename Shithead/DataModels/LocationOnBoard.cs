using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shithead.DataModels
{
    public class LocationOnBoard
    {
        private int x { get; set; }
        private int y { get; set; }
        LocationOnBoard(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
