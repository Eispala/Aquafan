using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.IO;

using System.ComponentModel;
using System.Diagnostics;

using System.Runtime.InteropServices;

using Microsoft.Win32;



namespace AquaFan
{
    public class Controller
    {
        
        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, uint Msg);
        private const uint SW_RESTORE = 0x09;

        //Liste in der Fans generiert werden
        private BindingList<fan> lGenerateFans = new BindingList<fan>();
        private frmConfiguration frmConfig;
        private frmChangeProfileName fChangeProfileName;
        private OpenFileDialog ofd;


        private int iFileNameCounter = 0;
        private bool bActiveProfileExists = false;
        private Profile pAddTabProfile;

        private string sMessageHeader;
        private string sMessageContent;


        #region Properties
        private string sDeviceSerial;

        public string DeviceSerial
        {
            get { return sDeviceSerial; }
            set { sDeviceSerial = value; }
        }


        private Profile currentProfile;

        public Profile CurrentProfile
        {
            get { return currentProfile; }
            set
            {
                currentProfile = value;

                /* Wenn die Fan-Liste nicht null ist, und fans vorhanden sind, prüfen ob ein fan selektiert ist.
                 * Falls kein fan selektiert ist (-1) den obersten selektieren
                 * Durch die && Verknüpfung werden die beiden letzen Prüfungen nur durchgeführt wenn CurrentProfile.ProfileFans nicht null ist.
                 */
                if (CurrentProfile.ProfileFans != null && CurrentProfile.ProfileFans.Count > 0 && CurrentProfile.ProfileComboBox.SelectedIndex == -1)
                {
                    CurrentProfile.ProfileComboBox.SelectedIndex = 0;
                }
            }
        }

        //Aktuelles Statuslabel
        ToolStripStatusLabel lblStatus;

        public ToolStripStatusLabel LblStatus
        {
            get { return lblStatus; }
            set { lblStatus = value; }
        }

        //Aktuelles Statusstrip
        private StatusStrip statusStrip;

        public StatusStrip StatusStrip
        {
            get { return statusStrip; }
            set { statusStrip = value; }
        }

        //Liste mit allen Profilen (TabPages)
        BindingList<Profile> profiles;

        public BindingList<Profile> Profiles
        {
            get { return profiles; }
            set { profiles = value; }
        }

        //Speichert ob beim ändern des aktiven Profils die Fan settings sofort übernommen werden sollen oder nicht
        private bool bApplyChangesWhenChangingProfile;

        public bool ApplyChangesWhenChangingActiveProfile
        {
            get { return bApplyChangesWhenChangingProfile; }
            set { bApplyChangesWhenChangingProfile = value; }
        }

        //Accept Button der Hauptform
        private Button btnAcceptFrmMain;

        public Button AcceptButtonFrmMain
        {
            get { return btnAcceptFrmMain; }
            set { btnAcceptFrmMain = value; }
        }

        //TabControl auf der Hauptform
        private TabControl profileControl;

        public TabControl ProfileTabControl
        {
            get { return profileControl; }
            set { profileControl = value; }
        }


        //Xml Controller objekt
        private xmlController xmlCntrl;

        public xmlController XmlControllerObject
        {
            get
            { return xmlCntrl; }
        }

        //Languagecontroler objekt
        private LanguageController lngCntrl;

        public LanguageController LanguageControllerObject
        {
            get { return lngCntrl; }
            set { lngCntrl = value; }
        }

        //Gibt den auf dem aktuell gewählten Profil selektierten Fan/Lüfter zurück
        public fan SelectedFan
        {
            get { return CurrentProfile.ProfileCurrentFan; }
            set { CurrentProfile.ProfileCurrentFan = value; }
        }

        #endregion

        public Controller(ToolStripStatusLabel _lblStatus, ToolStripMenuItem _languageMenuItem, StatusStrip _statusStrip, Button acceptButton, TabControl tbCntrl)
        {
            lblStatus = _lblStatus;
            statusStrip = _statusStrip;
            btnAcceptFrmMain = acceptButton;
            ProfileTabControl = tbCntrl;

            //Config Dateien prüfen
            MissingFileController fileCheck = new MissingFileController(this);

            //Controller Objekte instanziieren
            xmlCntrl = new xmlController(this);
            lngCntrl = new LanguageController(this, XmlControllerObject, _languageMenuItem);

            XmlControllerObject.DeviceSerial();

            SetStatus();

            //Lüfter Werte ändern sobald ein Profil aktiviert wird (True = Lüfter werden sofort übernommen, False = man muss auf "Übernehmen" klicken
            bApplyChangesWhenChangingProfile = XmlControllerObject.changeFanSpeedsByChangingProfile;

            LoadProfiles();

            if (XmlControllerObject.ApplyValuesAtProgramStart)
            {
                ApplyCurrentProfileChanges();
            }
        }        

