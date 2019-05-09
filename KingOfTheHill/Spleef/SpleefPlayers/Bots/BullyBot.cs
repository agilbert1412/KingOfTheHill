using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using KingOfTheHill.Players;

namespace KingOfTheHill.Spleef.SpleefPlayers.Bots
{
    public class BullyBot : SpleefPlayer
    {
        private int timeSpentHere = 0;
        private SpleefBoard.SpleefBoard _currentBoard = null;

        public override void StartAll(List<SpleefPlayerInfo> allPlayers)
        {

        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {
            timeSpentHere = 0;
        }

        public override SpleefDecision PlayTurn(List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard spleefBoard)
        {
            _currentBoard = spleefBoard;
            var myLocation = GetMyLocation();
            var aliveEnemies = GetAlivePlayers(allPlayers, spleefBoard).Where(x => x.ID != Info.ID).ToList();

            var closestPlayers = FindClosestPlayersByDistance(aliveEnemies, myLocation);

            if (!closestPlayers.Any())
            {
                return SpleefDecision.DefaultDecision;
            }

            var target = closestPlayers[RandomGen.Next(0, closestPlayers.Count)];
            var targetLocation = GetPlayerLocation(target);

            var distance = GetDistance(targetLocation, myLocation);

            if (distance > 0 && distance < 3 && timeSpentHere < 3)
            {
                timeSpentHere++;
                var offsetFromMe = new Point(targetLocation.X - myLocation.X, targetLocation.Y - myLocation.Y);
                return new SpleefDecision(SpleefAction.Hole, offsetFromMe);
            }

            List<Point> targetPath = null;
            var easiestToReachPlayers = FindClosestPlayersByPath(aliveEnemies, myLocation);
            while (targetPath == null && easiestToReachPlayers.Any())
            {
                var targetToRunAfter = easiestToReachPlayers[RandomGen.Next(0, easiestToReachPlayers.Count)];
                targetLocation = GetPlayerLocation(targetToRunAfter);
                targetPath = FindShortestPathAStar(spleefBoard, myLocation, targetLocation);
                easiestToReachPlayers.Remove(targetToRunAfter);
            }

            if (targetPath != null && targetPath.Count > 1)
            {
                var thisTurnDest = targetPath[1];
                var offsetFromMe = new Point(thisTurnDest.X - myLocation.X, thisTurnDest.Y - myLocation.Y);

                timeSpentHere = 0;
                return new SpleefDecision(SpleefAction.Move, offsetFromMe);
            }

            timeSpentHere = 0;
            return ChooseRandomSurvivalDirection(spleefBoard, myLocation);
        }

        private SpleefDecision ChooseRandomSurvivalDirection(SpleefBoard.SpleefBoard board, Point myLocation)
        {
            var possibilities = new List<SpleefDecision>();

            possibilities.Add(SpleefDecision.DefaultDecision);

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (j != 0 || i != 0)
                    {
                        var moveAction = new SpleefDecision(SpleefAction.Move, new Point(i, j));

                        if (moveAction.IsValid)
                        {
                            var dest = new Point(myLocation.X + moveAction.Target.X, myLocation.Y + moveAction.Target.Y);

                            if (dest.X >= 0 && dest.X < board.Width && dest.Y >= 0 && dest.Y < board.Height)
                            {
                                if (board[dest.X, dest.Y].IsSolid)
                                {
                                    possibilities.Add(moveAction);
                                }
                            }
                        }
                    }
                }
            }

            return possibilities[RandomGen.Next(0, possibilities.Count)];
        }

        private List<SpleefPlayerInfo> FindClosestPlayersByDistance(List<SpleefPlayerInfo> players, Point locationFrom)
        {
            return FindClosestPlayers(players, locationFrom, GetDistance);
        }

        private List<SpleefPlayerInfo> FindClosestPlayersByPath(List<SpleefPlayerInfo> players, Point locationFrom)
        {
            return FindClosestPlayers(players, locationFrom, GetPathDistance);
        }

        private int GetPathDistance(Point p1, Point p2)
        {
            var path =  FindShortestPathAStar(_currentBoard, p1, p2);
            if (path == null)
            {
                return int.MaxValue;
            }
            return path.Count;
        }

        private List<SpleefPlayerInfo> FindClosestPlayers(List<SpleefPlayerInfo> players, Point locationFrom, Func<Point, Point, int> EvaluationFunction)
        {
            var closestPlayers = new List<SpleefPlayerInfo>();
            var closestPlayerDistance = int.MaxValue;

            foreach (var p in players)
            {
                var loc = GetPlayerLocation(p);
                var dist = GetDistance(locationFrom, loc);

                if (dist < closestPlayerDistance)
                {
                    closestPlayerDistance = dist;
                    closestPlayers = new List<SpleefPlayerInfo>();
                }

                if (dist == closestPlayerDistance)
                {
                    closestPlayers.Add(p);
                }
            }

            return closestPlayers;
        }
    }
}
