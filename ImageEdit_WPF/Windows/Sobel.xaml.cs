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

using ImageEdit_WPF.HelperClasses;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;

namespace ImageEdit_WPF.Windows {
    /// <summary>
    /// Interaction logic for Sobel.xaml
    /// </summary>
    public partial class Sobel : Window {
        private ImageData m_data = null;

        /// <summary>
        /// Size of the kernels.
        /// </summary>
        private int m_sizeMask = 0;

        /// <summary>
        /// Sobel <c>constructor</c>.
        /// Here we initialiaze the images and also we set the focus
        /// at the 'OK' button and at one of the three radio boxes (kernel size).
        /// </summary>
        public Sobel(ImageData data) {
            m_data = data;

            InitializeComponent();
            three.IsChecked = true;
        }

        /// <summary>
        /// If kernel's size is 3x3, the following attributes are set.
        /// <list type="bullet">
        ///     <item>
        ///     <description>
        ///         Kernel size.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Height of the window.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Width of the window.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box 1 width.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box 1 height.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box 2 width.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box 2 height.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         OK button attributes.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Text Boxes visibility and values.
        ///     </description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// As for the textBoxes that hold the values of the kernel,
        /// some of them are not visible because is not needed to.
        /// They exist only for bigger kernels.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void three_Checked(object sender, RoutedEventArgs e) {
            m_sizeMask = 3;

            Height = 250;
            Width = 260;

            groupBox.Width = 110;
            groupBox.Height = 90;
            groupBox2.Width = 110;
            groupBox2.Height = 90;

            ok.Margin = new Thickness(90, 10, 90, 10);

            // X Axis Mask textboxes
            tbx1.Text = "-1";
            tbx2.Text = "0";
            tbx3.Text = "1";
            tbx8.Text = "-2";
            tbx9.Text = "0";
            tbx10.Text = "2";
            tbx15.Text = "-1";
            tbx16.Text = "0";
            tbx17.Text = "1";

            tbx1.Visibility = Visibility.Visible;
            tbx2.Visibility = Visibility.Visible;
            tbx3.Visibility = Visibility.Visible;
            tbx4.Visibility = Visibility.Collapsed;
            tbx5.Visibility = Visibility.Collapsed;
            tbx6.Visibility = Visibility.Collapsed;
            tbx7.Visibility = Visibility.Collapsed;
            tbx8.Visibility = Visibility.Visible;
            tbx9.Visibility = Visibility.Visible;
            tbx10.Visibility = Visibility.Visible;
            tbx11.Visibility = Visibility.Collapsed;
            tbx12.Visibility = Visibility.Collapsed;
            tbx13.Visibility = Visibility.Collapsed;
            tbx14.Visibility = Visibility.Collapsed;
            tbx15.Visibility = Visibility.Visible;
            tbx16.Visibility = Visibility.Visible;
            tbx17.Visibility = Visibility.Visible;
            tbx18.Visibility = Visibility.Collapsed;
            tbx19.Visibility = Visibility.Collapsed;
            tbx20.Visibility = Visibility.Collapsed;
            tbx21.Visibility = Visibility.Collapsed;
            tbx22.Visibility = Visibility.Collapsed;
            tbx23.Visibility = Visibility.Collapsed;
            tbx24.Visibility = Visibility.Collapsed;
            tbx25.Visibility = Visibility.Collapsed;
            tbx26.Visibility = Visibility.Collapsed;
            tbx27.Visibility = Visibility.Collapsed;
            tbx28.Visibility = Visibility.Collapsed;
            tbx29.Visibility = Visibility.Collapsed;
            tbx30.Visibility = Visibility.Collapsed;
            tbx31.Visibility = Visibility.Collapsed;
            tbx32.Visibility = Visibility.Collapsed;
            tbx33.Visibility = Visibility.Collapsed;
            tbx34.Visibility = Visibility.Collapsed;
            tbx35.Visibility = Visibility.Collapsed;
            tbx36.Visibility = Visibility.Collapsed;
            tbx37.Visibility = Visibility.Collapsed;
            tbx38.Visibility = Visibility.Collapsed;
            tbx39.Visibility = Visibility.Collapsed;
            tbx40.Visibility = Visibility.Collapsed;
            tbx41.Visibility = Visibility.Collapsed;
            tbx42.Visibility = Visibility.Collapsed;
            tbx43.Visibility = Visibility.Collapsed;
            tbx44.Visibility = Visibility.Collapsed;
            tbx45.Visibility = Visibility.Collapsed;
            tbx46.Visibility = Visibility.Collapsed;
            tbx47.Visibility = Visibility.Collapsed;
            tbx48.Visibility = Visibility.Collapsed;
            tbx49.Visibility = Visibility.Collapsed;

