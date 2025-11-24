using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Lab7_CG
{
    public partial class Form1 : Form
    {
        private enum Figure { DrawCube = 0, DrawTetrahedron, DrawOctahedron, DrawIcosahedron, DrawDodecahedron }
        private enum AffineOp { Move = 0, Scaling, Rotation, LineRotation, AxisXRotation, AxisYRotation, AxisZRotation }
        private enum Operation { Reflect_XY = 0, Reflect_YZ = 1, Reflect_XZ = 2 }
        public enum Projection { Perspective, Orthographic }
        public enum ShadingMode { Flat, GouraudLambert, PhongToon, Texture }

        Graphics g;
        string currPlane = "XY";
        Pen defaultPen = new Pen(Color.Black, 1);
        Pen axisPen = new Pen(Color.Red, 1);

        PolyHedron currentPolyhedron;
        private Vertex rotationLineStart = new Vertex(0, 0, 0);
        private Vertex rotationLineEnd = new Vertex(0, 0, 0);

        private List<Vertex> profilePoints = new List<Vertex>();

        private Camera camera;
        private Timer animationTimer;
        private float rotationAngle = 0;
        private bool useZBuffer = true;
        private Projection currentProjection = Projection.Perspective;
        private ShadingMode currentShading = ShadingMode.GouraudLambert;
        private float cameraMoveSpeed = 10f;
        private float cameraRotationSpeed = 2f;

        // Освещение
        private Vertex lightPosition = new Vertex(200, 200, 500);
        private Color objectColor = Color.LightSkyBlue;
        private Bitmap texture;

        // UI элементы для света
        private NumericUpDown numericLightX;
        private NumericUpDown numericLightY;
        private NumericUpDown numericLightZ;
        private Label lblLightPos;
        private Button btnUpdateLight;
        private ColorDialog colorDialog1;
        private Button btnObjectColor;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();

            camera = new Camera(
                new Vertex(pictureBox1.Width / 2, pictureBox1.Height / 2, 10000),
                yaw: 0,
                pitch: -10
            );

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);

            try
            {
                texture = CreateTestGridTexture(512, 512);
            }
            catch
            {
                texture = null;
            }

            currentPolyhedron = PolyHedron.GetCube()
                                              .Scaled(100, 100, 100)
                                              .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);
            comboBox1.SelectedIndex = 0;

            if (projectionListBox.Items.Count > 0)
                projectionListBox.SelectedIndex = 0;

            if (comboRevolveAxis.Items.Count > 0)
                comboRevolveAxis.SelectedIndex = 1;

            if (comboFunction.Items.Count > 0)
                comboFunction.SelectedIndex = 0;

            if (reflectionComboBox.Items.Count > 0)
                reflectionComboBox.SelectedIndex = 0;

            if (shadingComboBox.Items.Count > 0)
                shadingComboBox.SelectedIndex = 1;

            RenderScene();
            UpdateCameraInfo();
        }

        private Bitmap CreateTestGridTexture(int width, int height)
        {
            var texture = new Bitmap(width, height);
            using (var g = Graphics.FromImage(texture))
            {
                // Заливаем белым
                g.Clear(Color.White);

                // Рисуем сетку
                using (var pen = new Pen(Color.Red, 2))
                {
                    // Вертикальные линии
                    for (int x = 0; x < width; x += width / 4)
                    {
                        g.DrawLine(pen, x, 0, x, height);
                    }

                    // Горизонтальные линии
                    for (int y = 0; y < height; y += height / 3)
                    {
                        g.DrawLine(pen, 0, y, width, y);
                    }
                }

                // Добавляем номера для ориентации
                using (var font = new Font("Arial", 20))
                using (var brush = new SolidBrush(Color.Blue))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            g.DrawString($"{i},{j}", font, brush, i * (width / 4) + 10, j * (height / 3) + 10);
                        }
                    }
                }
            }
            return texture;
        }

        private Button btnLoadTexture;
        private Button btnResetTexture;
        private Label lblTextureInfo;
        private void InitializeCustomComponents()
        {
            // btnLoadTexture
            this.btnLoadTexture = new Button();
            this.btnLoadTexture.Location = new Point(1300, 240);
            this.btnLoadTexture.Name = "btnLoadTexture";
            this.btnLoadTexture.Size = new Size(95, 27);
            this.btnLoadTexture.TabIndex = 45;
            this.btnLoadTexture.Text = "Загрузить текстуру";
            this.btnLoadTexture.UseVisualStyleBackColor = true;
            this.btnLoadTexture.Click += new EventHandler(this.btnLoadTexture_Click);

            // btnResetTexture
            this.btnResetTexture = new Button();
            this.btnResetTexture.Location = new Point(1405, 240);
            this.btnResetTexture.Name = "btnResetTexture";
            this.btnResetTexture.Size = new Size(95, 27);
            this.btnResetTexture.TabIndex = 46;
            this.btnResetTexture.Text = "Сбросить текстуру";
            this.btnResetTexture.UseVisualStyleBackColor = true;
            this.btnResetTexture.Click += new EventHandler(this.btnResetTexture_Click);

            // lblTextureInfo
            this.lblTextureInfo = new Label();
            this.lblTextureInfo.AutoSize = true;
            this.lblTextureInfo.Location = new Point(1300, 270);
            this.lblTextureInfo.Name = "lblTextureInfo";
            this.lblTextureInfo.Size = new Size(120, 16);
            this.lblTextureInfo.TabIndex = 47;
            this.lblTextureInfo.Text = "Текстура: стандартная";

            // Добавление компонентов на форму
            this.Controls.Add(this.btnLoadTexture);
            this.Controls.Add(this.btnResetTexture);
            this.Controls.Add(this.lblTextureInfo);

            // Инициализация компонентов для управления светом
            this.lblLightPos = new Label();
            this.numericLightX = new NumericUpDown();
            this.numericLightY = new NumericUpDown();
            this.numericLightZ = new NumericUpDown();
            this.btnUpdateLight = new Button();
            this.btnObjectColor = new Button();
            this.colorDialog1 = new ColorDialog();

            // lblLightPos
            this.lblLightPos.AutoSize = true;
            this.lblLightPos.Location = new Point(1300, 120);
            this.lblLightPos.Name = "lblLightPos";
            this.lblLightPos.Size = new Size(130, 16);
            this.lblLightPos.TabIndex = 42;
            this.lblLightPos.Text = "Источник света X,Y,Z";

            // numericLightX
            this.numericLightX.Location = new Point(1300, 140);
            this.numericLightX.Minimum = -1000;
            this.numericLightX.Maximum = 1000;
            this.numericLightX.Value = 200;
            this.numericLightX.Width = 60;

            // numericLightY
            this.numericLightY.Location = new Point(1370, 140);
            this.numericLightY.Minimum = -1000;
            this.numericLightY.Maximum = 1000;
            this.numericLightY.Value = 200;
            this.numericLightY.Width = 60;

            // numericLightZ
            this.numericLightZ.Location = new Point(1440, 140);
            this.numericLightZ.Minimum = -1000;
            this.numericLightZ.Maximum = 10000;
            this.numericLightZ.Value = 500;
            this.numericLightZ.Width = 60;

            // btnUpdateLight
            this.btnUpdateLight.Location = new Point(1300, 170);
            this.btnUpdateLight.Name = "btnUpdateLight";
            this.btnUpdateLight.Size = new Size(200, 27);
            this.btnUpdateLight.TabIndex = 43;
            this.btnUpdateLight.Text = "Обновить источник света";
            this.btnUpdateLight.UseVisualStyleBackColor = true;
            this.btnUpdateLight.Click += new EventHandler(this.btnUpdateLight_Click);

            // btnObjectColor
            this.btnObjectColor.Location = new Point(1300, 205);
            this.btnObjectColor.Name = "btnObjectColor";
            this.btnObjectColor.Size = new Size(200, 27);
            this.btnObjectColor.TabIndex = 44;
            this.btnObjectColor.Text = "Цвет объекта...";
            this.btnObjectColor.UseVisualStyleBackColor = true;
            this.btnObjectColor.Click += new EventHandler(this.btnObjectColor_Click);

            // Добавление компонентов на форму
            this.Controls.Add(this.lblLightPos);
            this.Controls.Add(this.numericLightX);
            this.Controls.Add(this.numericLightY);
            this.Controls.Add(this.numericLightZ);
            this.Controls.Add(this.btnUpdateLight);
            this.Controls.Add(this.btnObjectColor);
        }

        private void btnLoadTexture_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png;*.gif)|*.bmp;*.jpg;*.jpeg;*.png;*.gif|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var newTexture = new Bitmap(openFileDialog1.FileName);
                    texture = newTexture;
                    lblTextureInfo.Text = $"Текстура: {System.IO.Path.GetFileName(openFileDialog1.FileName)}";
                    RenderScene();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки текстуры: " + ex.Message);
                }
            }
        }

        private void btnResetTexture_Click(object sender, EventArgs e)
        {
            try
            {
                texture = new Bitmap(64, 64);
                using (var gTex = Graphics.FromImage(texture))
                {
                    gTex.Clear(Color.White);
                    using (var brush = new SolidBrush(Color.Red))
                        gTex.FillRectangle(brush, 0, 0, 32, 32);
                    using (var brush = new SolidBrush(Color.Blue))
                        gTex.FillRectangle(brush, 32, 0, 32, 32);
                    using (var brush = new SolidBrush(Color.Green))
                        gTex.FillRectangle(brush, 0, 32, 32, 32);
                    using (var brush = new SolidBrush(Color.Yellow))
                        gTex.FillRectangle(brush, 32, 32, 32, 32);
                }
                lblTextureInfo.Text = "Текстура: стандартная";
                RenderScene();
            }
            catch
            {
                texture = null;
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (currentPolyhedron == null) return;

            bool cameraMoved = false;
            float moveSpeed = 10f;
            float rotationSpeed = 3f;

            switch (e.KeyCode)
            {
                case Keys.W:
                    camera.Move(camera.Direction * moveSpeed);
                    cameraMoved = true;
                    break;

                case Keys.S:
                    camera.Move(-camera.Direction * moveSpeed);
                    cameraMoved = true;
                    break;

                case Keys.A:
                    camera.Move(-camera.Right * moveSpeed);
                    cameraMoved = true;
                    break;

                case Keys.D:
                    camera.Move(camera.Right * moveSpeed);
                    cameraMoved = true;
                    break;

                case Keys.Q:
                    camera.Move(new Vertex(0, moveSpeed, 0));
                    cameraMoved = true;
                    break;

                case Keys.E:
                    camera.Move(new Vertex(0, -moveSpeed, 0));
                    cameraMoved = true;
                    break;

                case Keys.Left:
                    camera.Rotate(-rotationSpeed, 0);
                    cameraMoved = true;
                    break;

                case Keys.Right:
                    camera.Rotate(rotationSpeed, 0);
                    cameraMoved = true;
                    break;

                case Keys.Up:
                    camera.Rotate(0, rotationSpeed);
                    cameraMoved = true;
                    break;

                case Keys.Down:
                    camera.Rotate(0, -rotationSpeed);
                    cameraMoved = true;
                    break;

                case Keys.R:
                    ResetCamera();
                    cameraMoved = true;
                    break;
            }

            if (cameraMoved)
            {
                UpdateCameraInfo();
                RenderScene();
                e.Handled = true;
            }
        }

        private void ResetCamera()
        {
            camera.Position = new Vertex(pictureBox1.Width / 2, pictureBox1.Height / 2, 10000);
            camera.Yaw = 0;
            camera.Pitch = -10;
            camera.UpdateVectors();
        }

        private void UpdateCameraInfo()
        {
            textBox1.Text = $"Pos: ({camera.Position.X:F1}, {camera.Position.Y:F1}, {camera.Position.Z:F1})";
            textBox2.Text = $"Dir: ({camera.Direction.X:F2}, {camera.Direction.Y:F2}, {camera.Direction.Z:F2})";
            textBox3.Text = $"Yaw: {camera.Yaw:F1}°, Pitch: {camera.Pitch:F1}°";
        }

        private void RenderScene()
        {
            g.Clear(pictureBox1.BackColor);

            if (useZBuffer)
                RenderWithZBuffer();
            else
                RenderPolyhedron(currentPolyhedron);

            pictureBox1.Invalidate();
        }

        private void RenderWithZBuffer()
        {
            if (currentPolyhedron == null) return;

            var transformedPolyhedron = ApplyCameraTransform(currentPolyhedron);
            transformedPolyhedron.RenderWithZBuffer(
                g, pictureBox1, camera, currentProjection,
                currentShading, lightPosition, objectColor, texture);
        }

        private void RenderPolyhedron(PolyHedron polyhedron)
        {
            if (polyhedron == null) return;

            float scaleFactor = (float)1;
            PolyHedron scaledPolyhedron = polyhedron.ScaledAroundCenter(scaleFactor, scaleFactor, scaleFactor);

            var transformedPolyhedron = ApplyCameraTransform(scaledPolyhedron);

            // Простой рендеринг линиями
            foreach (var face in transformedPolyhedron.Faces)
            {
                var points = new List<PointF>();
                foreach (var vertexIndex in face.Vertices)
                {
                    Vertex v = transformedPolyhedron.Vertices[vertexIndex];
                    PointF projectedPoint = v.GetProjection(
                        currentProjection == Projection.Perspective ? 0 : 1,
                        pictureBox1.Width / 2,
                        pictureBox1.Height / 2,
                        120, 120);
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

            pictureBox1.Invalidate();
        }

        private PolyHedron ApplyCameraTransform(PolyHedron polyhedron)
        {
            var transformed = polyhedron.Clone();
            var viewMatrix = camera.ViewMatrix;

            for (int i = 0; i < transformed.Vertices.Count; i++)
            {
                Vertex worldVertex = transformed.Vertices[i];

                Matrix<float> vertexMatrix = new float[1, 4] {
            { worldVertex.X, worldVertex.Y, worldVertex.Z, 1 }
        };

                Matrix<float> transformedVertex = vertexMatrix * viewMatrix;

                // Сохраняем UV-координаты!
                transformed.Vertices[i] = new Vertex(
                    transformedVertex[0, 0],
                    transformedVertex[0, 1],
                    transformedVertex[0, 2],
                    worldVertex.U,  // Сохраняем исходные UV
                    worldVertex.V   // Сохраняем исходные UV
                );
            }

            return transformed;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);

            switch (comboBox1.SelectedIndex)
            {
                case (int)Figure.DrawCube:
                    currentPolyhedron = PolyHedron.GetCube()
                                             .Scaled(100, 100, 100)
                                             .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);
                    break;
                case (int)Figure.DrawTetrahedron:
                    currentPolyhedron = PolyHedron.GetTetrahedron()
                                             .Rotated(10, 10, 0)
                                             .Scaled(100, 100, 100)
                                             .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);
                    break;
                case (int)Figure.DrawOctahedron:
                    currentPolyhedron = PolyHedron.GetOctahedron()
                                             .Rotated(20, 20, 0)
                                             .Scaled(100, 100, 100)
                                             .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);
                    break;
                case (int)Figure.DrawIcosahedron:
                    currentPolyhedron = PolyHedron.GetIcosahedron()
                                              .Rotated(10, 10, 0)
                                              .Scaled(150, 150, 150)
                                              .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);
                    break;
                case (int)Figure.DrawDodecahedron:
                    currentPolyhedron = PolyHedron.GetDodecahedron()
                                             .Rotated(10, 10, 0)
                                             .Scaled(200, 200, 200)
                                             .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);
                    break;
            }
            RenderScene();
        }

        private void reflectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentPolyhedron == null) return;

            g.Clear(pictureBox1.BackColor);

            switch (reflectionComboBox.SelectedIndex)
            {
                case (int)Operation.Reflect_XY:
                    currPlane = "XY";
                    currentPolyhedron = currentPolyhedron.Moved(-pictureBox1.Width / 2, -pictureBox1.Height / 2, 0)
                        .Reflected("XY")
                        .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);
                    break;
                case (int)Operation.Reflect_YZ:
                    currPlane = "YZ";
                    currentPolyhedron = currentPolyhedron.Moved(-pictureBox1.Width / 2, -pictureBox1.Height / 2, 0)
                        .Reflected("YZ")
                        .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);
                    break;
                case (int)Operation.Reflect_XZ:
                    currPlane = "XZ";
                    currentPolyhedron = currentPolyhedron.Moved(-pictureBox1.Width / 2, -pictureBox1.Height / 2, 0)
                        .Reflected("XZ")
                        .Moved(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);
                    break;
            }
            RenderScene();
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
                    currentPolyhedron = currentPolyhedron
                                                       .Moved((float)numericUpDown1.Value, (float)numericUpDown2.Value, (float)numericUpDown3.Value);
                    break;
                case (int)AffineOp.Scaling:
                    anchor = new Vertex((float)numericUpDown4.Value, (float)numericUpDown5.Value, (float)numericUpDown6.Value);
                    currentPolyhedron = currentPolyhedron
                                                       .Moved(-anchor.X, -anchor.Y, -anchor.Z)
                                                       .Scaled((float)numericUpDown1.Value, (float)numericUpDown2.Value, (float)numericUpDown3.Value)
                                                       .Moved(anchor.X, anchor.Y, anchor.Z);
                    break;
                case (int)AffineOp.Rotation:
                    anchor = new Vertex((float)numericUpDown4.Value, (float)numericUpDown5.Value, (float)numericUpDown6.Value);
                    currentPolyhedron = currentPolyhedron
                                                       .Moved(-anchor.X, -anchor.Y, -anchor.Z)
                                                       .Rotated((float)numericUpDown1.Value, (float)numericUpDown2.Value, (float)numericUpDown3.Value)
                                                       .Moved(anchor.X, anchor.Y, anchor.Z);
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

                    currentPolyhedron = currentPolyhedron
                        .Moved(-anchor.X, -anchor.Y, -anchor.Z)
                        .LineRotated(l, m, n, (float)numericUpDown1.Value)
                        .Moved(anchor.X, anchor.Y, anchor.Z);
                    break;
                case (int)AffineOp.AxisXRotation:
                    centerX = 0; centerY = 0; centerZ = 0;
                    currentPolyhedron.FindCenter(currentPolyhedron.Vertices, ref centerX, ref centerY, ref centerZ);
                    currentPolyhedron = currentPolyhedron
                                                       .Moved((float)-centerX, (float)-centerY, (float)-centerZ)
                                                       .Rotated((float)numericUpDown1.Value, 0, 0)
                                                       .Moved((float)centerX, (float)centerY, (float)centerZ);
                    break;
                case (int)AffineOp.AxisYRotation:
                    centerX = 0; centerY = 0; centerZ = 0;
                    currentPolyhedron.FindCenter(currentPolyhedron.Vertices, ref centerX, ref centerY, ref centerZ);
                    currentPolyhedron = currentPolyhedron
                                                       .Moved((float)-centerX, (float)-centerY, (float)-centerZ)
                                                       .Rotated(0, (float)numericUpDown2.Value, 0)
                                                       .Moved((float)centerX, (float)centerY, (float)centerZ);
                    break;
                case (int)AffineOp.AxisZRotation:
                    centerX = 0; centerY = 0; centerZ = 0;
                    currentPolyhedron.FindCenter(currentPolyhedron.Vertices, ref centerX, ref centerY, ref centerZ);
                    currentPolyhedron = currentPolyhedron
                                                       .Moved((float)-centerX, (float)-centerY, (float)-centerZ)
                                                       .Rotated(0, 0, (float)numericUpDown2.Value)
                                                       .Moved((float)centerX, (float)centerY, (float)centerZ);
                    break;
            }
            RenderScene();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            RenderScene();
        }

        private void axisXNumeric_ValueChanged(object sender, EventArgs e)
        {
            axisZNumeric.Value = 360 - axisXNumeric.Value - axisYNumeric.Value;
            RenderScene();
        }

        private void axisYNumeric_ValueChanged(object sender, EventArgs e)
        {
            axisZNumeric.Value = 360 - axisXNumeric.Value - axisYNumeric.Value;
            RenderScene();
        }

        private void projectionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentProjection = projectionListBox.SelectedIndex == 0 ? Projection.Perspective : Projection.Orthographic;
            RenderScene();
        }

        private void shadingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentShading = (ShadingMode)shadingComboBox.SelectedIndex;
            RenderScene();
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
                    RenderScene();
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
            RenderScene();
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
            RenderScene();
        }

        private void chkZBuffer_CheckedChanged(object sender, EventArgs e)
        {
            useZBuffer = chkZBuffer.Checked;
            RenderScene();
        }

        private void btnUpdateLight_Click(object sender, EventArgs e)
        {
            lightPosition = new Vertex(
                (float)numericLightX.Value,
                (float)numericLightY.Value,
                (float)numericLightZ.Value
            );
            RenderScene();
        }

        private void btnObjectColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = objectColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                objectColor = colorDialog1.Color;
                RenderScene();
            }
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
            // Сохраняем UV-координаты из исходной вершины (если возможно)
            // Но проблема: мы не имеем доступа к исходной вершине здесь

            // Временное решение: возвращаем UV = 0, но это не решает проблему
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
            return new Matrix<T>(new T[1, 4] {
        { (T)Convert.ChangeType(vertex.X, typeof(T)),
          (T)Convert.ChangeType(vertex.Y, typeof(T)),
          (T)Convert.ChangeType(vertex.Z, typeof(T)),
          (T)Convert.ChangeType(1, typeof(T)) }
    });
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
        public float U { get; set; }
        public float V { get; set; }

        public Vertex(float x, float y, float z, float u = 0, float v = 0)
        {
            X = x;
            Y = y;
            Z = z;
            U = u;
            V = v;
        }

        public static Vertex operator *(Vertex v, float scalar)
        {
            return new Vertex(v.X * scalar, v.Y * scalar, v.Z * scalar, v.U, v.V);
        }

        public static Vertex operator -(Vertex v)
        {
            return new Vertex(-v.X, -v.Y, -v.Z, v.U, v.V);
        }

        public static Vertex operator *(float scalar, Vertex v)
        {
            return v * scalar;
        }

        public static Vertex operator /(Vertex v, float scalar)
        {
            if (scalar == 0) throw new DivideByZeroException();
            return new Vertex(v.X / scalar, v.Y / scalar, v.Z / scalar, v.U, v.V);
        }

        public static Vertex operator +(Vertex v1, Vertex v2)
        {
            return new Vertex(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.U, v1.V);
        }

        public static Vertex operator -(Vertex v1, Vertex v2)
        {
            return new Vertex(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z, v1.U, v1.V);
        }

        public static Vertex GetFaceCentroid(Face face, List<Vertex> vertices)
        {
            float x = 0, y = 0, z = 0;
            foreach (var index in face.Vertices)
            {
                x += vertices[index].X;
                y += vertices[index].Y;
                z += vertices[index].Z;
            }
            int count = face.Vertices.Count();
            return new Vertex(x / count, y / count, z / count);
        }

        public static Vertex GetPolyhedronCenter(PolyHedron polyhedron)
        {
            float x = 0, y = 0, z = 0;
            foreach (var face in polyhedron.Faces)
            {
                var centroid = GetFaceCentroid(face, polyhedron.Vertices);
                x += centroid.X;
                y += centroid.Y;
                z += centroid.Z;
            }
            int count = polyhedron.Faces.Count;
            return new Vertex(x / count, y / count, z / count);
        }

        public static Vertex GetFaceNormal(Face face, List<Vertex> vertices)
        {
            var v1 = vertices[face.Vertices[0]];
            var v2 = vertices[face.Vertices[1]];
            var v3 = vertices[face.Vertices[2]];

            var ab = new Vertex(v2.X - v1.X, v2.Y - v1.Y, v2.Z - v1.Z);
            var bc = new Vertex(v3.X - v2.X, v3.Y - v2.Y, v3.Z - v2.Z);

            var nx = ab.Y * bc.Z - ab.Z * bc.Y;
            var ny = ab.Z * bc.X - ab.X * bc.Z;
            var nz = ab.X * bc.Y - ab.Y * bc.X;

            return new Vertex(nx, ny, nz);
        }

        public static float Dot(Vertex v1, Vertex v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public static Vertex Cross(Vertex v1, Vertex v2)
        {
            return new Vertex(
                v1.Y * v2.Z - v1.Z * v2.Y,
                v1.Z * v2.X - v1.X * v2.Z,
                v1.X * v2.Y - v1.Y * v2.X
            ).Normalize();
        }

        public Vertex Normalize()
        {
            float length = (float)Math.Sqrt(X * X + Y * Y + Z * Z);
            if (length == 0) return this;
            return new Vertex(X / length, Y / length, Z / length, U, V);
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

    public class Camera
    {
        public Vertex Position { get; set; }
        public Vertex Target { get; set; }
        public Vertex Direction { get; set; }
        public Vertex Up { get; set; }
        public Vertex Right { get; set; }

        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public Matrix<float> ViewMatrix => CreateViewMatrix();
        public Matrix<float> ProjectionMatrix { get; set; }
        public Matrix<float> OrthographicMatrix { get; set; }

        public Camera(Vertex position, float yaw = 0, float pitch = 0)
        {
            Position = position;
            Yaw = yaw;
            Pitch = pitch;

            Direction = new Vertex(0, 0, -1).Normalize();

            float fov = (float)Math.PI / 3;
            float aspectRatio = 1.0f;
            float near = 0.1f;
            float far = 1000f;

            OrthographicMatrix = Utilities.CreateOrthographicFov(fov, aspectRatio, near, far);
            ProjectionMatrix = Utilities.CreatePerspectiveFieldOfView(fov, aspectRatio, near, far);
        }

        public void UpdateVectors()
        {
            Pitch = Math.Max(-89f, Math.Min(89f, Pitch));

            Direction = new Vertex(
                (float)(Math.Cos(Yaw * Math.PI / 180) * Math.Cos(Pitch * Math.PI / 180)),
                (float)Math.Sin(Pitch * Math.PI / 180),
                (float)(Math.Sin(Yaw * Math.PI / 180) * Math.Cos(Pitch * Math.PI / 180))
            ).Normalize();

            Right = Vertex.Cross(Direction, new Vertex(0, 1, 0)).Normalize();
            Up = Vertex.Cross(Right, Direction).Normalize();

            Target = Position + Direction;
        }

        private Matrix<float> CreateViewMatrix()
        {
            return new float[4, 4] {
            { Right.X, Right.Y, Right.Z, -Vertex.Dot(Right, Position) },
            { Up.X, Up.Y, Up.Z, -Vertex.Dot(Up, Position) },
            { -Direction.X, -Direction.Y, -Direction.Z, Vertex.Dot(Direction, Position) },
            { 0, 0, 0, 1 }
        };
        }

        public void Move(Vertex movement)
        {
            Position = Position + movement;
            Target = Position + Direction;
        }

        public void Rotate(float yawChange, float pitchChange)
        {
            Yaw += yawChange;
            Pitch += pitchChange;
            UpdateVectors();
        }
    }

    public static class Utilities
    {
        public static Matrix<float> CreateViewMatrix(Vertex position, Vertex target, Vertex up)
        {
            Vertex forward = (target - position).Normalize();
            Vertex right = Vertex.Cross(up, forward).Normalize();
            Vertex adjustedUp = Vertex.Cross(forward, right);

            return new float[4, 4] {
            { right.X, adjustedUp.X, forward.X, 0 },
            { right.Y, adjustedUp.Y, forward.Y, 0 },
            { right.Z, adjustedUp.Z, forward.Z, 0 },
            { -Vertex.Dot(right, position), -Vertex.Dot(adjustedUp, position), -Vertex.Dot(forward, position), 1 }
        };
        }

        public static Matrix<float> CreatePerspectiveFieldOfView(float fov, float aspectRatio, float near, float far)
        {
            float f = 1.0f / (float)Math.Tan(fov / 2.0f);

            return new float[4, 4] {
            { f / aspectRatio, 0, 0, 0 },
            { 0, f, 0, 0 },
            { 0, 0, (far + near) / (near - far), -1 },
            { 0, 0, (2 * far * near) / (near - far), 0 }
        };
        }

        public static Matrix<float> CreateOrthographicFov(float fov, float aspect, float near, float far)
        {
            float top = (float)Math.Tan(fov / 2) * near;
            float bottom = -top;
            float right = top * aspect;
            float left = -right;

            return new float[4, 4] {
            { 2.0f / (right - left), 0, 0, -(right + left) / (right - left) },
            { 0, 2.0f / (top - bottom), 0, -(top + bottom) / (top - bottom) },
            { 0, 0, -2.0f / (far - near), -(far + near) / (far - near) },
            { 0, 0, 0, 1 }
        };
        }
    }

    public struct Normal
    {
        public float NX { get; private set; }
        public float NY { get; private set; }
        public float NZ { get; private set; }

        public Normal(float nx, float ny, float nz)
        {
            float length = (float)Math.Sqrt(nx * nx + ny * ny + nz * nz);
            if (length > 0)
            {
                NX = nx / length;
                NY = ny / length;
                NZ = nz / length;
            }
            else
            {
                NX = 0;
                NY = 0;
                NZ = 0;
            }
        }
    }

    public class Face
    {
        public int[] Vertices { get; private set; }
        public Vertex Normal { get; set; }

        public Face(params int[] vertices)
        {
            Vertices = vertices;
        }

        public void SetNormal(Vertex normal)
        {
            Normal = normal;
        }
    }

    public class PolyHedron
    {
        public List<Face> Faces { get; set; }
        public List<Vertex> Vertices { get; set; }
        public List<Normal> Normals { get; set; }

        public PolyHedron()
        {
            Faces = new List<Face>();
            Vertices = new List<Vertex>();
            Normals = new List<Normal>();
        }

        public PolyHedron(List<Face> faces, List<Vertex> vertices, List<Normal> normals = null)
        {
            Faces = faces;
            Vertices = vertices;
            Normals = normals ?? new List<Normal>();
        }

        public PolyHedron(List<Face> faces, List<Vertex> vertices)
        {
            Faces = faces;
            Vertices = vertices;
            Normals = new List<Normal>();
        }

        public void ComputeVertexNormals()
        {
            if (Vertices.Count == 0) return;

            Normals = new List<Normal>(new Normal[Vertices.Count]);

            // Используем временные структуры для хранения суммы нормалей
            var sumX = new float[Vertices.Count];
            var sumY = new float[Vertices.Count];
            var sumZ = new float[Vertices.Count];

            for (int i = 0; i < Vertices.Count; i++)
            {
                sumX[i] = 0;
                sumY[i] = 0;
                sumZ[i] = 0;
            }

            foreach (var face in Faces)
            {
                if (face.Vertices.Length < 3) continue;

                Vertex v1 = Vertices[face.Vertices[0]];
                Vertex v2 = Vertices[face.Vertices[1]];
                Vertex v3 = Vertices[face.Vertices[2]];

                Vertex edge1 = v2 - v1;
                Vertex edge2 = v3 - v1;

                Vertex faceNormal = new Vertex(
                    edge1.Y * edge2.Z - edge1.Z * edge2.Y,
                    edge1.Z * edge2.X - edge1.X * edge2.Z,
                    edge1.X * edge2.Y - edge1.Y * edge2.X
                ).Normalize();

                // Суммируем компоненты нормалей отдельно
                foreach (int idx in face.Vertices)
                {
                    sumX[idx] += faceNormal.X;
                    sumY[idx] += faceNormal.Y;
                    sumZ[idx] += faceNormal.Z;
                }
            }

            // Нормализуем и создаем нормали, не затрагивая UV-координаты вершин
            for (int i = 0; i < Vertices.Count; i++)
            {
                float length = (float)Math.Sqrt(sumX[i] * sumX[i] + sumY[i] * sumY[i] + sumZ[i] * sumZ[i]);
                if (length > 0)
                {
                    Normals[i] = new Normal(sumX[i] / length, sumY[i] / length, sumZ[i] / length);
                }
                else
                {
                    Normals[i] = new Normal(0, 0, 0);
                }

                // UV-координаты остаются нетронутыми в Vertices[i]
            }
        }

        public static void AdjustNormals(PolyHedron polyhedron)
        {
            double centerX = 0, centerY = 0, centerZ = 0;
            polyhedron.FindCenter(polyhedron.Vertices, ref centerX, ref centerY, ref centerZ);
            Vertex center = new Vertex((float)centerX, (float)centerY, (float)centerZ);

            foreach (var face in polyhedron.Faces)
            {
                if (face.Vertices.Length >= 3)
                {
                    Vertex v1 = polyhedron.Vertices[face.Vertices[0]];
                    Vertex v2 = polyhedron.Vertices[face.Vertices[1]];
                    Vertex v3 = polyhedron.Vertices[face.Vertices[2]];

                    Vertex edge1 = v2 - v1;
                    Vertex edge2 = v3 - v1;

                    Vertex normal = new Vertex(
                        edge1.Y * edge2.Z - edge1.Z * edge2.Y,
                        edge1.Z * edge2.X - edge1.X * edge2.Z,
                        edge1.X * edge2.Y - edge1.Y * edge2.X
                    );

                    normal = normal.Normalize();

                    Vertex faceCenter = polyhedron.GetFaceCenter(face);
                    Vertex toCenter = (center - faceCenter).Normalize();

                    float dot = Vertex.Dot(normal, toCenter);

                    if (dot > 0)
                    {
                        normal = new Vertex(-normal.X, -normal.Y, -normal.Z);
                    }

                    face.Normal = normal;
                }
            }
        }

        public PolyHedron FilterVisibleFaces(Camera camera, Form1.Projection proj)
        {
            var visiblePolyhedron = this.Clone();
            visiblePolyhedron.Faces = new List<Face>();

            AdjustNormals(this);

            foreach (var face in this.Faces)
            {
                Vertex faceCenter = GetFaceCenter(face);
                Vertex normal = face.Normal.Normalize();

                Vertex toCamera = (camera.Position - faceCenter).Normalize();

                float dotProduct = Vertex.Dot(normal, toCamera);

                bool isVisible = dotProduct > 0;

                if (isVisible)
                {
                    visiblePolyhedron.Faces.Add(face);
                }
            }

            return visiblePolyhedron;
        }

        private Color LambertColor(Vertex normal, Vertex lightDir, Color baseColor)
        {
            var n = normal.Normalize();
            var l = lightDir.Normalize();

            float ndotl = Math.Max(0f, Vertex.Dot(n, l));
            float Ia = 0.1f;
            float Id = 0.9f;

            float k = Ia + Id * ndotl;
            k = Math.Min(1f, Math.Max(0f, k));

            int r = (int)(baseColor.R * k);
            int g = (int)(baseColor.G * k);
            int b = (int)(baseColor.B * k);

            return Color.FromArgb(r, g, b);
        }

        private float ToonIntensity(float lambert)
        {
            if (lambert <= 0.1f) return 0.0f;
            if (lambert <= 0.4f) return 0.3f;
            if (lambert <= 0.7f) return 0.6f;
            return 1.0f;
        }

        private Color PhongToonColor(Vertex normal, Vertex lightDir, Color baseColor)
        {
            var n = normal.Normalize();
            var l = lightDir.Normalize();

            float ndotl = Math.Max(0f, Vertex.Dot(n, l));
            float toon = ToonIntensity(ndotl);

            float Ia = 0.1f;
            float Id = 0.9f;

            float k = Ia + Id * toon;
            k = Math.Min(1f, Math.Max(0f, k));

            int r = (int)(baseColor.R * k);
            int g = (int)(baseColor.G * k);
            int b = (int)(baseColor.B * k);

            return Color.FromArgb(r, g, b);
        }

        private void DrawTriangleGouraud(
            Bitmap bmp,
            float[,] zBuffer,
            Vertex v0, Vertex v1, Vertex v2,
            PointF p0, PointF p1, PointF p2,
            Color c0, Color c1, Color c2)
        {
            int width = bmp.Width;
            int height = bmp.Height;

            int minX = (int)Math.Floor(Math.Min(p0.X, Math.Min(p1.X, p2.X)));
            int maxX = (int)Math.Ceiling(Math.Max(p0.X, Math.Max(p1.X, p2.X)));
            int minY = (int)Math.Floor(Math.Min(p0.Y, Math.Min(p1.Y, p2.Y)));
            int maxY = (int)Math.Ceiling(Math.Max(p0.Y, Math.Max(p1.Y, p2.Y)));

            minX = Math.Max(minX, 0);
            minY = Math.Max(minY, 0);
            maxX = Math.Min(maxX, width - 1);
            maxY = Math.Min(maxY, height - 1);

            float denom = (p1.Y - p2.Y) * (p0.X - p2.X) + (p2.X - p1.X) * (p0.Y - p2.Y);
            if (Math.Abs(denom) < 1e-6f) return;

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    float lambda0 = ((p1.Y - p2.Y) * (x - p2.X) + (p2.X - p1.X) * (y - p2.Y)) / denom;
                    float lambda1 = ((p2.Y - p0.Y) * (x - p2.X) + (p0.X - p2.X) * (y - p2.Y)) / denom;
                    float lambda2 = 1f - lambda0 - lambda1;

                    if (lambda0 < 0 || lambda1 < 0 || lambda2 < 0) continue;

                    float z = lambda0 * v0.Z + lambda1 * v1.Z + lambda2 * v2.Z;

                    if (z < zBuffer[x, y])
                    {
                        zBuffer[x, y] = z;

                        int r = (int)(lambda0 * c0.R + lambda1 * c1.R + lambda2 * c2.R);
                        int g = (int)(lambda0 * c0.G + lambda1 * c1.G + lambda2 * c2.G);
                        int b = (int)(lambda0 * c0.B + lambda1 * c1.B + lambda2 * c2.B);

                        bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }
            }
        }

        private void DrawTrianglePhongToon(
            Bitmap bmp,
            float[,] zBuffer,
            Vertex v0, Vertex v1, Vertex v2,
            PointF p0, PointF p1, PointF p2,
            Vertex n0, Vertex n1, Vertex n2,
            Vertex lightDir,
            Color baseColor)
        {
            int width = bmp.Width;
            int height = bmp.Height;

            int minX = (int)Math.Floor(Math.Min(p0.X, Math.Min(p1.X, p2.X)));
            int maxX = (int)Math.Ceiling(Math.Max(p0.X, Math.Max(p1.X, p2.X)));
            int minY = (int)Math.Floor(Math.Min(p0.Y, Math.Min(p1.Y, p2.Y)));
            int maxY = (int)Math.Ceiling(Math.Max(p0.Y, Math.Max(p1.Y, p2.Y)));

            minX = Math.Max(minX, 0);
            minY = Math.Max(minY, 0);
            maxX = Math.Min(maxX, width - 1);
            maxY = Math.Min(maxY, height - 1);

            float denom = (p1.Y - p2.Y) * (p0.X - p2.X) + (p2.X - p1.X) * (p0.Y - p2.Y);
            if (Math.Abs(denom) < 1e-6f) return;

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    float lambda0 = ((p1.Y - p2.Y) * (x - p2.X) + (p2.X - p1.X) * (y - p2.Y)) / denom;
                    float lambda1 = ((p2.Y - p0.Y) * (x - p2.X) + (p0.X - p2.X) * (y - p2.Y)) / denom;
                    float lambda2 = 1f - lambda0 - lambda1;

                    if (lambda0 < 0 || lambda1 < 0 || lambda2 < 0) continue;

                    float z = lambda0 * v0.Z + lambda1 * v1.Z + lambda2 * v2.Z;

                    if (z < zBuffer[x, y])
                    {
                        zBuffer[x, y] = z;

                        Vertex n = (n0 * lambda0 + n1 * lambda1 + n2 * lambda2).Normalize();
                        Color c = PhongToonColor(n, lightDir, baseColor);

                        bmp.SetPixel(x, y, c);
                    }
                }
            }
        }

        private void DrawTriangleTextured(
    Bitmap bmp,
    float[,] zBuffer,
    Vertex v0, Vertex v1, Vertex v2,
    PointF p0, PointF p1, PointF p2,
    Bitmap texture)
        {
            // В начале DrawTriangleTextured
            Console.WriteLine($"UV coords: v0=({v0.U}, {v0.V}), v1=({v1.U}, {v1.V}), v2=({v2.U}, {v2.V})");
            //v0 = new Vertex(v0.X, v0.Y, v0.Z, 0.0f, 0.0f);
            //v1 = new Vertex(v1.X, v1.Y, v1.Z, 1.0f, 0.0f);
            //v2 = new Vertex(v2.X, v2.Y, v2.Z, 0.5f, 1.0f);
            int width = bmp.Width;
            int height = bmp.Height;

            int minX = (int)Math.Floor(Math.Min(p0.X, Math.Min(p1.X, p2.X)));
            int maxX = (int)Math.Ceiling(Math.Max(p0.X, Math.Max(p1.X, p2.X)));
            int minY = (int)Math.Floor(Math.Min(p0.Y, Math.Min(p1.Y, p2.Y)));
            int maxY = (int)Math.Ceiling(Math.Max(p0.Y, Math.Max(p1.Y, p2.Y)));

            minX = Math.Max(minX, 0);
            minY = Math.Max(minY, 0);
            maxX = Math.Min(maxX, width - 1);
            maxY = Math.Min(maxY, height - 1);

            float denom = (p1.Y - p2.Y) * (p0.X - p2.X) + (p2.X - p1.X) * (p0.Y - p2.Y);
            if (Math.Abs(denom) < 1e-6f) return;

            int texW = texture.Width;
            int texH = texture.Height;

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    // Вычисляем барицентрические координаты
                    float lambda0 = ((p1.Y - p2.Y) * (x - p2.X) + (p2.X - p1.X) * (y - p2.Y)) / denom;
                    float lambda1 = ((p2.Y - p0.Y) * (x - p2.X) + (p0.X - p2.X) * (y - p2.Y)) / denom;
                    float lambda2 = 1f - lambda0 - lambda1;

                    if (lambda0 < 0 || lambda1 < 0 || lambda2 < 0) continue;

                    float z = lambda0 * v0.Z + lambda1 * v1.Z + lambda2 * v2.Z;

                    if (z < zBuffer[x, y])
                    {
                        zBuffer[x, y] = z;

                        // Линейная интерполяция UV
                        float u = lambda0 * v0.U + lambda1 * v1.U + lambda2 * v2.U;
                        float vCoord = lambda0 * v0.V + lambda1 * v1.V + lambda2 * v2.V;

                        // Зацикливание текстурных координат
                        u = u - (float)Math.Floor(u);
                        vCoord = vCoord - (float)Math.Floor(vCoord);

                        int texX = (int)(u * (texW - 1));
                        int texY = (int)(vCoord * (texH - 1));

                        texX = Math.Max(0, Math.Min(texX, texW - 1));
                        texY = Math.Max(0, Math.Min(texY, texH - 1));

                        Color texColor = texture.GetPixel(texX, texY);
                        bmp.SetPixel(x, y, texColor);
                    }
                }
            }
        }

        public void RenderWithZBuffer(
            Graphics g,
            PictureBox pictureBox,
            Camera camera,
            Form1.Projection proj,
            Form1.ShadingMode shadingMode,
            Vertex lightPos,
            Color objectColor,
            Bitmap texture = null)
        {
            float[,] zBuffer = new float[pictureBox.Width, pictureBox.Height];
            for (int x = 0; x < pictureBox.Width; x++)
                for (int y = 0; y < pictureBox.Height; y++)
                    zBuffer[x, y] = float.MaxValue;

            var visiblePolyhedron = FilterVisibleFaces(camera, proj);
            visiblePolyhedron.ComputeVertexNormals();

            var sortedFaces = visiblePolyhedron.Faces
                .OrderBy(face =>
                {
                    var center = GetFaceCenter(face);
                    return center.DistanceTo(camera.Position);
                })
                .ToList();

            Bitmap bmp = (Bitmap)pictureBox.Image;

            foreach (var face in sortedFaces)
            {
                var v3d = new List<Vertex>();
                var v2d = new List<PointF>();
                var vColors = new List<Color>();
                var vNormals = new List<Vertex>();

                foreach (var idx in face.Vertices)
                {
                    Vertex v = visiblePolyhedron.Vertices[idx];
                    v3d.Add(v);
                    PointF projectedPoint = v.GetProjection(
                        proj == Form1.Projection.Perspective ? 0 : 1,
                        pictureBox.Width / 2,
                        pictureBox.Height / 2,
                        120, 120);
                    v2d.Add(projectedPoint);

                    Normal n = visiblePolyhedron.Normals[idx];
                    vNormals.Add(new Vertex(n.NX, n.NY, n.NZ));
                }

                if (v3d.Count < 3) continue;

                Vertex lightDir = (lightPos - Vertex.GetPolyhedronCenter(visiblePolyhedron)).Normalize();

                if (shadingMode == Form1.ShadingMode.GouraudLambert)
                {
                    vColors.Clear();
                    for (int i = 0; i < v3d.Count; i++)
                    {
                        Color c = LambertColor(vNormals[i], lightDir, objectColor);
                        vColors.Add(c);
                    }
                }

                for (int i = 1; i < v3d.Count - 1; i++)
                {
                    Vertex vv0 = v3d[0];
                    Vertex vv1 = v3d[i];
                    Vertex vv2 = v3d[i + 1];

                    PointF pp0 = v2d[0];
                    PointF pp1 = v2d[i];
                    PointF pp2 = v2d[i + 1];

                    if (shadingMode == Form1.ShadingMode.GouraudLambert)
                    {
                        Color cc0 = vColors[0];
                        Color cc1 = vColors[i];
                        Color cc2 = vColors[i + 1];

                        DrawTriangleGouraud(bmp, zBuffer, vv0, vv1, vv2, pp0, pp1, pp2, cc0, cc1, cc2);
                    }
                    else if (shadingMode == Form1.ShadingMode.PhongToon)
                    {
                        Vertex nn0 = vNormals[0];
                        Vertex nn1 = vNormals[i];
                        Vertex nn2 = vNormals[i + 1];

                        DrawTrianglePhongToon(bmp, zBuffer, vv0, vv1, vv2, pp0, pp1, pp2,
                            nn0, nn1, nn2, lightDir, objectColor);
                    }
                    else if (shadingMode == Form1.ShadingMode.Texture && texture != null)
                    {
                        DrawTriangleTextured(bmp, zBuffer, vv0, vv1, vv2, pp0, pp1, pp2, texture);
                    }
                }
            }
        }

        private Color GetFaceColor(Face face)
        {
            if (face.Normal.X == 0 && face.Normal.Y == 0 && face.Normal.Z == 0)
                return Color.LightBlue;

            int r = Math.Max(0, Math.Min(255, (int)((face.Normal.X + 1) * 127.5)));
            int g = Math.Max(0, Math.Min(255, (int)((face.Normal.Y + 1) * 127.5)));
            int b = Math.Max(0, Math.Min(255, (int)((face.Normal.Z + 1) * 127.5)));

            return Color.FromArgb(r, g, b);
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
            float oneMinusCos = 1 - cos;

            // Матрица поворота вокруг произвольной оси
            float[,] rotationMatrix = new float[3, 3] {
        { l*l*oneMinusCos + cos,      l*m*oneMinusCos - n*sin, l*n*oneMinusCos + m*sin },
        { m*l*oneMinusCos + n*sin,    m*m*oneMinusCos + cos,   m*n*oneMinusCos - l*sin },
        { n*l*oneMinusCos - m*sin,    n*m*oneMinusCos + l*sin, n*n*oneMinusCos + cos   }
    };

            for (int i = 0; i < newPoly.Vertices.Count; i++)
            {
                var vertex = newPoly.Vertices[i];
                float x = vertex.X;
                float y = vertex.Y;
                float z = vertex.Z;

                float newX = x * rotationMatrix[0, 0] + y * rotationMatrix[0, 1] + z * rotationMatrix[0, 2];
                float newY = x * rotationMatrix[1, 0] + y * rotationMatrix[1, 1] + z * rotationMatrix[1, 2];
                float newZ = x * rotationMatrix[2, 0] + y * rotationMatrix[2, 1] + z * rotationMatrix[2, 2];

                // Явно сохраняем UV-координаты
                newPoly.Vertices[i] = new Vertex(
                    newX,
                    newY,
                    newZ,
                    vertex.U,
                    vertex.V
                );
            }

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

        public PolyHedron Moved(float a, float b, float c)
        {
            var newPoly = this.Clone();

            for (int i = 0; i < newPoly.Vertices.Count; i++)
            {
                var vertex = newPoly.Vertices[i];
                // Явно сохраняем UV-координаты
                newPoly.Vertices[i] = new Vertex(
                    vertex.X + a,
                    vertex.Y + b,
                    vertex.Z + c,
                    vertex.U,
                    vertex.V
                );
            }

            return newPoly;
        }

        public PolyHedron Scaled(float c1, float c2, float c3)
        {
            var newPoly = this.Clone();

            for (int i = 0; i < newPoly.Vertices.Count; i++)
            {
                var vertex = newPoly.Vertices[i];
                // Явно сохраняем UV-координаты
                newPoly.Vertices[i] = new Vertex(
                    vertex.X * c1,
                    vertex.Y * c2,
                    vertex.Z * c3,
                    vertex.U,
                    vertex.V
                );
            }

            return newPoly;
        }

        public PolyHedron RotatedXAxis(float alpha)
        {
            var newPoly = this.Clone();

            double angleRadians = (double)alpha * (Math.PI / 180);
            float cos = (float)Math.Cos(angleRadians);
            float sin = (float)Math.Sin(angleRadians);

            for (int i = 0; i < newPoly.Vertices.Count; i++)
            {
                var vertex = newPoly.Vertices[i];
                float newY = vertex.Y * cos - vertex.Z * sin;
                float newZ = vertex.Y * sin + vertex.Z * cos;

                // Явно сохраняем UV-координаты
                newPoly.Vertices[i] = new Vertex(
                    vertex.X,
                    newY,
                    newZ,
                    vertex.U,
                    vertex.V
                );
            }

            return newPoly;
        }

        public PolyHedron RotatedYAxis(float alpha)
        {
            var newPoly = this.Clone();

            double angleRadians = (double)alpha * (Math.PI / 180);
            float cos = (float)Math.Cos(angleRadians);
            float sin = (float)Math.Sin(angleRadians);

            for (int i = 0; i < newPoly.Vertices.Count; i++)
            {
                var vertex = newPoly.Vertices[i];
                float newX = vertex.X * cos + vertex.Z * sin;
                float newZ = -vertex.X * sin + vertex.Z * cos;

                // Явно сохраняем UV-координаты
                newPoly.Vertices[i] = new Vertex(
                    newX,
                    vertex.Y,
                    newZ,
                    vertex.U,
                    vertex.V
                );
            }

            return newPoly;
        }

        public PolyHedron RotatedZAxis(float alpha)
        {
            var newPoly = this.Clone();

            double angleRadians = (double)alpha * (Math.PI / 180);
            float cos = (float)Math.Cos(angleRadians);
            float sin = (float)Math.Sin(angleRadians);

            for (int i = 0; i < newPoly.Vertices.Count; i++)
            {
                var vertex = newPoly.Vertices[i];
                float newX = vertex.X * cos - vertex.Y * sin;
                float newY = vertex.X * sin + vertex.Y * cos;

                // Явно сохраняем UV-координаты
                newPoly.Vertices[i] = new Vertex(
                    newX,
                    newY,
                    vertex.Z,
                    vertex.U,
                    vertex.V
                );
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

        public static PolyHedron GetCube()
        {
            var cube = new PolyHedron();

            // Вершины куба
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

            cube.ApplyPlanarUV();
            cube.ComputeVertexNormals();
            return cube;
        }

        public static PolyHedron GetTetrahedron()
        {
            var tetra = new PolyHedron();

            // Вершины тетраэдра
            tetra.Vertices.Add(new Vertex(0, 1, 0));       // 0: верх
            tetra.Vertices.Add(new Vertex(0.94f, -0.33f, 0));     // 1: право-низ
            tetra.Vertices.Add(new Vertex(-0.47f, -0.33f, 0.82f)); // 2: лево-низ-близко
            tetra.Vertices.Add(new Vertex(-0.47f, -0.33f, -0.82f));// 3: лево-низ-далеко

            tetra.Faces.Add(new Face(0, 1, 2));
            tetra.Faces.Add(new Face(0, 2, 3));
            tetra.Faces.Add(new Face(0, 3, 1));
            tetra.Faces.Add(new Face(1, 3, 2));

            tetra.ApplyPlanarUV();
            tetra.ComputeVertexNormals();
            return tetra;
        }

        public static PolyHedron GetOctahedron()
        {
            var octa = new PolyHedron();

            // Вершины октаэдра
            octa.Vertices.Add(new Vertex(0, 1, 0));    // 0: верх
            octa.Vertices.Add(new Vertex(1, 0, 0));    // 1: право
            octa.Vertices.Add(new Vertex(0, 0, 1));    // 2: зад
            octa.Vertices.Add(new Vertex(-1, 0, 0));   // 3: лево
            octa.Vertices.Add(new Vertex(0, 0, -1));   // 4: перед
            octa.Vertices.Add(new Vertex(0, -1, 0));   // 5: низ

            // Грани октаэдра
            octa.Faces.Add(new Face(0, 1, 2));
            octa.Faces.Add(new Face(0, 2, 3));
            octa.Faces.Add(new Face(0, 3, 4));
            octa.Faces.Add(new Face(0, 4, 1));
            octa.Faces.Add(new Face(5, 2, 1));
            octa.Faces.Add(new Face(5, 3, 2));
            octa.Faces.Add(new Face(5, 4, 3));
            octa.Faces.Add(new Face(5, 1, 4));

            octa.ApplyPlanarUV();
            octa.ComputeVertexNormals();
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
                verticesBottom.Add((new Vertex((float)Math.Cos(angleRadians), -0.5f, (float)Math.Sin(angleRadians),
                    (float)(0.5 + Math.Cos(angleRadians) * 0.5), (float)(0.5 + Math.Sin(angleRadians) * 0.5)), number));
                angle += 72;
                number += 2;
            }

            angle = -54;
            number = 2;

            for (int i = 0; i < 5; i++)
            {
                var angleRadians = angle * (Math.PI / 180);
                verticesTop.Add((new Vertex((float)Math.Cos(angleRadians), 0.5f, (float)Math.Sin(angleRadians),
                    (float)(0.5 + Math.Cos(angleRadians) * 0.5), (float)(0.5 + Math.Sin(angleRadians) * 0.5)), number));
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

            // Добавляем вершины с UV-координатами
            icosa.Vertices.Add(new Vertex(0, -(float)Math.Sqrt(5) / 2, 0, 0.5f, 0.0f));
            icosa.Vertices.Add(new Vertex(0, (float)Math.Sqrt(5) / 2, 0, 0.5f, 1.0f));

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

            icosa.ComputeVertexNormals();
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

            dodeca.ApplyPlanarUV();
            dodeca.ComputeVertexNormals();
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
            var newPoly = new PolyHedron(new List<Face>(this.Faces), new List<Vertex>(this.Vertices), new List<Normal>(this.Normals));
            return newPoly;
        }

        public PolyHedron Reflected(string plane)
        {
            var newPoly = this.Clone();

            for (int i = 0; i < newPoly.Vertices.Count; i++)
            {
                var vertex = newPoly.Vertices[i];
                float newX = vertex.X;
                float newY = vertex.Y;
                float newZ = vertex.Z;

                switch (plane.ToUpper())
                {
                    case "XY":
                        newZ = -vertex.Z;
                        break;
                    case "YZ":
                        newX = -vertex.X;
                        break;
                    case "XZ":
                        newY = -vertex.Y;
                        break;
                    default:
                        throw new ArgumentException("Invalid plane");
                }

                // Явно сохраняем UV-координаты
                newPoly.Vertices[i] = new Vertex(
                    newX,
                    newY,
                    newZ,
                    vertex.U,
                    vertex.V
                );
            }

            return newPoly;
        }

        public PolyHedron ScaledAroundCenter(float scaleX, float scaleY, float scaleZ)
        {
            var newPoly = this.Clone();

            // Находим центр
            double centerX = 0, centerY = 0, centerZ = 0;
            FindCenter(newPoly.Vertices, ref centerX, ref centerY, ref centerZ);

            for (int i = 0; i < newPoly.Vertices.Count; i++)
            {
                var vertex = newPoly.Vertices[i];

                // Перемещаем к центру, масштабируем, возвращаем обратно
                float x = (vertex.X - (float)centerX) * scaleX + (float)centerX;
                float y = (vertex.Y - (float)centerY) * scaleY + (float)centerY;
                float z = (vertex.Z - (float)centerZ) * scaleZ + (float)centerZ;

                // Явно сохраняем UV-координаты
                newPoly.Vertices[i] = new Vertex(
                    x,
                    y,
                    z,
                    vertex.U,
                    vertex.V
                );
            }

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

                    // Добавляем UV-координаты на основе угла и позиции в профиле
                    float u = (float)k / segments;
                    float v = (float)Array.IndexOf(profile.ToArray(), p) / (profile.Count - 1);
                    vertices.Add(new Vertex(xr, yr, zr, u, v));
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

            var ph = new PolyHedron(faces, vertices);
            ph.ComputeVertexNormals();
            return ph;
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

            var ph = new PolyHedron(faces, vertices);
            ph.ApplyPlanarUV();
            ph.ComputeVertexNormals();
            return ph;
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

        public static PolyHedron BuildFunctionSurface(int funcIndex, double xMin, double xMax, double yMin, double yMax, int nx, int ny)
        {
            var vertices = new List<Vertex>();
            var faces = new List<Face>();

            double dx = (xMax - xMin) / nx;
            double dy = (yMax - yMin) / ny;

            for (int i = 0; i <= nx; i++)
            {
                double x = xMin + i * dx;
                for (int j = 0; j <= ny; j++)
                {
                    double y = yMin + j * dy;
                    double z = EvaluateFunction(funcIndex, x, y);
                    // Добавляем UV-координаты на основе позиции в сетке
                    float u = (float)i / nx;
                    float v = (float)j / ny;
                    vertices.Add(new Vertex((float)x, (float)z, (float)y, u, v));
                }
            }

            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < ny; j++)
                {
                    int v00 = i * (ny + 1) + j;
                    int v10 = (i + 1) * (ny + 1) + j;
                    int v11 = (i + 1) * (ny + 1) + j + 1;
                    int v01 = i * (ny + 1) + j + 1;
                    faces.Add(new Face(v00, v10, v11, v01));
                }
            }

            var ph = new PolyHedron(faces, vertices);
            ph.ComputeVertexNormals();
            return ph;
        }

        private static double EvaluateFunction(int funcIndex, double x, double y)
        {
            switch (funcIndex)
            {
                case 0:
                    return Math.Sin(Math.Sqrt(x * x + y * y));
                case 1:
                    return Math.Sin(x + y) + 1;
                case 2:
                    return -Math.Sin(x) * Math.Cos(y);
                default:
                    return 0;
            }
        }

        public void ApplyPlanarUV()
        {
            if (Vertices == null || Vertices.Count == 0 || Faces == null)
                return;

            var newVertices = new List<Vertex>();
            var newFaces = new List<Face>();

            // Определяем размер сетки для равномерного распределения граней
            int gridSize = (int)Math.Ceiling(Math.Sqrt(Faces.Count));
            float cellSize = 1.0f / gridSize;

            for (int faceIndex = 0; faceIndex < Faces.Count; faceIndex++)
            {
                var face = Faces[faceIndex];
                if (face.Vertices.Length < 3) continue;

                // Позиция этой грани в сетке текстуры
                int gridX = faceIndex % gridSize;
                int gridY = faceIndex / gridSize;

                float uStart = gridX * cellSize;
                float vStart = gridY * cellSize;
                float uEnd = uStart + cellSize;
                float vEnd = vStart + cellSize;

                // Получаем вершины грани и находим bounding box
                var faceVertices = face.Vertices.Select(idx => Vertices[idx]).ToList();

                float minX = faceVertices.Min(v => v.X);
                float maxX = faceVertices.Max(v => v.X);
                float minY = faceVertices.Min(v => v.Y);
                float maxY = faceVertices.Max(v => v.Y);
                float minZ = faceVertices.Min(v => v.Z);
                float maxZ = faceVertices.Max(v => v.Z);

                // Выбираем плоскость проекции на основе нормали грани
                var normal = GetFaceNormal(face);
                string projectionPlane = GetBestProjectionPlane(normal);

                // Создаем новые вершины для этой грани
                var newFaceIndices = new List<int>();

                foreach (int oldIndex in face.Vertices)
                {
                    var vertex = Vertices[oldIndex];
                    float u, v;

                    // Проецируем вершину на выбранную плоскость
                    switch (projectionPlane)
                    {
                        case "XY":
                            u = (vertex.X - minX) / (maxX - minX + 0.0001f);
                            v = (vertex.Y - minY) / (maxY - minY + 0.0001f);
                            break;
                        case "XZ":
                            u = (vertex.X - minX) / (maxX - minX + 0.0001f);
                            v = (vertex.Z - minZ) / (maxZ - minZ + 0.0001f);
                            break;
                        case "YZ":
                        default:
                            u = (vertex.Y - minY) / (maxY - minY + 0.0001f);
                            v = (vertex.Z - minZ) / (maxZ - minZ + 0.0001f);
                            break;
                    }

                    // Масштабируем до размера ячейки и инвертируем V-координату (если нужно)
                    u = uStart + u * cellSize;
                    v = vStart + (1 - v) * cellSize; // Инвертируем V для правильной ориентации

                    newVertices.Add(new Vertex(vertex.X, vertex.Y, vertex.Z, u, v));
                    newFaceIndices.Add(newVertices.Count - 1);
                }

                newFaces.Add(new Face(newFaceIndices.ToArray()));
            }

            Vertices = newVertices;
            Faces = newFaces;
        }

        public Vertex GetFaceNormal(Face face)
        {
            if (face.Vertices.Length < 3)
                return new Vertex(0, 0, 0);

            var v1 = Vertices[face.Vertices[0]];
            var v2 = Vertices[face.Vertices[1]];
            var v3 = Vertices[face.Vertices[2]];

            var edge1 = v2 - v1;
            var edge2 = v3 - v1;

            var normal = new Vertex(
                edge1.Y * edge2.Z - edge1.Z * edge2.Y,
                edge1.Z * edge2.X - edge1.X * edge2.Z,
                edge1.X * edge2.Y - edge1.Y * edge2.X
            );

            return normal.Normalize();
        }

        private string GetBestProjectionPlane(Vertex normal)
        {
            float absX = Math.Abs(normal.X);
            float absY = Math.Abs(normal.Y);
            float absZ = Math.Abs(normal.Z);

            if (absX >= absY && absX >= absZ)
                return "YZ";
            else if (absY >= absX && absY >= absZ)
                return "XZ";
            else
                return "XY";
        }
    }
}
