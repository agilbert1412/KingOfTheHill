using System;
using System.Windows.Forms;
using KingOfTheHill.ColorCraze;
using KingOfTheHill.Spleef;
using KingOfTheHill.Spleef.SpleefPlayers.Bots.SmartKaitoBot;

namespace KingOfTheHill.UI
{
    public partial class frmGameSelector : Form
    {
        public frmGameSelector()
        {
            InitializeComponent();
        }

        private void btnColorCraze_Click(object sender, EventArgs e)
        {
            var frmColor = new frmKingOfTheHill(new ColorCrazeController());
            this.Hide();
            frmColor.ShowDialog();
            this.Close();
        }

        private void btnSpleef_Click(object sender, EventArgs e)
        {
            var frmSpleef = new frmKingOfTheHill(new SpleefController());
            this.Hide();
            frmSpleef.ShowDialog();
            this.Close();
        }
    }
}
