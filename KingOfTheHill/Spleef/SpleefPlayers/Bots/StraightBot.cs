using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingOfTheHill.Players;

namespace KingOfTheHill.Spleef.SpleefPlayers.Bots
{
    public class StraightBot : SpleefPlayer
    {
        private Point currentDirection = Point.Empty;

        public override void StartAll(List<SpleefPlayerInfo> allPlayers)
        {

        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {
            currentDirection = Point.Empty;
        }

        public override SpleefDecision PlayTurn(List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard spleefBoard)
        {
            if (currentDirection == Point.Empty) {
                ChooseDirection();
            }

            var myLocation = GetMyLocation();

            var tries = 20;
            while (tries > 0 && (!CanGoDirection(spleefBoard, myLocation) || currentDirection == Point.Empty)) {
                ChooseDirection();
                tries--;
            }

            return new SpleefDecision(SpleefAction.Move, currentDirection);
        }

        private bool CanGoDirection(SpleefBoard.SpleefBoard board, Point myLocation)
        {
            var destination = GetTargetSquare(myLocation, currentDirection);
            return (destination.X >= 0 && destination.Y >= 0 && destination.X < board.Width &&
                    destination.Y < board.Height) && board[destination.X, destination.Y].IsSolid;
        }

        private void ChooseDirection()
        {
            currentDirection = Point.Empty;
            currentDirection.X = RandomGen.Next(0, 3) - 1;
            currentDirection.Y = RandomGen.Next(0, 3) - 1;
        }
    }
}
