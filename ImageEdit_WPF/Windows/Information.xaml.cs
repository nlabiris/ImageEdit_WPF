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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using ImageEdit_WPF.HelperClasses.Algorithms;

namespace ImageEdit_WPF.Windows {
    /// <summary>
    /// Interaction logic for Information.xaml
    /// </summary>
    public partial class Information : Window {
        private ImageData m_data = null;

        /// <summary>
        /// Information <c>constructor</c>.
        /// Here we calculate all the neccesary information about some aspects of the image.
        /// </summary>
        /// <param name="data"></param>
        public Information(ImageData data) {
            m_data = data;

            InitializeComponent();
            CalculateData();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalculateData() {
            FileInfo file = new FileInfo(m_data.M_inputFilename);
            ImageFormat format = m_data.M_bitmap.RawFormat;
            int bpp = Image.GetPixelFormatSize(m_data.M_bitmap.PixelFormat);
            string disksize = string.Empty;
            string memorysize = string.Empty;

            switch (bpp) {
                case 8:
                    disksize = file.Length/1000 + " KB" + " (" + file.Length + " Bytes)";
                    memorysize = (m_data.M_bitmap.Width*m_data.M_bitmap.Height*1)/1000000 + " MB" + " (" + m_data.M_bitmap.Width*m_data.M_bitmap.Height*1 + " Bytes)";
                    break;
                case 16:
                    disksize = file.Length/1000 + " KB" + " (" + file.Length + " Bytes)";
                    memorysize = (m_data.M_bitmap.Width*m_data.M_bitmap.Height*2)/1000000 + " MB" + " (" + m_data.M_bitmap.Width*m_data.M_bitmap.Height*2 + " Bytes)";
                    break;
                case 24:
                    disksize = file.Length/1000 + " KB" + " (" + file.Length + " Bytes)";
                    memorysize = (m_data.M_bitmap.Width*m_data.M_bitmap.Height*3)/1000000 + " MB" + " (" + m_data.M_bitmap.Width*m_data.M_bitmap.Height*3 + " Bytes)";
                    break;
                case 32:
                    disksize = file.Length/1000 + " KB" + " (" + file.Length + " Bytes)";
                    memorysize = (m_data.M_bitmap.Width*m_data.M_bitmap.Height*4)/1000000 + " MB" + " (" + m_data.M_bitmap.Width*m_data.M_bitmap.Height*4 + " Bytes)";
                    break;
            }

            filenameTbx.Text = file.Name;
            directoryTbx.Text = file.DirectoryName;
            pathTbx.Text = file.FullName;
            compressionTbx.Text = GetEncoderInfo(format);
            resolutionTbx.Text = m_data.M_bitmap.Width + " x " + m_data.M_bitmap.Height + " Pixels";
            colorsTbx.Text = Math.Pow(2, bpp).ToString();
            disksizeTbx.Text = disksize;
            memorysizeTbx.Text = memorysize;
            filedatetimeTbx.Text = file.LastWriteTime.ToString();
        }

        /// <summary>
        /// Get the format of the image.
        /// </summary>
        /// <param name="format">Format of the image.</param>
        /// <returns>
        /// A string with the name of the format - compression.
        /// </returns>
        private static string GetEncoderInfo(ImageFormat format) {
            while (true) {
                if (format.Equals(ImageFormat.Jpeg)) {
                    return "JPEG";
                } else if (format.Equals(ImageFormat.Png)) {
                    return "PNG";
                } else if (format.Equals(ImageFormat.Bmp)) {
                    return "BMP";
                }
            }
        }
    }
}
