﻿/*
Basic image processing software
<https://github.com/nlabiris/ImageEdit_WPF>

Copyright (C) 2015  Nikos Labiris

This file is only for future reference.

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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ImageAlgorithms.Algorithms {
    #region algorithms backup (do not use, only for reference) (31/1/2016)
    /// <summary>
    /// This class contains older implementations of algorithms. It is not meant for use in the program.
    /// Newer implementations are faster and with less bugs (if any).
    /// </summary>
    internal static class AlgorithmsBackup {
        #region Shift bits
        private static TimeSpan ShiftBits(ImageData data, int bits, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            for (i = 0; i < data.M_width; i++) {
                backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);
                    rgbValues[index + 2] = (byte)(rgbValues[index + 2] << bits); // R
                    rgbValues[index + 1] = (byte)(rgbValues[index + 1] << bits); // G
                    rgbValues[index] = (byte)(rgbValues[index] << bits); // B
                }
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Threshold
        private static TimeSpan Threshold(ImageData data, int threshold, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int r = 0;
            int g = 0;
            int b = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            for (i = 0; i < data.M_width; i++) {
                backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

                    r = rgbValues[index + 2];
                    g = rgbValues[index + 1];
                    b = rgbValues[index];

                    r = r < threshold ? 0 : 255;
                    g = g < threshold ? 0 : 255;
                    b = b < threshold ? 0 : 255;

                    rgbValues[index + 2] = (byte)r;
                    rgbValues[index + 1] = (byte)g;
                    rgbValues[index] = (byte)b;
                }
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Auto threshold
        private static TimeSpan AutoThreshold(ImageData data, int distance, BackgroundWorker backgroundWorker) {
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
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
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
            backgroundWorker.ReportProgress(20);

            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

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
            backgroundWorker.ReportProgress(40);

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
            backgroundWorker.ReportProgress(60);

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
            backgroundWorker.ReportProgress(70);

            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

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
            backgroundWorker.ReportProgress(100);
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Negative
        private static TimeSpan Negative(ImageData data, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            for (i = 0; i < data.M_width; i++) {
                backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);
                    rgbValues[index + 2] = (byte)(255 - rgbValues[index + 2]); // R
                    rgbValues[index + 1] = (byte)(255 - rgbValues[index + 1]); // G
                    rgbValues[index] = (byte)(255 - rgbValues[index]); // B
                }
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Square root
        private static TimeSpan SquareRoot(ImageData data, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            for (i = 0; i < data.M_width; i++) {
                backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);
                    rgbValues[index + 2] = (byte)Math.Sqrt(rgbValues[index + 2]*255); // R
                    rgbValues[index + 1] = (byte)Math.Sqrt(rgbValues[index + 1]*255); // G
                    rgbValues[index] = (byte)Math.Sqrt(rgbValues[index]*255); // B
                }
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Contrast enhancement
        private static TimeSpan ContrastEnhancement(ImageData data, int brightness, double contrast, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            double r = 0;
            double g = 0;
            double b = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            for (i = 0; i < data.M_width; i++) {
                backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);
                    r = (rgbValues[index + 2] + brightness)*contrast;
                    g = (rgbValues[index + 1] + brightness)*contrast;
                    b = (rgbValues[index] + brightness)*contrast;

                    if (r > 255.0) {
                        r = 255.0;
                    } else if (r < 0.0) {
                        r = 0.0;
                    }

                    if (g > 255.0) {
                        g = 255.0;
                    } else if (g < 0.0) {
                        g = 0.0;
                    }

                    if (b > 255.0) {
                        b = 255.0;
                    } else if (b < 0.0) {
                        b = 0.0;
                    }

                    rgbValues[index + 2] = (byte)r;
                    rgbValues[index + 1] = (byte)g;
                    rgbValues[index] = (byte)b;
                }
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Brightness
        private static TimeSpan Brightness(ImageData data, int brightness, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int r = 0;
            int g = 0;
            int b = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            for (i = 0; i < data.M_width; i++) {
                backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

                    r = rgbValues[index + 2] + brightness;
                    g = rgbValues[index + 1] + brightness;
                    b = rgbValues[index] + brightness;

                    if (r > 255) {
                        r = 255;
                    } else if (r < 0) {
                        r = 0;
                    }

                    if (g > 255) {
                        g = 255;
                    } else if (g < 0) {
                        g = 0;
                    }

                    if (b > 255) {
                        b = 255;
                    } else if (b < 0) {
                        b = 0;
                    }

                    rgbValues[index + 2] = (byte)r;
                    rgbValues[index + 1] = (byte)g;
                    rgbValues[index] = (byte)b;
                }
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Contrast
        private static TimeSpan Contrast(ImageData data, double contrast, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            double r = 0;
            double g = 0;
            double b = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithms
            for (i = 0; i < data.M_width; i++) {
                backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

                    r = rgbValues[index + 2]*contrast;
                    g = rgbValues[index + 1]*contrast;
                    b = rgbValues[index]*contrast;

                    if (r > 255.0) {
                        r = 255.0;
                    } else if (r < 0.0) {
                        r = 0.0;
                    }

                    if (g > 255.0) {
                        g = 255.0;
                    } else if (g < 0.0) {
                        g = 0.0;
                    }

                    if (b > 255.0) {
                        b = 255.0;
                    } else if (b < 0.0) {
                        b = 0.0;
                    }

                    rgbValues[index + 2] = (byte)r;
                    rgbValues[index + 1] = (byte)g;
                    rgbValues[index] = (byte)b;
                }
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Histogram for RGBY channels
        /// <summary>
        /// Calculate the average value of histogram.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="values">Histogram values.</param>
        /// <returns>Returns the average value.</returns>
        private static float HistogramMeanValue(ImageData data, int[] values) {
            int i = 0;
            float mean = 0;
            float histogramSum = 0;

            for (i = 0; i < 256; i++) {
                histogramSum = histogramSum + (i*values[i]);
            }
            mean = histogramSum/(float)(data.M_width*data.M_height);

            return mean;
        }

        /// <summary>
        /// Calculating the histogram of the red channel.
        /// </summary>
        /// <returns>
        /// Histogram of the red channel.
        /// </returns>
        private static int[] HistogramRed(ImageData data) {
            int[] histogramR = new int[256];
            int r = 0;
            int i = 0;
            int j = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (i = 0; i < 256; i++) {
                histogramR[i] = 0;
            }

            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

                    r = rgbValues[index + 2];
                    histogramR[r]++;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return histogramR;
        }

        /// <summary>
        /// Calculating the histogram of the green channel.
        /// </summary>
        /// <returns>
        /// Histogram of the green channel.
        /// </returns>
        private static int[] HistogramGreen(ImageData data) {
            int[] histogramG = new int[256];
            int g = 0;
            int i = 0;
            int j = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (i = 0; i < 256; i++) {
                histogramG[i] = 0;
            }

            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

                    g = rgbValues[index + 1];
                    histogramG[g]++;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return histogramG;
        }

        /// <summary>
        /// Calculating the histogram of the blue channel.
        /// </summary>
        /// <returns>
        /// Histogram of the blue channel.
        /// </returns>
        private static int[] HistogramBlue(ImageData data) {
            int[] histogramB = new int[256];
            int b = 0;
            int i = 0;
            int j = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (i = 0; i < 256; i++) {
                histogramB[i] = 0;
            }

            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

                    b = rgbValues[index];
                    histogramB[b]++;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return histogramB;
        }

        /// <summary>
        /// Calculating the histogram for the luminance values.
        /// </summary>
        /// <returns>
        /// Histogram of the luminance values.
        /// </returns>
        private static int[] HistogramLuminance(ImageData data) {
            int[] histogramY = new int[256];
            int r = 0;
            int g = 0;
            int b = 0;
            int y = 0;
            int i = 0;
            int j = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (i = 0; i < 256; i++) {
                histogramY[i] = 0;
            }

            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

                    r = rgbValues[index + 2];
                    g = rgbValues[index + 1];
                    b = rgbValues[index];

                    y = (int)(0.2126*r + 0.7152*g + 0.0722*b); // source = https://en.wikipedia.org/wiki/Grayscale#cite_note-5

                    histogramY[y]++;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return histogramY;
        }
        #endregion

        #region Histogram equalization [RGB]
        private static TimeSpan HistogramEqualization_RGB(ImageData data, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int b = 0;
            int g = 0;
            int r = 0;
            double[] possibilityR = new double[256];
            double[] possibilityG = new double[256];
            double[] possibilityB = new double[256];
            int[] histogramR = new int[256];
            int[] histogramG = new int[256];
            int[] histogramB = new int[256];
            double[] histogramEqR = new double[256];
            double[] histogramEqG = new double[256];
            double[] histogramEqB = new double[256];
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            for (i = 0; i < 256; i++) {
                histogramR[i] = 0;
                histogramG[i] = 0;
                histogramB[i] = 0;
            }
            backgroundWorker.ReportProgress(10);

            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

                    b = rgbValues[index + 2];
                    histogramB[b]++;
                    g = rgbValues[index + 1];
                    histogramG[g]++;
                    r = rgbValues[index];
                    histogramR[r]++;
                }
            }
            backgroundWorker.ReportProgress(40);

            for (i = 0; i < 256; i++) {
                possibilityB[i] = histogramB[i]/(double)(data.M_bitmap.Width*data.M_bitmap.Height);
                possibilityG[i] = histogramG[i]/(double)(data.M_bitmap.Width*data.M_bitmap.Height);
                possibilityR[i] = histogramR[i]/(double)(data.M_bitmap.Width*data.M_bitmap.Height);
            }
            backgroundWorker.ReportProgress(60);

            histogramEqB[0] = possibilityB[0];
            histogramEqG[0] = possibilityG[0];
            histogramEqR[0] = possibilityR[0];
            for (i = 1; i < 256; i++) {
                histogramEqB[i] = histogramEqB[i - 1] + possibilityB[i];
                histogramEqG[i] = histogramEqG[i - 1] + possibilityG[i];
                histogramEqR[i] = histogramEqR[i - 1] + possibilityR[i];
            }
            backgroundWorker.ReportProgress(70);

            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

                    b = rgbValues[index + 2];
                    b = (int)Math.Round(histogramEqB[b]*255);
                    g = rgbValues[index + 1];
                    g = (int)Math.Round(histogramEqG[g]*255);
                    r = rgbValues[index];
                    r = (int)Math.Round(histogramEqR[r]*255);

                    if (b > 255) {
                        b = 255;
                    } else if (b < 0) {
                        b = 0;
                    }

                    if (g > 255) {
                        g = 255;
                    } else if (g < 0) {
                        g = 0;
                    }

                    if (r > 255) {
                        r = 255;
                    } else if (r < 0) {
                        r = 0;
                    }

                    rgbValues[index + 2] = (byte)b;
                    rgbValues[index + 1] = (byte)g;
                    rgbValues[index] = (byte)r;
                }
            }
            backgroundWorker.ReportProgress(100);
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Histogram equalization [HSV]
        private static TimeSpan HistogramEqualization_HSV(ImageData data, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int k = 0;
            double max = 0.0;
            double min = 0.0;
            double chroma = 0.0;
            double c = 0.0;
            double x = 0.0;
            double m = 0.0;
            int[] histogramV = new int[256];
            double[] sumHistogramEqualizationV = new double[256];
            double[] sumHistogramV = new double[256];
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];
            double[] red = new double[bytes];
            double[] green = new double[bytes];
            double[] blue = new double[bytes];
            double[] hue = new double[bytes];
            double[] value = new double[bytes];
            double[] saturation = new double[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            for (i = 0; i < 256; i++) {
                histogramV[i] = 0;
                sumHistogramEqualizationV[i] = 0.0;
            }
            backgroundWorker.ReportProgress(10);

            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (bmpData.Stride*j) + (i*3);

                    blue[index] = rgbValues[index];
                    blue[index] = blue[index]/255.0;
                    green[index + 1] = rgbValues[index + 1];
                    green[index + 1] = green[index + 1]/255.0;
                    red[index + 2] = rgbValues[index + 2];
                    red[index + 2] = red[index + 2]/255.0;

                    min = Math.Min(red[index + 2], Math.Min(green[index + 1], blue[index]));
                    max = Math.Max(red[index + 2], Math.Max(green[index + 1], blue[index]));
                    chroma = max - min;
                    hue[index] = 0.0;
                    saturation[index + 1] = 0.0;
                    if (chroma != 0.0) {
                        if (Math.Abs(red[index + 2] - max) < 0.00001) {
                            hue[index] = ((green[index + 1] - blue[index])/chroma);
                            hue[index] = hue[index]%6.0;
                        } else if (Math.Abs(green[index + 1] - max) < 0.00001) {
                            hue[index] = ((blue[index] - red[index + 2])/chroma) + 2;
                        } else {
                            hue[index] = ((red[index + 2] - green[index + 1])/chroma) + 4;
                        }

                        hue[index] = hue[index]*60.0;
                        if (hue[index] < 0.0) {
                            hue[index] = hue[index] + 360.0;
                        }
                        saturation[index + 1] = chroma/max;
                    }
                    value[index + 2] = max;

                    value[index + 2] = value[index + 2]*255.0;

                    if (value[index + 2] > 255.0) {
                        value[index + 2] = 255.0;
                    }
                    if (value[index + 2] < 0.0) {
                        value[index + 2] = 0.0;
                    }

                    k = (int)value[index + 2];
                    histogramV[k]++;
                }
            }
            backgroundWorker.ReportProgress(40);

            for (i = 0; i < 256; i++) {
                sumHistogramEqualizationV[i] = histogramV[i]/(double)(data.M_bitmap.Width*data.M_bitmap.Height);
            }
            backgroundWorker.ReportProgress(60);

            sumHistogramV[0] = sumHistogramEqualizationV[0];
            for (i = 1; i < 256; i++) {
                sumHistogramV[i] = sumHistogramV[i - 1] + sumHistogramEqualizationV[i];
            }
            backgroundWorker.ReportProgress(70);

            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (bmpData.Stride*j) + (i*3);

                    k = (int)value[index + 2];
                    value[index + 2] = (byte)Math.Round(sumHistogramV[k]*255.0);
                    value[index + 2] = value[index + 2]/255;

                    c = value[index + 2]*saturation[index + 1];
                    hue[index] = hue[index]/60.0;
                    hue[index] = hue[index]%2;
                    x = c*(1.0 - Math.Abs(hue[index] - 1.0));
                    m = value[index + 2] - c;

                    if (hue[index] >= 0.0 && hue[index] < 60.0) {
                        red[index + 2] = c;
                        green[index + 1] = x;
                        blue[index] = 0;
                    } else if (hue[index] >= 60.0 && hue[index] < 120.0) {
                        red[index + 2] = x;
                        green[index + 1] = c;
                        blue[index] = 0;
                    } else if (hue[index] >= 120.0 && hue[index] < 180.0) {
                        red[index + 2] = 0;
                        green[index + 1] = c;
                        blue[index] = x;
                    } else if (hue[index] >= 180.0 && hue[index] < 240.0) {
                        red[index + 2] = 0;
                        green[index + 1] = x;
                        blue[index] = c;
                    } else if (hue[index] >= 240.0 && hue[index] < 300.0) {
                        red[index + 2] = x;
                        green[index + 1] = 0;
                        blue[index] = c;
                    } else if (hue[index] >= 300.0 && hue[index] < 360.0) {
                        red[index + 2] = c;
                        green[index + 1] = 0;
                        blue[index] = x;
                    }

                    red[index + 2] = red[index + 2] + m;
                    green[index + 1] = green[index + 1] + m;
                    blue[index] = blue[index] + m;

                    red[index + 2] = red[index + 2]*255.0;
                    green[index + 1] = green[index + 1]*255.0;
                    blue[index] = blue[index]*255.0;

                    if (red[index + 2] > 255.0) {
                        red[index + 2] = 255.0;
                    }

                    if (red[index + 2] < 0.0) {
                        red[index + 2] = 0.0;
                    }

                    if (green[index + 1] > 255.0) {
                        green[index + 1] = 255.0;
                    }

                    if (green[index + 1] < 0.0) {
                        green[index + 1] = 0.0;
                    }

                    if (blue[index] > 255.0) {
                        blue[index] = 255.0;
                    }

                    if (blue[index] < 0.0) {
                        blue[index] = 0.0;
                    }

                    rgbValues[index + 2] = (byte)red[index + 2];
                    rgbValues[index + 1] = (byte)green[index + 1];
                    rgbValues[index] = (byte)blue[index];
                }
            }
            backgroundWorker.ReportProgress(100);
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Histogram equalization [YUV]
        private static TimeSpan HistogramEqualization_YUV(ImageData data, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int k = 0;
            int[] histogramY = new int[256];
            double[] sumHistogramEqualizationY = new double[256];
            double[] sumHistogramY = new double[256];
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];
            double[] red = new double[bytes];
            double[] green = new double[bytes];
            double[] blue = new double[bytes];
            double[] luminanceY = new double[bytes];
            double[] chrominanceU = new double[bytes];
            double[] chrominanceV = new double[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            for (i = 0; i < 256; i++) {
                histogramY[i] = 0;
            }
            backgroundWorker.ReportProgress(10);

            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (bmpData.Stride*j) + (i*3);

                    blue[index] = rgbValues[index];
                    blue[index] = blue[index]/255.0;
                    green[index + 1] = rgbValues[index + 1];
                    green[index + 1] = green[index + 1]/255.0;
                    red[index + 2] = rgbValues[index + 2];
                    red[index + 2] = red[index + 2]/255.0;

                    luminanceY[index] = (0.299*red[index + 2]) + (0.587*green[index + 1]) + (0.114*blue[index]);
                    chrominanceU[index + 1] = (-0.14713*red[index + 2]) - (0.28886*green[index + 1]) + (0.436*blue[index]);
                    chrominanceV[index + 2] = (0.615*red[index + 2]) - (0.51499*green[index + 1]) - (0.10001*blue[index]);

                    luminanceY[index] = luminanceY[index]*255.0;
                    if (luminanceY[index] > 255.0) {
                        luminanceY[index] = 255.0;
                    }

                    if (luminanceY[index] < 0.0) {
                        luminanceY[index] = 0.0;
                    }

                    k = (int)luminanceY[index];
                    histogramY[k]++;
                }
            }
            backgroundWorker.ReportProgress(40);

            for (i = 0; i < 256; i++) {
                sumHistogramEqualizationY[i] = 0.0;
            }
            backgroundWorker.ReportProgress(50);

            for (i = 0; i < 256; i++) {
                sumHistogramEqualizationY[i] = histogramY[i]/(double)(data.M_bitmap.Width*data.M_bitmap.Height);
            }
            backgroundWorker.ReportProgress(60);

            sumHistogramY[0] = sumHistogramEqualizationY[0];
            for (i = 1; i < 256; i++) {
                sumHistogramY[i] = sumHistogramY[i - 1] + sumHistogramEqualizationY[i];
            }
            backgroundWorker.ReportProgress(70);

            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (bmpData.Stride*j) + (i*3);

                    k = (int)luminanceY[index];
                    luminanceY[index] = (byte)Math.Round(sumHistogramY[k]*255.0);
                    luminanceY[index] = luminanceY[index]/255;

                    red[index + 2] = luminanceY[index] + (0.0*chrominanceU[index + 1]) + (1.13983*chrominanceV[index + 2]);
                    green[index + 1] = luminanceY[index] + (-0.39465*chrominanceU[index + 1]) + (-0.58060*chrominanceV[index + 2]);
                    blue[index] = luminanceY[index] + (2.03211*chrominanceU[index + 1]) + (0.0*chrominanceV[index + 2]);

                    red[index + 2] = red[index + 2]*255.0;
                    green[index + 1] = green[index + 1]*255.0;
                    blue[index] = blue[index]*255.0;

                    if (red[index + 2] > 255.0) {
                        red[index + 2] = 255.0;
                    }

                    if (red[index + 2] < 0.0) {
                        red[index + 2] = 0.0;
                    }

                    if (green[index + 1] > 255.0) {
                        green[index + 1] = 255.0;
                    }

                    if (green[index + 1] < 0.0) {
                        green[index + 1] = 0.0;
                    }

                    if (blue[index] > 255.0) {
                        blue[index] = 255.0;
                    }

                    if (blue[index] < 0.0) {
                        blue[index] = 0.0;
                    }

                    rgbValues[index + 2] = (byte)red[index + 2];
                    rgbValues[index + 1] = (byte)green[index + 1];
                    rgbValues[index] = (byte)blue[index];
                }
            }
            backgroundWorker.ReportProgress(100);
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Image summarization
        private static TimeSpan ImageSummarization(ImageData data, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int b = 0;
            int g = 0;
            int r = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            for (i = 0; i < data.M_width; i++) {
                backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);
                    r = rgbValues[index + 2] + rgbValues[index + 2]; // R
                    g = rgbValues[index + 1] + rgbValues[index + 1]; // G
                    b = rgbValues[index] + rgbValues[index]; // B

                    if (r > 255) {
                        r = 255;
                    }

                    if (g > 255) {
                        g = 255;
                    }

                    if (b > 255) {
                        b = 255;
                    }

                    rgbValues[index + 2] = (byte)r; // R
                    rgbValues[index + 1] = (byte)g; // G
                    rgbValues[index] = (byte)b; // B
                }
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Image subtraction
        private static TimeSpan ImageSubtraction(ImageData data, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int b = 0;
            int g = 0;
            int r = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            for (i = 0; i < data.M_width; i++) {
                backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

                    r = rgbValues[index + 2] - rgbValues[index + 2]; // R
                    g = rgbValues[index + 1] - rgbValues[index + 1]; // G
                    b = rgbValues[index] - rgbValues[index]; // B

                    rgbValues[index + 2] = (byte)r; // R
                    rgbValues[index + 1] = (byte)g; // G
                    rgbValues[index] = (byte)b; // B
                }
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Edge detection [Sobel]
        private static TimeSpan EdgeDetection_Sobel(ImageData data, int sizeMask, int[,] maskX, int[,] maskY, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            double tR = 0.0;
            double tG = 0.0;
            double tB = 0.0;
            int txR = 0;
            int txG = 0;
            int txB = 0;
            int tyR = 0;
            int tyG = 0;
            int tyB = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] bgrValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            switch (sizeMask) {
                case 3:
                    for (i = 1; i < data.M_width - 1; i++) {
                        backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                        for (j = 1; j < data.M_height - 1; j++) {
                            txR = 0;
                            txG = 0;
                            txB = 0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 1)*bmpData.Stride) + ((i + k - 1)*3);
                                    txR = txR + rgbValues[index + 2]*maskX[k, l];
                                    txG = txG + rgbValues[index + 1]*maskX[k, l];
                                    txB = txB + rgbValues[index]*maskX[k, l];
                                }
                            }

                            tyR = 0;
                            tyG = 0;
                            tyB = 0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 1)*bmpData.Stride) + ((i + k - 1)*3);
                                    tyR = tyR + rgbValues[index + 2]*maskY[k, l];
                                    tyG = tyG + rgbValues[index + 1]*maskY[k, l];
                                    tyB = tyB + rgbValues[index]*maskY[k, l];
                                }
                            }

                            tR = Math.Sqrt(txR*txR + tyR*tyR);
                            tG = Math.Sqrt(txG*txG + tyG*tyG);
                            tB = Math.Sqrt(txB*txB + tyB*tyB);

                            if (tR > 255.0) {
                                tR = 255.0;
                            } else if (tR < 0.0) {
                                tR = 0.0;
                            }

                            if (tG > 255.0) {
                                tG = 255.0;
                            } else if (tG < 0.0) {
                                tG = 0.0;
                            }

                            if (tB > 255.0) {
                                tB = 255.0;
                            } else if (tB < 0.0) {
                                tB = 0.0;
                            }

                            index = (j*bmpData.Stride) + (i*3);

                            bgrValues[index + 2] = (byte)tR;
                            bgrValues[index + 1] = (byte)tG;
                            bgrValues[index] = (byte)tB;
                        }
                    }
                    break;
                case 5:
                    for (i = 2; i < data.M_width - 2; i++) {
                        backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                        for (j = 2; j < data.M_height - 2; j++) {
                            txR = 0;
                            txG = 0;
                            txB = 0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 2)*bmpData.Stride) + ((i + k - 2)*3);
                                    txR = txR + rgbValues[index + 2]*maskX[k, l];
                                    txG = txG + rgbValues[index + 1]*maskX[k, l];
                                    txB = txB + rgbValues[index]*maskX[k, l];
                                }
                            }

                            tyR = 0;
                            tyG = 0;
                            tyB = 0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 2)*bmpData.Stride) + ((i + k - 2)*3);
                                    tyR = tyR + rgbValues[index + 2]*maskY[k, l];
                                    tyG = tyG + rgbValues[index + 1]*maskY[k, l];
                                    tyB = tyB + rgbValues[index]*maskY[k, l];
                                }
                            }

                            tR = Math.Sqrt(txR*txR + tyR*tyR);
                            tG = Math.Sqrt(txG*txG + tyG*tyG);
                            tB = Math.Sqrt(txB*txB + tyB*tyB);

                            if (tR > 255.0) {
                                tR = 255.0;
                            } else if (tR < 0.0) {
                                tR = 0.0;
                            }

                            if (tG > 255.0) {
                                tG = 255.0;
                            } else if (tG < 0.0) {
                                tG = 0.0;
                            }

                            if (tB > 255.0) {
                                tB = 255.0;
                            } else if (tB < 0.0) {
                                tB = 0.0;
                            }

                            index = (j*bmpData.Stride) + (i*3);

                            bgrValues[index + 2] = (byte)tR;
                            bgrValues[index + 1] = (byte)tG;
                            bgrValues[index] = (byte)tB;
                        }
                    }
                    break;
                case 7:
                    for (i = 3; i < data.M_width - 3; i++) {
                        backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                        for (j = 3; j < data.M_height - 3; j++) {
                            txR = 0;
                            txG = 0;
                            txB = 0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 3)*bmpData.Stride) + ((i + k - 3)*3);
                                    txR = txR + rgbValues[index + 2]*maskX[k, l];
                                    txG = txG + rgbValues[index + 1]*maskX[k, l];
                                    txB = txB + rgbValues[index]*maskX[k, l];
                                }
                            }

                            tyR = 0;
                            tyG = 0;
                            tyB = 0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 3)*bmpData.Stride) + ((i + k - 3)*3);
                                    tyR = tyR + rgbValues[index + 2]*maskY[k, l];
                                    tyG = tyG + rgbValues[index + 1]*maskY[k, l];
                                    tyB = tyB + rgbValues[index]*maskY[k, l];
                                }
                            }

                            tR = Math.Sqrt(txR*txR + tyR*tyR);
                            tG = Math.Sqrt(txG*txG + tyG*tyG);
                            tB = Math.Sqrt(txB*txB + tyB*tyB);

                            if (tR > 255.0) {
                                tR = 255.0;
                            } else if (tR < 0.0) {
                                tR = 0.0;
                            }

                            if (tG > 255.0) {
                                tG = 255.0;
                            } else if (tG < 0.0) {
                                tG = 0.0;
                            }

                            if (tB > 255.0) {
                                tB = 255.0;
                            } else if (tB < 0.0) {
                                tB = 0.0;
                            }

                            index = (j*bmpData.Stride) + (i*3);

                            bgrValues[index + 2] = (byte)tR;
                            bgrValues[index + 1] = (byte)tG;
                            bgrValues[index] = (byte)tB;
                        }
                    }
                    break;
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

                    rgbValues[index + 2] = bgrValues[index + 2];
                    rgbValues[index + 1] = bgrValues[index + 1];
                    rgbValues[index] = bgrValues[index];
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Gaussian blur
        private static TimeSpan GaussianBlur(ImageData data, int sizeMask, int[,] maskX, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            double tR = 0.0;
            double tG = 0.0;
            double tB = 0.0;
            int sumMask = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] bgrValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            switch (sizeMask) {
                case 3:
                    for (i = 0; i < sizeMask; i++) {
                        for (j = 0; j < sizeMask; j++) {
                            sumMask += maskX[i, j];
                        }
                    }

                    for (i = 1; i < data.M_width - 1; i++) {
                        backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                        for (j = 1; j < data.M_height - 1; j++) {
                            tR = 0.0;
                            tG = 0.0;
                            tB = 0.0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 1)*bmpData.Stride) + ((i + k - 1)*3);
                                    tR = tR + (rgbValues[index + 2]*maskX[k, l])/sumMask;
                                    tG = tG + (rgbValues[index + 1]*maskX[k, l])/sumMask;
                                    tB = tB + (rgbValues[index]*maskX[k, l])/sumMask;
                                }
                            }

                            if (tR > 255.0) {
                                tR = 255.0;
                            } else if (tR < 0.0) {
                                tR = 0.0;
                            }

                            if (tG > 255.0) {
                                tG = 255.0;
                            } else if (tG < 0.0) {
                                tG = 0.0;
                            }

                            if (tB > 255.0) {
                                tB = 255.0;
                            } else if (tB < 0.0) {
                                tB = 0.0;
                            }

                            index = (j*bmpData.Stride) + (i*3);

                            bgrValues[index + 2] = (byte)tR;
                            bgrValues[index + 1] = (byte)tG;
                            bgrValues[index] = (byte)tB;
                        }
                    }
                    break;
                case 5:
                    for (i = 0; i < sizeMask; i++) {
                        for (j = 0; j < sizeMask; j++) {
                            sumMask += maskX[i, j];
                        }
                    }

                    for (i = 2; i < data.M_width - 2; i++) {
                        backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                        for (j = 2; j < data.M_height - 2; j++) {
                            tR = 0.0;
                            tG = 0.0;
                            tB = 0.0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 2)*bmpData.Stride) + ((i + k - 2)*3);
                                    tR = tR + (rgbValues[index + 2]*maskX[k, l])/sumMask;
                                    tG = tG + (rgbValues[index + 1]*maskX[k, l])/sumMask;
                                    tB = tB + (rgbValues[index]*maskX[k, l])/sumMask;
                                }
                            }

                            if (tR > 255.0) {
                                tR = 255.0;
                            } else if (tR < 0.0) {
                                tR = 0.0;
                            }

                            if (tG > 255.0) {
                                tG = 255.0;
                            } else if (tG < 0.0) {
                                tG = 0.0;
                            }

                            if (tB > 255.0) {
                                tB = 255.0;
                            } else if (tB < 0.0) {
                                tB = 0.0;
                            }

                            index = (j*bmpData.Stride) + (i*3);

                            bgrValues[index + 2] = (byte)tR;
                            bgrValues[index + 1] = (byte)tG;
                            bgrValues[index] = (byte)tB;
                        }
                    }
                    break;
                case 7:
                    for (i = 0; i < sizeMask; i++) {
                        for (j = 0; j < sizeMask; j++) {
                            sumMask += maskX[i, j];
                        }
                    }

                    for (i = 3; i < data.M_width - 3; i++) {
                        backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                        for (j = 3; j < data.M_height - 3; j++) {
                            tR = 0.0;
                            tG = 0.0;
                            tB = 0.0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 3)*bmpData.Stride) + ((i + k - 3)*3);
                                    tR = tR + (rgbValues[index + 2]*maskX[k, l])/sumMask;
                                    tG = tG + (rgbValues[index + 1]*maskX[k, l])/sumMask;
                                    tB = tB + (rgbValues[index]*maskX[k, l])/sumMask;
                                }
                            }

                            if (tR > 255.0) {
                                tR = 255.0;
                            } else if (tR < 0.0) {
                                tR = 0.0;
                            }

                            if (tG > 255.0) {
                                tG = 255.0;
                            } else if (tG < 0.0) {
                                tG = 0.0;
                            }

                            if (tB > 255.0) {
                                tB = 255.0;
                            } else if (tB < 0.0) {
                                tB = 0.0;
                            }

                            index = (j*bmpData.Stride) + (i*3);

                            bgrValues[index + 2] = (byte)tR;
                            bgrValues[index + 1] = (byte)tG;
                            bgrValues[index] = (byte)tB;
                        }
                    }
                    break;
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

                    rgbValues[index + 2] = bgrValues[index + 2];
                    rgbValues[index + 1] = bgrValues[index + 1];
                    rgbValues[index] = bgrValues[index];
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Sharpen
        private static TimeSpan Sharpen(ImageData data, int sizeMask, int[,] maskX, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            double tR = 0.0;
            double tG = 0.0;
            double tB = 0.0;
            int sumMask = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] bgrValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            switch (sizeMask) {
                case 3:
                    for (i = 0; i < sizeMask; i++) {
                        for (j = 0; j < sizeMask; j++) {
                            sumMask += maskX[i, j];
                        }
                    }

                    for (i = 1; i < data.M_width - 1; i++) {
                        backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                        for (j = 1; j < data.M_height - 1; j++) {
                            tR = 0.0;
                            tG = 0.0;
                            tB = 0.0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 1)*bmpData.Stride) + ((i + k - 1)*3);
                                    tR = tR + (rgbValues[index + 2]*maskX[k, l])/sumMask;
                                    tG = tG + (rgbValues[index + 1]*maskX[k, l])/sumMask;
                                    tB = tB + (rgbValues[index]*maskX[k, l])/sumMask;
                                }
                            }

                            if (tR > 255.0) {
                                tR = 255.0;
                            } else if (tR < 0.0) {
                                tR = 0.0;
                            }

                            if (tG > 255.0) {
                                tG = 255.0;
                            } else if (tG < 0.0) {
                                tG = 0.0;
                            }

                            if (tB > 255.0) {
                                tB = 255.0;
                            } else if (tB < 0.0) {
                                tB = 0.0;
                            }

                            index = (j*bmpData.Stride) + (i*3);

                            bgrValues[index + 2] = (byte)tR;
                            bgrValues[index + 1] = (byte)tG;
                            bgrValues[index] = (byte)tB;
                        }
                    }
                    break;
                case 5:
                    for (i = 0; i < sizeMask; i++) {
                        for (j = 0; j < sizeMask; j++) {
                            sumMask += maskX[i, j];
                        }
                    }

                    for (i = 2; i < data.M_width - 2; i++) {
                        backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                        for (j = 2; j < data.M_height - 2; j++) {
                            tR = 0.0;
                            tG = 0.0;
                            tB = 0.0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 2)*bmpData.Stride) + ((i + k - 2)*3);
                                    tR = tR + (rgbValues[index + 2]*maskX[k, l])/sumMask;
                                    tG = tG + (rgbValues[index + 1]*maskX[k, l])/sumMask;
                                    tB = tB + (rgbValues[index]*maskX[k, l])/sumMask;
                                }
                            }

                            if (tR > 255.0) {
                                tR = 255.0;
                            } else if (tR < 0.0) {
                                tR = 0.0;
                            }

                            if (tG > 255.0) {
                                tG = 255.0;
                            } else if (tG < 0.0) {
                                tG = 0.0;
                            }

                            if (tB > 255.0) {
                                tB = 255.0;
                            } else if (tB < 0.0) {
                                tB = 0.0;
                            }

                            index = (j*bmpData.Stride) + (i*3);

                            bgrValues[index + 2] = (byte)tR;
                            bgrValues[index + 1] = (byte)tG;
                            bgrValues[index] = (byte)tB;
                        }
                    }
                    break;
                case 7:
                    for (i = 0; i < sizeMask; i++) {
                        for (j = 0; j < sizeMask; j++) {
                            sumMask += maskX[i, j];
                        }
                    }

                    for (i = 3; i < data.M_width - 3; i++) {
                        backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                        for (j = 3; j < data.M_height - 3; j++) {
                            tR = 0.0;
                            tG = 0.0;
                            tB = 0.0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 3)*bmpData.Stride) + ((i + k - 3)*3);
                                    tR = tR + (rgbValues[index + 2]*maskX[k, l])/sumMask;
                                    tG = tG + (rgbValues[index + 1]*maskX[k, l])/sumMask;
                                    tB = tB + (rgbValues[index]*maskX[k, l])/sumMask;
                                }
                            }

                            if (tR > 255.0) {
                                tR = 255.0;
                            } else if (tR < 0.0) {
                                tR = 0.0;
                            }

                            if (tG > 255.0) {
                                tG = 255.0;
                            } else if (tG < 0.0) {
                                tG = 0.0;
                            }

                            if (tB > 255.0) {
                                tB = 255.0;
                            } else if (tB < 0.0) {
                                tB = 0.0;
                            }

                            index = (j*bmpData.Stride) + (i*3);

                            bgrValues[index + 2] = (byte)tR;
                            bgrValues[index + 1] = (byte)tG;
                            bgrValues[index] = (byte)tB;
                        }
                    }
                    break;
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

                    rgbValues[index + 2] = bgrValues[index + 2];
                    rgbValues[index + 1] = bgrValues[index + 1];
                    rgbValues[index] = bgrValues[index];
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Salt and Pepper Noise generator [Color]
        private static TimeSpan SaltPepperNoise_Color(ImageData data, double probability, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int d = 0;
            int d1 = 0;
            int d2 = 0;
            int index = 0;
            Random rand = new Random();

            d = (int)(probability*32768/2);
            d1 = d + 16384;
            d2 = 16384 - d;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            for (i = 0; i < data.M_width; i++) {
                backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

                    d = rand.Next(32768);
                    if (d >= 16384 && d < d1) {
                        rgbValues[index + 2] = 0;
                    }
                    if (d >= d2 && d <= 16384) {
                        rgbValues[index + 2] = 255;
                    }

                    d = rand.Next(32768);
                    if (d >= 16384 && d < d1) {
                        rgbValues[index + 1] = 0;
                    }
                    if (d >= d2 && d <= 16384) {
                        rgbValues[index + 1] = 255;
                    }

                    d = rand.Next(32768);
                    if (d >= 16384 && d < d1) {
                        rgbValues[index] = 0;
                    }
                    if (d >= d2 && d <= 16384) {
                        rgbValues[index] = 255;
                    }
                }
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Salt and Pepper Noise generator [BW]
        private static TimeSpan SaltPepperNoise_BW(ImageData data, double probability, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int d = 0;
            int d1 = 0;
            int d2 = 0;
            int index = 0;
            Random rand = new Random();

            d = (int)(probability*32768/2);
            d1 = d + 16384;
            d2 = 16384 - d;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            for (i = 0; i < data.M_width; i++) {
                backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

                    d = rand.Next(32768);
                    if (d >= 16384 && d < d1) {
                        rgbValues[index + 2] = 0;
                        rgbValues[index + 1] = 0;
                        rgbValues[index] = 0;
                    }
                    if (d >= d2 && d <= 16384) {
                        rgbValues[index + 2] = 255;
                        rgbValues[index + 1] = 255;
                        rgbValues[index] = 255;
                    }
                }
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Noise reduction filter [Mean]
        private static TimeSpan NoiseReduction_Mean(ImageData data, int sizeMask, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            int sumR = 0;
            int sumG = 0;
            int sumB = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            for (i = sizeMask/2; i < data.M_width - sizeMask/2; i++) {
                backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                for (j = sizeMask/2; j < data.M_height - sizeMask/2; j++) {
                    sumR = 0;
                    sumG = 0;
                    sumB = 0;
                    for (k = -sizeMask/2; k <= sizeMask/2; k++) {
                        for (l = -sizeMask/2; l <= sizeMask/2; l++) {
                            index = ((j + l)*bmpData.Stride) + ((i + k)*3);
                            sumR = sumR + rgbValues[index + 2];
                            sumG = sumG + rgbValues[index + 1];
                            sumB = sumB + rgbValues[index];
                        }
                    }

                    index = (j*bmpData.Stride) + (i*3);

                    rgbValues[index + 2] = (byte)(sumR/(sizeMask*sizeMask));
                    rgbValues[index + 1] = (byte)(sumG/(sizeMask*sizeMask));
                    rgbValues[index] = (byte)(sumB/(sizeMask*sizeMask));
                }
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Noise reduction filter [Median]
        private static TimeSpan NoiseReduction_Median(ImageData data, int sizeMask, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            int z = 0;
            int aR = 0;
            int aG = 0;
            int aB = 0;
            int[] arR = new int[121];
            int[] arG = new int[121];
            int[] arB = new int[121];
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            for (i = sizeMask/2; i < data.M_width - sizeMask/2; i++) {
                backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                for (j = sizeMask/2; j < data.M_height - sizeMask/2; j++) {
                    z = 0;
                    for (k = -sizeMask/2; k <= sizeMask/2; k++) {
                        for (l = -sizeMask/2; l <= sizeMask/2; l++) {
                            index = ((j + l)*bmpData.Stride) + ((i + k)*3);
                            arR[z] = rgbValues[index + 2];
                            arG[z] = rgbValues[index + 1];
                            arB[z] = rgbValues[index];
                            z++;
                        }
                    }

                    for (k = 1; k <= sizeMask*sizeMask - 1; k++) {
                        aR = arR[k];
                        l = k - 1;

                        while (l >= 0 && arR[l] > aR) {
                            arR[l + 1] = arR[l];
                            l--;
                        }

                        arR[l + 1] = aR;
                    }

                    for (k = 1; k <= sizeMask*sizeMask - 1; k++) {
                        aG = arG[k];
                        l = k - 1;

                        while (l >= 0 && arG[l] > aG) {
                            arG[l + 1] = arG[l];
                            l--;
                        }

                        arG[l + 1] = aG;
                    }

                    for (k = 1; k <= sizeMask*sizeMask - 1; k++) {
                        aB = arB[k];
                        l = k - 1;

                        while (l >= 0 && arB[l] > aB) {
                            arB[l + 1] = arB[l];
                            l--;
                        }

                        arB[l + 1] = aB;
                    }

                    index = (j*bmpData.Stride) + (i*3);

                    rgbValues[index + 2] = (byte)arR[sizeMask*sizeMask/2];
                    rgbValues[index + 1] = (byte)arG[sizeMask*sizeMask/2];
                    rgbValues[index] = (byte)arB[sizeMask*sizeMask/2];
                }
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion
    }
    #endregion

    #region algorithms backup [2] (do not use, only for reference) (31/1/2016)
    /// <summary>
    /// This class contains older implementations of algorithms. It is not meant for use in the program.
    /// Newer implementations are faster and with less bugs (if any).
    /// </summary>
    internal static class AlgorithmsBackup2 {
        #region Shift bits
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="bits">Bits.</param>
        /// <returns></returns>
        public static TimeSpan ShiftBits(ImageData data, int bits) {
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                rgb[k] = (byte)(b << bits); // B
                rgb[k + 1] = (byte)(g << bits); // G
                rgb[k + 2] = (byte)(r << bits); // R
            });

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Threshold
        /// <summary>
        /// Set a specific threshold. one threshold for all channels.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="threshold">Threshold.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan Threshold(ImageData data, int threshold) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];

                r = r < threshold ? 0 : 255;
                g = g < threshold ? 0 : 255;
                b = b < threshold ? 0 : 255;

                rgb[k] = (byte)b;
                rgb[k + 1] = (byte)g;
                rgb[k + 2] = (byte)r;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Auto threshold
        /// <summary>
        /// Set a threshold depending on the histogram of the image.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="distance">Distance of 2 peaks in histogram.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan AutoThreshold(ImageData data, int distance) {
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

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.For(0, 256, i => {
                histogramR[i] = 0;
                histogramG[i] = 0;
                histogramB[i] = 0;
                histogramSortR[i] = 0;
                histogramSortG[i] = 0;
                histogramSortB[i] = 0;
                positionR[i] = i;
                positionG[i] = i;
                positionB[i] = i;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];

                histogramB[b]++;
                histogramG[g]++;
                histogramR[r]++;
                histogramSortB[b]++;
                histogramSortG[g]++;
                histogramSortR[r]++;
            });

            Parallel.For(1, 256, k => {
                for (int l = 255; l >= k; l--) {
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
            });

            z1R = histogramSortR[0];
            positionz1R = positionR[0];
            z1G = histogramSortG[0];
            positionz1G = positionG[0];
            z1B = histogramSortB[0];
            positionz1B = positionB[0];

            for (int i = 1; i < 256; i++) {
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
                Parallel.For(positionz1R + 1, positionz2R, i => {
                    if ((histogramR[i]*1.0/z2R) < z) {
                        z = histogramR[i]*1.0/z2R;
                        thresholdR = i;
                    }
                });
            } else {
                z = histogramR[positionz2R + 1]*1.0/z1R;
                Parallel.For(positionz2R + 1, positionz1R, i => {
                    if ((histogramR[i]*1.0/z1R) < z) {
                        z = histogramR[i]*1.0/z1R;
                        thresholdR = i;
                    }
                });
            }

            if (positionz1G < positionz2G) {
                z = histogramG[positionz1G + 1]*1.0/z2G;
                Parallel.For(positionz1G + 1, positionz2G, i => {
                    if ((histogramG[i]*1.0/z2G) < z) {
                        z = histogramG[i]*1.0/z2G;
                        thresholdG = i;
                    }
                });
            } else {
                z = histogramG[positionz2G + 1]*1.0/z1G;
                Parallel.For(positionz2G + 1, positionz1G, i => {
                    if ((histogramG[i]*1.0/z1G) < z) {
                        z = histogramG[i]*1.0/z1G;
                        thresholdG = i;
                    }
                });
            }

            if (positionz1B < positionz2B) {
                z = histogramB[positionz1B + 1]*1.0/z2B;
                Parallel.For(positionz1B + 1, positionz2B, i => {
                    if ((histogramB[i]*1.0/z2B) < z) {
                        z = histogramB[i]*1.0/z2B;
                        thresholdB = i;
                    }
                });
            } else {
                z = histogramB[positionz2B + 1]*1.0/z1B;
                Parallel.For(positionz2B + 1, positionz1B, i => {
                    if ((histogramB[i]*1.0/z1B) < z) {
                        z = histogramB[i]*1.0/z1B;
                        thresholdB = i;
                    }
                });
            }

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];

                r = r < thresholdR ? 0 : 255;
                g = g < thresholdG ? 0 : 255;
                b = b < thresholdB ? 0 : 255;

                rgb[k + 2] = (byte)r;
                rgb[k + 1] = (byte)g;
                rgb[k] = (byte)b;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Negative
        /// <summary>
        /// Invert image.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan Negative(ImageData data) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                rgb[k] = (byte)(255 - b); // B
                rgb[k + 1] = (byte)(255 - g); // G
                rgb[k + 2] = (byte)(255 - r); // R
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Square root
        /// <summary>
        /// Increase brightness exponentially.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan SquareRoot(ImageData data) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                rgb[k] = (byte)Math.Sqrt(b*255); // B
                rgb[k + 1] = (byte)Math.Sqrt(g*255); // G
                rgb[k + 2] = (byte)Math.Sqrt(r*255); // R
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Contrast enhancement
        /// <summary>
        /// Contrast enhancement.
        /// <para>Change brightness and cotnrast at the same time.</para>
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="brightness">Brightness.</param>
        /// <param name="contrast">Contrast.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan ContrastEnhancement(ImageData data, int brightness, double contrast) {
            // Lock the bitmap's bits.
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                double b = rgb[k];
                double g = rgb[k + 1];
                double r = rgb[k + 2];
                b = (b + brightness)*contrast;
                g = (g + brightness)*contrast;
                r = (r + brightness)*contrast;

                if (r > 255.0) {
                    r = 255.0;
                } else if (r < 0.0) {
                    r = 0.0;
                }

                if (g > 255.0) {
                    g = 255.0;
                } else if (g < 0.0) {
                    g = 0.0;
                }

                if (b > 255.0) {
                    b = 255.0;
                } else if (b < 0.0) {
                    b = 0.0;
                }

                rgb[k] = (byte)b;
                rgb[k + 1] = (byte)g;
                rgb[k + 2] = (byte)r;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Brightness
        /// <summary>
        /// Change brightness of the image.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="brightness">Brightness.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan Brightness(ImageData data, int brightness) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                b = b + brightness;
                g = g + brightness;
                r = r + brightness;

                if (r > 255) {
                    r = 255;
                } else if (r < 0) {
                    r = 0;
                }

                if (g > 255) {
                    g = 255;
                } else if (g < 0) {
                    g = 0;
                }

                if (b > 255) {
                    b = 255;
                } else if (b < 0) {
                    b = 0;
                }

                rgb[k] = (byte)b;
                rgb[k + 1] = (byte)g;
                rgb[k + 2] = (byte)r;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Contrast
        /// <summary>
        /// Change contrast of the image.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="contrast">Contrast.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan Contrast(ImageData data, double contrast) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithms
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                double b = rgb[k];
                double g = rgb[k + 1];
                double r = rgb[k + 2];
                b = b*contrast;
                g = g*contrast;
                r = r*contrast;

                if (r > 255.0) {
                    r = 255.0;
                } else if (r < 0.0) {
                    r = 0.0;
                }

                if (g > 255.0) {
                    g = 255.0;
                } else if (g < 0.0) {
                    g = 0.0;
                }

                if (b > 255.0) {
                    b = 255.0;
                } else if (b < 0.0) {
                    b = 0.0;
                }

                rgb[k] = (byte)b;
                rgb[k + 1] = (byte)g;
                rgb[k + 2] = (byte)r;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        /// <summary>
        /// Convert colored image to grayscale.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <returns></returns>
        public static TimeSpan ConvertToGrayscale(ImageData data) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithms
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                double b = rgb[k];
                double g = rgb[k + 1];
                double r = rgb[k + 2];
                double y = b*0.114 + g*0.587 + r*0.299;

                rgb[k] = (byte)y;
                rgb[k + 1] = (byte)y;
                rgb[k + 2] = (byte)y;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }

        #region Sepia tone
        /// <summary>
        /// Sepia tone.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan Sepia(ImageData data) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithms
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                double b = rgb[k];
                double g = rgb[k + 1];
                double r = rgb[k + 2];

                b = b*0.131 + g*0.534 + r*0.272;
                g = b*0.168 + g*0.686 + r*0.349;
                r = b*0.189 + g*0.769 + r*0.393;

                rgb[k] = b > 255 ? (byte)255 : (byte)b;
                rgb[k + 1] = g > 255 ? (byte)255 : (byte)g;
                rgb[k + 2] = r > 255 ? (byte)255 : (byte)r;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Histogram for RGBY channels
        /// <summary>
        /// Calculate the average value of histogram.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="values">Histogram values.</param>
        /// <returns>Returns the average value.</returns>
        public static float HistogramMeanValue(ImageData data, int[] values) {
            float mean = 0;
            float histogramSum = 0;

            Parallel.For(0, 256, i => {
                histogramSum = histogramSum + (i*values[i]);
            });

            mean = histogramSum/(float)(data.M_width*data.M_height);

            return mean;
        }

        /// <summary>
        /// Calculating the histogram of the red channel.
        /// </summary>
        /// <returns>
        /// Histogram of the red channel.
        /// </returns>
        public static int[] HistogramRed(ImageData data) {
            int[] histogramR = new int[256];

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            Parallel.For(0, 256, i => {
                histogramR[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int r = rgb[k + 2];
                histogramR[r]++;
            });

            Marshal.Copy(rgb, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return histogramR;
        }

        /// <summary>
        /// Calculating the histogram of the green channel.
        /// </summary>
        /// <returns>
        /// Histogram of the green channel.
        /// </returns>
        public static int[] HistogramGreen(ImageData data) {
            int[] histogramG = new int[256];

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            Parallel.For(0, 256, i => {
                histogramG[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int g = rgb[k + 1];
                histogramG[g]++;
            });

            Marshal.Copy(rgb, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return histogramG;
        }

        /// <summary>
        /// Calculating the histogram of the blue channel.
        /// </summary>
        /// <returns>
        /// Histogram of the blue channel.
        /// </returns>
        public static int[] HistogramBlue(ImageData data) {
            int[] histogramB = new int[256];

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            Parallel.For(0, 256, i => {
                histogramB[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                histogramB[b]++;
            });

            Marshal.Copy(rgb, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return histogramB;
        }

        /// <summary>
        /// Calculating the histogram for the luminance values.
        /// </summary>
        /// <returns>
        /// Histogram of the luminance values.
        /// </returns>
        public static int[] HistogramLuminance(ImageData data) {
            int[] histogramY = new int[256];

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            Parallel.For(0, 256, i => {
                histogramY[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                int y = (int)(0.2126*r + 0.7152*g + 0.0722*b); // source = https://en.wikipedia.org/wiki/Grayscale#cite_note-5
                histogramY[y]++;
            });

            Marshal.Copy(rgb, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return histogramY;
        }
        #endregion

        #region Histogram equalization [RGB]
        /// <summary>
        /// Histogram equalization at the RGB color space.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan HistogramEqualization_RGB(ImageData data) {
            double[] possibilityR = new double[256];
            double[] possibilityG = new double[256];
            double[] possibilityB = new double[256];
            int[] histogramR = new int[256];
            int[] histogramG = new int[256];
            int[] histogramB = new int[256];
            double[] histogramEqR = new double[256];
            double[] histogramEqG = new double[256];
            double[] histogramEqB = new double[256];

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.For(0, 256, i => {
                histogramR[i] = 0;
                histogramG[i] = 0;
                histogramB[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                histogramB[b]++;
                histogramG[g]++;
                histogramR[r]++;
            });

            for (int i = 0; i < 256; i++) {
                possibilityB[i] = histogramB[i]/(double)(data.M_width*data.M_height);
                possibilityG[i] = histogramG[i]/(double)(data.M_width*data.M_height);
                possibilityR[i] = histogramR[i]/(double)(data.M_width*data.M_height);
            }

            histogramEqB[0] = possibilityB[0];
            histogramEqG[0] = possibilityG[0];
            histogramEqR[0] = possibilityR[0];
            for (int i = 1; i < 256; i++) {
                histogramEqB[i] = histogramEqB[i - 1] + possibilityB[i];
                histogramEqG[i] = histogramEqG[i - 1] + possibilityG[i];
                histogramEqR[i] = histogramEqR[i - 1] + possibilityR[i];
            }

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                b = (int)Math.Round(histogramEqB[b]*255);
                g = (int)Math.Round(histogramEqG[g]*255);
                r = (int)Math.Round(histogramEqR[r]*255);

                if (b > 255) {
                    b = 255;
                } else if (b < 0) {
                    b = 0;
                }

                if (g > 255) {
                    g = 255;
                } else if (g < 0) {
                    g = 0;
                }

                if (r > 255) {
                    r = 255;
                } else if (r < 0) {
                    r = 0;
                }

                rgb[k] = (byte)b;
                rgb[k + 1] = (byte)g;
                rgb[k + 2] = (byte)r;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Histogram equalization [HSV]
        /// <summary>
        /// Histogram equalization at the HSV color space.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Execution time.</returns>
        public static TimeSpan HistogramEqualization_HSV(ImageData data) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];
            double[] hsv = new double[bytes];
            int[] histogramV = new int[256];
            double[] sumHistogramEqualizationV = new double[256];
            double[] sumHistogramV = new double[256];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.For(0, 256, i => {
                histogramV[i] = 0;
                sumHistogramEqualizationV[i] = 0.0;
                sumHistogramV[i] = 0.0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
                double b = rgb[p];
                double g = rgb[p + 1];
                double r = rgb[p + 2];

                b = b/255.0;
                g = g/255.0;
                r = r/255.0;

                double min = Math.Min(r, Math.Min(g, b));
                double max = Math.Max(r, Math.Max(g, b));
                double chroma = max - min;
                hsv[p + 2] = 0.0;
                hsv[p + 1] = 0.0;
                if (chroma != 0.0) {
                    if (Math.Abs(r - max) < 0.00001) {
                        hsv[p + 2] = ((g - b)/chroma);
                        hsv[p + 2] = hsv[p + 2]%6.0;
                    } else if (Math.Abs(g - max) < 0.00001) {
                        hsv[p + 2] = ((b - r)/chroma) + 2;
                    } else {
                        hsv[p + 2] = ((r - g)/chroma) + 4;
                    }

                    hsv[p + 2] = hsv[p + 2]*60.0;
                    if (hsv[p + 2] < 0.0) {
                        hsv[p + 2] = hsv[p + 2] + 360.0;
                    }
                    hsv[p + 1] = chroma/max;
                }
                hsv[p] = max;

                hsv[p] = hsv[p]*255.0;

                if (hsv[p] > 255.0) {
                    hsv[p] = 255.0;
                }
                if (hsv[p] < 0.0) {
                    hsv[p] = 0.0;
                }

                int k = (int)hsv[p];
                histogramV[k]++;
            });

            for (int i = 0; i < 256; i++) {
                sumHistogramEqualizationV[i] = histogramV[i]/(double)(data.M_width*data.M_height);
            }

            sumHistogramV[0] = sumHistogramEqualizationV[0];
            for (int i = 1; i < 256; i++) {
                sumHistogramV[i] = sumHistogramV[i - 1] + sumHistogramEqualizationV[i];
            }

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
                double b = rgb[p];
                double g = rgb[p + 1];
                double r = rgb[p + 2];

                int k = (int)hsv[p];
                hsv[p] = (byte)Math.Round(sumHistogramV[k]*255.0);
                hsv[p] = hsv[p]/255;

                double c = hsv[p]*hsv[p + 1];
                hsv[p + 2] = hsv[p + 2]/60.0;
                hsv[p + 2] = hsv[p + 2]%2;
                double x = c*(1.0 - Math.Abs(hsv[p + 2] - 1.0));
                double m = hsv[p] - c;

                if (hsv[p + 2] >= 0.0 && hsv[p + 2] < 60.0) {
                    b = 0;
                    g = x;
                    r = c;
                } else if (hsv[p + 2] >= 60.0 && hsv[p + 2] < 120.0) {
                    b = 0;
                    g = c;
                    r = x;
                } else if (hsv[p + 2] >= 120.0 && hsv[p + 2] < 180.0) {
                    b = x;
                    g = c;
                    r = 0;
                } else if (hsv[p + 2] >= 180.0 && hsv[p + 2] < 240.0) {
                    b = c;
                    g = x;
                    r = 0;
                } else if (hsv[p + 2] >= 240.0 && hsv[p + 2] < 300.0) {
                    b = c;
                    g = 0;
                    r = x;
                } else if (hsv[p + 2] >= 300.0 && hsv[p + 2] < 360.0) {
                    b = x;
                    g = 0;
                    r = c;
                }

                b = b + m;
                g = g + m;
                r = r + m;

                b = b*255.0;
                g = g*255.0;
                r = r*255.0;

                if (r > 255.0) {
                    r = 255.0;
                }

                if (r < 0.0) {
                    r = 0.0;
                }

                if (g > 255.0) {
                    g = 255.0;
                }

                if (g < 0.0) {
                    g = 0.0;
                }

                if (b > 255.0) {
                    b = 255.0;
                }

                if (b < 0.0) {
                    b = 0.0;
                }

                rgb[p] = (byte)b;
                rgb[p + 1] = (byte)g;
                rgb[p + 2] = (byte)r;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Histogram equalization [YUV]
        /// <summary>
        /// Histogram equalization at the YUV color space.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan HistogramEqualization_YUV(ImageData data) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];
            double[] yuv = new double[bytes];
            int[] histogramY = new int[256];
            double[] sumHistogramEqualizationY = new double[256];
            double[] sumHistogramY = new double[256];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.For(0, 256, i => {
                histogramY[i] = 0;
                sumHistogramEqualizationY[i] = 0;
                sumHistogramY[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
                double b = rgb[p];
                double g = rgb[p + 1];
                double r = rgb[p + 2];
                b = b/255.0;
                g = g/255.0;
                r = r/255.0;

                yuv[p] = (0.299*r) + (0.587*g) + (0.114*b);
                yuv[p + 1] = (-0.14713*r) - (0.28886*g) + (0.436*b);
                yuv[p + 2] = (0.615*r) - (0.51499*g) - (0.10001*b);

                yuv[p] = yuv[p]*255.0;
                if (yuv[p] > 255.0) {
                    yuv[p] = 255.0;
                }

                if (yuv[p] < 0.0) {
                    yuv[p] = 0.0;
                }

                int k = (int)yuv[p];
                histogramY[k]++;
            });

            for (int i = 0; i < 256; i++) {
                sumHistogramEqualizationY[i] = histogramY[i]/(double)(data.M_bitmap.Width*data.M_bitmap.Height);
            }

            sumHistogramY[0] = sumHistogramEqualizationY[0];
            for (int i = 1; i < 256; i++) {
                sumHistogramY[i] = sumHistogramY[i - 1] + sumHistogramEqualizationY[i];
            }

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
                double b = 0;
                double g = 0;
                double r = 0;

                int k = (int)yuv[p];
                yuv[p] = (byte)Math.Round(sumHistogramY[k]*255.0);
                yuv[p] = yuv[p]/255;

                r = yuv[p] + (0.0*yuv[p + 1]) + (1.13983*yuv[p + 2]);
                g = yuv[p] + (-0.39465*yuv[p + 1]) + (-0.58060*yuv[p + 2]);
                b = yuv[p] + (2.03211*yuv[p + 1]) + (0.0*yuv[p + 2]);

                r = r*255.0;
                g = g*255.0;
                b = b*255.0;

                if (r > 255.0) {
                    r = 255.0;
                }

                if (r < 0.0) {
                    r = 0.0;
                }

                if (g > 255.0) {
                    g = 255.0;
                }

                if (g < 0.0) {
                    g = 0.0;
                }

                if (b > 255.0) {
                    b = 255.0;
                }

                if (b < 0.0) {
                    b = 0.0;
                }

                rgb[p + 2] = (byte)r;
                rgb[p + 1] = (byte)g;
                rgb[p] = (byte)b;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Image summarization
        /// <summary>
        /// Image Summarization.
        /// <para>Adding the same image, increasing its brightness.</para>
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan ImageSummarization(ImageData data) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
                int b = rgb[p] + rgb[p]; // B
                int g = rgb[p + 1] + rgb[p + 1]; // G
                int r = rgb[p + 2] + rgb[p + 2]; // R

                if (r > 255) {
                    r = 255;
                }

                if (g > 255) {
                    g = 255;
                }

                if (b > 255) {
                    b = 255;
                }

                rgb[p] = (byte)b; // B
                rgb[p + 1] = (byte)g; // G
                rgb[p + 2] = (byte)r; // R
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Image subtraction
        /// <summary>
        /// Image subtraction.
        /// <para>Subtract image from itselft which results to a black image. Not useful at all. It will be chnaged soon.</para>
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan ImageSubtraction(ImageData data) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
                int b = rgb[p] - rgb[p]; // B
                int g = rgb[p + 1] - rgb[p + 1]; // G
                int r = rgb[p + 2] - rgb[p + 2]; // R

                rgb[p] = (byte)b; // B
                rgb[p + 1] = (byte)g; // G
                rgb[p + 2] = (byte)r; // R
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Edge detection [Sobel]
        /// <summary>
        /// Sobel edge detector.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="kernelSize">Kernel size.</param>
        /// <param name="kernelX">X axis kernel</param>
        /// <param name="kernelY">Y axis kernel</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan EdgeDetection_Sobel(ImageData data, int kernelSize, double[,] kernelX, double[,] kernelY) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare 2 arrays to hold the bytes of the bitmap.
            // The first to read from and then write to the scond.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] bgrValues = new byte[bytes];

            // Calculate the offset regarding the size of the kernel.
            int filterOffset = (kernelSize - 1)/2;

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            Parallel.For(filterOffset, bmpData.Width - filterOffset, i => {
                for (int j = filterOffset; j < bmpData.Height - filterOffset; j++) {
                    int index = 0;
                    double txR = 0;
                    double txG = 0;
                    double txB = 0;
                    for (int k = 0; k < kernelSize; k++) {
                        for (int l = 0; l < kernelSize; l++) {
                            int indexX = ((j + l - filterOffset)*bmpData.Stride) + ((i + k - filterOffset)*bytesPerPixel);
                            txR = txR + rgbValues[indexX + 2]*kernelX[k, l];
                            txG = txG + rgbValues[indexX + 1]*kernelX[k, l];
                            txB = txB + rgbValues[indexX]*kernelX[k, l];
                        }
                    }

                    double tyR = 0;
                    double tyG = 0;
                    double tyB = 0;
                    for (int k = 0; k < kernelSize; k++) {
                        for (int l = 0; l < kernelSize; l++) {
                            int indexY = ((j + l - filterOffset)*bmpData.Stride) + ((i + k - filterOffset)*bytesPerPixel);
                            tyR = tyR + rgbValues[indexY + 2]*kernelY[k, l];
                            tyG = tyG + rgbValues[indexY + 1]*kernelY[k, l];
                            tyB = tyB + rgbValues[indexY]*kernelY[k, l];
                        }
                    }

                    double tR = Math.Sqrt(txR*txR + tyR*tyR);
                    double tG = Math.Sqrt(txG*txG + tyG*tyG);
                    double tB = Math.Sqrt(txB*txB + tyB*tyB);

                    if (tR > 255.0) {
                        tR = 255.0;
                    } else if (tR < 0.0) {
                        tR = 0.0;
                    }

                    if (tG > 255.0) {
                        tG = 255.0;
                    } else if (tG < 0.0) {
                        tG = 0.0;
                    }

                    if (tB > 255.0) {
                        tB = 255.0;
                    } else if (tB < 0.0) {
                        tB = 0.0;
                    }

                    index = (j*bmpData.Stride) + (i*bytesPerPixel);

                    bgrValues[index + 2] = (byte)tR;
                    bgrValues[index + 1] = (byte)tG;
                    bgrValues[index] = (byte)tB;
                }
            });
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(bgrValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);


            return elapsedTime;
        }
        #endregion

        #region Gaussian blur
        /// <summary>
        /// Gaussian blur filter.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="kernelSize">Kernel size.</param>
        /// <param name="kernelX">X axis kernel.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan GaussianBlur(ImageData data, int kernelSize, double[,] kernelX) {
            double sumMask = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare 2 arrays to hold the bytes of the bitmap.
            // The first to read from and then write to the scond.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] bgrValues = new byte[bytes];

            // Calculate the offset regarding the size of the kernel.
            int filterOffset = (kernelSize - 1)/2;

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            #region Algorithm
            for (int i = 0; i < kernelSize; i++) {
                for (int j = 0; j < kernelSize; j++) {
                    sumMask += kernelX[i, j];
                }
            }

            Parallel.For(filterOffset, bmpData.Width - filterOffset, i => {
                for (int j = filterOffset; j < bmpData.Height - filterOffset; j++) {
                    int index = 0;
                    double tR = 0.0;
                    double tG = 0.0;
                    double tB = 0.0;
                    for (int k = 0; k < kernelSize; k++) {
                        for (int l = 0; l < kernelSize; l++) {
                            int indexX = ((j + l - filterOffset)*bmpData.Stride) + ((i + k - filterOffset)*bytesPerPixel);
                            tR = tR + (rgbValues[indexX + 2]*kernelX[k, l])/sumMask;
                            tG = tG + (rgbValues[indexX + 1]*kernelX[k, l])/sumMask;
                            tB = tB + (rgbValues[indexX]*kernelX[k, l])/sumMask;
                        }
                    }

                    if (tR > 255.0) {
                        tR = 255.0;
                    } else if (tR < 0.0) {
                        tR = 0.0;
                    }

                    if (tG > 255.0) {
                        tG = 255.0;
                    } else if (tG < 0.0) {
                        tG = 0.0;
                    }

                    if (tB > 255.0) {
                        tB = 255.0;
                    } else if (tB < 0.0) {
                        tB = 0.0;
                    }

                    index = (j*bmpData.Stride) + (i*bytesPerPixel);

                    bgrValues[index + 2] = (byte)tR;
                    bgrValues[index + 1] = (byte)tG;
                    bgrValues[index] = (byte)tB;
                }
            });
            #endregion

            Marshal.Copy(bgrValues, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Sharpen
        /// <summary>
        /// Sharpen filter.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="kernelSize">Kernel size.</param>
        /// <param name="kernelX">X axis mask.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan Sharpen(ImageData data, int kernelSize, double[,] kernelX) {
            double sumMask = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare 2 arrays to hold the bytes of the bitmap.
            // The first to read from and then write to the scond.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] bgrValues = new byte[bytes];

            // Calculate the offset regarding the size of the kernel.
            int filterOffset = (kernelSize - 1)/2;

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            #region Algorithm
            for (int i = 0; i < kernelSize; i++) {
                for (int j = 0; j < kernelSize; j++) {
                    sumMask += kernelX[i, j];
                }
            }

            Parallel.For(filterOffset, bmpData.Width - filterOffset, i => {
                for (int j = filterOffset; j < bmpData.Height - filterOffset; j++) {
                    int index = 0;
                    double tR = 0.0;
                    double tG = 0.0;
                    double tB = 0.0;
                    for (int k = 0; k < kernelSize; k++) {
                        for (int l = 0; l < kernelSize; l++) {
                            int indexX = ((j + l - filterOffset)*bmpData.Stride) + ((i + k - filterOffset)*bytesPerPixel);
                            tR = tR + (rgbValues[indexX + 2]*kernelX[k, l])/sumMask;
                            tG = tG + (rgbValues[indexX + 1]*kernelX[k, l])/sumMask;
                            tB = tB + (rgbValues[indexX]*kernelX[k, l])/sumMask;
                        }
                    }

                    if (tR > 255.0) {
                        tR = 255.0;
                    } else if (tR < 0.0) {
                        tR = 0.0;
                    }

                    if (tG > 255.0) {
                        tG = 255.0;
                    } else if (tG < 0.0) {
                        tG = 0.0;
                    }

                    if (tB > 255.0) {
                        tB = 255.0;
                    } else if (tB < 0.0) {
                        tB = 0.0;
                    }

                    index = (j*bmpData.Stride) + (i*bytesPerPixel);

                    bgrValues[index + 2] = (byte)tR;
                    bgrValues[index + 1] = (byte)tG;
                    bgrValues[index] = (byte)tB;
                }
            });
            #endregion

            Marshal.Copy(bgrValues, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Salt and Pepper noise generator [Color]
        /// <summary>
        /// Colored Salt and Pepper noise generator.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="probability">Probability.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan SaltPepperNoise_Color(ImageData data, double probability) {
            int d = 0;
            int d1 = 0;
            int d2 = 0;
            int index = 0;
            Random rand = new Random();

            d = (int)(probability*32768/2);
            d1 = d + 16384;
            d2 = 16384 - d;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            for (int i = 0; i < bmpData.Width; i++) {
                for (int j = 0; j < bmpData.Height; j++) {
                    index = (j*bmpData.Stride) + (i*bytesPerPixel);

                    d = rand.Next(32768);
                    if (d >= 16384 && d < d1) {
                        rgb[index + 2] = 0;
                    }
                    if (d >= d2 && d <= 16384) {
                        rgb[index + 2] = 255;
                    }

                    d = rand.Next(32768);
                    if (d >= 16384 && d < d1) {
                        rgb[index + 1] = 0;
                    }
                    if (d >= d2 && d <= 16384) {
                        rgb[index + 1] = 255;
                    }

                    d = rand.Next(32768);
                    if (d >= 16384 && d < d1) {
                        rgb[index] = 0;
                    }
                    if (d >= d2 && d <= 16384) {
                        rgb[index] = 255;
                    }
                }
            }
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Salt and Pepper Noise generator [BW]
        /// <summary>
        /// Black and white Salt and Pepper noise generator.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="probability">Probability.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan SaltPepperNoise_BW(ImageData data, double probability) {
            int d = 0;
            int d1 = 0;
            int d2 = 0;
            int index = 0;
            Random rand = new Random();

            d = (int)(probability*32768/2);
            d1 = d + 16384;
            d2 = 16384 - d;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            for (int i = 0; i < bmpData.Width; i++) {
                for (int j = 0; j < bmpData.Height; j++) {
                    index = (j*bmpData.Stride) + (i*bytesPerPixel);

                    d = rand.Next(32768);
                    if (d >= 16384 && d < d1) {
                        rgb[index] = 0;
                        rgb[index + 1] = 0;
                        rgb[index + 2] = 0;
                    }
                    if (d >= d2 && d <= 16384) {
                        rgb[index] = 255;
                        rgb[index + 1] = 255;
                        rgb[index + 2] = 255;
                    }
                }
            }
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Noise reduction filter [Mean]
        /// <summary>
        /// Noise reduction filter (Arithmetic Mean algorithm).
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="kernelSize">Kernel size.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan NoiseReduction_Mean(ImageData data, int kernelSize) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.For(kernelSize/2, bmpData.Width - kernelSize/2, i => {
                for (int j = kernelSize/2; j < bmpData.Height - kernelSize/2; j++) {
                    int index = 0;
                    int sumR = 0;
                    int sumG = 0;
                    int sumB = 0;
                    for (int k = -kernelSize/2; k <= kernelSize/2; k++) {
                        for (int l = -kernelSize/2; l <= kernelSize/2; l++) {
                            int indexX = ((j + l)*bmpData.Stride) + ((i + k)*3);
                            sumB += rgb[indexX];
                            sumG += rgb[indexX + 1];
                            sumR += rgb[indexX + 2];
                        }
                    }

                    index = (j*bmpData.Stride) + (i*bytesPerPixel);

                    rgb[index] = (byte)(sumB/(kernelSize*kernelSize));
                    rgb[index + 1] = (byte)(sumG/(kernelSize*kernelSize));
                    rgb[index + 2] = (byte)(sumR/(kernelSize*kernelSize));
                }
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Noise reduction filter [Median]
        /// <summary>
        /// Noise reduction filter (Median algorithm).
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="kernelSize">Kernel size.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan NoiseReduction_Median(ImageData data, int kernelSize) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.For(kernelSize/2, bmpData.Width - kernelSize/2, i => {
                int[] arR = new int[121];
                int[] arG = new int[121];
                int[] arB = new int[121];
                for (int j = kernelSize/2; j < bmpData.Height - kernelSize/2; j++) {
                    int index = 0;
                    int z = 0;
                    for (int k = -kernelSize/2; k <= kernelSize/2; k++) {
                        for (int l = -kernelSize/2; l <= kernelSize/2; l++) {
                            int indexX = ((j + l)*bmpData.Stride) + ((i + k)*3);
                            arR[z] = rgb[indexX + 2];
                            arG[z] = rgb[indexX + 1];
                            arB[z] = rgb[indexX];
                            z++;
                        }
                    }

                    for (int k = 1; k <= kernelSize*kernelSize - 1; k++) {
                        int aR = arR[k];
                        int l = k - 1;

                        while (l >= 0 && arR[l] > aR) {
                            arR[l + 1] = arR[l];
                            l--;
                        }

                        arR[l + 1] = aR;
                    }

                    for (int k = 1; k <= kernelSize*kernelSize - 1; k++) {
                        int aG = arG[k];
                        int l = k - 1;

                        while (l >= 0 && arG[l] > aG) {
                            arG[l + 1] = arG[l];
                            l--;
                        }

                        arG[l + 1] = aG;
                    }

                    for (int k = 1; k <= kernelSize*kernelSize - 1; k++) {
                        int aB = arB[k];
                        int l = k - 1;

                        while (l >= 0 && arB[l] > aB) {
                            arB[l + 1] = arB[l];
                            l--;
                        }

                        arB[l + 1] = aB;
                    }

                    index = (j*bmpData.Stride) + (i*bytesPerPixel);

                    rgb[index + 2] = (byte)arR[kernelSize*kernelSize/2];
                    rgb[index + 1] = (byte)arG[kernelSize*kernelSize/2];
                    rgb[index] = (byte)arB[kernelSize*kernelSize/2];
                }
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion
    }
    #endregion

    #region algorithms backup [3] (do not use, only for reference) (15/2/2016)
    /// <summary>
    /// This class contains older implementations of algorithms. It is not meant for use in the program.
    /// Newer implementations are faster and with less bugs (if any).
    /// </summary>
    internal static class AlgorithmsBackup3 {
        #region Shift bits
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="bits">Bits.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan ShiftBits(ImageData data, int bits) {
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                rgb[k] = (byte)(b << bits); // B
                rgb[k + 1] = (byte)(g << bits); // G
                rgb[k + 2] = (byte)(r << bits); // R
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Threshold
        /// <summary>
        /// Set a specific threshold. one threshold for all channels.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="threshold">Threshold.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan Threshold(ImageData data, int threshold) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];

                r = r < threshold ? 0 : 255;
                g = g < threshold ? 0 : 255;
                b = b < threshold ? 0 : 255;

                rgb[k] = (byte)b;
                rgb[k + 1] = (byte)g;
                rgb[k + 2] = (byte)r;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Auto threshold
        /// <summary>
        /// Set a threshold depending on the histogram of the image.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="distance">Distance of 2 peaks in histogram.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan AutoThreshold(ImageData data, int distance) {
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

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.For(0, 256, i => {
                histogramR[i] = 0;
                histogramG[i] = 0;
                histogramB[i] = 0;
                histogramSortR[i] = 0;
                histogramSortG[i] = 0;
                histogramSortB[i] = 0;
                positionR[i] = i;
                positionG[i] = i;
                positionB[i] = i;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];

                histogramB[b]++;
                histogramG[g]++;
                histogramR[r]++;
                histogramSortB[b]++;
                histogramSortG[g]++;
                histogramSortR[r]++;
            });

            Parallel.For(1, 256, k => {
                for (int l = 255; l >= k; l--) {
                    int temp = 0;
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
            });

            z1R = histogramSortR[0];
            positionz1R = positionR[0];
            z1G = histogramSortG[0];
            positionz1G = positionG[0];
            z1B = histogramSortB[0];
            positionz1B = positionB[0];

            for (int i = 1; i < 256; i++) {
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
                Parallel.For(positionz1R + 1, positionz2R, i => {
                    if ((histogramR[i]*1.0/z2R) < z) {
                        z = histogramR[i]*1.0/z2R;
                        thresholdR = i;
                    }
                });
            } else {
                z = histogramR[positionz2R + 1]*1.0/z1R;
                Parallel.For(positionz2R + 1, positionz1R, i => {
                    if ((histogramR[i]*1.0/z1R) < z) {
                        z = histogramR[i]*1.0/z1R;
                        thresholdR = i;
                    }
                });
            }

            if (positionz1G < positionz2G) {
                z = histogramG[positionz1G + 1]*1.0/z2G;
                Parallel.For(positionz1G + 1, positionz2G, i => {
                    if ((histogramG[i]*1.0/z2G) < z) {
                        z = histogramG[i]*1.0/z2G;
                        thresholdG = i;
                    }
                });
            } else {
                z = histogramG[positionz2G + 1]*1.0/z1G;
                Parallel.For(positionz2G + 1, positionz1G, i => {
                    if ((histogramG[i]*1.0/z1G) < z) {
                        z = histogramG[i]*1.0/z1G;
                        thresholdG = i;
                    }
                });
            }

            if (positionz1B < positionz2B) {
                z = histogramB[positionz1B + 1]*1.0/z2B;
                Parallel.For(positionz1B + 1, positionz2B, i => {
                    if ((histogramB[i]*1.0/z2B) < z) {
                        z = histogramB[i]*1.0/z2B;
                        thresholdB = i;
                    }
                });
            } else {
                z = histogramB[positionz2B + 1]*1.0/z1B;
                Parallel.For(positionz2B + 1, positionz1B, i => {
                    if ((histogramB[i]*1.0/z1B) < z) {
                        z = histogramB[i]*1.0/z1B;
                        thresholdB = i;
                    }
                });
            }

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];

                r = r < thresholdR ? 0 : 255;
                g = g < thresholdG ? 0 : 255;
                b = b < thresholdB ? 0 : 255;

                rgb[k + 2] = (byte)r;
                rgb[k + 1] = (byte)g;
                rgb[k] = (byte)b;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Negative
        /// <summary>
        /// Invert image.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan Negative(ImageData data) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                rgb[k] = (byte)(255 - b); // B
                rgb[k + 1] = (byte)(255 - g); // G
                rgb[k + 2] = (byte)(255 - r); // R
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Square root
        /// <summary>
        /// Increase brightness exponentially.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan SquareRoot(ImageData data) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                rgb[k] = (byte)Math.Sqrt(b*255); // B
                rgb[k + 1] = (byte)Math.Sqrt(g*255); // G
                rgb[k + 2] = (byte)Math.Sqrt(r*255); // R
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Contrast enhancement
        /// <summary>
        /// Contrast enhancement.
        /// <para>Change brightness and cotnrast at the same time.</para>
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="brightness">Brightness.</param>
        /// <param name="contrast">Contrast.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan ContrastEnhancement(ImageData data, int brightness, double contrast) {
            // Lock the bitmap's bits.
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                double b = rgb[k];
                double g = rgb[k + 1];
                double r = rgb[k + 2];
                b = (b + brightness)*contrast;
                g = (g + brightness)*contrast;
                r = (r + brightness)*contrast;

                if (r > 255.0) {
                    r = 255.0;
                } else if (r < 0.0) {
                    r = 0.0;
                }

                if (g > 255.0) {
                    g = 255.0;
                } else if (g < 0.0) {
                    g = 0.0;
                }

                if (b > 255.0) {
                    b = 255.0;
                } else if (b < 0.0) {
                    b = 0.0;
                }

                rgb[k] = (byte)b;
                rgb[k + 1] = (byte)g;
                rgb[k + 2] = (byte)r;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Brightness
        /// <summary>
        /// Change brightness of the image.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="brightness">Brightness.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan Brightness(ImageData data, int brightness) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                b = b + brightness;
                g = g + brightness;
                r = r + brightness;

                if (r > 255) {
                    r = 255;
                } else if (r < 0) {
                    r = 0;
                }

                if (g > 255) {
                    g = 255;
                } else if (g < 0) {
                    g = 0;
                }

                if (b > 255) {
                    b = 255;
                } else if (b < 0) {
                    b = 0;
                }

                rgb[k] = (byte)b;
                rgb[k + 1] = (byte)g;
                rgb[k + 2] = (byte)r;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Contrast
        /// <summary>
        /// Change contrast of the image.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="contrast">Contrast.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan Contrast(ImageData data, double contrast) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                double b = rgb[k];
                double g = rgb[k + 1];
                double r = rgb[k + 2];
                b = b*contrast;
                g = g*contrast;
                r = r*contrast;

                if (r > 255.0) {
                    r = 255.0;
                } else if (r < 0.0) {
                    r = 0.0;
                }

                if (g > 255.0) {
                    g = 255.0;
                } else if (g < 0.0) {
                    g = 0.0;
                }

                if (b > 255.0) {
                    b = 255.0;
                } else if (b < 0.0) {
                    b = 0.0;
                }

                rgb[k] = (byte)b;
                rgb[k + 1] = (byte)g;
                rgb[k + 2] = (byte)r;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Convert to grayscale
        /// <summary>
        /// Convert colored image to grayscale.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <returns></returns>
        public static TimeSpan ConvertToGrayscale(ImageData data) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                double b = rgb[k];
                double g = rgb[k + 1];
                double r = rgb[k + 2];
                double y = b*0.114 + g*0.587 + r*0.299;

                rgb[k] = (byte)y;
                rgb[k + 1] = (byte)y;
                rgb[k + 2] = (byte)y;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Sepia tone
        /// <summary>
        /// Sepia tone.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan Sepia(ImageData data) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                double b = rgb[k];
                double g = rgb[k + 1];
                double r = rgb[k + 2];

                b = b*0.131 + g*0.534 + r*0.272;
                g = b*0.168 + g*0.686 + r*0.349;
                r = b*0.189 + g*0.769 + r*0.393;

                rgb[k] = b > 255 ? (byte)255 : (byte)b;
                rgb[k + 1] = g > 255 ? (byte)255 : (byte)g;
                rgb[k + 2] = r > 255 ? (byte)255 : (byte)r;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Histogram for RGBY channels
        /// <summary>
        /// Calculate the average value of histogram.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="values">Histogram values.</param>
        /// <returns>Returns the average value.</returns>
        public static float HistogramMeanValue(ImageData data, int[] values) {
            float mean = 0;
            float histogramSum = 0;

            Parallel.For(0, 256, i => {
                histogramSum = histogramSum + (i*values[i]);
            });

            mean = histogramSum/(float)(data.M_width*data.M_height);

            return mean;
        }

        /// <summary>
        /// Calculating the histogram of the red channel.
        /// </summary>
        /// <returns>
        /// Histogram of the red channel.
        /// </returns>
        public static int[] HistogramRed(ImageData data) {
            int[] histogramR = new int[256];

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            Parallel.For(0, 256, i => {
                histogramR[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int r = rgb[k + 2];
                histogramR[r]++;
            });

            Marshal.Copy(rgb, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return histogramR;
        }

        /// <summary>
        /// Calculating the histogram of the green channel.
        /// </summary>
        /// <returns>
        /// Histogram of the green channel.
        /// </returns>
        public static int[] HistogramGreen(ImageData data) {
            int[] histogramG = new int[256];

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            Parallel.For(0, 256, i => {
                histogramG[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int g = rgb[k + 1];
                histogramG[g]++;
            });

            Marshal.Copy(rgb, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return histogramG;
        }

        /// <summary>
        /// Calculating the histogram of the blue channel.
        /// </summary>
        /// <returns>
        /// Histogram of the blue channel.
        /// </returns>
        public static int[] HistogramBlue(ImageData data) {
            int[] histogramB = new int[256];

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            Parallel.For(0, 256, i => {
                histogramB[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                histogramB[b]++;
            });

            Marshal.Copy(rgb, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return histogramB;
        }

        /// <summary>
        /// Calculating the histogram for the luminance values.
        /// </summary>
        /// <returns>
        /// Histogram of the luminance values.
        /// </returns>
        public static int[] HistogramLuminance(ImageData data) {
            int[] histogramY = new int[256];

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            Parallel.For(0, 256, i => {
                histogramY[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                int y = (int)(0.2126*r + 0.7152*g + 0.0722*b); // source = https://en.wikipedia.org/wiki/Grayscale#cite_note-5
                histogramY[y]++;
            });

            Marshal.Copy(rgb, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return histogramY;
        }
        #endregion

        #region Histogram equalization [RGB]
        /// <summary>
        /// Histogram equalization at the RGB color space.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan HistogramEqualization_RGB(ImageData data) {
            double[] possibilityR = new double[256];
            double[] possibilityG = new double[256];
            double[] possibilityB = new double[256];
            int[] histogramR = new int[256];
            int[] histogramG = new int[256];
            int[] histogramB = new int[256];
            double[] histogramEqR = new double[256];
            double[] histogramEqG = new double[256];
            double[] histogramEqB = new double[256];

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.For(0, 256, i => {
                histogramR[i] = 0;
                histogramG[i] = 0;
                histogramB[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                histogramB[b]++;
                histogramG[g]++;
                histogramR[r]++;
            });

            for (int i = 0; i < 256; i++) {
                possibilityB[i] = histogramB[i]/(double)(data.M_width*data.M_height);
                possibilityG[i] = histogramG[i]/(double)(data.M_width*data.M_height);
                possibilityR[i] = histogramR[i]/(double)(data.M_width*data.M_height);
            }

            histogramEqB[0] = possibilityB[0];
            histogramEqG[0] = possibilityG[0];
            histogramEqR[0] = possibilityR[0];
            for (int i = 1; i < 256; i++) {
                histogramEqB[i] = histogramEqB[i - 1] + possibilityB[i];
                histogramEqG[i] = histogramEqG[i - 1] + possibilityG[i];
                histogramEqR[i] = histogramEqR[i - 1] + possibilityR[i];
            }

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                b = (int)Math.Round(histogramEqB[b]*255);
                g = (int)Math.Round(histogramEqG[g]*255);
                r = (int)Math.Round(histogramEqR[r]*255);

                if (b > 255) {
                    b = 255;
                } else if (b < 0) {
                    b = 0;
                }

                if (g > 255) {
                    g = 255;
                } else if (g < 0) {
                    g = 0;
                }

                if (r > 255) {
                    r = 255;
                } else if (r < 0) {
                    r = 0;
                }

                rgb[k] = (byte)b;
                rgb[k + 1] = (byte)g;
                rgb[k + 2] = (byte)r;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Histogram equalization [HSV]
        /// <summary>
        /// Histogram equalization at the HSV color space.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Execution time.</returns>
        public static TimeSpan HistogramEqualization_HSV(ImageData data) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];
            double[] hsv = new double[bytes];
            int[] histogramV = new int[256];
            double[] sumHistogramEqualizationV = new double[256];
            double[] sumHistogramV = new double[256];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.For(0, 256, i => {
                histogramV[i] = 0;
                sumHistogramEqualizationV[i] = 0.0;
                sumHistogramV[i] = 0.0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
                double b = rgb[p];
                double g = rgb[p + 1];
                double r = rgb[p + 2];

                b = b/255.0;
                g = g/255.0;
                r = r/255.0;

                double min = Math.Min(r, Math.Min(g, b));
                double max = Math.Max(r, Math.Max(g, b));
                double chroma = max - min;
                hsv[p + 2] = 0.0;
                hsv[p + 1] = 0.0;
                if (chroma != 0.0) {
                    if (Math.Abs(r - max) < 0.00001) {
                        hsv[p + 2] = ((g - b)/chroma);
                        hsv[p + 2] = hsv[p + 2]%6.0;
                    } else if (Math.Abs(g - max) < 0.00001) {
                        hsv[p + 2] = ((b - r)/chroma) + 2;
                    } else {
                        hsv[p + 2] = ((r - g)/chroma) + 4;
                    }

                    hsv[p + 2] = hsv[p + 2]*60.0;
                    if (hsv[p + 2] < 0.0) {
                        hsv[p + 2] = hsv[p + 2] + 360.0;
                    }
                    hsv[p + 1] = chroma/max;
                }
                hsv[p] = max;

                hsv[p] = hsv[p]*255.0;

                if (hsv[p] > 255.0) {
                    hsv[p] = 255.0;
                }
                if (hsv[p] < 0.0) {
                    hsv[p] = 0.0;
                }

                int k = (int)hsv[p];
                histogramV[k]++;
            });

            for (int i = 0; i < 256; i++) {
                sumHistogramEqualizationV[i] = histogramV[i]/(double)(data.M_width*data.M_height);
            }

            sumHistogramV[0] = sumHistogramEqualizationV[0];
            for (int i = 1; i < 256; i++) {
                sumHistogramV[i] = sumHistogramV[i - 1] + sumHistogramEqualizationV[i];
            }

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
                double b = rgb[p];
                double g = rgb[p + 1];
                double r = rgb[p + 2];

                int k = (int)hsv[p];
                hsv[p] = (byte)Math.Round(sumHistogramV[k]*255.0);
                hsv[p] = hsv[p]/255;

                double c = hsv[p]*hsv[p + 1];
                hsv[p + 2] = hsv[p + 2]/60.0;
                hsv[p + 2] = hsv[p + 2]%2;
                double x = c*(1.0 - Math.Abs(hsv[p + 2] - 1.0));
                double m = hsv[p] - c;

                if (hsv[p + 2] >= 0.0 && hsv[p + 2] < 60.0) {
                    b = 0;
                    g = x;
                    r = c;
                } else if (hsv[p + 2] >= 60.0 && hsv[p + 2] < 120.0) {
                    b = 0;
                    g = c;
                    r = x;
                } else if (hsv[p + 2] >= 120.0 && hsv[p + 2] < 180.0) {
                    b = x;
                    g = c;
                    r = 0;
                } else if (hsv[p + 2] >= 180.0 && hsv[p + 2] < 240.0) {
                    b = c;
                    g = x;
                    r = 0;
                } else if (hsv[p + 2] >= 240.0 && hsv[p + 2] < 300.0) {
                    b = c;
                    g = 0;
                    r = x;
                } else if (hsv[p + 2] >= 300.0 && hsv[p + 2] < 360.0) {
                    b = x;
                    g = 0;
                    r = c;
                }

                b = b + m;
                g = g + m;
                r = r + m;

                b = b*255.0;
                g = g*255.0;
                r = r*255.0;

                if (r > 255.0) {
                    r = 255.0;
                }

                if (r < 0.0) {
                    r = 0.0;
                }

                if (g > 255.0) {
                    g = 255.0;
                }

                if (g < 0.0) {
                    g = 0.0;
                }

                if (b > 255.0) {
                    b = 255.0;
                }

                if (b < 0.0) {
                    b = 0.0;
                }

                rgb[p] = (byte)b;
                rgb[p + 1] = (byte)g;
                rgb[p + 2] = (byte)r;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Histogram equalization [YUV]
        /// <summary>
        /// Histogram equalization at the YUV color space.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan HistogramEqualization_YUV(ImageData data) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];
            double[] yuv = new double[bytes];
            int[] histogramY = new int[256];
            double[] sumHistogramEqualizationY = new double[256];
            double[] sumHistogramY = new double[256];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.For(0, 256, i => {
                histogramY[i] = 0;
                sumHistogramEqualizationY[i] = 0;
                sumHistogramY[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
                double b = rgb[p];
                double g = rgb[p + 1];
                double r = rgb[p + 2];
                b = b/255.0;
                g = g/255.0;
                r = r/255.0;

                yuv[p] = (0.299*r) + (0.587*g) + (0.114*b);
                yuv[p + 1] = (-0.14713*r) - (0.28886*g) + (0.436*b);
                yuv[p + 2] = (0.615*r) - (0.51499*g) - (0.10001*b);

                yuv[p] = yuv[p]*255.0;
                if (yuv[p] > 255.0) {
                    yuv[p] = 255.0;
                }

                if (yuv[p] < 0.0) {
                    yuv[p] = 0.0;
                }

                int k = (int)yuv[p];
                histogramY[k]++;
            });

            for (int i = 0; i < 256; i++) {
                sumHistogramEqualizationY[i] = histogramY[i]/(double)(data.M_bitmap.Width*data.M_bitmap.Height);
            }

            sumHistogramY[0] = sumHistogramEqualizationY[0];
            for (int i = 1; i < 256; i++) {
                sumHistogramY[i] = sumHistogramY[i - 1] + sumHistogramEqualizationY[i];
            }

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
                double b = 0;
                double g = 0;
                double r = 0;

                int k = (int)yuv[p];
                yuv[p] = (byte)Math.Round(sumHistogramY[k]*255.0);
                yuv[p] = yuv[p]/255;

                r = yuv[p] + (0.0*yuv[p + 1]) + (1.13983*yuv[p + 2]);
                g = yuv[p] + (-0.39465*yuv[p + 1]) + (-0.58060*yuv[p + 2]);
                b = yuv[p] + (2.03211*yuv[p + 1]) + (0.0*yuv[p + 2]);

                r = r*255.0;
                g = g*255.0;
                b = b*255.0;

                if (r > 255.0) {
                    r = 255.0;
                }

                if (r < 0.0) {
                    r = 0.0;
                }

                if (g > 255.0) {
                    g = 255.0;
                }

                if (g < 0.0) {
                    g = 0.0;
                }

                if (b > 255.0) {
                    b = 255.0;
                }

                if (b < 0.0) {
                    b = 0.0;
                }

                rgb[p + 2] = (byte)r;
                rgb[p + 1] = (byte)g;
                rgb[p] = (byte)b;
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Image summarization
        /// <summary>
        /// Image Summarization.
        /// <para>Adding the same image, increasing its brightness.</para>
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan ImageSummarization(ImageData data) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
                int b = rgb[p] + rgb[p]; // B
                int g = rgb[p + 1] + rgb[p + 1]; // G
                int r = rgb[p + 2] + rgb[p + 2]; // R

                if (r > 255) {
                    r = 255;
                }

                if (g > 255) {
                    g = 255;
                }

                if (b > 255) {
                    b = 255;
                }

                rgb[p] = (byte)b; // B
                rgb[p + 1] = (byte)g; // G
                rgb[p + 2] = (byte)r; // R
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Image subtraction
        /// <summary>
        /// Image subtraction.
        /// <para>Subtract image from itselft which results to a black image. Not useful at all. It will be chnaged soon.</para>
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan ImageSubtraction(ImageData data) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, bytes, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
                int b = rgb[p] - rgb[p]; // B
                int g = rgb[p + 1] - rgb[p + 1]; // G
                int r = rgb[p + 2] - rgb[p + 2]; // R

                rgb[p] = (byte)b; // B
                rgb[p + 1] = (byte)g; // G
                rgb[p + 2] = (byte)r; // R
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Edge detection [Sobel]
        /// <summary>
        /// Sobel edge detector.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="kernelSize">Kernel size.</param>
        /// <param name="kernelX">X axis kernel</param>
        /// <param name="kernelY">Y axis kernel</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan EdgeDetection_Sobel(ImageData data, int kernelSize, double[,] kernelX, double[,] kernelY) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare 2 arrays to hold the bytes of the bitmap.
            // The first to read from and then write to the scond.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] bgrValues = new byte[bytes];

            // Calculate the offset regarding the size of the kernel.
            int filterOffset = (kernelSize - 1)/2;

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            Parallel.For(filterOffset, bmpData.Width - filterOffset, i => {
                for (int j = filterOffset; j < bmpData.Height - filterOffset; j++) {
                    int index = 0;
                    double txR = 0;
                    double txG = 0;
                    double txB = 0;
                    for (int k = 0; k < kernelSize; k++) {
                        for (int l = 0; l < kernelSize; l++) {
                            int indexX = ((j + l - filterOffset)*bmpData.Stride) + ((i + k - filterOffset)*bytesPerPixel);
                            txR = txR + rgbValues[indexX + 2]*kernelX[k, l];
                            txG = txG + rgbValues[indexX + 1]*kernelX[k, l];
                            txB = txB + rgbValues[indexX]*kernelX[k, l];
                        }
                    }

                    double tyR = 0;
                    double tyG = 0;
                    double tyB = 0;
                    for (int k = 0; k < kernelSize; k++) {
                        for (int l = 0; l < kernelSize; l++) {
                            int indexY = ((j + l - filterOffset)*bmpData.Stride) + ((i + k - filterOffset)*bytesPerPixel);
                            tyR = tyR + rgbValues[indexY + 2]*kernelY[k, l];
                            tyG = tyG + rgbValues[indexY + 1]*kernelY[k, l];
                            tyB = tyB + rgbValues[indexY]*kernelY[k, l];
                        }
                    }

                    double tR = Math.Sqrt(txR*txR + tyR*tyR);
                    double tG = Math.Sqrt(txG*txG + tyG*tyG);
                    double tB = Math.Sqrt(txB*txB + tyB*tyB);

                    if (tR > 255.0) {
                        tR = 255.0;
                    } else if (tR < 0.0) {
                        tR = 0.0;
                    }

                    if (tG > 255.0) {
                        tG = 255.0;
                    } else if (tG < 0.0) {
                        tG = 0.0;
                    }

                    if (tB > 255.0) {
                        tB = 255.0;
                    } else if (tB < 0.0) {
                        tB = 0.0;
                    }

                    index = (j*bmpData.Stride) + (i*bytesPerPixel);

                    bgrValues[index + 2] = (byte)tR;
                    bgrValues[index + 1] = (byte)tG;
                    bgrValues[index] = (byte)tB;
                }
            });
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(bgrValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);


            return elapsedTime;
        }
        #endregion

        #region Image Convolution
        /// <summary>
        /// Convolution algorithm.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="kernelSize">Kernel size.</param>
        /// <param name="kernel">Kernel.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan ImageConvolution(ImageData data, int kernelSize, double[,] kernel) {
            double sumMask = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare 2 arrays to hold the bytes of the bitmap.
            // The first to read from and then write to the scond.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] bgrValues = new byte[bytes];

            // Calculate the offset regarding the size of the kernel.
            int filterOffset = (kernelSize - 1)/2;

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            #region Algorithm
            for (int i = 0; i < kernelSize; i++) {
                for (int j = 0; j < kernelSize; j++) {
                    sumMask += kernel[i, j];
                }
            }

            Parallel.For(filterOffset, bmpData.Width - filterOffset, i => {
                for (int j = filterOffset; j < bmpData.Height - filterOffset; j++) {
                    int index = 0;
                    double tR = 0.0;
                    double tG = 0.0;
                    double tB = 0.0;
                    for (int k = 0; k < kernelSize; k++) {
                        for (int l = 0; l < kernelSize; l++) {
                            int indexX = ((j + l - filterOffset)*bmpData.Stride) + ((i + k - filterOffset)*bytesPerPixel);
                            tR = tR + (rgbValues[indexX + 2]*kernel[k, l])/sumMask;
                            tG = tG + (rgbValues[indexX + 1]*kernel[k, l])/sumMask;
                            tB = tB + (rgbValues[indexX]*kernel[k, l])/sumMask;
                        }
                    }

                    if (tR > 255.0) {
                        tR = 255.0;
                    } else if (tR < 0.0) {
                        tR = 0.0;
                    }

                    if (tG > 255.0) {
                        tG = 255.0;
                    } else if (tG < 0.0) {
                        tG = 0.0;
                    }

                    if (tB > 255.0) {
                        tB = 255.0;
                    } else if (tB < 0.0) {
                        tB = 0.0;
                    }

                    index = (j*bmpData.Stride) + (i*bytesPerPixel);

                    bgrValues[index + 2] = (byte)tR;
                    bgrValues[index + 1] = (byte)tG;
                    bgrValues[index] = (byte)tB;
                }
            });
            #endregion

            Marshal.Copy(bgrValues, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Salt and Pepper noise generator [Color]
        /// <summary>
        /// Colored Salt and Pepper noise generator.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="probability">Probability.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan SaltPepperNoise_Color(ImageData data, double probability) {
            int d = 0;
            int d1 = 0;
            int d2 = 0;
            int index = 0;
            Random rand = new Random();

            d = (int)(probability*32768/2);
            d1 = d + 16384;
            d2 = 16384 - d;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            for (int i = 0; i < bmpData.Width; i++) {
                for (int j = 0; j < bmpData.Height; j++) {
                    index = (j*bmpData.Stride) + (i*bytesPerPixel);

                    d = rand.Next(32768);
                    if (d >= 16384 && d < d1) {
                        rgb[index + 2] = 0;
                    }
                    if (d >= d2 && d <= 16384) {
                        rgb[index + 2] = 255;
                    }

                    d = rand.Next(32768);
                    if (d >= 16384 && d < d1) {
                        rgb[index + 1] = 0;
                    }
                    if (d >= d2 && d <= 16384) {
                        rgb[index + 1] = 255;
                    }

                    d = rand.Next(32768);
                    if (d >= 16384 && d < d1) {
                        rgb[index] = 0;
                    }
                    if (d >= d2 && d <= 16384) {
                        rgb[index] = 255;
                    }
                }
            }
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Salt and Pepper Noise generator [BW]
        /// <summary>
        /// Black and white Salt and Pepper noise generator.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="probability">Probability.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan SaltPepperNoise_BW(ImageData data, double probability) {
            int d = 0;
            int d1 = 0;
            int d2 = 0;
            int index = 0;
            Random rand = new Random();

            d = (int)(probability*32768/2);
            d1 = d + 16384;
            d2 = 16384 - d;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            for (int i = 0; i < bmpData.Width; i++) {
                for (int j = 0; j < bmpData.Height; j++) {
                    index = (j*bmpData.Stride) + (i*bytesPerPixel);

                    d = rand.Next(32768);
                    if (d >= 16384 && d < d1) {
                        rgb[index] = 0;
                        rgb[index + 1] = 0;
                        rgb[index + 2] = 0;
                    }
                    if (d >= d2 && d <= 16384) {
                        rgb[index] = 255;
                        rgb[index + 1] = 255;
                        rgb[index + 2] = 255;
                    }
                }
            }
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Noise reduction filter [Mean]
        /// <summary>
        /// Noise reduction filter (Arithmetic Mean algorithm).
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="kernelSize">Kernel size.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan NoiseReduction_Mean(ImageData data, int kernelSize) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.For(kernelSize/2, bmpData.Width - kernelSize/2, i => {
                for (int j = kernelSize/2; j < bmpData.Height - kernelSize/2; j++) {
                    int index = 0;
                    int sumR = 0;
                    int sumG = 0;
                    int sumB = 0;
                    for (int k = -kernelSize/2; k <= kernelSize/2; k++) {
                        for (int l = -kernelSize/2; l <= kernelSize/2; l++) {
                            int indexX = ((j + l)*bmpData.Stride) + ((i + k)*3);
                            sumB += rgb[indexX];
                            sumG += rgb[indexX + 1];
                            sumR += rgb[indexX + 2];
                        }
                    }

                    index = (j*bmpData.Stride) + (i*bytesPerPixel);

                    rgb[index] = (byte)(sumB/(kernelSize*kernelSize));
                    rgb[index + 1] = (byte)(sumG/(kernelSize*kernelSize));
                    rgb[index + 2] = (byte)(sumR/(kernelSize*kernelSize));
                }
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Noise reduction filter [Median]
        /// <summary>
        /// Noise reduction filter (Median algorithm).
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="kernelSize">Kernel size.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan NoiseReduction_Median(ImageData data, int kernelSize) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            Parallel.For(kernelSize/2, bmpData.Width - kernelSize/2, i => {
                int[] arR = new int[121];
                int[] arG = new int[121];
                int[] arB = new int[121];
                for (int j = kernelSize/2; j < bmpData.Height - kernelSize/2; j++) {
                    int index = 0;
                    int z = 0;
                    for (int k = -kernelSize/2; k <= kernelSize/2; k++) {
                        for (int l = -kernelSize/2; l <= kernelSize/2; l++) {
                            int indexX = ((j + l)*bmpData.Stride) + ((i + k)*bytesPerPixel);
                            arR[z] = rgb[indexX + 2];
                            arG[z] = rgb[indexX + 1];
                            arB[z] = rgb[indexX];
                            z++;
                        }
                    }

                    for (int k = 1; k <= kernelSize*kernelSize - 1; k++) {
                        int aR = arR[k];
                        int l = k - 1;

                        while (l >= 0 && arR[l] > aR) {
                            arR[l + 1] = arR[l];
                            l--;
                        }

                        arR[l + 1] = aR;
                    }

                    for (int k = 1; k <= kernelSize*kernelSize - 1; k++) {
                        int aG = arG[k];
                        int l = k - 1;

                        while (l >= 0 && arG[l] > aG) {
                            arG[l + 1] = arG[l];
                            l--;
                        }

                        arG[l + 1] = aG;
                    }

                    for (int k = 1; k <= kernelSize*kernelSize - 1; k++) {
                        int aB = arB[k];
                        int l = k - 1;

                        while (l >= 0 && arB[l] > aB) {
                            arB[l + 1] = arB[l];
                            l--;
                        }

                        arB[l + 1] = aB;
                    }

                    index = (j*bmpData.Stride) + (i*bytesPerPixel);

                    rgb[index + 2] = (byte)arR[kernelSize*kernelSize/2];
                    rgb[index + 1] = (byte)arG[kernelSize*kernelSize/2];
                    rgb[index] = (byte)arB[kernelSize*kernelSize/2];
                }
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Median filter (gradient based) [Not working for now]
        /// <summary>
        /// Median filter (gradient based).
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="kernelSize">Kernel size.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan MedianFilter(ImageData data, int kernelSize) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];
            byte[] bgr = new byte[bytes];

            // Calculate the offset regarding the size of the kernel.
            int filterOffset = (kernelSize - 1)/2;
            int calcOffset = 0;
            int byteOffset = 0;

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            // List of neighbouring pixels
            List<int> neighbourPixels = new List<int>();

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            /*for (int i = filterOffset; i < bmpData.Width - filterOffset; i++) {
                for (int j = filterOffset; j < bmpData.Height - filterOffset; j++) {
                    int index = (j*bmpData.Stride) + (i*bytesPerPixel);

                    neighbourPixels.Clear();
                    for (int k = 0; k < kernelSize; k++) {
                        for (int l = 0; l < kernelSize; l++) {
                            int indexX = ((j + l - filterOffset)*bmpData.Stride) + ((i + k - filterOffset)*bytesPerPixel);
                            //int indexX = index + (l*bytesPerPixel) + (k*bmpData.Stride);

                            neighbourPixels.Add(BitConverter.ToInt32(rgb, indexX));
                        }
                    }
                    neighbourPixels.Sort();

                    byte[] middlePixel = BitConverter.GetBytes(neighbourPixels[filterOffset]);

                    bgr[index] = middlePixel[0];
                    bgr[index + 1] = middlePixel[1];
                    bgr[index + 2] = middlePixel[2];
                }
            }*/

            for (int offsetY = filterOffset; offsetY < bmpData.Height - filterOffset; offsetY++) {
                for (int offsetX = filterOffset; offsetX < bmpData.Width - filterOffset; offsetX++) {
                    byteOffset = offsetY*bmpData.Stride + offsetX*3;

                    neighbourPixels.Clear();
                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++) {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++) {
                            calcOffset = byteOffset + (filterX*3) + (filterY*bmpData.Stride);

                            neighbourPixels.Add(BitConverter.ToInt32(rgb, calcOffset));
                        }
                    }
                    neighbourPixels.Sort();

                    byte[] middlePixel = BitConverter.GetBytes(neighbourPixels[filterOffset]);

                    bgr[byteOffset] = middlePixel[0];
                    bgr[byteOffset + 1] = middlePixel[1];
                    bgr[byteOffset + 2] = middlePixel[2];
                }
            }
            #endregion

            Marshal.Copy(bgr, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Cartoon effect
        /// <summary>
        /// Cartoon effect filter.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="threshold">Threshold.</param>
        /// <param name="smoothFilter">Smoothing filter.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan CartoonEffect(ImageData data, byte threshold = 0, KernelType smoothFilter = KernelType.None) {
            // Choose a smoothing filter
            switch (smoothFilter) {
                case KernelType.None:
                    break;
                case KernelType.Gaussian3x3:
                    ImageConvolution(data, 3, Kernel.M_Gaussian3x3);
                    break;
                case KernelType.Gaussian5x5:
                    ImageConvolution(data, 5, Kernel.M_Gaussian5x5);
                    break;
                case KernelType.Gaussian7x7:
                    ImageConvolution(data, 7, Kernel.M_Gaussian7x7);
                    break;
                case KernelType.Median3x3:
                    NoiseReduction_Median(data, 3);
                    break;
                case KernelType.Median5x5:
                    NoiseReduction_Median(data, 5);
                    break;
                case KernelType.Median7x7:
                    NoiseReduction_Median(data, 7);
                    break;
                case KernelType.Median9x9:
                    NoiseReduction_Median(data, 9);
                    break;
                case KernelType.Mean3x3:
                    ImageConvolution(data, 3, Kernel.M_Mean3x3);
                    break;
                case KernelType.Mean5x5:
                    ImageConvolution(data, 5, Kernel.M_Mean5x5);
                    break;
                case KernelType.Mean7x7:
                    ImageConvolution(data, 7, Kernel.M_Mean7x7);
                    break;
                case KernelType.LowPass3x3:
                    ImageConvolution(data, 3, Kernel.M_LowPass3x3);
                    break;
                case KernelType.LowPass5x5:
                    ImageConvolution(data, 5, Kernel.M_LowPass5x5);
                    break;
                case KernelType.Sharpen3x3:
                    ImageConvolution(data, 3, Kernel.M_Sharpen3x3);
                    break;
                case KernelType.Sharpen5x5:
                    ImageConvolution(data, 5, Kernel.M_Sharpen5x5);
                    break;
                case KernelType.Sharpen7x7:
                    ImageConvolution(data, 7, Kernel.M_Sharpen7x7);
                    break;
            }

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare 2 arrays to hold the bytes of the bitmap.
            // The first to read from and then write to the scond.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] bgrValues = new byte[bytes];

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            #region Algorithm
            Parallel.For(1, bmpData.Height - 1, i => {
                for (int j = 1; j < bmpData.Width - 1; j++) {
                    int index = i*bmpData.Stride + j*bytesPerPixel;

                    int bGradient = Math.Abs(rgbValues[index - bytesPerPixel] - rgbValues[index + bytesPerPixel]);
                    int gGradient = Math.Abs(rgbValues[index - bytesPerPixel] - rgbValues[index + bytesPerPixel]);
                    int rGradient = Math.Abs(rgbValues[index - bytesPerPixel] - rgbValues[index + bytesPerPixel]);

                    bGradient += Math.Abs(rgbValues[index - bmpData.Stride] - rgbValues[index + bmpData.Stride]);
                    gGradient += Math.Abs(rgbValues[index - bmpData.Stride] - rgbValues[index + bmpData.Stride]);
                    rGradient += Math.Abs(rgbValues[index - bmpData.Stride] - rgbValues[index + bmpData.Stride]);

                    bool exceedsThreshold = false;
                    if (bGradient + gGradient + rGradient > threshold) {
                        exceedsThreshold = true;
                    } else {
                        bGradient = Math.Abs(rgbValues[index - bytesPerPixel] - rgbValues[index + bytesPerPixel]);
                        gGradient = Math.Abs(rgbValues[index - bytesPerPixel] - rgbValues[index + bytesPerPixel]);
                        rGradient = Math.Abs(rgbValues[index - bytesPerPixel] - rgbValues[index + bytesPerPixel]);

                        if (bGradient + gGradient + rGradient > threshold) {
                            exceedsThreshold = true;
                        } else {
                            bGradient = Math.Abs(rgbValues[index - bmpData.Stride] - rgbValues[index + bmpData.Stride]);
                            gGradient = Math.Abs(rgbValues[index - bmpData.Stride] - rgbValues[index + bmpData.Stride]);
                            rGradient = Math.Abs(rgbValues[index - bmpData.Stride] - rgbValues[index + bmpData.Stride]);

                            if (bGradient + gGradient + rGradient > threshold) {
                                exceedsThreshold = true;
                            } else {
                                bGradient = Math.Abs(rgbValues[index - bytesPerPixel - bmpData.Stride] - rgbValues[index + bytesPerPixel + bmpData.Stride]);
                                gGradient = Math.Abs(rgbValues[index - bytesPerPixel - bmpData.Stride] - rgbValues[index + bytesPerPixel + bmpData.Stride]);
                                rGradient = Math.Abs(rgbValues[index - bytesPerPixel - bmpData.Stride] - rgbValues[index + bytesPerPixel + bmpData.Stride]);

                                gGradient += Math.Abs(rgbValues[index - bmpData.Stride + bytesPerPixel] - rgbValues[index + bmpData.Stride - bytesPerPixel]);
                                bGradient += Math.Abs(rgbValues[index - bmpData.Stride + bytesPerPixel] - rgbValues[index + bmpData.Stride - bytesPerPixel]);
                                rGradient += Math.Abs(rgbValues[index - bmpData.Stride + bytesPerPixel] - rgbValues[index + bmpData.Stride - bytesPerPixel]);

                                if (bGradient + gGradient + rGradient > threshold) {
                                    exceedsThreshold = true;
                                } else {
                                    exceedsThreshold = false;
                                }
                            }
                        }
                    }

                    int b = 0;
                    int g = 0;
                    int r = 0;
                    if (exceedsThreshold) {
                        b = 0;
                        g = 0;
                        r = 0;
                    } else {
                        b = rgbValues[index];
                        g = rgbValues[index + 1];
                        r = rgbValues[index + 2];
                    }

                    if (b > 255) {
                        b = 255;
                    } else if (b < 0) {
                        b = 0;
                    }
                    if (g > 255) {
                        g = 255;
                    } else if (g < 0) {
                        g = 0;
                    }
                    if (r > 255) {
                        r = 255;
                    } else if (r < 0) {
                        r = 0;
                    }

                    bgrValues[index] = (byte)b;
                    bgrValues[index + 1] = (byte)g;
                    bgrValues[index + 2] = (byte)r;
                }
            });
            #endregion

            Marshal.Copy(bgrValues, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Oil paint effect
        /// <summary>
        /// Cartoon effect filter.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="levels">Intensity levels.</param>
        /// <param name="kernelSize">Kernel size.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan OilPaintEffect(ImageData data, int levels, int kernelSize) {

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare 2 arrays to hold the bytes of the bitmap.
            // The first to read from and then write to the scond.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] bgrValues = new byte[bytes];

            // Calculate the offset regarding the size of the kernel.
            int filterOffset = (kernelSize - 1)/2;

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            levels--;

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            #region Algorithm
            for (int i = filterOffset; i < bmpData.Width - filterOffset; i++) {
                for (int j = filterOffset; j < bmpData.Height - filterOffset; j++) {
                    int index = j*bmpData.Stride + i*bytesPerPixel;

                    double b = 0;
                    double g = 0;
                    double r = 0;
                    int maxIntensity = 0;
                    int maxIndex = 0;
                    int[] intensityBin = new int[levels + 1];
                    int[] bBin = new int[levels + 1];
                    int[] gBin = new int[levels + 1];
                    int[] rBin = new int[levels + 1];

                    for (int k = 0; k < kernelSize; k++) {
                        for (int l = 0; l < kernelSize; l++) {
                            int indexX = ((j + l - filterOffset)*bmpData.Stride) + ((i + k - filterOffset)*bytesPerPixel);

                            int currentIntensity = (int)Math.Round(((double)(rgbValues[indexX] + rgbValues[indexX + 1] + rgbValues[indexX + 2])/3.0*(levels))/255.0);

                            intensityBin[currentIntensity] += 1;
                            bBin[currentIntensity] += rgbValues[indexX];
                            gBin[currentIntensity] += rgbValues[indexX + 1];
                            rBin[currentIntensity] += rgbValues[indexX + 2];

                            if (intensityBin[currentIntensity] > maxIntensity) {
                                maxIntensity = intensityBin[currentIntensity];
                                maxIndex = currentIntensity;
                            }
                        }
                    }

                    b = bBin[maxIndex]/(double)maxIntensity;
                    g = gBin[maxIndex]/(double)maxIntensity;
                    r = rBin[maxIndex]/(double)maxIntensity;

                    if (r > 255.0) {
                        r = 255.0;
                    } else if (r < 0.0) {
                        r = 0.0;
                    }

                    if (g > 255.0) {
                        g = 255.0;
                    } else if (g < 0.0) {
                        g = 0.0;
                    }

                    if (b > 255.0) {
                        b = 255.0;
                    } else if (b < 0.0) {
                        b = 0.0;
                    }

                    bgrValues[index] = (byte)b;
                    bgrValues[index + 1] = (byte)g;
                    bgrValues[index + 2] = (byte)r;
                }
            }
            #endregion

            Marshal.Copy(bgrValues, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Edge detection [Gradient-based]
        /// <summary>
        /// Cartoon effect filter.
        /// </summary>
        /// <param name="data">Image data.</param>
        /// <param name="filterType">Filter type.</param>
        /// <param name="derivativeLevel">Derivative</param>
        /// <param name="rFactor">Red factor</param>
        /// <param name="gFactor">Green factor.</param>
        /// <param name="bFactor">Blue factor.</param>
        /// <param name="threshold">Threshold.</param>
        /// <returns>Execution time.</returns>
        public static TimeSpan EdgeDetection_GradientBased(ImageData data, EdgeFilterType filterType, DerivativeLevel derivativeLevel, double rFactor = 1.0, double gFactor = 1.0, double bFactor = 1.0, byte threshold = 0) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare 2 arrays to hold the bytes of the bitmap.
            // The first to read from and then write to the scond.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] bgrValues = new byte[bytes];
            int derivative = (int)derivativeLevel;

            // Get the bytes per pixel value.
            int bytesPerPixel = Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8;

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            #region Algorithm
            for (int i = 1; i < bmpData.Height - 1; i++) {
                for (int j = 1; j < bmpData.Width - 1; j++) {
                    int index = i*bmpData.Stride + j*bytesPerPixel;

                    int bGradient = Math.Abs(rgbValues[index - bytesPerPixel] - rgbValues[index + bytesPerPixel])/derivative;
                    int gGradient = Math.Abs(rgbValues[index - bytesPerPixel] - rgbValues[index + bytesPerPixel])/derivative;
                    int rGradient = Math.Abs(rgbValues[index - bytesPerPixel] - rgbValues[index + bytesPerPixel])/derivative;

                    bGradient += Math.Abs(rgbValues[index - bmpData.Stride] - rgbValues[index + bmpData.Stride])/derivative;
                    gGradient += Math.Abs(rgbValues[index - bmpData.Stride] - rgbValues[index + bmpData.Stride])/derivative;
                    rGradient += Math.Abs(rgbValues[index - bmpData.Stride] - rgbValues[index + bmpData.Stride])/derivative;

                    bool exceedsThreshold = false;
                    if (bGradient + gGradient + rGradient > threshold) {
                        exceedsThreshold = true;
                    } else {
                        bGradient = Math.Abs(rgbValues[index - bytesPerPixel] - rgbValues[index + bytesPerPixel]);
                        gGradient = Math.Abs(rgbValues[index - bytesPerPixel] - rgbValues[index + bytesPerPixel]);
                        rGradient = Math.Abs(rgbValues[index - bytesPerPixel] - rgbValues[index + bytesPerPixel]);

                        if (bGradient + gGradient + rGradient > threshold) {
                            exceedsThreshold = true;
                        } else {
                            bGradient = Math.Abs(rgbValues[index - bmpData.Stride] - rgbValues[index + bmpData.Stride]);
                            gGradient = Math.Abs(rgbValues[index - bmpData.Stride] - rgbValues[index + bmpData.Stride]);
                            rGradient = Math.Abs(rgbValues[index - bmpData.Stride] - rgbValues[index + bmpData.Stride]);

                            if (bGradient + gGradient + rGradient > threshold) {
                                exceedsThreshold = true;
                            } else {
                                bGradient = Math.Abs(rgbValues[index - bytesPerPixel - bmpData.Stride] - rgbValues[index + bytesPerPixel + bmpData.Stride])/derivative;
                                gGradient = Math.Abs(rgbValues[index - bytesPerPixel - bmpData.Stride] - rgbValues[index + bytesPerPixel + bmpData.Stride])/derivative;
                                rGradient = Math.Abs(rgbValues[index - bytesPerPixel - bmpData.Stride] - rgbValues[index + bytesPerPixel + bmpData.Stride])/derivative;

                                gGradient += Math.Abs(rgbValues[index - bmpData.Stride + bytesPerPixel] - rgbValues[index + bmpData.Stride - bytesPerPixel])/derivative;
                                bGradient += Math.Abs(rgbValues[index - bmpData.Stride + bytesPerPixel] - rgbValues[index + bmpData.Stride - bytesPerPixel])/derivative;
                                rGradient += Math.Abs(rgbValues[index - bmpData.Stride + bytesPerPixel] - rgbValues[index + bmpData.Stride - bytesPerPixel])/derivative;

                                if (bGradient + gGradient + rGradient > threshold) {
                                    exceedsThreshold = true;
                                } else {
                                    exceedsThreshold = false;
                                }
                            }
                        }
                    }

                    double b = 0.0;
                    double g = 0.0;
                    double r = 0.0;

                    if (exceedsThreshold) {
                        switch (filterType) {
                            case EdgeFilterType.EdgeDetectMono:
                                b = g = r = 255;
                                break;
                            case EdgeFilterType.EdgeDetectGradient:
                                b = bGradient*bFactor;
                                g = gGradient*gFactor;
                                r = rGradient*rFactor;
                                break;
                            case EdgeFilterType.Sharpen:
                                b = rgbValues[index]*bFactor;
                                g = rgbValues[index + 1]*gFactor;
                                r = rgbValues[index + 2]*rFactor;
                                break;
                            case EdgeFilterType.SharpenGradient:
                                b = rgbValues[index] + bGradient*bFactor;
                                g = rgbValues[index + 1] + gGradient*gFactor;
                                r = rgbValues[index + 2] + rGradient*rFactor;
                                break;
                        }
                    } else {
                        if (filterType == EdgeFilterType.EdgeDetectMono || filterType == EdgeFilterType.EdgeDetectGradient) {
                            b = g = r = 0;
                        } else if (filterType == EdgeFilterType.Sharpen || filterType == EdgeFilterType.SharpenGradient) {
                            b = rgbValues[index];
                            g = rgbValues[index + 1];
                            r = rgbValues[index + 2];
                        }
                    }

                    if (b > 255.0) {
                        b = 255.0;
                    } else if (b < 0.0) {
                        b = 0.0;
                    }
                    if (g > 255.0) {
                        g = 255.0;
                    } else if (g < 0.0) {
                        g = 0.0;
                    }
                    if (r > 255.0) {
                        r = 255.0;
                    } else if (r < 0.0) {
                        r = 0.0;
                    }

                    bgrValues[index] = (byte)b;
                    bgrValues[index + 1] = (byte)g;
                    bgrValues[index + 2] = (byte)r;
                }
            }
            #endregion

            Marshal.Copy(bgrValues, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion
    }
    #endregion
}
