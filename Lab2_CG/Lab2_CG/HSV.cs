using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Lab2_CG
{
    class HSV
    {
        private PictureBox pictureBox;
        private TrackBar hueTrackBar;
        private TrackBar saturationTrackBar;
        private TrackBar valueTrackBar;
        private Bitmap orig;

        public HSV()
        {

        }
        public TrackBar getHueTrackBar()
        {
            return hueTrackBar;
        }
        public TrackBar getSaturationTrackBar()
        {
            return hueTrackBar;
        }
        public TrackBar getvalueTrackBar()
        {
            return valueTrackBar;
        }
        public Bitmap getOrig()
        {
            return orig;
        }

        public void Init(Control container)
        {
            int width = container.Width;
            int height = container.Height;

            pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = width - 20,
                Height = height - 150,
               
            };
            container.Controls.Add(pictureBox);

            hueTrackBar = new TrackBar
            {
                Width = width - 120,
                Minimum = 0,
                Maximum = 100,
                Value = 50,
                Top = pictureBox.Bottom + 10,
                
            };
            var hueLabel = new Label
            {
                Width = 100,
                Text = "Hue",
                Top = hueTrackBar.Top,
                Left = hueTrackBar.Right + 5

            };

            saturationTrackBar = new TrackBar
            {
                Width = width - 120,
                Minimum = 0,
                Maximum = 100,
                Value = 50,
                Top = hueTrackBar.Bottom + 10,
                
            };
            var saturationLabel = new Label
            {
                Width = 100,
                Text = "Saturation",
                Top = saturationTrackBar.Top,
                Left = saturationTrackBar.Right + 5
            };

            valueTrackBar = new TrackBar
            {
                Width = width - 120,
                Minimum = 0,
                Maximum = 100,
                Value = 50,
                Top = saturationTrackBar.Bottom + 10,
                
            };
            var valueLabel = new Label
            {
                Width = 100,
                Text = "Value",
                Top = valueTrackBar.Top,
                Left = valueTrackBar.Right + 5

            };

            hueTrackBar.Scroll += TrackBarScroll;
            saturationTrackBar.Scroll += TrackBarScroll;
            valueTrackBar.Scroll += TrackBarScroll;

            container.Controls.Add(hueTrackBar);
            container.Controls.Add(hueLabel);
            container.Controls.Add(saturationTrackBar);
            container.Controls.Add(saturationLabel);
            container.Controls.Add(valueTrackBar);
            container.Controls.Add(valueLabel);

            
        }

        private void RefreshPicture()
        {
            if (orig == null) return;

            int hue = hueTrackBar.Value;
            int saturation = saturationTrackBar.Value;
            int value = valueTrackBar.Value;

            Bitmap result = new Bitmap(orig);

            using (var fastBitmap = new FastBitmap(result))
            {
                for (var x = 0; x < fastBitmap.Width; x++)
                {
                    for (var y = 0; y < fastBitmap.Height; y++)
                    {
                        var color = fastBitmap[x, y];
                        var transformedColor = HSVTransform(color, hue, saturation, value);
                        fastBitmap[x, y] = transformedColor;
                    }
                }
            }

            pictureBox.Image?.Dispose();
            pictureBox.Image = result;
        }

        public Color HSVTransform(Color color, int hue, int saturation, int value)
        {
            double h, s, v;
            RGBtoHSV(color, out h, out s, out v);

            h = HueTransform(h, hue);
            s = SaturationTransform(s, saturation);
            v = ValueTransform(v, value);

            return HSVtoRGB(h, s, v);
        }

        private double HueTransform(double hue, int depth)
        {
            double hueShift = (depth - 50) * 3.6; 
            return (hue + hueShift + 360) % 360;
        }

        private double SaturationTransform(double saturation, int depth)
        {
            if (depth < 50)
                return saturation * depth * 2 / 100;
            else
                return saturation + (1.0 - saturation) * (depth - 100 / 2) * 2 / 100;
        }

        private double ValueTransform(double value, int depth)
        {
            if (depth < 100 / 2)
                return value * depth * 2 / 100;
            else
                return value + (1.0 - value) * (depth - 100 / 2) * 2 / 100;
        }

        private void RGBtoHSV(Color color, out double hue, out double saturation, out double value)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));

            value = max;

            if (max == 0)
                saturation = 0;
            else            
                saturation = 1 - (min / max);
     

            if (max == min)
                hue = 0; 
            else
            {
                if (max == r)
                {
                    if (g >= b)
                        hue = 60 * (g - b) / (max - min);
                    else
                        hue = 60 * (g - b) / (max - min) + 360;
                }
                else if (max == g)
                    hue = 60 * (b - r) / (max - min) + 120;
                else
                    hue = 60 * (r - g) / (max - min) + 240;
            }
        }

        private Color HSVtoRGB(double hue, double saturation, double value)
        {
            hue = hue % 360;
            if (hue < 0) hue += 360;

            double hi = Math.Floor(hue / 60) % 6;
            double f = (hue / 60) - Math.Floor(hue / 60);

            double p = value * (1 - saturation);
            double q = value * (1 - f * saturation);
            double t = value * (1 - (1 - f) * saturation);

            double r, g, b;

            if ((int)hi == 0)
            {
                r = value; g = t; b = p;
            }
            else if ((int)hi == 1)
            {
                r = q; g = value; b = p;
            }
            else if ((int)hi == 2)
            {
                r = p; g = value; b = t;
            }
            else if ((int)hi == 3)
            {
                r = p; g = q; b = value;
            }
            else if ((int)hi == 4)
            {
                r = t; g = p; b = value;
            }
            else
            {
                r = value; g = p; b = q;
            }         

            return Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
        }

        private void TrackBarScroll(object sender, EventArgs e)
        {
            RefreshPicture();
        }

        public void Show(Bitmap bitmap)
        {
            hueTrackBar.Value = 50;
            saturationTrackBar.Value = 50;
            valueTrackBar.Value = 50;
            this.orig = bitmap;
            RefreshPicture();
        }

       
    }
}