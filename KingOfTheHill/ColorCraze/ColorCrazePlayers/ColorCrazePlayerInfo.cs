﻿using System.Drawing;
using KingOfTheHill.Players;

namespace KingOfTheHill.ColorCraze.ColorCrazePlayers
{
    public class ColorCrazePlayerInfo : PlayerInfo
    {
        public Point CurrentLocation { get; set; }

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
