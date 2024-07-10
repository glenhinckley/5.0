namespace Manual_test_app.DEPLOYMENT
{
    partial class frmNewPackage
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
            this.cndSaveNewPackage = new System.Windows.Forms.Button();
            this.txtPackageName = new System.Windows.Forms.TextBox();
            this.txtPackageDescription = new System.Windows.Forms.TextBox();
            this.cmdBuildPackage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cndSaveNewPackage
            // 
            this.cndSaveNewPackage.Location = new System.Drawing.Point(506, 166);
            this.cndSaveNewPackage.Name = "cndSaveNewPackage";
            this.cndSaveNewPackage.Size = new System.Drawing.Size(75, 23);
            this.cndSaveNewPackage.TabIndex = 1;
            this.cndSaveNewPackage.Text = "Save New Package";
            this.cndSaveNewPackage.UseVisualStyleBackColor = true;
            this.cndSaveNewPackage.Click += new System.EventHandler(this.cndSaveNewPackage_Click);
            // 
            // txtPackageName
            // 
            this.txtPackageName.Location = new System.Drawing.Point(84, 35);
            this.txtPackageName.Name = "txtPackageName";
            this.txtPackageName.Size = new System.Drawing.Size(497, 20);
            this.txtPackageName.TabIndex = 2;
            // 
            // txtPackageDescription
            // 
            this.txtPackageDescription.Location = new System.Drawing.Point(84, 61);
            this.txtPackageDescription.Multiline = true;
            this.txtPackageDescription.Name = "txtPackageDescription";
            this.txtPackageDescription.Size = new System.Drawing.Size(497, 99);
            this.txtPackageDescription.TabIndex = 3;
            // 
            // cmdBuildPackage
            // 
            this.cmdBuildPackage.Enabled = false;
            this.cmdBuildPackage.Location = new System.Drawing.Point(374, 166);
            this.cmdBuildPackage.Name = "cmdBuildPackage";
            this.cmdBuildPackage.Size = new System.Drawing.Size(126, 23);
            this.cmdBuildPackage.TabIndex = 4;
            this.cmdBuildPackage.Text = "Build Package";
            this.cmdBuildPackage.UseVisualStyleBackColor = true;
            this.cmdBuildPackage.Click += new System.EventHandler(this.cmdBuildPackage_Click);
            // 
            // frmNewPackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 201);
            this.Controls.Add(this.cmdBuildPackage);
            this.Controls.Add(this.txtPackageDescription);
            this.Controls.Add(this.txtPackageName);
            this.Controls.Add(this.cndSaveNewPackage);
            this.Name = "frmNewPackage";
            this.Text = "frmPackages";
            this.Load += new System.EventHandler(this.frmNewPackage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cndSaveNewPackage;
        private System.Windows.Forms.TextBox txtPackageName;
        private System.Windows.Forms.TextBox txtPackageDescription;
        private System.Windows.Forms.Button cmdBuildPackage;
    }
}