using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingOfTheHill.Players;

namespace KingOfTheHill.Dilemma.DilemmaPlayers.Bots
{
    public class YourBot : DilemmaPlayer
    {
        public override void StartAll(List<DilemmaPlayerInfo> allPlayers)
        {
        }

        public override DilemmaDecision PlayTurn(Dictionary<PlayerInfo, int> allPlayersAndScores, List<DilemmaGame> completeHistory)
        {
            return DilemmaDecision.DefaultDecision;
        }
    }
}
