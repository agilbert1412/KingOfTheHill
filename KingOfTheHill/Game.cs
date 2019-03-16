using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill
{
    public abstract class Game
    {
        public abstract void Paint(Graphics gfx, Rectangle bounds);
        public abstract bool PlayStep(Dictionary<PlayerInfo, int> playersScores);
        public abstract Dictionary<PlayerInfo, int> GetStatus();
    }
}
