using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill
{
    public class GridBoard : Board
    {
        public int Width { get; }
        public int Height { get; }
        
        public GridSquare[,] Squares { get; set; }

        public GridBoard(int width, int height) : base()
        {
            Width = width;
            Height = height;
            Squares = new GridSquare[width, height];
        }

        public override void Paint(Graphics gfx, Rectangle bounds, bool clear)
        {
            if (clear)
            {
                gfx.Clear(Color.White);
            }

            gfx.DrawRectangle(Pens.Black, bounds);

            var stepX = bounds.Width / Width;
            var stepY = bounds.Height / Height;

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

        public virtual bool IsFull()
        {
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    if (Squares[i, j].Color == Color.White)
                        return false;
                }
            }
            return true;
        }
    }
}
