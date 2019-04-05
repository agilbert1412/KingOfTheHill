using System;
using System.Collections.Generic;
using System.Drawing;
using KingOfTheHill.Players;

namespace KingOfTheHill.ColorCraze.ColorCrazePlayers.Bots
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

        public override ColorCrazeDecision PlayTurn(List<ColorCrazePlayerInfo> allPlayers, Board.Board board)
        {
            // I suck at this game
            var action = new ColorCrazeDecision(Point.Empty);
            return action;
        }
    }
}
