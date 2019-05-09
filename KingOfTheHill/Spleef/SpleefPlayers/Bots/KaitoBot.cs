using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using KingOfTheHill.Players;
using KingOfTheHill.Spleef.SpleefBoard;
using KingOfTheHill.Spleef.SpleefPlayers.Bots.SmartKaitoBot;

namespace KingOfTheHill.Spleef.SpleefPlayers
{
    public class KaitoBot : SpleefPlayer
    {
        public Dictionary<SpleefPlayerInfo, int> _currentScores;
        
        private int timeSpentHere = 0;
        private int MaxDepth = 3;

        public override void StartAll(List<SpleefPlayerInfo> allPlayers)
        {

        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {
            _currentScores = new Dictionary<SpleefPlayerInfo, int>();

            foreach (var pAndS in allPlayersAndScores)
            {
                _currentScores.Add((SpleefPlayerInfo)pAndS.Key, pAndS.Value);
            }

            timeSpentHere = 0;
        }

        public override SpleefDecision PlayTurn(List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard board)
        {
            var myLocation = GetMyLocation();
            var enemies = GetAlivePlayers(allPlayers, board).Where(x => x.ID != Info.ID).ToList();

            var sharingArea = true;

            if (enemies.Count < 4)
            {
                sharingArea = false;
                foreach (var e in enemies)
                {
                    var path = FindShortestPathAStar(board, myLocation, e.CurrentLocation);
                    if (path != null)
                    {
                        sharingArea = true;
                    }
                }
            }

            var myPossibilities = GenerateDecisions(enemies, board, myLocation.X, myLocation.Y, timeSpentHere);

            if (myPossibilities.Count == 0)
            {
                return SpleefDecision.DefaultDecision;
            }

            var bestDecisions = new List<SpleefDecision>();
            var bestScore = double.MinValue;

            if (sharingArea)
            {
                foreach (var decision in myPossibilities)
                {
                    var score = EvaluateDecision(decision, enemies, board, myLocation.X, myLocation.Y, timeSpentHere,
                        MaxDepth);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestDecisions.Clear();
                        bestDecisions.Add(decision);
                    }
                    else if (score == bestScore)
                    {
                        bestDecisions.Add(decision);
                    }
                }
            }
            else
            {
                if (myPossibilities.Any(x => x.Action == SpleefAction.Wait))
                {
                    bestDecisions.Add(SpleefDecision.DefaultDecision);
                }
                else
                {
                    foreach (var decision in myPossibilities)
                    {
                        var numNextTurn = GetNumMovesNextTurn(decision, board, myLocation.X, myLocation.Y);

                        var score = 0;
                        if (numNextTurn > 0)
                        {
                            score = 8 - numNextTurn;
                        }

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestDecisions.Clear();
                            bestDecisions.Add(decision);
                        }
                        else if (score == bestScore)
                        {
                            bestDecisions.Add(decision);
                        }
                    }
                }
            }

            var choice = bestDecisions[RandomGen.Next(0, bestDecisions.Count)];

            if (choice.Action == SpleefAction.Move)
            {
                timeSpentHere = 0;
            }
            else
            {
                timeSpentHere++;
            }
            

            return choice;
        }

        private int GetNumMovesNextTurn(SpleefDecision decision, SpleefBoard.SpleefBoard board, int myLocX, int myLocY)
        {
            if (decision.Action != SpleefAction.Move)
                return 0;


            var newBoard = board.Clone();
            newBoard[myLocX, myLocY].Destroy();
            var destX = myLocX + decision.Target.X;
            var destY = myLocY + decision.Target.Y;

            var num = 0;

            for (var i = -1; i < 2; i++)
            {
                for (var j = -1; j < 2; j++)
                {
                    var destXAfter = destX + i;
                    var destYAfter = destY + j;
                    if ((i != 0 || j != 0) && destXAfter >= 0 && destYAfter >= 0 && destXAfter < board.Width && destYAfter < board.Height)
                    {
                        if (board[destX, destY].IsSolid)
                        {
                            num++;
                        }
                    }
                }
            }

            return num;
        }

        private double EvaluateDecision(SpleefDecision decision, List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard board, int myLocX, int myLocY, int turnsSpenthere, int depth)
        {
            if (depth <= 0)
            {
                return 1;
            }

            double score = 1;

            var newLocX = myLocX + decision.Target.X;
            var newLocY = myLocY + decision.Target.Y;

            var newBoard = board.Clone();
            var newTurnsSpent = turnsSpenthere + 1;

            if (decision.Action == SpleefAction.Move)
            {
                newTurnsSpent = 0;
                newBoard[myLocX, myLocY].Destroy();
            }

            var decisionsFromThere = GenerateDecisions(allPlayers, newBoard, newLocX, newLocY, newTurnsSpent);

            foreach (var deci in decisionsFromThere)
            {
                score += EvaluateDecision(deci, allPlayers, board, newLocX, newLocY, newTurnsSpent, depth - 1) / 5;
            }

            return score;
        }

        private List<SpleefDecision> GenerateDecisions(List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard board, int myLocX, int myLocY, int turnsSpentHere)
        {
            var decisions = new List<SpleefDecision>();

            var closestPlayerDistance = int.MaxValue;

            foreach (var p in allPlayers)
            {
                var dist = GetDistance(p.CurrentLocation.X, p.CurrentLocation.Y, myLocX, myLocY);
                if (dist < closestPlayerDistance)
                {
                    closestPlayerDistance = dist;
                }
            }

            if (turnsSpentHere < 3 && closestPlayerDistance > 2)
            {
                decisions.Add(new SpleefDecision(SpleefAction.Wait, Point.Empty));
            }

            for (var i = -1; i < 2; i++)
            {
                for (var j = -1; j < 2; j++)
                {
                    var destX = myLocX + i;
                    var destY = myLocY + j;
                    if ((i != 0 || j != 0) && destX >= 0 && destY >= 0 && destX < board.Width && destY < board.Height)
                    {
                        if (board[destX, destY].IsSolid && allPlayers.All(p =>
                                p.CurrentLocation.X != destX || p.CurrentLocation.Y != destY))
                        {
                            decisions.Add(new SpleefDecision(SpleefAction.Move, new Point(i, j)));
                        }
                    }
                }
            }

            return decisions;
        }
    }
}
