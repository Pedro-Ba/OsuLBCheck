namespace OsuLBCheck
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel2 = new Panel();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            textBox_clientSecret = new TextBox();
            textBox_clientID = new TextBox();
            label_ScoreToBeat = new Label();
            label_CurrentScore = new Label();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            pictureBox1 = new PictureBox();
            label_Info = new Label();
            tabPage2 = new TabPage();
            panel2.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(textBox_clientSecret);
            panel2.Controls.Add(textBox_clientID);
            panel2.Location = new Point(8, 26);
            panel2.Name = "panel2";
            panel2.Size = new Size(275, 121);
            panel2.TabIndex = 26;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(82, 20);
            label3.Name = "label3";
            label3.Size = new Size(64, 15);
            label3.TabIndex = 4;
            label3.Text = "API Values:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 76);
            label2.Name = "label2";
            label2.Size = new Size(76, 15);
            label2.TabIndex = 3;
            label2.Text = "Client Secret:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 47);
            label1.Name = "label1";
            label1.Size = new Size(55, 15);
            label1.TabIndex = 2;
            label1.Text = "Client ID:";
            // 
            // textBox_clientSecret
            // 
            textBox_clientSecret.Location = new Point(82, 73);
            textBox_clientSecret.Name = "textBox_clientSecret";
            textBox_clientSecret.Size = new Size(190, 23);
            textBox_clientSecret.TabIndex = 1;
            textBox_clientSecret.UseSystemPasswordChar = true;
            // 
            // textBox_clientID
            // 
            textBox_clientID.Location = new Point(82, 44);
            textBox_clientID.Name = "textBox_clientID";
            textBox_clientID.Size = new Size(190, 23);
            textBox_clientID.TabIndex = 0;
            textBox_clientID.UseSystemPasswordChar = true;
            // 
            // label_ScoreToBeat
            // 
            label_ScoreToBeat.AutoSize = true;
            label_ScoreToBeat.Font = new Font("Segoe UI", 16F);
            label_ScoreToBeat.Location = new Point(55, 681);
            label_ScoreToBeat.Name = "label_ScoreToBeat";
            label_ScoreToBeat.Size = new Size(0, 30);
            label_ScoreToBeat.TabIndex = 27;
            // 
            // label_CurrentScore
            // 
            label_CurrentScore.AutoSize = true;
            label_CurrentScore.Font = new Font("Segoe UI", 16F);
            label_CurrentScore.Location = new Point(55, 731);
            label_CurrentScore.Name = "label_CurrentScore";
            label_CurrentScore.Size = new Size(0, 30);
            label_CurrentScore.TabIndex = 28;
            // 
            // tabControl1
            // 
            tabControl1.Appearance = TabAppearance.Buttons;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(299, 910);
            tabControl1.TabIndex = 29;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.FromArgb(64, 64, 64);
            tabPage1.Controls.Add(pictureBox1);
            tabPage1.Controls.Add(label_Info);
            tabPage1.Controls.Add(label_CurrentScore);
            tabPage1.Controls.Add(label_ScoreToBeat);
            tabPage1.ForeColor = SystemColors.ButtonHighlight;
            tabPage1.Location = new Point(4, 27);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(291, 879);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Data";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(16, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(256, 256);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 30;
            pictureBox1.TabStop = false;
            // 
            // label_Info
            // 
            label_Info.AutoSize = true;
            label_Info.Font = new Font("Segoe UI", 12F);
            label_Info.Location = new Point(8, 299);
            label_Info.MaximumSize = new Size(290, 0);
            label_Info.Name = "label_Info";
            label_Info.Size = new Size(0, 21);
            label_Info.TabIndex = 29;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(panel2);
            tabPage2.Location = new Point(4, 27);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(297, 879);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "API Info";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(299, 910);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel panel2;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox textBox_clientSecret;
        private TextBox textBox_clientID;
        private Label label_ScoreToBeat;
        private Label label_CurrentScore;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label label_Info;
        private PictureBox pictureBox1;
    }


    #endregion
}
