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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;

namespace ImageEdit_WPF
{
    /// <summary>
    /// Interaction logic for Information.xaml
    /// </summary>
    public partial class Information : Window
    {
        private String filename;
        private Bitmap bmpOutput = null;

        public Information(String fname, Bitmap bmpO)
        {
            InitializeComponent();

            filename = fname;
            bmpOutput = bmpO;

            FileInfo file = new FileInfo(filename);
            ImageFormat format = bmpOutput.RawFormat;
            Int32 bpp = System.Drawing.Image.GetPixelFormatSize(bmpOutput.PixelFormat);
            String disksize = String.Empty;
            String memorysize = String.Empty;

            if (bpp == 8)
            {
                disksize = file.Length / 1000 + " KB" + " (" + file.Length + " Bytes)";
                memorysize = (bmpOutput.Width * bmpOutput.Height * 1) / 1000000 + " MB" + " (" + bmpOutput.Width * bmpOutput.Height * 1 + " Bytes)";
            }
            else if (bpp == 16)
            {
                disksize = file.Length / 1000 + " KB" + " (" + file.Length + " Bytes)";
                memorysize = (bmpOutput.Width * bmpOutput.Height * 2) / 1000000 + " MB" + " (" + bmpOutput.Width * bmpOutput.Height * 2 + " Bytes)";
            }
            else if (bpp == 24)
            {
                disksize = file.Length / 1000 + " KB" + " (" + file.Length + " Bytes)";
                memorysize = (bmpOutput.Width * bmpOutput.Height * 3) / 1000000 + " MB" + " (" + bmpOutput.Width * bmpOutput.Height * 3 + " Bytes)";
            }
            else if (bpp == 32)
            {
                disksize = file.Length / 1000 + " KB" + " (" + file.Length + " Bytes)";
                memorysize = (bmpOutput.Width * bmpOutput.Height * 4) / 1000000 + " MB" + " (" + bmpOutput.Width * bmpOutput.Height * 4 + " Bytes)";
            }

            filenameTbx.Text = file.Name;
            directoryTbx.Text = file.DirectoryName;
            pathTbx.Text = file.FullName;
            compressionTbx.Text = GetEncoderInfo(format);
            resolutionTbx.Text = bmpOutput.Width + " x " + bmpOutput.Height + " Pixels";
            colorsTbx.Text = Math.Pow(2, bpp).ToString();
            disksizeTbx.Text = disksize;
            memorysizeTbx.Text = memorysize;
            filedatetimeTbx.Text = file.LastWriteTime.ToString();
        }

        private String GetEncoderInfo(ImageFormat format)
        {
            while(true)
            {
                if(format.Equals(ImageFormat.Jpeg))
                {
                    return "JPEG";
                }
                else if(format.Equals(ImageFormat.Png))
                {
                    return "PNG";
                }
                else if(format.Equals(ImageFormat.Bmp))
                {
                    return "BMP";
                }
            }
        }
    }
}