using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill.ColorCraze.Players
{
    public class TrollBot : ColorCrazePlayer
    {
        Random r = new Random();

        public override void StartAll(List<PlayerInfo> allPlayers)
        {
            
        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {

        }

        public override TurnAction PlayTurn(List<ColorCrazePlayerInfo> allPlayers, Board board)
        {
            var directions = Enum.GetValues(typeof(ColorCrazeDirection));
            var randomDir = (ColorCrazeDirection)directions.GetValue(r.Next(directions.Length));
            var action = new TurnAction(Info, new ColorCrazeDecision(randomDir));
            return action;
        }
    }
}
