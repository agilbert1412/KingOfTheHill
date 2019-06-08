using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingOfTheHill.Players;

namespace KingOfTheHill.Dilemma.DilemmaPlayers.Bots
{
    public class CuteBot : DilemmaPlayer
    {
        public override void StartAll(List<DilemmaPlayerInfo> allPlayers)
        {
        }

        public override DilemmaDecision PlayTurn(DilemmaPlayerInfo currentOpponent, Dictionary<PlayerInfo, int> allPlayersAndScores, List<DilemmaGame> completeHistory)
        {
            return new DilemmaDecision(DilemmaAction.Cover);
        }
    }
}
