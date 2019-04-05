using System;
using System.Collections.Generic;
using System.Drawing;
using KingOfTheHill.Players;

namespace KingOfTheHill.ColorCraze.ColorCrazePlayers.Bots
{
    public class StraightBot : ColorCrazePlayer
    {
        Random r = new Random();
        Point currentDirection = new Point(0, 0);
        Point lastPosition = new Point(-1, -1);

        public override void StartAll(List<PlayerInfo> allPlayers)
        {
            
        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {
            currentDirection = new Point(0, 0);
            lastPosition = new Point(-1, -1);
        }

        public override ColorCrazeDecision PlayTurn(List<ColorCrazePlayerInfo> allPlayers, Board.Board board)
        {
            // Si on we don't have a current direction, we choose one at random
            if (currentDirection == Point.Empty)
            {
                var choice1 = 1;
                if (r.Next(0, 2) == 0)
                {
                    choice1 = -1;
                }
                var choice2 = 1;
                if (r.Next(0, 2) == 0)
                {
                    choice2 = -1;
                }
                currentDirection = new Point(choice1, choice2);
            }

            // We check if we managed to move last turn
            if (lastPosition.X == GetInfo().CurrentLocation.X && lastPosition.Y == GetInfo().CurrentLocation.Y)
            {
                // We failed, let's turn
                var choice1 = 1;
                if (r.Next(0, 2) == 0)
                {
                    choice1 = -1;
                }
                if (currentDirection.X != 0)
                {
                    currentDirection = new Point(0, choice1);
                }
                else
                {
                    currentDirection = new Point(choice1, 0);
                }
            }

            // Let's remember our position
            lastPosition = GetInfo().CurrentLocation;

            var action = new ColorCrazeDecision(currentDirection);
            return action;
        }
    }
}
