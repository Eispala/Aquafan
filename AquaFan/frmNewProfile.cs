using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AquaFan
{
    public partial class frmNewProfile : Form
    {
        public frmNewProfile(Controller _cntrl)
        {
            InitializeComponent();
            cntrl = _cntrl;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            btnCreate.Enabled = !string.IsNullOrEmpty(((TextBox)sender).Text.Trim());
        }

        private string sName;

        public string ProfileName
        {
            get { return sName; }
            set { sName = value; }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            sName = tbProfileName.Text;
        }

        private Controller cntrl;

        public Controller ParentControllerObject
        {
            get { return cntrl; }
            set { cntrl = value; }
        }

        private void frmNewProfile_Load(object sender, EventArgs e)
        {
            ParentControllerObject.CurrentForm = this;
            ParentControllerObject.LanguageControllerObject.collectControls();
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
