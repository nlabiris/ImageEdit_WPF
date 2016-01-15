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
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;

namespace ImageEdit_WPF.Windows {
    /// <summary>
    /// Interaction logic for AutoThreshold.xaml
    /// </summary>
    public partial class AutoThreshold : Window {
        private ImageEditData m_data = null;

        /// <summary>
        /// Auto Threshold <c>constructor</c>.
        /// Here we initialiaze the images and also we set the focus at the textBox being used.
        /// </summary>
        public AutoThreshold(ImageEditData data) {
            m_data = data;

            InitializeComponent();
            textboxDistance.Focus();
        }

        /// <summary>
        /// Implementation of the Auto Threshold algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ok_Click(object sender, RoutedEventArgs e) {
            int distance = 0;
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            int b = 0;
            int g = 0;
            int r = 0;
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

            try {
                distance = int.Parse(textboxDistance.Text);
                if (distance > 255 || distance < 0) {
                    string message = "Wrong range\r\n\r\nGive a number between 0 and 255";
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            } catch (ArgumentNullException ex) {
                MessageBox.Show(ex.Message, "ArgumentNullException", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            } catch (FormatException ex) {
                MessageBox.Show(ex.Message, "FormatException", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            } catch (OverflowException ex) {
                MessageBox.Show(ex.Message, "OverflowException", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            // Lock the bitmap's bits.  
            BitmapData bmpData = m_data.M_bitmap.LockBits(new Rectangle(0, 0, m_data.M_bitmap.Width, m_data.M_bitmap.Height), ImageLockMode.ReadWrite, m_data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*m_data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            for (i = 0; i < 256; i++) {
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

            for (i = 0; i < m_data.M_bitmap.Width; i++) {
                for (j = 0; j < m_data.M_bitmap.Height; j++) {
                    int index = (j*bmpData.Stride) + (i*3);

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

            for (k = 1; k < 256; k++) {
                for (l = 255; l >= k; l--) {
                    if (histogramSortR[l - 1] < histogramSortR[l]) {
                        temp = histogramSortR[l - 1];
                        histogramSortR[l - 1] = histogramSortR[l];
                        histogramSortR[l] = temp;
                        temp = positionR[l - 1];
                        positionR[l - 1] = positionR[l];
                        positionR[l] = temp;
                    }

                    if (histogramSortG[l - 1] < histogramSortG[l]) {
                        temp = histogramSortG[l - 1];
                        histogramSortG[l - 1] = histogramSortG[l];
                        histogramSortG[l] = temp;
                        temp = positionG[l - 1];
                        positionG[l - 1] = positionG[l];
                        positionG[l] = temp;
                    }

                    if (histogramSortB[l - 1] < histogramSortB[l]) {
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

            for (i = 1; i < 256; i++) {
                if (Math.Abs(positionR[i] - positionz1R) > distance) {
                    z2R = histogramSortR[i];
                    positionz2R = positionR[i];
                    break;
                }
                if (Math.Abs(positionG[i] - positionz1G) > distance) {
                    z2G = histogramSortG[i];
                    positionz2G = positionG[i];
                    break;
                }

                if (Math.Abs(positionB[i] - positionz1B) > distance) {
                    z2B = histogramSortB[i];
                    positionz2B = positionB[i];
                    break;
                }
            }


            if (positionz1R < positionz2R) {
                z = histogramR[positionz1R + 1]*1.0/z2R;
                for (i = positionz1R + 1; i < positionz2R; i++) {
                    if ((histogramR[i]*1.0/z2R) < z) {
                        z = histogramR[i]*1.0/z2R;
                        thresholdR = i;
                    }
                }
            } else {
                z = histogramR[positionz2R + 1]*1.0/z1R;
                for (i = positionz2R + 1; i < positionz1R; i++) {
                    if ((histogramR[i]*1.0/z1R) < z) {
                        z = histogramR[i]*1.0/z1R;
                        thresholdR = i;
                    }
                }
            }

            if (positionz1G < positionz2G) {
                z = histogramG[positionz1G + 1]*1.0/z2G;
                for (i = positionz1G + 1; i < positionz2G; i++) {
                    if ((histogramG[i]*1.0/z2G) < z) {
                        z = histogramG[i]*1.0/z2G;
                        thresholdG = i;
                    }
                }
            } else {
                z = histogramG[positionz2G + 1]*1.0/z1G;
                for (i = positionz2G + 1; i < positionz1G; i++) {
                    if ((histogramG[i]*1.0/z1G) < z) {
                        z = histogramG[i]*1.0/z1G;
                        thresholdG = i;
                    }
                }
            }

            if (positionz1B < positionz2B) {
                z = histogramB[positionz1B + 1]*1.0/z2B;
                for (i = positionz1B + 1; i < positionz2B; i++) {
                    if ((histogramB[i]*1.0/z2B) < z) {
                        z = histogramB[i]*1.0/z2B;
                        thresholdB = i;
                    }
                }
            } else {
                z = histogramB[positionz2B + 1]*1.0/z1B;
                for (i = positionz2B + 1; i < positionz1B; i++) {
                    if ((histogramB[i]*1.0/z1B) < z) {
                        z = histogramB[i]*1.0/z1B;
                        thresholdB = i;
                    }
                }
            }

            for (i = 0; i < m_data.M_bitmap.Width; i++) {
                for (j = 0; j < m_data.M_bitmap.Height; j++) {
                    int index = (j*bmpData.Stride) + (i*3);

                    r = rgbValues[index + 2];
                    g = rgbValues[index + 1];
                    b = rgbValues[index];

                    r = r < thresholdR ? 0 : 255;
                    g = g < thresholdG ? 0 : 255;
                    b = b < thresholdB ? 0 : 255;

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
            m_data.M_bitmap.UnlockBits(bmpData);

            m_data.M_bitmapBind = m_data.M_bitmap.BitmapToBitmapSource();

            string messageOperation = "Done!\r\n\r\nThreshold (RED) set at: " + thresholdR + "\r\n" + "Threshold (GREEN) set at: " + thresholdG + "\r\n" + "Threshold (BLUE) set at: " + thresholdB + "\r\n\r\n" + "Elapsed time (HH:MM:SS.MS): " + elapsedTime;
            MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK) {
                m_data.M_noChange = false;
                m_data.M_action = ActionType.AutoThreshold;
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