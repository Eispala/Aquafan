using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.IO;

using System.ComponentModel;
using System.Diagnostics;

using System.Runtime.InteropServices;

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
        private string sStartArguments = "";


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

        //Aktuelles Menustrip
        private MenuStrip menuStripCurrent;

        public MenuStrip CurrentMenuStrip
        {
            get { return menuStripCurrent; }
            set { menuStripCurrent = value; }
        }

        //Aktuell aktive Form
        private Form frmCurrent;

        public Form CurrentForm
        {
            get { return frmCurrent; }
            set { frmCurrent = value; }
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

        public Controller(ToolStripStatusLabel _lblStatus, ToolStripMenuItem _languageMenuItem, Form _frmMain, StatusStrip _statusStrip, MenuStrip _menuStrip, Button acceptButton, TabControl tbCntrl)
        {
            lblStatus = _lblStatus;
            statusStrip = _statusStrip;
            menuStripCurrent = _menuStrip;
            frmCurrent = _frmMain;
            btnAcceptFrmMain = acceptButton;
            ProfileTabControl = tbCntrl;

            //Config Dateien prüfen
            MissingFileController fileCheck = new MissingFileController();

            //Controller Objekte instanziieren
            xmlCntrl = new xmlController(this);
            lngCntrl = new LanguageController(this, XmlControllerObject, _languageMenuItem, _frmMain, menuStripCurrent);

            XmlControllerObject.DeviceSerial();

            setStatus();

            //Lüfter Werte ändern sobald ein Profil aktiviert wird (True = Lüfter werden sofort übernommen, False = man muss auf "Übernehmen" klicken
            bApplyChangesWhenChangingProfile = XmlControllerObject.changeFanSpeedsByChangingProfile;

            loadProfiles();
            showProfiles();

            if (XmlControllerObject.ApplyValuesAtProgramStart)
            {
                applyCurrentProfileChanges();
            }


        }

        /// <summary>
        /// Setzt den Statustext auf der Hauptform
        /// </summary>
        public void setStatus()
        {
            if (!XmlControllerObject.checkCmdPath)
            {
                lblStatus.Text = lngCntrl.getVariableText(lngCntrl.CurrentLanguage, "varCmdNotOK").Trim();
                statusStrip.BackColor = Color.Orange;
                btnAcceptFrmMain.Enabled = false;
                return;
            }

            if (string.IsNullOrEmpty(DeviceSerial))
            {
                lblStatus.Text += " " + lngCntrl.getVariableText(lngCntrl.CurrentLanguage, "varSerialNotOK").Trim();
                statusStrip.BackColor = Color.Orange;
                btnAcceptFrmMain.Enabled = false;
                return;
            }
            else
            {
                lblStatus.Text = lngCntrl.getVariableText(lngCntrl.CurrentLanguage, "varCmdOK");
                statusStrip.BackColor = Color.LightGreen;
                btnAcceptFrmMain.Enabled = true;
            }


        }

        /// <summary>
        /// Form zum setzen des Pfades des AquaComputerCmd.exe und der Standardsprache
        /// </summary>
        public void configureBaseSettings()
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
        public string setAquacomputerCmdPath(string currentSelectedPath)
        {
            if (ofd == null)
            {
                ofd = new OpenFileDialog();
            }

            ofd.Title = LanguageControllerObject.getVariableText(LanguageControllerObject.CurrentLanguage, "varOpenFileDialogTitle");
            ofd.Filter = LanguageControllerObject.getVariableText(LanguageControllerObject.CurrentLanguage, "varOpenFileDialogFilter");

            ofd.ShowDialog();

            if (File.Exists(ofd.FileName))
            {
                return ofd.FileName;
            }

            sMessageHeader = LanguageControllerObject.getVariableText(LanguageControllerObject.CurrentLanguage, "varMessageHeader");
            sMessageContent = LanguageControllerObject.getVariableText(LanguageControllerObject.CurrentLanguage, "varCmdNotExist");

            MessageBox.Show(sMessageContent, sMessageHeader, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            return currentSelectedPath;
        }

        /// <summary>
        /// Gibt eine Liste mit 12 Fans zurück
        /// </summary>
        /// <returns></returns>
        public BindingList<fan> generateFans()
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
        public BindingList<Profile> loadProfiles()
        {
            profiles = xmlCntrl.loadProfiles();
            return profiles;
        }

        /// <summary>
        /// Gibt eine Zeichenfolge mit Geschwindigkeit des aktuell gewählten Lüfter zurück, in der aktuellen Sprache
        /// </summary>
        /// <param name="speedValue"></param>
        /// <returns></returns>
        public string getCurrentSpeedText(int speedValue)
        {
            return LanguageControllerObject.getVariableText(LanguageControllerObject.CurrentLanguage, "lblCurrentDescription").Replace("[]", speedValue.ToString());
        }

        /// <summary>
        /// Speichert das aktive Profil
        /// </summary>
        public void saveActiveProfile()
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
                showProfiles();
            }

            checkForActiveProfile();
        }

        /// <summary>
        /// Speichert alle Profile
        /// </summary>
        public void saveAllProfiles()
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

            checkForActiveProfile();
        }

        private Process pApplyChanges = new Process();
        private ProcessStartInfo pApplyChangesStartInfo = new ProcessStartInfo();
        private System.Threading.Thread tStartBoost;
        private System.Timers.Timer tStartBoostTimer;
        /// <summary>
        /// Aktiviert die Lüftereinstellungen
        /// </summary>
        public void applyCurrentProfileChanges()
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
                applyFanSpeeds(getStartArguments(false));
                tStartBoost = new System.Threading.Thread(applyFanSettingsDelayed);
                tStartBoost.Start();
            }
            else
            {
                applyFanSpeeds(getStartArguments(true));
            }
        }

        private void applyFanSettingsDelayed()
        {
            tStartBoostTimer.Interval = 2500;
            tStartBoostTimer.Start();
        }

        /// <summary>
        /// Gibt die Fan-Settings zum Starten der aquaComputerCmd.exe zurück (True wenn die Werte aus dem Profil verwendet wrden sollen, False wenn der Startboost von 100% lueftergeschwindigkeit aktiviert ist)
        /// </summary>
        /// <param name="bCurrent"></param>
        /// <returns></returns>
        private string getStartArguments(bool bCurrent)
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
            applyFanSpeeds(getStartArguments(true));

            //Danach den Timer einfach stoppen
            ((System.Timers.Timer)sender).Stop();
        }

        Process pSetFanSpeeds = new Process();
        int iProcessCounter = 0;

        /// <summary>
        /// Aktivierte die übergebenen Geschwindigkeiten für die Lüfter
        /// </summary>
        /// <param name="startArguments"></param>
        private void applyFanSpeeds(string startArguments)
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
                lblStatus.Text = lngCntrl.getVariableText(lngCntrl.CurrentLanguage, "varFanSpeedSet");
                statusStrip.BackColor = Color.LightGreen;
                btnAcceptFrmMain.Enabled = true;
                tTest.Start();
            }
        }

        private void TTest_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lblStatus.Text = lngCntrl.getVariableText(lngCntrl.CurrentLanguage, "");
            statusStrip.BackColor = Color.LightGreen;
            btnAcceptFrmMain.Enabled = true;
            tTest.Stop();
        }

        /// <summary>
        /// Erstellt ein leeres Profil
        /// </summary>
        public bool createProfile()
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
            newProfile.ProfileFans = generateFans();
            newProfile.ProfileCurrentFan = newProfile.ProfileFans[0];
            newProfile.ProfileLabel.Text = getCurrentSpeedText(newProfile.ProfileCurrentFan.SpeedPercentage);
            newProfile.ProfileIsCreated = true;
            iFileNameCounter++;
            newProfile.ProfilePath = "FanProfiles\\" + DateTime.Now.Date.ToShortDateString() + "_" + iFileNameCounter.ToString() + ".xml";

            Profiles.Add(newProfile);
            bActiveProfileExists = false;

            checkForActiveProfile();

            return true;
        }

        private void checkForActiveProfile()
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

        public void createProfileWrapper(Form f)
        {
            if (createProfile())
            {
                CurrentForm = f;
                showProfiles();
            }
            else
            {
                reloadCurrentLanguage(f);
            }
        }

        /// <summary>
        /// Zeigt alle Profile in der Profiles-Liste an
        /// </summary>
        public void showProfiles()
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
                        p.ProfileLabel.Text = getCurrentSpeedText(p.ProfileCurrentFan.SpeedPercentage);
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
                        //profileControl.SelectedTab = p;
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

            //Sprache aendern nachdem alle Controls hinzugefuegt wurden
            LanguageControllerObject.collectControls(null, menuStripCurrent);
            LanguageControllerObject.changeLanguage(LanguageControllerObject.CurrentLanguage);
        }

        public void reloadCurrentLanguage(Form f)
        {
            //Sprache aendern nachdem alle Controls hinzugefuegt wurden
            LanguageControllerObject.collectControls(f, menuStripCurrent);
            LanguageControllerObject.changeLanguage(LanguageControllerObject.CurrentLanguage);
        }

        /// <summary>
        /// Fenster zum Profilnamen ändern öffnen
        /// </summary>
        public void changeProfileName()
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
        public void activateProfile(Profile profileToActivate)
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
                saveActiveProfile();
                applyCurrentProfileChanges();
            }
        }

        /// <summary>
        /// Resized die aktuelle Form auf den Normal State
        /// </summary>
        public void restoreFormSize()
        {
            if (CurrentForm.WindowState == FormWindowState.Normal | CurrentForm.WindowState == FormWindowState.Minimized)
            {
                CurrentForm.ShowInTaskbar = true;

                if (CurrentForm.WindowState == FormWindowState.Minimized)
                {
                    ShowWindow(CurrentForm.Handle, SW_RESTORE);
                }
            }
        }
    }
}
