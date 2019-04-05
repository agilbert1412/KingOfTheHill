using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using KingOfTheHill.Board;
using KingOfTheHill.Players;

namespace KingOfTheHill.Spleef.SpleefPlayers
{
    public abstract class SpleefPlayer : Player
    {
        public Stopwatch swPlays = new Stopwatch();
        public int nbPlays = 0;

        public SpleefPlayer()
        {
            Info = new SpleefPlayerInfo();
        }

        public SpleefPlayer(string name, int id)
        {
            Info = new SpleefPlayerInfo(name, id);
        }

        public void Paint(Graphics gfx, GridBoard board, Rectangle bounds)
        {
            var stepX = bounds.Width / board.Width;
            var stepY = bounds.Height / board.Height;

            var ellipse = new Rectangle(((SpleefPlayerInfo)Info).CurrentLocation.X * stepX, ((SpleefPlayerInfo)Info).CurrentLocation.Y * stepY, stepX, stepY);

            gfx.FillEllipse(new SolidBrush(((SpleefPlayerInfo)Info).PlayerColor), ellipse);
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

        internal override PlayerInfo GetInfo()
        {
            return (SpleefPlayerInfo)Info;
        }

        public abstract void StartAll(List<SpleefPlayerInfo> allPlayers);

        public abstract void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores);

        public abstract SpleefDecision PlayTurn(List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard board);

        public List<SpleefPlayerInfo> GetAlivePlayers(List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard board)
        {
            return allPlayers.Where(x => board[x.CurrentLocation.X, x.CurrentLocation.Y].IsSolid).ToList();
        }
    }
}
