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
using ImageEdit_WPF.HelperClasses;

namespace ImageEdit_WPF.Windows {
    /// <summary>
    /// Interaction logic for Histogram.xaml
    /// </summary>
    public partial class Histogram : Window, INotifyPropertyChanged {
        private ImageEditData m_data = null;

        /// <summary>
        /// Points that represent the values of the histogram.
        /// </summary>
        private PointCollection _histogramPoints = null;

        /// <summary>
        /// Histogram of the Red channel.
        /// </summary>
        private int[] _histogramR = new int[256];

        /// <summary>
        /// Histogram of the Green channel.
        /// </summary>
        private int[] _histogramG = new int[256];

        /// <summary>
        /// Histogram of the Blue channel.
        /// </summary>
        private int[] _histogramB = new int[256];

        /// <summary>
        /// Histogram of the Luminance values.
        /// </summary>
        private int[] _histogramY = new int[256];

        /// <summary>
        /// Check if the histogram of the red channel has been already calculated.
        /// </summary>
        private bool _isCalculatedR = false;

        /// <summary>
        /// Check if the histogram of the green channel has been already calculated.
        /// </summary>
        private bool _isCalculatedG = false;

        /// <summary>
        /// Check if the histogram of the blue channel has been already calculated.
        /// </summary>
        private bool _isCalculatedB = false;

        /// <summary>
        /// Check if the histogram of the luminance values has been already calculated.
        /// </summary>
        private bool _isCalculatedY = false;

        /// <summary>
        /// Event for detecting changes in properties.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Histogram <c>constructor</c>.
        /// Here we initialiaze the image, the data binding of the histogram diagram and the default hitogram that will be loaded.
        /// </summary>
        /// <param name="data">Input image.</param>
        public Histogram(ImageEditData data) {
            m_data = data;

            InitializeComponent();
            DataContext = this;
            gray.IsChecked = true;
        }

        /// <summary>
        /// Get or set histogram's points. Checking if we have a different set of points to show.
        /// </summary>
        public PointCollection HistogramPoints {
            get { return _histogramPoints; }
            set {
                if (_histogramPoints != value) {
                    _histogramPoints = value;
                    OnPropertytChanged("HistogramPoints");
                }
            }
        }

