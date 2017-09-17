using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace AquaFan
{
    public partial class frmMain : Form
    {
        bool bLoading = false;

        public frmMain()
        {
            InitializeComponent();
        }

        Controller cntrl;

        private void Form1_Load(object sender, EventArgs e)
        {
            bLoading = true;
            cntrl = new Controller(lblStatus, tBtnLanguage, this, statusStrip1, menuStrip1, btnAccept, tabProfiles);
            bLoading = false;

            if(cntrl.XmlControllerObject.StartMinimizedValue)
            {
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void ProfilePage_Click(object sender, EventArgs e)
        {
            
        }

        private void tBtnConfiguration_Click(object sender, EventArgs e)
        {
            cntrl.configureBaseSettings();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            cntrl.saveActiveProfile();
            cntrl.applyCurrentProfileChanges();
        }

        private void tbFanDescription_TextChanged(object sender, EventArgs e)
        {
            cntrl.SelectedFan.Description = ((TextBox)sender).Text;
            cntrl.SelectedFan.Updated = true;
        }

        private void tabProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(bLoading) { return; }

            if(((TabControl)sender).SelectedIndex < 0) { return; }

            if(((Profile)((TabControl)sender).TabPages[((TabControl)sender).SelectedIndex]).IsAddProfileTab)
            {

                cntrl.createProfileWrapper(this);

            }
            else
            {
                if(((TabControl)sender).TabPages[((TabControl)sender).SelectedIndex] is Profile)
                {
                    cntrl.CurrentProfile = (Profile)((TabControl)sender).TabPages[((TabControl)sender).SelectedIndex];
                    btnAccept.Enabled = cntrl.CurrentProfile.IsActiveProfile;
                }
            }
        }

        private void btnSaveCurrentProfile_Click(object sender, EventArgs e)
        {
            cntrl.saveActiveProfile();
        }

        private void btnSaveAllProfiles_Click(object sender, EventArgs e)
        {
            cntrl.saveAllProfiles();
            cntrl.loadProfiles();
            cntrl.showProfiles();
        }        

        private void btnCreateProfile_Click(object sender, EventArgs e)
        {
            cntrl.createProfileWrapper(this);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

            cntrl.createProfileWrapper(this);

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch(keyData)
            {
                case Keys.Control | Keys.T:
                    cntrl.createProfileWrapper(this);

                    return true;
                case Keys.Control | Keys.S:
                    cntrl.saveAllProfiles();
                    cntrl.loadProfiles();
                    cntrl.showProfiles();
                    return true;

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            btnAccept.Location = new Point(tabProfiles.Location.X + tabProfiles.Width - btnAccept.Width, tabProfiles.Location.Y + tabProfiles.Height + 6);
        }

        private void cmsChangeProfileName_Click(object sender, EventArgs e)
        {

        }

        bool bDoubleClickedAddProfileTab = false;
        private void tabProfiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            bDoubleClickedAddProfileTab = false;
            for (int i = 0; i < tabProfiles.TabCount; ++i)
            {
                if (tabProfiles.GetTabRect(i).Contains(e.Location))
                {
                    if (((Profile)tabProfiles.TabPages[i]).IsAddProfileTab)
                    {
                        
                        bDoubleClickedAddProfileTab = true;
                        return;
                    }
                    
                    cntrl.CurrentProfile = (((Profile)tabProfiles.TabPages[i]));
                }
            }
            if (bDoubleClickedAddProfileTab) { return; }
            
            cntrl.changeProfileName();
            cntrl.CurrentForm = this;
            cntrl.LanguageControllerObject.collectControls();
            cntrl.LanguageControllerObject.changeLanguage(cntrl.LanguageControllerObject.CurrentLanguage);

        }

        private void tabProfiles_KeyDown(object sender, KeyEventArgs e)
        {
            if(((Profile)((TabControl)tabProfiles).SelectedTab).IsAddProfileTab)
            {
                return;
            }

            if(e.KeyValue == 46)
            {
                cntrl.CurrentProfile = (Profile)((TabControl)tabProfiles).SelectedTab;
                
                if(cntrl != null)
                {
                    cntrl.CurrentProfile.DeleteProfile = !cntrl.CurrentProfile.Text.Contains(" [Del]");

                    if (cntrl.CurrentProfile.Text.Contains(" [Del]"))
                    {
                        cntrl.CurrentProfile.Text = cntrl.CurrentProfile.Text.Replace(" [Del]", "");
                        if (cntrl.CurrentProfile.IsActiveProfile) { btnAccept.Enabled = true; }
                    }
                    else
                    {
                        cntrl.CurrentProfile.Text += " [Del]";
                        if(cntrl.CurrentProfile.IsActiveProfile) { btnAccept.Enabled = false; }
                    }
                }
            }
        }

        private void tBtnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cmRightClick_Opening(object sender, CancelEventArgs e)
        {

        }

        private void NotifyIconItemClicked(object sender, EventArgs e)
        {
            bool bChangedProfile = false;
            foreach (Profile p in cntrl.Profiles)
            {
                if(p.Text == ((ToolStripMenuItem)sender).Text & !p.DeleteProfile &!p.IsAddProfileTab)
                {
                    cntrl.activateProfile(p);
                    bChangedProfile = true;
                }
            }

            if(bChangedProfile)
            {
                cntrl.saveActiveProfile();
                cntrl.applyCurrentProfileChanges();
            }
        }

        private void icon_Click(object sender, EventArgs e)
        {
            cmRightClick.Items.Clear();

            foreach (Profile p in cntrl.Profiles)
            {
                if (p.IsAddProfileTab) { continue; }

                cmRightClick.Items.Add(p.Text);
                cmRightClick.Items[cmRightClick.Items.Count - 1].Click += NotifyIconItemClicked;

                if (p.IsActiveProfile)
                {
                    cmRightClick.Items[cmRightClick.Items.Count - 1].ForeColor = Color.Green;
                }
            }
            cmRightClick.Items.Add("-");
            cmRightClick.Items.Add(cntrl.LanguageControllerObject.getVariableText(cntrl.LanguageControllerObject.CurrentLanguage, "tBtnResize"));
            cmRightClick.Items.Add(cntrl.LanguageControllerObject.getVariableText(cntrl.LanguageControllerObject.CurrentLanguage, "tBtnClose"));
            cmRightClick.Items[cmRightClick.Items.Count - 1].Click += NotifyIconCloseClick;
            cmRightClick.Items[cmRightClick.Items.Count - 2].Click += NotifyIconResizeClick;
        }

        private void NotifyIconResizeClick(object sender, EventArgs e)
        {
            cntrl.CurrentForm = this;
            cntrl.restoreFormSize();
        }

        private void NotifyIconCloseClick(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
            return;
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            }
            else
            {
                this.ShowInTaskbar = true;
            }
        }

        private void icon_DoubleClick(object sender, EventArgs e)
        {
            cntrl.CurrentForm = this;
            cntrl.restoreFormSize();
        }

        private void tabProfiles_Click(object sender, EventArgs e)
        {
            if(tabProfiles.TabPages.Count == 1)
            {
                //Nur das Add-Profil ist vorhanden
                cntrl.createProfileWrapper(this);
            }
        }

        private void tabProfiles_ControlAdded(object sender, ControlEventArgs e)
        {
            //Neu erstelltes Profil auswählen
            foreach (TabPage tp in tabProfiles.TabPages)
            {
                if (((Profile)tp).Created & !((Profile)tp).IsAddProfileTab)
                {
                    tabProfiles.SelectedTab = tp;
                }
            }
        }
    }
}
