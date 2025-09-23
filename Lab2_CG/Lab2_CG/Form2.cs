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
            hsvFilter = new HSV();
            hsvFilter.Init(hsvPanel);
            originalImage = null;

            loadButton.Click += LoadButton_Click;
            saveButton.Click += SaveButton_Click;
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
                        pictureBox.Image?.Dispose();
                        pictureBox.Image = new Bitmap(originalImage);
                        hsvFilter.Show(originalImage);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (hsvFilter.getOrig() == null) return;

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG files (*.png)|*.png";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fname = saveFileDialog.FileName;
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
}