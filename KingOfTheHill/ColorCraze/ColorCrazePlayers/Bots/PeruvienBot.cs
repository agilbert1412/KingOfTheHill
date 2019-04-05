using System;
using System.Collections.Generic;
using System.Drawing;
using KingOfTheHill.Players;

namespace KingOfTheHill.ColorCraze.ColorCrazePlayers.Bots
{
    public class PeruvienBot : ColorCrazePlayer
    {
        Point position = new Point();
        int boardSize = 0;
        List<PossibleCase> validMove = new List<PossibleCase>();
        Dictionary<int, PlayerScores> idAndPts = new Dictionary<int, PlayerScores>();
        int myId;
        PlayerScores myScores = new PlayerScores();
        Boolean endgame = false;

        public override void StartAll(List<PlayerInfo> allPlayers)
        {
            idAndPts.Clear();
            boardSize = allPlayers.Count;
            myId = GetInfo().ID;
            myScores.game = 0;
            myScores.round = 0;
            foreach (PlayerInfo inf in allPlayers)
            {
                if (inf.ID != myId) {
                    PlayerScores playerScore = new PlayerScores
                    {
                        game = 0,
                        round = 0
                    };
                    idAndPts.Add(inf.ID, playerScore);
                }
            }
        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {
            endgame = false;
            foreach (KeyValuePair<PlayerInfo, int> pair in allPlayersAndScores)
            {
                if (idAndPts.ContainsKey(pair.Key.ID))
                {
                    idAndPts[pair.Key.ID].game = pair.Value;
                    idAndPts[pair.Key.ID].round = 0;
                }
                else if (pair.Key.ID == myId)
                {
                    myScores.game = pair.Value;
                    myScores.round = 0;
                }
            }
        }

        public override ColorCrazeDecision PlayTurn(List<ColorCrazePlayerInfo> allPlayers, Board.Board board)
        {
            Update(allPlayers, board as ColorCrazeBoard.ColorCrazeBoard);

            var action = new ColorCrazeDecision(GetBestMove());
            return action;
        }

        private ColorCrazeDirection GetBestMove()
        {
            PossibleCase best;
            best.Direction = ColorCrazeDirection.Up;
            best.owner = -2;           

            foreach (PossibleCase move in validMove)
            {
                if (best.owner == -2 || best.owner == myId)
                {
                    best.owner = move.owner;
                    best.Direction = move.Direction;
                }
                else if(idAndPts.ContainsKey(move.owner) && idAndPts.ContainsKey(best.owner))
                {
                    if (!endgame)
                    {
                        if (HaveToSwitchByPointsGameRound(best,move))
                        {
                            best.owner = move.owner;
                            best.Direction = move.Direction;
                        }
                        
                    }
                    else
                    {
                        if (HaveToSwitchByEndGame(best, move))
                        {
                            best.owner = move.owner;
                            best.Direction = move.Direction;
                        }
                    }
                }
            }           

            return best.Direction;
        }

        private Boolean HaveToSwitchByPointsGameRound(PossibleCase actual, PossibleCase possible)
        {
            Boolean val =false; 

            if (idAndPts[actual.owner].round < idAndPts[possible.owner].round)
            {
                val = true;
            }
            else if (idAndPts[actual.owner].round == idAndPts[possible.owner].round)
            {
                val = idAndPts[actual.owner].game < idAndPts[possible.owner].game;
            }

            return val;
        }

        private Boolean HaveToSwitchByEndGame(PossibleCase actual, PossibleCase possible)
        {
            Boolean val = false;

            int diff = idAndPts[possible.owner].round - myScores.round;
            int diffBest = idAndPts[actual.owner].round - myScores.round;

            if (diffBest > 0 && diff > 0 && diff < diffBest)
            {
                val = true;
            }
            else if (diffBest < 0 && diff > 0)
            {
                val = true;
            }
            else if (diffBest < 0 && diff < 0 && diff > diffBest)
            {
                val = true;
            }

            return val;
        }

        private void Update(List<ColorCrazePlayerInfo> allPlayers, ColorCrazeBoard.ColorCrazeBoard board)
        {            
            UpdateValidMove(allPlayers, board);
            if (board.CountSquaresOfOwner(-1) < boardSize * boardSize * 0.1)
                endgame = true;
            foreach (KeyValuePair<int, PlayerScores> pair in idAndPts)
            {
                pair.Value.round = board.CountSquaresOfOwner(pair.Key);
            }
        }

        private void UpdateValidMove(List<ColorCrazePlayerInfo> allPlayers, ColorCrazeBoard.ColorCrazeBoard board)
        {
            position = GetInfo().CurrentLocation;

            List<Point> loc = allPlayers.ConvertAll(new Converter<ColorCrazePlayerInfo, Point>(PlayerToPoint));
            validMove.Clear();

            Point pts = new Point(position.X - 1, position.Y);
            if (position.X > 0 && !loc.Contains(pts))
                validMove.Add(GetPossibleCase(pts,board,ColorCrazeDirection.Left));

            pts = new Point(position.X, position.Y - 1);
            if (position.Y > 0 && !loc.Contains(pts))
                validMove.Add(GetPossibleCase(pts, board, ColorCrazeDirection.Up));

            pts = new Point(position.X + 1, position.Y);
            if (position.X < boardSize - 1 && !loc.Contains(pts))
                validMove.Add(GetPossibleCase(pts, board, ColorCrazeDirection.Right));

            pts = new Point(position.X, position.Y + 1);
            if (position.Y < boardSize - 1 && !loc.Contains(pts))
                validMove.Add(GetPossibleCase(pts, board, ColorCrazeDirection.Down));            
        }

        private static PossibleCase GetPossibleCase(Point pts, ColorCrazeBoard.ColorCrazeBoard board, ColorCrazeDirection direction)
        {
            PossibleCase possibleCase = new PossibleCase
            {
                Direction = direction,
                owner = board[pts.X, pts.Y].Owner
            };
            return possibleCase;
        }

        private Point PlayerToPoint(ColorCrazePlayerInfo player)
        {
            return player.CurrentLocation;
        }

        private struct PossibleCase
        {
            public ColorCrazeDirection Direction;
            public int owner;
        }

        private class PlayerScores
        {
            public int game;
            public int round;
        }
    }
    
}
