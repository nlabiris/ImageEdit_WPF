using System.Drawing.Imaging;

namespace ImageEdit_WPF.HelperClasses {
    public static class SaveImage {
        public static void Save(ImageFormat format, ImageEditData data) {
            ImageCodecInfo codec = GetEncoder(ImageFormat.Jpeg);
            Encoder quality = Encoder.Quality;
            EncoderParameters encoderArray = new EncoderParameters(1);
            EncoderParameter encoder = new EncoderParameter(quality, 85L);
            encoderArray.Param[0] = encoder;

            data.M_bitmap.Save(data.M_outputFilename, codec, encoderArray);
        }


        #region GetEncoder
        /// <summary>
        /// Get the encoder info in order to use it at <c>Save</c> or <c>Save as...</c> method.
        /// </summary>
        /// <param name="format">Format of the image</param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format) {
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