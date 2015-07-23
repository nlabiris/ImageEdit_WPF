using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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
            compressionTbx.Text = format.ToString();
            resolutionTbx.Text = bmpOutput.Width + " x " + bmpOutput.Height + " Pixels";
            colorsTbx.Text = bmpOutput.PixelFormat.ToString();
            disksizeTbx.Text = disksize;
            memorysizeTbx.Text = memorysize;
            filedatetimeTbx.Text = file.LastWriteTime.ToString();
        }
    }
}