            // Y Axis Mask textboxes
            tby1.Text = "1";
            tby2.Text = "2";
            tby3.Text = "1";
            tby8.Text = "0";
            tby9.Text = "0";
            tby10.Text = "0";
            tby15.Text = "-1";
            tby16.Text = "-2";
            tby17.Text = "-1";

            tby1.Visibility = Visibility.Visible;
            tby2.Visibility = Visibility.Visible;
            tby3.Visibility = Visibility.Visible;
            tby4.Visibility = Visibility.Collapsed;
            tby5.Visibility = Visibility.Collapsed;
            tby6.Visibility = Visibility.Collapsed;
            tby7.Visibility = Visibility.Collapsed;
            tby8.Visibility = Visibility.Visible;
            tby9.Visibility = Visibility.Visible;
            tby10.Visibility = Visibility.Visible;
            tby11.Visibility = Visibility.Collapsed;
            tby12.Visibility = Visibility.Collapsed;
            tby13.Visibility = Visibility.Collapsed;
            tby14.Visibility = Visibility.Collapsed;
            tby15.Visibility = Visibility.Visible;
            tby16.Visibility = Visibility.Visible;
            tby17.Visibility = Visibility.Visible;
            tby18.Visibility = Visibility.Collapsed;
            tby19.Visibility = Visibility.Collapsed;
            tby20.Visibility = Visibility.Collapsed;
            tby21.Visibility = Visibility.Collapsed;
            tby22.Visibility = Visibility.Collapsed;
            tby23.Visibility = Visibility.Collapsed;
            tby24.Visibility = Visibility.Collapsed;
            tby25.Visibility = Visibility.Collapsed;
            tby26.Visibility = Visibility.Collapsed;
            tby27.Visibility = Visibility.Collapsed;
            tby28.Visibility = Visibility.Collapsed;
            tby29.Visibility = Visibility.Collapsed;
            tby30.Visibility = Visibility.Collapsed;
            tby31.Visibility = Visibility.Collapsed;
            tby32.Visibility = Visibility.Collapsed;
            tby33.Visibility = Visibility.Collapsed;
            tby34.Visibility = Visibility.Collapsed;
            tby35.Visibility = Visibility.Collapsed;
            tby36.Visibility = Visibility.Collapsed;
            tby37.Visibility = Visibility.Collapsed;
            tby38.Visibility = Visibility.Collapsed;
            tby39.Visibility = Visibility.Collapsed;
            tby40.Visibility = Visibility.Collapsed;
            tby41.Visibility = Visibility.Collapsed;
            tby42.Visibility = Visibility.Collapsed;
            tby43.Visibility = Visibility.Collapsed;
            tby44.Visibility = Visibility.Collapsed;
            tby45.Visibility = Visibility.Collapsed;
            tby46.Visibility = Visibility.Collapsed;
            tby47.Visibility = Visibility.Collapsed;
            tby48.Visibility = Visibility.Collapsed;
            tby49.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If kernel's size is 5x5, the following attributes are set.
        /// <list type="bullet">
        ///     <item>
        ///     <description>
        ///         Kernel size.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Height of the window.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Width of the window.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box 1 width.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box 1 height.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box 2 width.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box 2 height.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         OK button attributes.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Text Boxes visibility and values.
        ///     </description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// As for the textBoxes that hold the values of the kernel,
        /// some of them are not visible because is not needed to.
        /// They exist only for bigger kernels.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void five_Checked(object sender, RoutedEventArgs e) {
            m_sizeMask = 5;

