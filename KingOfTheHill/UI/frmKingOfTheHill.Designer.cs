namespace KingOfTheHill.UI
{
    partial class frmKingOfTheHill
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlGame = new System.Windows.Forms.Panel();
            this.listBotsAvailable = new System.Windows.Forms.ListBox();
            this.listBotsInGame = new System.Windows.Forms.ListBox();
            this.btnAddBot = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRemoveBot = new System.Windows.Forms.Button();
            this.btnAddAllBots = new System.Windows.Forms.Button();
            this.btnRemoveAllBots = new System.Windows.Forms.Button();
            this.numGames = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStartGames = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.chkRefreshDisplay = new System.Windows.Forms.CheckBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.groupScores = new System.Windows.Forms.GroupBox();
            this.lblScoresScore = new System.Windows.Forms.Label();
            this.lblScoresBot = new System.Windows.Forms.Label();
            this.groupStatus = new System.Windows.Forms.GroupBox();
            this.lblStatusSquares = new System.Windows.Forms.Label();
            this.lblStatusBot = new System.Windows.Forms.Label();
            this.timerStep = new System.Windows.Forms.Timer(this.components);
            this.lblGameNum = new System.Windows.Forms.Label();
            this.ShowPlayerStatisticsButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numGames)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.groupScores.SuspendLayout();
            this.groupStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlGame
            // 
            this.pnlGame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGame.Location = new System.Drawing.Point(12, 12);
            this.pnlGame.Name = "pnlGame";
            this.pnlGame.Size = new System.Drawing.Size(550, 550);
            this.pnlGame.TabIndex = 0;
            this.pnlGame.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlGame_Paint);
            // 
            // listBotsAvailable
            // 
            this.listBotsAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listBotsAvailable.FormattingEnabled = true;
            this.listBotsAvailable.Location = new System.Drawing.Point(568, 28);
            this.listBotsAvailable.Name = "listBotsAvailable";
            this.listBotsAvailable.Size = new System.Drawing.Size(144, 251);
            this.listBotsAvailable.TabIndex = 1;
            this.listBotsAvailable.SelectedIndexChanged += new System.EventHandler(this.listBotsAvailable_SelectedIndexChanged);
            // 
            // listBotsInGame
            // 
            this.listBotsInGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listBotsInGame.FormattingEnabled = true;
            this.listBotsInGame.Location = new System.Drawing.Point(826, 28);
            this.listBotsInGame.Name = "listBotsInGame";
            this.listBotsInGame.Size = new System.Drawing.Size(144, 251);
            this.listBotsInGame.TabIndex = 2;
            this.listBotsInGame.SelectedIndexChanged += new System.EventHandler(this.listBotsInGame_SelectedIndexChanged);
            // 
            // btnAddBot
            // 
            this.btnAddBot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddBot.Location = new System.Drawing.Point(718, 28);
            this.btnAddBot.Name = "btnAddBot";
            this.btnAddBot.Size = new System.Drawing.Size(100, 30);
            this.btnAddBot.TabIndex = 3;
            this.btnAddBot.Text = "Add";
            this.btnAddBot.UseVisualStyleBackColor = true;
            this.btnAddBot.Click += new System.EventHandler(this.btnAddBot_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(565, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Bots Available";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(823, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Bots In the game";
            // 
            // btnRemoveBot
            // 
            this.btnRemoveBot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveBot.Location = new System.Drawing.Point(718, 100);
            this.btnRemoveBot.Name = "btnRemoveBot";
            this.btnRemoveBot.Size = new System.Drawing.Size(100, 30);
            this.btnRemoveBot.TabIndex = 6;
            this.btnRemoveBot.Text = "Remove";
            this.btnRemoveBot.UseVisualStyleBackColor = true;
            this.btnRemoveBot.Click += new System.EventHandler(this.btnRemoveBot_Click);
            // 
            // btnAddAllBots
            // 
            this.btnAddAllBots.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddAllBots.Location = new System.Drawing.Point(718, 64);
            this.btnAddAllBots.Name = "btnAddAllBots";
            this.btnAddAllBots.Size = new System.Drawing.Size(100, 30);
            this.btnAddAllBots.TabIndex = 7;
            this.btnAddAllBots.Text = "AddAll";
            this.btnAddAllBots.UseVisualStyleBackColor = true;
            this.btnAddAllBots.Click += new System.EventHandler(this.btnAddAllBots_Click);
            // 
            // btnRemoveAllBots
            // 
            this.btnRemoveAllBots.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveAllBots.Location = new System.Drawing.Point(718, 136);
            this.btnRemoveAllBots.Name = "btnRemoveAllBots";
            this.btnRemoveAllBots.Size = new System.Drawing.Size(100, 30);
            this.btnRemoveAllBots.TabIndex = 8;
            this.btnRemoveAllBots.Text = "Remove All";
            this.btnRemoveAllBots.UseVisualStyleBackColor = true;
            this.btnRemoveAllBots.Click += new System.EventHandler(this.btnRemoveAllBots_Click);
            // 
            // numGames
            // 
            this.numGames.Location = new System.Drawing.Point(136, 19);
            this.numGames.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numGames.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGames.Name = "numGames";
            this.numGames.Size = new System.Drawing.Size(68, 20);
            this.numGames.TabIndex = 9;
            this.numGames.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnStartGames);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numGames);
            this.groupBox1.Location = new System.Drawing.Point(753, 285);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(217, 93);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Start Games";
            // 
            // btnStartGames
            // 
            this.btnStartGames.Location = new System.Drawing.Point(104, 45);
            this.btnStartGames.Name = "btnStartGames";
            this.btnStartGames.Size = new System.Drawing.Size(100, 30);
            this.btnStartGames.TabIndex = 13;
            this.btnStartGames.Text = "Start Games";
            this.btnStartGames.UseVisualStyleBackColor = true;
            this.btnStartGames.Click += new System.EventHandler(this.btnStartGames_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Number of Games:";
            // 
            // chkRefreshDisplay
            // 
            this.chkRefreshDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRefreshDisplay.AutoSize = true;
            this.chkRefreshDisplay.Location = new System.Drawing.Point(568, 285);
            this.chkRefreshDisplay.Name = "chkRefreshDisplay";
            this.chkRefreshDisplay.Size = new System.Drawing.Size(161, 17);
            this.chkRefreshDisplay.TabIndex = 11;
            this.chkRefreshDisplay.Text = "Gotta Go Fast (Experimental)";
            this.chkRefreshDisplay.UseVisualStyleBackColor = true;
            this.chkRefreshDisplay.CheckedChanged += new System.EventHandler(this.chkRefreshDisplay_CheckedChanged);
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.Location = new System.Drawing.Point(604, 308);
            this.trackBar1.Maximum = 8;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(108, 45);
            this.trackBar1.TabIndex = 12;
            this.trackBar1.Value = 8;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(568, 311);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Slow";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(718, 311);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Fast";
            // 
            // btnPause
            // 
            this.btnPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPause.Location = new System.Drawing.Point(571, 348);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(100, 30);
            this.btnPause.TabIndex = 15;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Location = new System.Drawing.Point(571, 384);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 30);
            this.btnStop.TabIndex = 16;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // groupScores
            // 
            this.groupScores.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupScores.Controls.Add(this.lblScoresScore);
            this.groupScores.Controls.Add(this.lblScoresBot);
            this.groupScores.Location = new System.Drawing.Point(830, 384);
            this.groupScores.Name = "groupScores";
            this.groupScores.Size = new System.Drawing.Size(140, 179);
            this.groupScores.TabIndex = 17;
            this.groupScores.TabStop = false;
            this.groupScores.Text = "Scores";
            // 
            // lblScoresScore
            // 
            this.lblScoresScore.AutoSize = true;
            this.lblScoresScore.Location = new System.Drawing.Point(99, 16);
            this.lblScoresScore.Name = "lblScoresScore";
            this.lblScoresScore.Size = new System.Drawing.Size(35, 13);
            this.lblScoresScore.TabIndex = 1;
            this.lblScoresScore.Text = "Score";
            // 
            // lblScoresBot
            // 
            this.lblScoresBot.AutoSize = true;
            this.lblScoresBot.Location = new System.Drawing.Point(8, 17);
            this.lblScoresBot.Name = "lblScoresBot";
            this.lblScoresBot.Size = new System.Drawing.Size(23, 13);
            this.lblScoresBot.TabIndex = 0;
            this.lblScoresBot.Text = "Bot";
            // 
            // groupStatus
            // 
            this.groupStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupStatus.Controls.Add(this.lblStatusSquares);
            this.groupStatus.Controls.Add(this.lblStatusBot);
            this.groupStatus.Location = new System.Drawing.Point(677, 384);
            this.groupStatus.Name = "groupStatus";
            this.groupStatus.Size = new System.Drawing.Size(140, 179);
            this.groupStatus.TabIndex = 18;
            this.groupStatus.TabStop = false;
            this.groupStatus.Text = "Status";
            // 
            // lblStatusSquares
            // 
            this.lblStatusSquares.AutoSize = true;
            this.lblStatusSquares.Location = new System.Drawing.Point(88, 17);
            this.lblStatusSquares.Name = "lblStatusSquares";
            this.lblStatusSquares.Size = new System.Drawing.Size(46, 13);
            this.lblStatusSquares.TabIndex = 3;
            this.lblStatusSquares.Text = "Squares";
            // 
            // lblStatusBot
            // 
            this.lblStatusBot.AutoSize = true;
            this.lblStatusBot.Location = new System.Drawing.Point(6, 17);
            this.lblStatusBot.Name = "lblStatusBot";
            this.lblStatusBot.Size = new System.Drawing.Size(23, 13);
            this.lblStatusBot.TabIndex = 2;
            this.lblStatusBot.Text = "Bot";
            // 
            // timerStep
            // 
            this.timerStep.Interval = 2000;
            this.timerStep.Tick += new System.EventHandler(this.timerStep_Tick);
            // 
            // lblGameNum
            // 
            this.lblGameNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGameNum.Location = new System.Drawing.Point(677, 348);
            this.lblGameNum.Name = "lblGameNum";
            this.lblGameNum.Size = new System.Drawing.Size(70, 30);
            this.lblGameNum.TabIndex = 14;
            // 
            // ShowPlayerStatisticsButton
            // 
            this.ShowPlayerStatisticsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowPlayerStatisticsButton.Location = new System.Drawing.Point(568, 532);
            this.ShowPlayerStatisticsButton.Name = "ShowPlayerStatisticsButton";
            this.ShowPlayerStatisticsButton.Size = new System.Drawing.Size(100, 30);
            this.ShowPlayerStatisticsButton.TabIndex = 19;
            this.ShowPlayerStatisticsButton.Text = "ShowPlayerStatistics";
            this.ShowPlayerStatisticsButton.UseVisualStyleBackColor = true;
            this.ShowPlayerStatisticsButton.Click += new System.EventHandler(this.ShowPlayerStatisticsButton_Click);
            // 
            // frmColorCraze
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 575);
            this.Controls.Add(this.ShowPlayerStatisticsButton);
            this.Controls.Add(this.lblGameNum);
            this.Controls.Add(this.groupStatus);
            this.Controls.Add(this.groupScores);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.chkRefreshDisplay);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRemoveAllBots);
            this.Controls.Add(this.btnAddAllBots);
            this.Controls.Add(this.btnRemoveBot);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAddBot);
            this.Controls.Add(this.listBotsInGame);
            this.Controls.Add(this.listBotsAvailable);
            this.Controls.Add(this.pnlGame);
            this.DoubleBuffered = true;
            this.Name = "frmColorCraze";
            this.Text = "frmColorCraze";
            this.Load += new System.EventHandler(this.frmColorCraze_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numGames)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.groupScores.ResumeLayout(false);
            this.groupScores.PerformLayout();
            this.groupStatus.ResumeLayout(false);
            this.groupStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlGame;
        private System.Windows.Forms.ListBox listBotsAvailable;
        private System.Windows.Forms.ListBox listBotsInGame;
        private System.Windows.Forms.Button btnAddBot;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRemoveBot;
        private System.Windows.Forms.Button btnAddAllBots;
        private System.Windows.Forms.Button btnRemoveAllBots;
        private System.Windows.Forms.NumericUpDown numGames;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStartGames;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkRefreshDisplay;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.GroupBox groupScores;
        private System.Windows.Forms.GroupBox groupStatus;
        private System.Windows.Forms.Timer timerStep;
        private System.Windows.Forms.Label lblScoresScore;
        private System.Windows.Forms.Label lblScoresBot;
        private System.Windows.Forms.Label lblStatusSquares;
        private System.Windows.Forms.Label lblStatusBot;
        private System.Windows.Forms.Label lblGameNum;
        private System.Windows.Forms.Button ShowPlayerStatisticsButton;
    }
}