namespace PDF_Analiser
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btn_Generate = new System.Windows.Forms.Button();
            this.lbl_FilePath = new System.Windows.Forms.Label();
            this.btn_Browse = new System.Windows.Forms.Button();
            this.txt_FilePath = new System.Windows.Forms.TextBox();
            this.richConsole = new System.Windows.Forms.RichTextBox();
            this.progressLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.progressBar1.Location = new System.Drawing.Point(40, 386);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(464, 15);
            this.progressBar1.TabIndex = 11;
            this.progressBar1.UseWaitCursor = true;
            // 
            // btn_Generate
            // 
            this.btn_Generate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_Generate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Generate.Location = new System.Drawing.Point(200, 101);
            this.btn_Generate.Name = "btn_Generate";
            this.btn_Generate.Size = new System.Drawing.Size(124, 39);
            this.btn_Generate.TabIndex = 10;
            this.btn_Generate.Text = "Start Analyze";
            this.btn_Generate.UseVisualStyleBackColor = true;
            this.btn_Generate.Click += new System.EventHandler(this.btn_Generate_Click);
            // 
            // lbl_FilePath
            // 
            this.lbl_FilePath.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_FilePath.AutoSize = true;
            this.lbl_FilePath.Location = new System.Drawing.Point(37, 46);
            this.lbl_FilePath.Name = "lbl_FilePath";
            this.lbl_FilePath.Size = new System.Drawing.Size(34, 13);
            this.lbl_FilePath.TabIndex = 9;
            this.lbl_FilePath.Text = "Input:";
            // 
            // btn_Browse
            // 
            this.btn_Browse.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_Browse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Browse.Location = new System.Drawing.Point(429, 33);
            this.btn_Browse.Name = "btn_Browse";
            this.btn_Browse.Size = new System.Drawing.Size(75, 38);
            this.btn_Browse.TabIndex = 8;
            this.btn_Browse.Text = "Browse";
            this.btn_Browse.UseVisualStyleBackColor = true;
            this.btn_Browse.Click += new System.EventHandler(this.btn_Browse_Click);
            // 
            // txt_FilePath
            // 
            this.txt_FilePath.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txt_FilePath.Location = new System.Drawing.Point(77, 43);
            this.txt_FilePath.Name = "txt_FilePath";
            this.txt_FilePath.Size = new System.Drawing.Size(330, 20);
            this.txt_FilePath.TabIndex = 7;
            this.txt_FilePath.WordWrap = false;
            // 
            // richConsole
            // 
            this.richConsole.BackColor = System.Drawing.Color.White;
            this.richConsole.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richConsole.ForeColor = System.Drawing.Color.Black;
            this.richConsole.Location = new System.Drawing.Point(12, 159);
            this.richConsole.Name = "richConsole";
            this.richConsole.ReadOnly = true;
            this.richConsole.Size = new System.Drawing.Size(503, 209);
            this.richConsole.TabIndex = 12;
            this.richConsole.Text = "";
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.BackColor = System.Drawing.Color.Transparent;
            this.progressLabel.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.progressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressLabel.Location = new System.Drawing.Point(222, 387);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(14, 12);
            this.progressLabel.TabIndex = 13;
            this.progressLabel.Text = "[-]";
            this.progressLabel.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 413);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.richConsole);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btn_Generate);
            this.Controls.Add(this.lbl_FilePath);
            this.Controls.Add(this.btn_Browse);
            this.Controls.Add(this.txt_FilePath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDF Analyzer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btn_Generate;
        private System.Windows.Forms.Label lbl_FilePath;
        private System.Windows.Forms.Button btn_Browse;
        private System.Windows.Forms.TextBox txt_FilePath;
        private System.Windows.Forms.RichTextBox richConsole;
        private System.Windows.Forms.Label progressLabel;
    }
}

