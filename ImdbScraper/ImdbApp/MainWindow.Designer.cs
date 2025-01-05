namespace ImdbApp
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            label1 = new Label();
            cboImdbId = new ComboBox();
            label2 = new Label();
            lblSuccess = new Label();
            lblScrapeDate = new Label();
            label4 = new Label();
            btnGet = new Button();
            label3 = new Label();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 8);
            label1.Name = "label1";
            label1.Size = new Size(132, 15);
            label1.TabIndex = 0;
            label1.Text = "IMDb ID or image page:";
            // 
            // cboImdbId
            // 
            cboImdbId.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboImdbId.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboImdbId.FormattingEnabled = true;
            cboImdbId.Location = new Point(8, 24);
            cboImdbId.Name = "cboImdbId";
            cboImdbId.Size = new Size(260, 23);
            cboImdbId.TabIndex = 1;
            cboImdbId.KeyDown += cboImdbId_KeyDown;
            cboImdbId.Leave += cboImdbId_Leave;
            cboImdbId.Validating += cboImdbId_Validating;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 52);
            label2.Name = "label2";
            label2.Size = new Size(51, 15);
            label2.TabIndex = 3;
            label2.Text = "Success:";
            // 
            // lblSuccess
            // 
            lblSuccess.AutoSize = true;
            lblSuccess.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSuccess.Location = new Point(8, 72);
            lblSuccess.Name = "lblSuccess";
            lblSuccess.Size = new Size(28, 15);
            lblSuccess.TabIndex = 4;
            lblSuccess.Text = "       ";
            // 
            // lblScrapeDate
            // 
            lblScrapeDate.AutoSize = true;
            lblScrapeDate.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblScrapeDate.Location = new Point(8, 112);
            lblScrapeDate.Name = "lblScrapeDate";
            lblScrapeDate.Size = new Size(28, 15);
            lblScrapeDate.TabIndex = 6;
            lblScrapeDate.Text = "       ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(8, 92);
            label4.Name = "label4";
            label4.Size = new Size(71, 15);
            label4.TabIndex = 5;
            label4.Text = "Scrape date:";
            // 
            // btnGet
            // 
            btnGet.Location = new Point(272, 24);
            btnGet.Name = "btnGet";
            btnGet.Size = new Size(52, 23);
            btnGet.TabIndex = 2;
            btnGet.Text = "Get";
            btnGet.UseVisualStyleBackColor = true;
            btnGet.Click += btnGet_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(8, 132);
            label3.Name = "label3";
            label3.Size = new Size(71, 15);
            label3.TabIndex = 7;
            label3.Text = "Scrape data:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(8, 148);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(316, 180);
            textBox1.TabIndex = 8;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(328, 332);
            Controls.Add(textBox1);
            Controls.Add(label3);
            Controls.Add(btnGet);
            Controls.Add(lblScrapeDate);
            Controls.Add(label4);
            Controls.Add(lblSuccess);
            Controls.Add(label2);
            Controls.Add(cboImdbId);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainWindow";
            Text = "IMDb App";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cboImdbId;
        private Label label2;
        private Label lblSuccess;
        private Label lblScrapeDate;
        private Label label4;
        private Button btnGet;
        private Label label3;
        private TextBox textBox1;
    }
}
