using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using KingOfTheHill.Board;
using KingOfTheHill.ColorCraze.ColorCrazeBoard;
using KingOfTheHill.Players;

namespace KingOfTheHill.ColorCraze.ColorCrazePlayers.Bots
{


    public class GS_Bot : ColorCrazePlayer
    {
        public interface ITurnStrategy
        {
            MovementGrade Grade(ColorCrazePlayerInfo playerInfo, Dictionary<int, Point> playersLocationMap);
        }

        public class MovementGrade
        {
            public MovementGrade(Point location, int grade, ColorCrazeDirection? direction = null)
            {
                Grade = grade;
                Location = location;
                Direction = direction;
            }

            public ColorCrazeDirection? Direction { get; }

            public Point Location { get; }

            public int Grade { get; }
        }

        public class StraitCapture : ITurnStrategy
        {
            private GridBoard board;
            private readonly List<ColorCrazeDirection> directions = new List<ColorCrazeDirection>()
        {
            ColorCrazeDirection.Down,
            ColorCrazeDirection.Up,
            ColorCrazeDirection.Left,
            ColorCrazeDirection.Right
        };

            public StraitCapture(GridBoard board)
            {
                this.board = board;
            }

            public MovementGrade Grade(ColorCrazePlayerInfo playerInfo, Dictionary<int, Point> playersLocationMap)
            {
                var fromLocation = playerInfo.CurrentLocation;
                var occupiedLocations = playersLocationMap.Values.ToList();
                var numberOfPlayers = occupiedLocations.Count;

                if (numberOfPlayers == 1)
                    return new MovementGrade(fromLocation, 100);

                var turnSimulator = new TurnSimulator(this.board);
                var gradeStep = 100 / (numberOfPlayers - 1); //step in %

                var maxGradedMovement = new MovementGrade(fromLocation, 0);
                foreach (var direction in this.directions)
                {
                    var grade = 0;
                    var nextLocation = fromLocation;
                    for (var i = 0; i < numberOfPlayers - 1; i++)
                    {
                        nextLocation = turnSimulator.Simulate(occupiedLocations, nextLocation, direction);
                        var square = board.Squares[nextLocation.X, nextLocation.Y] as ColorCrazeGridSquare;
                        var isOwned = square.Owner == playerInfo.ID;
                        if (nextLocation != fromLocation && !isOwned)
                        {
                            grade += gradeStep;
                            var nextMovement = new MovementGrade(nextLocation, grade, direction);
                            maxGradedMovement = maxGradedMovement.Grade < grade ? nextMovement : maxGradedMovement;
                        }
                    }
                }

                return maxGradedMovement;
            }
        }

        internal class TurnEvaluator
        {
            private readonly List<ITurnStrategy> strategies;

            public TurnEvaluator(Board.Board board)
            {
                var gridBoard = board as GridBoard;
                strategies = new List<ITurnStrategy>()
            {
                new StraitCapture(gridBoard)
            };
            }

            public MovementGrade GetTopGradedMovement(ColorCrazePlayerInfo playerInfo, Dictionary<int, Point> playersLocationMap)
            {
                var scores = new Dictionary<ITurnStrategy, MovementGrade>();
                var maxGrade = 0;
                foreach (var strategy in strategies)
                {
                    var gradedMovement = strategy.Grade(playerInfo, playersLocationMap);
                    scores.Add(strategy, gradedMovement);
                    maxGrade = maxGrade < gradedMovement.Grade ? gradedMovement.Grade : maxGrade;
                }

                var movement = scores.First(i => i.Value.Grade == maxGrade).Value;
                return movement;
            }
        }

        internal class TurnSimulator
        {
            private readonly GridBoard board;

            public TurnSimulator(GridBoard board)
            {
                this.board = board;
            }

            public Point Simulate(List<Point> occupiedLocations, Point fromLocation, ColorCrazeDirection direction)
            {
                var movement = fromLocation;
                switch (direction)
                {
                    case ColorCrazeDirection.Up:
                        movement = new Point(fromLocation.X, fromLocation.Y - 1);
                        break;
                    case ColorCrazeDirection.Down:
                        movement = new Point(fromLocation.X, fromLocation.Y + 1);
                        break;
                    case ColorCrazeDirection.Left:
                        movement = new Point(fromLocation.X - 1, fromLocation.Y);
                        break;
                    case ColorCrazeDirection.Right:
                        movement = new Point(fromLocation.X + 1, fromLocation.Y);
                        break;
                }

                if (occupiedLocations.Contains(movement))
                    return fromLocation;

                if (movement.X >= this.board.Width
                    || movement.Y >= this.board.Height
                    || movement.X < 0
                    || movement.Y < 0)
                    return fromLocation;

                return movement;
            }
        }

        public override void StartAll(List<PlayerInfo> allPlayers)
        {
            //this.Info = new ColorCrazePlayerInfo("ZBot", 10);
        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {

        }

        public override ColorCrazeDecision PlayTurn(List<ColorCrazePlayerInfo> players, Board.Board board)
        {
            var playersLocationMap = new Dictionary<int, Point>();
            foreach (var player in players)
            {
                playersLocationMap.Add(player.ID, player.CurrentLocation);
            }

            var zBotPlayer = this.GetInfo();
            var turnEvaluator = new TurnEvaluator(board);
            var nextMovementPoint = turnEvaluator.GetTopGradedMovement(
                zBotPlayer,
                playersLocationMap);

            return nextMovementPoint.Direction == null
                ? new ColorCrazeDecision(zBotPlayer.CurrentLocation)
                : new ColorCrazeDecision((ColorCrazeDirection)nextMovementPoint.Direction);
        }
    }


}
