using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingOfTheHill.ColorCraze.ColorCrazePlayers.Bots;

namespace KingOfTheHill.ColorCraze.ColorCrazePlayers
{
    public class ColorCrazePlayerFactory : IPlayerFactory
    {
        static List<Type> typesOfPlayers = new List<Type>()
        {
            typeof(AANBot),
            typeof(GS_Bot),
            typeof(JealousBot),
            typeof(KaitoBot),
            typeof(PeruvienBot),
            typeof(SelfishRobinHood),
            typeof(TrollBot),
            typeof(FillerBot),
            typeof(StraightBot),
            typeof(YourBot)
        };

        public List<string> GetPlayerTypes()
        {
            return typesOfPlayers.Select(x => x.ToString().Split('.').Last()).ToList();
        }

        public Player CreatePlayerOfType(int playerType)
        {
            ColorCrazePlayer player = (ColorCrazePlayer)Activator.CreateInstance(typesOfPlayers[playerType]);
            player.Info.Name = player.GetType().ToString().Split('.').Last();
            return player;
        }
    }
}
