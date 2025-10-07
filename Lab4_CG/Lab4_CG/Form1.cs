using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Lab4_CG
{
    public partial class Form1 : Form
    {
        List<Polygon> polygons;
        Color currColor = Color.Black;
        int currPolygonInd = 0;

        private enum Action { Move, Rotation1, Rotation2 };

        Point? clickedPoint = null;
        Bitmap curr_bitmap = null;

        public Form1()
        {
            InitializeComponent();
            Bitmap PictureBoxClear = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(PictureBoxClear))
                g.Clear(Color.White);
            pictureBox.Image = PictureBoxClear;
            polygons = new List<Polygon>();
            polygons.Add(new Polygon());
            transformBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void resetFlags()
        {
            clickedPoint = null;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            resetFlags();
            polygons.Clear();
            currPolygonInd = 0;

            Bitmap PictureBoxClear = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(PictureBoxClear))
                g.Clear(Color.White);

            polygons.Clear();

            pictureBox.Image = PictureBoxClear;
        }

        private void NewPolygon_Click(object sender, EventArgs e)
        {
            resetFlags();

            if (polygons.Count > currPolygonInd)
            {
                int len = polygons[currPolygonInd].points.Count;
                if (len > 2)
                    MyGraphics.DrawLineVu(pictureBox, currColor, polygons[currPolygonInd].points[len - 1], polygons[currPolygonInd].points[0]);
                currPolygonInd++;
                curr_bitmap = new Bitmap(pictureBox.Image);
            }
            Random r = new Random();
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            Point mousePoint = new Point(e.Location.X, e.Location.Y);

            if (polygons.Count == currPolygonInd)
                polygons.Add(new Polygon());

            if (clickedPoint == null)
            {
                polygons[currPolygonInd].points.Add(new Point(e.Location.X, e.Location.Y));
                int len = polygons[currPolygonInd].GetPointCount();
                if (len > 1)
                    MyGraphics.DrawLineVu(pictureBox, currColor, polygons[currPolygonInd].points[len - 2], polygons[currPolygonInd].points[len - 1]);
            }
            else
            {
                clickedPoint = mousePoint;
            }
        }

        private void transformButton_Click(object sender, EventArgs e)
        {
            switch (transformBox.SelectedIndex)
            {
                case (int)Action.Move:
                    Move();
                    break;
                case (int)Action.Rotation1:
                    RotateCenter(true);
                    break;
                case (int)Action.Rotation2:
                    RotateCenter();
                    break;
                default:
                    MessageBox.Show("Не реализованная операция!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private Matrix ApplyMove(Matrix point, double dx, double dy)
        {
            Matrix translationMatrix = new double[,]
                    {
                    { 1, 0, 0 },
                    { 0, 1, 0 },
                    { -dx, -dy, 1 }
                    };

            return point * translationMatrix;
        }

        private Matrix ApplyRotation(Matrix point, double angle)
        {
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            Matrix translationMatrix = new double[,]
            {
                    { cos, sin, 0 },
                    { -sin, cos, 0 },
                    { 0, 0, 1 }
            };

            return point * translationMatrix;
        }

        private void Move()
        {
            double dx = (double)numericUpDown4.Value;
            double dy = (double)numericUpDown5.Value;

            polygons.ForEach(poly =>
            {
                MyGraphics.ClearPolygon(pictureBox, poly.points);

                for (int i = 0; i < poly.points.Count; i++)
                {
                    Matrix pointMatrix = poly.points[i];

                    Matrix newPointMatrix =
                    ApplyMove(
                        poly.points[i],
                        -dx,
                        dy
                    );

                    poly.points[i] = newPointMatrix;

                }

                MyGraphics.DrawPolygon(pictureBox, currColor, poly.points);
            }

            );

            pictureBox.Refresh();
            curr_bitmap = new Bitmap(pictureBox.Image);
        }

        private void RotateCenter(bool useCenter = false)
        {
            var angle = numericUpDown1.Value;

            double angleRadians = (double)angle * (Math.PI / 180);

            polygons.ForEach(poly =>
            {
                Point center = (Point)(useCenter ? clickedPoint : poly.GetCenter());

                MyGraphics.ClearPolygon(pictureBox, poly.points);

                for (int i = 0; i < poly.points.Count; i++)
                {
                    Matrix newPointMatrix =
                    ApplyMove(
                        ApplyRotation(
                            ApplyMove(
                                poly.points[i],
                                center.X,
                                center.Y
                            ),
                            angleRadians
                        ),
                        -center.X,
                        -center.Y
                    );

                    poly.points[i] = newPointMatrix;

                }

                MyGraphics.DrawPolygon(pictureBox, currColor, poly.points);
            }

            );

            pictureBox.Refresh();
            curr_bitmap = new Bitmap(pictureBox.Image);
        }
    }

    public class Polygon
    {
        public List<Point> points;

        public Polygon() => points = new List<Point>();

        public int GetPointCount() => points.Count;

        public Point GetCenter()
        {
            float x = 0, y = 0;
            for (int i = 0; i < points.Count; i++)
            {
                x += points[i].X;
                y += points[i].Y;
            }
            return new Point((int)Math.Round(x / points.Count), (int)Math.Round(y / points.Count));
        }
    }

    public class Matrix
    {
        public double[,] Values { get; }

        public Matrix(double[,] values)
        {
            Values = values;
        }

        public static implicit operator Matrix(double[,] values)
        {
            return new Matrix(values);
        }

        public static implicit operator Point(Matrix m)
        {
            return new Point((int)Math.Round(m[0, 0]), (int)Math.Round(m[0, 1]));
        }

        public Matrix(Point point)
        {
            Values = new double[1, 3] { { point.X, point.Y, 1 } };
        }

        public static implicit operator Matrix(Point point)
        {
            return new Matrix(point);
        }

        public static Matrix operator *(Matrix A, Matrix B)
        {
            int rowsA = A.Values.GetLength(0);
            int colsA = A.Values.GetLength(1);
            int rowsB = B.Values.GetLength(0);
            int colsB = B.Values.GetLength(1);

            double[,] result = new double[rowsA, colsB];

            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < colsA; k++)
                    {
                        result[i, j] += A.Values[i, k] * B.Values[k, j];
                    }
                }
            }

            return new Matrix(result);
        }

        public double this[int row, int column]
        {
            get
            {
                return Values[row, column];
            }
            set
            {
                Values[row, column] = value;
            }
        }

        public Matrix Clone() => new Matrix((double[,])Values.Clone());
    }

    public static class MyGraphics
    {

        public static void ClearPolygon(PictureBox pictureBox, List<Point> points)
        {
            DrawPolygon(pictureBox, Color.White, points);
        }

        public static void DrawPolygon(PictureBox pictureBox, Color color, List<Point> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                Point p0 = points[i];
                Point p1 = points[(i + 1) % points.Count];
                DrawLineVu(pictureBox, color, p0, p1);
            }
        }

        public static void DrawLineVu(PictureBox pictureBox, Color color, Point p0, Point p1)
        {
            int x0 = p0.X;
            int x1 = p1.X;
            int y0 = p0.Y;
            int y1 = p1.Y;

            Bitmap pb = new Bitmap(pictureBox.Image);

            if (x1 >= 0 && x1 < pictureBox.Width && y1 >= 0 && y1 < pictureBox.Height)
                pb.SetPixel(x1, y1, Color.FromArgb(255, 0, 0, 0));

            float deltaX = x1 - x0;
            float deltaY = y1 - y0;
            float m = Math.Abs(deltaY / deltaX);

            if (m <= 1)
            {
                float gradient = deltaY / deltaX;
                if (x0 <= x1)
                {
                    float y = y0 + gradient;
                    for (int x = x0 + 1; x <= x1; x++)
                    {
                        if (x >= 0 && x < pictureBox.Width && (int)y >= 0 && (int)y < pictureBox.Height)
                        {
                            int alpha = (int)((1 - (y - (int)y)) * 255);
                            pb.SetPixel(x, (int)y, Color.FromArgb(255, Math.Max(0, Math.Min(255, color.R * alpha / 255 + (255 - alpha) * 255 / 255)),
                                Math.Max(0, Math.Min(255, color.G * alpha / 255 + (255 - alpha) * 255 / 255)),
                                Math.Max(0, Math.Min(255, color.B * alpha / 255 + (255 - alpha) * 255 / 255))));
                        }
                        if (x >= 0 && x < pictureBox.Width && (int)y + 1 >= 0 && (int)y + 1 < pictureBox.Height)
                        {
                            int alpha = (int)((y - (int)y) * 255);
                            pb.SetPixel(x, (int)y + 1, Color.FromArgb(255, Math.Max(0, Math.Min(255, color.R * alpha / 255 + (255 - alpha) * 255 / 255)),
                                Math.Max(0, Math.Min(255, color.G * alpha / 255 + (255 - alpha) * 255 / 255)),
                                Math.Max(0, Math.Min(255, color.B * alpha / 255 + (255 - alpha) * 255 / 255))));
                        }
                        y += gradient;
                    }
                }
                else
                {
                    gradient *= -1;
                    float y = y0 + gradient;
                    for (int x = x0 - 1; x >= x1; x--)
                    {
                        if (x >= 0 && x < pictureBox.Width && (int)y >= 0 && (int)y < pictureBox.Height)
                        {
                            int alpha = (int)((1 - (y - (int)y)) * 255);
                            pb.SetPixel(x, (int)y, Color.FromArgb(255, Math.Max(0, Math.Min(255, color.R * alpha / 255 + (255 - alpha) * 255 / 255)),
                                Math.Max(0, Math.Min(255, color.G * alpha / 255 + (255 - alpha) * 255 / 255)),
                                Math.Max(0, Math.Min(255, color.B * alpha / 255 + (255 - alpha) * 255 / 255))));
                        }
                        if (x >= 0 && x < pictureBox.Width && (int)y + 1 >= 0 && (int)y + 1 < pictureBox.Height)
                        {
                            int alpha = (int)((y - (int)y) * 255);
                            pb.SetPixel(x, (int)y + 1, Color.FromArgb(255, Math.Max(0, Math.Min(255, color.R * alpha / 255 + (255 - alpha) * 255 / 255)),
                                Math.Max(0, Math.Min(255, color.G * alpha / 255 + (255 - alpha) * 255 / 255)),
                                Math.Max(0, Math.Min(255, color.B * alpha / 255 + (255 - alpha) * 255 / 255))));
                        }
                        y += gradient;
                    }
                }
            }
            else
            {
                float gradient = deltaX / deltaY;
                if (y0 <= y1)
                {
                    float x = x0 + gradient;
                    for (int y = y0 + 1; y <= y1; y++)
                    {
                        if ((int)x >= 0 && (int)x < pictureBox.Width && y >= 0 && y < pictureBox.Height)
                        {
                            int alpha = (int)((1 - (x - (int)x)) * 255);
                            pb.SetPixel((int)x, y, Color.FromArgb(255, Math.Max(0, Math.Min(255, color.R * alpha / 255 + (255 - alpha) * 255 / 255)),
                                Math.Max(0, Math.Min(255, color.G * alpha / 255 + (255 - alpha) * 255 / 255)),
                                Math.Max(0, Math.Min(255, color.B * alpha / 255 + (255 - alpha) * 255 / 255))));
                        }
                        if ((int)x + 1 >= 0 && (int)x + 1 < pictureBox.Width && y >= 0 && y < pictureBox.Height)
                        {
                            int alpha = (int)((x - (int)x) * 255);
                            pb.SetPixel((int)x + 1, y, Color.FromArgb(255, Math.Max(0, Math.Min(255, color.R * alpha / 255 + (255 - alpha) * 255 / 255)),
                                Math.Max(0, Math.Min(255, color.G * alpha / 255 + (255 - alpha) * 255 / 255)),
                                Math.Max(0, Math.Min(255, color.B * alpha / 255 + (255 - alpha) * 255 / 255))));
                        }
                        x += gradient;
                    }
                }
                else
                {
                    gradient *= -1;
                    float x = x0 + gradient;
                    for (int y = y0 - 1; y >= y1; y--)
                    {
                        if ((int)x >= 0 && (int)x < pictureBox.Width && y >= 0 && y < pictureBox.Height)
                        {
                            int alpha = (int)((1 - (x - (int)x)) * 255);
                            pb.SetPixel((int)x, y, Color.FromArgb(255, Math.Max(0, Math.Min(255, color.R * alpha / 255 + (255 - alpha) * 255 / 255)),
                                Math.Max(0, Math.Min(255, color.G * alpha / 255 + (255 - alpha) * 255 / 255)),
                                Math.Max(0, Math.Min(255, color.B * alpha / 255 + (255 - alpha) * 255 / 255))));
                        }
                        if ((int)x + 1 >= 0 && (int)x + 1 < pictureBox.Width && y >= 0 && y < pictureBox.Height)
                        {
                            int alpha = (int)((x - (int)x) * 255);
                            pb.SetPixel((int)x + 1, y, Color.FromArgb(255, Math.Max(0, Math.Min(255, color.R * alpha / 255 + (255 - alpha) * 255 / 255)),
                                Math.Max(0, Math.Min(255, color.G * alpha / 255 + (255 - alpha) * 255 / 255)),
                                Math.Max(0, Math.Min(255, color.B * alpha / 255 + (255 - alpha) * 255 / 255))));
                        }
                        x += gradient;
                    }
                }
            }
            pictureBox.Image = pb;

        }
    }
}