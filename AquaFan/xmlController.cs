using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Windows.Forms;

using System.Drawing;

namespace AquaFan
{
    public class xmlController
    {
        private Controller parentController;

        public Controller ParentControllerObject
        {
            get { return parentController; }
            set { parentController = value; }
        }

        public string CmdPath
        {
            get { return sCmdPath; }
            set { sCmdPath = value; }
        }

        string sProgramConfigPath = "Config\\ProgramConfig.xml";
        string sProgramConfigRoot = "ProgramConfig";
        string sFanConfigRoot = "FanConfig";
        string sXmlHash;
        string sCmdPath;

        XmlNode xmlNodeCmdPath;
        XmlNode xmlNodeLanguage;
        XmlNode xmlNodeDeviceSerial;
        XmlNode xmlNodeChangeFanSpeedsByActiveProfile;
        XmlNode xmlNodeFan;
        XmlNodeList xmlNodeListTranslations;
        XmlNode xmlNodeApplyAtStart;
        XmlNode xmlNodeStartMinimized;

        XmlDocument xmlDocProgramConfig = new XmlDocument();
        XmlDocument xmlDocLanguage = new XmlDocument();
        XmlDocument xmlDocFanConfig = new XmlDocument();

        public xmlController(Controller prntController)
        {
            ParentControllerObject = prntController;
            xmlDocProgramConfig.Load(parentController.GetApplicationPath() + "\\" + sProgramConfigPath);
            sXmlHash = xmlDocProgramConfig.DocumentElement.Attributes["identifier"].Value.ToString();

            xmlNodeCmdPath = xmlDocProgramConfig.SelectSingleNode(sProgramConfigRoot + "//CommandLineConfig//AquaComputerCmdPath");
            if (xmlNodeCmdPath == null)
            {
                //Message
            }
            else
            {
                sCmdPath = xmlNodeCmdPath.Attributes["value"].Value;
            }

            DeviceSerial();
        }

        public bool checkCmdPath
        {
            get
            {
                return File.Exists(xmlNodeCmdPath.Attributes["value"].Value);
            }
        }

        /// <summary>
        /// Deaktiviert das übergebene Profil auf Dateiebene
        /// </summary>
        /// <param name="p"></param>
        public void setProfileInactive(Profile p)
        {
            if(File.Exists(p.ProfilePath))
            {
                xmlDocFanConfig.RemoveAll();
                xmlDocFanConfig.Load(p.ProfilePath);

                xmlDocFanConfig.DocumentElement.Attributes["active"].Value = "False";
                xmlDocFanConfig.Save(p.ProfilePath);
            }
        }

