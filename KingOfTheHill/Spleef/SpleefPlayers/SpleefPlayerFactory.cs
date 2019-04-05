using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using KingOfTheHill.Spleef.SpleefPlayers.Bots;

namespace KingOfTheHill.Spleef.SpleefPlayers
{
    public class SpleefPlayerFactory : IPlayerFactory
    {
        static List<Type> typesOfPlayers = new List<Type>()
        {
            typeof(StraightBot),
            typeof(BullyBot),
            typeof(TrollBot),
            typeof(YourBot)
        };

        public List<string> GetPlayerTypes()
        {
            return typesOfPlayers.Select(x => x.ToString().Split('.').Last()).ToList();
        }

        public Player CreatePlayerOfType(int playerType)
        {
            SpleefPlayer player = (SpleefPlayer)Activator.CreateInstance(typesOfPlayers[playerType]);
            player.Info.Name = player.GetType().ToString().Split('.').Last();
            return player;
        }
    }
}
