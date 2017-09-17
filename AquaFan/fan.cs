using System;
using System.Collections.Generic;
using System.Text;

namespace AquaFan
{
    public class fan
    {
        private Controller parentControllerObject;

        public Controller ParentControllerObject
        {
            get { return parentControllerObject; }
            set { parentControllerObject = value; }
        }

        public fan(string _name, Controller cntrl)
        {
            sName = _name;
            parentControllerObject = cntrl;
        }

        private string sName = "";

        public string Name
        {
            get { return sName; }
            set { sName = value; }
        }

        private int iSpeedPercentage = 0;

        public int SpeedPercentage
        {
            get { return iSpeedPercentage; }
            set { iSpeedPercentage = value; }
        }

        private bool bUpdatedValue;

        public bool Updated
        {
            get { return bUpdatedValue; }
            set { bUpdatedValue = value; }
        }

        private int iOldSpeed = 0;

        public int OldSpeed
        {
            get { return iOldSpeed; }
            set { iOldSpeed = value; }
        }

        private string sDescription = "";

        public string Description
        {
            get { return sDescription; }
            set { sDescription = value; }
        }


    }
}
