using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

namespace KingOfTheHill.ColorCraze.Players
{
    public class JealousBot : ColorCrazePlayer
    {
        private class TurnAdviser
        {
            public int MyId { private get; set; }
            private readonly Dictionary<ColorCrazeDirection, int> TargetQuadrantByDirection = new Dictionary<ColorCrazeDirection, int>
            {
                {ColorCrazeDirection.Up,1},
                {ColorCrazeDirection.Right,2},
                {ColorCrazeDirection.Down,3},
                {ColorCrazeDirection.Left,4}
            };


            internal ColorCrazeDirection AdviceNextTurn(List<EnemyData> enemies, List<NextMoveCandidate> nextMoves)
            {
                foreach (var move in nextMoves)
                {
                    // Prefer capturing color of top 3
                    var playerRating = IsOneOfTop(enemies, move.OwnerId, 3);
                    if (playerRating != -1)
                    {
                        move.MoveScore += 10 * (enemies.Count - playerRating);
                    }

                    // Prefer going in direction of top 3
                    if (IsApproachTop3(enemies, TargetQuadrantByDirection[move.Direction]))
                    {
                        move.MoveScore += 3;
                    }

                    // Prefer capturing owned instead of going for white
                    if (move.OwnerId == -1)
                    {
                        move.MoveScore -= 1;
                    }

                    // Avoid going over owned color
                    if (move.OwnerId == MyId)
                    {
                        move.MoveScore -= 10;
                    }
                }

                var movesByScore = nextMoves.OrderByDescending(m => m.MoveScore);
                return movesByScore.First().Direction;
            }


            private static bool IsApproachTop3(List<EnemyData> enemies, int quadrant)
            {
                var sorted = enemies.OrderByDescending(e => e.NumberOfPoints);
                var top3 = sorted.Take(3);
                var cellCountQuadrant1 = 0;
                var cellCountQuadrant2 = 0;
                var cellCountQuadrant3 = 0;
                var cellCountQuadrant4 = 0;

                foreach (var enemyData in top3)
                {
                    cellCountQuadrant1 += enemyData.Points.Count(p => p.RelativeQuadrant == 1);
                    cellCountQuadrant2 += enemyData.Points.Count(p => p.RelativeQuadrant == 2);
                    cellCountQuadrant3 += enemyData.Points.Count(p => p.RelativeQuadrant == 3);
                    cellCountQuadrant4 += enemyData.Points.Count(p => p.RelativeQuadrant == 4);
                }

                if ((quadrant == 1 &&
                     cellCountQuadrant1 >= cellCountQuadrant2 &&
                     cellCountQuadrant1 >= cellCountQuadrant3 &&
                     cellCountQuadrant1 >= cellCountQuadrant4)
                    ||
                    (quadrant == 2 &&
                     cellCountQuadrant2 >= cellCountQuadrant1 &&
                     cellCountQuadrant2 >= cellCountQuadrant3 &&
                     cellCountQuadrant2 >= cellCountQuadrant4)
                    ||
                    (quadrant == 3 &&
                     cellCountQuadrant3 >= cellCountQuadrant1 &&
                     cellCountQuadrant3 >= cellCountQuadrant2 &&
                     cellCountQuadrant3 >= cellCountQuadrant4)
                    ||
                    (quadrant == 4 &&
                     cellCountQuadrant4 >= cellCountQuadrant1 &&
                     cellCountQuadrant4 >= cellCountQuadrant2 &&
                     cellCountQuadrant4 >= cellCountQuadrant3))
                {
                    return true;
                }

                return false;
            }
            private static int IsOneOfTop(List<EnemyData> enemies, int moveOwnerId, int topCount)
            {
                var sorted = enemies.OrderByDescending(e => e.NumberOfPoints);
                var topPlayers = sorted.Take(topCount).ToArray();

                for (var i = 0; i < topCount; i++)
                {
                    if (topPlayers[i].EnemyId == moveOwnerId)
                        return i;
                }

                return -1;
            }
        }

        private class BoardAnalysis
        {
            public int MyId { private get; set; }


