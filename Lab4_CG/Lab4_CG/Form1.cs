using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CG_Lab
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
        bool isSelectingScalePoint = false;
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

       
        private void ClearButton_Click(object sender, EventArgs e)
        {
            polygons.Clear();
            currPolygonInd = 0;

            Bitmap PictureBoxClear = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(PictureBoxClear))
                g.Clear(Color.White);

            polygons.Clear();
            polygons.Add(new Polygon());

            pictureBox.Image = PictureBoxClear;
        }

        private void NewPolygon_Click(object sender, EventArgs e)
        {
            

            if (polygons.Count > currPolygonInd)
            {
                int len = polygons[currPolygonInd].points.Count;
                if (len > 2)
                    MyGraphics.DrawLineVu(pictureBox, currColor, polygons[currPolygonInd].points[len - 1], polygons[currPolygonInd].points[0]);
                currPolygonInd++;
                curr_bitmap = new Bitmap(pictureBox.Image);
            }
            Random r = new Random();
            currColor = Color.FromArgb(r.Next(100, 256), r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            Point mousePoint = new Point(e.Location.X, e.Location.Y);


            if (isSelectingScalePoint)
            {
                // Выбор точки для масштабирования
                clickedPoint = mousePoint;
                DrawScalePoint(mousePoint);
                isSelectingScalePoint = false;
                MessageBox.Show($"Точка для масштабирования установлена: ({mousePoint.X}, {mousePoint.Y})", "Точка установлена");
                return;
            }

            if (polygons.Count == currPolygonInd)
                polygons.Add(new Polygon());

            if (!isDeterminingPosition && !isDrawingEdgeForIntersection && !isInPolygon && !isSelectingScalePoint)
            {
                polygons[currPolygonInd].points.Add(new Point(e.Location.X, e.Location.Y));
                int len = polygons[currPolygonInd].GetPointCount();
                if (len > 1)
                    MyGraphics.DrawLineVu(pictureBox, currColor, polygons[currPolygonInd].points[len - 2], polygons[currPolygonInd].points[len - 1]);
            }
        }

        private void DrawScalePoint(Point point)
        {
            Bitmap pb = new Bitmap(pictureBox.Image);
            using (Graphics g = Graphics.FromImage(pb))
            {
                // Рисуем синий крестик для точки масштабирования
                g.DrawLine(Pens.Blue, point.X - 5, point.Y - 5, point.X + 5, point.Y + 5);
                g.DrawLine(Pens.Blue, point.X - 5, point.Y + 5, point.X + 5, point.Y - 5);
                // Рисуем квадрат вокруг точки
                g.DrawRectangle(Pens.Blue, point.X - 8, point.Y - 8, 16, 16);
            }
            pictureBox.Image = pb;
        }

        

        // Метод для отрисовки точки
        private void DrawPoint(PointF p)
        {
            Bitmap pb = new Bitmap(pictureBox.Image);
            using (Graphics g = Graphics.FromImage(pb))
            {
                g.FillEllipse(Brushes.Red, p.X - 3, p.Y - 3, 6, 6); // Рисуем точку (красный круг)
            }
            pictureBox.Image = pb;
        }

        // Метод для удаления точки с экрана (перерисовка)
        private void ClearPoint()
        {
            pictureBox.Image = curr_bitmap;
        }

        // Классификация точки относительно ребра (слева или справа)
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
        // Функция поиска ближайшего ребра к точке
        private (Point, Point) FindNearestEdge(Point point)
        {
            double minDistance = double.MaxValue;
            Point nearestEdgeStart = new Point();
            Point nearestEdgeEnd = new Point();

            for (int j = 0; j < polygons.Count; j++)
            {
                // Перебираем все рёбра в текущем полигоне
                for (int i = 0; i < polygons[j].GetPointCount(); i++)
                {
                    Point edgeStart = polygons[j].points[i];
                    Point edgeEnd = polygons[j].points[(i + 1) % polygons[j].GetPointCount()];

                    // Вычисляем расстояние от точки до текущего ребра
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

        // Функция вычисления расстояния от точки до ребра
        private double DistancePointToLine(Point point, Point lineStart, Point lineEnd)
        {
            double x0 = point.X, y0 = point.Y;
            double x1 = lineStart.X, y1 = lineStart.Y;
            double x2 = lineEnd.X, y2 = lineEnd.Y;

            // Вычисляем параметр t
            double dx = x2 - x1;
            double dy = y2 - y1;
            double t = ((x0 - x1) * dx + (y0 - y1) * dy) / (dx * dx + dy * dy);

            // Если проекция находится на отрезке
            if (t >= 0 && t <= 1)
            {
                // Рассчитываем проекцию
                double projX = x1 + t * dx;
                double projY = y1 + t * dy;
                return Math.Sqrt((x0 - projX) * (x0 - projX) + (y0 - projY) * (y0 - projY));
            }
            else
            {
                // Если проекция вне отрезка, берем минимальное расстояние до концов отрезка
                double distToA = Math.Sqrt((x0 - x1) * (x0 - x1) + (y0 - y1) * (y0 - y1));
                double distToB = Math.Sqrt((x0 - x2) * (x0 - x2) + (y0 - y2) * (y0 - y2));
                return Math.Min(distToA, distToB);
            }
        }

        // Проверка принадлежности точки полигону (выпуклый или невыпуклый)
        private bool IsPointInPolygon(Point p, Polygon polygon)
        {
            int intersections = 0;
            for (int j = 0; j < polygons.Count; j++)
            {
                Point extreme = new Point(pictureBox.Width + 1, p.Y); // Точка на бесконечности
                // Перебираем все рёбра в текущем полигоне
                for (int i = 0; i < polygons[j].GetPointCount(); i++)
                {
                    if (polygons[j].points[i] == p)
                    {
                        MessageBox.Show("Точка совпадает с вершиной полигона.");
                        return true; // Точка на вершине
                    }

                    Point v1 = polygons[j].points[i];
                    Point v2 = polygons[j].points[(i + 1) % polygons[j].GetPointCount()];

                    if (IsPointOnEdge(p, v1, v2))
                    {
                        MessageBox.Show("Точка лежит на ребре полигона.");
                        return true; // Точка на ребре
                    }
                    if (FindIntersection(v1, v2, p, extreme) != null)
                        intersections++;

                }
                // Нечетное число пересечений — точка внутри
            }
            return intersections % 2 == 1;
        }

        // Проверка, лежит ли точка на ребре
        private bool IsPointOnEdge(Point p, Point v1, Point v2)
        {
            // Проверка, лежит ли точка на линии, соединяющей v1 и v2
            float crossProduct = (p.Y - v1.Y) * (v2.X - v1.X) - (p.X - v1.X) * (v2.Y - v1.Y);

            // Если crossProduct не равен нулю, точка не лежит на линии
            if (Math.Abs(crossProduct) > float.Epsilon)
                return false;

            // Проверяем, что точка p лежит между v1 и v2
            bool isWithinX = p.X >= Math.Min(v1.X, v2.X) && p.X <= Math.Max(v1.X, v2.X);
            bool isWithinY = p.Y >= Math.Min(v1.Y, v2.Y) && p.Y <= Math.Max(v1.Y, v2.Y);

            return isWithinX && isWithinY;
        }
        // Поиск пересечения двух рёбер
        private Point? FindIntersection(Point a, Point b, Point c, Point d)
        {
            // Вектор нормали к отрезку (c -> d)
            Point n = new Point(-(d.Y - c.Y), d.X - c.X);

            // Вычисляем скалярные произведения для формулы
            float denominator = n.X * (b.X - a.X) + n.Y * (b.Y - a.Y);
            float numerator = -(n.X * (a.X - c.X) + n.Y * (a.Y - c.Y));

            // Если denominator == 0, значит отрезки параллельны или совпадают
            if (Math.Abs(denominator) < 1e-6)
                return null;  // Отрезки параллельны или совпадают

            // Вычисляем параметр t для отрезка (a -> b)
            float t = numerator / denominator;

            // Проверяем, лежит ли t в диапазоне от 0 до 1 (то есть на отрезке a -> b)
            if (t < 0 || t > 1)
                return null;  // Пересечение вне отрезка (a -> b)

            // Вычисляем координаты точки пересечения
            float intersectionX = a.X + t * (b.X - a.X);
            float intersectionY = a.Y + t * (b.Y - a.Y);

            // Проверяем, лежит ли точка пересечения на обоих отрезках
            if (IsBetween(a.X, b.X, intersectionX) && IsBetween(a.Y, b.Y, intersectionY) &&
                IsBetween(c.X, d.X, intersectionX) && IsBetween(c.Y, d.Y, intersectionY))
            {
                return new Point((int)intersectionX, (int)intersectionY);
            }
            else
            {
                return null; // Точка не лежит на отрезках
            }
        }

        private bool IsBetween(float a, float b, float value)
        {
            return (value >= Math.Min(a, b) && value <= Math.Max(a, b));
        }

        
        private void transformButton_Click(object sender, EventArgs e)
        {
            if (transformBox.SelectedIndex == (int)Action.Scaling1 && clickedPoint == null)
            {
                MessageBox.Show("Сначала выберите точку для масштабирования! Кликните на pictureBox чтобы установить точку.", "Точка не выбрана");
                isSelectingScalePoint = true;
                return;
            }

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

        private void transformBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (transformBox.SelectedIndex == (int)Action.Scaling1)
            {
                MessageBox.Show("Теперь кликните на pictureBox чтобы выбрать точку для масштабирования", "Выбор точки");
                isSelectingScalePoint = true;
            }
        }

        private Matrix ApplyMove(Matrix point, double dx, double dy)
        {
            Matrix translationMatrix = new double[,]
            {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { dx, dy, 1 }
            };

            return point * translationMatrix;
        }

        private Matrix ApplyRotation(Matrix point, double angle)
        {
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            Matrix rotationMatrix = new double[,]
            {
                { cos, sin, 0 },
                { -sin, cos, 0 },
                { 0, 0, 1 }
            };

            return point * rotationMatrix;
        }

        private Matrix ApplyScaling(Matrix point, double a, double b)
        {
            Matrix scalingMatrix = new double[,]
            {
                { a, 0, 0 },
                { 0, b, 0 },
                { 0, 0, 1 }
            };

            return point * scalingMatrix;
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
                    Matrix newPointMatrix = ApplyMove(poly.points[i], dx, dy);
                    poly.points[i] = newPointMatrix;
                }

                MyGraphics.DrawPolygon(pictureBox, currColor, poly.points);
            });

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
            });

            pictureBox.Refresh();
            curr_bitmap = new Bitmap(pictureBox.Image);
        }

        private void Scaling(bool useCenter = false)
        {
            if (polygons.Count == 0 || polygons[currPolygonInd].points.Count == 0)
            {
                MessageBox.Show("Сначала создайте полигон!", "Ошибка");
                return;
            }

            double a = (double)numericUpDown2.Value;
            double b = (double)numericUpDown3.Value;

            polygons.ForEach(poly =>
            {
                Point scalePoint = useCenter ? (Point)clickedPoint : poly.GetCenter();

                MyGraphics.ClearPolygon(pictureBox, poly.points);

                for (int i = 0; i < poly.points.Count; i++)
                {
                    // Перенос в начало координат -> масштабирование -> обратный перенос
                    Matrix pointAtOrigin = ApplyMove(poly.points[i], -scalePoint.X, -scalePoint.Y);
                    Matrix scaledPoint = ApplyScaling(pointAtOrigin, a, b);
                    Matrix finalPoint = ApplyMove(scaledPoint, scalePoint.X, scalePoint.Y);

                    poly.points[i] = finalPoint;
                }

                MyGraphics.DrawPolygon(pictureBox, currColor, poly.points);
            });

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
            if (points.Count == 0) return new Point(0, 0);

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
            get => Values[row, column];
            set => Values[row, column] = value;
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