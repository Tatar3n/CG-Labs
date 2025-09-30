using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab3_CG
{
    public partial class MainForm : Form
    {
        private Button btnTask1;
        private Button btnWunB;
        private Button btnTask3;
        private Label label1;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.btnTask1 = new System.Windows.Forms.Button();
            this.btnWunB = new System.Windows.Forms.Button();
            this.btnTask3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // btnTask1
            // 
            this.btnTask1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnTask1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTask1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnTask1.Location = new System.Drawing.Point(50, 50);
            this.btnTask1.Name = "btnTask1";
            this.btnTask1.Size = new System.Drawing.Size(150, 35);
            this.btnTask1.TabIndex = 1;
            this.btnTask1.Text = "Task 1";
            this.btnTask1.UseVisualStyleBackColor = false;
            this.btnTask1.Click += new System.EventHandler(this.btnTask1_Click);

            // btnShowBoth
            // 
            this.btnWunB.BackColor = System.Drawing.Color.SteelBlue;
            this.btnWunB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWunB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnWunB.ForeColor = System.Drawing.Color.White;
            this.btnWunB.Location = new System.Drawing.Point(50, 95);
            this.btnWunB.Name = "btnShowBoth";
            this.btnWunB.Size = new System.Drawing.Size(150, 35);
            this.btnWunB.TabIndex = 2;
            this.btnWunB.Text = "WuAndB";
            this.btnWunB.UseVisualStyleBackColor = false;
            this.btnWunB.Click += new System.EventHandler(this.btnShowWuNB_Click);

            // btnTask3
            // 
            this.btnTask3.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnTask3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTask3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnTask3.Location = new System.Drawing.Point(50, 140);
            this.btnTask3.Name = "btnTask3";
            this.btnTask3.Size = new System.Drawing.Size(150, 35);
            this.btnTask3.TabIndex = 3;
            this.btnTask3.Text = "Task 3";
            this.btnTask3.UseVisualStyleBackColor = false;
            this.btnTask3.Click += new System.EventHandler(this.btnTask3_Click);

            // MainForm
            // 
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(250, 200);
            this.Controls.Add(this.btnTask3);
            this.Controls.Add(this.btnWunB);
            this.Controls.Add(this.btnTask1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lab3 Tasks";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnTask1_Click(object sender, EventArgs e)
        {
            // здесь пиши свою форму ок
        }

        private void btnShowWuNB_Click(object sender, EventArgs e)
        {
            WuAndB form = new WuAndB();
            form.ShowDialog();
        }

        private void btnTask3_Click(object sender, EventArgs e)
        {
            // здесь пиши свою форму ок
        }
    }
}