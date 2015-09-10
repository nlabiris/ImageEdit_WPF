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
            Int32 k;
            Int32 l;
            Double tR;
            Double tG;
            Double tB;
            Double txR;
            Double txG;
            Double txB;
            Double tyR;
            Double tyG;
            Double tyB;
            Double[,] Mask3X;
            Double[,] Mask3Y;
            Double[,] Mask5X;
            Double[,] Mask5Y;
            Double[,] Mask7X;
            Double[,] Mask7Y;

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
                Mask3X = new Double[3, 3] {
                                        { Double.Parse(tbx1.Text, new CultureInfo("el-GR")), Double.Parse(tbx2.Text, new CultureInfo("el-GR")), Double.Parse(tbx3.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tbx8.Text, new CultureInfo("el-GR")), Double.Parse(tbx9.Text, new CultureInfo("el-GR")), Double.Parse(tbx10.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tbx15.Text, new CultureInfo("el-GR")), Double.Parse(tbx16.Text, new CultureInfo("el-GR")), Double.Parse(tbx17.Text, new CultureInfo("el-GR")) }
                                     };
                Mask3Y = new Double[3, 3] {
                                        { Double.Parse(tby1.Text, new CultureInfo("el-GR")), Double.Parse(tby2.Text, new CultureInfo("el-GR")), Double.Parse(tby3.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tby8.Text, new CultureInfo("el-GR")), Double.Parse(tby9.Text, new CultureInfo("el-GR")), Double.Parse(tby10.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tby15.Text, new CultureInfo("el-GR")), Double.Parse(tby16.Text, new CultureInfo("el-GR")), Double.Parse(tby17.Text, new CultureInfo("el-GR")) }
                                     };

                for (i = 1; i < bmpOutput.Width - 1; i++)
                {
                    for (j = 1; j < bmpOutput.Height - 1; j++)
                    {
                        int index;

                        txR = 0.0;
                        txG = 0.0;
                        txB = 0.0;
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

                        tyR = 0.0;
                        tyG = 0.0;
                        tyB = 0.0;
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
                Mask5X = new Double[5, 5] {
                                        { Double.Parse(tbx1.Text, new CultureInfo("el-GR")), Double.Parse(tbx2.Text, new CultureInfo("el-GR")), Double.Parse(tbx3.Text, new CultureInfo("el-GR")), Double.Parse(tbx4.Text, new CultureInfo("el-GR")), Double.Parse(tbx5.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tbx8.Text, new CultureInfo("el-GR")), Double.Parse(tbx9.Text, new CultureInfo("el-GR")), Double.Parse(tbx10.Text, new CultureInfo("el-GR")), Double.Parse(tbx11.Text, new CultureInfo("el-GR")), Double.Parse(tbx12.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tbx15.Text, new CultureInfo("el-GR")), Double.Parse(tbx16.Text, new CultureInfo("el-GR")), Double.Parse(tbx17.Text, new CultureInfo("el-GR")), Double.Parse(tbx18.Text, new CultureInfo("el-GR")), Double.Parse(tbx19.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tbx22.Text, new CultureInfo("el-GR")), Double.Parse(tbx23.Text, new CultureInfo("el-GR")), Double.Parse(tbx24.Text, new CultureInfo("el-GR")), Double.Parse(tbx25.Text, new CultureInfo("el-GR")), Double.Parse(tbx26.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tbx29.Text, new CultureInfo("el-GR")), Double.Parse(tbx30.Text, new CultureInfo("el-GR")), Double.Parse(tbx31.Text, new CultureInfo("el-GR")), Double.Parse(tbx32.Text, new CultureInfo("el-GR")), Double.Parse(tbx33.Text, new CultureInfo("el-GR")) }
                                     };
                Mask5Y = new Double[5, 5] {
                                        { Double.Parse(tby1.Text, new CultureInfo("el-GR")), Double.Parse(tby2.Text, new CultureInfo("el-GR")), Double.Parse(tby3.Text, new CultureInfo("el-GR")), Double.Parse(tby4.Text, new CultureInfo("el-GR")), Double.Parse(tby5.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tby6.Text, new CultureInfo("el-GR")), Double.Parse(tby7.Text, new CultureInfo("el-GR")), Double.Parse(tby8.Text, new CultureInfo("el-GR")), Double.Parse(tby9.Text, new CultureInfo("el-GR")), Double.Parse(tby10.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tby11.Text, new CultureInfo("el-GR")), Double.Parse(tby12.Text, new CultureInfo("el-GR")), Double.Parse(tby13.Text, new CultureInfo("el-GR")), Double.Parse(tby14.Text, new CultureInfo("el-GR")), Double.Parse(tby15.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tby16.Text, new CultureInfo("el-GR")), Double.Parse(tby17.Text, new CultureInfo("el-GR")), Double.Parse(tby18.Text, new CultureInfo("el-GR")), Double.Parse(tby19.Text, new CultureInfo("el-GR")), Double.Parse(tby20.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tby21.Text, new CultureInfo("el-GR")), Double.Parse(tby22.Text, new CultureInfo("el-GR")), Double.Parse(tby23.Text, new CultureInfo("el-GR")), Double.Parse(tby24.Text, new CultureInfo("el-GR")), Double.Parse(tby25.Text, new CultureInfo("el-GR")) }
                                     };

                for (i = 1; i < bmpOutput.Width - 1; i++)
                {
                    for (j = 1; j < bmpOutput.Height - 1; j++)
                    {
                        int index;

                        txR = 0.0;
                        txG = 0.0;
                        txB = 0.0;
                        for (k = 0; k < SizeMask; k++)
                        {
                            for (l = 0; l < SizeMask; l++)
                            {
                                index = ((j + l - 1) * bmpData.Stride) + ((i + k - 1) * 3);
                                txR = txR + rgbValues[index + 2] * Mask5X[k, l];
                                txG = txG + rgbValues[index + 1] * Mask5X[k, l];
                                txB = txB + rgbValues[index] * Mask5X[k, l];
                            }
                        }

                        tyR = 0.0;
                        tyG = 0.0;
                        tyB = 0.0;
                        for (k = 0; k < SizeMask; k++)
                        {
                            for (l = 0; l < SizeMask; l++)
                            {
                                index = ((j + l - 1) * bmpData.Stride) + ((i + k - 1) * 3);
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
                Mask7X = new Double[7, 7] {
                                        { Double.Parse(tbx1.Text, new CultureInfo("el-GR")), Double.Parse(tbx2.Text, new CultureInfo("el-GR")), Double.Parse(tbx3.Text, new CultureInfo("el-GR")), Double.Parse(tbx4.Text, new CultureInfo("el-GR")), Double.Parse(tbx5.Text, new CultureInfo("el-GR")), Double.Parse(tbx6.Text, new CultureInfo("el-GR")), Double.Parse(tbx7.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tbx8.Text, new CultureInfo("el-GR")), Double.Parse(tbx9.Text, new CultureInfo("el-GR")), Double.Parse(tbx10.Text, new CultureInfo("el-GR")), Double.Parse(tbx11.Text, new CultureInfo("el-GR")), Double.Parse(tbx12.Text, new CultureInfo("el-GR")), Double.Parse(tbx13.Text, new CultureInfo("el-GR")), Double.Parse(tbx14.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tbx15.Text, new CultureInfo("el-GR")), Double.Parse(tbx16.Text, new CultureInfo("el-GR")), Double.Parse(tbx17.Text, new CultureInfo("el-GR")), Double.Parse(tbx18.Text, new CultureInfo("el-GR")), Double.Parse(tbx19.Text, new CultureInfo("el-GR")), Double.Parse(tbx20.Text, new CultureInfo("el-GR")), Double.Parse(tbx21.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tbx22.Text, new CultureInfo("el-GR")), Double.Parse(tbx23.Text, new CultureInfo("el-GR")), Double.Parse(tbx24.Text, new CultureInfo("el-GR")), Double.Parse(tbx25.Text, new CultureInfo("el-GR")), Double.Parse(tbx26.Text, new CultureInfo("el-GR")), Double.Parse(tbx27.Text, new CultureInfo("el-GR")), Double.Parse(tbx28.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tbx29.Text, new CultureInfo("el-GR")), Double.Parse(tbx30.Text, new CultureInfo("el-GR")), Double.Parse(tbx31.Text, new CultureInfo("el-GR")), Double.Parse(tbx32.Text, new CultureInfo("el-GR")), Double.Parse(tbx33.Text, new CultureInfo("el-GR")), Double.Parse(tbx34.Text, new CultureInfo("el-GR")), Double.Parse(tbx35.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tbx36.Text, new CultureInfo("el-GR")), Double.Parse(tbx37.Text, new CultureInfo("el-GR")), Double.Parse(tbx38.Text, new CultureInfo("el-GR")), Double.Parse(tbx39.Text, new CultureInfo("el-GR")), Double.Parse(tbx40.Text, new CultureInfo("el-GR")), Double.Parse(tbx41.Text, new CultureInfo("el-GR")), Double.Parse(tbx42.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tbx43.Text, new CultureInfo("el-GR")), Double.Parse(tbx44.Text, new CultureInfo("el-GR")), Double.Parse(tbx45.Text, new CultureInfo("el-GR")), Double.Parse(tbx46.Text, new CultureInfo("el-GR")), Double.Parse(tbx47.Text, new CultureInfo("el-GR")), Double.Parse(tbx48.Text, new CultureInfo("el-GR")), Double.Parse(tbx49.Text, new CultureInfo("el-GR")) }
                                     };
                Mask7Y = new Double[7, 7] {
                                        { Double.Parse(tby1.Text, new CultureInfo("el-GR")), Double.Parse(tby2.Text, new CultureInfo("el-GR")), Double.Parse(tby3.Text, new CultureInfo("el-GR")), Double.Parse(tby4.Text, new CultureInfo("el-GR")), Double.Parse(tby5.Text, new CultureInfo("el-GR")), Double.Parse(tby6.Text, new CultureInfo("el-GR")), Double.Parse(tby7.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tby8.Text, new CultureInfo("el-GR")), Double.Parse(tby9.Text, new CultureInfo("el-GR")), Double.Parse(tby10.Text, new CultureInfo("el-GR")), Double.Parse(tby11.Text, new CultureInfo("el-GR")), Double.Parse(tby12.Text, new CultureInfo("el-GR")), Double.Parse(tby13.Text, new CultureInfo("el-GR")), Double.Parse(tby14.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tby15.Text, new CultureInfo("el-GR")), Double.Parse(tby16.Text, new CultureInfo("el-GR")), Double.Parse(tby17.Text, new CultureInfo("el-GR")), Double.Parse(tby18.Text, new CultureInfo("el-GR")), Double.Parse(tby19.Text, new CultureInfo("el-GR")), Double.Parse(tby20.Text, new CultureInfo("el-GR")), Double.Parse(tby21.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tby22.Text, new CultureInfo("el-GR")), Double.Parse(tby23.Text, new CultureInfo("el-GR")), Double.Parse(tby24.Text, new CultureInfo("el-GR")), Double.Parse(tby25.Text, new CultureInfo("el-GR")), Double.Parse(tby26.Text, new CultureInfo("el-GR")), Double.Parse(tby27.Text, new CultureInfo("el-GR")), Double.Parse(tby28.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tby29.Text, new CultureInfo("el-GR")), Double.Parse(tby30.Text, new CultureInfo("el-GR")), Double.Parse(tby31.Text, new CultureInfo("el-GR")), Double.Parse(tby32.Text, new CultureInfo("el-GR")), Double.Parse(tby33.Text, new CultureInfo("el-GR")), Double.Parse(tby34.Text, new CultureInfo("el-GR")), Double.Parse(tby35.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tby36.Text, new CultureInfo("el-GR")), Double.Parse(tby37.Text, new CultureInfo("el-GR")), Double.Parse(tby38.Text, new CultureInfo("el-GR")), Double.Parse(tby39.Text, new CultureInfo("el-GR")), Double.Parse(tby40.Text, new CultureInfo("el-GR")), Double.Parse(tby41.Text, new CultureInfo("el-GR")), Double.Parse(tby42.Text, new CultureInfo("el-GR")) },
                                        { Double.Parse(tby43.Text, new CultureInfo("el-GR")), Double.Parse(tby44.Text, new CultureInfo("el-GR")), Double.Parse(tby45.Text, new CultureInfo("el-GR")), Double.Parse(tby46.Text, new CultureInfo("el-GR")), Double.Parse(tby47.Text, new CultureInfo("el-GR")), Double.Parse(tby48.Text, new CultureInfo("el-GR")), Double.Parse(tby49.Text, new CultureInfo("el-GR")) }
                                     };

                for (i = 1; i < bmpOutput.Width - 1; i++)
                {
                    for (j = 1; j < bmpOutput.Height - 1; j++)
                    {
                        int index;

                        txR = 0.0;
                        txG = 0.0;
                        txB = 0.0;
                        for (k = 0; k < SizeMask; k++)
                        {
                            for (l = 0; l < SizeMask; l++)
                            {
                                index = ((j + l - 1) * bmpData.Stride) + ((i + k - 1) * 3);
                                txR = txR + rgbValues[index + 2] * Mask7X[k, l];
                                txG = txG + rgbValues[index + 1] * Mask7X[k, l];
                                txB = txB + rgbValues[index] * Mask7X[k, l];
                            }
                        }

                        tyR = 0.0;
                        tyG = 0.0;
                        tyB = 0.0;
                        for (k = 0; k < SizeMask; k++)
                        {
                            for (l = 0; l < SizeMask; l++)
                            {
                                index = ((j + l - 1) * bmpData.Stride) + ((i + k - 1) * 3);
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
