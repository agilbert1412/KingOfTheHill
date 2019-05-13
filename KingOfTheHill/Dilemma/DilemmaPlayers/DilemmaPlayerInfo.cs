using System.Drawing;
using KingOfTheHill.Players;

namespace KingOfTheHill.Dilemma.DilemmaPlayers
{
    public class DilemmaPlayerInfo : PlayerInfo
    {

        public DilemmaPlayerInfo() : base()
        {
        }

        public DilemmaPlayerInfo(string name, int id) : base(name, id)
        {
        }

        public DilemmaPlayerInfo Clone()
        {
            var clone = new DilemmaPlayerInfo(Name.Clone().ToString(), ID);
            clone.PlayerColor = this.PlayerColor;
            return clone;
        }
    }
}
