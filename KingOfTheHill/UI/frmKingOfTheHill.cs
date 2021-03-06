﻿using KingOfTheHill.ColorCraze.ColorCrazePlayers;
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

namespace KingOfTheHill.UI
{
    public partial class frmKingOfTheHill : Form
    {
        private IKingOfTheHillController _controller;

        private bool timerShouldRun = false;
		
        public frmKingOfTheHill(IKingOfTheHillController gameController)
        {
            InitializeComponent();

            _controller = gameController;

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            pnlGame, new object[] { true });
        }

        private void frmColorCraze_Load(object sender, EventArgs e)
        {
            _controller.Initialize();

            var allTypes = _controller.GetPlayerFactory().GetPlayerTypes();

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
            _controller.DrawCurrentGame(e.Graphics, pnlGame);
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
            _controller.AddPlayerOfType(index, AllColors[_controller.GetCurrentPlayers().Count % AllColors.Count]);

            UpdateBotsInGame();
        }

        private void UpdateBotsInGame()
        {
            listBotsInGame.Items.Clear();
            var currentPlayers = _controller.GetCurrentPlayers();
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
            _controller.RemoveBotAt(idx);

            UpdateBotsInGame();
            listBotsInGame.SelectedIndex = idx < listBotsInGame.Items.Count ? idx : idx - 1;
            if (listBotsInGame.SelectedIndex < 0)
            {
                btnRemoveBot.Enabled = false;
            }
        }

        private void btnRemoveAllBots_Click(object sender, EventArgs e)
        {
            _controller.RemoveAllBots();

            UpdateBotsInGame();
            listBotsInGame.SelectedIndex = -1;
            btnRemoveBot.Enabled = false;
        }

        private void btnStartGames_Click(object sender, EventArgs e)
        {
            _controller.StartGames((int)numGames.Value);

            btnStartGames.Enabled = false;

            pnlGame.Refresh();
            btnStartGames.Enabled = false;
            btnStop.Enabled = true;
            btnPause.Enabled = true;
            btnRemoveAllBots.Enabled = false;
            btnAddAllBots.Enabled = false;

            timerShouldRun = true;
            timerStep.Start();
        }

        private void timerStep_Tick(object sender, EventArgs e)
        {
            timerStep.Stop();
            _controller.DoGameStep(pnlGame);

            _controller.ShowScoresLabels(groupScores);
            _controller.ShowStatusLabels(groupStatus);

            lblGameNum.Text = _controller.GetCurrentGameText();

            if (_controller.IsCurrentlyRunning())
            {
                pnlGame.Refresh();
                if (timerShouldRun)
                {
                    timerStep.Start();
                }
            }
            else
            {
                timerShouldRun = false;
                timerStep.Stop();
                btnStartGames.Enabled = true;
                btnStop.Enabled = false;
                btnPause.Enabled = false;
                btnRemoveAllBots.Enabled = true;
                btnAddAllBots.Enabled = true;
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
                timerShouldRun = false;
                timerStep.Stop();
                btnPause.Text = "Resume";
                chkRefreshDisplay.Checked = false;
            }
            else
            {
                timerShouldRun = true;
                timerStep.Start();
                btnPause.Text = "Pause";
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timerShouldRun = false;
            timerStep.Stop();
            btnStartGames.Enabled = true;
            btnStop.Enabled = false;
            btnPause.Enabled = false;
            chkRefreshDisplay.Checked = false;
            btnRemoveAllBots.Enabled = true;
            btnAddAllBots.Enabled = true;
        }

        private void chkRefreshDisplay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRefreshDisplay.Checked)
            {
                timerShouldRun = false;
                timerStep.Stop();
                trackBar1.Enabled = false;
                while (_controller.IsCurrentlyRunning() && chkRefreshDisplay.Checked)
                {
                    Application.DoEvents();
                    timerStep_Tick(null, null);
                }
                trackBar1.Enabled = true;
                if (btnPause.Text == "Pause")
                {
                    timerShouldRun = true;
                    timerStep.Start();
                }
            }
        }

        List<Color> AllColors = new List<Color>()
        {
            Color.Red,
            Color.SteelBlue,
            Color.Teal,
            Color.MediumPurple,
            Color.YellowGreen,
            Color.Orange,
            Color.LimeGreen,
            Color.DarkGreen,
            Color.Brown,

            Color.Chocolate,
            Color.Crimson,
            Color.DeepPink,
            Color.Firebrick,
            Color.ForestGreen,
            Color.Fuchsia,
            Color.Gold,
            Color.GreenYellow,
            Color.HotPink,
            Color.IndianRed,
            Color.Indigo,
            Color.Lavender,
            Color.LawnGreen,
            Color.Lime,
            Color.Magenta,
            Color.Maroon,
            Color.Navy,
            Color.Olive,
            Color.Orchid,
            Color.Salmon,
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
            var temp = _controller.GetMatchStatistics();
            dg.DataSource = temp.ToList();
            dialog.Controls.Add(dg);
            dg.Dock = DockStyle.Fill;
            dialog.ShowDialog(this);
        }
    }
}
