namespace KingOfTheHill.UI
{
    partial class frmGameSelector
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
            this.btnColorCraze = new System.Windows.Forms.Button();
            this.btnSpleef = new System.Windows.Forms.Button();
            this.btnDilemma = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnColorCraze
            // 
            this.btnColorCraze.Location = new System.Drawing.Point(52, 38);
            this.btnColorCraze.Name = "btnColorCraze";
            this.btnColorCraze.Size = new System.Drawing.Size(100, 50);
            this.btnColorCraze.TabIndex = 0;
            this.btnColorCraze.Text = "Color Craze";
            this.btnColorCraze.UseVisualStyleBackColor = true;
            this.btnColorCraze.Click += new System.EventHandler(this.btnColorCraze_Click);
            // 
            // btnSpleef
            // 
            this.btnSpleef.Location = new System.Drawing.Point(185, 38);
            this.btnSpleef.Name = "btnSpleef";
            this.btnSpleef.Size = new System.Drawing.Size(100, 50);
            this.btnSpleef.TabIndex = 1;
            this.btnSpleef.Text = "Spleef";
            this.btnSpleef.UseVisualStyleBackColor = true;
            this.btnSpleef.Click += new System.EventHandler(this.btnSpleef_Click);
            // 
            // btnDilemma
            // 
            this.btnDilemma.Location = new System.Drawing.Point(52, 116);
            this.btnDilemma.Name = "btnDilemma";
            this.btnDilemma.Size = new System.Drawing.Size(100, 50);
            this.btnDilemma.TabIndex = 2;
            this.btnDilemma.Text = "Prisoner\'s Dilemma";
            this.btnDilemma.UseVisualStyleBackColor = true;
            this.btnDilemma.Click += new System.EventHandler(this.btnDilemma_Click);
            // 
            // frmGameSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 311);
            this.Controls.Add(this.btnDilemma);
            this.Controls.Add(this.btnSpleef);
            this.Controls.Add(this.btnColorCraze);
            this.Name = "frmGameSelector";
            this.Text = "King of The Hill Game Selector";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnColorCraze;
        private System.Windows.Forms.Button btnSpleef;
        private System.Windows.Forms.Button btnDilemma;
    }
}

