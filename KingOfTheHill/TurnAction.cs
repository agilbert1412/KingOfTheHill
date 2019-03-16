using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill
{
    public class TurnAction
    {
        public PlayerInfo PlayerTaking { get; set; }
        public Decision ActionTaken { get; set; }

        public TurnAction(PlayerInfo player, Decision action)
        {
            PlayerTaking = player;
            ActionTaken = action;
        }
    }
}
