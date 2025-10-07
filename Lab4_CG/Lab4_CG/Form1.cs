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

        private enum Action { Move, Rotation1, Rotation2, Scaling1, Scaling2 };

        Point? clickedPoint = null;
        bool isDeterminingPosition = false;
        bool isDrawingEdgeForIntersection = false;
        bool isInPolygon = false;
        List<Point> clickedPoints = new List<Point>();
        bool pointExists = false;
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
            isDeterminingPosition = false;
            clickedPoint = null;
            isDrawingEdgeForIntersection = false;
            searchingBox.Text = "Векторные алгоритмы";
            isInPolygon = false;
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

            helpingFunction(mousePoint, e);

            if (polygons.Count == currPolygonInd)
                polygons.Add(new Polygon());

            if (!isDeterminingPosition && !isDrawingEdgeForIntersection && !isInPolygon)
            {
                polygons[currPolygonInd].points.Add(new Point(e.Location.X, e.Location.Y));
                int len = polygons[currPolygonInd].GetPointCount();
                if (len > 1)
                    MyGraphics.DrawLineVu(pictureBox, currColor, polygons[currPolygonInd].points[len - 2], polygons[currPolygonInd].points[len - 1]);
            }
        }
        private void helpingFunction(Point mousePoint, MouseEventArgs e)
        {
            if (isDeterminingPosition)
            {
                clickedPoint = mousePoint;
                if (pointExists)
                {
                    ClearPoint();
                    pointExists = false;
                    clickedPoint = null;
                }
                else
                {
                    clickedPoint = new Point(e.Location.X, e.Location.Y);
                    DrawPoint(clickedPoint.Value);
                    pointExists = true;
                    if (clickedPoint == null)
                    {
                        MessageBox.Show("Пожалуйста, выберите точку на холсте.", "Ошибка");
                        return;
                    }

                    Point edgeStart = FindNearestEdge((Point)clickedPoint).Item2;
                    Point edgeEnd = FindNearestEdge((Point)clickedPoint).Item1;

                    string position = ClassifyPointRelativeToEdge(edgeStart, edgeEnd, clickedPoint.Value);

                    MessageBox.Show($"Точка ({clickedPoint.Value.X}, {clickedPoint.Value.Y}) находится {position} относительно ребра ({edgeStart.X}, {edgeStart.Y}) - ({edgeEnd.X}, {edgeEnd.Y}).");
                }
            }
            if (isDrawingEdgeForIntersection)
            {
                clickedPoints.Add(new Point(e.Location.X, e.Location.Y));

                if (clickedPoints.Count == 2)
                {
                    pictureBox.Image = curr_bitmap;
                    Point newEdgeStart = clickedPoints[0];
                    Point newEdgeEnd = clickedPoints[1];

                    for (int j = 0; j < polygons.Count; j++)
                    {
                        for (int i = 0; i < polygons[j].GetPointCount(); i++)
                        {
                            {
                                Point edgeStart = polygons[j].points[i];
                                Point edgeEnd = polygons[j].points[(i + 1) % polygons[j].GetPointCount()];
                                Point? intersection = FindIntersection(newEdgeStart, newEdgeEnd, edgeStart, edgeEnd);

                                if (intersection != null)
                                {

                                    // Рисуем точку пересечения
                                    MyGraphics.DrawLineVu(pictureBox, currColor, newEdgeStart, newEdgeEnd);

                                    DrawPoint(intersection.Value);
                                    MessageBox.Show($"Точка пересечения: ({intersection.Value.X}, {intersection.Value.Y})");
                                    break;
                                }
                            }
                        }
                        clickedPoints.Clear();
                    }
                }
            }
            if (isInPolygon)
            {
                clickedPoint = mousePoint;
                if (pointExists)
                {
                    ClearPoint();
                    pointExists = false;
                    clickedPoint = null;
                }
                else
                {
                    clickedPoint = new Point(e.Location.X, e.Location.Y);
                    DrawPoint(clickedPoint.Value);
                    pointExists = true;
                    if (clickedPoint == null)
                    {
                        MessageBox.Show("Пожалуйста, выберите точку на холсте.", "Ошибка");
                        return;
                    }

                    bool isIn = IsPointInPolygon(mousePoint, polygons[currPolygonInd - 1]);

                    if (isIn)
                        MessageBox.Show($"Точка ({clickedPoint.Value.X}, {clickedPoint.Value.Y}) находится внутри полигона.");
                    else
                    {
                        MessageBox.Show($"Точка ({clickedPoint.Value.X}, {clickedPoint.Value.Y}) не принадлежит полигону.");
                    }
                }
            }
        }

        private void DrawPoint(PointF p)
        {
            Bitmap pb = new Bitmap(pictureBox.Image);
            using (Graphics g = Graphics.FromImage(pb))
            {
                g.FillEllipse(Brushes.Red, p.X - 3, p.Y - 3, 6, 6);
            }
            pictureBox.Image = pb;
        }

        private void ClearPoint()
        {
            pictureBox.Image = curr_bitmap;
        }

        private string ClassifyPointRelativeToEdge(Point edgeStart, Point edgeEnd, Point p)
        {
            int position = (p.Y - edgeStart.Y) * (edgeEnd.X - edgeStart.X) - (p.X - edgeStart.X) * (edgeEnd.Y - edgeStart.Y);

            if (position > 0)
                return "Слева";
            else if (position < 0)
                return "Справа";
            else
                return "На ребре";
        }
        private (Point, Point) FindNearestEdge(Point point)
        {
            double minDistance = double.MaxValue;
            Point nearestEdgeStart = new Point();
            Point nearestEdgeEnd = new Point();

            for (int j = 0; j < polygons.Count; j++)
            {
                for (int i = 0; i < polygons[j].GetPointCount(); i++)
                {
                    Point edgeStart = polygons[j].points[i];
                    Point edgeEnd = polygons[j].points[(i + 1) % polygons[j].GetPointCount()];

                    double distance = DistancePointToLine(point, edgeStart, edgeEnd);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestEdgeStart = edgeStart;
                        nearestEdgeEnd = edgeEnd;
                    }
                }
            }
            return (nearestEdgeStart, nearestEdgeEnd);
        }

        private double DistancePointToLine(Point point, Point lineStart, Point lineEnd)
        {
            double x0 = point.X, y0 = point.Y;
            double x1 = lineStart.X, y1 = lineStart.Y;
            double x2 = lineEnd.X, y2 = lineEnd.Y;

            double dx = x2 - x1;
            double dy = y2 - y1;
            double t = ((x0 - x1) * dx + (y0 - y1) * dy) / (dx * dx + dy * dy);

            if (t >= 0 && t <= 1)
            {
                double projX = x1 + t * dx;
                double projY = y1 + t * dy;
                return Math.Sqrt((x0 - projX) * (x0 - projX) + (y0 - projY) * (y0 - projY));
            }
            else
            {
                double distToA = Math.Sqrt((x0 - x1) * (x0 - x1) + (y0 - y1) * (y0 - y1));
                double distToB = Math.Sqrt((x0 - x2) * (x0 - x2) + (y0 - y2) * (y0 - y2));
                return Math.Min(distToA, distToB);
            }
        }

        private bool IsPointInPolygon(Point p, Polygon polygon)
        {
            int intersections = 0;
            for (int j = 0; j < polygons.Count; j++)
            {
                Point extreme = new Point(pictureBox.Width + 1, p.Y);
                for (int i = 0; i < polygons[j].GetPointCount(); i++)
                {
                    if (polygons[j].points[i] == p)
                    {
                        MessageBox.Show("Точка совпадает с вершиной полигона.");
                        return true;
                    }

                    Point v1 = polygons[j].points[i];
                    Point v2 = polygons[j].points[(i + 1) % polygons[j].GetPointCount()];

                    if (IsPointOnEdge(p, v1, v2))
                    {
                        MessageBox.Show("Точка лежит на ребре полигона.");
                        return true;
                    }
                    if (FindIntersection(v1, v2, p, extreme) != null)
                        intersections++;

                }
            }
            return intersections % 2 == 1;
        }

        private bool IsPointOnEdge(Point p, Point v1, Point v2)
        {
            float crossProduct = (p.Y - v1.Y) * (v2.X - v1.X) - (p.X - v1.X) * (v2.Y - v1.Y);

            if (Math.Abs(crossProduct) > float.Epsilon)
                return false;

            bool isWithinX = p.X >= Math.Min(v1.X, v2.X) && p.X <= Math.Max(v1.X, v2.X);
            bool isWithinY = p.Y >= Math.Min(v1.Y, v2.Y) && p.Y <= Math.Max(v1.Y, v2.Y);

            return isWithinX && isWithinY;
        }
        private Point? FindIntersection(Point a, Point b, Point c, Point d)
        {
            Point n = new Point(-(d.Y - c.Y), d.X - c.X);

            float denominator = n.X * (b.X - a.X) + n.Y * (b.Y - a.Y);
            float numerator = -(n.X * (a.X - c.X) + n.Y * (a.Y - c.Y));

            if (Math.Abs(denominator) < 1e-6)
                return null;

            float t = numerator / denominator;

            if (t < 0 || t > 1)
                return null;

            float intersectionX = a.X + t * (b.X - a.X);
            float intersectionY = a.Y + t * (b.Y - a.Y);

            if (IsBetween(a.X, b.X, intersectionX) && IsBetween(a.Y, b.Y, intersectionY) &&
                IsBetween(c.X, d.X, intersectionX) && IsBetween(c.Y, d.Y, intersectionY))
            {
                return new Point((int)intersectionX, (int)intersectionY);
            }
            else
            {
                return null;
            }


        }

        private bool IsBetween(float a, float b, float value)
        {
            return (value >= Math.Min(a, b) && value <= Math.Max(a, b));
        }

        private void searchingBox_TextChanged(object sender, EventArgs e)
        {
            if (searchingBox.Text == "Положение точки")
            {
                isDrawingEdgeForIntersection = false;
                isDeterminingPosition = true;
                clickedPoints.Clear();
                isInPolygon = false;

            }
            if (searchingBox.Text == "Точка пересечения")
            {
                isDeterminingPosition = false;
                clickedPoint = null;
                isDrawingEdgeForIntersection = true;
                isInPolygon = false;
            }
            if (searchingBox.Text == "Принадлежит ли точка полигону")
            {
                isDeterminingPosition = false;
                clickedPoint = null;
                isDrawingEdgeForIntersection = false;
                clickedPoints.Clear();
                isInPolygon = true;
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
                case (int)Action.Scaling1:
                    Scaling(true);
                    break;
                case (int)Action.Scaling2:
                    Scaling();
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

        private Matrix ApplyScaling(Matrix point, double a, double b)
        {
            Matrix translationMatrix = new double[,]
            {
                    { a, 0, 0 },
                    { 0, b, 0 },
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

        private void Scaling(bool useCenter = false)
        {
            double a = (double)numericUpDown2.Value;
            double b = (double)numericUpDown3.Value;

            polygons.ForEach(poly =>
            {
                Point point = (Point)(useCenter ? clickedPoint : poly.GetCenter());

                MyGraphics.ClearPolygon(pictureBox, poly.points);

                for (int i = 0; i < poly.points.Count; i++)
                {
                    poly.points[i] =
                    ApplyMove(
                        ApplyScaling(
                            ApplyMove(
                                poly.points[i],
                                point.X,
                                point.Y
                            ),
                            a,
                            b
                        ),
                    -point.X,
                    -point.Y
                    );
                }

                MyGraphics.DrawPolygon(pictureBox, currColor, poly.points);
            }
            );

            pictureBox.Refresh();
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
