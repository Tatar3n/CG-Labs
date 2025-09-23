using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab2_CG
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Главное меню - Обработка изображений";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.LightGray;

            var lblTitle = new Label
            {
                Text = "Выберите программу:",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(100, 20),
                Size = new Size(200, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitle);

            var btnRgbExtractor = new Button
            {
                Text = "RGB Channel Extractor",
                Location = new Point(100, 70),
                Size = new Size(200, 40),
                Font = new Font("Arial", 10),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White
            };
            btnRgbExtractor.Click += (s, e) => { new MainForm().Show(); };
            this.Controls.Add(btnRgbExtractor);

            var btnHsvFilter = new Button
            {
                Text = "HSV Filter",
                Location = new Point(100, 120),
                Size = new Size(200, 40),
                Font = new Font("Arial", 10),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White
            };
            btnHsvFilter.Click += (s, e) => { new Form2().Show(); };
            this.Controls.Add(btnHsvFilter);

            var btnGrayscale = new Button
            {
                Text = "Grayscale Converter",
                Location = new Point(100, 170),
                Size = new Size(200, 40),
                Font = new Font("Arial", 10),
                BackColor = Color.IndianRed,
                ForeColor = Color.White
            };
            btnGrayscale.Click += (s, e) => { new Lab2_CG.Form1().Show(); };
            this.Controls.Add(btnGrayscale);

            var btnExit = new Button
            {
                Text = "Выход",
                Location = new Point(100, 220),
                Size = new Size(200, 30),
                Font = new Font("Arial", 9),
                BackColor = Color.DarkGray,
                ForeColor = Color.White
            };
            btnExit.Click += (s, e) => Application.Exit();
            this.Controls.Add(btnExit);

            this.ResumeLayout(false);
        }
    }
}