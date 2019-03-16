namespace KingOfTheHill
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
            // frmGameSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 311);
            this.Controls.Add(this.btnColorCraze);
            this.Name = "frmGameSelector";
            this.Text = "King of The Hill Game Selector";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnColorCraze;
    }
}

