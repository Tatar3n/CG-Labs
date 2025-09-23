using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Lab2_CG
{
    public partial class Form2 : Form
    {
        private HSV hsvFilter;
        private Bitmap originalImage;

        public Form2()
        {
            InitializeComponent();
            this.Text = "HSV Filter";
            this.Size = new Size(1000, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            var loadButton = new Button
            {
                Text = "Load Image",
                Location = new Point(10, 10),
                Size = new Size(100, 30)
            };
            loadButton.Click += LoadButton_Click;

            var pictureBox = new PictureBox
            {
                Location = new Point(10, 50),
                Size = new Size(200, 200),
                SizeMode = PictureBoxSizeMode.Zoom,
            };

            var hsvPanel = new Panel
            {
                Location = new Point(200, 50),
                Size = new Size(600, 400),
            };
            var saveButton = new Button
            {
                Text = "Save Image",
                Location = new Point(110,10),
                Size = new Size(100, 30)
            };
            saveButton.Click += SaveButton_Click;


            hsvFilter = new HSV();
            hsvFilter.Init(hsvPanel);

            this.Controls.Add(loadButton);

            this.Controls.Add(pictureBox);
            this.Controls.Add(hsvPanel);
            this.Controls.Add(saveButton);

            this.originalImage = null;
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        originalImage = new Bitmap(openFileDialog.FileName);
                        
                        var pictureBox = this.Controls[1] as PictureBox;
                        if (pictureBox != null)
                        {
                            pictureBox.Image?.Dispose();
                            pictureBox.Image = new Bitmap(originalImage);
                        }

                        hsvFilter.Show(originalImage);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        public void SaveButton_Click(object sender, EventArgs e)
        {
            if (hsvFilter.getOrig() == null) return;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "png files (*.png)|*.png";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fname = saveFileDialog1.FileName;
                if (!string.IsNullOrEmpty(fname))
                {
                    int hue = hsvFilter.getHueTrackBar().Value;
                    int saturation = hsvFilter.getSaturationTrackBar().Value;
                    int value = hsvFilter.getvalueTrackBar().Value;

                    Bitmap result = new Bitmap(hsvFilter.getOrig());

                    using (var fastBitmap = new FastBitmap(result))
                    {
                        for (var x = 0; x < fastBitmap.Width; x++)
                        {
                            for (var y = 0; y < fastBitmap.Height; y++)
                            {
                                var color = fastBitmap[x, y];
                                var transformedColor = hsvFilter.HSVTransform(color, hue, saturation, value);
                                fastBitmap[x, y] = transformedColor;
                            }
                        }
                    }

                    result.Save(fname, ImageFormat.Png);
                    result.Dispose();
                }
            }
        }

    }
}