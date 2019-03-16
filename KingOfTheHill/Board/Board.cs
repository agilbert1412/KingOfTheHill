using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill
{
    public abstract class Board
    {
        public abstract void Paint(Graphics gfx, Rectangle bounds, bool clear);
    }
}
