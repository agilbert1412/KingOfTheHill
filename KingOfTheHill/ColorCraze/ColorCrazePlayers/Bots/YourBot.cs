using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill.ColorCraze.Players
{
    public class YourBot : ColorCrazePlayer
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
            // I suck at this game
            var action = new TurnAction(Info, new ColorCrazeDecision(Point.Empty));
            return action;
        }
    }
}
