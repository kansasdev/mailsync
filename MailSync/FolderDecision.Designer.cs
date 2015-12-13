using System.Resources;
using System.Threading;

namespace MailSync
{
    partial class FolderDecision
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private ResourceManager rm;
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
            if (Thread.CurrentThread.CurrentCulture.Name == "pl-PL")
            {
                rm = new ResourceManager("MailSync.Properties.Resources_pl_PL", typeof(RibbonOutlook).Assembly);
            }
            else
            {
                rm = new ResourceManager("MailSync.Properties.Resources", typeof(RibbonOutlook).Assembly);
            }

            this.lstView = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.cmbPickDate = new System.Windows.Forms.ComboBox();
            this.lblCombo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstView
            // 
            this.lstView.Location = new System.Drawing.Point(3, 1);
            this.lstView.MultiSelect = false;
            this.lstView.Name = "lstView";
            this.lstView.Size = new System.Drawing.Size(367, 213);
            this.lstView.TabIndex = 0;
            this.lstView.UseCompatibleStateImageBehavior = false;
            this.lstView.View = System.Windows.Forms.View.List;
            this.lstView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lstView_ItemSelectionChanged);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(260, 218);
            this.button1.Name = "button1";
            this.button1.Text = rm.GetString("btnDestinationDirRes");
            this.button1.Size = new System.Drawing.Size(110, 38);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbPickDate
            // 
            this.cmbPickDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPickDate.FormattingEnabled = true;
            this.cmbPickDate.Items.AddRange(new object[] {
            "1",
            "3",
            "7",
            "14",
            "30",
            "90",
            "180",
            "ALL"});
            this.cmbPickDate.Location = new System.Drawing.Point(205, 218);
            this.cmbPickDate.Name = "cmbPickDate";
            this.cmbPickDate.Size = new System.Drawing.Size(49, 21);
            this.cmbPickDate.TabIndex = 2;
            this.cmbPickDate.Visible = false;
            this.cmbPickDate.SelectedIndexChanged += new System.EventHandler(this.cmbPickDate_SelectedIndexChanged);
            // 
            // lblCombo
            // 
            this.lblCombo.AutoSize = true;
            this.lblCombo.Location = new System.Drawing.Point(12, 223);
            this.lblCombo.Name = "lblCombo";
            this.lblCombo.Size = new System.Drawing.Size(0, 13);
            this.lblCombo.TabIndex = 3;
            this.lblCombo.Visible = false;
            this.lblCombo.Text = rm.GetString("cmbTextChooseOption");
            // 
            // FolderDecision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 261);
            this.Controls.Add(this.cmbPickDate);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lstView);
            this.Controls.Add(this.lblCombo);
            this.Name = rm.GetString("lblChooseOption");
            this.Text = rm.GetString("lblChooseOption");
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstView;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cmbPickDate;
        private System.Windows.Forms.Label lblCombo;
    }
}