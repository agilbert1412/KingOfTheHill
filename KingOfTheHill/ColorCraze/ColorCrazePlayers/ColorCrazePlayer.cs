using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill.ColorCraze.Players
{
    public abstract class ColorCrazePlayer : Player
    {
        public ColorCrazePlayer()
        {
            Info = new ColorCrazePlayerInfo();
        }

        public ColorCrazePlayer(string name, int id)
        {
            Info = new ColorCrazePlayerInfo(name, id);
        }

        public void Paint(Graphics gfx, GridBoard board, Rectangle bounds)
        {
            var stepX = bounds.Width / board.Width;
            var stepY = bounds.Height / board.Height;

            var ellipse = new Rectangle(((ColorCrazePlayerInfo)Info).CurrentLocation.X * stepX, ((ColorCrazePlayerInfo)Info).CurrentLocation.Y * stepY, stepX, stepY);

            gfx.FillEllipse(new SolidBrush(((ColorCrazePlayerInfo)Info).PlayerColor), ellipse);
            gfx.DrawEllipse(Pens.Black, ellipse);

            var drawFont = new Font("Arial", stepX / 10);
            var drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;

            gfx.DrawString(Info.Name, drawFont, Brushes.Black, ellipse, drawFormat);
        }

        protected int GetDistance(Point point1, Point point2)
        {
            return GetDistance(point1.X, point1.Y, point2.X, point2.Y);
        }

        protected int GetDistance(int x1, int y1, int x2, int y2)
        {
            return (Math.Abs(x1 - x2) + Math.Abs(y1 - y2));
        }

        public ColorCrazePlayerInfo GetInfo()
        {
            return (ColorCrazePlayerInfo)Info;
        }

        public abstract void StartAll(List<PlayerInfo> allPlayers);
        public abstract void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores);
        public abstract TurnAction PlayTurn(List<ColorCrazePlayerInfo> allPlayers, Board board);
    }
}
