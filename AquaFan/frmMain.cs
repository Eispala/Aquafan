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
            cntrl = new Controller(lblStatus, tBtnLanguage, statusStrip1, btnAccept, tabProfiles);
            bLoading = false;

            if(cntrl.XmlControllerObject.StartMinimizedValue)
            {
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }

            cntrl.ShowProfiles();
            cntrl.ReloadCurrentLanguage(this, menuStrip1);
            cntrl.LanguageControllerObject.LanguageChanged += LanguageControllerObject_LanguageChanged;

            tabProfiles.SelectedTab = cntrl.CurrentProfile;
        }

        private void LanguageControllerObject_LanguageChanged(string sChosenLanguage)
        {
            cntrl.LanguageControllerObject.CurrentLanguage = sChosenLanguage;
            cntrl.ReloadCurrentLanguage(this, menuStrip1);
            cntrl.SetStatus();
        }

        private void ProfilePage_Click(object sender, EventArgs e)
        {
            
        }

        private void tBtnConfiguration_Click(object sender, EventArgs e)
        {
            cntrl.ConfigureBaseSettings();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            cntrl.SaveActiveProfile();
            cntrl.ApplyCurrentProfileChanges();
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

                cntrl.CreateFanProfile(this, menuStrip1);

            }
            else
            {
                if(((TabControl)sender).TabPages[((TabControl)sender).SelectedIndex] is Profile)
                {
                    Profile p = (Profile)((TabControl)sender).TabPages[((TabControl)sender).SelectedIndex];
                    if (p.IsActiveProfile)
                        cntrl.CurrentProfile = p;

                    btnAccept.Enabled = cntrl.CurrentProfile.IsActiveProfile;
                }
            }
        }

        private void btnSaveCurrentProfile_Click(object sender, EventArgs e)
        {
            cntrl.SaveActiveProfile();
        }

        private void btnSaveAllProfiles_Click(object sender, EventArgs e)
        {
            cntrl.SaveAllProfiles();
            cntrl.LoadProfiles();
            cntrl.ShowProfiles();
            cntrl.ReloadCurrentLanguage(this, menuStrip1);
        }        

        private void btnCreateProfile_Click(object sender, EventArgs e)
        {
            cntrl.CreateFanProfile(this, menuStrip1);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

            cntrl.CreateFanProfile(this, menuStrip1);

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch(keyData)
            {
                case Keys.Control | Keys.T:
                    cntrl.CreateFanProfile(this, menuStrip1);

                    return true;
                case Keys.Control | Keys.S:
                    cntrl.SaveAllProfiles();
                    cntrl.LoadProfiles();
                    cntrl.ShowProfiles();
                    return true;

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            btnAccept.Location = new Point(tabProfiles.Location.X + tabProfiles.Width - btnAccept.Width, tabProfiles.Location.Y + tabProfiles.Height + 6);
        }

        bool bDoubleClickedAddProfileTab = false;

        /// <summary>
        /// Zeigt den Dialog zum aendern des Profilnamens an
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            
            cntrl.ChangeProfileName();
            cntrl.ReloadCurrentLanguage(this, menuStrip1);

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
                    cntrl.ActivateProfile(p);
                    bChangedProfile = true;
                }
            }

            if(bChangedProfile)
            {
                cntrl.SaveActiveProfile();
                cntrl.ApplyCurrentProfileChanges();
            }
        }

        private void Icon_Click(object sender, EventArgs e)
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
            cmRightClick.Items.Add(cntrl.LanguageControllerObject.GetVariableText(cntrl.LanguageControllerObject.CurrentLanguage, "tBtnResize"));
            cmRightClick.Items.Add(cntrl.LanguageControllerObject.GetVariableText(cntrl.LanguageControllerObject.CurrentLanguage, "tBtnClose"));
            cmRightClick.Items[cmRightClick.Items.Count - 1].Click += NotifyIconCloseClick;
            cmRightClick.Items[cmRightClick.Items.Count - 2].Click += NotifyIconResizeClick;
        }

        private void NotifyIconResizeClick(object sender, EventArgs e)
        {
            cntrl.RestoreFormSize(this);
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
            cntrl.RestoreFormSize(this);
        }

        private void tabProfiles_Click(object sender, EventArgs e)
        {
            if(tabProfiles.TabPages.Count == 1)
            {
                //Nur das Add-Profil ist vorhanden
                cntrl.CreateFanProfile(this, menuStrip1);
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
