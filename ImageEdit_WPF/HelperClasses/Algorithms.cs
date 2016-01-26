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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageEdit_WPF.HelperClasses {
    public static class Algorithms {
        #region Shift bits
        public static TimeSpan ShiftBits(ImageData data, int bits, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan Threshold(ImageData data, int threshold, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan AutoThreshold(ImageData data, int distance, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan Negative(ImageData data, BackgroundWorker backgroundWorker)
        {
            #region unsafe
            unsafe {
                int i = 0;
                int j = 0;
                int index = 0;

                // Lock the bitmap's bits.  
                BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

                // Get the address of the first line.
                byte* ptr = (byte*)bmpData.Scan0;

                Stopwatch watch = Stopwatch.StartNew();

                #region Algorithm
                for (i = 0; i < data.M_width; i++) {
                    backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                    for (j = 0; j < data.M_height; j++) {
                        index = (j*bmpData.Stride) + (i*3);
                        ptr[index + 2] = (byte)(255 - ptr[index + 2]); // R
                        ptr[index + 1] = (byte)(255 - ptr[index + 1]); // G
                        ptr[index] = (byte)(255 - ptr[index]); // B
                    }
                }
                #endregion

                watch.Stop();
                TimeSpan elapsedTime = watch.Elapsed;

                // Unlock the bits.
                data.M_bitmap.UnlockBits(bmpData);

                return elapsedTime;
            }
            #endregion
        }
        #endregion

        #region Square root
        public static TimeSpan SquareRoot(ImageData data, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan ContrastEnhancement(ImageData data, int brightness, double contrast, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan Brightness(ImageData data, int brightness, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan Contrast(ImageData data, double contrast, BackgroundWorker backgroundWorker) {
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
        public static float HistogramMeanValue(ImageData data, int[] values) {
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
        public static int[] HistogramRed(ImageData data) {
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
        public static int[] HistogramGreen(ImageData data) {
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
        public static int[] HistogramBlue(ImageData data) {
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
        public static int[] HistogramLuminance(ImageData data) {
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
        public static TimeSpan HistogramEqualization_RGB(ImageData data, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan HistogramEqualization_HSV(ImageData data, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan HistogramEqualization_YUV(ImageData data, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan ImageSummarization(ImageData data, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan ImageSubtraction(ImageData data, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan EdgeDetection_Sobel(ImageData data, int sizeMask, int[,] maskX, int[,] maskY, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan GaussianBlur(ImageData data, int sizeMask, int[,] maskX, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan Sharpen(ImageData data, int sizeMask, int[,] maskX, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan SaltPepperNoise_Color(ImageData data, double probability, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan SaltPepperNoise_BW(ImageData data, double probability, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan NoiseReduction_Mean(ImageData data, int sizeMask, BackgroundWorker backgroundWorker) {
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
        public static TimeSpan NoiseReduction_Median(ImageData data, int sizeMask, BackgroundWorker backgroundWorker) {
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
}