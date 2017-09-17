namespace AquaFan
{
    partial class frmConfiguration
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.tbAquaComputerCmdPath = new System.Windows.Forms.TextBox();
            this.btnSelectAquacomputerPath = new System.Windows.Forms.Button();
            this.lblAquacomputerPath = new System.Windows.Forms.Label();
            this.cbLanguages = new System.Windows.Forms.ComboBox();
            this.lblDefaultLanguage = new System.Windows.Forms.Label();
            this.btnSaveConfiguration = new System.Windows.Forms.Button();
            this.tbDeviceSerial = new System.Windows.Forms.TextBox();
            this.lblDeviceSerial = new System.Windows.Forms.Label();
            this.grpAqua = new System.Windows.Forms.GroupBox();
            this.grpProgramConfig = new System.Windows.Forms.GroupBox();
            this.chkApplyChangesAtStartup = new System.Windows.Forms.CheckBox();
            this.chkSaveBeforeApply = new System.Windows.Forms.CheckBox();
            this.chkStartMinimized = new System.Windows.Forms.CheckBox();
            this.grpAqua.SuspendLayout();
            this.grpProgramConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbAquaComputerCmdPath
            // 
            this.tbAquaComputerCmdPath.Enabled = false;
            this.tbAquaComputerCmdPath.Location = new System.Drawing.Point(6, 68);
            this.tbAquaComputerCmdPath.Name = "tbAquaComputerCmdPath";
            this.tbAquaComputerCmdPath.Size = new System.Drawing.Size(655, 20);
            this.tbAquaComputerCmdPath.TabIndex = 0;
            // 
            // btnSelectAquacomputerPath
            // 
            this.btnSelectAquacomputerPath.Location = new System.Drawing.Point(667, 66);
            this.btnSelectAquacomputerPath.Name = "btnSelectAquacomputerPath";
            this.btnSelectAquacomputerPath.Size = new System.Drawing.Size(33, 23);
            this.btnSelectAquacomputerPath.TabIndex = 1;
            this.btnSelectAquacomputerPath.Text = "...";
            this.btnSelectAquacomputerPath.UseVisualStyleBackColor = true;
            this.btnSelectAquacomputerPath.Click += new System.EventHandler(this.btnSelectAquacomputerPath_Click);
            // 
            // lblAquacomputerPath
            // 
            this.lblAquacomputerPath.AutoSize = true;
            this.lblAquacomputerPath.Location = new System.Drawing.Point(6, 47);
            this.lblAquacomputerPath.Name = "lblAquacomputerPath";
            this.lblAquacomputerPath.Size = new System.Drawing.Size(35, 13);
            this.lblAquacomputerPath.TabIndex = 2;
            this.lblAquacomputerPath.Text = "label1";
            // 
            // cbLanguages
            // 
            this.cbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguages.FormattingEnabled = true;
            this.cbLanguages.Location = new System.Drawing.Point(6, 32);
            this.cbLanguages.Name = "cbLanguages";
            this.cbLanguages.Size = new System.Drawing.Size(121, 21);
            this.cbLanguages.TabIndex = 3;
            // 
            // lblDefaultLanguage
            // 
            this.lblDefaultLanguage.AutoSize = true;
            this.lblDefaultLanguage.Location = new System.Drawing.Point(6, 16);
            this.lblDefaultLanguage.Name = "lblDefaultLanguage";
            this.lblDefaultLanguage.Size = new System.Drawing.Size(35, 13);
            this.lblDefaultLanguage.TabIndex = 4;
            this.lblDefaultLanguage.Text = "label1";
            // 
            // btnSaveConfiguration
            // 
            this.btnSaveConfiguration.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSaveConfiguration.Location = new System.Drawing.Point(529, 304);
            this.btnSaveConfiguration.Name = "btnSaveConfiguration";
            this.btnSaveConfiguration.Size = new System.Drawing.Size(189, 23);
            this.btnSaveConfiguration.TabIndex = 5;
            this.btnSaveConfiguration.Text = "button1";
            this.btnSaveConfiguration.UseVisualStyleBackColor = true;
            this.btnSaveConfiguration.Click += new System.EventHandler(this.btnSaveConfiguration_Click);
            // 
            // tbDeviceSerial
            // 
            this.tbDeviceSerial.Location = new System.Drawing.Point(528, 20);
            this.tbDeviceSerial.Name = "tbDeviceSerial";
            this.tbDeviceSerial.Size = new System.Drawing.Size(172, 20);
            this.tbDeviceSerial.TabIndex = 7;
            // 
            // lblDeviceSerial
            // 
            this.lblDeviceSerial.AutoSize = true;
            this.lblDeviceSerial.Location = new System.Drawing.Point(6, 23);
            this.lblDeviceSerial.Name = "lblDeviceSerial";
            this.lblDeviceSerial.Size = new System.Drawing.Size(77, 13);
            this.lblDeviceSerial.TabIndex = 6;
            this.lblDeviceSerial.Text = "lblDeviceSerial";
            // 
            // grpAqua
            // 
            this.grpAqua.Controls.Add(this.tbDeviceSerial);
            this.grpAqua.Controls.Add(this.lblDeviceSerial);
            this.grpAqua.Controls.Add(this.tbAquaComputerCmdPath);
            this.grpAqua.Controls.Add(this.btnSelectAquacomputerPath);
            this.grpAqua.Controls.Add(this.lblAquacomputerPath);
            this.grpAqua.Location = new System.Drawing.Point(12, 12);
            this.grpAqua.Name = "grpAqua";
            this.grpAqua.Size = new System.Drawing.Size(706, 127);
            this.grpAqua.TabIndex = 8;
            this.grpAqua.TabStop = false;
            this.grpAqua.Text = "groupBox1";
            // 
            // grpProgramConfig
            // 
            this.grpProgramConfig.Controls.Add(this.chkStartMinimized);
            this.grpProgramConfig.Controls.Add(this.chkApplyChangesAtStartup);
            this.grpProgramConfig.Controls.Add(this.chkSaveBeforeApply);
            this.grpProgramConfig.Controls.Add(this.lblDefaultLanguage);
            this.grpProgramConfig.Controls.Add(this.cbLanguages);
            this.grpProgramConfig.Location = new System.Drawing.Point(12, 145);
            this.grpProgramConfig.Name = "grpProgramConfig";
            this.grpProgramConfig.Size = new System.Drawing.Size(706, 153);
            this.grpProgramConfig.TabIndex = 9;
            this.grpProgramConfig.TabStop = false;
            // 
            // chkApplyChangesAtStartup
            // 
            this.chkApplyChangesAtStartup.AutoSize = true;
            this.chkApplyChangesAtStartup.Location = new System.Drawing.Point(6, 96);
            this.chkApplyChangesAtStartup.Name = "chkApplyChangesAtStartup";
            this.chkApplyChangesAtStartup.Size = new System.Drawing.Size(156, 17);
            this.chkApplyChangesAtStartup.TabIndex = 6;
            this.chkApplyChangesAtStartup.Text = "chkApplyChangesAtStartup";
            this.chkApplyChangesAtStartup.UseVisualStyleBackColor = true;
            // 
            // chkSaveBeforeApply
            // 
            this.chkSaveBeforeApply.AutoSize = true;
            this.chkSaveBeforeApply.Location = new System.Drawing.Point(6, 72);
            this.chkSaveBeforeApply.Name = "chkSaveBeforeApply";
            this.chkSaveBeforeApply.Size = new System.Drawing.Size(80, 17);
            this.chkSaveBeforeApply.TabIndex = 5;
            this.chkSaveBeforeApply.Text = "checkBox1";
            this.chkSaveBeforeApply.UseVisualStyleBackColor = true;
            // 
            // chkStartMinimized
            // 
            this.chkStartMinimized.AutoSize = true;
            this.chkStartMinimized.Location = new System.Drawing.Point(6, 119);
            this.chkStartMinimized.Name = "chkStartMinimized";
            this.chkStartMinimized.Size = new System.Drawing.Size(112, 17);
            this.chkStartMinimized.TabIndex = 7;
            this.chkStartMinimized.Text = "chkStartMinimized";
            this.chkStartMinimized.UseVisualStyleBackColor = true;
            // 
            // frmConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 349);
            this.Controls.Add(this.btnSaveConfiguration);
            this.Controls.Add(this.grpAqua);
            this.Controls.Add(this.grpProgramConfig);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmConfiguration";
            this.Text = "frmConfiguration";
            this.Load += new System.EventHandler(this.frmConfiguration_Load);
            this.grpAqua.ResumeLayout(false);
            this.grpAqua.PerformLayout();
            this.grpProgramConfig.ResumeLayout(false);
            this.grpProgramConfig.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbAquaComputerCmdPath;
        private System.Windows.Forms.Button btnSelectAquacomputerPath;
        private System.Windows.Forms.Label lblAquacomputerPath;
        private System.Windows.Forms.ComboBox cbLanguages;
        private System.Windows.Forms.Label lblDefaultLanguage;
        private System.Windows.Forms.Button btnSaveConfiguration;
        private System.Windows.Forms.TextBox tbDeviceSerial;
        private System.Windows.Forms.Label lblDeviceSerial;
        private System.Windows.Forms.GroupBox grpAqua;
        private System.Windows.Forms.GroupBox grpProgramConfig;
        private System.Windows.Forms.CheckBox chkSaveBeforeApply;
        private System.Windows.Forms.CheckBox chkApplyChangesAtStartup;
        private System.Windows.Forms.CheckBox chkStartMinimized;
    }
}