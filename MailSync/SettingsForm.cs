using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailSync
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            if (Thread.CurrentThread.CurrentCulture.Name == "pl-PL")
            {

                rm = new ResourceManager("MailSync.Properties.Resources_pl_PL", typeof(RibbonOutlook).Assembly);
            }
            else
            {

                rm = new ResourceManager("MailSync.Properties.Resources", typeof(RibbonOutlook).Assembly);
            }
            InitializeComponent();
            //set runtime text
            this.cbLocked.Text = rm.GetString("settingsChkUnblock");
            this.btnOK.Text = rm.GetString("settingsBtnOK");
            this.btnCancel.Text = rm.GetString("settingsBtnCancel");
            this.lblServer.Text = rm.GetString("settingsLblServer");
            this.lblProtocol.Text = rm.GetString("settingsLblProtocol");
            this.lblUsername.Text = rm.GetString("settingsLblUsername");
            this.lblDevice.Text = rm.GetString("settingsLblDevice");
            this.cbExchange.Text = rm.GetString("settingsCbExchange");
            this.lblEmail.Text = rm.GetString("settingsLblEmail");
            

            tbServer.Text = Properties.Settings.Default.EASServer;
            tbProtocolVersion.Text = Properties.Settings.Default.ProtocolVersion;
            tbUsername.Text = Properties.Settings.Default.Username;
            tbDevice.Text = Properties.Settings.Default.DevID;
            tbEmail.Text = Properties.Settings.Default.Email;

            if(cbExchange.Checked)
            {
                tbEmail.Enabled = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.EASServer = tbServer.Text;
            Properties.Settings.Default.ProtocolVersion = tbProtocolVersion.Text;
            Properties.Settings.Default.IsExchange = cbExchange.Checked;
            if (cbExchange.Checked)
            {
                if(tbDevice.Text!="")
                {
                    Properties.Settings.Default.DevID = tbDevice.Text;
                    Properties.Settings.Default.Username = tbUsername.Text;
                    Properties.Settings.Default.Email = tbEmail.Text;
                }
            }
            else
            {
                if (!cbLocked.Checked && tbDevice.Text != "")
                {
                    Properties.Settings.Default.DevID = tbDevice.Text;
                    Properties.Settings.Default.Username = tbUsername.Text;
                }
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

        private void cbExchange_CheckedChanged_1(object sender, EventArgs e)
        {
            if(cbExchange.Checked)
            {
                tbEmail.Enabled = true;
                cbLocked.Checked = true;
                if (cbLocked.Checked)
                {
                    tbDevice.Enabled = false;
                }
                else
                {
                    tbDevice.Enabled = true;
                }
            }
            else
            {
                tbEmail.Enabled = false;
                cbLocked.Checked = false;
                if (cbLocked.Checked)
                {
                    tbDevice.Enabled = false;
                }
                else
                {
                    tbDevice.Enabled = true;
                }

            }
        }
    }
}
