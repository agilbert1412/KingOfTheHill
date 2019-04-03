using KingOfTheHill.ColorCraze.ColorCrazePlayers;
using KingOfTheHill.ColorCraze.Players;
using KingOfTheHill.Players;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KingOfTheHill.ColorCraze
{
    public partial class frmColorCraze : Form
    {
        List<ColorCrazeGame> currentGames;

        private int currentMaxGames = 1;

        List<ColorCrazePlayer> currentPlayers;

        Dictionary<PlayerInfo, int> currentScores;
		
		public class PlayerMatchResult
        {
            [Browsable(false)]
            public PlayerInfo Player { get; set; }

            public string PlayerName => Player.Name;
            public int TotalGames { get; set; }
            public double TotalCapturedCells { get; set; }
            public double AverageCapturedCellsPerGame => TotalCapturedCells / TotalGames;
        }
		
        public frmColorCraze()
        {
            InitializeComponent();

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            pnlGame, new object[] { true });
        }

        private void frmColorCraze_Load(object sender, EventArgs e)
        {
            currentGames = new List<ColorCrazeGame>();
            currentPlayers = new List<ColorCrazePlayer>();
            currentScores = new Dictionary<PlayerInfo, int>();

            var allTypes = ColorCrazePlayerFactory.GetPlayerTypes();

            listBotsAvailable.Items.Clear();
            listBotsInGame.Items.Clear();
            for(var i = 0; i < allTypes.Count; i++)
            {
                listBotsAvailable.Items.Add(allTypes[i]);
            }

            btnAddBot.Enabled = false;
            btnRemoveBot.Enabled = false;
            btnStartGames.Enabled = false;
            btnStop.Enabled = false;
            btnPause.Enabled = false;

            timerStep.Interval = (int)(2000 / Math.Pow(2, trackBar1.Value - 1));
        }

        private void pnlGame_Paint(object sender, PaintEventArgs e)
        {
            if (currentGames.Any()/* && chkRefreshDisplay.Checked*/)
            {
                var gfx = e.Graphics;

                var w = currentGames.First().Board.Width;
                var h = currentGames.First().Board.Height;

                var rect = new Rectangle(pnlGame.DisplayRectangle.X, pnlGame.DisplayRectangle.Y, (pnlGame.DisplayRectangle.Width / w) * w, (pnlGame.DisplayRectangle.Height / h) * h);

                currentGames.First().Paint(gfx, rect);
            }
        }

        private void listBotsAvailable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBotsAvailable.SelectedIndex > -1)
            {
                btnAddBot.Enabled = true;
            }
        }

        private void listBotsInGame_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBotsInGame.SelectedIndex > -1)
            {
                btnRemoveBot.Enabled = true;
            }
        }

        private void AddBot(int index)
        {
            var newBot = ColorCrazePlayerFactory.CreatePlayerOfType(index);
            newBot.Info.Name = listBotsAvailable.Items[index] + " " + (currentPlayers.Count(x => x.GetType() == newBot.GetType()) + 1);
            newBot.Info.ID = currentPlayers.Count;
            newBot.GetInfo().PlayerColor = AllColors[currentPlayers.Count % AllColors.Count];

            currentPlayers.Add(newBot);
            UpdateBotsInGame();
        }

        private void UpdateBotsInGame()
        {
            listBotsInGame.Items.Clear();
            for (var i = 0; i < currentPlayers.Count; i++)
            {
                listBotsInGame.Items.Add(currentPlayers[i].Info.Name);
            }
            if (listBotsInGame.SelectedIndex > -1)
            {
                btnRemoveBot.Enabled = true;
            }
            btnStartGames.Enabled = listBotsInGame.Items.Count > 1;
        }

        private void btnAddBot_Click(object sender, EventArgs e)
        {
            AddBot(listBotsAvailable.SelectedIndex);
        }

        private void btnAddAllBots_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBotsAvailable.Items.Count; i++ )
            {
                AddBot(i);
            }
        }

        private void btnRemoveBot_Click(object sender, EventArgs e)
        {
            var idx = listBotsInGame.SelectedIndex;
            currentPlayers.RemoveAt(idx);
            UpdateBotsInGame();
            listBotsInGame.SelectedIndex = idx < listBotsInGame.Items.Count ? idx : idx - 1;
            if (listBotsInGame.SelectedIndex < 0)
            {
                btnRemoveBot.Enabled = false;
            }
        }

        private void btnRemoveAllBots_Click(object sender, EventArgs e)
        {
            currentPlayers.Clear();
            UpdateBotsInGame();
            listBotsInGame.SelectedIndex = -1;
            btnRemoveBot.Enabled = false;
        }

        private void btnStartGames_Click(object sender, EventArgs e)
        {
            currentGames.Clear();
            for (var i = 0; i < numGames.Value; i++)
            {
                currentGames.Add(new ColorCrazeGame(currentPlayers));
            }

            currentMaxGames = (int)numGames.Value;
            currentScores = new Dictionary<PlayerInfo, int>();
            var allInfos = currentPlayers.Select(x => x.Info).ToList();
            _matchResults = PrepareMatchResultList(currentMaxGames, allInfos);

            foreach (var p in currentPlayers)
            {
                currentScores.Add(p.Info, 0);
                p.StartAll(allInfos);
            }

            btnStartGames.Enabled = false;

            pnlGame.Refresh();
            btnStartGames.Enabled = false;
            btnStop.Enabled = true;
            btnPause.Enabled = true;
            btnRemoveAllBots.Enabled = false;
            btnAddAllBots.Enabled = false;

            timerStep.Start();
        }

        private List<PlayerMatchResult> _matchResults;

        

        private List<PlayerMatchResult> PrepareMatchResultList(int numberOfGames, List<PlayerInfo> players)
        {
            var temp = new List<PlayerMatchResult>();

            foreach (var player in players)
            {
                temp.Add(new PlayerMatchResult
                {
                    TotalCapturedCells = 0,
                    TotalGames = numberOfGames,
                    Player = player
                });
            }

            return temp;
        }

        private void timerStep_Tick(object sender, EventArgs e)
        {
            if (currentGames.Any())
            {
               
                var gameIsOver = currentGames.First().PlayStep(currentScores);
                pnlGame.Refresh();
                Application.DoEvents();
                lblGameNum.Text = (currentMaxGames - currentGames.Count + 1) + " / " + currentMaxGames;

                ShowScoresLabels();
                ShowStatusLabels();

                if (gameIsOver)
                {
                    var scores = currentGames.First().GetStatus().OrderByDescending(x => x.Value).ToList();



                    foreach (var score in scores)
                    {
                        var playerId = score.Key.ID;
                        var capturedCells = score.Value;
                        var playerResult = _matchResults.First(p => p.Player.ID == playerId);
                        playerResult.TotalCapturedCells += capturedCells;
                        if (currentGames.Count>1)
                        {
                            playerResult.TotalGames++;
                        }
                    }








                    var scoreAtThisPosition = scores.Count() - 1;
                    for(var i = 0; i < scores.Count(); i++)
                    {
                        currentScores[scores[i].Key] += scoreAtThisPosition;

                        while (i + 1 < scores.Count && scores[i + 1].Value == scores[i].Value)
                        {
                            i++;
                            currentScores[scores[i].Key] += scoreAtThisPosition;
                        }
                        scoreAtThisPosition = scores.Count - 2 - i;
                    }
                    ShowScoresLabels();

                    currentGames.RemoveAt(0);
                }

                if (currentGames.Any())
                {
                    pnlGame.Refresh();
                }
                else
                {
                    timerStep.Stop();
                    btnStartGames.Enabled = true;
                    btnStop.Enabled = false;
                    btnPause.Enabled = false;
                    btnRemoveAllBots.Enabled = true;
                    btnAddAllBots.Enabled = true;
                }

            }
        }

        private void ShowStatusLabels()
        {
            if (currentGames.Any())
            {
                var orderedScores = currentGames.First().GetStatus().OrderByDescending(x => x.Value).ToList();
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

                    var thisPlayer = currentPlayers.First(x => x.Info.ID == orderedScores[i].Key.ID).GetInfo();

                    botLabel.ForeColor = thisPlayer.PlayerColor;
                    scoreLabel.ForeColor = thisPlayer.PlayerColor;

                    botLabel.BackColor = Color.Transparent;
                    scoreLabel.BackColor = Color.Transparent;

                    botLabel.Text = currentGames.First().GetTime(thisPlayer) + " " + orderedScores[i].Key.Name;
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

        private void ShowScoresLabels()
        {
            var orderedScores = currentScores.OrderByDescending(x => x.Value).ToList();
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

                botLabel.ForeColor = currentPlayers.First(x => x.Info.ID == orderedScores[i].Key.ID).GetInfo().PlayerColor;
                scoreLabel.ForeColor = currentPlayers.First(x => x.Info.ID == orderedScores[i].Key.ID).GetInfo().PlayerColor;

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

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (!chkRefreshDisplay.Checked)
                timerStep.Interval = (int)(1000 / Math.Pow(2, trackBar1.Value - 1));
            else
                timerStep.Interval = 1;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (btnPause.Text == "Pause")
            {
                timerStep.Stop();
                btnPause.Text = "Resume";
                chkRefreshDisplay.Checked = false;
            }
            else
            {
                timerStep.Start();
                btnPause.Text = "Pause";
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timerStep.Stop();
            btnStartGames.Enabled = true;
            btnStop.Enabled = false;
            btnPause.Enabled = false;
            chkRefreshDisplay.Checked = false;
        }

        private void chkRefreshDisplay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRefreshDisplay.Checked)
            {
                timerStep.Stop();
                trackBar1.Enabled = false;
                while (currentGames.Any() && chkRefreshDisplay.Checked)
                {
                    Application.DoEvents();
                    timerStep_Tick(null, null);
                }
                trackBar1.Enabled = true;
                if (btnPause.Text == "Pause")
                {
                    timerStep.Start();
                }
            }
        }

        List<Color> AllColors = new List<Color>()
        {
            Color.DarkRed,
            Color.DarkBlue,
            Color.Teal,
            Color.Purple,
            Color.YellowGreen,
            Color.Orange,
            Color.Green,
            Color.Pink,
            Color.Gray,
            Color.LightBlue,
            Color.DarkGreen,
            Color.Brown,

            Color.Beige,
            Color.Chocolate,
            Color.Crimson,
            Color.DeepPink,
            Color.Firebrick,
            Color.FloralWhite,
            Color.ForestGreen,
            Color.Fuchsia,
            Color.Gold,
            Color.GreenYellow,
            Color.Honeydew,
            Color.HotPink,
            Color.IndianRed,
            Color.Indigo,
            Color.Ivory,
            Color.Khaki,
            Color.Lavender,
            Color.LawnGreen,
            Color.Lime,
            Color.Magenta,
            Color.Maroon,
            Color.MintCream,
            Color.Navy,
            Color.Olive,
            Color.Orchid,
            Color.PeachPuff,
            Color.Salmon,
            Color.Snow,
            Color.Tan,
            Color.Tomato,
            Color.Turquoise,
            Color.Wheat,
            Color.Cyan,
            Color.Violet
        };

        private void ShowPlayerStatisticsButton_Click(object sender, EventArgs e)
        {
            var dialog = new Form();
            var dg = new DataGrid();
            var temp = _matchResults.OrderBy(mr=>mr.AverageCapturedCellsPerGame);
            dg.DataSource = temp.ToList();
            dialog.Controls.Add(dg);
            dg.Dock = DockStyle.Fill;
            dialog.ShowDialog(this);
            
            
        }
    }
}
