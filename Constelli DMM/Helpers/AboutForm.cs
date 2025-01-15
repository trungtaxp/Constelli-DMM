using System;
using System.Windows.Forms;

namespace Constelli_DMM.Helpers
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lblSoftwareInfo = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblDeveloper = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSoftwareInfo
            // 
            this.lblSoftwareInfo.AutoSize = true;
            this.lblSoftwareInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.lblSoftwareInfo.Location = new System.Drawing.Point(43, 91);
            this.lblSoftwareInfo.Name = "lblSoftwareInfo";
            this.lblSoftwareInfo.Size = new System.Drawing.Size(237, 25);
            this.lblSoftwareInfo.TabIndex = 0;
            this.lblSoftwareInfo.Text = "CONTROL LEVER TEST";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.lblVersion.Location = new System.Drawing.Point(95, 148);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(133, 25);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version: 1.0.3";
            // 
            // lblDeveloper
            // 
            this.lblDeveloper.AutoSize = true;
            this.lblDeveloper.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.lblDeveloper.Location = new System.Drawing.Point(58, 205);
            this.lblDeveloper.Name = "lblDeveloper";
            this.lblDeveloper.Size = new System.Drawing.Size(207, 25);
            this.lblDeveloper.TabIndex = 2;
            this.lblDeveloper.Text = "Developer by Constelli";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Constelli_DMM.Properties.Resources.wide_logo;
            this.pictureBox1.ErrorImage = global::Constelli_DMM.Properties.Resources.wide_logo;
            this.pictureBox1.InitialImage = global::Constelli_DMM.Properties.Resources.wide_logo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(299, 33);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label1.Location = new System.Drawing.Point(21, 262);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(281, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "License due date: Permanently";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(-2, 319);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(327, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Copyright by Constelli Signals Private Limited.\r\n";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(85, 344);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "All Rights Reserved.";
            // 
            // AboutForm
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(323, 392);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblDeveloper);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblSoftwareInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AboutForm";
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblSoftwareInfo;
        private System.Windows.Forms.Label lblVersion;
        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private System.Windows.Forms.Label lblDeveloper;
    }
}
