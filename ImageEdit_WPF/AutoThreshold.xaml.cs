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
        private readonly Bitmap _bmpOutput = null;
        private Bitmap _bmpUndoRedo = null;

        public AutoThreshold(Bitmap bmpO, Bitmap bmpUR)
        {
            InitializeComponent();

            _bmpOutput = bmpO;
            _bmpUndoRedo = bmpUR;

            textboxDistance.Focus();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            int distance = 0;
            int i;
            int j;
            int k;
            int l;
            int b;
            int g;
            int r;
            double z = 0.0;
            int z1R = 0;
            int z1G = 0;
            int z1B = 0;
            int z2R = 0;
            int z2G = 0;
            int z2B = 0;
            int positionz1R = 0;
            int positionz1G = 0;
            int positionz1B = 0;
            int positionz2R = 0;
            int positionz2G = 0;
            int positionz2B = 0;
            int temp = 0;
            int thresholdR = 0;
            int thresholdG = 0;
            int thresholdB = 0;
            int[] histogramR = new int[256];
            int[] histogramG = new int[256];
            int[] histogramB = new int[256];
            int[] histogramSortR = new int[256];
            int[] histogramSortG = new int[256];
            int[] histogramSortB = new int[256];
            int[] positionR = new int[256];
            int[] positionG = new int[256];
            int[] positionB = new int[256];

            try
            {
                distance = int.Parse(textboxDistance.Text);
                if (distance > 255 || distance < 0)
                {
                    string message = "Wrong range" + Environment.NewLine + Environment.NewLine + "Give a number between 0 and 255";
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
            BitmapData bmpData = _bmpOutput.LockBits(new Rectangle(0, 0, _bmpOutput.Width, _bmpOutput.Height), ImageLockMode.ReadWrite, _bmpOutput.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride) * _bmpOutput.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            for (i = 0; i < 256; i++)
            {
                histogramR[i] = 0;
                histogramG[i] = 0;
                histogramB[i] = 0;
                histogramSortR[i] = 0;
                histogramSortG[i] = 0;
                histogramSortB[i] = 0;
                positionR[i] = i;
                positionG[i] = i;
                positionB[i] = i;
            }

            for (i = 0; i < _bmpOutput.Width; i++)
            {
                for (j = 0; j < _bmpOutput.Height; j++)
                {
                    int index = (j * bmpData.Stride) + (i * 3);

                    b = rgbValues[index];
                    histogramB[b]++;
                    histogramSortB[b]++;
                    g = rgbValues[index + 1];
                    histogramG[g]++;
                    histogramSortG[g]++;
                    r = rgbValues[index + 2];
                    histogramR[r]++;
                    histogramSortR[r]++;
                }
            }

            for (k = 1; k < 256; k++)
            {
                for (l = 255; l >= k; l--)
                {
                    if (histogramSortR[l - 1] < histogramSortR[l])
                    {
                        temp = histogramSortR[l - 1];
                        histogramSortR[l - 1] = histogramSortR[l];
                        histogramSortR[l] = temp;
                        temp = positionR[l - 1];
                        positionR[l - 1] = positionR[l];
                        positionR[l] = temp;
                    }

                    if (histogramSortG[l - 1] < histogramSortG[l])
                    {
                        temp = histogramSortG[l - 1];
                        histogramSortG[l - 1] = histogramSortG[l];
                        histogramSortG[l] = temp;
                        temp = positionG[l - 1];
                        positionG[l - 1] = positionG[l];
                        positionG[l] = temp;
                    }

                    if (histogramSortB[l - 1] < histogramSortB[l])
                    {
                        temp = histogramSortB[l - 1];
                        histogramSortB[l - 1] = histogramSortB[l];
                        histogramSortB[l] = temp;
                        temp = positionB[l - 1];
                        positionB[l - 1] = positionB[l];
                        positionB[l] = temp;
                    }
                }
            }

            z1R = histogramSortR[0];
            positionz1R = positionR[0];
            z1G = histogramSortG[0];
            positionz1G = positionG[0];
            z1B = histogramSortB[0];
            positionz1B = positionB[0];

            for (i = 1; i < 256; i++)
            {
                if ((Math.Abs(positionR[i] - positionz1R)) > distance)
                {
                    z2R = histogramSortR[i];
                    positionz2R = positionR[i];
                    break;
                }
                if ((Math.Abs(positionG[i] - positionz1G)) > distance)
                {
                    z2G = histogramSortG[i];
                    positionz2G = positionG[i];
                    break;
                }

                if ((Math.Abs(positionB[i] - positionz1B)) > distance)
                {
                    z2B = histogramSortB[i];
                    positionz2B = positionB[i];
                    break;
                }
            }


            if (positionz1R < positionz2R)
            {
                z = histogramR[positionz1R + 1] * 1.0 / z2R;
                for (i = positionz1R + 1; i < positionz2R; i++)
                {
                    if ((histogramR[i] * 1.0 / z2R) < z)
                    {
                        z = histogramR[i] * 1.0 / z2R;
                        thresholdR = i;
                    }
                }
            }
            else
            {
                z = histogramR[positionz2R + 1] * 1.0 / z1R;
                for (i = positionz2R + 1; i < positionz1R; i++)
                {
                    if ((histogramR[i] * 1.0 / z1R) < z)
                    {
                        z = histogramR[i] * 1.0 / z1R;
                        thresholdR = i;
                    }
                }
            }

            if (positionz1G < positionz2G)
            {
                z = histogramG[positionz1G + 1] * 1.0 / z2G;
                for (i = positionz1G + 1; i < positionz2G; i++)
                {
                    if ((histogramG[i] * 1.0 / z2G) < z)
                    {
                        z = histogramG[i] * 1.0 / z2G;
                        thresholdG = i;
                    }
                }
            }
            else
            {
                z = histogramG[positionz2G + 1] * 1.0 / z1G;
                for (i = positionz2G + 1; i < positionz1G; i++)
                {
                    if ((histogramG[i] * 1.0 / z1G) < z)
                    {
                        z = histogramG[i] * 1.0 / z1G;
                        thresholdG = i;
                    }
                }
            }

            if (positionz1B < positionz2B)
            {
                z = histogramB[positionz1B + 1] * 1.0 / z2B;
                for (i = positionz1B + 1; i < positionz2B; i++)
                {
                    if ((histogramB[i] * 1.0 / z2B) < z)
                    {
                        z = histogramB[i] * 1.0 / z2B;
                        thresholdB = i;
                    }
                }
            }
            else
            {
                z = histogramB[positionz2B + 1] * 1.0 / z1B;
                for (i = positionz2B + 1; i < positionz1B; i++)
                {
                    if ((histogramB[i] * 1.0 / z1B) < z)
                    {
                        z = histogramB[i] * 1.0 / z1B;
                        thresholdB = i;
                    }
                }
            }

            for (i = 0; i < _bmpOutput.Width; i++)
            {
                for (j = 0; j < _bmpOutput.Height; j++)
                {
                    int index = (j * bmpData.Stride) + (i * 3);

                    r = rgbValues[index + 2];
                    g = rgbValues[index + 1];
                    b = rgbValues[index];

                    if (r < thresholdR)
                    {
                        r = 0;
                    }
                    else
                    {
                        r = 255;
                    }

                    if (g < thresholdG)
                    {
                        g = 0;
                    }
                    else
                    {
                        g = 255;
                    }

                    if (b < thresholdB)
                    {
                        b = 0;
                    }
                    else
                    {
                        b = 255;
                    }

                    rgbValues[index + 2] = (byte)r;
                    rgbValues[index + 1] = (byte)g;
                    rgbValues[index] = (byte)b;
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

            string messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Threshold (RED) set at: " + thresholdR + Environment.NewLine + "Threshold (GREEN) set at: " + thresholdG + Environment.NewLine + "Threshold (BLUE) set at: " + thresholdB + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
            MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                MainWindow.NoChange = false;
                MainWindow.Action = ActionType.AutoThreshold;
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
