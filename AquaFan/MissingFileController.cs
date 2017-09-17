using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Windows.Forms;

namespace AquaFan
{
    /// <summary>
    /// Prueft ob die default Dateien vorhanden sind
    /// </summary>
    public class MissingFileController
    {
        public MissingFileController()
        {
            getMissingFiles();
        }

        string[] sProgramFiles = new string[3];
        string sErrorMessage;


        public void getMissingFiles()
        {
            //Die Nachrichten sind hardcoded, weil man sich ja nicht darauf verlassen kann dass Dateien die die Fehlermeldung enthalten vorhanden sind
            sErrorMessage = "The following default are missing:\r\n";
            
            sProgramFiles[0] = "Config\\ProgramConfig.xml";
            sProgramFiles[1] = "Languages\\lang_DE.xml";
            sProgramFiles[2] = "Languages\\lang_EN.xml";
            
            foreach(string s in sProgramFiles)
            {
                if(!File.Exists(s))
                {
                    sErrorMessage += s + "\r\n";
                }
            }

            if(sErrorMessage != "The following default are missing:\r\n")
            {
                sErrorMessage += "Please redownload the program.\r\nThe program will now exit.";
                MessageBox.Show(sErrorMessage, "AquaFan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
        }
    }
}
