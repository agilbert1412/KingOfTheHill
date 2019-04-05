using System.Drawing;

namespace KingOfTheHill.Board
{
    public class GridSquare
    {
        public virtual Color Color { get; set; }

        public GridSquare()
        {
            Color = Color.White;
        }
    }
}
