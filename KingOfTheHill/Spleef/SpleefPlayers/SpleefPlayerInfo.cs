using System.Drawing;
using KingOfTheHill.Players;

namespace KingOfTheHill.Spleef.SpleefPlayers
{
    public class SpleefPlayerInfo : PlayerInfo
    {
        public Point CurrentLocation { get; set; }

        public SpleefPlayerInfo() : base()
        {
            CurrentLocation = new Point(0, 0);
        }

        public SpleefPlayerInfo(string name, int id) : base(name, id)
        {
            CurrentLocation = new Point(0, 0);
        }
    }
}
