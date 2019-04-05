using KingOfTheHill.Board;

namespace KingOfTheHill.Spleef.SpleefBoard
{
    public class SpleefBoard : GridBoard
    {
        public SpleefBoard(int width, int height) : base(width, height)
        {
            Squares = new GridSquare[width, height];
            
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    Squares[i, j] = new SpleefGridSquare();
                }
            }
        }

        public SpleefGridSquare this[int i,int j]
        {
            get
            {
                return (SpleefGridSquare)Squares[i, j];
            }
        }
    }
}
