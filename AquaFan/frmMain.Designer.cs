namespace AquaFan
{
    partial class frmMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tBtnFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tBtnLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSave = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSaveCurrentProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSaveAllProfiles = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSaveSelectedProfiles = new System.Windows.Forms.ToolStripMenuItem();
            this.tBtnClose = new System.Windows.Forms.ToolStripMenuItem();
            this.tBtnEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCreateProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.tBtnConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAccept = new System.Windows.Forms.Button();
            this.tabProfiles = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cmRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.icon = new System.Windows.Forms.NotifyIcon(this.components);
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabProfiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 259);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(923, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tBtnFile,
            this.tBtnEdit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(923, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tBtnFile
            // 
            this.tBtnFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tBtnLanguage,
            this.btnSave,
            this.tBtnClose});
            this.tBtnFile.Name = "tBtnFile";
            this.tBtnFile.Size = new System.Drawing.Size(55, 20);
            this.tBtnFile.Text = "btnFile";
            // 
            // tBtnLanguage
            // 
            this.tBtnLanguage.Name = "tBtnLanguage";
            this.tBtnLanguage.Size = new System.Drawing.Size(152, 22);
            this.tBtnLanguage.Text = "btnLanguage";
            // 
            // btnSave
            // 
            this.btnSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSaveCurrentProfile,
            this.btnSaveAllProfiles,
            this.btnSaveSelectedProfiles});
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(152, 22);
            this.btnSave.Text = "btnSave";
            // 
            // btnSaveCurrentProfile
            // 
            this.btnSaveCurrentProfile.Name = "btnSaveCurrentProfile";
            this.btnSaveCurrentProfile.Size = new System.Drawing.Size(165, 22);
            this.btnSaveCurrentProfile.Text = "Aktives Profil";
            this.btnSaveCurrentProfile.Click += new System.EventHandler(this.btnSaveCurrentProfile_Click);
            // 
            // btnSaveAllProfiles
            // 
            this.btnSaveAllProfiles.Name = "btnSaveAllProfiles";
            this.btnSaveAllProfiles.Size = new System.Drawing.Size(165, 22);
            this.btnSaveAllProfiles.Text = "Alle Profile";
            this.btnSaveAllProfiles.Click += new System.EventHandler(this.btnSaveAllProfiles_Click);
            // 
            // btnSaveSelectedProfiles
            // 
            this.btnSaveSelectedProfiles.Name = "btnSaveSelectedProfiles";
            this.btnSaveSelectedProfiles.Size = new System.Drawing.Size(165, 22);
            this.btnSaveSelectedProfiles.Text = "Selektierte Profile";
            this.btnSaveSelectedProfiles.Visible = false;
            // 
            // tBtnClose
            // 
            this.tBtnClose.Name = "tBtnClose";
            this.tBtnClose.Size = new System.Drawing.Size(152, 22);
            this.tBtnClose.Text = "btnClose";
            this.tBtnClose.Click += new System.EventHandler(this.tBtnClose_Click);
            // 
            // tBtnEdit
            // 
            this.tBtnEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCreateProfile,
            this.tBtnConfiguration});
            this.tBtnEdit.Name = "tBtnEdit";
            this.tBtnEdit.Size = new System.Drawing.Size(57, 20);
            this.tBtnEdit.Text = "btnEdit";
            // 
            // btnCreateProfile
            // 
            this.btnCreateProfile.Name = "btnCreateProfile";
            this.btnCreateProfile.Size = new System.Drawing.Size(160, 22);
            this.btnCreateProfile.Text = "btnCreateProfile";
            this.btnCreateProfile.Visible = false;
            this.btnCreateProfile.Click += new System.EventHandler(this.btnCreateProfile_Click);
            // 
            // tBtnConfiguration
            // 
            this.tBtnConfiguration.Name = "tBtnConfiguration";
            this.tBtnConfiguration.Size = new System.Drawing.Size(160, 22);
            this.tBtnConfiguration.Text = "btnConfig";
            this.tBtnConfiguration.Click += new System.EventHandler(this.tBtnConfiguration_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(816, 224);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(91, 23);
            this.btnAccept.TabIndex = 5;
            this.btnAccept.Text = "btnAccept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // tabProfiles
            // 
            this.tabProfiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabProfiles.Controls.Add(this.tabPage1);
            this.tabProfiles.Location = new System.Drawing.Point(12, 27);
            this.tabProfiles.Name = "tabProfiles";
            this.tabProfiles.SelectedIndex = 0;
            this.tabProfiles.Size = new System.Drawing.Size(899, 187);
            this.tabProfiles.TabIndex = 6;
            this.tabProfiles.SelectedIndexChanged += new System.EventHandler(this.tabProfiles_SelectedIndexChanged);
            this.tabProfiles.Click += new System.EventHandler(this.tabProfiles_Click);
            this.tabProfiles.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.tabProfiles_ControlAdded);
            this.tabProfiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tabProfiles_KeyDown);
            this.tabProfiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tabProfiles_MouseDoubleClick);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(891, 161);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "+ (Strg + T)";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // cmRightClick
            // 
            this.cmRightClick.Name = "cmRightClick";
            this.cmRightClick.Size = new System.Drawing.Size(61, 4);
            this.cmRightClick.Opening += new System.ComponentModel.CancelEventHandler(this.cmRightClick_Opening);
            // 
            // icon
            // 
            this.icon.ContextMenuStrip = this.cmRightClick;
            this.icon.Icon = ((System.Drawing.Icon)(resources.GetObject("icon.Icon")));
            this.icon.Text = "AquaFan";
            this.icon.Visible = true;
            this.icon.Click += new System.EventHandler(this.icon_Click);
            this.icon.DoubleClick += new System.EventHandler(this.icon_DoubleClick);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 216);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(802, 34);
            this.richTextBox1.TabIndex = 7;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            this.richTextBox1.Visible = false;
            // 
            // frmMain
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 281);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.tabProfiles);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.frmMain_SizeChanged);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabProfiles.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tBtnFile;
        private System.Windows.Forms.ToolStripMenuItem tBtnLanguage;
        private System.Windows.Forms.ToolStripMenuItem tBtnClose;
        private System.Windows.Forms.ToolStripMenuItem tBtnEdit;
        private System.Windows.Forms.ToolStripMenuItem tBtnConfiguration;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.TabControl tabProfiles;
        private System.Windows.Forms.ToolStripMenuItem btnSave;
        private System.Windows.Forms.ToolStripMenuItem btnSaveCurrentProfile;
        private System.Windows.Forms.ToolStripMenuItem btnSaveAllProfiles;
        private System.Windows.Forms.ToolStripMenuItem btnSaveSelectedProfiles;
        private System.Windows.Forms.ToolStripMenuItem btnCreateProfile;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ContextMenuStrip cmRightClick;
        private System.Windows.Forms.NotifyIcon icon;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

