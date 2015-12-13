using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailSync
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            tbServer.Text = Properties.Settings.Default.EASServer;
            tbProtocolVersion.Text = Properties.Settings.Default.ProtocolVersion;
            tbUsername.Text = Properties.Settings.Default.Username;
            tbDevice.Text = Properties.Settings.Default.DevID;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.EASServer = tbServer.Text;
            Properties.Settings.Default.ProtocolVersion = tbProtocolVersion.Text;
            if(!cbLocked.Checked && tbDevice.Text!="")
            {
                Properties.Settings.Default.DevID = tbDevice.Text;
                Properties.Settings.Default.Username = tbUsername.Text;
            }
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbLocked_CheckedChanged(object sender, EventArgs e)
        {
            if (tbDevice.Enabled)
            {
                tbDevice.Enabled = false;
                tbUsername.Enabled = false;
            }
            else
            {
                tbDevice.Enabled = true;
                tbUsername.Enabled = true;
            }
        }
    }
}
