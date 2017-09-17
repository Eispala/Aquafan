using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AquaFan
{
    public partial class frmChangeProfileName : Form
    {
        #region Variablen
        private string sNewName;

        public string NewProfileName
        {
            get { return sNewName; }
            set { sNewName = value; }
        }

        private Controller _ParentControllerObject;

        public Controller ParentControllerObject
        {
            get { return _ParentControllerObject; }
            set { _ParentControllerObject = value; }
        } 
        #endregion

        public frmChangeProfileName(Controller cntrl, string sOldProfileName)
        {
            InitializeComponent();
            _ParentControllerObject = cntrl;
            tbNewProfileName.Text = sOldProfileName;
        }

        private void btnAcceptNewProfileName_Click(object sender, EventArgs e)
        {
            sNewName = tbNewProfileName.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            btnAcceptNewProfileName.Enabled = !string.IsNullOrEmpty(((TextBox)sender).Text.Trim());
        }

        private void frmChangeProfileName_Load(object sender, EventArgs e)
        {
            ParentControllerObject.CurrentForm = this;
            ParentControllerObject.LanguageControllerObject.collectControls(this);
            ParentControllerObject.LanguageControllerObject.changeLanguage(ParentControllerObject.LanguageControllerObject.CurrentLanguage);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
