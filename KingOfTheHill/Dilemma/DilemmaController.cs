using System;
using KingOfTheHill.Players;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using KingOfTheHill.Dilemma.DilemmaPlayers;

namespace KingOfTheHill.Dilemma
{
    public class DilemmaController : IKingOfTheHillController
    {
        private const string LBL_STATUS_BOT_NAME = "lblStatusBot";
        private const string LBL_STATUS_BOT = "lblStatusSquares";
        private const string LBL_SCORE_BOT_NAME = "lblScoresBot";
        private const string LBL_SCORE_BOT = "lblScoresScore";

        private List<DilemmaGame> _currentGames;
        private int _currentMaxGames = 1;
        private List<DilemmaPlayer> _currentPlayers;
        private Dictionary<PlayerInfo, int> _currentScores;
        private DilemmaPlayerFactory _playerFactory;

        private int currentGameEnumerator;
        private Random rand = new Random();

        public void Initialize()
        {
            _currentGames = new List<DilemmaGame>();
            _currentPlayers = new List<DilemmaPlayer>();
            _currentScores = new Dictionary<PlayerInfo, int>();
            _playerFactory = new DilemmaPlayerFactory();
        }

        public IPlayerFactory GetPlayerFactory()
        {
            return _playerFactory;
        }

        public void DrawCurrentGame(Graphics gfx, Panel pnlGame)
        {
            if (currentGameEnumerator < _currentGames.Count - 1)
            {
                var heightRectangle = pnlGame.DisplayRectangle.Height / 3;
                var rectTop = new Rectangle(pnlGame.DisplayRectangle.X, pnlGame.DisplayRectangle.Y, pnlGame.DisplayRectangle.Width, heightRectangle);
                var rectMid = new Rectangle(pnlGame.DisplayRectangle.X, pnlGame.DisplayRectangle.Y + heightRectangle, pnlGame.DisplayRectangle.Width, heightRectangle);
                var rectBottom = new Rectangle(pnlGame.DisplayRectangle.X, pnlGame.DisplayRectangle.Y + (heightRectangle * 2), pnlGame.DisplayRectangle.Width, heightRectangle);

                if (currentGameEnumerator > 1)
                {
                    _currentGames[currentGameEnumerator - 2].Paint(gfx, rectTop);
                }

                if (currentGameEnumerator > 0)
                {
                    _currentGames[currentGameEnumerator - 1].Paint(gfx, rectMid);
                }

                if (currentGameEnumerator < _currentGames.Count)
                {
                    _currentGames[currentGameEnumerator].Paint(gfx, rectBottom);
                }
            }
        }

        public void AddPlayerOfType(int index, Color color)
        {
            var newBot = _playerFactory.CreatePlayerOfType(index) as DilemmaPlayer;
            newBot.Info.Name += " " + (_currentPlayers.Count(x => x.GetType() == newBot.GetType()) + 1);
            newBot.Info.ID = _currentPlayers.Count;
            newBot.GetInfo().PlayerColor = color;

            _currentPlayers.Add(newBot);
        }

        public void AddPlayer(Player player, Color color)
        {
            var newBot = player as DilemmaPlayer;

            var nameParts = newBot.Info.Name.Split(' ');

            if (nameParts.Length < 2)
            {
                newBot.Info.Name += " " + (_currentPlayers.Count(x => x.GetType() == newBot.GetType()) + 1);
            }

            newBot.Info.ID = _currentPlayers.Count;
            newBot.GetInfo().PlayerColor = color;

            _currentPlayers.Add(newBot);
        }

        public List<Player> GetCurrentPlayers()
        {
            return _currentPlayers.Select(x => x as Player).ToList();
        }

        public void RemoveBotAt(int idx)
        {
            _currentPlayers.RemoveAt(idx);
        }

        public void RemoveAllBots()
        {
            _currentPlayers.Clear();
        }

        public void StartGames(int numGamesValue)
        {
            var gamePlayers = new List<DilemmaPlayer[]>();

            for (var i = 0; i < _currentPlayers.Count; i++)
            {
                for (var j = i + 1; j < _currentPlayers.Count; j++)
                {
                    for (var k = 0; k < numGamesValue; k++)
                    {
                        gamePlayers.Add(new DilemmaPlayer[]
                        {
                            _currentPlayers[i],
                            _currentPlayers[j]
                        });
                    }
                }
            }

            gamePlayers = Shuffle(gamePlayers);

            _currentGames.Clear();
            for (var i = 0; i < gamePlayers.Count; i++)
            {
                _currentGames.Add(new DilemmaGame(gamePlayers[i]));
            }

            _currentMaxGames = _currentGames.Count;
            _currentScores = new Dictionary<PlayerInfo, int>();
            var allInfos = _currentPlayers.Select(x => x.Info as DilemmaPlayerInfo).ToList();
            
            foreach (var p in _currentPlayers)
            {
                _currentScores.Add(p.Info, 0);
                p.StartAll(allInfos);
            }

            currentGameEnumerator = 0;
        }

