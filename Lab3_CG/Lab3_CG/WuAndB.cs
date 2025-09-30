using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab3_CG
{
    public partial class WuAndB : Form
    {
        private Bitmap canvasBresenham;
        private Bitmap canvasWu;
        private Point startPoint;
        private bool isDrawing;

        public WuAndB()
        {
            InitializeComponent();
            canvasBresenham = new Bitmap(pictureBoxBresenham.Width, pictureBoxBresenham.Height);
            canvasWu = new Bitmap(pictureBoxWu.Width, pictureBoxWu.Height);
            pictureBoxBresenham.Image = canvasBresenham;
            pictureBoxWu.Image = canvasWu;
            ClearCanvases();
        }

        private void ClearCanvases()
        {
            using (Graphics g = Graphics.FromImage(canvasBresenham))
                g.Clear(Color.White);
            using (Graphics g = Graphics.FromImage(canvasWu))
                g.Clear(Color.White);
            pictureBoxBresenham.Invalidate();
            pictureBoxWu.Invalidate();
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPoint = e.Location;
                isDrawing = true;
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isDrawing)
            {
                DrawBothAlgorithms(startPoint, e.Location);
                isDrawing = false;
            }
        }

        private void DrawBothAlgorithms(Point p1, Point p2)
        {
            DrawBresenhamLine(p1, p2);
            DrawWuLine(p1, p2);
            pictureBoxBresenham.Invalidate();
            pictureBoxWu.Invalidate();
        }

        private void DrawBresenhamLine(Point p1, Point p2)
        {
            int x0 = p1.X;
            int y0 = p1.Y;
            int x1 = p2.X;
            int y1 = p2.Y;
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = (x0 < x1) ? 1 : -1;
            int sy = (y0 < y1) ? 1 : -1;
            if (dx >= dy)
                DrawBresenhamLineGentle(x0, y0, x1, y1, dx, dy, sx, sy);
            else
                DrawBresenhamLineSteep(x0, y0, x1, y1, dx, dy, sx, sy);
        }

        private void DrawBresenhamLineGentle(int x0, int y0, int x1, int y1, int dx, int dy, int sx, int sy)
        {
            int d = 2 * dy - dx;
            int y = y0;
            for (int x = x0; x != x1 + sx; x += sx)
            {
                SetPixelBresenham(x, y, Color.Black);
                if (d < 0)
                    d = d + 2 * dy;
                else
                {
                    y += sy;
                    d = d + 2 * (dy - dx);
                }
            }
        }

        private void DrawBresenhamLineSteep(int x0, int y0, int x1, int y1, int dx, int dy, int sx, int sy)
        {
            int d = 2 * dx - dy;
            int x = x0;
            for (int y = y0; y != y1 + sy; y += sy)
            {
                SetPixelBresenham(x, y, Color.Black);
                if (d < 0)
                    d = d + 2 * dx;
                else
                {
                    x += sx;
                    d = d + 2 * (dx - dy);
                }
            }
        }

        private void DrawWuLine(Point p1, Point p2)
        {
            int x0 = p1.X;
            int y0 = p1.Y;
            int x1 = p2.X;
            int y1 = p2.Y;
            DrawPointWu(x0, y0, 1.0f);
            float dx = x1 - x0;
            float dy = y1 - y0;
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if (x1 < x0)
                {
                    Swap(ref x0, ref x1);
                    Swap(ref y0, ref y1);
                    dx = -dx;
                    dy = -dy;
                }
                float gradient = dy / dx;
                float y = y0 + gradient;
                for (int x = x0 + 1; x <= x1 - 1; x++)
                {
                    DrawPointWu(x, (int)y, 1 - (y - (int)y));
                    DrawPointWu(x, (int)y + 1, y - (int)y);
                    y += gradient;
                }
            }
            else
            {
                if (y1 < y0)
                {
                    Swap(ref x0, ref x1);
                    Swap(ref y0, ref y1);
                    dx = -dx;
                    dy = -dy;
                }
                float gradient = dx / dy;
                float x = x0 + gradient;
                for (int y = y0 + 1; y <= y1 - 1; y++)
                {
                    DrawPointWu((int)x, y, 1 - (x - (int)x));
                    DrawPointWu((int)x + 1, y, x - (int)x);
                    x += gradient;
                }
            }
            DrawPointWu(x1, y1, 1.0f);
        }

        private void DrawPointWu(int x, int y, float intensity)
        {
            if (x >= 0 && x < canvasWu.Width && y >= 0 && y < canvasWu.Height)
            {
                Color foreground = Color.Black;
                Color background = canvasWu.GetPixel(x, y);
                int r = (int)(foreground.R * intensity + background.R * (1 - intensity));
                int g = (int)(foreground.G * intensity + background.G * (1 - intensity));
                int b = (int)(foreground.B * intensity + background.B * (1 - intensity));
                Color result = Color.FromArgb(r, g, b);
                canvasWu.SetPixel(x, y, result);
            }
        }

        private void SetPixelBresenham(int x, int y, Color color)
        {
            if (x >= 0 && x < canvasBresenham.Width && y >= 0 && y < canvasBresenham.Height)
                canvasBresenham.SetPixel(x, y, color);
        }

        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearCanvases();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}