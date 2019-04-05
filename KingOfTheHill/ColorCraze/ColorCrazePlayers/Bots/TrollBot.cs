using System;
using System.Collections.Generic;
using KingOfTheHill.Players;

namespace KingOfTheHill.ColorCraze.ColorCrazePlayers.Bots
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

        public override ColorCrazeDecision PlayTurn(List<ColorCrazePlayerInfo> allPlayers, Board.Board board)
        {
            var directions = Enum.GetValues(typeof(ColorCrazeDirection));
            var randomDir = (ColorCrazeDirection)directions.GetValue(r.Next(directions.Length));
            var action = new ColorCrazeDecision(randomDir);
            return action;
        }
    }
}
