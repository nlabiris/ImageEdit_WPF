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
    /// Interaction logic for Sharpen.xaml
    /// </summary>
    public partial class Sharpen : Window
    {
        private String filename;
        private Bitmap bmpOutput = null;
        private Bitmap bmpUndoRedo = null;
        private Int32 SizeMask = 0;

        public Sharpen(String fname, Bitmap bmpO, Bitmap bmpUR)
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

        private void five_Checked(object sender, RoutedEventArgs e)
        {
            SizeMask = 5;

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

        private void seven_Checked(object sender, RoutedEventArgs e)
        {
            SizeMask = 7;

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

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            Int32 i = 0;
            Int32 j = 0;
            Int32 k;
            Int32 l;
            Int32 tR;
            Int32 tG;
            Int32 tB;
            Int32[,] Mask3X;
            Int32[,] Mask5X;
            Int32[,] Mask7X;

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

                for (i = 1; i < bmpOutput.Width - 1; i++)
                {
                    for (j = 1; j < bmpOutput.Height - 1; j++)
                    {
                        int index;

                        tR = 0;
                        tG = 0;
                        tB = 0;
                        for (k = 0; k < SizeMask; k++)
                        {
                            for (l = 0; l < SizeMask; l++)
                            {
                                index = ((j + l - 1) * bmpData.Stride) + ((i + k - 1) * 3);
                                tR = tR + rgbValues[index + 2] * Mask3X[k, l];
                                tG = tG + rgbValues[index + 1] * Mask3X[k, l];
                                tB = tB + rgbValues[index] * Mask3X[k, l];
                            }
                        }

                        if (tR > 255)
                        {
                            tR = 255;
                        }
                        else if (tR < 0)
                        {
                            tR = 0;
                        }

                        if (tG > 255)
                        {
                            tG = 255;
                        }
                        else if (tG < 0)
                        {
                            tG = 0;
                        }

                        if (tB > 255)
                        {
                            tB = 255;
                        }
                        else if (tB < 0)
                        {
                            tB = 0;
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

                for (i = 2; i < bmpOutput.Width - 2; i++)
                {
                    for (j = 2; j < bmpOutput.Height - 2; j++)
                    {
                        int index;

                        tR = 0;
                        tG = 0;
                        tB = 0;
                        for (k = 0; k < SizeMask; k++)
                        {
                            for (l = 0; l < SizeMask; l++)
                            {
                                index = ((j + l - 2) * bmpData.Stride) + ((i + k - 2) * 3);
                                tR = tR + rgbValues[index + 2] * Mask5X[k, l];
                                tG = tG + rgbValues[index + 1] * Mask5X[k, l];
                                tB = tB + rgbValues[index] * Mask5X[k, l];
                            }
                        }

                        if (tR > 255)
                        {
                            tR = 255;
                        }
                        else if (tR < 0)
                        {
                            tR = 0;
                        }

                        if (tG > 255)
                        {
                            tG = 255;
                        }
                        else if (tG < 0)
                        {
                            tG = 0;
                        }

                        if (tB > 255)
                        {
                            tB = 255;
                        }
                        else if (tB < 0)
                        {
                            tB = 0;
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

                for (i = 3; i < bmpOutput.Width - 3; i++)
                {
                    for (j = 3; j < bmpOutput.Height - 3; j++)
                    {
                        int index;

                        tR = 0;
                        tG = 0;
                        tB = 0;
                        for (k = 0; k < SizeMask; k++)
                        {
                            for (l = 0; l < SizeMask; l++)
                            {
                                index = ((j + l - 3) * bmpData.Stride) + ((i + k - 3) * 3);
                                tR = tR + rgbValues[index + 2] * Mask7X[k, l];
                                tG = tG + rgbValues[index + 1] * Mask7X[k, l];
                                tB = tB + rgbValues[index] * Mask7X[k, l];
                            }
                        }

                        if (tR > 255)
                        {
                            tR = 255;
                        }
                        else if (tR < 0)
                        {
                            tR = 0;
                        }

                        if (tG > 255)
                        {
                            tG = 255;
                        }
                        else if (tG < 0)
                        {
                            tG = 0;
                        }

                        if (tB > 255)
                        {
                            tB = 255;
                        }
                        else if (tB < 0)
                        {
                            tB = 0;
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
