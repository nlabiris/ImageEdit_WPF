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

using ImageAlgorithms.Algorithms;
using ImageEdit_WPF.HelperClasses;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace ImageEdit_WPF.Windows {
    /// <summary>
    /// Interaction logic for Histogram.xaml
    /// </summary>
    public partial class Histogram : Window {
        /// <summary>
        /// Image data.
        /// </summary>
        private ImageData m_data = null;

        /// <summary>
        /// Object used for binding with the UI.
        /// </summary>
        private ViewModel m_vm = null;

        /// <summary>
        /// Histogram of the Red channel.
        /// </summary>
        private int[] m_histogramR = null;

        /// <summary>
        /// Histogram of the Green channel.
        /// </summary>
        private int[] m_histogramG = null;

        /// <summary>
        /// Histogram of the Blue channel.
        /// </summary>
        private int[] m_histogramB = null;

        /// <summary>
        /// Histogram of the Luminance values.
        /// </summary>
        private int[] m_histogramY = null;

        /// <summary>
        /// Mean value of the histogram for the Red channel.
        /// </summary>
        private float m_histogramMeanR = 0;

        /// <summary>
        /// Mean value of the histogram for the Green channel.
        /// </summary>
        private float m_histogramMeanG = 0;

        /// <summary>
        /// Mean value of the histogram for the Blue channel.
        /// </summary>
        private float m_histogramMeanB = 0;

        /// <summary>
        /// Mean value of the histogram for the Luminance values.
        /// </summary>
        private float m_histogramMeanY = 0;

        /// <summary>
        /// Check if we have already calculated histogram for the Red channel.
        /// </summary>
        private bool m_isCalculatedR = false;

        /// <summary>
        /// Check if we have already calculated histogram for the Green channel.
        /// </summary>
        private bool m_isCalculatedG = false;

        /// <summary>
        /// Check if we have already calculated histogram for the Blue channel.
        /// </summary>
        private bool m_isCalculatedB = false;

        /// <summary>
        /// Check if we have already calculated histogram for the Luminance.
        /// </summary>
        private bool m_isCalculatedY = false;

        /// <summary>
        /// Histogram <c>constructor</c>.
        /// Here we initialiaze the image, the data binding of the histogram diagram and the default hitogram that will be loaded.
        /// </summary>
        /// <param name="data">Input image.</param>
        public Histogram(ImageData data, ViewModel vm) {
            m_data = data;
            m_vm = vm;

            InitializeComponent();
            DataContext = m_vm;
            gray.IsChecked = true;
        }

        #region Channels
        /// <summary>
        /// Convert raw integer values into a <c>PointCollection</c>.
        /// </summary>
        /// <param name="values">Histogram for each channel as well as the one with the luminance values.</param>
        /// <returns>
        /// A set of <c>PointCollection</c> for the prefered histogram.
        /// </returns>
        private static PointCollection ConvertToPointCollection(int[] values) {
            int max = values.Max();

            PointCollection points = new PointCollection();
            // first point (lower-left corner)
            points.Add(new Point(0, max));
            // middle points
            for (int i = 0; i < values.Length; i++) {
                points.Add(new Point(i, max - values[i]));
            }
            // last point (lower-right corner)
            points.Add(new Point(values.Length - 1, max));

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
            if (m_isCalculatedY) {
                m_vm.M_histogramPoints = ConvertToPointCollection(m_histogramY);
                meanValue.Text = m_histogramMeanY.ToString("F");
            } else {
                m_histogramY = Algorithms.HistogramLuminance(m_data);
                m_histogramMeanY = Algorithms.HistogramMeanValue(m_data, m_histogramY);
                m_vm.M_histogramPoints = ConvertToPointCollection(m_histogramY);
                meanValue.Text = m_histogramMeanY.ToString("F");
                m_isCalculatedY = true;
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
            if (m_isCalculatedR) {
                m_vm.M_histogramPoints = ConvertToPointCollection(m_histogramR);
                meanValue.Text = m_histogramMeanR.ToString("F");
            } else {
                m_histogramR = Algorithms.HistogramRed(m_data);
                m_histogramMeanR = Algorithms.HistogramMeanValue(m_data, m_histogramR);
                m_vm.M_histogramPoints = ConvertToPointCollection(m_histogramR);
                meanValue.Text = m_histogramMeanR.ToString("F");
                m_isCalculatedR = true;
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
            if (m_isCalculatedG) {
                m_vm.M_histogramPoints = ConvertToPointCollection(m_histogramG);
                meanValue.Text = m_histogramMeanG.ToString("F");
            } else {
                m_histogramG = Algorithms.HistogramGreen(m_data);
                m_histogramMeanG = Algorithms.HistogramMeanValue(m_data, m_histogramG);
                m_vm.M_histogramPoints = ConvertToPointCollection(m_histogramG);
                meanValue.Text = m_histogramMeanG.ToString("F");
                m_isCalculatedG = true;
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
            if (m_isCalculatedB) {
                m_vm.M_histogramPoints = ConvertToPointCollection(m_histogramB);
                meanValue.Text = m_histogramMeanB.ToString("F");
            } else {
                m_histogramB = Algorithms.HistogramBlue(m_data);
                m_histogramMeanB = Algorithms.HistogramMeanValue(m_data, m_histogramB);
                m_vm.M_histogramPoints = ConvertToPointCollection(m_histogramB);
                meanValue.Text = m_histogramMeanB.ToString("F");
                m_isCalculatedB = true;
            }
        }
        #endregion
    }
}