            Height = 290;
            Width = 380;

            groupBox.Width = 170;
            groupBox.Height = 130;
            groupBox2.Width = 170;
            groupBox2.Height = 130;

            ok.Margin = new Thickness(150, 10, 150, 10);

            // X Axis Mask textboxes
            tbx1.Text = "-1";
            tbx2.Text = "-2";
            tbx3.Text = "0";
            tbx4.Text = "2";
            tbx5.Text = "1";
            tbx8.Text = "-4";
            tbx9.Text = "-10";
            tbx10.Text = "0";
            tbx11.Text = "10";
            tbx12.Text = "4";
            tbx15.Text = "-7";
            tbx16.Text = "-17";
            tbx17.Text = "0";
            tbx18.Text = "17";
            tbx19.Text = "7";
            tbx22.Text = "-4";
            tbx23.Text = "-10";
            tbx24.Text = "0";
            tbx25.Text = "10";
            tbx26.Text = "4";
            tbx29.Text = "-1";
            tbx30.Text = "-2";
            tbx31.Text = "0";
            tbx32.Text = "2";
            tbx33.Text = "1";

            tbx1.Visibility = Visibility.Visible;
            tbx2.Visibility = Visibility.Visible;
            tbx3.Visibility = Visibility.Visible;
            tbx4.Visibility = Visibility.Visible;
            tbx5.Visibility = Visibility.Visible;
            tbx6.Visibility = Visibility.Collapsed;
            tbx7.Visibility = Visibility.Collapsed;
            tbx8.Visibility = Visibility.Visible;
            tbx9.Visibility = Visibility.Visible;
            tbx10.Visibility = Visibility.Visible;
            tbx11.Visibility = Visibility.Visible;
            tbx12.Visibility = Visibility.Visible;
            tbx13.Visibility = Visibility.Collapsed;
            tbx14.Visibility = Visibility.Collapsed;
            tbx15.Visibility = Visibility.Visible;
            tbx16.Visibility = Visibility.Visible;
            tbx17.Visibility = Visibility.Visible;
            tbx18.Visibility = Visibility.Visible;
            tbx19.Visibility = Visibility.Visible;
            tbx20.Visibility = Visibility.Collapsed;
            tbx21.Visibility = Visibility.Collapsed;
            tbx22.Visibility = Visibility.Visible;
            tbx23.Visibility = Visibility.Visible;
            tbx24.Visibility = Visibility.Visible;
            tbx25.Visibility = Visibility.Visible;
            tbx26.Visibility = Visibility.Visible;
            tbx27.Visibility = Visibility.Collapsed;
            tbx28.Visibility = Visibility.Collapsed;
            tbx29.Visibility = Visibility.Visible;
            tbx30.Visibility = Visibility.Visible;
            tbx31.Visibility = Visibility.Visible;
            tbx32.Visibility = Visibility.Visible;
            tbx33.Visibility = Visibility.Visible;
            tbx34.Visibility = Visibility.Collapsed;
            tbx35.Visibility = Visibility.Collapsed;
            tbx36.Visibility = Visibility.Collapsed;
            tbx37.Visibility = Visibility.Collapsed;
            tbx38.Visibility = Visibility.Collapsed;
            tbx39.Visibility = Visibility.Collapsed;
            tbx40.Visibility = Visibility.Collapsed;
            tbx41.Visibility = Visibility.Collapsed;
            tbx42.Visibility = Visibility.Collapsed;
            tbx43.Visibility = Visibility.Collapsed;
            tbx44.Visibility = Visibility.Collapsed;
            tbx45.Visibility = Visibility.Collapsed;
            tbx46.Visibility = Visibility.Collapsed;
            tbx47.Visibility = Visibility.Collapsed;
            tbx48.Visibility = Visibility.Collapsed;
            tbx49.Visibility = Visibility.Collapsed;

