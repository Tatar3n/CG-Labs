using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace CG_Lab
{
    public partial class Mountain : Form
    {
        List<Point> points;

        public Mountain()
        {
            InitializeComponent();
            StartHeight.Maximum = pictureBox.Height;
            EndHeight.Maximum = pictureBox.Height;
            PrevStep.Enabled = false;
            NextStep.Enabled = false;
        }

        private void MountainButton_Click(object sender, EventArgs e)
        {
            points = new List<Point>();
            points.Add(new Point(0, pictureBox.Height - (int)StartHeight.Value));
            points.Add(new Point(pictureBox.Width, pictureBox.Height - (int)EndHeight.Value));
            NextStep.Enabled = true;
            DrawMountains();
            label2.Visible = true;
            PrevStep.Visible = true;
            NextStep.Visible = true;
        }

        private void NextStep_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            double R = (double)Roughness.Value;
            List<Point> newPoints = new List<Point>();
            newPoints.Add(points[0]);
            for (int i = 1; i < points.Count; i++)
            {
                double L = (double)Math.Sqrt(Math.Pow((double)(points[i].X - points[i - 1].X), 2) + Math.Pow((double)(points[i].Y - points[i - 1].Y), 2));
                newPoints.Add(new Point((points[i - 1].X + points[i].X) / 2,
                    (points[i-1].Y + points[i].Y) / 2 + (int)rand.Next((int)Math.Round(-R * L), (int)Math.Round(R * L + 1))));
                newPoints.Add(points[i]);
            }
            if (newPoints.Count >= pictureBox.Width / 4)
                NextStep.Enabled = false;
            PrevStep.Enabled = true;
            points = newPoints;
            DrawMountains();
        }

        private void DrawMountains()
        {
            Bitmap newMountain = new Bitmap(pictureBox.Width, pictureBox.Height);
            Pen pen = new Pen(Color.Blue, 2);
            using (Graphics g = Graphics.FromImage(newMountain))
                g.DrawLines(pen, points.ToArray());
            pictureBox.Image = newMountain;
        }

        private void PrevStep_Click(object sender, EventArgs e)
        {
            List<Point> newPoints = new List<Point>();
            for (int i = 0; i < points.Count; i += 2)
                newPoints.Add(points[i]);
            if (newPoints.Count == 2)
                PrevStep.Enabled = false;
            NextStep.Enabled = true;
            points = newPoints;
            DrawMountains();
        }

      
    }
}
