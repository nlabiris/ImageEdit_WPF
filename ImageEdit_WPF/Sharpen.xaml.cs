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
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageEdit_WPF
{
    /// <summary>
    /// Interaction logic for Sharpen.xaml
    /// </summary>
    public partial class Sharpen : Window
    {
        /// <summary>
        /// Output image.
        /// </summary>
        private readonly Bitmap _bmpOutput = null;

        /// <summary>
        /// Image used at the Undo/Redo system.
        /// </summary>
        private Bitmap _bmpUndoRedo = null;

        /// <summary>
        /// Size of the kernel.
        /// </summary>
        private int _sizeMask = 0;

        /// <summary>
        /// Sharpen <c>constructor</c>.
        /// Here we initialiaze the images and also we set the focus
        /// at the 'OK' button and at one of the three radio boxes (kernel size).
        /// </summary>
        /// <param name="bmpO">Output image.</param>
        /// <param name="bmpUR">Image used at the Undo/Redo system.</param>
        public Sharpen(Bitmap bmpO, Bitmap bmpUR)
        {
            InitializeComponent();

            _bmpOutput = bmpO;
            _bmpUndoRedo = bmpUR;

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
        ///         Group box width.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box height.
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
        private void three_Checked(object sender, RoutedEventArgs e)
        {
            _sizeMask = 3;

            this.Height = 250;
            this.Width = 180;

            groupBox.Width = 110;
            groupBox.Height = 90;

            ok.Margin = new Thickness(50, 10, 50, 10);

            tbx1.Text = "-1";
            tbx2.Text = "-1";
            tbx3.Text = "-1";
            tbx8.Text = "-1";
            tbx9.Text = "9";
            tbx10.Text = "-1";
            tbx15.Text = "-1";
            tbx16.Text = "-1";
            tbx17.Text = "-1";

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
        ///         Group box width.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box height.
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
        private void five_Checked(object sender, RoutedEventArgs e)
        {
            _sizeMask = 5;

            this.Height = 290;
            this.Width = 220;

            groupBox.Width = 170;
            groupBox.Height = 130;

            ok.Margin = new Thickness(70, 10, 70, 10);

            tbx1.Text = "-1";
            tbx2.Text = "-1";
            tbx3.Text = "-1";
            tbx4.Text = "-1";
            tbx5.Text = "-1";
            tbx8.Text = "-1";
            tbx9.Text = "-1";
            tbx10.Text = "-1";
            tbx11.Text = "-1";
            tbx12.Text = "-1";
            tbx15.Text = "-1";
            tbx16.Text = "-1";
            tbx17.Text = "25";
            tbx18.Text = "-1";
            tbx19.Text = "-1";
            tbx22.Text = "-1";
            tbx23.Text = "-1";
            tbx24.Text = "-1";
            tbx25.Text = "-1";
            tbx26.Text = "-1";
            tbx29.Text = "-1";
            tbx30.Text = "-1";
            tbx31.Text = "-1";
            tbx32.Text = "-1";
            tbx33.Text = "-1";

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
        }

        /// <summary>
        /// If kernel's size is 7x7, the following attributes are set.
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
        ///         Group box width.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box height.
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
        private void seven_Checked(object sender, RoutedEventArgs e)
        {
            _sizeMask = 7;

            this.Height = 330;
            this.Width = 270;

            groupBox.Width = 230;
            groupBox.Height = 170;

            ok.Margin = new Thickness(90, 10, 90, 10);

            tbx1.Text = "-1";
            tbx2.Text = "-1";
            tbx3.Text = "-1";
            tbx4.Text = "-1";
            tbx5.Text = "-1";
            tbx6.Text = "-1";
            tbx7.Text = "-1";
            tbx8.Text = "-1";
            tbx9.Text = "-1";
            tbx10.Text = "-1";
            tbx11.Text = "-1";
            tbx12.Text = "-1";
            tbx13.Text = "-1";
            tbx14.Text = "-1";
            tbx15.Text = "-1";
            tbx16.Text = "-1";
            tbx17.Text = "-1";
            tbx18.Text = "-1";
            tbx19.Text = "-1";
            tbx20.Text = "-1";
            tbx21.Text = "-1";
            tbx22.Text = "-1";
            tbx23.Text = "-1";
            tbx24.Text = "-1";
            tbx25.Text = "49";
            tbx26.Text = "-1";
            tbx27.Text = "-1";
            tbx28.Text = "-1";
            tbx29.Text = "-1";
            tbx30.Text = "-1";
            tbx31.Text = "-1";
            tbx32.Text = "-1";
            tbx33.Text = "-1";
            tbx34.Text = "-1";
            tbx35.Text = "-1";
            tbx36.Text = "-1";
            tbx37.Text = "-1";
            tbx38.Text = "-1";
            tbx39.Text = "-1";
            tbx40.Text = "-1";
            tbx41.Text = "-1";
            tbx42.Text = "-1";
            tbx43.Text = "-1";
            tbx44.Text = "-1";
            tbx45.Text = "-1";
            tbx46.Text = "-1";
            tbx47.Text = "-1";
            tbx48.Text = "-1";
            tbx49.Text = "-1";

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
        }

        /// <summary>
        /// Implementation of the Sharpen algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            double tR = 0.0;
            double tG = 0.0;
            double tB = 0.0;
            int[,] mask3X;
            int[,] mask5X;
            int[,] mask7X;
            int sumMask = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = _bmpOutput.LockBits(new Rectangle(0, 0, _bmpOutput.Width, _bmpOutput.Height), ImageLockMode.ReadWrite, _bmpOutput.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride) * _bmpOutput.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] bgrValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            if (_sizeMask == 3)
            {
                mask3X = new int[3, 3] {
                                            { int.Parse(tbx1.Text), int.Parse(tbx2.Text), int.Parse(tbx3.Text) },
                                            { int.Parse(tbx8.Text), int.Parse(tbx9.Text), int.Parse(tbx10.Text) },
                                            { int.Parse(tbx15.Text), int.Parse(tbx16.Text), int.Parse(tbx17.Text) }
                                         };
                
                for (i = 0; i < _sizeMask; i++)
                {
                    for (j = 0; j < _sizeMask; j++)
                    {
                        sumMask += mask3X[i, j];
                    }
                }

                for (i = 1; i < _bmpOutput.Width - 1; i++)
                {
                    for (j = 1; j < _bmpOutput.Height - 1; j++)
                    {
                        int index;

                        tR = 0.0;
                        tG = 0.0;
                        tB = 0.0;
                        for (k = 0; k < _sizeMask; k++)
                        {
                            for (l = 0; l < _sizeMask; l++)
                            {
                                index = ((j + l - 1) * bmpData.Stride) + ((i + k - 1) * 3);
                                tR = tR + (rgbValues[index + 2] * mask3X[k, l]) / sumMask;
                                tG = tG + (rgbValues[index + 1] * mask3X[k, l]) / sumMask;
                                tB = tB + (rgbValues[index] * mask3X[k, l]) / sumMask;
                            }
                        }

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

                        bgrValues[index + 2] = (byte)tR;
                        bgrValues[index + 1] = (byte)tG;
                        bgrValues[index] = (byte)tB;
                    }
                }
            }
            else if (_sizeMask == 5)
            {
                mask5X = new int[5, 5] {
                                            { int.Parse(tbx1.Text), int.Parse(tbx2.Text), int.Parse(tbx3.Text), int.Parse(tbx4.Text), int.Parse(tbx5.Text) },
                                            { int.Parse(tbx8.Text), int.Parse(tbx9.Text), int.Parse(tbx10.Text), int.Parse(tbx11.Text), int.Parse(tbx12.Text) },
                                            { int.Parse(tbx15.Text), int.Parse(tbx16.Text), int.Parse(tbx17.Text), int.Parse(tbx18.Text), int.Parse(tbx19.Text) },
                                            { int.Parse(tbx22.Text), int.Parse(tbx23.Text), int.Parse(tbx24.Text), int.Parse(tbx25.Text), int.Parse(tbx26.Text) },
                                            { int.Parse(tbx29.Text), int.Parse(tbx30.Text), int.Parse(tbx31.Text), int.Parse(tbx32.Text), int.Parse(tbx33.Text) }
                                         };

                for (i = 0; i < _sizeMask; i++)
                {
                    for (j = 0; j < _sizeMask; j++)
                    {
                        sumMask += mask5X[i, j];
                    }
                }

                for (i = 2; i < _bmpOutput.Width - 2; i++)
                {
                    for (j = 2; j < _bmpOutput.Height - 2; j++)
                    {
                        int index;

                        tR = 0.0;
                        tG = 0.0;
                        tB = 0.0;
                        for (k = 0; k < _sizeMask; k++)
                        {
                            for (l = 0; l < _sizeMask; l++)
                            {
                                index = ((j + l - 2) * bmpData.Stride) + ((i + k - 2) * 3);
                                tR = tR + (rgbValues[index + 2] * mask5X[k, l]) / sumMask;
                                tG = tG + (rgbValues[index + 1] * mask5X[k, l]) / sumMask;
                                tB = tB + (rgbValues[index] * mask5X[k, l]) / sumMask;
                            }
                        }

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

                        bgrValues[index + 2] = (byte)tR;
                        bgrValues[index + 1] = (byte)tG;
                        bgrValues[index] = (byte)tB;
                    }
                }
            }
            else if (_sizeMask == 7)
            {
                mask7X = new int[7, 7] {
                                            { int.Parse(tbx1.Text), int.Parse(tbx2.Text), int.Parse(tbx3.Text), int.Parse(tbx4.Text), int.Parse(tbx5.Text), int.Parse(tbx6.Text), int.Parse(tbx7.Text) },
                                            { int.Parse(tbx8.Text), int.Parse(tbx9.Text), int.Parse(tbx10.Text), int.Parse(tbx11.Text), int.Parse(tbx12.Text), int.Parse(tbx13.Text), int.Parse(tbx14.Text) },
                                            { int.Parse(tbx15.Text), int.Parse(tbx16.Text), int.Parse(tbx17.Text), int.Parse(tbx18.Text), int.Parse(tbx19.Text), int.Parse(tbx20.Text), int.Parse(tbx21.Text) },
                                            { int.Parse(tbx22.Text), int.Parse(tbx23.Text), int.Parse(tbx24.Text), int.Parse(tbx25.Text), int.Parse(tbx26.Text), int.Parse(tbx27.Text), int.Parse(tbx28.Text) },
                                            { int.Parse(tbx29.Text), int.Parse(tbx30.Text), int.Parse(tbx31.Text), int.Parse(tbx32.Text), int.Parse(tbx33.Text), int.Parse(tbx34.Text), int.Parse(tbx35.Text) },
                                            { int.Parse(tbx36.Text), int.Parse(tbx37.Text), int.Parse(tbx38.Text), int.Parse(tbx39.Text), int.Parse(tbx40.Text), int.Parse(tbx41.Text), int.Parse(tbx42.Text) },
                                            { int.Parse(tbx43.Text), int.Parse(tbx44.Text), int.Parse(tbx45.Text), int.Parse(tbx46.Text), int.Parse(tbx47.Text), int.Parse(tbx48.Text), int.Parse(tbx49.Text) }
                                         };

                for (i = 0; i < _sizeMask; i++)
                {
                    for (j = 0; j < _sizeMask; j++)
                    {
                        sumMask += mask7X[i, j];
                    }
                }

                for (i = 3; i < _bmpOutput.Width - 3; i++)
                {
                    for (j = 3; j < _bmpOutput.Height - 3; j++)
                    {
                        int index;

                        tR = 0.0;
                        tG = 0.0;
                        tB = 0.0;
                        for (k = 0; k < _sizeMask; k++)
                        {
                            for (l = 0; l < _sizeMask; l++)
                            {
                                index = ((j + l - 3) * bmpData.Stride) + ((i + k - 3) * 3);
                                tR = tR + (rgbValues[index + 2] * mask7X[k, l]) / sumMask;
                                tG = tG + (rgbValues[index + 1] * mask7X[k, l]) / sumMask;
                                tB = tB + (rgbValues[index] * mask7X[k, l]) / sumMask;
                            }
                        }

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

                        bgrValues[index + 2] = (byte)tR;
                        bgrValues[index + 1] = (byte)tG;
                        bgrValues[index] = (byte)tB;
                    }
                }
            }

            for (i = 0; i < _bmpOutput.Width; i++)
            {
                for (j = 0; j < _bmpOutput.Height; j++)
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
            _bmpOutput.UnlockBits(bmpData);

            // Convert Bitmap to BitmapImage
            BitmapToBitmapImage();

            string messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
            MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                MainWindow.NoChange = false;
                MainWindow.Action = ActionType.ImageConvolution;
                _bmpUndoRedo = _bmpOutput.Clone() as Bitmap;
                MainWindow.UndoStack.Push(_bmpUndoRedo);
                MainWindow.RedoStack.Clear();
                foreach (Window mainWindow in Application.Current.Windows)
                {
                    if (mainWindow.GetType() == typeof(MainWindow))
                    {
                        ((MainWindow) mainWindow).undo.IsEnabled = true;
                        ((MainWindow) mainWindow).redo.IsEnabled = false;
                    }
                }
                this.Close();
            }
        }

        /// <summary>
        /// <c>Bitmap</c> to <c>BitmpaImage</c> conversion method in order to show the edited image at the main window.
        /// </summary>
        public void BitmapToBitmapImage()
        {
            MemoryStream str = new MemoryStream();
            _bmpOutput.Save(str, ImageFormat.Bmp);
            str.Seek(0, SeekOrigin.Begin);
            BmpBitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

            foreach (Window mainWindow in Application.Current.Windows)
            {
                if (mainWindow.GetType() == typeof(MainWindow))
                {
                    ((MainWindow) mainWindow).mainImage.Source = bdc.Frames[0];
                }
            }
        }
    }
}