            // Y Axis Mask textboxes
            tby1.Text = "1";
            tby2.Text = "4";
            tby3.Text = "7";
            tby4.Text = "4";
            tby5.Text = "1";
            tby8.Text = "2";
            tby9.Text = "10";
            tby10.Text = "17";
            tby11.Text = "10";
            tby12.Text = "2";
            tby15.Text = "0";
            tby16.Text = "0";
            tby17.Text = "0";
            tby18.Text = "0";
            tby19.Text = "0";
            tby22.Text = "-2";
            tby23.Text = "-10";
            tby24.Text = "-17";
            tby25.Text = "-10";
            tby26.Text = "-2";
            tby29.Text = "-1";
            tby30.Text = "-4";
            tby31.Text = "-7";
            tby32.Text = "-4";
            tby33.Text = "-1";

            tby1.Visibility = Visibility.Visible;
            tby2.Visibility = Visibility.Visible;
            tby3.Visibility = Visibility.Visible;
            tby4.Visibility = Visibility.Visible;
            tby5.Visibility = Visibility.Visible;
            tby6.Visibility = Visibility.Collapsed;
            tby7.Visibility = Visibility.Collapsed;
            tby8.Visibility = Visibility.Visible;
            tby9.Visibility = Visibility.Visible;
            tby10.Visibility = Visibility.Visible;
            tby11.Visibility = Visibility.Visible;
            tby12.Visibility = Visibility.Visible;
            tby13.Visibility = Visibility.Collapsed;
            tby14.Visibility = Visibility.Collapsed;
            tby15.Visibility = Visibility.Visible;
            tby16.Visibility = Visibility.Visible;
            tby17.Visibility = Visibility.Visible;
            tby18.Visibility = Visibility.Visible;
            tby19.Visibility = Visibility.Visible;
            tby20.Visibility = Visibility.Collapsed;
            tby21.Visibility = Visibility.Collapsed;
            tby22.Visibility = Visibility.Visible;
            tby23.Visibility = Visibility.Visible;
            tby24.Visibility = Visibility.Visible;
            tby25.Visibility = Visibility.Visible;
            tby26.Visibility = Visibility.Visible;
            tby27.Visibility = Visibility.Collapsed;
            tby28.Visibility = Visibility.Collapsed;
            tby29.Visibility = Visibility.Visible;
            tby30.Visibility = Visibility.Visible;
            tby31.Visibility = Visibility.Visible;
            tby32.Visibility = Visibility.Visible;
            tby33.Visibility = Visibility.Visible;
            tby34.Visibility = Visibility.Collapsed;
            tby35.Visibility = Visibility.Collapsed;
            tby36.Visibility = Visibility.Collapsed;
            tby37.Visibility = Visibility.Collapsed;
            tby38.Visibility = Visibility.Collapsed;
            tby39.Visibility = Visibility.Collapsed;
            tby40.Visibility = Visibility.Collapsed;
            tby41.Visibility = Visibility.Collapsed;
            tby42.Visibility = Visibility.Collapsed;
            tby43.Visibility = Visibility.Collapsed;
            tby44.Visibility = Visibility.Collapsed;
            tby45.Visibility = Visibility.Collapsed;
            tby46.Visibility = Visibility.Collapsed;
            tby47.Visibility = Visibility.Collapsed;
            tby48.Visibility = Visibility.Collapsed;
            tby49.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If kernel's size is 3x3, the following attributes are set.
        /// <list type="bullet">
        ///     <item>
        ///     <description>
        ///         Kernel size.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Height of the window.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Width of the window.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box 1 width.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box 1 height.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box 2 width.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box 2 height.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         OK button attributes.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Text Boxes visibility and values.
        ///     </description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seven_Checked(object sender, RoutedEventArgs e) {
            m_sizeMask = 7;

            Height = 330;
            Width = 500;

            groupBox.Width = 230;
            groupBox.Height = 170;
            groupBox2.Width = 230;
            groupBox2.Height = 170;

