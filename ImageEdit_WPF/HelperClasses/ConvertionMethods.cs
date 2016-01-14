using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageEdit_WPF.HelperClasses {
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

        #region Bitmap To BitmapImage
        /// <summary>
        /// <c>Bitmap</c> to <c>BitmpaImage</c> conversion method in order to show the edited image at the main window.
        /// </summary>
        /// <remarks>Extension method.</remarks>
        public static BitmapFrame BitmapToBitmapImage(this Bitmap bitmap) {
            // Convert Bitmap to BitmapImage
            MemoryStream str = new MemoryStream();
            bitmap.Save(str, ImageFormat.Bmp);
            str.Seek(0, SeekOrigin.Begin);
            BmpBitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

            return bdc.Frames[0];
        }
        #endregion

        #region GetEncoder
        /// <summary>
        /// Get the encoder info in order to use it at <c>Save</c> or <c>Save as...</c> method.
        /// </summary>
        /// <param name="format">Format of the image</param>
        /// <returns></returns>
        public static ImageCodecInfo GetEncoder(ImageFormat format) {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs) {
                if (codec.FormatID == format.Guid) {
                    return codec;
                }
            }
            return null;
        }
        #endregion
    }
}