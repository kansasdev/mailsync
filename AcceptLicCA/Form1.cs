using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AcceptLicCA
{
    public partial class FormLicCA : Form
    {
        public bool CertInstalled;

        public FormLicCA()
        {
            if (Thread.CurrentThread.CurrentCulture.Name == "pl-PL")
            {
                rm = new ResourceManager("AcceptLicCA.Properties.Resources_pl", typeof(FormLicCA).Assembly);
            }
            else
            {
                rm = new ResourceManager("AcceptLicCA.Properties.Resources", typeof(FormLicCA).Assembly);
            }
           

            InitializeComponent();

            this.btnCancel.Text = rm.GetString("btnCancelRes");
            this.lblInfo.Text = rm.GetString("InfoRes");
            this.btnInstallCA.Text = rm.GetString("btnInstallRes");
            this.Text = rm.GetString("FormTitleRes");

            CertificateFinder cf = new CertificateFinder();
            string err = string.Empty;
            bool result = cf.FindProperCA("39 42 A2 17 39 F9 39 99 4E 94 48 D7 D6 11 DB EA", ref err);
            if(result)
            {
                lblCA.ForeColor = Color.Green;
                lblCA.Text = rm.GetString("ProperCAInstalledRes");
                btnContinue.Enabled = true;
                btnInstallCA.Enabled = false;
                CertInstalled = true;
            }
            else
            {
                if(err==string.Empty)
                {
                    lblCA.ForeColor = Color.Red;
                    lblCA.Text = rm.GetString("NoCAInstalledRes");
                    btnContinue.Enabled = false;
                    btnInstallCA.Enabled = true;
                }
                else
                {
                    MessageBox.Show(err);
                    lblCA.ForeColor = Color.Red;
                    lblCA.Text = rm.GetString("GenericErrorRes");
                    btnInstallCA.Enabled = false;
                    btnContinue.Enabled = false;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CertInstalled = false;
            this.Close();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnInstallCA_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] cert = Properties.Resources.kansas;
                string error = string.Empty;
                X509Certificate2 certCA = new X509Certificate2(cert);
                CertificateFinder cf = new CertificateFinder();
                bool result = cf.AddCACert(certCA, ref error);
                if(result)
                {
                    lblCA.Text = "OK";
                    lblCA.ForeColor = Color.Green;
                    CertInstalled = true;
                    this.Close();
                }
                else
                {
                    lblCA.Text = rm.GetString("NoCAAdded");
                    lblCA.ForeColor = Color.Red;
                    CertInstalled = false;
                    if(!string.IsNullOrEmpty(error))
                    {
                        MessageBox.Show(error);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(rm.GetString("GenericErrorRes") + ex.Message);
                CertInstalled = false;
            }
        }
    }
}
