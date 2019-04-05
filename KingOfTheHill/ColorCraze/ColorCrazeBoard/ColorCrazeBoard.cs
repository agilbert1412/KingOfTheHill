using KingOfTheHill.Board;

namespace KingOfTheHill.ColorCraze.ColorCrazeBoard
{
    public class ColorCrazeBoard : GridBoard
    {
        public ColorCrazeBoard(int width, int height) : base(width, height)
        {
            Squares = new GridSquare[width, height];
            
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    Squares[i, j] = new ColorCrazeGridSquare();
                }
            }
        }

        public override bool IsFull()
        {
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    if (((ColorCrazeGridSquare)(Squares[i, j])).Owner == -1)
                        return false;
                }
            }
            return true;
        }

        public virtual int CountSquaresOfOwner(int owner)
        {
            var num = 0;
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    if (((ColorCrazeGridSquare)(Squares[i, j])).Owner == owner)
                        num++;
                }
            }
            return num;
        }

        public ColorCrazeGridSquare this[int i,int j]
        {
            get
            {
                return (ColorCrazeGridSquare)Squares[i, j];
            }
        }
    }
}
