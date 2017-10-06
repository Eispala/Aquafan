using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Windows.Forms;

namespace AquaFan
{
    public class LanguageController
    {
        private string sLanguageFolder;

        private Controller cntrl;
        private xmlController xmlCntrl;
        private ToolStripMenuItem languageMenu;

        private List<object> lControls = new List<object>();
        private Dictionary<string, Dictionary<string, string>> dLanguages = new Dictionary<string, Dictionary<string, string>>();

        public Dictionary<string, Dictionary<string, string>> Languages
        {
            get { return dLanguages; }
            set { dLanguages = value; }
        }

        private string _currentLanguage = "";

        public string CurrentLanguage
        {
            get {
                if(_currentLanguage == "")
                {
                    _currentLanguage = xmlCntrl.DefaultLanguage;
                }
                return _currentLanguage; }
            set { _currentLanguage = value; }
        }


        public LanguageController(Controller cntrlPrnt, xmlController _xmlCntrl, ToolStripMenuItem _languageMenuItem)
        {
            sLanguageFolder = Application.StartupPath + "\\Languages\\";
            cntrl = cntrlPrnt;
            xmlCntrl = _xmlCntrl;
            languageMenu = _languageMenuItem;
            getAvailableLanguages();
        }

        /// <summary>
        /// Liest alle Sprachdateien im Programmordner ein und erstellt ein ToolStripMenuItem dafür
        /// </summary>
        private void getAvailableLanguages()
        {
            foreach (string s in Directory.GetFiles(sLanguageFolder, "lang_*.xml"))
            {
                dLanguages.Add(Path.GetFileNameWithoutExtension(s.Replace(sLanguageFolder, "")).Replace("lang_", ""), (xmlCntrl.readTranslationForLanguage(s)));

                ToolStripMenuItem languageItem = new ToolStripMenuItem(Path.GetFileNameWithoutExtension(s.Replace(sLanguageFolder, "")).Replace("lang_", ""));
                languageItem.Click += languageItem_Click;
                languageMenu.DropDownItems.Add(languageItem);
            }
        }

        public event EventHandler LanguageChanged;
        public delegate void EventHandler(string sChosenLanguage);

        void languageItem_Click(object sender, EventArgs e)
        {
            LanguageChanged(((ToolStripMenuItem)sender).Text);
            //collectControls(cntrl.CurrentForm, cntrl.CurrentMenuStrip);
            //changeLanguage(((ToolStripMenuItem)sender).Text);
            ////Status mit neuer Sprache setzen
            //cntrl.setStatus();
        }

        /// <summary>
        /// Fuegt alle Untercontrols des uebergebenen Controls in die Liste von Controls ein
        /// </summary>
        /// <param name="parentControl"></param>
        private void getControls(Control parentControl)
        {
            if(parentControl == null) { return; }
            if (!lControls.Contains(parentControl))
                lControls.Add(parentControl);

            if (parentControl.Controls.Count > 0)
            {
                foreach (Control subControl in parentControl.Controls)
                {
                    getControls(subControl);
                }
            }
        }

        /// <summary>
        /// Fuegt alle Buttons eines Menues in die Liste von Controls ein
        /// </summary>
        /// <param name="parentButton"></param>
        private void getMenuItems(ToolStripMenuItem parentButton)
        {
            if (!lControls.Contains(parentButton))
                lControls.Add(parentButton);

            if (parentButton.DropDownItems.Count > 0)
            {
                foreach (ToolStripMenuItem childButton in parentButton.DropDownItems)
                {
                    getMenuItems(childButton);
                }
            }
        }

        /// <summary>
        /// Gibt den Text zu einem Bestimmten Objekt in einer Bestimmten Sprache zurueck
        /// </summary>
        /// <param name="language"></param>
        /// <param name="variableName"></param>
        /// <returns></returns>
        public string getVariableText(string language, string variableName)
        {
            if(dLanguages.ContainsKey(language))
            {
                if(dLanguages[language].ContainsKey(variableName))
                {
                    return dLanguages[language][variableName];
                }
                else
                {
                    return "";
                    //return "Text " + variableName + " nicht gefunden";
                }
            }
            else
            {
                return "Sprache " + language + " nicht gefunden.";
            }
            
        }

        /// <summary>
        /// Fuegt alle Controls und Sub-Controls der uebergebenen Steuerelemente in die Liste der gerade dargestellten Controlls ein.
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="menu"></param>
        public void collectControls(Form frm = null, MenuStrip menu = null)
        {
            lControls.Clear();
            //if (frm == null) { getControls(cntrl.CurrentForm); }
            //else 
            //{
                getControls(frm);
            //}

            if(menu == null) { return; }

            foreach (ToolStripMenuItem menuEntry in menu.Items)
            {
                getMenuItems(menuEntry);
            }
        }

        /// <summary>
        /// Ädert die Sprache auf die Übergebene
        /// </summary>
        /// <param name="language"></param>
        public void changeLanguage(string language)
        {
            /*
             * Schleife ueber alle Controls
             * Dann Schleife ueber alle LanguageNodes der Sprachen, und Text zuweisen
             */
            _currentLanguage = language;
            if(!dLanguages.ContainsKey(language)) { return; }

            foreach (object o in lControls)
            {
                if (o is Control)
                {
                    if(dLanguages[language].ContainsKey(((Control)o).Name))
                    {
                        ((Control)o).Text = dLanguages[language][((Control)o).Name];
                    }
                }
                else if (o is ToolStripMenuItem)
                {
                    if (dLanguages[language].ContainsKey(((ToolStripMenuItem)o).Name))
                    {
                        ((ToolStripMenuItem)o).Text = dLanguages[language][((ToolStripMenuItem)o).Name];
                    }
                }
            }
            if(cntrl.CurrentProfile == null) { return; }
            cntrl.CurrentProfile.ProfileLabel.Text = cntrl.getCurrentSpeedText(cntrl.SelectedFan.SpeedPercentage);
        }
    }
}
