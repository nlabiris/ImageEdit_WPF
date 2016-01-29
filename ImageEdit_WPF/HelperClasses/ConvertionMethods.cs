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

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageEdit_WPF.HelperClasses {
    /// <summary>
    /// Static class with extension methods for convertions between different type of classes that handle images.
    /// </summary>
    public static class ConvertionMethods {
        #region BitmapSource To BitmapImage
        /// <summary>
        /// Extension method
        /// </summary>
        /// <param name="bitmapsource"></param>
        /// <returns></returns>
        public static BitmapImage BitmapSourceToBitmapImage(this BitmapSource bitmapsource) {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapsource));
            MemoryStream stream = new MemoryStream();
            encoder.Save(stream);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = stream;
            bi.EndInit();

            return bi;
        }
        #endregion

        #region BitmapSource To Bitmap
        /// <summary>
        /// Extension method
        /// </summary>
        /// <param name="bitmapsource"></param>
        /// <returns></returns>
        public static Bitmap BitmapSourceToBitmap(this BitmapSource bitmapsource) {
            //convert image format
            FormatConvertedBitmap src = new FormatConvertedBitmap();
            src.BeginInit();
            src.Source = bitmapsource;
            src.DestinationFormat = System.Windows.Media.PixelFormats.Bgra32;
            src.EndInit();

            //copy to bitmap
            Bitmap bitmap = new Bitmap(src.PixelWidth, src.PixelHeight, PixelFormat.Format32bppArgb);
            BitmapData data = bitmap.LockBits(new Rectangle(System.Drawing.Point.Empty, bitmap.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            src.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height*data.Stride, data.Stride);
            bitmap.UnlockBits(data);

            return bitmap;
        }
        #endregion

        #region Bitmap To BitmapSource
        /// <summary>
        /// <c>Bitmap</c> to <c>BitmapSource</c> conversion method in order to show the edited image at the main window.
        /// </summary>
        /// <remarks>Extension method.</remarks>
        public static BitmapFrame BitmapToBitmapSource(this Bitmap bitmap) {
            // Convert Bitmap to BitmapImage
            MemoryStream str = new MemoryStream();
            bitmap.Save(str, ImageFormat.Bmp);
            str.Seek(0, SeekOrigin.Begin);
            BmpBitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

            return bdc.Frames[0];
        }
        #endregion
    }
}
