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
    public partial class FolderDecision : Form
    {
        public List<string> lstKatalogi;
        public string SelectedFolder;
        public string SelectedTime;

        public FolderDecision()
        {
            InitializeComponent();
        }

        public FolderDecision(List<string> lstOutlookFolders,bool isOnlineSyncOption)
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

            lstKatalogi = new List<string>(lstOutlookFolders);

            foreach (string s in lstKatalogi)
            {
                lstView.Items.Add(new ListViewItem(s));
            }
            if(isOnlineSyncOption)
            {
                cmbPickDate.Visible = true;
                lblCombo.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection sel = lstView.SelectedItems;
            ListViewItem lvi = sel[0];
            SelectedFolder = lvi.Text;

            this.Close();

        }

        private void lstView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (cmbPickDate.Visible)
            {
                if (cmbPickDate.SelectedIndex != -1)
                {
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                }
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void cmbPickDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedTime = cmbPickDate.SelectedItem.ToString();
            if (lstView.SelectedItems.Count>0)
            {
                button1.Enabled = true;

            }
        }
    }
}
