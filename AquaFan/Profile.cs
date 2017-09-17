using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace AquaFan
{
    public class Profile : TabPage
    {
        private int iXOutOfGroup = 25;
        private int iXInGroup = 20;


        #region Controls
        private CheckBox chkStartBoost;

        public CheckBox ProfileCheckBoxStartBoost
        {
            get { return chkStartBoost; }
            set { chkStartBoost = value; }
        }

        RadioButton rdbReturnButton;

        public RadioButton ProfileRadioButton
        {
            get { return rdbReturnButton; }
            set { rdbReturnButton = value; }
        }

        private ComboBox cbProfileBox;

        public ComboBox ProfileComboBox
        {
            get { return cbProfileBox; }
            set { cbProfileBox = value; }
        }

        private TrackBar tProfileTrackBar;

        public TrackBar ProfileTrackBar
        {
            get { return tProfileTrackBar; }
            set { tProfileTrackBar = value; }
        }

        private Label lblProfileLabel;

        public Label ProfileLabel
        {
            get { return lblProfileLabel; }
            set { lblProfileLabel = value; }
        }

        private TextBox tbDescription;

        public TextBox ProfileTextBox
        {
            get { return tbDescription; }
            set { tbDescription = value; }
        }
        #endregion

        #region Properties
        private Controller cntrlParentControllerObject;

        public Controller ParentControllerObject
        {
            get { return cntrlParentControllerObject; }
            set { cntrlParentControllerObject = value; }
        }

        private fan pCurrentFan;

        public fan ProfileCurrentFan
        {
            get { return pCurrentFan; }
            set { pCurrentFan = value; }
        }

        private BindingList<fan> blFans;

        public BindingList<fan> ProfileFans
        {
            get { return blFans; }
            set
            {
                blFans = value;
                ProfileComboBox.Items.Clear();
                foreach (fan f in value)
                {
                    ProfileComboBox.Items.Add(f);
                }
            }
        }

        private string sProfilePath;

        public string ProfilePath
        {
            get { return sProfilePath; }
            set { sProfilePath = value; }
        }

        private bool bIsActiveProfile;

        public bool IsActiveProfile
        {
            get { return bIsActiveProfile; }
            set
            { bIsActiveProfile = value; }
        }

        private string sDescription;

        public string Description
        {
            get { return sDescription; }
            set { sDescription = value; }
        }

        private bool bDelete;

        public bool DeleteProfile
        {
            get { return bDelete; }
            set { bDelete = value; }
        }

        private bool bIsAddProfileTab;

        public bool IsAddProfileTab
        {
            get { return bIsAddProfileTab; }
            set { bIsAddProfileTab = value; }
        }

        private bool bStartBoost;

        public bool StartBoost
        {
            get { return bStartBoost; }
            set { bStartBoost = value; }
        }

        private bool bCreated;

        public bool ProfileIsCreated
        {
            get { return bCreated; }
            set { bCreated = value; }
        }

        #endregion

        public Profile(string tabPageText, Controller cntrl)
        {
            this.Text = tabPageText;
            ParentControllerObject = cntrl;

            //Controls
            rdbReturnButton = new RadioButton();
            cbProfileBox = new ComboBox();
            tbDescription = new TextBox();
            tProfileTrackBar = new TrackBar();
            lblProfileLabel = new Label();
            GroupBox grpFanSettings = new GroupBox();
            chkStartBoost = new CheckBox();

            //Events
            rdbReturnButton.Click += BtnProfileActive_Click;
            cbProfileBox.SelectedValueChanged += CbProfileTabComboBox_SelectedValueChanged;
            tbDescription.TextChanged += TbDescription_TextChanged;
            tProfileTrackBar.ValueChanged += T_ValueChanged;
            chkStartBoost.CheckedChanged += ChkStartBoost_CheckedChanged;

            rdbReturnButton.Name = "rdbActive";
            chkStartBoost.Name = "chkStartBoost";
            chkStartBoost.AutoSize = true;
            rdbReturnButton.Text = "";

            //Location
            rdbReturnButton.Location = new Point(iXOutOfGroup, rdbReturnButton.Location.Y);
            chkStartBoost.Location = new Point(iXOutOfGroup, rdbReturnButton.Location.Y + rdbReturnButton.Height);
            cbProfileBox.Location = new Point(iXInGroup, chkStartBoost.Location.Y);
            tbDescription.Location = new Point(iXInGroup, cbProfileBox.Location.Y + cbProfileBox.Height + 15);
            tProfileTrackBar.Location = new Point(iXInGroup + cbProfileBox.Location.X + cbProfileBox.Width, cbProfileBox.Location.Y);
            lblProfileLabel.Location = new Point(tProfileTrackBar.Location.X, tProfileTrackBar.Location.Y + tProfileTrackBar.Height);

            //Appearance
            cbProfileBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cbProfileBox.DisplayMember = "Name";
            lblProfileLabel.AutoSize = true;

            tProfileTrackBar.Width = 300;
            tbDescription.Width = 120;

            ProfileComboBox = cbProfileBox;
            ProfileTrackBar = tProfileTrackBar;
            ProfileLabel = lblProfileLabel;
            ProfileTextBox = tbDescription;

            tProfileTrackBar.Minimum = 0;
            tProfileTrackBar.Maximum = 100;

            Controls.Add(chkStartBoost);
            Controls.Add(rdbReturnButton);
            Controls.Add(grpFanSettings);

            grpFanSettings.Controls.Add(lblProfileLabel);
            grpFanSettings.Controls.Add(tProfileTrackBar);
            grpFanSettings.Controls.Add(tbDescription);
            grpFanSettings.Controls.Add(lblProfileLabel);
            grpFanSettings.Controls.Add(cbProfileBox);

            grpFanSettings.Location = new Point(5, chkStartBoost.Location.Y + 20);
            grpFanSettings.Width = tProfileTrackBar.Location.X + tProfileTrackBar.Width + 200;
            grpFanSettings.Height = 95;
        }

        private void ChkStartBoost_CheckedChanged(object sender, EventArgs e)
        {
            bStartBoost = ((CheckBox)sender).Checked;
        }

        private void TbDescription_TextChanged(object sender, EventArgs e)
        {
            ProfileCurrentFan.Description = ((TextBox)sender).Text;
        }

        private void BtnProfileActive_Click(object sender, EventArgs e)
        {
            if (ParentControllerObject != null)
            {
                ParentControllerObject.activateProfile(this);
            }

            ParentControllerObject.AcceptButtonFrmMain.Enabled = this.IsActiveProfile;
        }

        private void T_ValueChanged(object sender, EventArgs e)
        {
            if(ParentControllerObject == null) { return; }
            ProfileCurrentFan.SpeedPercentage = ((TrackBar)sender).Value;
            ProfileLabel.Text = ParentControllerObject.LanguageControllerObject.getVariableText(ParentControllerObject.LanguageControllerObject.CurrentLanguage, "lblCurrentDescription").Replace("[]", ((TrackBar)sender).Value.ToString());
        }

        private void CbProfileTabComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ProfileCurrentFan = (fan)((ComboBox)sender).SelectedItem;
            ProfileTrackBar.Value = ProfileCurrentFan.SpeedPercentage;
            ProfileTextBox.Text = ProfileCurrentFan.Description;
        }
    }
}
