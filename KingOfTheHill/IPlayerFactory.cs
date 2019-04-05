using System.Collections.Generic;
using KingOfTheHill.Players;

namespace KingOfTheHill
{
    public interface IPlayerFactory
    {
        List<string> GetPlayerTypes();
        Player CreatePlayerOfType(int playerType);
    }
}