using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill.Players
{
    public abstract class Player
    {
        public PlayerInfo Info { get; set; }

        public Player()
        {
            Info = new PlayerInfo();
        }

        public Player(string name, int id)
        {
            Info = new PlayerInfo(name, id);
        }

        public virtual PlayerInfo GetInfo()
        {
            return Info;
        }
    }
}
