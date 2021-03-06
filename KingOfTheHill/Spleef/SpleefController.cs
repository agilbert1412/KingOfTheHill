﻿using System;
using KingOfTheHill.Players;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using KingOfTheHill.Spleef.SpleefPlayers;

namespace KingOfTheHill.Spleef
{
    public class SpleefController : IKingOfTheHillController
    {
        private const string LBL_STATUS_BOT_NAME = "lblStatusBot";
        private const string LBL_STATUS_BOT = "lblStatusSquares";
        private const string LBL_SCORE_BOT_NAME = "lblScoresBot";
        private const string LBL_SCORE_BOT = "lblScoresScore";

        private List<SpleefGame> _currentGames;
        private int _currentMaxGames = 1;
        private List<SpleefPlayer> _currentPlayers;
        private Dictionary<PlayerInfo, int> _currentScores;
        private SpleefPlayerFactory _playerFactory;

        private List<ColorCrazePlayerMatchResult> _matchResults;

        private Dictionary<PlayerInfo, int> turnDied;

        public void Initialize()
        {
            _currentGames = new List<SpleefGame>();
            _currentPlayers = new List<SpleefPlayer>();
            _currentScores = new Dictionary<PlayerInfo, int>();
            _playerFactory = new SpleefPlayerFactory();
            turnDied = new Dictionary<PlayerInfo, int>();
        }

        public IPlayerFactory GetPlayerFactory()
        {
            return _playerFactory;
        }

        public void DrawCurrentGame(Graphics gfx, Panel pnlGame)
        {
            if (_currentGames.Any())
            {
                var w = _currentGames.First().Board.Width;
                var h = _currentGames.First().Board.Height;

                var rect = new Rectangle(pnlGame.DisplayRectangle.X, pnlGame.DisplayRectangle.Y, (pnlGame.DisplayRectangle.Width / w) * w, (pnlGame.DisplayRectangle.Height / h) * h);

                _currentGames.First().Paint(gfx, rect);
            }
        }

        public void AddPlayerOfType(int index, Color color)
        {
            var newBot = _playerFactory.CreatePlayerOfType(index) as SpleefPlayer;
            newBot.Info.Name += " " + (_currentPlayers.Count(x => x.GetType() == newBot.GetType()) + 1);
            newBot.Info.ID = _currentPlayers.Count;
            newBot.GetInfo().PlayerColor = color;

            _currentPlayers.Add(newBot);
        }

        public void AddPlayer(Player player, Color color)
        {
            var newBot = player as SpleefPlayer;

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
            _currentGames.Clear();
            for (var i = 0; i < numGamesValue; i++)
            {
                _currentGames.Add(new SpleefGame(_currentPlayers));
            }

            _currentMaxGames = numGamesValue;
            _currentScores = new Dictionary<PlayerInfo, int>();
            var allInfos = _currentPlayers.Select(x => x.Info as SpleefPlayerInfo).ToList();
            _matchResults = PrepareMatchResultList(_currentMaxGames, allInfos);

            ResetTurnsDied();
            foreach (var p in _currentPlayers)
            {
                _currentScores.Add(p.Info, 0);
                p.StartAll(allInfos);
            }
        }

        private void ResetTurnsDied()
        {
            turnDied.Clear();
            foreach (var p in _currentPlayers)
            {
                turnDied.Add(p.GetInfo(), 0);
            }
        }

        private List<ColorCrazePlayerMatchResult> PrepareMatchResultList(int numberOfGames, List<SpleefPlayerInfo> players)
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
                var thisGame = _currentGames.First();

                var playersAliveBefore = thisGame.GetAlivePlayers();
                var nbPlayersAliveBefore = playersAliveBefore.Count;

                var gameIsOver = thisGame.PlayStep(_currentScores, null);

                var playersAliveAfter = thisGame.GetAlivePlayers();
                var nbPlayersAliveAfter = playersAliveAfter.Count;

                var nbPointsEarned = Math.Max(nbPlayersAliveBefore - nbPlayersAliveAfter, 0);

                foreach (var p in playersAliveAfter)
                {
                    _currentScores[p.Info] += nbPointsEarned;
                    turnDied[p.Info]++;
                }


                pnlGame.Refresh();
                Application.DoEvents();

                if (gameIsOver)
                {
                    var winners = thisGame.GetAlivePlayers();
                    if (winners.Count == 1)
                    {
                        var winner = winners.First();
                        _currentScores[winner.Info] += 2;
                    }

                    var scores = thisGame.GetStatus().OrderByDescending(x => x.Value).ToList();
                    
                    foreach (var score in scores)
                    {
                        var playerId = score.Key.ID;
                        var capturedCells = score.Value;
                        var playerResult = _matchResults.First(p => p.Player.ID == playerId);
                        //playerResult.TotalCapturedCells += capturedCells;
                        if (_currentGames.Count > 1)
                        {
                            playerResult.TotalGames++;
                        }
                    }

                    _currentGames.RemoveAt(0);
                    ResetTurnsDied();
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
                var allStatus = _currentGames.First().GetStatus();
                var usefulStatus = allStatus.Where(x => x.Value).ToList();
                var deadPlayers = allStatus.Where(x => !x.Value).OrderByDescending(x => turnDied[x.Key]).ToList();

                foreach (var p in deadPlayers)
                {
                    usefulStatus.Add(p);
                }

                int i = 0;
                for (i = 0; i < usefulStatus.Count(); i++)
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

                    var thisPlayer = _currentPlayers.First(x => x.Info.ID == usefulStatus[i].Key.ID).GetInfo();

                    botLabel.ForeColor = thisPlayer.PlayerColor;
                    scoreLabel.ForeColor = usefulStatus[i].Value ? Color.DarkGreen : Color.DarkRed;

                    botLabel.BackColor = Color.Transparent;
                    scoreLabel.BackColor = Color.Transparent;

                    botLabel.Text = _currentGames.First().GetTime(thisPlayer) + " " + usefulStatus[i].Key.Name;
                    scoreLabel.Text = usefulStatus[i].Value ? "ALIVE" : "DEAD";

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
    }

    public class ColorCrazePlayerMatchResult
    {
        [Browsable(false)]
        public SpleefPlayerInfo Player { get; set; }

        public string PlayerName => Player.Name;
        public int TotalGames { get; set; }
        public double TotalCapturedCells { get; set; }
        public double AverageCapturedCellsPerGame => TotalCapturedCells / TotalGames;
    }
}
