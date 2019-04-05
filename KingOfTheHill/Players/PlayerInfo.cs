using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill.Players
{
    public class PlayerInfo
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public Color PlayerColor { get; set; }

        public PlayerInfo()
        {
            Name = "";
            ID = -1;
        }

        public PlayerInfo(string name, int id)
        {
            Name = name;
            ID = id;
        }
    }
}
