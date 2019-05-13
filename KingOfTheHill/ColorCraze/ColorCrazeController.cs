using KingOfTheHill.Players;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using KingOfTheHill.ColorCraze.ColorCrazePlayers;

namespace KingOfTheHill.ColorCraze
{
    public class ColorCrazeController : IKingOfTheHillController
    {
        private List<ColorCrazeGame> _currentGames;
        private int _currentMaxGames = 1;
        private List<ColorCrazePlayer> _currentPlayers;
        private Dictionary<PlayerInfo, int> _currentScores;
        private ColorCrazePlayerFactory _playerFactory;

        private List<ColorCrazePlayerMatchResult> _matchResults;

        public void Initialize()
        {
            _currentGames = new List<ColorCrazeGame>();
            _currentPlayers = new List<ColorCrazePlayer>();
            _currentScores = new Dictionary<PlayerInfo, int>();
            _playerFactory = new ColorCrazePlayerFactory();
        }

        public IPlayerFactory GetPlayerFactory()
        {
            return _playerFactory;
        }

        public void DrawCurrentGame(Graphics gfx, Panel pnlGame)
        {
            if (_currentGames.Any()/* && chkRefreshDisplay.Checked*/)
            {
                var w = _currentGames.First().Board.Width;
                var h = _currentGames.First().Board.Height;

                var rect = new Rectangle(pnlGame.DisplayRectangle.X, pnlGame.DisplayRectangle.Y, (pnlGame.DisplayRectangle.Width / w) * w, (pnlGame.DisplayRectangle.Height / h) * h);

                _currentGames.First().Paint(gfx, rect);
            }
        }

        public void AddPlayerOfType(int index, Color color)
        {
            var newBot = _playerFactory.CreatePlayerOfType(index) as ColorCrazePlayer;
            newBot.Info.Name += " " + (_currentPlayers.Count(x => x.GetType() == newBot.GetType()) + 1);
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
            _currentGames.Clear();
            for (var i = 0; i < numGamesValue; i++)
            {
                _currentGames.Add(new ColorCrazeGame(_currentPlayers));
            }

            _currentMaxGames = numGamesValue;
            _currentScores = new Dictionary<PlayerInfo, int>();
            var allInfos = _currentPlayers.Select(x => x.Info).ToList();
            var allInfosAsCC = _currentPlayers.Select(x => x.Info as ColorCrazePlayerInfo).ToList();
            _matchResults = PrepareMatchResultList(_currentMaxGames, allInfosAsCC);

            foreach (var p in _currentPlayers)
            {
                _currentScores.Add(p.Info, 0);
                p.StartAll(allInfos);
            }
        }

        private List<ColorCrazePlayerMatchResult> PrepareMatchResultList(int numberOfGames, List<ColorCrazePlayerInfo> players)
        {
            var temp = new List<ColorCrazePlayerMatchResult>();

            foreach (var player in players)
            {
                temp.Add(new ColorCrazePlayerMatchResult
                {
                    TotalCapturedCells = 0,
                    TotalGames = numberOfGames,
                    Player = player
                });
            }

            return temp;
        }

        public void DoGameStep(Panel pnlGame)
        {
            if (_currentGames.Any())
            {
                var gameIsOver = _currentGames.First().PlayStep(_currentScores, null);
                pnlGame.Refresh();
                Application.DoEvents();

                if (gameIsOver)
                {
                    var scores = _currentGames.First().GetStatus().OrderByDescending(x => x.Value).ToList();
                    
                    foreach (var score in scores)
                    {
                        var playerId = score.Key.ID;
                        var capturedCells = score.Value;
                        var playerResult = _matchResults.First(p => p.Player.ID == playerId);
                        playerResult.TotalCapturedCells += capturedCells;
                        if (_currentGames.Count > 1)
                        {
                            playerResult.TotalGames++;
                        }
                    }
                    
                    var scoreAtThisPosition = scores.Count() - 1;
                    for (var i = 0; i < scores.Count(); i++)
                    {
                        var pInfo = scores[i].Key as ColorCrazePlayerInfo;

                        _currentScores[pInfo] += scoreAtThisPosition;

                        while (i + 1 < scores.Count && scores[i + 1].Value == scores[i].Value)
                        {
                            i++;
                            _currentScores[pInfo] += scoreAtThisPosition;
                        }
                        scoreAtThisPosition = scores.Count - 2 - i;
                    }

                    _currentGames.RemoveAt(0);
                }

                if (_currentGames.Any())
                {
                    pnlGame.Refresh();
                }
            }
        }

        public string GetCurrentGameText()
        {
            return (_currentMaxGames - _currentGames.Count + 1) + " / " + _currentMaxGames;
        }

        public bool IsCurrentlyRunning()
        {
            return _currentGames.Any();
        }

        public IEnumerable<object> GetMatchStatistics()
        {
            return _matchResults.OrderBy(mr => mr.AverageCapturedCellsPerGame).Cast<object>();
        }

        public void ShowStatusLabels(GroupBox groupStatus)
        {
            if (_currentGames.Any())
            {
                var orderedScores = _currentGames.First().GetStatus().OrderByDescending(x => x.Value).ToList();
                int i = 0;
                for (i = 0; i < orderedScores.Count(); i++)
                {
                    var labelBotName = "lblStatusBot" + i;
                    var labelStatusName = "lblStatusSquares" + i;
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

                    var thisPlayer = _currentPlayers.First(x => x.Info.ID == orderedScores[i].Key.ID).GetInfo();

                    botLabel.ForeColor = thisPlayer.PlayerColor;
                    scoreLabel.ForeColor = thisPlayer.PlayerColor;

                    botLabel.BackColor = Color.Transparent;
                    scoreLabel.BackColor = Color.Transparent;

                    botLabel.Text = _currentGames.First().GetTime(thisPlayer) + " " + orderedScores[i].Key.Name;
                    scoreLabel.Text = orderedScores[i].Value.ToString();

                    botLabel.Visible = true;
                    scoreLabel.Visible = true;
                }
                while (groupStatus.Controls.ContainsKey("lblStatusBot" + i))
                {
                    groupStatus.Controls["lblStatusBot" + i].Visible = false;
                    groupStatus.Controls["lblStatusSquares" + i].Visible = false;
                    i++;
                }
            }
        }

        public void ShowScoresLabels(GroupBox groupScores)
        {
            var orderedScores = _currentScores.OrderByDescending(x => x.Value).ToList();
            int i = 0;
            for (i = 0; i < orderedScores.Count(); i++)
            {
                var labelBotName = "lblScoresBot" + i;
                var labelScoreName = "lblScoresScore" + i;
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
            while (groupScores.Controls.ContainsKey("lblScoresBot" + i))
            {
                groupScores.Controls["lblScoresBot" + i].Visible = false;
                groupScores.Controls["lblScoresScore" + i].Visible = false;
                i++;
            }
        }
    }

    public class ColorCrazePlayerMatchResult
    {
        [Browsable(false)]
        public ColorCrazePlayerInfo Player { get; set; }

        public string PlayerName => Player.Name;
        public int TotalGames { get; set; }
        public double TotalCapturedCells { get; set; }
        public double AverageCapturedCellsPerGame => TotalCapturedCells / TotalGames;
    }
}