            ok.Margin = new Thickness(210, 10, 210, 10);

            // X Axis Mask textboxes
            tbx1.Text = "-1";
            tbx2.Text = "-3";
            tbx3.Text = "-3";
            tbx4.Text = "0";
            tbx5.Text = "3";
            tbx6.Text = "3";
            tbx7.Text = "1";
            tbx8.Text = "-4";
            tbx9.Text = "-11";
            tbx10.Text = "-13";
            tbx11.Text = "0";
            tbx12.Text = "13";
            tbx13.Text = "11";
            tbx14.Text = "4";
            tbx15.Text = "-9";
            tbx16.Text = "-26";
            tbx17.Text = "-30";
            tbx18.Text = "0";
            tbx19.Text = "30";
            tbx20.Text = "26";
            tbx21.Text = "9";
            tbx22.Text = "-13";
            tbx23.Text = "-34";
            tbx24.Text = "-40";
            tbx25.Text = "0";
            tbx26.Text = "40";
            tbx27.Text = "34";
            tbx28.Text = "13";
            tbx29.Text = "-9";
            tbx30.Text = "-26";
            tbx31.Text = "-30";
            tbx32.Text = "0";
            tbx33.Text = "30";
            tbx34.Text = "26";
            tbx35.Text = "9";
            tbx36.Text = "-4";
            tbx37.Text = "-11";
            tbx38.Text = "-13";
            tbx39.Text = "0";
            tbx40.Text = "13";
            tbx41.Text = "11";
            tbx42.Text = "4";
            tbx43.Text = "-1";
            tbx44.Text = "-3";
            tbx45.Text = "-3";
            tbx46.Text = "0";
            tbx47.Text = "3";
            tbx48.Text = "3";
            tbx49.Text = "1";

            tbx1.Visibility = Visibility.Visible;
            tbx2.Visibility = Visibility.Visible;
            tbx3.Visibility = Visibility.Visible;
            tbx4.Visibility = Visibility.Visible;
            tbx5.Visibility = Visibility.Visible;
            tbx6.Visibility = Visibility.Visible;
            tbx7.Visibility = Visibility.Visible;
            tbx8.Visibility = Visibility.Visible;
            tbx9.Visibility = Visibility.Visible;
            tbx10.Visibility = Visibility.Visible;
            tbx11.Visibility = Visibility.Visible;
            tbx12.Visibility = Visibility.Visible;
            tbx13.Visibility = Visibility.Visible;
            tbx14.Visibility = Visibility.Visible;
            tbx15.Visibility = Visibility.Visible;
            tbx16.Visibility = Visibility.Visible;
            tbx17.Visibility = Visibility.Visible;
            tbx18.Visibility = Visibility.Visible;
            tbx19.Visibility = Visibility.Visible;
            tbx20.Visibility = Visibility.Visible;
            tbx21.Visibility = Visibility.Visible;
            tbx22.Visibility = Visibility.Visible;
            tbx23.Visibility = Visibility.Visible;
            tbx24.Visibility = Visibility.Visible;
            tbx25.Visibility = Visibility.Visible;
            tbx26.Visibility = Visibility.Visible;
            tbx27.Visibility = Visibility.Visible;
            tbx28.Visibility = Visibility.Visible;
            tbx29.Visibility = Visibility.Visible;
            tbx30.Visibility = Visibility.Visible;
            tbx31.Visibility = Visibility.Visible;
            tbx32.Visibility = Visibility.Visible;
            tbx33.Visibility = Visibility.Visible;
            tbx34.Visibility = Visibility.Visible;
            tbx35.Visibility = Visibility.Visible;
            tbx36.Visibility = Visibility.Visible;
            tbx37.Visibility = Visibility.Visible;
            tbx38.Visibility = Visibility.Visible;
            tbx39.Visibility = Visibility.Visible;
            tbx40.Visibility = Visibility.Visible;
            tbx41.Visibility = Visibility.Visible;
            tbx42.Visibility = Visibility.Visible;
            tbx43.Visibility = Visibility.Visible;
            tbx44.Visibility = Visibility.Visible;
            tbx45.Visibility = Visibility.Visible;
            tbx46.Visibility = Visibility.Visible;
            tbx47.Visibility = Visibility.Visible;
            tbx48.Visibility = Visibility.Visible;
            tbx49.Visibility = Visibility.Visible;

