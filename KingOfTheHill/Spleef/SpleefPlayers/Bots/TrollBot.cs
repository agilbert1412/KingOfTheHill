using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingOfTheHill.Players;

namespace KingOfTheHill.Spleef.SpleefPlayers.Bots
{
    public class TrollBot : SpleefPlayer
    {
        public override void StartAll(List<SpleefPlayerInfo> allPlayers)
        {

        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {

        }

        public override SpleefDecision PlayTurn(List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard spleefBoard)
        {
            var possibilities = new List<SpleefDecision>();

            possibilities.Add(SpleefDecision.DefaultDecision);

            var myLocation = ((SpleefPlayerInfo) GetInfo()).CurrentLocation;

            for (int i = -2; i < 3; i++)
            {
                for (int j = -2; j < 3; j++)
                {
                    if (j != 0 || i != 0)
                    {
                        var moveAction = new SpleefDecision(SpleefAction.Move, new Point(i, j));
                        var holeAction = new SpleefDecision(SpleefAction.Move, new Point(i, j));

                        if (moveAction.IsValid)
                        {
                            var dest = new Point(myLocation.X + moveAction.Target.X, myLocation.Y + moveAction.Target.Y);

                            if (dest.X >= 0 && dest.X < spleefBoard.Width && dest.Y >= 0 && dest.Y < spleefBoard.Height)
                            {
                                if (spleefBoard[dest.X, dest.Y].IsSolid)
                                {
                                    possibilities.Add(moveAction);
                                }
                            }
                        }
                        if (holeAction.IsValid)
                        {
                            possibilities.Add(holeAction);
                        }
                    }
                }
            }

            return possibilities[RandomGen.Next(0, possibilities.Count)];
        }
    }
}
