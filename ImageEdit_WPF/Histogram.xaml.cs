/*
Basic image processing software
<https://github.com/nlabiris/ImageEdit_WPF>

Copyright (C) 2015  Nikos Labiris

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/



using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace ImageEdit_WPF
{
    /// <summary>
    /// Interaction logic for Histogram.xaml
    /// </summary>
    public partial class Histogram : Window, INotifyPropertyChanged
    {
        private readonly Bitmap _bmpForEditing = null;
        private PointCollection _luminanceHistogramPoints = null;
        private readonly int[] _histogramR = new int[256];
        private readonly int[] _histogramG = new int[256];
        private readonly int[] _histogramB = new int[256];
        private readonly int[] _histogramY = new int[256];
        private bool _isCalculatedR = false;
        private bool _isCalculatedG = false;
        private bool _isCalculatedB = false;
        private bool _isCalculatedY = false;
        public event PropertyChangedEventHandler PropertyChanged;

        public Histogram(Bitmap bmp)
        {
            InitializeComponent();

            _bmpForEditing = bmp;

            this.DataContext = this;

            gray.IsChecked = true;
        }

        public PointCollection LuminanceHistogramPoints
        {
            get
            {
                return this._luminanceHistogramPoints;
            }
            set
            {
                if (this._luminanceHistogramPoints != value)
                {
                    this._luminanceHistogramPoints = value;
                    if (this.PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("LuminanceHistogramPoints"));
                    }
                }
            }
        }

        public int[] HistogramRed()
        {
            int r;

            // Lock the bitmap's bits.  
            BitmapData bmpData = _bmpForEditing.LockBits(new Rectangle(0, 0, _bmpForEditing.Width, _bmpForEditing.Height), ImageLockMode.ReadWrite, _bmpForEditing.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride) * _bmpForEditing.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < 256; i++)
            {
                _histogramR[i] = 0;
            }

            for (int i = 0; i < _bmpForEditing.Width; i++)
            {
                for (int j = 0; j < _bmpForEditing.Height; j++)
                {
                    int index = (j * bmpData.Stride) + (i * 3);

                    r = rgbValues[index + 2];
                    _histogramR[r]++;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            _bmpForEditing.UnlockBits(bmpData);

            _isCalculatedR = true;

            return _histogramR;
        }

        public int[] HistogramGreen()
        {
            int g;

            // Lock the bitmap's bits.  
            BitmapData bmpData = _bmpForEditing.LockBits(new Rectangle(0, 0, _bmpForEditing.Width, _bmpForEditing.Height), ImageLockMode.ReadWrite, _bmpForEditing.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride) * _bmpForEditing.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < 256; i++)
            {
                _histogramG[i] = 0;
            }

            for (int i = 0; i < _bmpForEditing.Width; i++)
            {
                for (int j = 0; j < _bmpForEditing.Height; j++)
                {
                    int index = (j * bmpData.Stride) + (i * 3);

                    g = rgbValues[index + 1];
                    _histogramG[g]++;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            _bmpForEditing.UnlockBits(bmpData);

            _isCalculatedG = true;

            return _histogramG;
        }

        public int[] HistogramBlue()
        {
            int b;

            // Lock the bitmap's bits.  
            BitmapData bmpData = _bmpForEditing.LockBits(new Rectangle(0, 0, _bmpForEditing.Width, _bmpForEditing.Height), ImageLockMode.ReadWrite, _bmpForEditing.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride) * _bmpForEditing.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < 256; i++)
            {
                _histogramB[i] = 0;
            }

            for (int i = 0; i < _bmpForEditing.Width; i++)
            {
                for (int j = 0; j < _bmpForEditing.Height; j++)
                {
                    int index = (j * bmpData.Stride) + (i * 3);

                    b = rgbValues[index];
                    _histogramB[b]++;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            _bmpForEditing.UnlockBits(bmpData);

            _isCalculatedB = true;

            return _histogramB;
        }



        public int[] HistogramLuminance()
        {
            int r;
            int g;
            int b;
            int y;

            // Lock the bitmap's bits.  
            BitmapData bmpData = _bmpForEditing.LockBits(new Rectangle(0, 0, _bmpForEditing.Width, _bmpForEditing.Height), ImageLockMode.ReadWrite, _bmpForEditing.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride) * _bmpForEditing.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < 256; i++)
            {
                _histogramY[i] = 0;
            }

            for (int i = 0; i < _bmpForEditing.Width; i++)
            {
                for (int j = 0; j < _bmpForEditing.Height; j++)
                {
                    int index = (j * bmpData.Stride) + (i * 3);

                    r = rgbValues[index + 2];
                    g = rgbValues[index + 1];
                    b = rgbValues[index];

                    y = (int)(0.2126 * r + 0.7152 * g + 0.0722 * b);

                    _histogramY[y]++;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            _bmpForEditing.UnlockBits(bmpData);

            _isCalculatedY = true;

            return _histogramY;
        }

        private PointCollection ConvertToPointCollection(int[] values)
        {
            int max = values.Max();

            PointCollection points = new PointCollection();
            // first point (lower-left corner)
            points.Add(new System.Windows.Point(0, max));
            // middle points
            for (int i = 0; i < values.Length; i++)
            {
                points.Add(new System.Windows.Point(i, max - values[i]));
            }
            // last point (lower-right corner)
            points.Add(new System.Windows.Point(values.Length - 1, max));

            return points;
        }

        private void gray_Checked(object sender, RoutedEventArgs e)
        {
            groupBox.Header = "Luminosity (Gray)";
            polygon.Fill = new SolidColorBrush(Colors.Black);
            if (_isCalculatedY)
            {
                this.LuminanceHistogramPoints = ConvertToPointCollection(_histogramY);
            }
            else
            {
                this.LuminanceHistogramPoints = ConvertToPointCollection(HistogramLuminance());
            }
        }

        private void red_Checked(object sender, RoutedEventArgs e)
        {
            groupBox.Header = "Red";
            polygon.Fill = new SolidColorBrush(Colors.Red);
            if (_isCalculatedR)
            {
                this.LuminanceHistogramPoints = ConvertToPointCollection(_histogramR);
            }
            else
            {
                this.LuminanceHistogramPoints = ConvertToPointCollection(HistogramRed());
            }
        }

        private void green_Checked(object sender, RoutedEventArgs e)
        {
            groupBox.Header = "Green";
            polygon.Fill = new SolidColorBrush(Colors.Green);
            if (_isCalculatedG)
            {
                this.LuminanceHistogramPoints = ConvertToPointCollection(_histogramG);
            }
            else
            {
                this.LuminanceHistogramPoints = ConvertToPointCollection(HistogramGreen());
            }
        }

        private void blue_Checked(object sender, RoutedEventArgs e)
        {
            groupBox.Header = "Blue";
            polygon.Fill = new SolidColorBrush(Colors.Blue);
            if (_isCalculatedB)
            {
                this.LuminanceHistogramPoints = ConvertToPointCollection(_histogramB);
            }
            else
            {
                this.LuminanceHistogramPoints = ConvertToPointCollection(HistogramBlue());
            }
        } 
    }
}
