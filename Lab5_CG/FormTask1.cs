using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace CG_Lab
{
    public partial class FormTask1 : Form
    {
        private LSystem lSystem;
        private LSystemGenerator generator;
        private FractalDrawer drawer;

        private int currentIteration = 0;
        private int maxIterations = 5;
        private int[] maxIterationsArray;

        private float stepDecreasePercent = 0f;
        private int colorChangeValue = 0;
        private Pen pen = Pens.Black;
        private float penThicknessDecreasePercent = 0f;

        public FormTask1()
        {
            InitializeComponent();
            maxIterationsArray = new int[] { 5, 5, 3, 5, 5, 5, 9, 3, 5, 12 };
            comboBox1.SelectedIndex = 0;
            UpdateIterationControls();
        }

        private void LoadLSystem(string filePath)
        {
            lSystem = new LSystem(filePath);
            generator = new LSystemGenerator(lSystem);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (lSystem == null || generator == null)
                return;

            var sequence = generator.GenerateSequence(currentIteration);

            drawer = new FractalDrawer(e.Graphics, this.ClientSize, pen, new PointF(0, 0), lSystem.StartDirection);

            // Определяем, является ли текущий фрактал деревом
            bool isTree = comboBox1.SelectedIndex == 8 || comboBox1.SelectedIndex == 9;

            drawer.Draw(sequence, lSystem.Angle, 10, stepDecreasePercent, colorChangeValue, penThicknessDecreasePercent, isTree);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLSystem(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\")) + comboBox1.SelectedItem.ToString() + ".txt");

            maxIterations = maxIterationsArray[comboBox1.SelectedIndex];
            currentIteration = 0;

            // Настройки для дерева со случайностью
            if (comboBox1.SelectedIndex == 9)
            {
                stepDecreasePercent = 15f;
                colorChangeValue = 18;
                pen = new Pen(Color.Brown, 20); // Начинаем с коричневого
                penThicknessDecreasePercent = 15f;
            }
            // Настройки для простого дерева
            else if (comboBox1.SelectedIndex == 8)
            {
                stepDecreasePercent = 15f;
                colorChangeValue = 15;
                pen = new Pen(Color.Brown, 15);
                penThicknessDecreasePercent = 15f;
            }
            else
            {
                stepDecreasePercent = 0f;
                colorChangeValue = 0;
                pen = Pens.Black;
                penThicknessDecreasePercent = 0f;
            }

            UpdateIterationControls();
            Invalidate();
        }

        private void btnPrevIteration_Click(object sender, EventArgs e)
        {
            if (currentIteration > 0)
            {
                currentIteration--;
                UpdateIterationControls();
                Invalidate();
            }
        }

        private void btnNextIteration_Click(object sender, EventArgs e)
        {
            if (currentIteration < maxIterations)
            {
                currentIteration++;
                UpdateIterationControls();
                Invalidate();
            }
        }

        private void UpdateIterationControls()
        {
            lblIterationInfo.Text = $"Итерация: {currentIteration}/{maxIterations}";
            btnPrevIteration.Enabled = currentIteration > 0;
            btnNextIteration.Enabled = currentIteration < maxIterations;
        }
    }

    public class LSystem
    {
        public string Axiom { get; set; }
        public double Angle { get; set; }
        public double StartDirection { get; set; }
        public Dictionary<char, string> Rules { get; set; }

        public LSystem(string filePath)
        {
            Rules = new Dictionary<char, string>();
            ParseFile(filePath);
        }

        private void ParseFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            var firstLine = lines[0].Split(' ');
            Axiom = firstLine[0];
            Angle = double.Parse(firstLine[1]);
            StartDirection = double.Parse(firstLine[2]);

            for (int i = 1; i < lines.Length; i++)
            {
                var rule = lines[i];
                if (!string.IsNullOrEmpty(rule))
                {
                    var parts = rule.Split('>');
                    Rules[parts[0][0]] = parts[1];
                }
            }
        }
    }

    public class LSystemGenerator
    {
        private readonly LSystem lSystem;
        public LSystemGenerator(LSystem lsystem)
        {
            lSystem = lsystem;
        }

        public string GenerateSequence(int iterations)
        {
            var current = lSystem.Axiom;

            for (int i = 0; i < iterations; i++)
            {
                var next = new StringBuilder();

                foreach (var symbol in current)
                {
                    if (lSystem.Rules.ContainsKey(symbol))
                    {
                        next.Append(lSystem.Rules[symbol]);
                    }
                    else
                    {
                        next.Append(symbol);
                    }
                }

                current = next.ToString();
            }

            return current;
        }
    }

    public class FractalDrawer
    {
        private readonly Graphics graphics;
        private readonly Size windowSize;
        private Pen pen;
        private PointF currentPosition;
        private double currentDirection;

        private double minX, minY, maxX, maxY;
        private double scaleCoef;

        private readonly Random random = new Random();
        private Queue<double> angles = new Queue<double>();

        public FractalDrawer(Graphics graphics, Size windowSize, Pen pen, PointF startPosition, double startDirection)
        {
            this.graphics = graphics;
            this.windowSize = windowSize;
            this.pen = pen;
            currentPosition = startPosition;
            currentDirection = startDirection;
        }

        public void CalculateBounds(string sequence, double angleIncrement, float stepLength, float stepDecreasePercent = 0f, bool isTree = false)
        {
            var stack = new Stack<(PointF position, double direction, float stepLength)>();
            var drawStack = new Stack<bool>();

            minX = maxX = 0;
            minY = maxY = 0;

            PointF currentPosition = this.currentPosition;
            double currentDirection = isTree ? -90 : this.currentDirection; // Для деревьев направление вверх
            bool isDrawing = true;

            double initialAngle = angleIncrement;

            foreach (var symbol in sequence)
            {
                if (symbol == 'F' || symbol == 'A' || symbol == 'B' || symbol == 'G')
                {
                    stepLength -= stepLength * (stepDecreasePercent / 100);
                    var nextPosition = CalculateNextPosition(stepLength, currentPosition, currentDirection);

                    if (isDrawing)
                    {
                        minX = Math.Min(minX, nextPosition.X);
                        minY = Math.Min(minY, nextPosition.Y);
                        maxX = Math.Max(maxX, nextPosition.X);
                        maxY = Math.Max(maxY, nextPosition.Y);
                    }

                    currentPosition = nextPosition;
                }
                else
                {
                    switch (symbol)
                    {
                        case '+':
                            currentDirection += angleIncrement;
                            break;

                        case '-':
                            currentDirection -= angleIncrement;
                            break;

                        case '[':
                            stack.Push((currentPosition, currentDirection, stepLength));
                            drawStack.Push(isDrawing);
                            break;

                        case ']':
                            if (stack.Count > 0)
                            {
                                var savedState = stack.Pop();
                                currentPosition = savedState.position;
                                currentDirection = savedState.direction;
                                stepLength = savedState.stepLength;
                                isDrawing = drawStack.Pop();
                            }
                            break;

                        case 'X':
                            break;

                        case '@':
                            angleIncrement = random.NextDouble() * initialAngle;
                            angles.Enqueue(angleIncrement);
                            break;
                    }
                }
            }

            double width = maxX - minX;
            double height = maxY - minY;

            if (width == 0) width = 1;
            if (height == 0) height = 1;

            double scaleX = (windowSize.Width - 80) / width;
            double scaleY = (windowSize.Height - 80) / height;

            scaleCoef = Math.Min(scaleX, scaleY);
        }

        public void Draw(string sequence, double angleIncrement, float stepLength,
                float stepDecreasePercent = 0f, int colorChangeValue = 0,
                float penThicknessDecreasePercent = 0f, bool isTree = false) // Добавил параметр isTree
        {
            if (string.IsNullOrEmpty(sequence))
                return;

            Pen initialPen = pen;

            CalculateBounds(sequence, angleIncrement, stepLength, stepDecreasePercent, isTree);

            double offsetX = (windowSize.Width - (maxX - minX) * scaleCoef) / 2;
            double offsetY = (windowSize.Height - (maxY - minY) * scaleCoef) / 2;

            double x = -minX * scaleCoef + offsetX;
            double y = -minY * scaleCoef + offsetY;

            // Для деревьев начинаем снизу экрана
            if (isTree)
            {
                x = windowSize.Width / 2;
                y = windowSize.Height - 50; // Отступ снизу
            }

            currentPosition = new PointF((float)x, (float)y);
            currentDirection = isTree ? -90 : 0; // Для деревьев направление вверх

            var stack = new Stack<(PointF position, double direction, float stepLength, Pen pen)>();
            var drawStack = new Stack<bool>();

            stepLength *= (float)scaleCoef;
            bool isDrawing = true;

            foreach (var symbol in sequence)
            {
                if (symbol == 'F' || symbol == 'A' || symbol == 'B' || symbol == 'G')
                {
                    if (isDrawing)
                    {
                        // Изменение цвета и толщины для деревьев
                        if (colorChangeValue > 0 && isTree)
                        {
                            int newRed = Math.Max(0, Math.Min(255, pen.Color.R - colorChangeValue));
                            int newGreen = Math.Max(0, Math.Min(255, pen.Color.G + colorChangeValue));
                            int newBlue = Math.Max(0, Math.Min(255, pen.Color.B - colorChangeValue / 2));

                            pen = new Pen(Color.FromArgb(newRed, newGreen, newBlue),
                                         Math.Max(1, pen.Width - pen.Width * (penThicknessDecreasePercent / 100)));
                        }
                        else if (isTree)
                        {
                            pen = new Pen(pen.Color, Math.Max(1, pen.Width - pen.Width * (penThicknessDecreasePercent / 100)));
                        }

                        stepLength = Math.Max(1, stepLength - stepLength * (stepDecreasePercent / 100));

                        var nextPosition = CalculateNextPosition(stepLength, currentPosition, currentDirection);
                        graphics.DrawLine(pen, currentPosition, nextPosition);
                        currentPosition = nextPosition;
                    }
                    else
                    {
                        stepLength = Math.Max(1, stepLength - stepLength * (stepDecreasePercent / 100));
                        currentPosition = CalculateNextPosition(stepLength, currentPosition, currentDirection);
                    }
                }
                else
                {
                    switch (symbol)
                    {
                        case '+':
                            // Для деревьев добавляем случайность в углы
                            if (isTree)
                            {
                                double randomAngle = angleIncrement * (0.8 + random.NextDouble() * 0.4);
                                currentDirection += randomAngle;
                            }
                            else
                            {
                                currentDirection += angleIncrement;
                            }
                            break;

                        case '-':
                            if (isTree)
                            {
                                double randomAngle = angleIncrement * (0.8 + random.NextDouble() * 0.4);
                                currentDirection -= randomAngle;
                            }
                            else
                            {
                                currentDirection -= angleIncrement;
                            }
                            break;

                        case '[':
                            stack.Push((currentPosition, currentDirection, stepLength, pen));
                            drawStack.Push(isDrawing);
                            break;

                        case ']':
                            if (stack.Count > 0)
                            {
                                var savedState = stack.Pop();
                                currentPosition = savedState.position;
                                currentDirection = savedState.direction;
                                stepLength = savedState.stepLength;
                                pen = savedState.pen;
                                isDrawing = drawStack.Pop();
                            }
                            break;

                        case 'X':
                            break;

                        case '@':
                            if (angles.Count > 0)
                                angleIncrement = angles.Dequeue();
                            break;
                    }
                }
            }
            pen = initialPen;
        }

        private PointF CalculateNextPosition(float stepLength, PointF position, double direction)
        {
            var radianAngle = direction * (Math.PI / 180.0);
            var nextX = position.X + (float)(stepLength * Math.Cos(radianAngle));
            var nextY = position.Y + (float)(stepLength * Math.Sin(radianAngle)); 
            return new PointF(nextX, nextY);
        }
    }
}