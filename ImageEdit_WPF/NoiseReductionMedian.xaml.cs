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
    /// Interaction logic for NoiseReductionMedian.xaml
    /// </summary>
    public partial class NoiseReductionMedian : Window
    {
        private String filename;
        private Bitmap bmpOutput = null;
        private Bitmap bmpUndoRedo = null;
        private Int32 SizeMask = 0;

        public NoiseReductionMedian(String fname, Bitmap bmpO, Bitmap bmpUR)
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
        }

        private void five_Checked(object sender, RoutedEventArgs e)
        {
            SizeMask = 5;
        }

        private void seven_Checked(object sender, RoutedEventArgs e)
        {
            SizeMask = 7;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            Int32 i = 0;
            Int32 j = 0;
            Int32 k = 0;
            Int32 l = 0;
            Int32 z = 0;
            Int32 aR = 0;
            Int32 aG = 0;
            Int32 aB = 0;
            Int32[] arR = new Int32[121];
            Int32[] arG = new Int32[121];
            Int32[] arB = new Int32[121];

            // Lock the bitmap's bits.  
            BitmapData bmpData = bmpOutput.LockBits(new System.Drawing.Rectangle(0, 0, bmpOutput.Width, bmpOutput.Height), ImageLockMode.ReadWrite, bmpOutput.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            Int32 bytes = Math.Abs(bmpData.Stride) * bmpOutput.Height;
            Byte[] rgbValues = new Byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            if (SizeMask == 3)
            {
                for (i = SizeMask / 2; i < bmpOutput.Width - SizeMask / 2; i++)
                {
                    for (j = SizeMask / 2; j < bmpOutput.Height - SizeMask / 2; j++)
                    {
                        int index;
                        z = 0;

                        for (k = 0; k < SizeMask; k++)
                        {
                            for (l = 0; l < SizeMask; l++)
                            {
                                index = ((j + l - 1) * bmpData.Stride) + ((i + k - 1) * 3);
                                arR[z] = rgbValues[index + 2];
                                arG[z] = rgbValues[index + 1];
                                arB[z] = rgbValues[index];
                                z++;
                            }
                        }

                        for (k = 1; k <= SizeMask * SizeMask - 1; k++)
                        {
                            aR = arR[k];
                            aG = arG[k];
                            aB = arB[k];
                            l = k - 1;

                            while (l >= 0 && arR[l] > aR)
                            {
                                arR[l + 1] = arR[l];
                                l--;
                            }
                            while (l >= 0 && arG[l] > aG)
                            {
                                arG[l + 1] = arG[l];
                                l--;
                            }
                            while (l >= 0 && arB[l] > aB)
                            {
                                arB[l + 1] = arB[l];
                                l--;
                            }

                            arR[l + 1] = aR;
                            arG[l + 1] = aG;
                            arB[l + 1] = aB;
                        }

                        index = (j * bmpData.Stride) + (i * 3);

                        rgbValues[index + 2] = (Byte)arR[SizeMask * SizeMask / 2];
                        rgbValues[index + 1] = (Byte)arG[SizeMask * SizeMask / 2];
                        rgbValues[index] = (Byte)arB[SizeMask * SizeMask / 2];
                    }
                }
            }
            else if (SizeMask == 5)
            {
                for (i = SizeMask / 2; i < bmpOutput.Width - SizeMask / 2; i++)
                {
                    for (j = SizeMask / 2; j < bmpOutput.Height - SizeMask / 2; j++)
                    {
                        int index;
                        z = 0;

                        for (k = 0; k < SizeMask; k++)
                        {
                            for (l = 0; l < SizeMask; l++)
                            {
                                index = ((j + l - 1) * bmpData.Stride) + ((i + k - 1) * 3);
                                arR[z] = rgbValues[index + 2];
                                arG[z] = rgbValues[index + 1];
                                arB[z] = rgbValues[index];
                                z++;
                            }
                        }

                        for (k = 1; k <= SizeMask * SizeMask - 1; k++)
                        {
                            aR = arR[k];
                            aG = arG[k];
                            aB = arB[k];
                            l = k - 1;

                            while (l >= 0 && arR[l] > aR)
                            {
                                arR[l + 1] = arR[l];
                                l--;
                            }
                            while (l >= 0 && arG[l] > aG)
                            {
                                arG[l + 1] = arG[l];
                                l--;
                            }
                            while (l >= 0 && arB[l] > aB)
                            {
                                arB[l + 1] = arB[l];
                                l--;
                            }

                            arR[l + 1] = aR;
                            arG[l + 1] = aG;
                            arB[l + 1] = aB;
                        }

                        index = (j * bmpData.Stride) + (i * 3);

                        rgbValues[index + 2] = (Byte)arR[SizeMask * SizeMask / 2];
                        rgbValues[index + 1] = (Byte)arG[SizeMask * SizeMask / 2];
                        rgbValues[index] = (Byte)arB[SizeMask * SizeMask / 2];
                    }
                }
            }
            else if (SizeMask == 7)
            {
                for (i = SizeMask / 2; i < bmpOutput.Width - SizeMask / 2; i++)
                {
                    for (j = SizeMask / 2; j < bmpOutput.Height - SizeMask / 2; j++)
                    {
                        int index;
                        z = 0;

                        for (k = 0; k < SizeMask; k++)
                        {
                            for (l = 0; l < SizeMask; l++)
                            {
                                index = ((j + l - 1) * bmpData.Stride) + ((i + k - 1) * 3);
                                arR[z] = rgbValues[index + 2];
                                arG[z] = rgbValues[index + 1];
                                arB[z] = rgbValues[index];
                                z++;
                            }
                        }

                        for (k = 1; k <= SizeMask * SizeMask - 1; k++)
                        {
                            aR = arR[k];
                            aG = arG[k];
                            aB = arB[k];
                            l = k - 1;

                            while (l >= 0 && arR[l] > aR)
                            {
                                arR[l + 1] = arR[l];
                                l--;
                            }
                            while (l >= 0 && arG[l] > aG)
                            {
                                arG[l + 1] = arG[l];
                                l--;
                            }
                            while (l >= 0 && arB[l] > aB)
                            {
                                arB[l + 1] = arB[l];
                                l--;
                            }

                            arR[l + 1] = aR;
                            arG[l + 1] = aG;
                            arB[l + 1] = aB;
                        }

                        index = (j * bmpData.Stride) + (i * 3);

                        rgbValues[index + 2] = (Byte)arR[SizeMask * SizeMask / 2];
                        rgbValues[index + 1] = (Byte)arG[SizeMask * SizeMask / 2];
                        rgbValues[index] = (Byte)arB[SizeMask * SizeMask / 2];
                    }
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
