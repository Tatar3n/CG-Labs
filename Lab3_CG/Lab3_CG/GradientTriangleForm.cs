using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GradientTriangleApp
{
    public partial class GradientTriangleForm : Form
    {
        private Color vertexColor1 = Color.Red;
        private Color vertexColor2 = Color.Green;
        private Color vertexColor3 = Color.Blue;

        public GradientTriangleForm()
        {
            InitializeComponent();
        }

        private void DrawTriangle(object sender, EventArgs e)
        {
            using (var bitmap = new Bitmap(canvas.Width, canvas.Height))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White);

                int x1 = (int)x1Input.Value;
                int y1 = (int)y1Input.Value;
                int x2 = (int)x2Input.Value;
                int y2 = (int)y2Input.Value;
                int x3 = (int)x3Input.Value;
                int y3 = (int)y3Input.Value;

                using (var pen = new Pen(Color.Black))
                {
                    graphics.DrawLine(pen, x1, y1, x2, y2);
                    graphics.DrawLine(pen, x2, y2, x3, y3);
                    graphics.DrawLine(pen, x3, y3, x1, y1);
                }

                canvas.Image = (Bitmap)bitmap.Clone();
            }

            colorInputs.Visible = true;
        }

        private void DrawGradientTriangle(object sender, EventArgs e)
        {
            int x1 = (int)x1Input.Value;
            int y1 = (int)y1Input.Value;
            int x2 = (int)x2Input.Value;
            int y2 = (int)y2Input.Value;
            int x3 = (int)x3Input.Value;
            int y3 = (int)y3Input.Value;

            var bitmap = DrawGradientTriangle(x1, y1, x2, y2, x3, y3, vertexColor1, vertexColor2, vertexColor3);
            canvas.Image = bitmap;
        }

        public Bitmap DrawGradientTriangle(int x1, int y1, int x2, int y2, int x3, int y3, Color color1, Color color2, Color color3)
        {
            var bitmap = new Bitmap(canvas.Width, canvas.Height);

            // Сортируем вершины по Y
            var vertices = new List<Vertex>
            {
                new Vertex { X = x1, Y = y1, Color = color1 },
                new Vertex { X = x2, Y = y2, Color = color2 },
                new Vertex { X = x3, Y = y3, Color = color3 }
            };

            // Сортируем вершины по возрастанию Y
            vertices.Sort((a, b) => a.Y.CompareTo(b.Y));

            int minY = vertices[0].Y;
            int maxY = vertices[2].Y;

            for (int y = minY; y <= maxY; y++)
            {
                var intersections = new List<IntersectionPoint>();

                // Проверяем все три стороны
                AddIntersectionIfNeeded(vertices[0], vertices[1], y, intersections);
                AddIntersectionIfNeeded(vertices[1], vertices[2], y, intersections);
                AddIntersectionIfNeeded(vertices[0], vertices[2], y, intersections);

                // Должно быть ровно 2 пересечения
                if (intersections.Count == 2)
                {
                    var p1 = intersections[0];
                    var p2 = intersections[1];

                    if (p1.X > p2.X)
                    {
                        var temp = p1;
                        p1 = p2;
                        p2 = temp;
                    }

                    int startX = (int)Math.Floor(p1.X);
                    int endX = (int)Math.Ceiling(p2.X);

                    for (int x = startX; x <= endX; x++)
                    {
                        if (x < p1.X || x > p2.X) continue;

                        double t = (p2.X - p1.X) != 0 ? (x - p1.X) / (p2.X - p1.X) : 0;
                        Color color = LerpColor(p1.Color, p2.Color, t);

                        if (x >= 0 && x < bitmap.Width && y >= 0 && y < bitmap.Height)
                        {
                            bitmap.SetPixel(x, y, color);
                        }
                    }
                }
                else if (intersections.Count > 2)
                {
                    // Если больше 2 пересечений, сортируем по X и берем крайние
                    intersections.Sort((a, b) => a.X.CompareTo(b.X));
                    var p1 = intersections[0];
                    var p2 = intersections[intersections.Count - 1];

                    int startX = (int)Math.Floor(p1.X);
                    int endX = (int)Math.Ceiling(p2.X);

                    for (int x = startX; x <= endX; x++)
                    {
                        if (x < p1.X || x > p2.X) continue;

                        double t = (p2.X - p1.X) != 0 ? (x - p1.X) / (p2.X - p1.X) : 0;
                        Color color = LerpColor(p1.Color, p2.Color, t);

                        if (x >= 0 && x < bitmap.Width && y >= 0 && y < bitmap.Height)
                        {
                            bitmap.SetPixel(x, y, color);
                        }
                    }
                }
            }

            return bitmap;
        }

        private void AddIntersectionIfNeeded(Vertex v1, Vertex v2, int y, List<IntersectionPoint> intersections)
        {
            if (v1.Y == v2.Y) return; // Горизонтальная линия

            if (y >= Math.Min(v1.Y, v2.Y) && y <= Math.Max(v1.Y, v2.Y))
            {
                double t = (double)(y - v1.Y) / (v2.Y - v1.Y);
                intersections.Add(new IntersectionPoint
                {
                    X = v1.X + t * (v2.X - v1.X),
                    Color = LerpColor(v1.Color, v2.Color, t)
                });
            }
        }

        private Color LerpColor(Color color1, Color color2, double t)
        {
            t = Math.Max(0, Math.Min(1, t));

            int r = (int)Math.Round(color1.R + t * (color2.R - color1.R));
            int g = (int)Math.Round(color1.G + t * (color2.G - color1.G));
            int b = (int)Math.Round(color1.B + t * (color2.B - color1.B));

            r = Math.Max(0, Math.Min(255, r));
            g = Math.Max(0, Math.Min(255, g));
            b = Math.Max(0, Math.Min(255, b));

            return Color.FromArgb(r, g, b);
        }

        private void ColorButton1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                vertexColor1 = colorDialog1.Color;
                colorButton1.BackColor = vertexColor1;
            }
        }

        private void ColorButton2_Click(object sender, EventArgs e)
        {
            if (colorDialog2.ShowDialog() == DialogResult.OK)
            {
                vertexColor2 = colorDialog2.Color;
                colorButton2.BackColor = vertexColor2;
            }
        }

        private void ColorButton3_Click(object sender, EventArgs e)
        {
            if (colorDialog3.ShowDialog() == DialogResult.OK)
            {
                vertexColor3 = colorDialog3.Color;
                colorButton3.BackColor = vertexColor3;
            }
        }

        private class Vertex
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Color Color { get; set; }
        }

        private class IntersectionPoint
        {
            public double X { get; set; }
            public Color Color { get; set; }
        }
    }
}