        public void SetStartWithWindows(bool autostart)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if(autostart & StartWithWindowsEnabled() != true)
            {
                registryKey.SetValue("Aquafan", Application.ExecutablePath);
            }
            else if(!autostart)
            {
                registryKey.DeleteValue("Aquafan", false);
            }            
        }

        public bool StartWithWindowsEnabled()
        {
            return Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false).GetValue("Aquafan") != null;
        }

        public string GetApplicationPath()
        {
            string x = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false).GetValue("Aquafan").ToString();
            return Path.GetDirectoryName(x);
        }

        /// <summary>
        /// Setzt den Statustext auf der Hauptform
        /// </summary>
        public void SetStatus()
        {
            if (!XmlControllerObject.checkCmdPath)
            {
                lblStatus.Text = lngCntrl.GetVariableText(lngCntrl.CurrentLanguage, "varCmdNotOK").Trim();
                statusStrip.BackColor = Color.Orange;
                btnAcceptFrmMain.Enabled = false;
                return;
            }

            if (string.IsNullOrEmpty(DeviceSerial))
            {
                lblStatus.Text += " " + lngCntrl.GetVariableText(lngCntrl.CurrentLanguage, "varSerialNotOK").Trim();
                statusStrip.BackColor = Color.Orange;
                btnAcceptFrmMain.Enabled = false;
                return;
            }
            else
            {
                lblStatus.Text = lngCntrl.GetVariableText(lngCntrl.CurrentLanguage, "varCmdOK");
                statusStrip.BackColor = Color.LightGreen;
                btnAcceptFrmMain.Enabled = true;
            }


        }

        /// <summary>
        /// Form zum setzen des Pfades des AquaComputerCmd.exe und der Standardsprache
        /// </summary>
        public void ConfigureBaseSettings()
        {
            if (frmConfig == null)
            {
                frmConfig = new frmConfiguration();
            }

            frmConfig.ControllerObject = this;
            frmConfig.ShowDialog();
        }

        /// <summary>
        /// OpenFileDialog zum suchen der AquaComputerCmd.exe anzeigen und Wert den angegebenen Pfad zurück geben
        /// </summary>
        /// <param name="currentSelectedPath"></param>
        /// <returns></returns>
        public string SetAquacomputerCMDPath(string currentSelectedPath)
        {
            if (ofd == null)
            {
                ofd = new OpenFileDialog();
            }

            ofd.Title = LanguageControllerObject.GetVariableText(LanguageControllerObject.CurrentLanguage, "varOpenFileDialogTitle");
            ofd.Filter = LanguageControllerObject.GetVariableText(LanguageControllerObject.CurrentLanguage, "varOpenFileDialogFilter");

            ofd.ShowDialog();

            if (File.Exists(ofd.FileName))
            {
                return ofd.FileName;
            }

            sMessageHeader = LanguageControllerObject.GetVariableText(LanguageControllerObject.CurrentLanguage, "varMessageHeader");
            sMessageContent = LanguageControllerObject.GetVariableText(LanguageControllerObject.CurrentLanguage, "varCmdNotExist");

            MessageBox.Show(sMessageContent, sMessageHeader, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            return currentSelectedPath;
        }

        /// <summary>
        /// Gibt eine Liste mit 12 Fans zurück
        /// </summary>
        /// <returns></returns>
        public BindingList<fan> GenerateFans()
        {
            lGenerateFans = new BindingList<fan>();

            lGenerateFans.AllowNew = true;
            lGenerateFans.AllowEdit = true;
            lGenerateFans.AllowRemove = false;

            for (int i = 1; i < 13; ++i)
            {
                lGenerateFans.Add(new fan("fan" + i.ToString(), this));
            }

            return lGenerateFans;
        }

        /// <summary>
        /// Lädt alle Profile
        /// </summary>
        /// <returns></returns>
        public BindingList<Profile> LoadProfiles()
        {
            profiles = xmlCntrl.loadProfiles();
            return profiles;
        }

        /// <summary>
        /// Gibt eine Zeichenfolge mit Geschwindigkeit des aktuell gewählten Lüfter zurück, in der aktuellen Sprache
        /// </summary>
        /// <param name="speedValue"></param>
        /// <returns></returns>
        public string GetCurrentSpeedText(int speedValue)
        {
            return LanguageControllerObject.GetVariableText(LanguageControllerObject.CurrentLanguage, "lblCurrentDescription").Replace("[]", speedValue.ToString());
        }

        /// <summary>
        /// Speichert das aktive Profil
        /// </summary>
        public void SaveActiveProfile()
        {
            BindingList<Profile> lProfilesToDelete = new BindingList<Profile>();

            foreach (Profile p in Profiles)
            {
                if (p.IsAddProfileTab) { continue; }
                if (p.IsActiveProfile)
                {
                    if (p.DeleteProfile) { lProfilesToDelete.Add(p); }

                    XmlControllerObject.saveProfile(p);
                }
                else
                {
                    XmlControllerObject.setProfileInactive(p);
                }
            }

            foreach (Profile p in lProfilesToDelete)
            {
                Profiles.Remove(p);
                ShowProfiles();
            }

            CheckForActiveProfile();
        }

        /// <summary>
        /// Speichert alle Profile
        /// </summary>
        public void SaveAllProfiles()
        {
            BindingList<Profile> lProfilesToDelete = new BindingList<Profile>();
            foreach (Profile p in Profiles)
            {
                if (p.IsAddProfileTab) { continue; }
                if (p.DeleteProfile)
                {
                    lProfilesToDelete.Add(p);
                    File.Delete(p.ProfilePath);
                }
                else
                {
                    XmlControllerObject.saveProfile(p);
                }
            }

            foreach (Profile p in lProfilesToDelete)
            {
                Profiles.Remove(p);
            }

            CheckForActiveProfile();
        }

        private Process pApplyChanges = new Process();
        private ProcessStartInfo pApplyChangesStartInfo = new ProcessStartInfo();
        private System.Threading.Thread tStartBoost;
        private System.Timers.Timer tStartBoostTimer;
        /// <summary>
        /// Aktiviert die Lüftereinstellungen
        /// </summary>
        public void ApplyCurrentProfileChanges()
        {
            if (CurrentProfile == null || CurrentProfile.ProfileFans == null) { return; }
            if (!File.Exists(xmlCntrl.CmdPath)) { return; }

            tStartBoostTimer = new System.Timers.Timer();
            tStartBoostTimer.Elapsed += TStartBoostTimer_Elapsed;
            pApplyChangesStartInfo = new ProcessStartInfo();
            pApplyChangesStartInfo.FileName = xmlCntrl.CmdPath;


            //Wenn der Startboost aktiviert ist, muessen ersteinmal alle Luefter auf 100% gestellt werden
            if (CurrentProfile.StartBoost)
            {
                ApplyFanSpeeds(GetStartArguments(false));
                tStartBoost = new System.Threading.Thread(ApplyFanSettingsDelayed);
                tStartBoost.Start();
            }
            else
            {
                ApplyFanSpeeds(GetStartArguments(true));
            }
        }

        private void ApplyFanSettingsDelayed()
        {
            tStartBoostTimer.Interval = 2500;
            tStartBoostTimer.Start();
        }

        /// <summary>
        /// Gibt die Fan-Settings zum Starten der aquaComputerCmd.exe zurück (True wenn die Werte aus dem Profil verwendet wrden sollen, False wenn der Startboost von 100% lueftergeschwindigkeit aktiviert ist)
        /// </summary>
        /// <param name="bCurrent"></param>
        /// <returns></returns>
        private string GetStartArguments(bool bCurrent)
        {
            string sReturnArguments = "";
            foreach (fan f in CurrentProfile.ProfileFans)
            {
                if (bCurrent)
                {
                    sReturnArguments += " -" + f.Name + ":" + f.SpeedPercentage.ToString();
                    iProcessCounter = 1;
                }
                else
                {
                    iProcessCounter = 2;
                    sReturnArguments += " -" + f.Name + ":" + "100";
                }
            }
            return sReturnArguments;
        }

        private void TStartBoostTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Nachdem der StartBoost Timer 1 mal getickt hat, muessen die richtigen Werte des Profils uebernommen werden
            ApplyFanSpeeds(GetStartArguments(true));

            //Danach den Timer einfach stoppen
            ((System.Timers.Timer)sender).Stop();
        }

        Process pSetFanSpeeds = new Process();
        int iProcessCounter = 0;

        /// <summary>
        /// Aktivierte die übergebenen Geschwindigkeiten für die Lüfter
        /// </summary>
        /// <param name="startArguments"></param>
        private void ApplyFanSpeeds(string startArguments)
        {
            //iProcessCounter++;
            pSetFanSpeeds = new Process();

            pApplyChangesStartInfo.Arguments = " -sn:" + xmlCntrl.DeviceSerial() + startArguments;
            pSetFanSpeeds.StartInfo = pApplyChangesStartInfo;
            pSetFanSpeeds.StartInfo.CreateNoWindow = true;
            pSetFanSpeeds.StartInfo.UseShellExecute = false;
            pSetFanSpeeds.EnableRaisingEvents = true;
            pSetFanSpeeds.Exited += P_Exited;
            pSetFanSpeeds.Start();
        }

        private void PSetFanSpeeds_Disposed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        System.Timers.Timer tTest = new System.Timers.Timer(2000);

        private void P_Exited(object sender, EventArgs e)
        {
            iProcessCounter--;
            if(iProcessCounter-- <= 0)
            {
                tTest.Elapsed -= TTest_Elapsed;
                tTest.Elapsed += TTest_Elapsed;
                lblStatus.Text = lngCntrl.GetVariableText(lngCntrl.CurrentLanguage, "varFanSpeedSet");
                statusStrip.BackColor = Color.LightGreen;
                btnAcceptFrmMain.Enabled = true;
                tTest.Start();
            }
        }

        private void TTest_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lblStatus.Text = lngCntrl.GetVariableText(lngCntrl.CurrentLanguage, "");
            statusStrip.BackColor = Color.LightGreen;
            btnAcceptFrmMain.Enabled = true;
            tTest.Stop();
        }

        /// <summary>
        /// Erstellt ein leeres Profil
        /// </summary>
        public bool CreateDefaultProfile()
        {
            foreach (Profile p in Profiles)
            {
                if (p.ProfilePath != null && Convert.ToInt32(p.ProfilePath.Substring(p.ProfilePath.Length - 5, 1)) > iFileNameCounter)
                {
                    iFileNameCounter = Convert.ToInt32(p.ProfilePath.Substring(p.ProfilePath.Length - 5, 1));
                }
            }

            foreach (Profile p in Profiles)
            {
                p.ProfileIsCreated = false;
            }

            frmNewProfile newProfileForm = new frmNewProfile(this);
            newProfileForm.ShowDialog();

            //Wenn das neue Profil nicht angelegt werden soll, dann das zuletzt gewählte aufmachen
            if (newProfileForm.DialogResult != DialogResult.OK)
            {
                if (CurrentProfile != null)
                {
                    ProfileTabControl.SelectedTab = CurrentProfile;
                    return false;
                }

                foreach (Profile p in Profiles)
                {
                    if (p.IsActiveProfile)
                    {
                        ProfileTabControl.SelectedTab = p;
                    }
                }
                return false;
            }

            Profile newProfile = new Profile(newProfileForm.ProfileName, this);
            newProfile.ProfileFans = GenerateFans();
            newProfile.ProfileCurrentFan = newProfile.ProfileFans[0];
            newProfile.ProfileLabel.Text = GetCurrentSpeedText(newProfile.ProfileCurrentFan.SpeedPercentage);
            newProfile.ProfileIsCreated = true;
            iFileNameCounter++;
            newProfile.ProfilePath = "FanProfiles\\" + DateTime.Now.Date.ToShortDateString() + "_" + iFileNameCounter.ToString() + ".xml";

            Profiles.Add(newProfile);
            bActiveProfileExists = false;

            CheckForActiveProfile();

            return true;
        }

        private void CheckForActiveProfile()
        {
            foreach (Profile p in Profiles)
            {
                if (p.IsActiveProfile)
                {
                    bActiveProfileExists = true;
                }
            }

            if (!bActiveProfileExists)
            {
                foreach (Profile p in Profiles)
                {
                    if (!p.DeleteProfile && !p.IsAddProfileTab)
                    {
                        p.IsActiveProfile = true;
                        p.ProfileRadioButton.Checked = true;
                    }
                }
            }
        }

        public void CreateFanProfile(Form f, MenuStrip menu)
        {
            CreateDefaultProfile();
            ShowProfiles();
            ReloadCurrentLanguage(f, menu);
        }

        /// <summary>
        /// Zeigt alle Profile in der Profiles-Liste an
        /// </summary>
        public void ShowProfiles()
        {
            pAddTabProfile = null;
            profileControl.TabPages.Clear();

            if (Profiles != null)
            {
                foreach (Profile p in Profiles)
                {
                    //Das Profil das zum Hinzufuegen benutzt wird soll nicht editierbar sein
                    if (p.IsAddProfileTab)
                    {
                        p.ProfileComboBox.Enabled = false;
                        p.ProfileLabel.Enabled = false;
                        p.ProfileRadioButton.Enabled = false;
                        p.ProfileTextBox.Enabled = false;
                        p.ProfileTrackBar.Enabled = false;
                        p.ProfileCheckBoxStartBoost.Enabled = false;
                        btnAcceptFrmMain.Enabled = false;
                        pAddTabProfile = p;
                        continue;
                    }

                    //Fan/Lüfter selektieren wenn keiner selektiert ist
                    if (p.ProfileComboBox.SelectedIndex == -1)
                    {
                        p.ProfileComboBox.SelectedIndex = 0;
                        p.ProfileLabel.Text = GetCurrentSpeedText(p.ProfileCurrentFan.SpeedPercentage);
                    }

                    profileControl.Controls.Add(p);
                }

                if (pAddTabProfile != null)
                {
                    profileControl.Controls.Add(pAddTabProfile);
                }

                foreach (Profile p in profileControl.TabPages)
                {
                    //Aktives Profil setzen
                    if (p.IsActiveProfile)
                    {
                        btnAcceptFrmMain.Enabled = p.IsActiveProfile;
                        p.ProfileRadioButton.Checked = true;
                    }
                    else
                    {
                        p.IsActiveProfile = false;
                        p.ProfileRadioButton.Checked = false;
                    }
                }
            }
        }

        public void ReloadCurrentLanguage(Form f, MenuStrip menu)
        {
            //Sprache aendern nachdem alle Controls hinzugefuegt wurden
            LanguageControllerObject.CollectControls(f, menu);
            LanguageControllerObject.ChangeLanguage(LanguageControllerObject.CurrentLanguage);
        }

        /// <summary>
        /// Fenster zum Profilnamen ändern öffnen
        /// </summary>
        public void ChangeProfileName()
        {
            fChangeProfileName = new frmChangeProfileName(this, CurrentProfile.Text);
            DialogResult dr = fChangeProfileName.ShowDialog();
            if (dr == DialogResult.OK)
            {
                CurrentProfile.Text = fChangeProfileName.NewProfileName;
            }
        }

        /// <summary>
        /// Aktiviert das übergebene Profil
        /// </summary>
        /// <param name="profileToActivate"></param>
        public void ActivateProfile(Profile profileToActivate)
        {
            foreach (Profile profile in Profiles)
            {
                if (profile.Text == profileToActivate.Text)
                {
                    profileToActivate.IsActiveProfile = true;
                    profileToActivate.ProfileRadioButton.Checked = true;
                    CurrentProfile = profileToActivate;
                }
                else
                {
                    profile.IsActiveProfile = false;
                    profile.ProfileRadioButton.Checked = false;
                }
            }

            if (XmlControllerObject.changeFanSpeedsByChangingProfile)
            {
                SaveActiveProfile();
                ApplyCurrentProfileChanges();
            }
        }

        /// <summary>
        /// Resized die aktuelle Form auf den Normal State
        /// </summary>
        public void RestoreFormSize(Form frm)
        {
            if (frm.WindowState == FormWindowState.Normal | frm.WindowState == FormWindowState.Minimized)
            {
                frm.ShowInTaskbar = true;

                if (frm.WindowState == FormWindowState.Minimized)
                {
                    ShowWindow(frm.Handle, SW_RESTORE);
                }
            }
        }
    }
}
