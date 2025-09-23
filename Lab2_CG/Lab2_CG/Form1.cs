using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lab2_CG
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage;
        private Bitmap grayscaleImage1;
        private Bitmap grayscaleImage2;
        private Bitmap differenceImage;

        public Form1()
        {
            InitializeComponent();
            InitializeCharts();
        }

        private void InitializeCharts()
        {
            chartMethod1.Series.Clear();
            chartMethod1.ChartAreas.Clear();

            ChartArea chartArea1 = new ChartArea("Area1");
            chartArea1.AxisX.Title = "Интенсивность";
            chartArea1.AxisY.Title = "Количество пикселей";
            chartArea1.AxisX.Minimum = 0;
            chartArea1.AxisX.Maximum = 255;
            chartMethod1.ChartAreas.Add(chartArea1);

            Series series1 = new Series("Метод 1");
            series1.ChartType = SeriesChartType.Column;
            series1.Color = Color.SteelBlue;
            series1.BorderColor = Color.DarkBlue;
            series1.BorderWidth = 1;
            chartMethod1.Series.Add(series1);

            chartMethod2.Series.Clear();
            chartMethod2.ChartAreas.Clear();

            ChartArea chartArea2 = new ChartArea("Area2");
            chartArea2.AxisX.Title = "Интенсивность";
            chartArea2.AxisY.Title = "Количество пикселей";
            chartArea2.AxisX.Minimum = 0;
            chartArea2.AxisX.Maximum = 255;
            chartMethod2.ChartAreas.Add(chartArea2);

            Series series2 = new Series("Метод 2");
            series2.ChartType = SeriesChartType.Column;
            series2.Color = Color.IndianRed;
            series2.BorderColor = Color.DarkRed;
            series2.BorderWidth = 1;
            chartMethod2.Series.Add(series2);

            foreach (var chart in new[] { chartMethod1, chartMethod2 })
            {
                chart.ChartAreas[0].AxisX.Interval = 32;
                chart.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0}";
                chart.Legends.Clear();
            }
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        originalImage = new Bitmap(openFileDialog.FileName);
                        pictureBoxOriginal.Image = originalImage;

                        ConvertToGrayscale();

                        BuildHistograms();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ConvertToGrayscale()
        {
            if (originalImage == null) return;

            grayscaleImage1 = ConvertToGrayscaleWithFormula(originalImage, 0.299, 0.587, 0.114);

            grayscaleImage2 = ConvertToGrayscaleWithFormula(originalImage, 0.2126, 0.7152, 0.0722);

            differenceImage = CreateDifferenceImage(grayscaleImage1, grayscaleImage2);

            pictureBoxGray1.Image = grayscaleImage1;
            pictureBoxGray2.Image = grayscaleImage2;
            pictureBoxDiff.Image = differenceImage;

            UpdateImageInfo();
        }

        private Bitmap ConvertToGrayscaleWithFormula(Bitmap source, double rCoeff, double gCoeff, double bCoeff)
        {
            Bitmap result = new Bitmap(source.Width, source.Height);

            using (var fastSource = new FastBitmap(source))
            using (var fastResult = new FastBitmap(result))
            {
                for (int y = 0; y < source.Height; y++)
                {
                    for (int x = 0; x < source.Width; x++)
                    {
                        Color pixel = fastSource[x, y];
                        int gray = (int)(pixel.R * rCoeff + pixel.G * gCoeff + pixel.B * bCoeff);
                        gray = Math.Max(0, Math.Min(255, gray));
                        fastResult[x, y] = Color.FromArgb(gray, gray, gray);
                    }
                }
            }

            return result;
        }

        private Bitmap CreateDifferenceImage(Bitmap image1, Bitmap image2)
        {
            Bitmap result = new Bitmap(image1.Width, image1.Height);

            using (var fastImage1 = new FastBitmap(image1))
            using (var fastImage2 = new FastBitmap(image2))
            using (var fastResult = new FastBitmap(result))
            {
                for (int y = 0; y < image1.Height; y++)
                {
                    for (int x = 0; x < image1.Width; x++)
                    {
                        Color color1 = fastImage1[x, y];
                        Color color2 = fastImage2[x, y];
                        int diff = Math.Abs(color1.R - color2.R);
                        diff = Math.Min(255, diff * 3);
                        fastResult[x, y] = Color.FromArgb(diff, diff, diff);
                    }
                }
            }

            return result;
        }

        private void BuildHistograms()
        {
            if (grayscaleImage1 == null || grayscaleImage2 == null) return;

            int[] histogram1 = new int[256];
            int[] histogram2 = new int[256];

            using (var fastGray1 = new FastBitmap(grayscaleImage1))
            {
                for (int y = 0; y < fastGray1.Height; y++)
                {
                    for (int x = 0; x < fastGray1.Width; x++)
                    {
                        Color color = fastGray1[x, y];
                        histogram1[color.R]++;
                    }
                }
            }

            using (var fastGray2 = new FastBitmap(grayscaleImage2))
            {
                for (int y = 0; y < fastGray2.Height; y++)
                {
                    for (int x = 0; x < fastGray2.Width; x++)
                    {
                        Color color = fastGray2[x, y];
                        histogram2[color.R]++;
                    }
                }
            }

            chartMethod1.Series[0].Points.Clear();
            for (int i = 0; i < 256; i++)
            {
                chartMethod1.Series[0].Points.AddXY(i, histogram1[i]);
            }

            chartMethod2.Series[0].Points.Clear();
            for (int i = 0; i < 256; i++)
            {
                chartMethod2.Series[0].Points.AddXY(i, histogram2[i]);
            }

            chartMethod1.ChartAreas[0].AxisY.Maximum = double.NaN;
            chartMethod1.ChartAreas[0].AxisY.Minimum = 0;
            chartMethod2.ChartAreas[0].AxisY.Maximum = double.NaN;
            chartMethod2.ChartAreas[0].AxisY.Minimum = 0;
        }

        private void UpdateImageInfo()
        {
            if (originalImage == null) return;

            lblOriginalInfo.Text = $"{originalImage.Width} × {originalImage.Height}";
            lblGray1Info.Text = $"{grayscaleImage1.Width} × {grayscaleImage1.Height}";
            lblGray2Info.Text = $"{grayscaleImage2.Width} × {grayscaleImage2.Height}";
            lblDiffInfo.Text = $"{differenceImage.Width} × {differenceImage.Height}";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetColorScheme();
        }

        private void SetColorScheme()
        {
            this.BackColor = Color.FromArgb(240, 240, 240);

            foreach (Label lbl in new[] { lblTitleOriginal, lblTitleGray1, lblTitleGray2, lblTitleDiff })
            {
                lbl.BackColor = Color.SteelBlue;
                lbl.ForeColor = Color.White;
                lbl.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }

            foreach (Label lbl in new[] { lblOriginalInfo, lblGray1Info, lblGray2Info, lblDiffInfo })
            {
                lbl.BackColor = Color.LightSteelBlue;
                lbl.ForeColor = Color.DarkBlue;
            }

            foreach (Button btn in new[] { btnLoadImage })
            {
                btn.BackColor = Color.SteelBlue;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
            }
        }
    }
}