            // Y Axis Mask textboxes
            tby1.Text = "1";
            tby2.Text = "4";
            tby3.Text = "9";
            tby4.Text = "13";
            tby5.Text = "9";
            tby6.Text = "4";
            tby7.Text = "1";
            tby8.Text = "3";
            tby9.Text = "11";
            tby10.Text = "26";
            tby11.Text = "34";
            tby12.Text = "26";
            tby13.Text = "11";
            tby14.Text = "3";
            tby15.Text = "3";
            tby16.Text = "13";
            tby17.Text = "30";
            tby18.Text = "40";
            tby19.Text = "30";
            tby20.Text = "13";
            tby21.Text = "3";
            tby22.Text = "0";
            tby23.Text = "0";
            tby24.Text = "0";
            tby25.Text = "0";
            tby26.Text = "0";
            tby27.Text = "0";
            tby28.Text = "0";
            tby29.Text = "-3";
            tby30.Text = "-13";
            tby31.Text = "-30";
            tby32.Text = "-40";
            tby33.Text = "-30";
            tby34.Text = "-13";
            tby35.Text = "-3";
            tby36.Text = "-3";
            tby37.Text = "-11";
            tby38.Text = "-26";
            tby39.Text = "-34";
            tby40.Text = "-26";
            tby41.Text = "-11";
            tby42.Text = "-3";
            tby43.Text = "-1";
            tby44.Text = "-4";
            tby45.Text = "-9";
            tby46.Text = "-13";
            tby47.Text = "-9";
            tby48.Text = "-4";
            tby49.Text = "-1";

