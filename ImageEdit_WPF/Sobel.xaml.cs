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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageEdit_WPF
{
    /// <summary>
    /// Interaction logic for Sobel.xaml
    /// </summary>
    public partial class Sobel : Window
    {
        private String filename;
        private Bitmap bmpOutput = null;
        private Bitmap bmpUndoRedo = null;
        private Int32 SizeMask = 0;

        public Sobel(String fname, Bitmap bmpO, Bitmap bmpUR)
        {
            InitializeComponent();

            filename = fname;
            bmpOutput = bmpO;
            bmpUndoRedo = bmpUR;

            three.IsChecked = true;
        }

        private void three_Checked(object sender, RoutedEventArgs e)
        {
            SizeMask = 3;

            this.Height = 250;
            this.Width = 260;

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

        private void five_Checked(object sender, RoutedEventArgs e)
        {
            SizeMask = 5;

            this.Height = 290;
            this.Width = 380;

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

        private void seven_Checked(object sender, RoutedEventArgs e)
        {
            SizeMask = 7;

            this.Height = 330;
            this.Width = 500;

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

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            Int32 i = 0;
            Int32 j = 0;
            Int32 k = 0;
            Int32 l = 0;
            Double tR = 0.0;
            Double tG = 0.0;
            Double tB = 0.0;
            Int32 txR = 0;
            Int32 txG = 0;
            Int32 txB = 0;
            Int32 tyR = 0;
            Int32 tyG = 0;
            Int32 tyB = 0;
            Int32[,] Mask3X;
            Int32[,] Mask3Y;
            Int32[,] Mask5X;
            Int32[,] Mask5Y;
            Int32[,] Mask7X;
            Int32[,] Mask7Y;

            // Lock the bitmap's bits.  
            BitmapData bmpData = bmpOutput.LockBits(new System.Drawing.Rectangle(0, 0, bmpOutput.Width, bmpOutput.Height), ImageLockMode.ReadWrite, bmpOutput.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            Int32 bytes = Math.Abs(bmpData.Stride) * bmpOutput.Height;
            Byte[] rgbValues = new Byte[bytes];
            Byte[] bgrValues = new Byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            if (SizeMask == 3)
            {
                Mask3X = new Int32[3, 3] {
                                            { Int32.Parse(tbx1.Text), Int32.Parse(tbx2.Text), Int32.Parse(tbx3.Text) },
                                            { Int32.Parse(tbx8.Text), Int32.Parse(tbx9.Text), Int32.Parse(tbx10.Text) },
                                            { Int32.Parse(tbx15.Text), Int32.Parse(tbx16.Text), Int32.Parse(tbx17.Text) }
                                         };
                Mask3Y = new Int32[3, 3] {
                                            { Int32.Parse(tby1.Text), Int32.Parse(tby2.Text), Int32.Parse(tby3.Text) },
                                            { Int32.Parse(tby8.Text), Int32.Parse(tby9.Text), Int32.Parse(tby10.Text) },
                                            { Int32.Parse(tby15.Text), Int32.Parse(tby16.Text), Int32.Parse(tby17.Text) }
                                         };

                for (i = 1; i < bmpOutput.Width - 1; i++)
                {
                    for (j = 1; j < bmpOutput.Height - 1; j++)
                    {
                        int index;

                        txR = 0;
                        txG = 0;
                        txB = 0;
                        for (k = 0; k < SizeMask; k++)
                        {
                            for (l = 0; l < SizeMask; l++)
                            {
                                index = ((j + l - 1) * bmpData.Stride) + ((i + k - 1) * 3);
                                txR = txR + rgbValues[index + 2] * Mask3X[k, l];
                                txG = txG + rgbValues[index + 1] * Mask3X[k, l];
                                txB = txB + rgbValues[index] * Mask3X[k, l];
                            }
                        }

                        tyR = 0;
                        tyG = 0;
                        tyB = 0;
                        for (k = 0; k < SizeMask; k++)
                        {
                            for (l = 0; l < SizeMask; l++)
                            {
                                index = ((j + l - 1) * bmpData.Stride) + ((i + k - 1) * 3);
                                tyR = tyR + rgbValues[index + 2] * Mask3Y[k, l];
                                tyG = tyG + rgbValues[index + 1] * Mask3Y[k, l];
                                tyB = tyB + rgbValues[index] * Mask3Y[k, l];
                            }
                        }

                        tR = Math.Sqrt(txR * txR + tyR * tyR);
                        tG = Math.Sqrt(txG * txG + tyG * tyG);
                        tB = Math.Sqrt(txB * txB + tyB * tyB);

                        if (tR > 255.0)
                        {
                            tR = 255.0;
                        }
                        else if (tR < 0.0)
                        {
                            tR = 0.0;
                        }

                        if (tG > 255.0)
                        {
                            tG = 255.0;
                        }
                        else if (tG < 0.0)
                        {
                            tG = 0.0;
                        }

                        if (tB > 255.0)
                        {
                            tB = 255.0;
                        }
                        else if (tB < 0.0)
                        {
                            tB = 0.0;
                        }

                        index = (j * bmpData.Stride) + (i * 3);

                        bgrValues[index + 2] = (Byte)tR;
                        bgrValues[index + 1] = (Byte)tG;
                        bgrValues[index] = (Byte)tB;
                    }
                }
            }
            else if (SizeMask == 5)
            {
                Mask5X = new Int32[5, 5] {
                                            { Int32.Parse(tbx1.Text), Int32.Parse(tbx2.Text), Int32.Parse(tbx3.Text), Int32.Parse(tbx4.Text), Int32.Parse(tbx5.Text) },
                                            { Int32.Parse(tbx8.Text), Int32.Parse(tbx9.Text), Int32.Parse(tbx10.Text), Int32.Parse(tbx11.Text), Int32.Parse(tbx12.Text) },
                                            { Int32.Parse(tbx15.Text), Int32.Parse(tbx16.Text), Int32.Parse(tbx17.Text), Int32.Parse(tbx18.Text), Int32.Parse(tbx19.Text) },
                                            { Int32.Parse(tbx22.Text), Int32.Parse(tbx23.Text), Int32.Parse(tbx24.Text), Int32.Parse(tbx25.Text), Int32.Parse(tbx26.Text) },
                                            { Int32.Parse(tbx29.Text), Int32.Parse(tbx30.Text), Int32.Parse(tbx31.Text), Int32.Parse(tbx32.Text), Int32.Parse(tbx33.Text) }
                                         };
                Mask5Y = new Int32[5, 5] {
                                            { Int32.Parse(tby1.Text), Int32.Parse(tby2.Text), Int32.Parse(tby3.Text), Int32.Parse(tby4.Text), Int32.Parse(tby5.Text) },
                                            { Int32.Parse(tby8.Text), Int32.Parse(tby9.Text), Int32.Parse(tby10.Text), Int32.Parse(tby11.Text), Int32.Parse(tby12.Text) },
                                            { Int32.Parse(tby15.Text), Int32.Parse(tby16.Text), Int32.Parse(tby17.Text), Int32.Parse(tby18.Text), Int32.Parse(tby19.Text) },
                                            { Int32.Parse(tby22.Text), Int32.Parse(tby23.Text), Int32.Parse(tby24.Text), Int32.Parse(tby25.Text), Int32.Parse(tby26.Text) },
                                            { Int32.Parse(tby29.Text), Int32.Parse(tby30.Text), Int32.Parse(tby31.Text), Int32.Parse(tby32.Text), Int32.Parse(tby33.Text) }
                                         };

                for (i = 2; i < bmpOutput.Width - 2; i++)
                {
                    for (j = 2; j < bmpOutput.Height - 2; j++)
                    {
                        int index;

                        txR = 0;
                        txG = 0;
                        txB = 0;
                        for (k = 0; k < SizeMask; k++)
                        {
                            for (l = 0; l < SizeMask; l++)
                            {
                                index = ((j + l - 2) * bmpData.Stride) + ((i + k - 2) * 3);
                                txR = txR + rgbValues[index + 2] * Mask5X[k, l];
                                txG = txG + rgbValues[index + 1] * Mask5X[k, l];
                                txB = txB + rgbValues[index] * Mask5X[k, l];
                            }
                        }

                        tyR = 0;
                        tyG = 0;
                        tyB = 0;
                        for (k = 0; k < SizeMask; k++)
                        {
                            for (l = 0; l < SizeMask; l++)
                            {
                                index = ((j + l - 2) * bmpData.Stride) + ((i + k - 2) * 3);
                                tyR = tyR + rgbValues[index + 2] * Mask5Y[k, l];
                                tyG = tyG + rgbValues[index + 1] * Mask5Y[k, l];
                                tyB = tyB + rgbValues[index] * Mask5Y[k, l];
                            }
                        }

                        tR = Math.Sqrt(txR * txR + tyR * tyR);
                        tG = Math.Sqrt(txG * txG + tyG * tyG);
                        tB = Math.Sqrt(txB * txB + tyB * tyB);

                        if (tR > 255.0)
                        {
                            tR = 255.0;
                        }
                        else if (tR < 0.0)
                        {
                            tR = 0.0;
                        }

                        if (tG > 255.0)
                        {
                            tG = 255.0;
                        }
                        else if (tG < 0.0)
                        {
                            tG = 0.0;
                        }

                        if (tB > 255.0)
                        {
                            tB = 255.0;
                        }
                        else if (tB < 0.0)
                        {
                            tB = 0.0;
                        }

                        index = (j * bmpData.Stride) + (i * 3);

                        bgrValues[index + 2] = (Byte)tR;
                        bgrValues[index + 1] = (Byte)tG;
                        bgrValues[index] = (Byte)tB;
                    }
                }
            }
            else if (SizeMask == 7)
            {
                Mask7X = new Int32[7, 7] {
                                            { Int32.Parse(tbx1.Text), Int32.Parse(tbx2.Text), Int32.Parse(tbx3.Text), Int32.Parse(tbx4.Text), Int32.Parse(tbx5.Text), Int32.Parse(tbx6.Text), Int32.Parse(tbx7.Text) },
                                            { Int32.Parse(tbx8.Text), Int32.Parse(tbx9.Text), Int32.Parse(tbx10.Text), Int32.Parse(tbx11.Text), Int32.Parse(tbx12.Text), Int32.Parse(tbx13.Text), Int32.Parse(tbx14.Text) },
                                            { Int32.Parse(tbx15.Text), Int32.Parse(tbx16.Text), Int32.Parse(tbx17.Text), Int32.Parse(tbx18.Text), Int32.Parse(tbx19.Text), Int32.Parse(tbx20.Text), Int32.Parse(tbx21.Text) },
                                            { Int32.Parse(tbx22.Text), Int32.Parse(tbx23.Text), Int32.Parse(tbx24.Text), Int32.Parse(tbx25.Text), Int32.Parse(tbx26.Text), Int32.Parse(tbx27.Text), Int32.Parse(tbx28.Text) },
                                            { Int32.Parse(tbx29.Text), Int32.Parse(tbx30.Text), Int32.Parse(tbx31.Text), Int32.Parse(tbx32.Text), Int32.Parse(tbx33.Text), Int32.Parse(tbx34.Text), Int32.Parse(tbx35.Text) },
                                            { Int32.Parse(tbx36.Text), Int32.Parse(tbx37.Text), Int32.Parse(tbx38.Text), Int32.Parse(tbx39.Text), Int32.Parse(tbx40.Text), Int32.Parse(tbx41.Text), Int32.Parse(tbx42.Text) },
                                            { Int32.Parse(tbx43.Text), Int32.Parse(tbx44.Text), Int32.Parse(tbx45.Text), Int32.Parse(tbx46.Text), Int32.Parse(tbx47.Text), Int32.Parse(tbx48.Text), Int32.Parse(tbx49.Text) }
                                         };
                Mask7Y = new Int32[7, 7] {
                                            { Int32.Parse(tby1.Text), Int32.Parse(tby2.Text), Int32.Parse(tby3.Text), Int32.Parse(tby4.Text), Int32.Parse(tby5.Text), Int32.Parse(tby6.Text), Int32.Parse(tby7.Text) },
                                            { Int32.Parse(tby8.Text), Int32.Parse(tby9.Text), Int32.Parse(tby10.Text), Int32.Parse(tby11.Text), Int32.Parse(tby12.Text), Int32.Parse(tby13.Text), Int32.Parse(tby14.Text) },
                                            { Int32.Parse(tby15.Text), Int32.Parse(tby16.Text), Int32.Parse(tby17.Text), Int32.Parse(tby18.Text), Int32.Parse(tby19.Text), Int32.Parse(tby20.Text), Int32.Parse(tby21.Text) },
                                            { Int32.Parse(tby22.Text), Int32.Parse(tby23.Text), Int32.Parse(tby24.Text), Int32.Parse(tby25.Text), Int32.Parse(tby26.Text), Int32.Parse(tby27.Text), Int32.Parse(tby28.Text) },
                                            { Int32.Parse(tby29.Text), Int32.Parse(tby30.Text), Int32.Parse(tby31.Text), Int32.Parse(tby32.Text), Int32.Parse(tby33.Text), Int32.Parse(tby34.Text), Int32.Parse(tby35.Text) },
                                            { Int32.Parse(tby36.Text), Int32.Parse(tby37.Text), Int32.Parse(tby38.Text), Int32.Parse(tby39.Text), Int32.Parse(tby40.Text), Int32.Parse(tby41.Text), Int32.Parse(tby42.Text) },
                                            { Int32.Parse(tby43.Text), Int32.Parse(tby44.Text), Int32.Parse(tby45.Text), Int32.Parse(tby46.Text), Int32.Parse(tby47.Text), Int32.Parse(tby48.Text), Int32.Parse(tby49.Text) }
                                         };

                for (i = 3; i < bmpOutput.Width - 3; i++)
                {
                    for (j = 3; j < bmpOutput.Height - 3; j++)
                    {
                        int index;

                        txR = 0;
                        txG = 0;
                        txB = 0;
                        for (k = 0; k < SizeMask; k++)
                        {
                            for (l = 0; l < SizeMask; l++)
                            {
                                index = ((j + l - 3) * bmpData.Stride) + ((i + k - 3) * 3);
                                txR = txR + rgbValues[index + 2] * Mask7X[k, l];
                                txG = txG + rgbValues[index + 1] * Mask7X[k, l];
                                txB = txB + rgbValues[index] * Mask7X[k, l];
                            }
                        }

                        tyR = 0;
                        tyG = 0;
                        tyB = 0;
                        for (k = 0; k < SizeMask; k++)
                        {
                            for (l = 0; l < SizeMask; l++)
                            {
                                index = ((j + l - 3) * bmpData.Stride) + ((i + k - 3) * 3);
                                tyR = tyR + rgbValues[index + 2] * Mask7Y[k, l];
                                tyG = tyG + rgbValues[index + 1] * Mask7Y[k, l];
                                tyB = tyB + rgbValues[index] * Mask7Y[k, l];
                            }
                        }

                        tR = Math.Sqrt(txR * txR + tyR * tyR);
                        tG = Math.Sqrt(txG * txG + tyG * tyG);
                        tB = Math.Sqrt(txB * txB + tyB * tyB);

                        if (tR > 255.0)
                        {
                            tR = 255.0;
                        }
                        else if (tR < 0.0)
                        {
                            tR = 0.0;
                        }

                        if (tG > 255.0)
                        {
                            tG = 255.0;
                        }
                        else if (tG < 0.0)
                        {
                            tG = 0.0;
                        }

                        if (tB > 255.0)
                        {
                            tB = 255.0;
                        }
                        else if (tB < 0.0)
                        {
                            tB = 0.0;
                        }

                        index = (j * bmpData.Stride) + (i * 3);

                        bgrValues[index + 2] = (Byte)tR;
                        bgrValues[index + 1] = (Byte)tG;
                        bgrValues[index] = (Byte)tB;
                    }
                }
            }

            for (i = 0; i < bmpOutput.Width; i++)
            {
                for (j = 0; j < bmpOutput.Height; j++)
                {
                    int index = (j * bmpData.Stride) + (i * 3);

                    rgbValues[index + 2] = bgrValues[index + 2];
                    rgbValues[index + 1] = bgrValues[index + 1];
                    rgbValues[index] = bgrValues[index];
                }
            }

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmpOutput.UnlockBits(bmpData);

            // Convert Bitmap to BitmapImage
            BitmapToBitmapImage();

            String messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
            MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                MainWindow.noChange = false;
                MainWindow.Action = ActionType.ImageConvolution;
                bmpUndoRedo = bmpOutput.Clone() as System.Drawing.Bitmap;
                MainWindow.undoStack.Push(bmpUndoRedo);
                MainWindow.redoStack.Clear();
                foreach (Window mainWindow in Application.Current.Windows)
                {
                    if (mainWindow.GetType() == typeof(MainWindow))
                    {
                        (mainWindow as MainWindow).undo.IsEnabled = true;
                        (mainWindow as MainWindow).redo.IsEnabled = false;
                    }
                }
                this.Close();
            }
        }

        public void BitmapToBitmapImage()
        {
            MemoryStream str = new MemoryStream();
            bmpOutput.Save(str, ImageFormat.Bmp);
            str.Seek(0, SeekOrigin.Begin);
            BmpBitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

            foreach (Window mainWindow in Application.Current.Windows)
            {
                if (mainWindow.GetType() == typeof(MainWindow))
                {
                    (mainWindow as MainWindow).mainImage.Source = bdc.Frames[0];
                }
            }
        }
    }
}
