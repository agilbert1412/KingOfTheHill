using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingOfTheHill.Players;

namespace KingOfTheHill.Dilemma.DilemmaPlayers.Bots
{
    public class TrollBot : DilemmaPlayer
    {
        public override void StartAll(List<DilemmaPlayerInfo> allPlayers)
        {
        }

        public override DilemmaDecision PlayTurn(DilemmaPlayerInfo currentOpponent, Dictionary<PlayerInfo, int> allPlayersAndScores, List<DilemmaGame> completeHistory)
        {
            if (RandomGen.Next(100) < 50)
            {
                return new DilemmaDecision(DilemmaAction.Cover);
            }
            else
            {
                return new DilemmaDecision(DilemmaAction.Betray);
            }
        }
    }
}
