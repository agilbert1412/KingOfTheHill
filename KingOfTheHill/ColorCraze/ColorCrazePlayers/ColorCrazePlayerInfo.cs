using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill.ColorCraze.Players
{
    public class ColorCrazePlayerInfo : PlayerInfo
    {
        public Point CurrentLocation { get; set; }
        public Color PlayerColor { get; set; }

        public ColorCrazePlayerInfo() : base()
        {
            CurrentLocation = new Point(0, 0);
        }

        public ColorCrazePlayerInfo(string name, int id) : base(name, id)
        {
            CurrentLocation = new Point(0, 0);
        }
    }
}
