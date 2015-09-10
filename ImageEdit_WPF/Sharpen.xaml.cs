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

        public Sharpen(String fname, Bitmap bmpO, Bitmap bmpUR)
        {
            InitializeComponent();

            filename = fname;
            bmpOutput = bmpO;
            bmpUndoRedo = bmpUR;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            Int32 SizeMask = 3;
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
            Double[,] MaskX;

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

            MaskX = new Double[3, 3] {
                                                    { Double.Parse(tbx1.Text, new CultureInfo("el-GR")), Double.Parse(tbx2.Text, new CultureInfo("el-GR")), Double.Parse(tbx3.Text, new CultureInfo("el-GR")) },
                                                    { Double.Parse(tbx4.Text, new CultureInfo("el-GR")), Double.Parse(tbx5.Text, new CultureInfo("el-GR")), Double.Parse(tbx6.Text, new CultureInfo("el-GR")) },
                                                    { Double.Parse(tbx7.Text, new CultureInfo("el-GR")), Double.Parse(tbx8.Text, new CultureInfo("el-GR")), Double.Parse(tbx9.Text, new CultureInfo("el-GR")) }
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
                            txR = txR + rgbValues[index + 2] * MaskX[k, l];
                            txG = txG + rgbValues[index + 1] * MaskX[k, l];
                            txB = txB + rgbValues[index] * MaskX[k, l];
                        }
                    }

                    tR = Math.Sqrt(txR * txR);
                    tG = Math.Sqrt(txG * txG);
                    tB = Math.Sqrt(txB * txB);

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
