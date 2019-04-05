using KingOfTheHill.Board;

namespace KingOfTheHill.ColorCraze.ColorCrazeBoard
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
