using System.Resources;
using System.Threading;
namespace MailSync
{
    partial class RibbonOutlook : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private ResourceManager rm;

        public RibbonOutlook()
            : base(Globals.Factory.GetRibbonFactory())
        {
            if (Thread.CurrentThread.CurrentCulture.Name == "pl-PL") //!!!!
            {
                rm = new ResourceManager("MailSync.Properties.Resources_pl_PL", typeof(RibbonOutlook).Assembly);
            }
            else
            {
                rm = new ResourceManager("MailSync.Properties.Resources", typeof(RibbonOutlook).Assembly);
            }

            
            InitializeComponent();
          
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SyncTab = this.Factory.CreateRibbonTab();
            this.grpImport = this.Factory.CreateRibbonGroup();
            this.btnImport = this.Factory.CreateRibbonButton();
            this.grpKonwersja = this.Factory.CreateRibbonGroup();
            this.btnDirectory = this.Factory.CreateRibbonButton();
            this.grpCleaning = this.Factory.CreateRibbonGroup();
            this.btnClean = this.Factory.CreateRibbonButton();
            this.grpHelp = this.Factory.CreateRibbonGroup();
            this.btnHelp = this.Factory.CreateRibbonButton();
            this.grpInformation = this.Factory.CreateRibbonGroup();
            this.lblNew = this.Factory.CreateRibbonLabel();
            this.lblConverted = this.Factory.CreateRibbonLabel();
            this.lblTotal = this.Factory.CreateRibbonLabel();
            this.SyncTab.SuspendLayout();
            this.grpImport.SuspendLayout();
            this.grpKonwersja.SuspendLayout();
            this.grpCleaning.SuspendLayout();
            this.grpHelp.SuspendLayout();
            this.grpInformation.SuspendLayout();
            // 
            // SyncTab
            // 
            this.SyncTab.Groups.Add(this.grpImport);
            this.SyncTab.Groups.Add(this.grpKonwersja);
            this.SyncTab.Groups.Add(this.grpCleaning);
            this.SyncTab.Groups.Add(this.grpHelp);
            this.SyncTab.Groups.Add(this.grpInformation);
            this.SyncTab.Label = rm.GetString("tabSyncLabelRes");
            this.SyncTab.Name = "SyncTab";
            // 
            // grpImport
            // 
            this.grpImport.Items.Add(this.btnImport);
            this.grpImport.Label = rm.GetString("grpImportLabelRes");
            this.grpImport.Name = "grpImport";
            // 
            // btnImport
            // 
            this.btnImport.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnImport.Label = rm.GetString("btnImportLabelRes");
            this.btnImport.Name = "btnImport";
            this.btnImport.OfficeImageId = "ImportOutlook";
            this.btnImport.ShowImage = true;
            this.btnImport.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnImport_Click);
            // 
            // grpKonwersja
            // 
            this.grpKonwersja.Items.Add(this.btnDirectory);
            this.grpKonwersja.Label = rm.GetString("grpKonwersjaLabelRes");
            this.grpKonwersja.Name = "grpKonwersja";
            // 
            // btnDirectory
            // 
            this.btnDirectory.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnDirectory.Label = rm.GetString("btnDirectoryLabelRes");
            this.btnDirectory.Name = "btnDirectory";
            this.btnDirectory.OfficeImageId = "MenuOpenAppointment";
            this.btnDirectory.ShowImage = true;
            this.btnDirectory.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnDirectory_Click);
            // 
            // grpCzyszczenie
            // 
            this.grpCleaning.Items.Add(this.btnClean);
            this.grpCleaning.Label = rm.GetString("grpCleaningLabelRes");
            this.grpCleaning.Name = "grpCleaning";
            // 
            // btnClean
            // 
            this.btnClean.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnClean.Label = rm.GetString("btnCleanLabelRes");
            this.btnClean.Name = "btnClean";
            this.btnClean.OfficeImageId = "DeleteAll";
            this.btnClean.ShowImage = true;
            this.btnClean.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnClean_Click);
            // 
            // grpPomoc
            // 
            this.grpHelp.Items.Add(this.btnHelp);
            this.grpHelp.Label = rm.GetString("grpHelpLabelRes");
            this.grpHelp.Name = "grpHelp";
            // 
            // btnHelp
            // 
            this.btnHelp.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnHelp.Label = rm.GetString("btnHelpLabelRes");
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.OfficeImageId = "HelpDevResources";
            this.btnHelp.ShowImage = true;
            this.btnHelp.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnHelp_Click);
            // 
            // grpInformacje
            // 
            this.grpInformation.Items.Add(this.lblNew);
            this.grpInformation.Items.Add(this.lblConverted);
            this.grpInformation.Items.Add(this.lblTotal);
            this.grpInformation.Label = rm.GetString("grpInformationLabelRes");
            this.grpInformation.Name = "grpInformation";
            // 
            // lblNew
            // 
            this.lblNew.Label = " ";
            this.lblNew.Name = "lblNew";
            // 
            // lblConverted
            // 
            this.lblConverted.Label = " ";
            this.lblConverted.Name = "lblConverted";
            // 
            // lblTotal
            // 
            this.lblTotal.Label = " ";
            this.lblTotal.Name = "lblTotal";
            // 
            // RibbonOutlook
            // 
            this.Name = "RibbonOutlook";
            this.RibbonType = "Microsoft.Outlook.Explorer";
            this.Tabs.Add(this.SyncTab);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.RibbonOutlook_Load);
            this.SyncTab.ResumeLayout(false);
            this.SyncTab.PerformLayout();
            this.grpImport.ResumeLayout(false);
            this.grpImport.PerformLayout();
            this.grpKonwersja.ResumeLayout(false);
            this.grpKonwersja.PerformLayout();
            this.grpCleaning.ResumeLayout(false);
            this.grpCleaning.PerformLayout();
            this.grpHelp.ResumeLayout(false);
            this.grpHelp.PerformLayout();
            this.grpInformation.ResumeLayout(false);
            this.grpInformation.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab SyncTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpImport;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnImport;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDirectory;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpCleaning;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpInformation;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblNew;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblConverted;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblTotal;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpKonwersja;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpHelp;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnClean;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnHelp;
    }

    partial class ThisRibbonCollection
    {
        internal RibbonOutlook RibbonOutlook
        {
            get { return this.GetRibbon<RibbonOutlook>(); }
        }
    }
}
