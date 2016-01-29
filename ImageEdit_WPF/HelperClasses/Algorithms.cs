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
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ImageEdit_WPF.HelperClasses {
    public static class Algorithms {
        #region Shift bits
        public static TimeSpan ShiftBits(ImageData data, int bits) {
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*bmpData.Height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                rgb[k] = (byte)(b << bits); // B
                rgb[k + 1] = (byte)(g << bits); // G
                rgb[k + 2] = (byte)(r << bits); // R
            });

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Threshold
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
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
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

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Auto threshold
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
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

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

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
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

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
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

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Negative
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
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                rgb[k] = (byte)(255 - b); // B
                rgb[k + 1] = (byte)(255 - g); // G
                rgb[k + 2] = (byte)(255 - r); // R
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Square root
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
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                rgb[k] = (byte)Math.Sqrt(b*255); // B
                rgb[k + 1] = (byte)Math.Sqrt(g*255); // G
                rgb[k + 2] = (byte)Math.Sqrt(r*255); // R
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Contrast enhancement
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
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
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

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Brightness
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
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
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

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Contrast
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
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

            #region Algorithms
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
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

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

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
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

            Parallel.For(0, 256, i => {
                histogramR[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int r = rgb[k + 2];
                histogramR[r]++;
            });

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

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
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

            Parallel.For(0, 256, i => {
                histogramG[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int g = rgb[k + 1];
                histogramG[g]++;
            });

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

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
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

            Parallel.For(0, 256, i => {
                histogramB[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                histogramB[b]++;
            });

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

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
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

            Parallel.For(0, 256, i => {
                histogramY[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
                int b = rgb[k];
                int g = rgb[k + 1];
                int r = rgb[k + 2];
                int y = (int)(0.2126*r + 0.7152*g + 0.0722*b); // source = https://en.wikipedia.org/wiki/Grayscale#cite_note-5
                histogramY[y]++;
            });

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return histogramY;
        }
        #endregion

        #region Histogram equalization [RGB]
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
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

            #region Algorithm
            Parallel.For(0, 256, i => {
                histogramR[i] = 0;
                histogramG[i] = 0;
                histogramB[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
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

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), k => {
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

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Histogram equalization [HSV]
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
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

            #region Algorithm
            Parallel.For(0, 256, i => {
                histogramV[i] = 0;
                sumHistogramEqualizationV[i] = 0.0;
                sumHistogramV[i] = 0.0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
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

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
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

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Histogram equalization [YUV]
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
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

            #region Algorithm
            Parallel.For(0, 256, i => {
                histogramY[i] = 0;
                sumHistogramEqualizationY[i] = 0;
                sumHistogramY[i] = 0;
            });

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
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

            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
                double b = rgb[p];
                double g = rgb[p + 1];
                double r = rgb[p + 2];

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

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Image summarization
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
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
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

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Image subtraction
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
            Marshal.Copy(ptr, rgb, 0, rgb.Length);

            #region Algorithm
            Parallel.ForEach(BetterEnumerable.SteppedRange(0, rgb.Length, Image.GetPixelFormatSize(data.M_bitmap.PixelFormat)/8), p => {
                int b = rgb[p] - rgb[p]; // B
                int g = rgb[p + 1] - rgb[p + 1]; // G
                int r = rgb[p + 2] - rgb[p + 2]; // R

                rgb[p] = (byte)b; // B
                rgb[p + 1] = (byte)g; // G
                rgb[p + 2] = (byte)r; // R
            });
            #endregion

            Marshal.Copy(rgb, 0, ptr, rgb.Length);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Edge detection [Sobel]
        public static TimeSpan EdgeDetection_Sobel(ImageData data, int sizeMask, int[,] maskX, int[,] maskY) {
            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride*data.M_height;
            byte[] rgbValues = new byte[bytes];
            byte[] bgrValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            switch (sizeMask) {
                case 3:
                    for (int i = 1; i < data.M_width - 1; i++) {
                        for (int j = 1; j < data.M_height - 1; j++) {
                            int index;
                            double txR = 0;
                            double txG = 0;
                            double txB = 0;
                            for (int k = 0; k < sizeMask; k++) {
                                for (int l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 1)*bmpData.Stride) + ((i + k - 1)*3);
                                    txR = txR + rgbValues[index + 2]*maskX[k, l];
                                    txG = txG + rgbValues[index + 1]*maskX[k, l];
                                    txB = txB + rgbValues[index]*maskX[k, l];
                                }
                            }

                            double tyR = 0;
                            double tyG = 0;
                            double tyB = 0;
                            for (int k = 0; k < sizeMask; k++) {
                                for (int l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 1)*bmpData.Stride) + ((i + k - 1)*3);
                                    tyR = tyR + rgbValues[index + 2]*maskY[k, l];
                                    tyG = tyG + rgbValues[index + 1]*maskY[k, l];
                                    tyB = tyB + rgbValues[index]*maskY[k, l];
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

                            index = (j*bmpData.Stride) + (i*3);

                            bgrValues[index + 2] = (byte)tR;
                            bgrValues[index + 1] = (byte)tG;
                            bgrValues[index] = (byte)tB;
                        }
                    }
                    break;
                case 5:
                    for (int i = 2; i < data.M_width - 2; i++) {
                        for (int j = 2; j < data.M_height - 2; j++) {
                            int index;
                            double txR = 0;
                            double txG = 0;
                            double txB = 0;
                            for (int k = 0; k < sizeMask; k++) {
                                for (int l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 2)*bmpData.Stride) + ((i + k - 2)*3);
                                    txR = txR + rgbValues[index + 2]*maskX[k, l];
                                    txG = txG + rgbValues[index + 1]*maskX[k, l];
                                    txB = txB + rgbValues[index]*maskX[k, l];
                                }
                            }

                            double tyR = 0;
                            double tyG = 0;
                            double tyB = 0;
                            for (int k = 0; k < sizeMask; k++) {
                                for (int l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 2)*bmpData.Stride) + ((i + k - 2)*3);
                                    tyR = tyR + rgbValues[index + 2]*maskY[k, l];
                                    tyG = tyG + rgbValues[index + 1]*maskY[k, l];
                                    tyB = tyB + rgbValues[index]*maskY[k, l];
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

                            index = (j*bmpData.Stride) + (i*3);

                            bgrValues[index + 2] = (byte)tR;
                            bgrValues[index + 1] = (byte)tG;
                            bgrValues[index] = (byte)tB;
                        }
                    }
                    break;
                case 7:
                    for (int i = 3; i < data.M_width - 3; i++) {
                        for (int j = 3; j < data.M_height - 3; j++) {
                            int index;
                            double txR = 0;
                            double txG = 0;
                            double txB = 0;
                            for (int k = 0; k < sizeMask; k++) {
                                for (int l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 3)*bmpData.Stride) + ((i + k - 3)*3);
                                    txR = txR + rgbValues[index + 2]*maskX[k, l];
                                    txG = txG + rgbValues[index + 1]*maskX[k, l];
                                    txB = txB + rgbValues[index]*maskX[k, l];
                                }
                            }

                            double tyR = 0;
                            double tyG = 0;
                            double tyB = 0;
                            for (int k = 0; k < sizeMask; k++) {
                                for (int l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 3)*bmpData.Stride) + ((i + k - 3)*3);
                                    tyR = tyR + rgbValues[index + 2]*maskY[k, l];
                                    tyG = tyG + rgbValues[index + 1]*maskY[k, l];
                                    tyB = tyB + rgbValues[index]*maskY[k, l];
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

            // Copy the RGB values back to the bitmap
            Marshal.Copy(bgrValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);


            return elapsedTime;
        }
        #endregion

        #region Gaussian blur
        public static TimeSpan GaussianBlur(ImageData data, int sizeMask, int[,] maskX) {
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
            int bytes = bmpData.Stride * data.M_height;
            byte[] rgbValues = new byte[bytes];
            byte[] bgrValues = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            #region Algorithm
            switch (sizeMask) {
                case 3:
                    for (i = 0; i < sizeMask; i++) {
                        for (j = 0; j < sizeMask; j++) {
                            sumMask += maskX[i, j];
                        }
                    }

                    for (i = 1; i < data.M_width - 1; i++) {
                        for (j = 1; j < data.M_height - 1; j++) {
                            tR = 0.0;
                            tG = 0.0;
                            tB = 0.0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 1)*bmpData.Stride) + ((i + k - 1)*3);
                                    tR = tR + (double)(rgbValues[index + 2] * maskX[k, l]) / sumMask;
                                    tG = tG + (double)(rgbValues[index + 1] * maskX[k, l]) / sumMask;
                                    tB = tB + (double)(rgbValues[index] * maskX[k, l]) / sumMask;
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
                        for (j = 2; j < data.M_height - 2; j++) {
                            tR = 0.0;
                            tG = 0.0;
                            tB = 0.0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 2)*bmpData.Stride) + ((i + k - 2)*3);
                                    tR = tR + (double)(rgbValues[index + 2] * maskX[k, l]) / sumMask;
                                    tG = tG + (double)(rgbValues[index + 1] * maskX[k, l]) / sumMask;
                                    tB = tB + (double)(rgbValues[index] * maskX[k, l]) / sumMask;
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
                        for (j = 3; j < data.M_height - 3; j++) {
                            tR = 0.0;
                            tG = 0.0;
                            tB = 0.0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 3)*bmpData.Stride) + ((i + k - 3)*3);
                                    tR = tR + (double)(rgbValues[index + 2] * maskX[k, l]) / sumMask;
                                    tG = tG + (double)(rgbValues[index + 1] * maskX[k, l]) / sumMask;
                                    tB = tB + (double)(rgbValues[index] * maskX[k, l]) / sumMask;
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

            Marshal.Copy(bgrValues, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Sharpen
        public static TimeSpan Sharpen(ImageData data, int sizeMask, int[,] maskX) {
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
            int bytes = bmpData.Stride * data.M_height;
            byte[] rgbValues = new byte[bytes];
            byte[] bgrValues = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            #region Algorithm
            switch (sizeMask) {
                case 3:
                    for (i = 0; i < sizeMask; i++) {
                        for (j = 0; j < sizeMask; j++) {
                            sumMask += maskX[i, j];
                        }
                    }

                    for (i = 1; i < data.M_width - 1; i++) {
                        for (j = 1; j < data.M_height - 1; j++) {
                            tR = 0.0;
                            tG = 0.0;
                            tB = 0.0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 1)*bmpData.Stride) + ((i + k - 1)*3);
                                    tR = tR + (double)(rgbValues[index + 2] * maskX[k, l]) / sumMask;
                                    tG = tG + (double)(rgbValues[index + 1] * maskX[k, l]) / sumMask;
                                    tB = tB + (double)(rgbValues[index] * maskX[k, l]) / sumMask;
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
                        for (j = 2; j < data.M_height - 2; j++) {
                            tR = 0.0;
                            tG = 0.0;
                            tB = 0.0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 2)*bmpData.Stride) + ((i + k - 2)*3);
                                    tR = tR + (double)(rgbValues[index + 2] * maskX[k, l]) / sumMask;
                                    tG = tG + (double)(rgbValues[index + 1] * maskX[k, l]) / sumMask;
                                    tB = tB + (double)(rgbValues[index] * maskX[k, l]) / sumMask;
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
                        for (j = 3; j < data.M_height - 3; j++) {
                            tR = 0.0;
                            tG = 0.0;
                            tB = 0.0;
                            for (k = 0; k < sizeMask; k++) {
                                for (l = 0; l < sizeMask; l++) {
                                    index = ((j + l - 3)*bmpData.Stride) + ((i + k - 3)*3);
                                    tR = tR + (double)(rgbValues[index + 2] * maskX[k, l]) / sumMask;
                                    tG = tG + (double)(rgbValues[index + 1] * maskX[k, l]) / sumMask;
                                    tB = tB + (double)(rgbValues[index] * maskX[k, l]) / sumMask;
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

            Marshal.Copy(bgrValues, 0, ptr, bytes);

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
        }
        #endregion

        #region Salt and Pepper Noise generator [Color]
        public static TimeSpan SaltPepperNoise_Color(ImageData data, double probability) {
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
            int bytes = bmpData.Stride * data.M_height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

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
        public static TimeSpan SaltPepperNoise_BW(ImageData data, double probability) {
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
            int bytes = bmpData.Stride * data.M_height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            for (i = 0; i < data.M_width; i++) {
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);

                    d = rand.Next(32768);
                    if (d >= 16384 && d < d1) {
                        rgb[index + 2] = 0;
                        rgb[index + 1] = 0;
                        rgb[index] = 0;
                    }
                    if (d >= d2 && d <= 16384) {
                        rgb[index + 2] = 255;
                        rgb[index + 1] = 255;
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

        #region Noise reduction filter [Mean]
        public static TimeSpan NoiseReduction_Mean(ImageData data, int sizeMask) {
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
            int bytes = bmpData.Stride*data.M_height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            for (i = sizeMask/2; i < data.M_width - sizeMask/2; i++) {
                for (j = sizeMask/2; j < data.M_height - sizeMask/2; j++) {
                    sumR = 0;
                    sumG = 0;
                    sumB = 0;
                    for (k = -sizeMask/2; k <= sizeMask/2; k++) {
                        for (l = -sizeMask/2; l <= sizeMask/2; l++) {
                            index = ((j + l)*bmpData.Stride) + ((i + k)*3);
                            sumR = sumR + rgb[index + 2];
                            sumG = sumG + rgb[index + 1];
                            sumB = sumB + rgb[index];
                        }
                    }

                    index = (j*bmpData.Stride) + (i*3);

                    rgb[index + 2] = (byte)(sumR/(sizeMask*sizeMask));
                    rgb[index + 1] = (byte)(sumG/(sizeMask*sizeMask));
                    rgb[index] = (byte)(sumB/(sizeMask*sizeMask));
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

        #region Noise reduction filter [Median]
        public static TimeSpan NoiseReduction_Median(ImageData data, int sizeMask) {
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
            int bytes = bmpData.Stride * data.M_height;
            byte[] rgb = new byte[bytes];

            Stopwatch watch = Stopwatch.StartNew();

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            #region Algorithm
            for (i = sizeMask/2; i < data.M_width - sizeMask/2; i++) {
                for (j = sizeMask/2; j < data.M_height - sizeMask/2; j++) {
                    z = 0;
                    for (k = -sizeMask/2; k <= sizeMask/2; k++) {
                        for (l = -sizeMask/2; l <= sizeMask/2; l++) {
                            index = ((j + l)*bmpData.Stride) + ((i + k)*3);
                            arR[z] = rgb[index + 2];
                            arG[z] = rgb[index + 1];
                            arB[z] = rgb[index];
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

                    rgb[index + 2] = (byte)arR[sizeMask * sizeMask / 2];
                    rgb[index + 1] = (byte)arG[sizeMask * sizeMask / 2];
                    rgb[index] = (byte)arB[sizeMask * sizeMask / 2];
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
    }
}
