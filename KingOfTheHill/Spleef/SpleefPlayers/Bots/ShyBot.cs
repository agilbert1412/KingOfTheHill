using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingOfTheHill.Players;

namespace KingOfTheHill.Spleef.SpleefPlayers.Bots
{
    public class ShyBot : SpleefPlayer
    {
        public override void StartAll(List<SpleefPlayerInfo> allPlayers)
        {

        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {

        }

        public override SpleefDecision PlayTurn(List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard board)
        {
            var highestTotalOpponentDistance = double.MinValue;
            var bestSquare = Point.Empty;

            var myLocation = GetMyLocation();
            var alivePlayers = GetAlivePlayers(allPlayers, board).Where(x => x.ID != Info.ID).ToList();

            for (var i = -1; i < 2; i++)
            {
                for (var j = -1; j < 2; j++)
                {
                    var sqrX = myLocation.X + i;
                    var sqrY = myLocation.Y + j;
                    if (sqrX >= 0 && sqrY >= 0 && sqrX < board.Width && sqrY < board.Height && board[sqrX, sqrY].IsSolid && (i != 0 || j != 0))
                    {
                        var targetSquare = new Point(sqrX, sqrY);
                        var totOpponentDistance = 0.0;
                        foreach (var p in alivePlayers)
                        {
                            var path = FindShortestPathAStar(board, targetSquare, p.CurrentLocation);
                            if (path != null && path.Count > 1)
                            {
                                totOpponentDistance += Math.Sqrt(path.Count);
                            }
                        }

                        if (totOpponentDistance > highestTotalOpponentDistance)
                        {
                            highestTotalOpponentDistance = totOpponentDistance;
                            bestSquare = new Point(i, j);
                        }
                    }
                }
            }

            return new SpleefDecision(SpleefAction.Move, bestSquare);
        }
    }
}