        private void OnPropertytChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Histogram calculations
        /// <summary>
        /// Calculating the histogram of the red channel.
        /// </summary>
        /// <returns>
        /// Histogram of the red channel.
        /// </returns>
        public int[] HistogramRed() {
            int r;

            // Lock the bitmap's bits.  
            BitmapData bmpData = m_data.M_bmpOutput.LockBits(new Rectangle(0, 0, m_data.M_bmpOutput.Width, m_data.M_bmpOutput.Height), ImageLockMode.ReadWrite, m_data.M_bmpOutput.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*m_data.M_bmpOutput.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < 256; i++) {
                _histogramR[i] = 0;
            }

            for (int i = 0; i < m_data.M_bmpOutput.Width; i++) {
                for (int j = 0; j < m_data.M_bmpOutput.Height; j++) {
                    int index = (j*bmpData.Stride) + (i*3);

                    r = rgbValues[index + 2];
                    _histogramR[r]++;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            m_data.M_bmpOutput.UnlockBits(bmpData);

            _isCalculatedR = true;

            return _histogramR;
        }

        /// <summary>
        /// Calculating the histogram of the green channel.
        /// </summary>
        /// <returns>
        /// Histogram of the green channel.
        /// </returns>
        public int[] HistogramGreen() {
            int g;

            // Lock the bitmap's bits.  
            BitmapData bmpData = m_data.M_bmpOutput.LockBits(new Rectangle(0, 0, m_data.M_bmpOutput.Width, m_data.M_bmpOutput.Height), ImageLockMode.ReadWrite, m_data.M_bmpOutput.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*m_data.M_bmpOutput.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < 256; i++) {
                _histogramG[i] = 0;
            }

            for (int i = 0; i < m_data.M_bmpOutput.Width; i++) {
                for (int j = 0; j < m_data.M_bmpOutput.Height; j++) {
                    int index = (j*bmpData.Stride) + (i*3);

                    g = rgbValues[index + 1];
                    _histogramG[g]++;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            m_data.M_bmpOutput.UnlockBits(bmpData);

            _isCalculatedG = true;

            return _histogramG;
        }

        /// <summary>
        /// Calculating the histogram of the blue channel.
        /// </summary>
        /// <returns>
        /// Histogram of the blue channel.
        /// </returns>
        public int[] HistogramBlue() {
            int b;

            // Lock the bitmap's bits.  
            BitmapData bmpData = m_data.M_bmpOutput.LockBits(new Rectangle(0, 0, m_data.M_bmpOutput.Width, m_data.M_bmpOutput.Height), ImageLockMode.ReadWrite, m_data.M_bmpOutput.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*m_data.M_bmpOutput.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < 256; i++) {
                _histogramB[i] = 0;
            }

            for (int i = 0; i < m_data.M_bmpOutput.Width; i++) {
                for (int j = 0; j < m_data.M_bmpOutput.Height; j++) {
                    int index = (j*bmpData.Stride) + (i*3);

                    b = rgbValues[index];
                    _histogramB[b]++;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            m_data.M_bmpOutput.UnlockBits(bmpData);

            _isCalculatedB = true;

            return _histogramB;
        }

        /// <summary>
        /// Calculating the histogram for the luminance values.
        /// </summary>
        /// <returns>
        /// Histogram of the luminance values.
        /// </returns>
        public int[] HistogramLuminance() {
            int r;
            int g;
            int b;
            int y;

            // Lock the bitmap's bits.  
            BitmapData bmpData = m_data.M_bmpOutput.LockBits(new Rectangle(0, 0, m_data.M_bmpOutput.Width, m_data.M_bmpOutput.Height), ImageLockMode.ReadWrite, m_data.M_bmpOutput.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*m_data.M_bmpOutput.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < 256; i++) {
                _histogramY[i] = 0;
            }

            for (int i = 0; i < m_data.M_bmpOutput.Width; i++) {
                for (int j = 0; j < m_data.M_bmpOutput.Height; j++) {
                    int index = (j*bmpData.Stride) + (i*3);

                    r = rgbValues[index + 2];
                    g = rgbValues[index + 1];
                    b = rgbValues[index];

                    y = (int)(0.2126*r + 0.7152*g + 0.0722*b); // source = ????

                    _histogramY[y]++;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            m_data.M_bmpOutput.UnlockBits(bmpData);

            _isCalculatedY = true;

            return _histogramY;
        }
        #endregion

        #region Channels
        /// <summary>
        /// Convert raw integer values into a <c>PointCollection</c>.
        /// </summary>
        /// <param name="values">Histogram for each channel as well as the one with the luminance values.</param>
        /// <returns>
        /// A set of <c>PointCollection</c> for the prefered histogram.
        /// </returns>
        private PointCollection ConvertToPointCollection(int[] values) {
            int max = values.Max();

            PointCollection points = new PointCollection();
            // first point (lower-left corner)
            points.Add(new System.Windows.Point(0, max));
            // middle points
            for (int i = 0; i < values.Length; i++) {
                points.Add(new System.Windows.Point(i, max - values[i]));
            }
            // last point (lower-right corner)
            points.Add(new System.Windows.Point(values.Length - 1, max));

            return points;
        }

        /// <summary>
        /// If gray radioBox is selected, calculate or just show the histogram of the luminance values.
        /// groupBox header is also changed to reflect the current selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gray_Checked(object sender, RoutedEventArgs e) {
            groupBox.Header = "Luminosity";
            polygon.Fill = new SolidColorBrush(Colors.Black);
            if (_isCalculatedY) {
                HistogramPoints = ConvertToPointCollection(_histogramY);
            } else {
                HistogramPoints = ConvertToPointCollection(HistogramLuminance());
            }
        }

        /// <summary>
        /// If red radioBox is selected, calculate or just show the histogram of the red channel.
        /// groupBox header is also changed to reflect the current selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void red_Checked(object sender, RoutedEventArgs e) {
            groupBox.Header = "Red";
            polygon.Fill = new SolidColorBrush(Colors.Red);
            if (_isCalculatedR) {
                HistogramPoints = ConvertToPointCollection(_histogramR);
            } else {
                HistogramPoints = ConvertToPointCollection(HistogramRed());
            }
        }

        /// <summary>
        /// If red radioBox is selected, calculate or just show the histogram of the green channel.
        /// groupBox header is also changed to reflect the current selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void green_Checked(object sender, RoutedEventArgs e) {
            groupBox.Header = "Green";
            polygon.Fill = new SolidColorBrush(Colors.Green);
            if (_isCalculatedG) {
                HistogramPoints = ConvertToPointCollection(_histogramG);
            } else {
                HistogramPoints = ConvertToPointCollection(HistogramGreen());
            }
        }

        /// <summary>
        /// If red radioBox is selected, calculate or just show the histogram of the blue channel.
        /// groupBox header is also changed to reflect the current selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void blue_Checked(object sender, RoutedEventArgs e) {
            groupBox.Header = "Blue";
            polygon.Fill = new SolidColorBrush(Colors.Blue);
            if (_isCalculatedB) {
                HistogramPoints = ConvertToPointCollection(_histogramB);
            } else {
                HistogramPoints = ConvertToPointCollection(HistogramBlue());
            }
        }
        #endregion
    }
}