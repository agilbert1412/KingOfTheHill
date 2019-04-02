using KingOfTheHill.ColorCraze.Players;
using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill.ColorCraze.ColorCrazePlayers
{
    public static class ColorCrazePlayerFactory
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

        public static List<string> GetPlayerTypes()
        {
            return typesOfPlayers.Select(x => x.ToString().Split('.').Last()).ToList();
        }

        public static ColorCrazePlayer CreatePlayerOfType(int playerType)
        {
            ColorCrazePlayer player = (ColorCrazePlayer)Activator.CreateInstance(typesOfPlayers[playerType]);
            return player;
        }
    }
}
