using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using KingOfTheHill.Dilemma.DilemmaPlayers.Bots;

namespace KingOfTheHill.Dilemma.DilemmaPlayers
{
    public class DilemmaPlayerFactory : IPlayerFactory
    {
        static List<Type> typesOfPlayers = new List<Type>()
        {
            typeof(LilShitBot),
            typeof(CuteBot),
            typeof(TrollBot),
            typeof(YourBot)
        };

        public List<string> GetPlayerTypes()
        {
            return typesOfPlayers.Select(x => x.ToString().Split('.').Last()).ToList();
        }

        public Player CreatePlayerOfType(int playerType)
        {
            DilemmaPlayer player = (DilemmaPlayer)Activator.CreateInstance(typesOfPlayers[playerType]);
            player.Info.Name = player.GetType().ToString().Split('.').Last();
            return player;
        }
    }
}
