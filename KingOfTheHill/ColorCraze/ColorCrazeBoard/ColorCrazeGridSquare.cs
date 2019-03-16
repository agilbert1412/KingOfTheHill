using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill
{
    public class ColorCrazeGridSquare : GridSquare
    {
        public int Owner { get; set; }

        public ColorCrazeGridSquare() : base()
        {
            Owner = -1;
        }
    }
}