        /// <summary>
        /// Liest die Texte fuer eine gegebene Sprache
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Dictionary<string, string> readTranslationForLanguage(string path)
        {
            Dictionary<string, string> dReturn = new Dictionary<string, string>();

            if (File.Exists(path))
            {
                xmlDocLanguage.RemoveAll();
                xmlDocLanguage.Load(path);
                xmlNodeListTranslations = xmlDocLanguage.SelectNodes("Language//LanguageNode");

                if (xmlNodeListTranslations == null) { return dReturn; }

                foreach (XmlNode node in xmlNodeListTranslations)
                {
                    dReturn.Add(node.Attributes["controlName"].Value, node.Attributes["text"].Value);
                }
            }

            return dReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void setApplyChangesAtActiveProfile(bool value)
        {
            xmlNodeApplyAtStart = xmlDocProgramConfig.SelectSingleNode(sProgramConfigRoot + "//ApplyActiveProfileAtProgramStart");
            if(xmlNodeApplyAtStart != null)
            {
                xmlNodeApplyAtStart.Attributes["value"].Value = value.ToString();
                xmlDocProgramConfig.Save(parentController.GetApplicationPath() + "\\" + sProgramConfigPath);
            }
        }

        public void setStartMinimizedValue(bool value)
        {
            xmlNodeStartMinimized = xmlDocProgramConfig.SelectSingleNode(sProgramConfigRoot + "//StartMinimized");
            if(xmlNodeStartMinimized != null)
            {
                xmlNodeStartMinimized.Attributes["value"].Value = value.ToString();
                xmlDocProgramConfig.Save(parentController.GetApplicationPath() + "\\" + sProgramConfigPath);
            }
        }

        /// <summary>
        /// Gibt an ob das Programm minimiert gestartet werden soll
        /// </summary>
        public bool StartMinimizedValue
        {
            get
            {
                xmlNodeStartMinimized = xmlDocProgramConfig.SelectSingleNode(sProgramConfigRoot + "//StartMinimized");
                if (xmlNodeStartMinimized != null)
                {
                    return Convert.ToBoolean(xmlNodeStartMinimized.Attributes["value"].Value);
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gibt an ob das Aktive Profil bei Programmstart direkt übernommen werden soll
        /// </summary>
        public bool ApplyValuesAtProgramStart
        {
            get
            {
                xmlNodeApplyAtStart = xmlDocProgramConfig.SelectSingleNode(sProgramConfigRoot + "//ApplyActiveProfileAtProgramStart");
                if (xmlNodeApplyAtStart != null)
                {
                    return Convert.ToBoolean(xmlNodeApplyAtStart.Attributes["value"].Value);
                }

                return false;
            }
        }

        /// <summary>
        /// Gibt die Sprache beim Starten des Programmes zurueck
        /// </summary>
        public string DefaultLanguage
        {
            get
            {
                xmlNodeLanguage = xmlDocProgramConfig.SelectSingleNode(sProgramConfigRoot + "//Language");

                if (xmlNodeLanguage == null) { return "XX"; }

                return xmlNodeLanguage.Attributes["selectedLanguage"].Value;
            }
        }

        /// <summary>
        /// Speichert die Sprache die beim Starten des Programmes benutzt wird
        /// </summary>
        /// <param name="language"></param>
        public void setDefaultLanguage(string language)
        {
            xmlNodeLanguage = xmlDocProgramConfig.SelectSingleNode(sProgramConfigRoot + "//Language");
            if (xmlNodeLanguage == null)
            {
                //message
                return;
            }

            xmlNodeLanguage.Attributes["selectedLanguage"].Value = language;
            xmlDocProgramConfig.Save(parentController.GetApplicationPath() + "\\" + sProgramConfigPath);
        }

        /// <summary>
        /// Speichert den Pfad der AquaComputerCmd.exe
        /// </summary>
        /// <param name="path"></param>
        public void setAquacomputerCmd(string path)
        {
            xmlNodeCmdPath.Attributes["value"].Value = path;

            xmlDocProgramConfig.Save(parentController.GetApplicationPath() + "\\" + sProgramConfigPath);
            CmdPath = path;
        }

        /// <summary>
        /// Liest die Aquaero Seriennummer aus der Konfigurationsdatei
        /// </summary>
        /// <returns></returns>
        public string DeviceSerial()
        {
            xmlNodeDeviceSerial = xmlDocProgramConfig.SelectSingleNode(sProgramConfigRoot + "//DeviceSerial");
            if (xmlNodeDeviceSerial.Attributes["value"].Value == "")
            {
                //Message
            }

            ParentControllerObject.DeviceSerial = xmlNodeDeviceSerial.Attributes["value"].Value;

            return xmlNodeDeviceSerial.Attributes["value"].Value;
            
        }

        /// <summary>
        /// Speichert die Aaquaero Seriennummer in die Konfigurationsdatei
        /// </summary>
        /// <param name="serial"></param>
        public void setDeviceSerial(string serial)
        {
            xmlNodeDeviceSerial = xmlDocProgramConfig.SelectSingleNode(sProgramConfigRoot + "//DeviceSerial");
            if (xmlNodeDeviceSerial == null)
            {
                //message
                return;
            }

            xmlNodeDeviceSerial.Attributes["value"].Value = serial;
            ParentControllerObject.DeviceSerial = serial;
            xmlDocProgramConfig.Save(parentController.GetApplicationPath() + "\\" + sProgramConfigPath);

        }

        /// <summary>
        /// Gibt zurück ob Änderungen an den Reglern erst beim Speichern übernommen werden, oder sofort
        /// </summary>
        /// <returns></returns>
        public bool changeFanSpeedsByChangingProfile
        {
            get
            {
                xmlNodeChangeFanSpeedsByActiveProfile = xmlDocProgramConfig.SelectSingleNode(sProgramConfigRoot + "//ChangeFanSpeedsByActiveProfile");

                if (xmlNodeChangeFanSpeedsByActiveProfile == null)
                {
                    //message
                    return false;
                }

                return Convert.ToBoolean(xmlNodeChangeFanSpeedsByActiveProfile.Attributes["value"].Value);
            }
        }

        /// <summary>
        /// Speichert ob Änderungen an den Reglern erst beim Speichern oder sofort übernommen werden sollen
        /// </summary>
        /// <param name="value"></param>
        public void setChangeFanSpeedsByAciveProfile(bool value)
        {
            if (xmlNodeChangeFanSpeedsByActiveProfile == null)
            {
                //Message
                return;
            }

            xmlNodeChangeFanSpeedsByActiveProfile.Attributes["value"].Value = value.ToString();
            xmlDocProgramConfig.Save(parentController.GetApplicationPath() + "\\" + sProgramConfigPath);
        }

        /// <summary>
        /// Fan Settings Speichern - Geschwindigkeit, Beschreibung
        /// Die Liste kann jedes mal komplett neu aufgebaut werden, da nur 12 Datensaetze gespeichert werden
        /// </summary>
        /// <param name="lFans"></param>
        public void saveProfile(Profile profileToSave)
        {
            if (xmlDocFanConfig != null)
            {
                xmlDocFanConfig.RemoveAll();
            }
            else
            {
                xmlDocFanConfig = new XmlDocument();
            }

            xmlDocFanConfig.AppendChild(xmlDocFanConfig.CreateElement(sFanConfigRoot));
            xmlDocFanConfig.DocumentElement.Attributes.Append(xmlDocFanConfig.CreateAttribute("active"));
            xmlDocFanConfig.DocumentElement.Attributes.Append(xmlDocFanConfig.CreateAttribute("profileName"));
            xmlDocFanConfig.DocumentElement.Attributes.Append(xmlDocFanConfig.CreateAttribute("startboost"));
            xmlDocFanConfig.DocumentElement.Attributes["active"].Value = profileToSave.IsActiveProfile.ToString();
            xmlDocFanConfig.DocumentElement.Attributes["profileName"].Value = profileToSave.Text;
            xmlDocFanConfig.DocumentElement.Attributes["startboost"].Value = profileToSave.StartBoost.ToString();

            foreach (fan fToSave in profileToSave.ProfileFans)
            {
                xmlNodeFan = xmlDocFanConfig.CreateElement("fan");
                xmlNodeFan.Attributes.Append(xmlDocFanConfig.CreateAttribute("name"));
                xmlNodeFan.Attributes.Append(xmlDocFanConfig.CreateAttribute("speed"));
                xmlNodeFan.Attributes.Append(xmlDocFanConfig.CreateAttribute("description"));

                xmlNodeFan.Attributes["name"].Value = fToSave.Name;
                xmlNodeFan.Attributes["speed"].Value = fToSave.SpeedPercentage.ToString();
                xmlNodeFan.Attributes["description"].Value = fToSave.Description;

                fToSave.Updated = false;

                xmlDocFanConfig.DocumentElement.AppendChild(xmlNodeFan);
            }

            xmlDocFanConfig.Save(profileToSave.ProfilePath);
        }

        /// <summary>
        /// Gibt eine Liste mit allen vorhandenen Profilen zurueck
        /// </summary>
        /// <returns></returns>
        public BindingList<Profile> loadProfiles()
        {

            BindingList<Profile> lReturningProfiles = new BindingList<Profile>();

            foreach (string sProfilePath in Directory.GetFiles(ParentControllerObject.GetApplicationPath() + "\\FanProfiles"))
            {
                xmlDocFanConfig.RemoveAll();
                xmlDocFanConfig.Load(sProfilePath);


                Profile p = new Profile(xmlDocFanConfig.DocumentElement.Attributes["profileName"].Value, ParentControllerObject);
                if (xmlDocFanConfig.DocumentElement.Attributes["startboost"] == null)
                {
                    p.ProfileCheckBoxStartBoost.Checked = false;
                }
                else
                {
                    p.ProfileCheckBoxStartBoost.Checked = Convert.ToBoolean(xmlDocFanConfig.DocumentElement.Attributes["startboost"].Value);
                }

                p.ProfilePath = sProfilePath;

                p.IsActiveProfile = Convert.ToBoolean(xmlDocFanConfig.DocumentElement.Attributes["active"].Value);
                p.ProfileRadioButton.Checked = p.IsActiveProfile;

                if (p.IsActiveProfile)
                {
                    ParentControllerObject.CurrentProfile = p;
                }

                BindingList<fan> lFans = loadFanDataFromFile(sProfilePath);
                p.ProfileFans = lFans;
                lReturningProfiles.Add(p);
                
            }

            Profile addProfile = new Profile("+ (Strg + T)", ParentControllerObject);
            addProfile.ProfileFans = ParentControllerObject.GenerateFans();
            addProfile.ProfileCurrentFan = addProfile.ProfileFans[0];
            addProfile.ProfileLabel.Text = ParentControllerObject.GetCurrentSpeedText(addProfile.ProfileCurrentFan.SpeedPercentage);
            addProfile.IsAddProfileTab = true;

            

            lReturningProfiles.Add(addProfile);
            return lReturningProfiles;
        }

        /// <summary>
        /// Liest die Fans aus der gegebenen Datei
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private BindingList<fan> loadFanDataFromFile(string file)
        {
            BindingList<fan> lReturn = new BindingList<fan>();

            xmlDocFanConfig.RemoveAll();
            xmlDocFanConfig.Load(file);

            foreach (XmlNode xmlNodeFan in xmlDocFanConfig.DocumentElement)
            {
                fan f = new fan(xmlNodeFan.Attributes["name"].Value, ParentControllerObject);
                f.SpeedPercentage = Convert.ToInt32(xmlNodeFan.Attributes["speed"].Value);
                f.Description = xmlNodeFan.Attributes["description"].Value;
                lReturn.Add(f);
            }

            return lReturn;
        }
    }
}
