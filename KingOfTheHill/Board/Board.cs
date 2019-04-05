using System.Drawing;

namespace KingOfTheHill.Board
{
    public abstract class Board
    {
        public abstract void Paint(Graphics gfx, Rectangle bounds, bool clear);
    }
}