            public List<EnemyData> AnalyzeBoard(ColorCrazeBoard board, int myX, int myY)
            {
                var enemies = new List<EnemyData>();


                for (var i = 0; i < board.Width; i++)
                {
                    if (i == MyId) continue;

                    enemies.Add(new EnemyData { EnemyId = i });
                }

                Parallel.ForEach(enemies, enemy =>
                {
                    var points = new List<EnemyPoint>();

                    for (var i = 0; i < board.Width; i++)
                    {
                        for (var j = 0; j < board.Height; j++)
                        {
                            if (board[i, j].Owner != enemy.EnemyId)
                            {
                                continue;
                            }

                            var point = new EnemyPoint
                            {
                                X = i,
                                Y = j,
                                dX = Math.Abs(i - myX),
                                dY = Math.Abs(j - myY)
                            };


                            if (i <= myX && j < myY)
                            {
                                point.RelativeQuadrant = 1;
                            }
                            else if (i > myX && j <= myY)
                            {
                                point.RelativeQuadrant = 2;
                            }
                            else if (i >= myX && j > myY)
                            {
                                point.RelativeQuadrant = 3;
                            }
                            else if (i < myX && j >= myY)
                            {
                                point.RelativeQuadrant = 4;
                            }

                            points.Add(point);
                        }
                    }

                    enemy.Points = points;
                });

                return enemies;
            }
            public List<NextMoveCandidate> GetPossibleMoves(ColorCrazeBoard board, List<ColorCrazePlayerInfo> allPlayers, int myX, int myY)
            {
                var nextMoves = new List<NextMoveCandidate>();

                if (myX - 1 >= 0 && NotOccupied(allPlayers, myX - 1, myY))
                {
                    nextMoves.Add(new NextMoveCandidate
                    {
                        Direction = ColorCrazeDirection.Left,
                        OwnerId = board[myX - 1, myY].Owner
                    });
                }

                if (myX + 1 < board.Width && NotOccupied(allPlayers, myX + 1, myY))
                {
                    nextMoves.Add(new NextMoveCandidate
                    {
                        Direction = ColorCrazeDirection.Right,
                        OwnerId = board[myX + 1, myY].Owner
                    });
                }

                if (myY - 1 >= 0 && NotOccupied(allPlayers, myX, myY - 1))
                {
                    nextMoves.Add(new NextMoveCandidate
                    {
                        Direction = ColorCrazeDirection.Up,
                        OwnerId = board[myX, myY - 1].Owner
                    });
                }

                if (myY + 1 < board.Height && NotOccupied(allPlayers, myX, myY + 1))
                {
                    nextMoves.Add(new NextMoveCandidate
                    {
                        Direction = ColorCrazeDirection.Down,
                        OwnerId = board[myX, myY + 1].Owner
                    });
                }

                if (nextMoves.Count == 0) //We are locked :(
                {
                    nextMoves.Add(new NextMoveCandidate{Direction = ColorCrazeDirection.Up});
                }

                return nextMoves;
            }


            private static bool NotOccupied(List<ColorCrazePlayerInfo> allPlayers, int x, int y)
            {
                return !allPlayers.Any(p => p.CurrentLocation.X == x && p.CurrentLocation.Y == y);
            }
        }

        private class EnemyData
        {
            public EnemyData()
            {
                Points = new List<EnemyPoint>();
            }
            public int EnemyId { get; set; }
            public List<EnemyPoint> Points { get; set; }
            public int NumberOfPoints => Points.Count;
        }

        private class EnemyPoint
        {
            public int X { get; set; }
            public int Y { get; set; }

            public int dX { get; set; }
            public int dY { get; set; }

            public int RelativeQuadrant { get; set; }
        }

        private class NextMoveCandidate
        {
            public ColorCrazeDirection Direction { get; set; }
            public int OwnerId { get; set; }
            public double MoveScore { get; set; }
        }


        private TurnAdviser _turnAdviser;
        private BoardAnalysis _boardAnalysis;


        public override void StartAll(List<PlayerInfo> allPlayers)
        {
        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {

            _turnAdviser = new TurnAdviser {MyId = Info.ID};
            _boardAnalysis = new BoardAnalysis {MyId = Info.ID};
        }

        public override TurnAction PlayTurn(List<ColorCrazePlayerInfo> allPlayers, Board board)
        {
            var myX = ((ColorCrazePlayerInfo)Info).CurrentLocation.X;
            var myY = ((ColorCrazePlayerInfo)Info).CurrentLocation.Y;
            var enemies = _boardAnalysis.AnalyzeBoard((ColorCrazeBoard)board, myX, myY);
            var nextMoves = _boardAnalysis.GetPossibleMoves((ColorCrazeBoard)board, allPlayers, myX, myY);
            var direction = _turnAdviser.AdviceNextTurn(enemies, nextMoves);
            var action = new TurnAction(Info, new ColorCrazeDecision(direction));
            return action;
        }
    }
}
