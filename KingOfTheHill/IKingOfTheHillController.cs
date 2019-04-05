using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KingOfTheHill.ColorCraze.ColorCrazePlayers;
using KingOfTheHill.Players;

namespace KingOfTheHill
{
    public interface IKingOfTheHillController
    {
        void Initialize();
        IPlayerFactory GetPlayerFactory();
        void DrawCurrentGame(Graphics gfx, Panel pnlGame);
        void AddPlayerOfType(int index, Color color);
        List<Player> GetCurrentPlayers();
        void RemoveBotAt(int idx);
        void RemoveAllBots();
        void StartGames(int numGamesValue);
        void DoGameStep(Panel pnlGame);
        string GetCurrentGameText();
        bool IsCurrentlyRunning();
        IEnumerable<object> GetMatchStatistics();
        void ShowScoresLabels(GroupBox groupStatus);
        void ShowStatusLabels(GroupBox groupScores);
    }
}
