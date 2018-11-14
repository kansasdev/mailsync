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
            this.tbServer = new System.Windows.Forms.TextBox();
            this.tbProtocolVersion = new System.Windows.Forms.TextBox();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.tbDevice = new System.Windows.Forms.TextBox();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.cbLocked = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblServer = new System.Windows.Forms.Label();
            this.lblProtocol = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblDevice = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.cbExchange = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(170, 23);
            this.tbServer.Margin = new System.Windows.Forms.Padding(6);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(370, 31);
            this.tbServer.TabIndex = 0;
            // 
            // tbProtocolVersion
            // 
            this.tbProtocolVersion.Location = new System.Drawing.Point(170, 75);
            this.tbProtocolVersion.Margin = new System.Windows.Forms.Padding(6);
            this.tbProtocolVersion.Name = "tbProtocolVersion";
            this.tbProtocolVersion.Size = new System.Drawing.Size(370, 31);
            this.tbProtocolVersion.TabIndex = 1;
            // 
            // tbUsername
            // 
            this.tbUsername.Enabled = false;
            this.tbUsername.Location = new System.Drawing.Point(170, 122);
            this.tbUsername.Margin = new System.Windows.Forms.Padding(6);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(370, 31);
            this.tbUsername.TabIndex = 2;
            // 
            // tbDevice
            // 
            this.tbDevice.Enabled = false;
            this.tbDevice.Location = new System.Drawing.Point(170, 221);
            this.tbDevice.Margin = new System.Windows.Forms.Padding(6);
            this.tbDevice.Name = "tbDevice";
            this.tbDevice.Size = new System.Drawing.Size(186, 31);
            this.tbDevice.TabIndex = 3;
            // 
            // tbEmail
            // 
            this.tbEmail.Enabled = false;
            this.tbEmail.Location = new System.Drawing.Point(170, 170);
            this.tbEmail.Margin = new System.Windows.Forms.Padding(6);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(370, 31);
            this.tbEmail.TabIndex = 13;
            // 
            // cbLocked
            // 
            this.cbLocked.AutoSize = true;
            this.cbLocked.Checked = true;
            this.cbLocked.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLocked.Location = new System.Drawing.Point(372, 221);
            this.cbLocked.Margin = new System.Windows.Forms.Padding(6);
            this.cbLocked.Name = "cbLocked";
            this.cbLocked.Size = new System.Drawing.Size(144, 29);
            this.cbLocked.TabIndex = 4;
            this.cbLocked.Text = "BLOCKED";
            this.cbLocked.UseVisualStyleBackColor = true;
            this.cbLocked.CheckedChanged += new System.EventHandler(this.cbLocked_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnOK.Location = new System.Drawing.Point(170, 331);
            this.btnOK.Margin = new System.Windows.Forms.Padding(6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(190, 58);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(372, 331);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(190, 58);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(10, 23);
            this.lblServer.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(98, 25);
            this.lblServer.TabIndex = 7;
            this.lblServer.Text = "SERVER";
            // 
            // lblProtocol
            // 
            this.lblProtocol.AutoSize = true;
            this.lblProtocol.Location = new System.Drawing.Point(10, 75);
            this.lblProtocol.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblProtocol.Name = "lblProtocol";
            this.lblProtocol.Size = new System.Drawing.Size(129, 25);
            this.lblProtocol.TabIndex = 8;
            this.lblProtocol.Text = "PROTOCOL";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(10, 128);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(131, 25);
            this.lblUsername.TabIndex = 9;
            this.lblUsername.Text = "USERNAME";
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(10, 227);
            this.lblDevice.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(89, 25);
            this.lblDevice.TabIndex = 10;
            this.lblDevice.Text = "DEVICE";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(10, 176);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(75, 25);
            this.lblEmail.TabIndex = 13;
            this.lblEmail.Text = "EMAIL";
            // 
            // cbExchange
            // 
            this.cbExchange.AutoSize = true;
            this.cbExchange.Checked = true;
            this.cbExchange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExchange.Location = new System.Drawing.Point(372, 279);
            this.cbExchange.Margin = new System.Windows.Forms.Padding(6);
            this.cbExchange.Name = "cbExchange";
            this.cbExchange.Size = new System.Drawing.Size(198, 29);
            this.cbExchange.TabIndex = 11;
            this.cbExchange.Text = "IS EXCHANGE?";
            this.cbExchange.UseVisualStyleBackColor = true;
            this.cbExchange.CheckedChanged += new System.EventHandler(this.cbExchange_CheckedChanged_1);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 417);
            this.Controls.Add(this.cbExchange);
            this.Controls.Add(this.lblDevice);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblProtocol);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbLocked);
            this.Controls.Add(this.tbDevice);
            this.Controls.Add(this.tbUsername);
            this.Controls.Add(this.tbProtocolVersion);
            this.Controls.Add(this.tbServer);
            this.Controls.Add(this.tbEmail);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void CbExchange_CheckedChanged(object sender, EventArgs e)
        {
            if(cbExchange.Checked)
            {
                tbEmail.Enabled = true;
            }
            else
            {
                tbEmail.Enabled = false;
            }
        }

        #endregion

        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.TextBox tbProtocolVersion;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.TextBox tbDevice;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.CheckBox cbLocked;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblProtocol;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.CheckBox cbExchange;
    }
}