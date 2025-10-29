using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CG_Lab
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
        private bool showRotationLine = false;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            currentPolyhedron = PolyHedron.GetCube();
            comboBox1.SelectedIndex = 0;
        }

        private void DrawPolyhedron(PolyHedron polyhedron, string plane)
        {
            float scaleFactor = (float)numericScale.Value;
            g.Clear(pictureBox1.BackColor);

            PolyHedron ph = polyhedron.ScaledAroundCenter(scaleFactor, scaleFactor, scaleFactor);

            float centerY = pictureBox1.Height / 2;

            foreach (var face in ph.Faces)
            {
                var points = new List<PointF>();

                foreach (var vertexIndex in face.Vertices)
                {
                    Vertex v = ph.Vertices[vertexIndex];
                    Vertex projectedVertex = new Vertex(v.X, v.Y, v.Z);
                    PointF projectedPoint = projectedVertex.GetProjection(projectionListBox.SelectedIndex, pictureBox1.Width / 2, pictureBox1.Height / 2,
                        (float)axisXNumeric.Value, (float)axisYNumeric.Value);
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


            PointF startProj = rotationLineStart.GetProjection(projectionListBox.SelectedIndex, pictureBox1.Width / 2, pictureBox1.Height / 2,
                (float)axisXNumeric.Value, (float)axisYNumeric.Value);
            PointF endProj = rotationLineEnd.GetProjection(projectionListBox.SelectedIndex, pictureBox1.Width / 2, pictureBox1.Height / 2,
                (float)axisXNumeric.Value, (float)axisYNumeric.Value);
            g.DrawLine(axisPen, startProj, endProj);





            double cX = 0, cY = 0, cZ = 0;
            polyhedron.FindCenter(polyhedron.Vertices, ref cX, ref cY, ref cZ);
            textBox1.Text = cX.ToString("F2");
            textBox2.Text = cY.ToString("F2");
            textBox3.Text = cZ.ToString("F2");

            pictureBox1.Invalidate();
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

        public PointF GetProjection(int projIndex, float w, float h, float ax, float ay)
        {
            PointF res = new PointF(0, 0);

            switch (projIndex)
            {
                case 0:
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

                case 1:
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


    }
}