            tby1.Visibility = Visibility.Visible;
            tby2.Visibility = Visibility.Visible;
            tby3.Visibility = Visibility.Visible;
            tby4.Visibility = Visibility.Visible;
            tby5.Visibility = Visibility.Visible;
            tby6.Visibility = Visibility.Visible;
            tby7.Visibility = Visibility.Visible;
            tby8.Visibility = Visibility.Visible;
            tby9.Visibility = Visibility.Visible;
            tby10.Visibility = Visibility.Visible;
            tby11.Visibility = Visibility.Visible;
            tby12.Visibility = Visibility.Visible;
            tby13.Visibility = Visibility.Visible;
            tby14.Visibility = Visibility.Visible;
            tby15.Visibility = Visibility.Visible;
            tby16.Visibility = Visibility.Visible;
            tby17.Visibility = Visibility.Visible;
            tby18.Visibility = Visibility.Visible;
            tby19.Visibility = Visibility.Visible;
            tby20.Visibility = Visibility.Visible;
            tby21.Visibility = Visibility.Visible;
            tby22.Visibility = Visibility.Visible;
            tby23.Visibility = Visibility.Visible;
            tby24.Visibility = Visibility.Visible;
            tby25.Visibility = Visibility.Visible;
            tby26.Visibility = Visibility.Visible;
            tby27.Visibility = Visibility.Visible;
            tby28.Visibility = Visibility.Visible;
            tby29.Visibility = Visibility.Visible;
            tby30.Visibility = Visibility.Visible;
            tby31.Visibility = Visibility.Visible;
            tby32.Visibility = Visibility.Visible;
            tby33.Visibility = Visibility.Visible;
            tby34.Visibility = Visibility.Visible;
            tby35.Visibility = Visibility.Visible;
            tby36.Visibility = Visibility.Visible;
            tby37.Visibility = Visibility.Visible;
            tby38.Visibility = Visibility.Visible;
            tby39.Visibility = Visibility.Visible;
            tby40.Visibility = Visibility.Visible;
            tby41.Visibility = Visibility.Visible;
            tby42.Visibility = Visibility.Visible;
            tby43.Visibility = Visibility.Visible;
            tby44.Visibility = Visibility.Visible;
            tby45.Visibility = Visibility.Visible;
            tby46.Visibility = Visibility.Visible;
            tby47.Visibility = Visibility.Visible;
            tby48.Visibility = Visibility.Visible;
            tby49.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Implementation of the Sobel (Edge detector) algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ok_Click(object sender, RoutedEventArgs e) {
            int[,] maskX = null;
            int[,] maskY = null;

            switch(m_sizeMask)
            {
                case 3:
                    maskX = new int[3, 3] {
                        {int.Parse(tbx1.Text), int.Parse(tbx2.Text), int.Parse(tbx3.Text)},
                        {int.Parse(tbx8.Text), int.Parse(tbx9.Text), int.Parse(tbx10.Text)},
                        {int.Parse(tbx15.Text), int.Parse(tbx16.Text), int.Parse(tbx17.Text)}
                    };
                    maskY = new int[3, 3] {
                        {int.Parse(tby1.Text), int.Parse(tby2.Text), int.Parse(tby3.Text)},
                        {int.Parse(tby8.Text), int.Parse(tby9.Text), int.Parse(tby10.Text)},
                        {int.Parse(tby15.Text), int.Parse(tby16.Text), int.Parse(tby17.Text)}
                    };
                    break;
                case 5:
                    maskX = new int[5, 5] {
                        {int.Parse(tbx1.Text), int.Parse(tbx2.Text), int.Parse(tbx3.Text), int.Parse(tbx4.Text), int.Parse(tbx5.Text)},
                        {int.Parse(tbx8.Text), int.Parse(tbx9.Text), int.Parse(tbx10.Text), int.Parse(tbx11.Text), int.Parse(tbx12.Text)},
                        {int.Parse(tbx15.Text), int.Parse(tbx16.Text), int.Parse(tbx17.Text), int.Parse(tbx18.Text), int.Parse(tbx19.Text)},
                        {int.Parse(tbx22.Text), int.Parse(tbx23.Text), int.Parse(tbx24.Text), int.Parse(tbx25.Text), int.Parse(tbx26.Text)},
                        {int.Parse(tbx29.Text), int.Parse(tbx30.Text), int.Parse(tbx31.Text), int.Parse(tbx32.Text), int.Parse(tbx33.Text)}
                    };
                    maskY = new int[5, 5] {
                        {int.Parse(tby1.Text), int.Parse(tby2.Text), int.Parse(tby3.Text), int.Parse(tby4.Text), int.Parse(tby5.Text)},
                        {int.Parse(tby8.Text), int.Parse(tby9.Text), int.Parse(tby10.Text), int.Parse(tby11.Text), int.Parse(tby12.Text)},
                        {int.Parse(tby15.Text), int.Parse(tby16.Text), int.Parse(tby17.Text), int.Parse(tby18.Text), int.Parse(tby19.Text)},
                        {int.Parse(tby22.Text), int.Parse(tby23.Text), int.Parse(tby24.Text), int.Parse(tby25.Text), int.Parse(tby26.Text)},
                        {int.Parse(tby29.Text), int.Parse(tby30.Text), int.Parse(tby31.Text), int.Parse(tby32.Text), int.Parse(tby33.Text)}
                    };
                    break;
                case 7:
                    maskX = new int[7, 7] {
                        {int.Parse(tbx1.Text), int.Parse(tbx2.Text), int.Parse(tbx3.Text), int.Parse(tbx4.Text), int.Parse(tbx5.Text), int.Parse(tbx6.Text), int.Parse(tbx7.Text)},
                        {int.Parse(tbx8.Text), int.Parse(tbx9.Text), int.Parse(tbx10.Text), int.Parse(tbx11.Text), int.Parse(tbx12.Text), int.Parse(tbx13.Text), int.Parse(tbx14.Text)},
                        {int.Parse(tbx15.Text), int.Parse(tbx16.Text), int.Parse(tbx17.Text), int.Parse(tbx18.Text), int.Parse(tbx19.Text), int.Parse(tbx20.Text), int.Parse(tbx21.Text)},
                        {int.Parse(tbx22.Text), int.Parse(tbx23.Text), int.Parse(tbx24.Text), int.Parse(tbx25.Text), int.Parse(tbx26.Text), int.Parse(tbx27.Text), int.Parse(tbx28.Text)},
                        {int.Parse(tbx29.Text), int.Parse(tbx30.Text), int.Parse(tbx31.Text), int.Parse(tbx32.Text), int.Parse(tbx33.Text), int.Parse(tbx34.Text), int.Parse(tbx35.Text)},
                        {int.Parse(tbx36.Text), int.Parse(tbx37.Text), int.Parse(tbx38.Text), int.Parse(tbx39.Text), int.Parse(tbx40.Text), int.Parse(tbx41.Text), int.Parse(tbx42.Text)},
                        {int.Parse(tbx43.Text), int.Parse(tbx44.Text), int.Parse(tbx45.Text), int.Parse(tbx46.Text), int.Parse(tbx47.Text), int.Parse(tbx48.Text), int.Parse(tbx49.Text)}
                    };
                    maskY = new int[7, 7] {
                        {int.Parse(tby1.Text), int.Parse(tby2.Text), int.Parse(tby3.Text), int.Parse(tby4.Text), int.Parse(tby5.Text), int.Parse(tby6.Text), int.Parse(tby7.Text)},
                        {int.Parse(tby8.Text), int.Parse(tby9.Text), int.Parse(tby10.Text), int.Parse(tby11.Text), int.Parse(tby12.Text), int.Parse(tby13.Text), int.Parse(tby14.Text)},
                        {int.Parse(tby15.Text), int.Parse(tby16.Text), int.Parse(tby17.Text), int.Parse(tby18.Text), int.Parse(tby19.Text), int.Parse(tby20.Text), int.Parse(tby21.Text)},
                        {int.Parse(tby22.Text), int.Parse(tby23.Text), int.Parse(tby24.Text), int.Parse(tby25.Text), int.Parse(tby26.Text), int.Parse(tby27.Text), int.Parse(tby28.Text)},
                        {int.Parse(tby29.Text), int.Parse(tby30.Text), int.Parse(tby31.Text), int.Parse(tby32.Text), int.Parse(tby33.Text), int.Parse(tby34.Text), int.Parse(tby35.Text)},
                        {int.Parse(tby36.Text), int.Parse(tby37.Text), int.Parse(tby38.Text), int.Parse(tby39.Text), int.Parse(tby40.Text), int.Parse(tby41.Text), int.Parse(tby42.Text)},
                        {int.Parse(tby43.Text), int.Parse(tby44.Text), int.Parse(tby45.Text), int.Parse(tby46.Text), int.Parse(tby47.Text), int.Parse(tby48.Text), int.Parse(tby49.Text)}
                    };
                    break;
            }

            TimeSpan elapsedTime = Algorithms.EdgeDetection_Sobel(m_data, m_sizeMask, maskX, maskY);

            m_data.M_bitmapBind = m_data.M_bitmap.BitmapToBitmapSource();

            string messageOperation = "Done!\r\n\r\nElapsed time (HH:MM:SS.MS): " + elapsedTime;
            MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK) {
                m_data.M_noChange = false;
                m_data.M_action = ActionType.ImageConvolution;
                m_data.M_bmpUndoRedo = m_data.M_bitmap.Clone() as Bitmap;
                m_data.M_undoStack.Push(m_data.M_bmpUndoRedo);
                m_data.M_redoStack.Clear();
                foreach (Window mainWindow in Application.Current.Windows) {
                    if (mainWindow.GetType() == typeof (MainWindow)) {
                        ((MainWindow)mainWindow).undo.IsEnabled = true;
                        ((MainWindow)mainWindow).redo.IsEnabled = false;
                    }
                }
                Close();
            }
        }
    }
}