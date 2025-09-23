// RGBChannelExtractorForm.cs
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Linq;

namespace Lab2_CG
{
    public partial class MainForm : Form
    {
        private Bitmap originalImage;
        private PictureBox pbOriginal, pbRed, pbGreen, pbBlue;
        private Panel histogramPanel;
        private Button btnLoad;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "RGB Channel Extractor";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            SetupUI();

            this.ResumeLayout(false);
        }

        private void SetupUI()
        {
            btnLoad = new Button
            {
                Text = "Загрузить изображение",
                Location = new Point(10, 10),
                Size = new Size(150, 30)
            };
            btnLoad.Click += BtnLoad_Click;
            this.Controls.Add(btnLoad);

            pbOriginal = new PictureBox
            {
                Location = new Point(10, 50),
                Size = new Size(300, 300),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            this.Controls.Add(pbOriginal);

            var lblOriginal = new Label
            {
                Text = "Оригинал",
                Location = new Point(10, 355),
                AutoSize = true
            };
            this.Controls.Add(lblOriginal);

            pbRed = new PictureBox
            {
                Location = new Point(320, 50),
                Size = new Size(200, 200),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            this.Controls.Add(pbRed);

            var lblRed = new Label
            {
                Text = "Красный канал (R)",
                Location = new Point(320, 255),
                AutoSize = true,
                ForeColor = Color.Red
            };
            this.Controls.Add(lblRed);

            pbGreen = new PictureBox
            {
                Location = new Point(530, 50),
                Size = new Size(200, 200),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            this.Controls.Add(pbGreen);

            var lblGreen = new Label
            {
                Text = "Зеленый канал (G)",
                Location = new Point(530, 255),
                AutoSize = true,
                ForeColor = Color.Green
            };
            this.Controls.Add(lblGreen);

            pbBlue = new PictureBox
            {
                Location = new Point(740, 50),
                Size = new Size(200, 200),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            this.Controls.Add(pbBlue);

            var lblBlue = new Label
            {
                Text = "Синий канал (B)",
                Location = new Point(740, 255),
                AutoSize = true,
                ForeColor = Color.Blue
            };
            this.Controls.Add(lblBlue);

            histogramPanel = new Panel
            {
                Location = new Point(10, 380),
                Size = new Size(1160, 600),
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };
            this.Controls.Add(histogramPanel);
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        originalImage = new Bitmap(openFileDialog.FileName);
                        pbOriginal.Image = originalImage;
                        ExtractChannels();
                        DrawHistograms();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
                    }
                }
            }
        }

        private void ExtractChannels()
        {
            if (originalImage == null) return;

            int width = originalImage.Width;
            int height = originalImage.Height;

            Bitmap redChannel = new Bitmap(width, height);
            Bitmap greenChannel = new Bitmap(width, height);
            Bitmap blueChannel = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixel = originalImage.GetPixel(x, y);

                    redChannel.SetPixel(x, y, Color.FromArgb(pixel.R, 0, 0));

                    greenChannel.SetPixel(x, y, Color.FromArgb(0, pixel.G, 0));

                    blueChannel.SetPixel(x, y, Color.FromArgb(0, 0, pixel.B));
                }
            }

            pbRed.Image = redChannel;
            pbGreen.Image = greenChannel;
            pbBlue.Image = blueChannel;
        }

        private void DrawHistograms()
        {
            if (originalImage == null) return;

            histogramPanel.Controls.Clear();

            int[] redHistogram = new int[256];
            int[] greenHistogram = new int[256];
            int[] blueHistogram = new int[256];

            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color pixel = originalImage.GetPixel(x, y);
                    redHistogram[pixel.R]++;
                    greenHistogram[pixel.G]++;
                    blueHistogram[pixel.B]++;
                }
            }

            int maxRed = redHistogram.Max();
            int maxGreen = greenHistogram.Max();
            int maxBlue = blueHistogram.Max();
            int maxOverall = Math.Max(maxRed, Math.Max(maxGreen, maxBlue));

            CreateHistogram(redHistogram, maxOverall, Color.Red, "Красный канал", 0);
            CreateHistogram(greenHistogram, maxOverall, Color.Green, "Зеленый канал", 1);
            CreateHistogram(blueHistogram, maxOverall, Color.Blue, "Синий канал", 2);
        }

        private void CreateHistogram(int[] histogram, int maxValue, Color color, string title, int index)
        {
            int width = 750;
            int height = 180;
            Bitmap histogramBitmap = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(histogramBitmap))
            {
                g.Clear(Color.White);

                using (Font titleFont = new Font("Arial", 10, FontStyle.Bold))
                {
                    g.DrawString(title, titleFont, new SolidBrush(color), 10, 10);
                }

                g.DrawLine(Pens.Black, 50, 40, 50, height - 30);
                g.DrawLine(Pens.Black, 50, height - 30, width - 20, height - 30);

                float barWidth = (width - 70) / 256f;
                using (Pen histogramPen = new Pen(color, barWidth))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        if (histogram[i] > 0)
                        {
                            float x = 50 + i * barWidth;
                            float barHeight = (histogram[i] / (float)maxValue) * (height - 70);
                            float y = height - 30 - barHeight;

                            g.DrawLine(histogramPen, x, height - 30, x, y);
                        }
                    }
                }

                using (Font axisFont = new Font("Arial", 8))
                {
                    g.DrawString("Интенсивность", axisFont, Brushes.Black, width / 2 - 30, height - 15);

                    StringFormat verticalFormat = new StringFormat();
                    verticalFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                    g.DrawString("Количество пикселей", axisFont, Brushes.Black, 15, height / 2 - 30, verticalFormat);
                }

                using (Font smallFont = new Font("Arial", 7))
                {
                    for (int i = 0; i <= 255; i += 32)
                    {
                        float x = 50 + i * barWidth;
                        g.DrawLine(Pens.Gray, x, height - 30, x, height - 25);
                        g.DrawString(i.ToString(), smallFont, Brushes.Black, x - 10, height - 25);
                    }

                    int step = maxValue / 5;
                    for (int i = 0; i <= 5; i++)
                    {
                        int value = i * step;
                        float y = height - 30 - (i * (height - 70) / 5f);
                        g.DrawLine(Pens.Gray, 45, y, 50, y);
                        g.DrawString(value.ToString("N0"), smallFont, Brushes.Black, 25, y - 5);
                    }
                }
            }

            PictureBox histogramBox = new PictureBox
            {
                Image = histogramBitmap,
                Size = new Size(width, height),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(10, 10 + index * (height + 10))
            };

            histogramPanel.Controls.Add(histogramBox);
        }
    }
}