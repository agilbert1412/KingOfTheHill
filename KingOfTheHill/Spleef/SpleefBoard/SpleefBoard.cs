using System.Drawing;
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

        public override void Paint(Graphics gfx, Rectangle bounds, bool clear)
        {
            if (clear)
            {
                gfx.Clear(Color.White);
            }

            gfx.DrawRectangle(Pens.Black, bounds);

            var startRow = 0;
            var startColumn = 0;
            var endRow = Width - 1;
            var endColumn = Height - 1;

            var stepX = bounds.Width / Width;
            var stepY = bounds.Height / Height;

            while (!RowHasInterestingSquares(startRow, IsInterestingSquare))
            {
                startRow++;
            }
            while (!RowHasInterestingSquares(endRow, IsInterestingSquare))
            {
                endRow--;
            }

            while (!ColumnHasInterestingSquares(startColumn, IsInterestingSquare))
            {
                startColumn++;
            }
            while (!ColumnHasInterestingSquares(endColumn, IsInterestingSquare))
            {
                endColumn--;
            }

            for (var i = bounds.X; i < bounds.Width; i += stepX)
            {
                gfx.DrawLine(Pens.Black, i, bounds.Y, i, bounds.Y + bounds.Height);
            }

            for (var i = bounds.Y; i < bounds.Height; i += stepY)
            {
                gfx.DrawLine(Pens.Black, bounds.X, i, bounds.X + bounds.Width, i);
            }

            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    gfx.FillRectangle(new SolidBrush(Squares[i, j].Color), (i * stepX) + 1, (j * stepY) + 1, stepX - 1, stepY - 1);
                }
            }
        }

        private bool IsInterestingSquare(GridSquare square)
        {
            if (square is SpleefGridSquare spleefSquare)
            {
                return spleefSquare.IsSolid;
            }

            return true;
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