        public void DoGameStep(Panel pnlGame)
        {
            if (currentGameEnumerator < _currentGames.Count)
            {
                var thisGame = _currentGames[currentGameEnumerator];

                thisGame.PlayStep(_currentScores, _currentGames.Take(currentGameEnumerator).Select(x => x.Clone()).ToList());

                currentGameEnumerator++;

                var players = thisGame.PlayersDecisions.Select(x => x.Key).ToArray();
                var decisions = thisGame.PlayersDecisions.Select(x => x.Value).ToArray();

                var action0 = decisions[0].Action;
                var action1 = decisions[1].Action;

                var pointChanges = DilemmaDecision.GetPointsChanges(action0, action1);

                _currentScores[players[0]] += pointChanges[0];
                _currentScores[players[1]] += pointChanges[1];

                pnlGame.Refresh();
                Application.DoEvents();

                pnlGame.Refresh();
            }
        }

        public string GetCurrentGameText()
        {
            return (currentGameEnumerator + 1) + " / " + _currentMaxGames;
        }

        public bool IsCurrentlyRunning()
        {
            return _currentGames.Any() && currentGameEnumerator < _currentGames.Count;
        }

        public IEnumerable<object> GetMatchStatistics()
        {
            return new List<object>();
        }

        public void ShowStatusLabels(GroupBox groupStatus)
        {
            var allStatus = _currentPlayers.Select(x => x.GetInfo() as DilemmaPlayerInfo).ToList();

            int i = 0;
            for (i = 0; i < allStatus.Count(); i++)
            {
                var labelBotName = LBL_STATUS_BOT_NAME + i;
                var labelStatusName = LBL_STATUS_BOT + i;
                if (!groupStatus.Controls.ContainsKey(labelBotName))
                {
                    var lblBot = new Label();
                    lblBot.Name = labelBotName;
                    groupStatus.Controls.Add(lblBot);

                    var lblScore = new Label();
                    lblScore.Name = labelStatusName;
                    groupStatus.Controls.Add(lblScore);
                }

                var botLabel = (Label)groupStatus.Controls[labelBotName];
                var scoreLabel = (Label)groupStatus.Controls[labelStatusName];

                botLabel.Location = new Point(8, 40 + (13 * i));
                scoreLabel.Location = new Point(94, 40 + (13 * i));

                scoreLabel.AutoSize = false;
                scoreLabel.TextAlign = ContentAlignment.MiddleRight;
                scoreLabel.Size = new Size(40, 13);
                botLabel.Size = new Size(80, 13);

                var thisPlayer = _currentPlayers.First(x => x.Info.ID == allStatus[i].ID).GetInfo();

                botLabel.ForeColor = thisPlayer.PlayerColor;
                scoreLabel.ForeColor = Color.Black;

                botLabel.BackColor = Color.Transparent;
                scoreLabel.BackColor = Color.Transparent;

                botLabel.Text =  allStatus[i].Name;
                scoreLabel.Text = _currentGames.First().GetTime(thisPlayer).ToString();

                botLabel.Visible = true;
                scoreLabel.Visible = true;
            }
            while (groupStatus.Controls.ContainsKey(LBL_STATUS_BOT_NAME + i))
            {
                groupStatus.Controls[LBL_STATUS_BOT_NAME + i].Visible = false;
                groupStatus.Controls[LBL_STATUS_BOT + i].Visible = false;
                i++;
            }
        }

        public Dictionary<PlayerInfo, int> GetScores()
        {
            return _currentScores.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        public void ShowScoresLabels(GroupBox groupScores)
        {
            var orderedScores = _currentScores.OrderByDescending(x => x.Value).ToList();
            int i = 0;
            for (i = 0; i < orderedScores.Count(); i++)
            {
                var labelBotName = LBL_SCORE_BOT_NAME + i;
                var labelScoreName = LBL_SCORE_BOT + i;
                if (!groupScores.Controls.ContainsKey(labelBotName))
                {
                    var lblBot = new Label();
                    lblBot.Name = labelBotName;
                    groupScores.Controls.Add(lblBot);

                    var lblScore = new Label();
                    lblScore.Name = labelScoreName;
                    groupScores.Controls.Add(lblScore);
                }

                var botLabel = (Label)groupScores.Controls[labelBotName];
                var scoreLabel = (Label)groupScores.Controls[labelScoreName];

                botLabel.Location = new Point(8, 40 + (13 * i));
                scoreLabel.Location = new Point(94, 40 + (13 * i));

                scoreLabel.AutoSize = false;
                scoreLabel.TextAlign = ContentAlignment.MiddleRight;
                scoreLabel.Size = new Size(40, 13);
                botLabel.Size = new Size(80, 13);

                botLabel.ForeColor = _currentPlayers.First(x => x.Info.ID == orderedScores[i].Key.ID).GetInfo().PlayerColor;
                scoreLabel.ForeColor = _currentPlayers.First(x => x.Info.ID == orderedScores[i].Key.ID).GetInfo().PlayerColor;

                botLabel.Text = orderedScores[i].Key.Name;
                scoreLabel.Text = orderedScores[i].Value.ToString();

                botLabel.Visible = true;
                scoreLabel.Visible = true;
            }
            while (groupScores.Controls.ContainsKey(LBL_SCORE_BOT_NAME + i))
            {
                groupScores.Controls[LBL_SCORE_BOT_NAME + i].Visible = false;
                groupScores.Controls[LBL_SCORE_BOT+ i].Visible = false;
                i++;
            }
        }

        private List<T> Shuffle<T>(List<T> list)
        {
            var newLst = new List<T>();
            var oldLst = new List<T>(list);

            while (oldLst.Any())
            {
                var chosen = rand.Next(0, oldLst.Count);
                newLst.Add(oldLst[chosen]);
                oldLst.RemoveAt(chosen);
            }

            return newLst;
        }
    }
}
