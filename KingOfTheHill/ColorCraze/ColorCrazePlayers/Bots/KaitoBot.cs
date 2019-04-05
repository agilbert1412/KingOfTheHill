using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using KingOfTheHill.Players;

namespace KingOfTheHill.ColorCraze.ColorCrazePlayers.Bots
{
    public class KaitoBot : ColorCrazePlayer
    {
        Random r = new Random();

        private Dictionary<int, int> playerScores;

        public override void StartAll(List<PlayerInfo> allPlayers)
        {

        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {
            playerScores = new Dictionary<int, int>();

            foreach (var infoAndScore in allPlayersAndScores)
            {
                playerScores.Add(infoAndScore.Key.ID, infoAndScore.Value);
            }
        }

        public override ColorCrazeDecision PlayTurn(List<ColorCrazePlayerInfo> allPlayers, Board.Board board)
        {
            var colorBoard = (ColorCrazeBoard.ColorCrazeBoard)board;
            var myInfo = (ColorCrazePlayerInfo)Info;
            var depth = 7;
            var colorNums = new Dictionary<int, int>();

            for (var x = 0; x < colorBoard.Width; x++)
            {
                for (var y = 0; y < colorBoard.Height; y++)
                {
                    var owner = colorBoard[x, y].Owner;
                    if (!colorNums.ContainsKey(owner))
                    {
                        colorNums.Add(owner, 0);
                    }
                    colorNums[owner] += owner > -1 ? playerScores[owner] : 1;
                }
            }

            var walkedSquares = new List<Point>();
            walkedSquares.Add(myInfo.CurrentLocation);

            var Scores = new Dictionary<ColorCrazeDirection, double>() {
                { ColorCrazeDirection.Right, ScoreSquare(colorBoard, allPlayers, colorNums, new Point(myInfo.CurrentLocation.X + 1, myInfo.CurrentLocation.Y), Info.ID, depth, depth, walkedSquares)},
		        { ColorCrazeDirection.Left, ScoreSquare(colorBoard, allPlayers, colorNums, new Point(myInfo.CurrentLocation.X - 1, myInfo.CurrentLocation.Y), Info.ID, depth, depth, walkedSquares)},
		        { ColorCrazeDirection.Down,  ScoreSquare(colorBoard, allPlayers, colorNums, new Point(myInfo.CurrentLocation.X, myInfo. CurrentLocation.Y + 1), Info.ID, depth, depth, walkedSquares)},
		        { ColorCrazeDirection.Up, ScoreSquare(colorBoard, allPlayers, colorNums, new Point(myInfo.CurrentLocation.X, myInfo.CurrentLocation.Y - 1), Info.ID, depth, depth, walkedSquares)}
	        };

            var bestScore = Scores.First();
	
	        foreach (var s in Scores) {
		        if (s.Value > bestScore.Value) {
			        bestScore = s;
		        }
            }

            return new ColorCrazeDecision(bestScore.Key);
        }

        private double ScoreSquare(
            ColorCrazeBoard.ColorCrazeBoard colorBoard,
            List<ColorCrazePlayerInfo> players,
            Dictionary<int, int> colorNums,
            Point square,
            int myID,
            int depth,
            int origDepth,
            List<Point> walkedSquares)
        {

            if (square.X < 0 || square.X >= colorBoard.Width || square.Y < 0 || square.Y >= colorBoard.Height)
            {
                return 0;
            }


            var score = (double)colorNums[colorBoard[square.X,square.Y].Owner];
            if (colorBoard[square.X,square.Y].Owner == myID)
            {
                score = 0;
            }
            else if (colorBoard[square.X, square.Y].Owner == -1)
            {
                score = score / 5;
            }

            if (depth == origDepth)
            {
		        foreach (var p in players)
                {
                    if (p.CurrentLocation.X == square.X && p.CurrentLocation.Y == square.Y)
                    {
                        return 0;
                    }
                }
            }

            var wouldWalkedSquares = walkedSquares.ToList();
            wouldWalkedSquares.Add(square);

            var ratioDeeper = 0.4;
            if (origDepth == depth)
            {
                ratioDeeper = ratioDeeper * 0.5;
            }

            if (depth > 0)
            {
                score += (ScoreSquare(colorBoard, players, colorNums, new Point(square.X + 1, square.Y), myID, depth - 1, origDepth, wouldWalkedSquares) * ratioDeeper);
                score += (ScoreSquare(colorBoard, players, colorNums, new Point(square.X - 1, square.Y), myID, depth - 1, origDepth, wouldWalkedSquares) * ratioDeeper);
                score += (ScoreSquare(colorBoard, players, colorNums, new Point(square.X, square.Y + 1), myID, depth - 1, origDepth, wouldWalkedSquares) * ratioDeeper);
                score += (ScoreSquare(colorBoard, players, colorNums, new Point(square.X, square.Y - 1), myID, depth - 1, origDepth, wouldWalkedSquares) * ratioDeeper);

            }

            if (colorBoard[square.X,square.Y].Owner == myID)
            {
                score = score / 8;
            }

            return score;
        }
    }
}
