/*
Basic image processing software
<https://github.com/nlabiris/ImageEdit>

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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageEdit_WPF
{
    /// <summary>
    /// Interaction logic for Histogram.xaml
    /// </summary>
    public partial class Histogram : Window, INotifyPropertyChanged
    {
        private String filename;
        private Bitmap bmpForEditing = null;
        private PointCollection luminanceHistogramPoints = null;
        private Int32[] HistogramR = new Int32[256];
        private Int32[] HistogramG = new Int32[256];
        private Int32[] HistogramB = new Int32[256];
        private Int32[] HistogramY = new Int32[256];
        private Boolean isCalculatedR = false;
        private Boolean isCalculatedG = false;
        private Boolean isCalculatedB = false;
        private Boolean isCalculatedY = false;
        public event PropertyChangedEventHandler PropertyChanged;

        public Histogram(String fname, Bitmap bmp)
        {
            InitializeComponent();

            filename = fname;
            bmpForEditing = bmp;

            this.DataContext = this;

            gray.IsChecked = true;
        }

        public PointCollection LuminanceHistogramPoints
        {
            get
            {
                return this.luminanceHistogramPoints;
            }
            set
            {
                if (this.luminanceHistogramPoints != value)
                {
                    this.luminanceHistogramPoints = value;
                    if (this.PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("LuminanceHistogramPoints"));
                    }
                }
            }
        }

        public Int32[] HistogramRed()
        {
            Int32 R;

            // Lock the bitmap's bits.  
            BitmapData bmpData = bmpForEditing.LockBits(new System.Drawing.Rectangle(0, 0, bmpForEditing.Width, bmpForEditing.Height), ImageLockMode.ReadWrite, bmpForEditing.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            Int32 bytes = Math.Abs(bmpData.Stride) * bmpForEditing.Height;
            Byte[] rgbValues = new Byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < 256; i++)
            {
                HistogramR[i] = 0;
            }

            for (int i = 0; i < bmpForEditing.Width; i++)
            {
                for (int j = 0; j < bmpForEditing.Height; j++)
                {
                    int index = (j * bmpData.Stride) + (i * 3);

                    R = rgbValues[index + 2];
                    HistogramR[R]++;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmpForEditing.UnlockBits(bmpData);

            isCalculatedR = true;

            return HistogramR;
        }

        public Int32[] HistogramGreen()
        {
            Int32 G;

            // Lock the bitmap's bits.  
            BitmapData bmpData = bmpForEditing.LockBits(new System.Drawing.Rectangle(0, 0, bmpForEditing.Width, bmpForEditing.Height), ImageLockMode.ReadWrite, bmpForEditing.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            Int32 bytes = Math.Abs(bmpData.Stride) * bmpForEditing.Height;
            Byte[] rgbValues = new Byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < 256; i++)
            {
                HistogramG[i] = 0;
            }

            for (int i = 0; i < bmpForEditing.Width; i++)
            {
                for (int j = 0; j < bmpForEditing.Height; j++)
                {
                    int index = (j * bmpData.Stride) + (i * 3);

                    G = rgbValues[index + 1];
                    HistogramG[G]++;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmpForEditing.UnlockBits(bmpData);

            isCalculatedG = true;

            return HistogramG;
        }

        public Int32[] HistogramBlue()
        {
            Int32 B;

            // Lock the bitmap's bits.  
            BitmapData bmpData = bmpForEditing.LockBits(new System.Drawing.Rectangle(0, 0, bmpForEditing.Width, bmpForEditing.Height), ImageLockMode.ReadWrite, bmpForEditing.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            Int32 bytes = Math.Abs(bmpData.Stride) * bmpForEditing.Height;
            Byte[] rgbValues = new Byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < 256; i++)
            {
                HistogramB[i] = 0;
            }

            for (int i = 0; i < bmpForEditing.Width; i++)
            {
                for (int j = 0; j < bmpForEditing.Height; j++)
                {
                    int index = (j * bmpData.Stride) + (i * 3);

                    B = rgbValues[index];
                    HistogramB[B]++;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmpForEditing.UnlockBits(bmpData);

            isCalculatedB = true;

            return HistogramB;
        }



        public Int32[] HistogramLuminance()
        {
            Int32 R;
            Int32 G;
            Int32 B;
            Int32 Y;

            // Lock the bitmap's bits.  
            BitmapData bmpData = bmpForEditing.LockBits(new System.Drawing.Rectangle(0, 0, bmpForEditing.Width, bmpForEditing.Height), ImageLockMode.ReadWrite, bmpForEditing.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            Int32 bytes = Math.Abs(bmpData.Stride) * bmpForEditing.Height;
            Byte[] rgbValues = new Byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < 256; i++)
            {
                HistogramY[i] = 0;
            }

            for (int i = 0; i < bmpForEditing.Width; i++)
            {
                for (int j = 0; j < bmpForEditing.Height; j++)
                {
                    int index = (j * bmpData.Stride) + (i * 3);

                    R = rgbValues[index + 2];
                    G = rgbValues[index + 1];
                    B = rgbValues[index];

                    Y = (Int32)(0.2126 * R + 0.7152 * G + 0.0722 * B);

                    HistogramY[Y]++;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmpForEditing.UnlockBits(bmpData);

            isCalculatedY = true;

            return HistogramY;
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
            if (isCalculatedY)
            {
                this.LuminanceHistogramPoints = ConvertToPointCollection(HistogramY);
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
            if (isCalculatedR)
            {
                this.LuminanceHistogramPoints = ConvertToPointCollection(HistogramR);
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
            if (isCalculatedG)
            {
                this.LuminanceHistogramPoints = ConvertToPointCollection(HistogramG);
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
            if (isCalculatedB)
            {
                this.LuminanceHistogramPoints = ConvertToPointCollection(HistogramB);
            }
            else
            {
                this.LuminanceHistogramPoints = ConvertToPointCollection(HistogramBlue());
            }
        } 
    }
}
