using System.Drawing;
using System.Windows.Forms;
using System;

namespace Lab3_CG
{
    partial class WuAndB
    {

        private PictureBox pictureBoxBresenham;
        private PictureBox pictureBoxWu;
        private Button btnClear;
        private Button btnBack;
        private Label label1;
        private Label label2;
        private Label label3;
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

        private void InitializeComponent()
        {
            this.pictureBoxBresenham = new PictureBox();
            this.pictureBoxWu = new PictureBox();
            this.btnClear = new Button();
            this.btnBack = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBresenham)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWu)).BeginInit();
            this.SuspendLayout();

            this.pictureBoxBresenham.BackColor = Color.White;
            this.pictureBoxBresenham.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBoxBresenham.Location = new Point(10, 60);
            this.pictureBoxBresenham.Size = new Size(300, 300);
            this.pictureBoxBresenham.MouseDown += new MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBoxBresenham.MouseUp += new MouseEventHandler(this.pictureBox_MouseUp);

            this.pictureBoxWu.BackColor = Color.White;
            this.pictureBoxWu.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBoxWu.Location = new Point(320, 60);
            this.pictureBoxWu.Size = new Size(300, 300);
            this.pictureBoxWu.MouseDown += new MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBoxWu.MouseUp += new MouseEventHandler(this.pictureBox_MouseUp);

            this.btnClear.Location = new Point(520, 380);
            this.btnClear.Size = new Size(90, 30);
            this.btnClear.Text = "Очистить";
            this.btnClear.Click += new EventHandler(this.btnClear_Click);

            this.btnBack.Location = new Point(420, 380);
            this.btnBack.Size = new Size(90, 30);
            this.btnBack.Text = "Назад";
            this.btnBack.Click += new EventHandler(this.btnBack_Click);

            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.label1.Location = new Point(10, 10);
            this.label1.Size = new Size(400, 20);
            this.label1.Text = "Надо зажать!!!";

            this.label2.AutoSize = true;
            this.label2.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.label2.Location = new Point(10, 40);
            this.label2.Size = new Size(150, 15);
            this.label2.Text = "Алгоритм Брезенхема";

            this.label3.AutoSize = true;
            this.label3.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.label3.Location = new Point(320, 40);
            this.label3.Size = new Size(100, 15);
            this.label3.Text = "Алгоритм Ву";

            this.ClientSize = new Size(630, 420);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.pictureBoxWu);
            this.Controls.Add(this.pictureBoxBresenham);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "WuAndB";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBresenham)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}