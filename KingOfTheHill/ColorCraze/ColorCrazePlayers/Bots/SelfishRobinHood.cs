using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using KingOfTheHill.ColorCraze.ColorCrazeBoard;
using KingOfTheHill.Players;

namespace KingOfTheHill.ColorCraze.ColorCrazePlayers.Bots
{
    public class SelfishRobinHood : ColorCrazePlayer
    {
        Random r = new Random();

        private Dictionary<int, int> leaderboard = new Dictionary<int, int>();

        public override void StartAll(List<PlayerInfo> allPlayers)
        {
            
        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {
            leaderboard.Clear();
            int score = 2;
            foreach (KeyValuePair<PlayerInfo, int> item in allPlayersAndScores.OrderBy(key => key.Value))
            {
                if(item.Key.ID != GetInfo().ID)
                {
                    leaderboard.Add(item.Key.ID, score);
                    score += 1;
                }
            }
        }

        public override ColorCrazeDecision PlayTurn(List<ColorCrazePlayerInfo> allPlayers, Board.Board board)
        {
            var gridBoard = (ColorCrazeBoard.ColorCrazeBoard) board;

            var currentLocation = GetInfo().CurrentLocation;

            var possibleDirections = new List<ColorCrazeDirection>();

            var actionsWithScore = new Dictionary<ColorCrazeDirection, int>();

            // A drette
            actionsWithScore.Add(ColorCrazeDirection.Right, GetScoreForDirection(allPlayers, gridBoard, currentLocation.X + 1, currentLocation.Y));

            // A goche
            actionsWithScore.Add(ColorCrazeDirection.Left, GetScoreForDirection(allPlayers, gridBoard, currentLocation.X - 1, currentLocation.Y));

            // en O
            actionsWithScore.Add(ColorCrazeDirection.Up, GetScoreForDirection(allPlayers, gridBoard, currentLocation.X, currentLocation.Y - 1));

            // en Ba
            actionsWithScore.Add(ColorCrazeDirection.Down, GetScoreForDirection(allPlayers, gridBoard, currentLocation.X, currentLocation.Y + 1));

            foreach (KeyValuePair<ColorCrazeDirection, int> item in actionsWithScore.OrderBy(key => key.Value))
            {
                if(item.Value != -1)
                {
                    possibleDirections.Add(item.Key);
                }
            }

            ColorCrazeDecision action;

            if (possibleDirections.Count > 0 && actionsWithScore.TryGetValue(possibleDirections.Last(), out int value))
            {
                if(value != 0)
                {
                    action = new ColorCrazeDecision(possibleDirections.Last());
                }
                else
                {
                    action = getOutOfThereBot(gridBoard, possibleDirections);
                }
            }
            else
            {
                action = new ColorCrazeDecision(new Point(0,0));
            }

            return action;
        }

        private int GetScoreForDirection(List<ColorCrazePlayerInfo> allPlayers, ColorCrazeBoard.ColorCrazeBoard board, int x, int y)
        {
            int score = 0;

            if (x >= 0 && x < board.Width && y >= 0 && y < board.Height)
            {
                ColorCrazeGridSquare square = board[x, y];

                foreach(ColorCrazePlayerInfo player in allPlayers)
                {
                    if(player.CurrentLocation.X == x && player.CurrentLocation.Y == y)
                    {
                        return -1;
                    }
                }

                if (square.Owner == GetInfo().ID)
                {
                    return 0;
                }

                if (square.Color == Color.White)
                {
                    return 1;
                }

                if (leaderboard.TryGetValue(square.Owner, out int value))
                {
                    score += value;
                }

                return score;
            }

             return -1;
        }

        private ColorCrazeDecision getOutOfThereBot(ColorCrazeBoard.ColorCrazeBoard gridBoard, List<ColorCrazeDirection> possDirections)
        {
            int X = GetInfo().CurrentLocation.X;
            int Y = GetInfo().CurrentLocation.Y;
            int height = gridBoard.Height;
            int width = gridBoard.Width;

            if(X < width / 2)
            {
                if (possDirections.Contains(ColorCrazeDirection.Right))
                {
                    return new ColorCrazeDecision(ColorCrazeDirection.Right);
                }
            }

            if (Y < height / 2)
            {
                if (possDirections.Contains(ColorCrazeDirection.Down))
                {
                    return new ColorCrazeDecision(ColorCrazeDirection.Down);
                }
            }

            if (X > width / 2)
            {
                if (possDirections.Contains(ColorCrazeDirection.Left))
                {
                    return new ColorCrazeDecision(ColorCrazeDirection.Left);
                }
            }

            if (Y >  height / 2)
            {
                if (possDirections.Contains(ColorCrazeDirection.Up))
                {
                    return new ColorCrazeDecision(ColorCrazeDirection.Up);
                }
            }

            return new ColorCrazeDecision(new Point(0, 0));
        }
    }
}
