using System;
using System.Resources;
using System.Threading;

namespace MailSync
{
    partial class SettingsForm
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

        protected override void OnLoad(EventArgs e)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.tbServer, rm.GetString("tooltipServerSettings"));

            System.Windows.Forms.ToolTip ToolTip2 = new System.Windows.Forms.ToolTip();
            ToolTip2.SetToolTip(this.tbProtocolVersion, rm.GetString("tooltipProtocolSettings"));

            System.Windows.Forms.ToolTip ToolTip3 = new System.Windows.Forms.ToolTip();
            ToolTip3.SetToolTip(this.cbLocked, rm.GetString("tooltipCheckboxSettings"));

            base.OnLoad(e);
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

            this.tbServer = new System.Windows.Forms.TextBox();
            this.tbProtocolVersion = new System.Windows.Forms.TextBox();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.tbDevice = new System.Windows.Forms.TextBox();
            this.cbLocked = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblServer = new System.Windows.Forms.Label();
            this.lblProtocol = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblDevice = new System.Windows.Forms.Label();
            this.cbExchange = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(85, 12);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(187, 20);
            this.tbServer.TabIndex = 0;
            // 
            // tbProtocolVersion
            // 
            this.tbProtocolVersion.Location = new System.Drawing.Point(85, 39);
            this.tbProtocolVersion.Name = "tbProtocolVersion";
            this.tbProtocolVersion.Size = new System.Drawing.Size(187, 20);
            this.tbProtocolVersion.TabIndex = 1;
            // 
            // tbUsername
            // 
            this.tbUsername.Enabled = false;
            this.tbUsername.Location = new System.Drawing.Point(85, 65);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(187, 20);
            this.tbUsername.TabIndex = 2;
            // 
            // tbDevice
            // 
            this.tbDevice.Enabled = false;
            this.tbDevice.Location = new System.Drawing.Point(85, 91);
            this.tbDevice.Name = "tbDevice";
            this.tbDevice.Size = new System.Drawing.Size(95, 20);
            this.tbDevice.TabIndex = 3;
            // 
            // cbLocked
            // 
            this.cbLocked.AutoSize = true;
            this.cbLocked.Checked = true;
            this.cbLocked.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLocked.Location = new System.Drawing.Point(192, 93);
            this.cbLocked.Name = "cbLocked";
            this.cbLocked.Size = new System.Drawing.Size(15, 14);
            this.cbLocked.TabIndex = 4;
            this.cbLocked.UseVisualStyleBackColor = true;
            this.cbLocked.Text = rm.GetString("settingsChkUnblock");
            this.cbLocked.CheckedChanged += new System.EventHandler(this.cbLocked_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnOK.Location = new System.Drawing.Point(85, 138);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 30);
            this.btnOK.TabIndex = 5;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Text = rm.GetString("settingsBtnOK");
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(186, 138);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 30);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Text = rm.GetString("settingsBtnCancel");
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(5, 18);
            this.lblServer.Name = "lblServer";
            //this.lblServer.Size = new System.Drawing.Size(0, 13);
            this.lblServer.Text = rm.GetString("settingsLblServer");
            this.lblServer.TabIndex = 7;
            // 
            // lblProtocol
            // 
            this.lblProtocol.AutoSize = true;
            this.lblProtocol.Location = new System.Drawing.Point(5, 42);
            this.lblProtocol.Name = "lblProtocol";
            //this.lblProtocol.Size = new System.Drawing.Size(0, 13);
            this.lblProtocol.Text = rm.GetString("settingsLblProtocol");
            this.lblProtocol.TabIndex = 8;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(5, 68);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Text = rm.GetString("settingsLblUsername");
            //this.lblUsername.Size = new System.Drawing.Size(0, 13);
            this.lblUsername.TabIndex = 9;
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(5, 93);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(0, 13);
            this.lblDevice.Text = rm.GetString("settingsLblDevice");
            this.lblDevice.TabIndex = 10;
            // 
            // cbExchange
            // 
            this.cbExchange.AutoSize = true;
            this.cbExchange.Checked = true;
            this.cbExchange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExchange.Location = new System.Drawing.Point(192, 118);
            this.cbExchange.Name = "cbExchange";
            //this.cbExchange.Size = new System.Drawing.Size(15, 14);
            this.cbExchange.Text = rm.GetString("settingsCbExchange");
            this.cbExchange.TabIndex = 11;
            this.cbExchange.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 180);
            this.Controls.Add(this.cbExchange);
            this.Controls.Add(this.lblDevice);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblProtocol);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbLocked);
            this.Controls.Add(this.tbDevice);
            this.Controls.Add(this.tbUsername);
            this.Controls.Add(this.tbProtocolVersion);
            this.Controls.Add(this.tbServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.Text = rm.GetString("settingsTitle");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.TextBox tbProtocolVersion;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.TextBox tbDevice;
        private System.Windows.Forms.CheckBox cbLocked;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblProtocol;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.CheckBox cbExchange;
    }
}