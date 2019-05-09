using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using KingOfTheHill.Players;


namespace KingOfTheHill.Spleef.SpleefPlayers.Bots
{
    class PacifistBot : SpleefPlayer
    {
        private const int PRECISION = 15;
        //TODO Code something to be careful during the "20" first turn
        private int turnNbr = 0;
        private bool isAlone = false;

        private List<Point> possibleMove = new List<Point>
        {
            new Point(1,0),
            new Point(0,1),
            new Point(-1,0),
            new Point(0,-1),
            new Point(1,1),
            new Point(-1,1),
            new Point(-1,-1),
            new Point(1,-1),
        };
        private List<Point> otherPlayersPosition;
        private HashSet<Point> pointWePassed = new HashSet<Point>();
        private Dictionary<Point, int> allMoveValue;


        public override SpleefDecision PlayTurn(List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard board)
        {
            Point currentPosition = this.GetMyLocation();
            allMoveValue = new Dictionary<Point, int>();
            List<Point> usedPosition = new List<Point>();
            otherPlayersPosition = new List<Point>();

            foreach(SpleefPlayerInfo player in allPlayers)
            {
                if(player != this.Info)
                {
                    usedPosition.Add(player.CurrentLocation);
                    otherPlayersPosition.Add(player.CurrentLocation);
                }

            }

            foreach (Point move in possibleMove)
            {
                SpleefBoard.SpleefBoard boardClone = board.Clone(); 
                Point newPosition = new Point(currentPosition.X + move.X, currentPosition.Y + move.Y);
                if (newPosition.X >= 0 && newPosition.Y >= 0 && newPosition.X < board.Width && newPosition.Y < board.Height && this.IsAlive(newPosition, board))
                {
                    allMoveValue.Add(move, seeTheFuture(1, newPosition, allPlayers, boardClone, usedPosition));
                }
                else
                {
                    allMoveValue.Add(move, 0);
                }
            }

            Point bestMove = new Point(0, 0);
            int bestValue = 0;
            foreach(var move in allMoveValue)
            {
                if(move.Value > bestValue)
                {
                    bestMove = move.Key;
                    bestValue = move.Value;
                }
            }


            if (bestValue == 0)
            {
                //If we reach this point, we cry and stop moving hoping the other die before us 
                return new SpleefDecision(SpleefAction.Wait, new Point());
            }
            else
            {
                // if true, it mean that there is not a lot of point left
                if(bestValue != PRECISION)
                {
                    pointWePassed = new HashSet<Point>();
                    
                    if(board[currentPosition.X, currentPosition.Y].HealthRemaining > 1 &&(isAlone || seeIfAlone(bestMove, allPlayers, board.Clone(), usedPosition)))
                    {
                        isAlone = true;
                        return new SpleefDecision(SpleefAction.Wait, new Point());
                    }
                }

                //We simply play our best move
                return new SpleefDecision(SpleefAction.Move, bestMove);
            }

        }

        public override void StartAll(List<SpleefPlayerInfo> allPlayers)
        {
            
        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {
            turnNbr = 0;
            isAlone = false;
        }

        private bool seeIfAlone(Point currentPosition, List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard board, List<Point> alreadyUsedPoint)
        {
            foreach(Point move in possibleMove)
            {
                Point newPosition = new Point(currentPosition.X + move.X, currentPosition.Y + move.Y);
                if (newPosition.X >= 0 && newPosition.Y >= 0 && newPosition.X < board.Width && newPosition.Y < board.Height && this.IsAlive(newPosition, board) && !pointWePassed.Contains(newPosition))
                {
                    pointWePassed.Add(newPosition);
                    if(otherPlayersPosition.Contains(newPosition))
                    {
                        return false;
                    }
                    bool alone = seeIfAlone(newPosition, allPlayers, board, alreadyUsedPoint);
                    if(!alone)
                    {
                        return alone;
                    }
                    pointWePassed.Remove(alreadyUsedPoint[alreadyUsedPoint.Count-1]);
                }
            }
            return true;
        }

        private int seeTheFuture(int numberOfTime, Point currentPosition, List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard board, List<Point> alreadyUsedPoint)
        {
            int bestValue = numberOfTime;
            if(bestValue < PRECISION)
            {
                foreach (Point move in possibleMove)
                {
                    Point newPosition = new Point(currentPosition.X + move.X, currentPosition.Y + move.Y);
                    if (newPosition.X >= 0 && newPosition.Y >= 0 && newPosition.X < board.Width && newPosition.Y < board.Height && this.IsAlive(newPosition, board) && !alreadyUsedPoint.Contains(newPosition))
                    {
                        alreadyUsedPoint.Add(newPosition);
                        int value = seeTheFuture(numberOfTime + 1, newPosition, allPlayers, board, alreadyUsedPoint);
                        if (value > bestValue)
                        {
                            bestValue = value;
                        }
                        if(bestValue == PRECISION)
                        {
                            return bestValue;
                        }
                        alreadyUsedPoint.Remove(alreadyUsedPoint[alreadyUsedPoint.Count-1]);
                    }
                }
            }
            return bestValue;
        }
    }
}
