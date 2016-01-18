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

using System.Drawing.Imaging;

namespace ImageEdit_WPF.HelperClasses {
    public static class SaveImage {
        public static void Save(ImageFormat format, ImageData data) {
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