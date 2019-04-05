using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingOfTheHill.Players;

namespace KingOfTheHill.Spleef.SpleefPlayers.Bots
{
    public class YourBot : SpleefPlayer
    {
        public override void StartAll(List<SpleefPlayerInfo> allPlayers)
        {

        }

        public override void StartGame(Dictionary<PlayerInfo, int> allPlayersAndScores)
        {

        }

        public override SpleefDecision PlayTurn(List<SpleefPlayerInfo> allPlayers, SpleefBoard.SpleefBoard spleefBoard)
        {
            return SpleefDecision.DefaultDecision;
        }
    }
}
