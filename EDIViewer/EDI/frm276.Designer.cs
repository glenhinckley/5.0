namespace Manual_test_app
{
    partial class frm276
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
            this.txtRaw276 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtConString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtRaw276
            // 
            this.txtRaw276.Location = new System.Drawing.Point(12, 26);
            this.txtRaw276.Multiline = true;
            this.txtRaw276.Name = "txtRaw276";
            this.txtRaw276.Size = new System.Drawing.Size(1114, 634);
            this.txtRaw276.TabIndex = 0;
            this.txtRaw276.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1051, 686);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtConString
            // 
            this.txtConString.Location = new System.Drawing.Point(12, 765);
            this.txtConString.Name = "txtConString";
            this.txtConString.Size = new System.Drawing.Size(1114, 20);
            this.txtConString.TabIndex = 27;
            this.txtConString.TextChanged += new System.EventHandler(this.txtConString_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 686);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "label1";
            // 
            // frm276
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1162, 798);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtConString);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtRaw276);
            this.Name = "frm276";
            this.Text = "frmEDI276Insert";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRaw276;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtConString;
        private System.Windows.Forms.Label label1;
    }
}