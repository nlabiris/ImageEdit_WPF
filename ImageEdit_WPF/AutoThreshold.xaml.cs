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
    /// Interaction logic for AutoThreshold.xaml
    /// </summary>
    public partial class AutoThreshold : Window
    {
        private String filename;
        private Bitmap bmpOutput = null;
        private Bitmap bmpUndoRedo = null;

        public AutoThreshold(String fname, Bitmap bmpO, Bitmap bmpUR)
        {
            InitializeComponent();

            filename = fname;
            bmpOutput = bmpO;
            bmpUndoRedo = bmpUR;

            textboxDistance.Focus();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            Int32 distance = 0;
            Int32 i;
            Int32 j;
            Int32 k;
            Int32 l;
            Int32 B;
            Int32 G;
            Int32 R;
            Double z = 0.0;
            Int32 z1R = 0;
            Int32 z1G = 0;
            Int32 z1B = 0;
            Int32 z2R = 0;
            Int32 z2G = 0;
            Int32 z2B = 0;
            Int32 Positionz1R = 0;
            Int32 Positionz1G = 0;
            Int32 Positionz1B = 0;
            Int32 Positionz2R = 0;
            Int32 Positionz2G = 0;
            Int32 Positionz2B = 0;
            Int32 Temp = 0;
            Int32 ThresholdR = 0;
            Int32 ThresholdG = 0;
            Int32 ThresholdB = 0;
            Int32[] HistogramR = new Int32[256];
            Int32[] HistogramG = new Int32[256];
            Int32[] HistogramB = new Int32[256];
            Int32[] HistogramSortR = new Int32[256];
            Int32[] HistogramSortG = new Int32[256];
            Int32[] HistogramSortB = new Int32[256];
            Int32[] PositionR = new Int32[256];
            Int32[] PositionG = new Int32[256];
            Int32[] PositionB = new Int32[256];

            try
            {
                distance = Int32.Parse(textboxDistance.Text);
                if (distance > 255 || distance < 0)
                {
                    String message = "Wrong range" + Environment.NewLine + Environment.NewLine + "Give a number between 0 and 255";
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentNullException", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "FormatException", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
            catch (OverflowException ex)
            {
                MessageBox.Show(ex.Message, "OverflowException", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

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

            for (i = 0; i < 256; i++)
            {
                HistogramR[i] = 0;
                HistogramG[i] = 0;
                HistogramB[i] = 0;
                HistogramSortR[i] = 0;
                HistogramSortG[i] = 0;
                HistogramSortB[i] = 0;
                PositionR[i] = i;
                PositionG[i] = i;
                PositionB[i] = i;
            }

            for (i = 0; i < bmpOutput.Width; i++)
            {
                for (j = 0; j < bmpOutput.Height; j++)
                {
                    int index = (j * bmpData.Stride) + (i * 3);

                    B = rgbValues[index];
                    HistogramB[B]++;
                    HistogramSortB[B]++;
                    G = rgbValues[index + 1];
                    HistogramG[G]++;
                    HistogramSortG[G]++;
                    R = rgbValues[index + 2];
                    HistogramR[R]++;
                    HistogramSortR[R]++;
                }
            }

            for (k = 1; k < 256; k++)
            {
                for (l = 255; l >= k; l--)
                {
                    if (HistogramSortR[l - 1] < HistogramSortR[l])
                    {
                        Temp = HistogramSortR[l - 1];
                        HistogramSortR[l - 1] = HistogramSortR[l];
                        HistogramSortR[l] = Temp;
                        Temp = PositionR[l - 1];
                        PositionR[l - 1] = PositionR[l];
                        PositionR[l] = Temp;
                    }

                    if (HistogramSortG[l - 1] < HistogramSortG[l])
                    {
                        Temp = HistogramSortG[l - 1];
                        HistogramSortG[l - 1] = HistogramSortG[l];
                        HistogramSortG[l] = Temp;
                        Temp = PositionG[l - 1];
                        PositionG[l - 1] = PositionG[l];
                        PositionG[l] = Temp;
                    }

                    if (HistogramSortB[l - 1] < HistogramSortB[l])
                    {
                        Temp = HistogramSortB[l - 1];
                        HistogramSortB[l - 1] = HistogramSortB[l];
                        HistogramSortB[l] = Temp;
                        Temp = PositionB[l - 1];
                        PositionB[l - 1] = PositionB[l];
                        PositionB[l] = Temp;
                    }
                }
            }

            z1R = HistogramSortR[0];
            Positionz1R = PositionR[0];
            z1G = HistogramSortG[0];
            Positionz1G = PositionG[0];
            z1B = HistogramSortB[0];
            Positionz1B = PositionB[0];

            for (i = 1; i < 256; i++)
            {
                if ((Math.Abs(PositionR[i] - Positionz1R)) > distance)
                {
                    z2R = HistogramSortR[i];
                    Positionz2R = PositionR[i];
                    break;
                }
                if ((Math.Abs(PositionG[i] - Positionz1G)) > distance)
                {
                    z2G = HistogramSortG[i];
                    Positionz2G = PositionG[i];
                    break;
                }

                if ((Math.Abs(PositionB[i] - Positionz1B)) > distance)
                {
                    z2B = HistogramSortB[i];
                    Positionz2B = PositionB[i];
                    break;
                }
            }


            if (Positionz1R < Positionz2R)
            {
                z = HistogramR[Positionz1R + 1] * 1.0 / z2R;
                for (i = Positionz1R + 1; i < Positionz2R; i++)
                {
                    if ((HistogramR[i] * 1.0 / z2R) < z)
                    {
                        z = HistogramR[i] * 1.0 / z2R;
                        ThresholdR = i;
                    }
                }
            }
            else
            {
                z = HistogramR[Positionz2R + 1] * 1.0 / z1R;
                for (i = Positionz2R + 1; i < Positionz1R; i++)
                {
                    if ((HistogramR[i] * 1.0 / z1R) < z)
                    {
                        z = HistogramR[i] * 1.0 / z1R;
                        ThresholdR = i;
                    }
                }
            }

            if (Positionz1G < Positionz2G)
            {
                z = HistogramG[Positionz1G + 1] * 1.0 / z2G;
                for (i = Positionz1G + 1; i < Positionz2G; i++)
                {
                    if ((HistogramG[i] * 1.0 / z2G) < z)
                    {
                        z = HistogramG[i] * 1.0 / z2G;
                        ThresholdG = i;
                    }
                }
            }
            else
            {
                z = HistogramG[Positionz2G + 1] * 1.0 / z1G;
                for (i = Positionz2G + 1; i < Positionz1G; i++)
                {
                    if ((HistogramG[i] * 1.0 / z1G) < z)
                    {
                        z = HistogramG[i] * 1.0 / z1G;
                        ThresholdG = i;
                    }
                }
            }

            if (Positionz1B < Positionz2B)
            {
                z = HistogramB[Positionz1B + 1] * 1.0 / z2B;
                for (i = Positionz1B + 1; i < Positionz2B; i++)
                {
                    if ((HistogramB[i] * 1.0 / z2B) < z)
                    {
                        z = HistogramB[i] * 1.0 / z2B;
                        ThresholdB = i;
                    }
                }
            }
            else
            {
                z = HistogramB[Positionz2B + 1] * 1.0 / z1B;
                for (i = Positionz2B + 1; i < Positionz1B; i++)
                {
                    if ((HistogramB[i] * 1.0 / z1B) < z)
                    {
                        z = HistogramB[i] * 1.0 / z1B;
                        ThresholdB = i;
                    }
                }
            }

            for (i = 0; i < bmpOutput.Width; i++)
            {
                for (j = 0; j < bmpOutput.Height; j++)
                {
                    int index = (j * bmpData.Stride) + (i * 3);

                    R = rgbValues[index + 2];
                    G = rgbValues[index + 1];
                    B = rgbValues[index];

                    if (R < ThresholdR)
                    {
                        R = 0;
                    }
                    else
                    {
                        R = 255;
                    }

                    if (G < ThresholdG)
                    {
                        G = 0;
                    }
                    else
                    {
                        G = 255;
                    }

                    if (B < ThresholdB)
                    {
                        B = 0;
                    }
                    else
                    {
                        B = 255;
                    }

                    rgbValues[index + 2] = (Byte)R;
                    rgbValues[index + 1] = (Byte)G;
                    rgbValues[index] = (Byte)B;
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

            String messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Threshold (RED) set at: " + ThresholdR + Environment.NewLine + "Threshold (GREEN) set at: " + ThresholdG + Environment.NewLine + "Threshold (BLUE) set at: " + ThresholdB + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
            MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                MainWindow.noChange = false;
                MainWindow.Action = ActionType.AutoThreshold;
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
