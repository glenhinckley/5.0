namespace Manual_test_app
{
    partial class frmUnitTest_AddressAPI
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
            this.cmdSwap = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdSwap
            // 
            this.cmdSwap.Location = new System.Drawing.Point(61, 313);
            this.cmdSwap.Name = "cmdSwap";
            this.cmdSwap.Size = new System.Drawing.Size(182, 23);
            this.cmdSwap.TabIndex = 0;
            this.cmdSwap.Text = "Verify by Address Swap";
            this.cmdSwap.UseVisualStyleBackColor = true;
            this.cmdSwap.Click += new System.EventHandler(this.cmdSwap_Click);
            // 
            // frmUnitTest_AddressAPI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1110, 674);
            this.Controls.Add(this.cmdSwap);
            this.Name = "frmUnitTest_AddressAPI";
            this.Text = "frmUnitTest_AddressAPI";
            this.Load += new System.EventHandler(this.frmUnitTest_AddressAPI_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdSwap;
    }
}