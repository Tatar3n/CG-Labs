using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab7_CG
{
    public partial class Form1 : Form
    {
        private enum Figure { DrawCube = 0, DrawTetrahedron, DrawOctahedron, DrawIcosahedron, DrawDodecahedron }
        private enum AffineOp { Move = 0, Scaling, Rotation, LineRotation, AxisXRotation, AxisYRotation, AxisZRotation }
        private enum Operation { Reflect_XY = 0, Reflect_YZ = 1, Reflect_XZ = 2 }

        Graphics g;
        string currPlane = "XY";
        Pen defaultPen = new Pen(Color.Black, 1);
        Pen axisPen = new Pen(Color.Red, 1);

        PolyHedron currentPolyhedron;
        private Vertex rotationLineStart = new Vertex(0, 0, 0);
        private Vertex rotationLineEnd = new Vertex(0, 0, 0);

        private List<Vertex> profilePoints = new List<Vertex>();

        public Form1()
        {
            InitializeComponent();

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);

            currentPolyhedron = PolyHedron.GetCube();
            comboBox1.SelectedIndex = 0;

            if (projectionListBox.Items.Count > 0)
                projectionListBox.SelectedIndex = 0;

            if (comboRevolveAxis.Items.Count > 0)
                comboRevolveAxis.SelectedIndex = 1;

            if (comboFunction.Items.Count > 0)
                comboFunction.SelectedIndex = 0;

            if (reflectionComboBox.Items.Count > 0)
                reflectionComboBox.SelectedIndex = 0;
        }

        private void DrawPolyhedron(PolyHedron polyhedron, string plane)
        {
            if (polyhedron == null) return;

            float scaleFactor = (float)numericScale.Value;
            g.Clear(pictureBox1.BackColor);

            PolyHedron ph = polyhedron.ScaledAroundCenter(scaleFactor, scaleFactor, scaleFactor);

            foreach (var face in ph.Faces)
            {
                var points = new List<PointF>();

                foreach (var vertexIndex in face.Vertices)
                {
                    Vertex v = ph.Vertices[vertexIndex];
                    Vertex projectedVertex = new Vertex(v.X, v.Y, v.Z);
                    PointF projectedPoint = projectedVertex.GetProjection(
                        projectionListBox.SelectedIndex < 0 ? 0 : projectionListBox.SelectedIndex,
                        pictureBox1.Width / 2,
                        pictureBox1.Height / 2,
                        (float)axisXNumeric.Value,
                        (float)axisYNumeric.Value);
                    points.Add(projectedPoint);
                }

                if (points.Count > 1)
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        PointF start = points[i];
                        PointF end = points[(i + 1) % points.Count];
                        g.DrawLine(defaultPen, start, end);
                    }
                }
            }

            PointF startProj = rotationLineStart.GetProjection(
                projectionListBox.SelectedIndex < 0 ? 0 : projectionListBox.SelectedIndex,
                pictureBox1.Width / 2,
                pictureBox1.Height / 2,
                (float)axisXNumeric.Value,
                (float)axisYNumeric.Value);
            PointF endProj = rotationLineEnd.GetProjection(
                projectionListBox.SelectedIndex < 0 ? 0 : projectionListBox.SelectedIndex,
                pictureBox1.Width / 2,
                pictureBox1.Height / 2,
                (float)axisXNumeric.Value,
                (float)axisYNumeric.Value);
            g.DrawLine(axisPen, startProj, endProj);

            double cX = 0, cY = 0, cZ = 0;
            polyhedron.FindCenter(polyhedron.Vertices, ref cX, ref cY, ref cZ);
            textBox1.Text = cX.ToString("F2");
            textBox2.Text = cY.ToString("F2");
            textBox3.Text = cZ.ToString("F2");

            pictureBox1.Invalidate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);

            switch (comboBox1.SelectedIndex)
            {
                case (int)Figure.DrawCube:
                    DrawPolyhedron(currentPolyhedron = PolyHedron.GetCube()
                                             .Scaled(100, 100, 100)
                                             .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0),
                                             currPlane);
                    break;
                case (int)Figure.DrawTetrahedron:
                    DrawPolyhedron(currentPolyhedron = PolyHedron.GetTetrahedron()
                                             .Rotated(10, 10, 0)
                                             .Scaled(100, 100, 100)
                                             .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0),
                                             currPlane);
                    break;
                case (int)Figure.DrawOctahedron:
                    DrawPolyhedron(currentPolyhedron = PolyHedron.GetOctahedron()
                                             .Rotated(20, 20, 0)
                                             .Scaled(100, 100, 100)
                                             .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0),
                                             currPlane);
                    break;
                case (int)Figure.DrawIcosahedron:
                    DrawPolyhedron(currentPolyhedron = PolyHedron.GetIcosahedron()
                                             .Rotated(10, 10, 0)
                                             .Scaled(150, 150, 150)
                                             .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0),
                                             currPlane);
                    break;
                case (int)Figure.DrawDodecahedron:
                    DrawPolyhedron(currentPolyhedron = PolyHedron.GetDodecahedron()
                                             .Rotated(10, 10, 0)
                                             .Scaled(200, 200, 200)
                                             .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0),
                                             currPlane);
                    break;
            }
        }

        private void reflectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentPolyhedron == null) return;

            numericScale.Value = 1;
            g.Clear(pictureBox1.BackColor);

            switch (reflectionComboBox.SelectedIndex)
            {
                case (int)Operation.Reflect_XY:
                    currPlane = "XY";
                    currentPolyhedron = currentPolyhedron.Moved(-pictureBox1.Width / 2, -pictureBox1.Height / 2, 0)
                        .Reflected("XY")
                        .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);
                    DrawPolyhedron(currentPolyhedron, "XY");
                    break;
                case (int)Operation.Reflect_YZ:
                    currPlane = "YZ";
                    currentPolyhedron = currentPolyhedron.Moved(-pictureBox1.Width / 2, -pictureBox1.Height / 2, 0)
                        .Reflected("YZ")
                        .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);
                    DrawPolyhedron(currentPolyhedron, "YZ");
                    break;
                case (int)Operation.Reflect_XZ:
                    currPlane = "XZ";
                    currentPolyhedron = currentPolyhedron.Moved(-pictureBox1.Width / 2, -pictureBox1.Height / 2, 0)
                        .Reflected("XZ")
                        .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);
                    DrawPolyhedron(currentPolyhedron, "XZ");
                    break;
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            numericUpDown12.Value = numericUpDown4.Value;
            numericUpDown11.Value = numericUpDown5.Value;
            numericUpDown4.Value = e.X;
            numericUpDown5.Value = e.Y;

            if (chkProfileMode.Checked)
            {
                float x = e.X - pictureBox1.Width / 2f;
                float y = pictureBox1.Height / 2f - e.Y;
                profilePoints.Add(new Vertex(x, y, 0));
            }
        }

        private void affineOpButton_Click(object sender, EventArgs e)
        {
            if (currentPolyhedron == null) return;

            g.Clear(pictureBox1.BackColor);

            Vertex anchor;
            double centerX, centerY, centerZ;

            switch (comboBox2.SelectedIndex)
            {
                case (int)AffineOp.Move:
                    DrawPolyhedron(currentPolyhedron = currentPolyhedron
                                                       .Moved((float)numericUpDown1.Value, (float)numericUpDown2.Value, (float)numericUpDown3.Value),
                                                       currPlane);
                    break;
                case (int)AffineOp.Scaling:
                    anchor = new Vertex((float)numericUpDown4.Value, (float)numericUpDown5.Value, (float)numericUpDown6.Value);
                    DrawPolyhedron(currentPolyhedron = currentPolyhedron
                                                       .Moved(-anchor.X, -anchor.Y, -anchor.Z)
                                                       .Scaled((float)numericUpDown1.Value, (float)numericUpDown2.Value, (float)numericUpDown3.Value)
                                                       .Moved(anchor.X, anchor.Y, anchor.Z),
                                                       currPlane);
                    break;
                case (int)AffineOp.Rotation:
                    anchor = new Vertex((float)numericUpDown4.Value, (float)numericUpDown5.Value, (float)numericUpDown6.Value);
                    DrawPolyhedron(currentPolyhedron = currentPolyhedron
                                                       .Moved(-anchor.X, -anchor.Y, -anchor.Z)
                                                       .Rotated((float)numericUpDown1.Value, (float)numericUpDown2.Value, (float)numericUpDown3.Value)
                                                       .Moved(anchor.X, anchor.Y, anchor.Z),
                                                       currPlane);
                    break;
                case (int)AffineOp.LineRotation:
                    anchor = new Vertex((float)numericUpDown4.Value, (float)numericUpDown5.Value, (float)numericUpDown6.Value);
                    Vertex v = new Vertex(anchor.X - (float)numericUpDown12.Value, anchor.Y - (float)numericUpDown11.Value, anchor.Z - (float)numericUpDown10.Value);
                    float length = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
                    if (length == 0) return;
                    float l = v.X / length;
                    float m = v.Y / length;
                    float n = v.Z / length;

                    rotationLineStart = new Vertex((float)numericUpDown12.Value, (float)numericUpDown11.Value, (float)numericUpDown10.Value);
                    rotationLineEnd = anchor;

                    DrawPolyhedron(currentPolyhedron = currentPolyhedron
                        .Moved(-anchor.X, -anchor.Y, -anchor.Z)
                        .LineRotated(l, m, n, (float)numericUpDown1.Value)
                        .Moved(anchor.X, anchor.Y, anchor.Z),
                        currPlane);
                    break;
                case (int)AffineOp.AxisXRotation:
                    centerX = 0; centerY = 0; centerZ = 0;
                    currentPolyhedron.FindCenter(currentPolyhedron.Vertices, ref centerX, ref centerY, ref centerZ);
                    DrawPolyhedron(currentPolyhedron = currentPolyhedron
                                                       .Moved((float)-centerX, (float)-centerY, (float)-centerZ)
                                                       .Rotated((float)numericUpDown1.Value, 0, 0)
                                                       .Moved((float)centerX, (float)centerY, (float)centerZ),
                                                       currPlane);
                    break;
                case (int)AffineOp.AxisYRotation:
                    centerX = 0; centerY = 0; centerZ = 0;
                    currentPolyhedron.FindCenter(currentPolyhedron.Vertices, ref centerX, ref centerY, ref centerZ);
                    DrawPolyhedron(currentPolyhedron = currentPolyhedron
                                                       .Moved((float)-centerX, (float)-centerY, (float)-centerZ)
                                                       .Rotated(0, (float)numericUpDown2.Value, 0)
                                                       .Moved((float)centerX, (float)centerY, (float)centerZ),
                                                       currPlane);
                    break;
                case (int)AffineOp.AxisZRotation:
                    centerX = 0; centerY = 0; centerZ = 0;
                    currentPolyhedron.FindCenter(currentPolyhedron.Vertices, ref centerX, ref centerY, ref centerZ);
                    DrawPolyhedron(currentPolyhedron = currentPolyhedron
                                                       .Moved((float)-centerX, (float)-centerY, (float)-centerZ)
                                                       .Rotated(0, 0, (float)numericUpDown2.Value)
                                                       .Moved((float)centerX, (float)centerY, (float)centerZ),
                                                       currPlane);
                    break;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            DrawPolyhedron(currentPolyhedron, currPlane);
        }

        private void axisXNumeric_ValueChanged(object sender, EventArgs e)
        {
            axisZNumeric.Value = 360 - axisXNumeric.Value - axisYNumeric.Value;
            DrawPolyhedron(currentPolyhedron, currPlane);
        }

        private void axisYNumeric_ValueChanged(object sender, EventArgs e)
        {
            axisZNumeric.Value = 360 - axisXNumeric.Value - axisYNumeric.Value;
            DrawPolyhedron(currentPolyhedron, currPlane);
        }

        private void projectionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);
            DrawPolyhedron(currentPolyhedron, currPlane);
        }

        private void btnLoadObj_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Wavefront OBJ (*.obj)|*.obj|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var ph = PolyHedron.LoadFromObj(openFileDialog1.FileName);
                    currentPolyhedron = ph.AutoScaleToFit(pictureBox1.Width, pictureBox1.Height, 20);
                    currPlane = "XY";
                    DrawPolyhedron(currentPolyhedron, currPlane);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки OBJ: " + ex.Message);
                }
            }
        }

        private void btnSaveObj_Click(object sender, EventArgs e)
        {
            if (currentPolyhedron == null)
            {
                MessageBox.Show("Нет модели для сохранения.");
                return;
            }

            saveFileDialog1.Filter = "Wavefront OBJ (*.obj)|*.obj|All files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    currentPolyhedron.SaveAsObj(saveFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения OBJ: " + ex.Message);
                }
            }
        }

        private void btnClearProfile_Click(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);
            profilePoints.Clear();
        }

        private void btnBuildRevolution_Click(object sender, EventArgs e)
        {
            if (profilePoints.Count < 2)
            {
                MessageBox.Show("Сначала задайте образующую (минимум две точки). Включите режим образующей и щёлкайте по области рисования.");
                return;
            }

            int segments = (int)nudRevolveSegments.Value;
            if (segments < 3)
            {
                MessageBox.Show("Количество разбиений должно быть не менее 3.");
                return;
            }

            char axis = 'Y';
            switch (comboRevolveAxis.SelectedIndex)
            {
                case 0: axis = 'X'; break;
                case 1: axis = 'Y'; break;
                case 2: axis = 'Z'; break;
            }

            var ph = PolyHedron.BuildSurfaceOfRevolution(profilePoints, axis, segments);
            currentPolyhedron = ph.AutoScaleToFit(pictureBox1.Width, pictureBox1.Height, 20);
            currPlane = "XY";
            DrawPolyhedron(currentPolyhedron, currPlane);
        }

        private void btnBuildFunctionSurface_Click(object sender, EventArgs e)
        {
            int funcIndex = comboFunction.SelectedIndex;
            if (funcIndex < 0) funcIndex = 0;

            double xMin = (double)nudXMin.Value;
            double xMax = (double)nudXMax.Value;
            double yMin = (double)nudYMin.Value;
            double yMax = (double)nudYMax.Value;

            int nx = (int)nudXSteps.Value;
            int ny = (int)nudYSteps.Value;

            if (xMax <= xMin || yMax <= yMin)
            {
                MessageBox.Show("Проверьте диапазоны X и Y.");
                return;
            }

            if (nx <= 0 || ny <= 0)
            {
                MessageBox.Show("Количество разбиений по осям должно быть больше 0.");
                return;
            }

            var ph = PolyHedron.BuildFunctionSurface(funcIndex, xMin, xMax, yMin, yMax, nx, ny);
            currentPolyhedron = ph.AutoScaleToFit(pictureBox1.Width, pictureBox1.Height, 20);
            currPlane = "XY";
            DrawPolyhedron(currentPolyhedron, currPlane);
        }
    }


    public class Matrix<T> where T : struct, IConvertible
    {
        public T[,] Values { get; }

        public Matrix(T[,] values)
        {
            Values = values;
        }

        public static implicit operator Matrix<T>(T[,] values)
        {
            return new Matrix<T>(values);
        }

        public static implicit operator Vertex(Matrix<T> m)
        {
            return new Vertex(Convert.ToSingle(m[0, 0]), Convert.ToSingle(m[0, 1]), Convert.ToSingle(m[0, 2]));
        }

        public Matrix(Vertex vertex)
        {
            Values = new T[1, 4] {
                  {
                    (T)Convert.ChangeType(vertex.X, typeof(T)),
                    (T)Convert.ChangeType(vertex.Y, typeof(T)),
                    (T)Convert.ChangeType(vertex.Z, typeof(T)),
                    (T)Convert.ChangeType(1, typeof(T))
                  }
            };
        }

        public static implicit operator Matrix<T>(Vertex vertex)
        {
            return new Matrix<T>(vertex);
        }

        public static Matrix<T> operator *(Matrix<T> A, Matrix<T> B)
        {
            int rowsA = A.Values.GetLength(0);
            int colsA = A.Values.GetLength(1);
            int rowsB = B.Values.GetLength(0);
            int colsB = B.Values.GetLength(1);

            T[,] result = new T[rowsA, colsB];

            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    result[i, j] = default;
                    for (int k = 0; k < colsA; k++)
                    {
                        result[i, j] += (dynamic)A.Values[i, k] * (dynamic)B.Values[k, j];
                    }
                }
            }

            return new Matrix<T>(result);
        }

        public T this[int row, int column]
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
    }

    public struct Vertex
    {
        public float X { get; private set; }

        public float Y { get; private set; }

        public float Z { get; private set; }

        public Vertex(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float DistanceTo(in Vertex other)
        {
            float xDelta = X - other.X;
            float yDelta = Y - other.Y;
            float zDelta = Z - other.Z;

            return (float)Math.Sqrt(xDelta * xDelta + yDelta * yDelta + zDelta * zDelta);
        }

        public static Vertex operator *(Vertex v, Matrix<float> m)
        {
            Matrix<float> mv = v;
            Matrix<float> res = mv * m;
            return res;
        }

        public PointF GetProjection(int projIndex, float w, float h, float ax, float ay)
        {
            PointF res = new PointF(0, 0);
            switch (projIndex)
            {
                case 0:
                    {
                        Vertex v = new Vertex(X - w, Y - h, Z);
                        Matrix<float> m = new float[4, 4] {
                        { 1, 0, 0, 0 },
                        { 0, 1, 0, 0 },
                        { 0, 0, 0, 1.0f / 400 },
                        { 0, 0, 0, 1 }
                    };
                        Matrix<float> m1 = v * m;
                        v = new Vertex(m1[0, 0] / m1[0, 3], m1[0, 1] / m1[0, 3], m1[0, 2] / m1[0, 3]);
                        res = new PointF(v.X + w, v.Y + h);
                        break;
                    }
                case 1:
                    {
                        double angleX = ax * (Math.PI / 180);
                        double angleY = ay * (Math.PI / 180);
                        float cosX = (float)Math.Cos(angleX);
                        float cosY = (float)Math.Cos(angleY);
                        float sinX = (float)Math.Sin(angleX);
                        float sinY = (float)Math.Sin(angleY);
                        Matrix<float> m2 = new float[4, 4] {
                        { cosY, sinX * sinY, 0, 0 },
                        { 0, cosX, 0, 0 },
                        { sinY, -sinX * cosY, 0, 0 },
                        { 0, 0, 0, 1 }
                    };
                        Vertex v1 = new Vertex(X - w, Y - h, Z);
                        v1 = v1 * m2;
                        res = new PointF(v1.X + w, v1.Y + h);
                        break;
                    }
            }
            return res;
        }
    }

    public class Face
    {
        public int[] Vertices { get; private set; }

        public Face(params int[] vertices)
        {
            Vertices = vertices;
        }
    }

    public class PolyHedron
    {
        public List<Face> Faces { get; private set; }

        public List<Vertex> Vertices { get; private set; }

        public PolyHedron()
        {
            Faces = new List<Face>();
            Vertices = new List<Vertex>();
        }

        public PolyHedron(List<Face> faces, List<Vertex> vertices)
        {
            Faces = faces;
            Vertices = vertices;
        }

        public void FindCenter(List<Vertex> vertices, ref double a, ref double b, ref double c)
        {
            a = 0;
            b = 0;
            c = 0;
            foreach (var vertex in vertices)
            {
                a += vertex.X;
                b += vertex.Y;
                c += vertex.Z;
            }
            a /= vertices.Count;
            b /= vertices.Count;
            c /= vertices.Count;
        }

        public PolyHedron LineRotated(float l, float m, float n, float angle)
        {
            var newPoly = this.Clone();
            double angleRadians = (double)angle * (Math.PI / 180);
            float cos = (float)Math.Cos(angleRadians);
            float sin = (float)Math.Sin(angleRadians);
            Matrix<float> RxMatrix = new float[4, 4]
            {
                { l*l+cos*(1-l*l), l*(1-cos)*m+n*sin,  l*(1-cos)*n-m*sin,  0 },
                { l*(1-cos)*m-n*sin, m*m+cos*(1-m*m), m*(1-cos)*n+l*sin,  0 },
                { l*(1-cos)*n+m*sin, m*(1-cos)*n-l*sin,  n*n+cos*(1-n*n),  0 },
                { 0,  0,  0,  1 }
            };
            for (int i = 0; i < newPoly.Vertices.Count; i++)
                newPoly.Vertices[i] *= RxMatrix;
            return newPoly;
        }

        public PolyHedron ApplyRx(float l, float m, float n, bool reverse = false)
        {
            var newPoly = this.Clone();

            float d = (float)Math.Sqrt(m * m + n * n);
            float mult = reverse ? -1 : 1;

            Matrix<float> RxMatrix = new float[4, 4]
            {
                { 1,  0,  0,  0 },
                { 0,  n/d, m/d * mult,  0 },
                { 0,  -m/d * mult,  n/d,  0 },
                { 0,  0,  0,  1 }
            };

            for (int i = 0; i < newPoly.Vertices.Count; i++)
            {
                newPoly.Vertices[i] *= RxMatrix;
            }

            return newPoly;
        }

        public PolyHedron ApplyRy(float l, float m, float n, bool reverse = false)
        {
            var newPoly = this.Clone();

            float d = (float)Math.Sqrt(m * m + n * n);
            d = reverse ? -d : d;

            Matrix<float> RyMatrix = new float[4, 4]
            {
                { l,  0,  d,  0 },
                { 0,  1, 0,  0 },
                { -d,  0,  l,  0 },
                { 0,  0,  0,  1 }
            };

            for (int i = 0; i < newPoly.Vertices.Count; i++)
            {
                newPoly.Vertices[i] *= RyMatrix;
            }

            return newPoly;
        }

        public PolyHedron ApplyRz(float angle)
        {
            var newPoly = this.Clone();

            double angleRadians = (double)angle * (Math.PI / 180);

            float cos = (float)Math.Cos(angleRadians);
            float sin = (float)Math.Sin(angleRadians);

            Matrix<float> RzMatrix = new float[4, 4]
            {
                { cos,  sin,  0,  0 },
                { -sin,  cos, 0,  0 },
                { 0,  0,  1,  0 },
                { 0,  0,  0,  1 }
            };

            for (int i = 0; i < newPoly.Vertices.Count; i++)
            {
                newPoly.Vertices[i] *= RzMatrix;
            }

            return newPoly;
        }

        public PolyHedron Scaled(float c1, float c2, float c3)
        {
            var newPoly = this.Clone();
            Matrix<float> translationMatrix = new float[4, 4]
            {
                { c1, 0,  0,  0 },
                { 0,  c2, 0,  0 },
                { 0,  0,  c3, 0 },
                { 0,  0,  0,  1 }
            };
            for (int i = 0; i < newPoly.Vertices.Count; i++)
                newPoly.Vertices[i] *= translationMatrix;
            return newPoly;
        }

        public PolyHedron Moved(float a, float b, float c)
        {
            var newPoly = this.Clone();

            Matrix<float> translationMatrix = new float[4, 4]
            {
                { 1,  0,  0,  0 },
                { 0,  1,  0,  0 },
                { 0,  0,  1,  0 },
                { a,  b,  c,  1 }
            };

            for (int i = 0; i < newPoly.Vertices.Count; i++)
            {
                newPoly.Vertices[i] *= translationMatrix;
            }

            return newPoly;
        }

        public PolyHedron RotatedXAxis(float alpha)
        {
            var newPoly = this.Clone();

            double angleRadians = (double)alpha * (Math.PI / 180);

            float cos = (float)Math.Cos(angleRadians);
            float sin = (float)Math.Sin(angleRadians);

            Matrix<float> translationMatrix = new float[4, 4]
            {
                { 1,  0,  0,  0 },
                { 0,  cos, sin,  0 },
                { 0,  -sin,  cos,  0 },
                { 0,  0,  0,  1 }
            };

            for (int i = 0; i < newPoly.Vertices.Count; i++)
            {
                newPoly.Vertices[i] *= translationMatrix;
            }

            return newPoly;
        }

        public PolyHedron RotatedYAxis(float alpha)
        {
            var newPoly = this.Clone();

            double angleRadians = (double)alpha * (Math.PI / 180);

            float cos = (float)Math.Cos(angleRadians);
            float sin = (float)Math.Sin(angleRadians);

            Matrix<float> translationMatrix = new float[4, 4]
            {
                { cos,  0,  -sin,  0 },
                { 0,  1, 0,  0 },
                { sin,  0,  cos,  0 },
                { 0,  0,  0,  1 }
            };

            for (int i = 0; i < newPoly.Vertices.Count; i++)
            {
                newPoly.Vertices[i] *= translationMatrix;
            }

            return newPoly;
        }

        public PolyHedron RotatedZAxis(float alpha)
        {
            var newPoly = this.Clone();

            double angleRadians = (double)alpha * (Math.PI / 180);

            float cos = (float)Math.Cos(angleRadians);
            float sin = (float)Math.Sin(angleRadians);

            Matrix<float> translationMatrix = new float[4, 4]
            {
                { cos,  sin,  0,  0 },
                { -sin, cos, 0,  0 },
                { 0,  0,  1,  0 },
                { 0,  0,  0,  1 }
            };

            for (int i = 0; i < newPoly.Vertices.Count; i++)
            {
                newPoly.Vertices[i] *= translationMatrix;
            }

            return newPoly;
        }

        public PolyHedron Rotated(float xAngle, float yAngle, float zAngle)
        {
            return this.Clone()
                .RotatedXAxis(xAngle)
                .RotatedYAxis(yAngle)
                .RotatedZAxis(zAngle);
        }

        // Платоновы тела
        public static PolyHedron GetCube()
        {
            var cube = new PolyHedron();

            cube.Vertices.Add(new Vertex(-1, -1, -1));
            cube.Vertices.Add(new Vertex(1, -1, -1));
            cube.Vertices.Add(new Vertex(1, 1, -1));
            cube.Vertices.Add(new Vertex(-1, 1, -1));
            cube.Vertices.Add(new Vertex(-1, -1, 1));
            cube.Vertices.Add(new Vertex(1, -1, 1));
            cube.Vertices.Add(new Vertex(1, 1, 1));
            cube.Vertices.Add(new Vertex(-1, 1, 1));

            cube.Faces.Add(new Face(0, 1, 2, 3));
            cube.Faces.Add(new Face(4, 5, 6, 7));
            cube.Faces.Add(new Face(0, 1, 5, 4));
            cube.Faces.Add(new Face(3, 2, 6, 7));
            cube.Faces.Add(new Face(1, 2, 6, 5));
            cube.Faces.Add(new Face(0, 3, 7, 4));

            return cube;
        }

        public static PolyHedron GetTetrahedron()
        {
            var tetra = new PolyHedron();

            tetra.Vertices.Add(new Vertex(-1, 1, -1));
            tetra.Vertices.Add(new Vertex(1, -1, -1));
            tetra.Vertices.Add(new Vertex(1, 1, 1));
            tetra.Vertices.Add(new Vertex(-1, -1, 1));

            tetra.Faces.Add(new Face(0, 1, 2));
            tetra.Faces.Add(new Face(0, 1, 3));
            tetra.Faces.Add(new Face(0, 2, 3));
            tetra.Faces.Add(new Face(1, 2, 3));

            return tetra;
        }

        public static PolyHedron GetOctahedron()
        {
            var cube = GetCube();

            var octa = new PolyHedron();

            foreach (Face face in cube.Faces)
            {
                octa.Vertices.Add(cube.GetFaceCenter(face));
            }

            var octaCenters = cube.Scaled(1 / 3f, 1 / 3f, 1 / 3f).Vertices;

            for (int i = 0; i < 8; i++)
            {
                var faceVertices = octa.Vertices.Select((v, ind) => (v, ind))
                                                .OrderBy(p => octaCenters[i].DistanceTo(in p.v))
                                                .Select(p => p.ind)
                                                .Take(3);

                octa.Faces.Add(new Face(faceVertices.ToArray()));
            }

            return octa;
        }

        public static PolyHedron GetIcosahedron()
        {
            var icosa = new PolyHedron();

            var verticesBottom = new List<(Vertex v, int number)>(5);
            var verticesTop = new List<(Vertex v, int number)>(5);

            double angle = -90;
            int number = 1;

            for (int i = 0; i < 5; i++)
            {
                var angleRadians = angle * (Math.PI / 180);
                verticesBottom.Add((new Vertex((float)Math.Cos(angleRadians), -0.5f, (float)Math.Sin(angleRadians)), number));
                angle += 72;
                number += 2;
            }

            angle = -54;
            number = 2;

            for (int i = 0; i < 5; i++)
            {
                var angleRadians = angle * (Math.PI / 180);
                verticesTop.Add((new Vertex((float)Math.Cos(angleRadians), 0.5f, (float)Math.Sin(angleRadians)), number));
                angle += 72;
                number += 2;
            }

            icosa.Vertices = verticesBottom.Concat(verticesTop).OrderBy(p => p.number).Select(p => p.v).ToList();

            for (int i = 1; i <= 8; i++)
            {
                icosa.Faces.Add(new Face(i - 1, i, i + 1));
            }

            icosa.Faces.Add(new Face(8, 9, 0));
            icosa.Faces.Add(new Face(9, 0, 1));

            icosa.Vertices.Add(new Vertex(0, -(float)Math.Sqrt(5) / 2, 0));
            icosa.Vertices.Add(new Vertex(0, (float)Math.Sqrt(5) / 2, 0));

            number = 1;

            for (int i = 0; i < 4; i++)
            {
                icosa.Faces.Add(new Face(10, number - 1, number + 1));
                number += 2;
            }

            icosa.Faces.Add(new Face(10, 8, 0));

            number = 2;

            for (int i = 0; i < 4; i++)
            {
                icosa.Faces.Add(new Face(11, number - 1, number + 1));
                number += 2;
            }

            icosa.Faces.Add(new Face(11, 9, 1));

            return icosa;
        }

        public static PolyHedron GetDodecahedron()
        {
            var icosa = GetIcosahedron();

            var dodeca = new PolyHedron();

            foreach (Face face in icosa.Faces)
            {
                dodeca.Vertices.Add(icosa.GetFaceCenter(face));
            }

            for (int i = 0; i < 12; i++)
            {
                var faceVertices = dodeca.Vertices.Select((v, ind) => (v, ind))
                                                .OrderBy(p => icosa.Vertices[i].DistanceTo(in p.v))
                                                .Select(p => p.ind)
                                                .Take(5);

                int first = faceVertices.First();

                var rest = faceVertices.Skip(1).Select(ind => (dodeca.Vertices[ind], ind)).OrderBy(p => dodeca.Vertices[first].DistanceTo(in p.Item1));

                var next = rest.First().ind;

                var lastTwo = rest.Skip(2).OrderBy(p => dodeca.Vertices[next].DistanceTo(in p.Item1));

                dodeca.Faces.Add(new Face(faceVertices.Take(1)
                                          .Concat(new int[1] { next })
                                          .Concat(lastTwo.Select(p => p.ind))
                                          .Concat(rest.Select(p => p.ind).Skip(1).Take(1))
                                          .ToArray()
                                 ));
            }

            return dodeca;
        }

        public Vertex GetFaceCenter(Face face)
        {
            float x = 0, y = 0, z = 0;

            foreach (int vertexIndex in face.Vertices)
            {
                x += Vertices[vertexIndex].X;
                y += Vertices[vertexIndex].Y;
                z += Vertices[vertexIndex].Z;
            }

            return new Vertex(x / face.Vertices.Length, y / face.Vertices.Length, z / face.Vertices.Length);
        }

        public PolyHedron Clone()
        {
            var newPoly = new PolyHedron(this.Faces, new List<Vertex>(this.Vertices));
            return newPoly;
        }

        public PolyHedron Reflected(string plane)
        {
            var newPoly = this.Clone();

            Matrix<float> reflectionMatrix;

            switch (plane.ToUpper())
            {
                case "XY":
                    reflectionMatrix = new float[4, 4]
                    {
                        { 1,  0,  0,  0 },
                        { 0,  1,  0,  0 },
                        { 0,  0, -1,  0 },
                        { 0,  0,  0,  1 }
                    };
                    break;

                case "YZ":
                    reflectionMatrix = new float[4, 4]
                    {
                        { -1,  0,  0,  0 },
                        { 0,  1,  0,  0 },
                        { 0,  0,  1,  0 },
                        { 0,  0,  0,  1 }
                    };
                    break;

                case "XZ":
                    reflectionMatrix = new float[4, 4]
                    {
                        { 1,  0,  0,  0 },
                        { 0, -1,  0,  0 },
                        { 0,  0,  1,  0 },
                        { 0,  0,  0,  1 }
                    };
                    break;

                default:
                    throw new ArgumentException("Invalid plane");
            }

            for (int i = 0; i < newPoly.Vertices.Count; i++)
            {
                newPoly.Vertices[i] *= reflectionMatrix;
            }

            return newPoly;
        }

        public PolyHedron ScaledAroundCenter(float scaleX, float scaleY, float scaleZ)
        {
            var newPoly = this.Clone();
            double centerX = 0, centerY = 0, centerZ = 0;
            FindCenter(newPoly.Vertices, ref centerX, ref centerY, ref centerZ);
            Matrix<float> moveToOriginMatrix = new float[4, 4]
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, 0 },
                { (float)-centerX, (float)-centerY, (float)-centerZ, 1 }
            };
            for (int i = 0; i < newPoly.Vertices.Count; i++)
                newPoly.Vertices[i] *= moveToOriginMatrix;
            Matrix<float> scalingMatrix = new float[4, 4]
            {
                { scaleX, 0, 0, 0 },
                { 0, scaleY, 0, 0 },
                { 0, 0, scaleZ, 0 },
                { 0, 0, 0, 1 }
            };
            for (int i = 0; i < newPoly.Vertices.Count; i++)
                newPoly.Vertices[i] *= scalingMatrix;
            Matrix<float> moveBackMatrix = new float[4, 4]
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, 0 },
                { (float)centerX, (float)centerY, (float)centerZ, 1 }
            };
            for (int i = 0; i < newPoly.Vertices.Count; i++)
                newPoly.Vertices[i] *= moveBackMatrix;
            return newPoly;
        }

        public PolyHedron AutoScaleToFit(int width, int height, float margin)
        {
            if (Vertices.Count == 0) return this;

            float minX = Vertices.Min(v => v.X);
            float maxX = Vertices.Max(v => v.X);
            float minY = Vertices.Min(v => v.Y);
            float maxY = Vertices.Max(v => v.Y);
            float minZ = Vertices.Min(v => v.Z);
            float maxZ = Vertices.Max(v => v.Z);

            float sizeX = maxX - minX;
            float sizeY = maxY - minY;
            if (sizeX == 0) sizeX = 1;
            if (sizeY == 0) sizeY = 1;

            float scaleX = (width - 2 * margin) / sizeX;
            float scaleY = (height - 2 * margin) / sizeY;
            float scale = Math.Min(scaleX, scaleY);

            float cx = (minX + maxX) / 2f;
            float cy = (minY + maxY) / 2f;
            float cz = (minZ + maxZ) / 2f;

            return this.Clone()
                .Moved(-cx, -cy, -cz)
                .Scaled(scale, scale, scale)
                .Moved(width / 2f, height / 2f, 0);
        }

     

        public static PolyHedron BuildSurfaceOfRevolution(IList<Vertex> profile, char axis, int segments)
        {
            if (profile == null || profile.Count < 2)
                throw new ArgumentException("Профиль должен содержать как минимум две точки.");

            var vertices = new List<Vertex>();
            var faces = new List<Face>();

            double angleStep = 2.0 * Math.PI / segments;

            for (int k = 0; k < segments; k++)
            {
                double angle = k * angleStep;
                float cos = (float)Math.Cos(angle);
                float sin = (float)Math.Sin(angle);

                foreach (var p in profile)
                {
                    float x = p.X;
                    float y = p.Y;
                    float z = p.Z;
                    float xr = x, yr = y, zr = z;

                    switch (axis)
                    {
                        case 'X':
                            yr = y * cos - z * sin;
                            zr = y * sin + z * cos;
                            xr = x;
                            break;
                        case 'Y':
                        default:
                            xr = x * cos + z * sin;
                            zr = -x * sin + z * cos;
                            yr = y;
                            break;
                        case 'Z':
                            xr = x * cos - y * sin;
                            yr = x * sin + y * cos;
                            zr = z;
                            break;
                    }

                    vertices.Add(new Vertex(xr, yr, zr));
                }
            }

            int m = profile.Count;

            for (int k = 0; k < segments; k++)
            {
                int nextBand = (k + 1) % segments;
                for (int j = 0; j < m - 1; j++)
                {
                    int curr = k * m + j;
                    int currNext = k * m + j + 1;
                    int next = nextBand * m + j;
                    int nextNext = nextBand * m + j + 1;

                    faces.Add(new Face(curr, next, nextNext, currNext));
                }
            }

            return new PolyHedron(faces, vertices);
        }

           public static PolyHedron LoadFromObj(string path)
   {
       var vertices = new List<Vertex>();
       var faces = new List<Face>();

       foreach (var line in File.ReadAllLines(path))
       {
           var trimmed = line.Trim();
           if (trimmed.Length == 0 || trimmed.StartsWith("#")) continue;

           if (trimmed.StartsWith("v "))
           {
               var parts = trimmed.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
               if (parts.Length >= 4)
               {
                   float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                   float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                   float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                   vertices.Add(new Vertex(x, y, z));
               }
           }
           else if (trimmed.StartsWith("f "))
           {
               var parts = trimmed.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
               var idxs = new List<int>();
               for (int i = 1; i < parts.Length; i++)
               {
                   var token = parts[i];
                   var idxParts = token.Split('/');
                   if (int.TryParse(idxParts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int idx))
                   {
                       idxs.Add(idx - 1);
                   }
               }
               if (idxs.Count >= 3)
               {
                   faces.Add(new Face(idxs.ToArray()));
               }
           }
       }

       return new PolyHedron(faces, vertices);
   }

   public void SaveAsObj(string path)
   {
       using (var sw = new StreamWriter(path))
       {

           foreach (var v in Vertices)
           {
               sw.WriteLine(string.Format(
                   CultureInfo.InvariantCulture,
                   "v {0} {1} {2}",
                   v.X, v.Y, v.Z));
           }

           foreach (var f in Faces)
           {
               var line = "f " + string.Join(" ", f.Vertices.Select(i => (i + 1).ToString(CultureInfo.InvariantCulture)));
               sw.WriteLine(line);
           }
       }
   }



    }
}
