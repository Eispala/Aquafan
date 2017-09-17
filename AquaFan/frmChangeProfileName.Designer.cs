namespace AquaFan
{
    partial class frmChangeProfileName
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblChangeProfileName = new System.Windows.Forms.Label();
            this.tbNewProfileName = new System.Windows.Forms.TextBox();
            this.btnAcceptNewProfileName = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblChangeProfileName
            // 
            this.lblChangeProfileName.AutoSize = true;
            this.lblChangeProfileName.Location = new System.Drawing.Point(12, 26);
            this.lblChangeProfileName.Name = "lblChangeProfileName";
            this.lblChangeProfileName.Size = new System.Drawing.Size(35, 13);
            this.lblChangeProfileName.TabIndex = 0;
            this.lblChangeProfileName.Text = "label1";
            // 
            // textBox1
            // 
            this.tbNewProfileName.Location = new System.Drawing.Point(101, 23);
            this.tbNewProfileName.Name = "textBox1";
            this.tbNewProfileName.Size = new System.Drawing.Size(389, 20);
            this.tbNewProfileName.TabIndex = 1;
            this.tbNewProfileName.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnAcceptNewProfileName
            // 
            this.btnAcceptNewProfileName.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAcceptNewProfileName.Enabled = false;
            this.btnAcceptNewProfileName.Location = new System.Drawing.Point(373, 77);
            this.btnAcceptNewProfileName.Name = "btnAcceptNewProfileName";
            this.btnAcceptNewProfileName.Size = new System.Drawing.Size(117, 23);
            this.btnAcceptNewProfileName.TabIndex = 2;
            this.btnAcceptNewProfileName.Text = "button1";
            this.btnAcceptNewProfileName.UseVisualStyleBackColor = true;
            this.btnAcceptNewProfileName.Click += new System.EventHandler(this.btnAcceptNewProfileName_Click);
            // 
            // frmChangeProfileName
            // 
            this.AcceptButton = this.btnAcceptNewProfileName;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 112);
            this.Controls.Add(this.btnAcceptNewProfileName);
            this.Controls.Add(this.tbNewProfileName);
            this.Controls.Add(this.lblChangeProfileName);
            this.Name = "frmChangeProfileName";
            this.Text = "frmChangeProfileName";
            this.Load += new System.EventHandler(this.frmChangeProfileName_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblChangeProfileName;
        private System.Windows.Forms.TextBox tbNewProfileName;
        private System.Windows.Forms.Button btnAcceptNewProfileName;
    }
}