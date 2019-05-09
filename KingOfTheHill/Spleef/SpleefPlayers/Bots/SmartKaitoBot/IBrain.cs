using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingOfTheHill.Players;

namespace KingOfTheHill.Spleef.SpleefPlayers.Bots.SmartKaitoBot
{
    public interface IBrain
    {
        void LoadBrain(Dictionary<string, double> values);

        Dictionary<string, double> GetBrain();

        void Mutate(double variance);

        PlayerInfo GetInfo();
    }
}
