using KingOfTheHill.Board;
using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace KingOfTheHill.Dilemma.DilemmaPlayers
{
    public abstract class DilemmaPlayer : Player
    {
        public Stopwatch swPlays = new Stopwatch();
        public int nbPlays = 0;



        protected Random RandomGen = new Random();

        public DilemmaPlayer()
        {
            Info = new DilemmaPlayerInfo();
        }

        public DilemmaPlayer(string name, int id)
        {
            Info = new DilemmaPlayerInfo(name, id);
        }

        public void Paint(Graphics gfx, Rectangle bounds)
        {
            var picRect = new Rectangle(bounds.X, bounds.Y + (bounds.Height * 4 / 5), bounds.Width, bounds.Height / 5);
            var nameRect = new Rectangle(bounds.X, bounds.Y + (bounds.Height/ 5), bounds.Width, bounds.Height * 4 / 5);

            gfx.FillRectangle(new SolidBrush(Lighten(((DilemmaPlayerInfo)Info).PlayerColor)), nameRect);

            var drawFont = new Font("Arial", nameRect.Width / Info.Name.Length, FontStyle.Bold);
            var drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;

            gfx.DrawString(Info.Name, drawFont, Brushes.Black, nameRect, drawFormat);
        }

        public override PlayerInfo GetInfo()
        {
            return (DilemmaPlayerInfo)Info;
        }

        public abstract void StartAll(List<DilemmaPlayerInfo> allPlayers);

        public abstract DilemmaDecision PlayTurn(DilemmaPlayerInfo currentOpponent, Dictionary<PlayerInfo, int> allPlayersAndScores, List<DilemmaGame> completeHistory);

        #region Useful Methods

        private Color Lighten(Color origColor)
        {
            return Color.FromArgb(
                origColor.A,
                (int)Math.Min(origColor.R/* * (1 + (percent / 100f))*/ + 40, 255),
                (int)Math.Min(origColor.G/* * (1 + (percent / 100f))*/ + 40, 255),
                (int)Math.Min(origColor.B/* * (1 + (percent / 100f))*/ + 40, 255)
            );
        }

        #endregion Useful Methods
    }
}
