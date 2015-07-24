﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Drawing.Imaging;
using Microsoft.Win32;
using ImageMagick;


namespace ImageEdit_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String InputFilename = String.Empty;
        private String OutputFilename = String.Empty;
        private BitmapImage bmpInput = null;
        private System.Drawing.Bitmap bmpOutput = null;
        public static Boolean noChange = true;

        public MainWindow()
        {
            InitializeComponent();

            if (statusBar.Visibility == Visibility.Visible)
            {
                statusBarShowHide.IsChecked = true;
            }
            else
            {
                statusBarShowHide.IsChecked = false;
            }
        }

        #region Open
        private void open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "JPEG/JPG - JPG Files|*.jpeg;*.jpg|BMP - Windows Bitmap|*.bmp|PNG - Portable Network Graphics|*.png";

                if (openFile.ShowDialog() == true)
                {
                    InputFilename = openFile.FileName;
                    Uri uri = new Uri(InputFilename, UriKind.Absolute);
                    bmpInput = new BitmapImage(uri);
                    bmpOutput = new System.Drawing.Bitmap(InputFilename);
                    mainImage.Source = bmpInput;
                }

                Int32 bpp = System.Drawing.Image.GetPixelFormatSize(bmpOutput.PixelFormat);
                String resolution = bmpOutput.Width + " x " + bmpOutput.Height + " x " + bpp + " bpp";
                String size = String.Empty;
                FileInfo filesize = new FileInfo(InputFilename);
                if (bpp == 8)
                {
                    size = filesize.Length / 1000 + " KB" + " / " + (bmpOutput.Width * bmpOutput.Height * 1) / 1000000 + " MB";
                }
                else if (bpp == 16)
                {
                    size = filesize.Length / 1000 + " KB" + " / " + (bmpOutput.Width * bmpOutput.Height * 2) / 1000000 + " MB";
                }
                else if (bpp == 24)
                {
                    size = filesize.Length / 1000 + " KB" + " / " + (bmpOutput.Width * bmpOutput.Height * 3) / 1000000 + " MB";
                }
                else if (bpp == 32)
                {
                    size = filesize.Length / 1000 + " KB" + " / " + (bmpOutput.Width * bmpOutput.Height * 4) / 1000000 + " MB";
                }

                imageResolution.Text = resolution;
                imageSize.Text = size;
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (UriFormatException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Reopen
        private void reopen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (InputFilename != String.Empty)
                {
                    Uri uri = new Uri(InputFilename, UriKind.Absolute);
                    bmpInput = new BitmapImage(uri);
                    bmpOutput = new System.Drawing.Bitmap(InputFilename);
                    mainImage.Source = bmpInput;
                }
                else
                {
                    MessageBox.Show("Open image first!", "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (UriFormatException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Save
        private void save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (OutputFilename != String.Empty)
                {
                    
                    //MagickImage image = new MagickImage(bmpOutput);
                    //image.Quality = 100;
                    //image.Format = MagickFormat.Jpeg;
                    //image.CompressionMethod = CompressionMethod.JPEG;
                    //image.Write(OutputFilename);
                    ImageCodecInfo codec = GetEncoder(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder quality = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters encoderArray = new EncoderParameters(1);
                    EncoderParameter encoder = new EncoderParameter(quality, 100L);
                    encoderArray.Param[0] = encoder;

                    bmpOutput.Save(OutputFilename, codec, encoderArray);
                }
                else
                {
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.Filter = "JPEG/JPG - JPG Files|*.jpeg;*.jpg|BMP - Windows Bitmap|*.bmp|PNG - Portable Network Graphics|*.png";

                    if (saveFile.ShowDialog() == true)
                    {
                        OutputFilename = saveFile.FileName;

                        //MagickImage image = new MagickImage(bmpOutput);
                        //image.Quality = 100;
                        //image.Format = MagickFormat.Jpeg;
                        //image.CompressionMethod = CompressionMethod.JPEG;
                        //image.Write(OutputFilename);
                        ImageCodecInfo codec = GetEncoder(ImageFormat.Jpeg);
                        System.Drawing.Imaging.Encoder quality = System.Drawing.Imaging.Encoder.Quality;
                        EncoderParameters encoderArray = new EncoderParameters(1);
                        EncoderParameter encoder = new EncoderParameter(quality, 100L);
                        encoderArray.Param[0] = encoder;

                        bmpOutput.Save(OutputFilename, codec, encoderArray);
                    }
                }

                noChange = true;
            }
            catch (EncoderFallbackException ex)
            {
                MessageBox.Show(ex.Message, "EncoderFallbackException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentNullException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ExternalException ex)
            {
                MessageBox.Show(ex.Message, "ExternalException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Save as...
        private void saveAs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "JPEG/JPG - JPG Files|*.jpeg;*.jpg|BMP - Windows Bitmap|*.bmp|PNG - Portable Network Graphics|*.png";

                if (saveFile.ShowDialog() == true)
                {
                    OutputFilename = saveFile.FileName;

                    //MagickImage image = new MagickImage(bmpOutput);
                    //image.Quality = 100;
                    //image.Format = MagickFormat.Jpeg;
                    //image.CompressionMethod = CompressionMethod.JPEG;
                    //image.Write(OutputFilename);
                    ImageCodecInfo codec = GetEncoder(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder quality = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters encoderArray = new EncoderParameters(1);
                    EncoderParameter encoder = new EncoderParameter(quality, 100L);
                    encoderArray.Param[0] = encoder;

                    bmpOutput.Save(OutputFilename, codec, encoderArray);
                }

                noChange = true;
            }
            catch (EncoderFallbackException ex)
            {
                MessageBox.Show(ex.Message, "EncoderFallbackException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentNullException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ExternalException ex)
            {
                MessageBox.Show(ex.Message, "ExternalException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Status bar
        private void statusBarShowHide_Click(object sender, RoutedEventArgs e)
        {
            if (statusBar.Visibility == Visibility.Collapsed)
            {
                statusBar.Visibility = Visibility.Visible;
                statusBarShowHide.IsChecked = true;
            }
            else
            {
                statusBar.Visibility = Visibility.Collapsed;
                statusBarShowHide.IsChecked = false;
            }
        }
        #endregion

        #region Help
        private void help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This is the help window", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region About
        private void about_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ImageEdit v0.18.16.171 beta", "Version", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Information
        private void information_Click(object sender, RoutedEventArgs e)
        {
            if (InputFilename == String.Empty || bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Information informationWindow = new Information(InputFilename, bmpOutput);
                informationWindow.Owner = this;
                informationWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Shift Bits
        private void shiftBits_Click(object sender, RoutedEventArgs e)
        {
            if (InputFilename == String.Empty || bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                ShiftBits shiftBitsWindow = new ShiftBits(InputFilename, bmpOutput);
                shiftBitsWindow.Owner = this;
                shiftBitsWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Threshold
        private void threshold_Click(object sender, RoutedEventArgs e)
        {
            if (InputFilename == String.Empty || bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Threshold thresholdWindow = new Threshold(InputFilename, bmpOutput);
                thresholdWindow.Owner = this;
                thresholdWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Auto Threshold
        private void autoThreshold_Click(object sender, RoutedEventArgs e)
        {
            if (InputFilename == String.Empty || bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                AutoThreshold autoThresholdWindow = new AutoThreshold(InputFilename, bmpOutput);
                autoThresholdWindow.Owner = this;
                autoThresholdWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Negtative
        private void negative_Click(object sender, RoutedEventArgs e)
        {
            if (InputFilename == String.Empty || bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Lock the bitmap's bits.  
                BitmapData bmpData = bmpOutput.LockBits(new System.Drawing.Rectangle(0, 0, bmpOutput.Width, bmpOutput.Height), ImageLockMode.ReadWrite, bmpOutput.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. 
                Int32 bytes = Math.Abs(bmpData.Stride) * bmpOutput.Height;
                Byte[] rgbValues = new Byte[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(ptr, rgbValues, 0, bytes);

                Stopwatch watch = Stopwatch.StartNew();

                for (int i = 0; i < bmpOutput.Width; i++)
                {
                    for (int j = 0; j < bmpOutput.Height; j++)
                    {
                        int index = (j * bmpData.Stride) + (i * 3);
                        rgbValues[index + 2] = (Byte)(255 - rgbValues[index + 2]); // R
                        rgbValues[index + 1] = (Byte)(255 - rgbValues[index + 1]); // G
                        rgbValues[index] = (Byte)(255 - rgbValues[index]); // B
                    }
                }

                watch.Stop();
                TimeSpan elapsedTime = watch.Elapsed;

                // Copy the RGB values back to the bitmap
                Marshal.Copy(rgbValues, 0, ptr, bytes);

                // Unlock the bits.
                bmpOutput.UnlockBits(bmpData);

                // Convert Bitmap to BitmapImage
                MemoryStream str = new MemoryStream();
                bmpOutput.Save(str, ImageFormat.Bmp);
                str.Seek(0, SeekOrigin.Begin);
                BitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                mainImage.Source = bdc.Frames[0];

                String messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
                MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    MainWindow.noChange = false;
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Square root
        private void squareRoot_Click(object sender, RoutedEventArgs e)
        {
            if (InputFilename == String.Empty || bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Lock the bitmap's bits.  
                BitmapData bmpData = bmpOutput.LockBits(new System.Drawing.Rectangle(0, 0, bmpOutput.Width, bmpOutput.Height), ImageLockMode.ReadWrite, bmpOutput.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. 
                Int32 bytes = Math.Abs(bmpData.Stride) * bmpOutput.Height;
                Byte[] rgbValues = new Byte[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(ptr, rgbValues, 0, bytes);

                Stopwatch watch = Stopwatch.StartNew();

                for (int i = 0; i < bmpOutput.Width; i++)
                {
                    for (int j = 0; j < bmpOutput.Height; j++)
                    {
                        int index = (j * bmpData.Stride) + (i * 3);
                        rgbValues[index + 2] = (Byte)Math.Sqrt(rgbValues[index + 2] * 255); // R
                        rgbValues[index + 1] = (Byte)Math.Sqrt(rgbValues[index + 1] * 255); // G
                        rgbValues[index] = (Byte)Math.Sqrt(rgbValues[index] * 255); // B
                    }
                }

                watch.Stop();
                TimeSpan elapsedTime = watch.Elapsed;

                // Copy the RGB values back to the bitmap
                Marshal.Copy(rgbValues, 0, ptr, bytes);

                // Unlock the bits.
                bmpOutput.UnlockBits(bmpData);

                // Convert Bitmap to BitmapImage
                MemoryStream str = new MemoryStream();
                bmpOutput.Save(str, ImageFormat.Bmp);
                str.Seek(0, SeekOrigin.Begin);
                BitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                mainImage.Source = bdc.Frames[0];

                String messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
                MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    MainWindow.noChange = false;
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Contrast Enhancement
        private void contrastEnhancement_Click(object sender, RoutedEventArgs e)
        {
            if (InputFilename == String.Empty || bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                ContrastEnhancement contrastEnhancement = new ContrastEnhancement(InputFilename, bmpOutput);
                contrastEnhancement.Owner = this;
                contrastEnhancement.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Brightness
        private void brightness_Click(object sender, RoutedEventArgs e)
        {
            if (InputFilename == String.Empty || bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Brightness brightness = new Brightness(InputFilename, bmpOutput);
                brightness.Owner = this;
                brightness.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Contrast
        private void contrast_Click(object sender, RoutedEventArgs e)
        {
            if (InputFilename == String.Empty || bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Contrast contrastWindow = new Contrast(InputFilename, bmpOutput);
                contrastWindow.Owner = this;
                contrastWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Histogram
        private void histogram_Click(object sender, RoutedEventArgs e)
        {
            if (InputFilename == String.Empty || bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Histogram histogramWindow = new Histogram(InputFilename, bmpOutput);
                histogramWindow.Owner = this;
                histogramWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Histogram Equalization [RGB]
        private void histogramEqualizationRGB_Click(object sender, RoutedEventArgs e)
        {
            if (InputFilename == String.Empty || bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Int32 B;
                Int32 G;
                Int32 R;
                Double[] PossibilityR = new Double[256];
                Double[] PossibilityG = new Double[256];
                Double[] PossibilityB = new Double[256];
                Int32[] HistogramR = new Int32[256];
                Int32[] HistogramG = new Int32[256];
                Int32[] HistogramB = new Int32[256];
                Double[] HistogramEqR = new Double[256];
                Double[] HistogramEqG = new Double[256];
                Double[] HistogramEqB = new Double[256];

                // Lock the bitmap's bits.  
                BitmapData bmpData = bmpOutput.LockBits(new System.Drawing.Rectangle(0, 0, bmpOutput.Width, bmpOutput.Height), ImageLockMode.ReadWrite, bmpOutput.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. 
                Int32 bytes = Math.Abs(bmpData.Stride) * bmpOutput.Height;
                Byte[] rgbValues = new Byte[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(ptr, rgbValues, 0, bytes);

                Stopwatch watch = Stopwatch.StartNew();

                for (int i = 0; i < 256; i++)
                {
                    HistogramR[i] = 0;
                    HistogramG[i] = 0;
                    HistogramB[i] = 0;
                }

                for (int i = 0; i < bmpOutput.Width; i++)
                {
                    for (int j = 0; j < bmpOutput.Height; j++)
                    {
                        int index = (j * bmpData.Stride) + (i * 3);


                        B = rgbValues[index + 2];
                        HistogramB[B]++;
                        G = rgbValues[index + 1];
                        HistogramG[G]++;
                        R = rgbValues[index];
                        HistogramR[R]++;
                    }
                }

                for (int i = 0; i < 256; i++)
                {
                    PossibilityB[i] = HistogramB[i] / (Double)(bmpOutput.Width * bmpOutput.Height);
                    PossibilityG[i] = HistogramG[i] / (Double)(bmpOutput.Width * bmpOutput.Height);
                    PossibilityR[i] = HistogramR[i] / (Double)(bmpOutput.Width * bmpOutput.Height);
                }

                HistogramEqB[0] = PossibilityB[0];
                HistogramEqG[0] = PossibilityG[0];
                HistogramEqR[0] = PossibilityR[0];
                for (int i = 1; i < 256; i++)
                {
                    HistogramEqB[i] = HistogramEqB[i - 1] + PossibilityB[i];
                    HistogramEqG[i] = HistogramEqG[i - 1] + PossibilityG[i];
                    HistogramEqR[i] = HistogramEqR[i - 1] + PossibilityR[i];
                }

                for (int i = 0; i < bmpOutput.Width; i++)
                {
                    for (int j = 0; j < bmpOutput.Height; j++)
                    {
                        int index = (j * bmpData.Stride) + (i * 3);

                        B = rgbValues[index + 2];
                        B = (Int32)Math.Round(HistogramEqB[B] * 255);
                        G = rgbValues[index + 1];
                        G = (Int32)Math.Round(HistogramEqG[G] * 255);
                        R = rgbValues[index];
                        R = (Int32)Math.Round(HistogramEqR[R] * 255);

                        if (B > 255)
                        {
                            B = 255;
                        }
                        else if (B < 0)
                        {
                            B = 0;
                        }

                        if (G > 255)
                        {
                            G = 255;
                        }
                        else if (G < 0)
                        {
                            G = 0;
                        }

                        if (R > 255)
                        {
                            R = 255;
                        }
                        else if (R < 0)
                        {
                            R = 0;
                        }

                        rgbValues[index + 2] = (Byte)B;
                        rgbValues[index + 1] = (Byte)G;
                        rgbValues[index] = (Byte)R;
                    }
                }

                watch.Stop();
                TimeSpan elapsedTime = watch.Elapsed;

                // Copy the RGB values back to the bitmap
                Marshal.Copy(rgbValues, 0, ptr, bytes);

                // Unlock the bits.
                bmpOutput.UnlockBits(bmpData);

                // Convert Bitmap to BitmapImage
                MemoryStream str = new MemoryStream();
                bmpOutput.Save(str, ImageFormat.Bmp);
                str.Seek(0, SeekOrigin.Begin);
                BitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                mainImage.Source = bdc.Frames[0];

                String messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
                MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    MainWindow.noChange = false;
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Histogram Equalization [HSV]
        private void histogramEqualizationHSV_Click(object sender, RoutedEventArgs e)
        {
            if (InputFilename == String.Empty || bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Int32 i;
                Int32 j;
                Int32 k = 0;
                Double Max = 0.0;
                Double Min = 0.0;
                Double Chroma = 0.0;
                Double c = 0.0;
                Double x = 0.0;
                Double m = 0.0;
                Byte[,] ImageR = new Byte[bmpOutput.Width, bmpOutput.Height];
                Byte[,] ImageG = new Byte[bmpOutput.Width, bmpOutput.Height];
                Byte[,] ImageB = new Byte[bmpOutput.Width, bmpOutput.Height];
                Int32[] HistogramV = new Int32[256];
                Double[] SumHistogramEqualizationV = new Double[256];
                Double[] SumHistogramV = new Double[256];

                // Lock the bitmap's bits.  
                BitmapData bmpData = bmpOutput.LockBits(new System.Drawing.Rectangle(0, 0, bmpOutput.Width, bmpOutput.Height), ImageLockMode.ReadWrite, bmpOutput.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. 
                Int32 bytes = Math.Abs(bmpData.Stride) * bmpOutput.Height;
                Byte[] rgbValues = new Byte[bytes];
                Double[] Red = new Double[bytes];
                Double[] Green = new Double[bytes];
                Double[] Blue = new Double[bytes];
                Double[] Hue = new Double[bytes];
                Double[] Value = new Double[bytes];
                Double[] Saturation = new Double[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(ptr, rgbValues, 0, bytes);

                Stopwatch watch = Stopwatch.StartNew();

                for (i = 0; i < 256; i++)
                {
                    HistogramV[i] = 0;
                    SumHistogramEqualizationV[i] = 0.0;
                }

                for (i = 0; i < bmpOutput.Width; i++)
                {
                    for (j = 0; j < bmpOutput.Height; j++)
                    {
                        int index = (bmpData.Stride * j) + (i * 3);

                        Blue[index] = (Double)rgbValues[index];
                        Blue[index] = Blue[index] / 255.0;
                        Green[index + 1] = (Double)rgbValues[index + 1];
                        Green[index + 1] = Green[index + 1] / 255.0;
                        Red[index + 2] = (Double)rgbValues[index + 2];
                        Red[index + 2] = Red[index + 2] / 255.0;


                        Min = Math.Min(Red[index + 2], Math.Min(Green[index + 1], Blue[index]));
                        Max = Math.Max(Red[index + 2], Math.Max(Green[index + 1], Blue[index]));
                        Chroma = Max - Min;
                        Hue[index] = 0.0;
                        Saturation[index + 1] = 0.0;
                        if (Chroma != 0.0)
                        {
                            if (Math.Abs(Red[index + 2] - Max) < 0.00001)
                            {
                                Hue[index] = ((Green[index + 1] - Blue[index]) / Chroma);
                                Hue[index] = Hue[index] % 6.0;
                            }
                            else if (Math.Abs(Green[index + 1] - Max) < 0.00001)
                            {
                                Hue[index] = ((Blue[index] - Red[index + 2]) / Chroma) + 2;
                            }
                            else
                            {
                                Hue[index] = ((Red[index + 2] - Green[index + 1]) / Chroma) + 4;
                            }

                            Hue[index] = Hue[index] * 60.0;
                            if (Hue[index] < 0.0)
                            {
                                Hue[index] = Hue[index] + 360.0;
                            }
                            Saturation[index + 1] = Chroma / Max;
                        }
                        Value[index + 2] = Max;

                        Value[index + 2] = Value[index + 2] * 255.0;

                        if (Value[index + 2] > 255.0)
                        {
                            Value[index + 2] = 255.0;
                        }
                        if (Value[index + 2] < 0.0)
                        {
                            Value[index + 2] = 0.0;
                        }

                        k = (Int32)Value[index + 2];
                        HistogramV[k]++;
                    }
                }

                for (i = 0; i < 256; i++)
                {
                    SumHistogramEqualizationV[i] = (Double)HistogramV[i] / (Double)(bmpOutput.Width * bmpOutput.Height);
                }

                SumHistogramV[0] = SumHistogramEqualizationV[0];
                for (i = 1; i < 256; i++)
                {
                    SumHistogramV[i] = SumHistogramV[i - 1] + SumHistogramEqualizationV[i];
                }

                for (i = 0; i < bmpOutput.Width; i++)
                {
                    for (j = 0; j < bmpOutput.Height; j++)
                    {
                        int index = (bmpData.Stride * j) + (i * 3);

                        k = (Int32)Value[index + 2];
                        Value[index + 2] = (Byte)Math.Round(SumHistogramV[k] * 255.0);
                        Value[index + 2] = Value[index + 2] / 255;

                        c = Value[index + 2] * Saturation[index + 1];
                        Hue[index] = Hue[index] / 60.0;
                        Hue[index] = Hue[index] % 2;
                        x = c * (1.0 - Math.Abs(Hue[index] - 1.0));
                        m = Value[index + 2] - c;

                        if (Hue[index] >= 0.0 && Hue[index] < 60.0)
                        {
                            Red[index + 2] = c;
                            Green[index + 1] = x;
                            Blue[index] = 0;
                        }
                        else if (Hue[index] >= 60.0 && Hue[index] < 120.0)
                        {
                            Red[index + 2] = x;
                            Green[index + 1] = c;
                            Blue[index] = 0;
                        }
                        else if (Hue[index] >= 120.0 && Hue[index] < 180.0)
                        {
                            Red[index + 2] = 0;
                            Green[index + 1] = c;
                            Blue[index] = x;
                        }
                        else if (Hue[index] >= 180.0 && Hue[index] < 240.0)
                        {
                            Red[index + 2] = 0;
                            Green[index + 1] = x;
                            Blue[index] = c;
                        }
                        else if (Hue[index] >= 240.0 && Hue[index] < 300.0)
                        {
                            Red[index + 2] = x;
                            Green[index + 1] = 0;
                            Blue[index] = c;
                        }
                        else if (Hue[index] >= 300.0 && Hue[index] < 360.0)
                        {
                            Red[index + 2] = c;
                            Green[index + 1] = 0;
                            Blue[index] = x;
                        }

                        Red[index + 2] = Red[index + 2] + m;
                        Green[index + 1] = Green[index + 1] + m;
                        Blue[index] = Blue[index] + m;

                        Red[index + 2] = Red[index + 2] * 255.0;
                        Green[index + 1] = Green[index + 1] * 255.0;
                        Blue[index] = Blue[index] * 255.0;

                        if (Red[index + 2] > 255.0)
                        {
                            Red[index + 2] = 255.0;
                        }

                        if (Red[index + 2] < 0.0)
                        {
                            Red[index + 2] = 0.0;
                        }

                        if (Green[index + 1] > 255.0)
                        {
                            Green[index + 1] = 255.0;
                        }

                        if (Green[index + 1] < 0.0)
                        {
                            Green[index + 1] = 0.0;
                        }

                        if (Blue[index] > 255.0)
                        {
                            Blue[index] = 255.0;
                        }

                        if (Blue[index] < 0.0)
                        {
                            Blue[index] = 0.0;
                        }

                        rgbValues[index + 2] = (Byte)Red[index + 2];
                        rgbValues[index + 1] = (Byte)Green[index + 1];
                        rgbValues[index] = (Byte)Blue[index];
                    }
                }

                watch.Stop();
                TimeSpan elapsedTime = watch.Elapsed;

                // Copy the RGB values back to the bitmap
                Marshal.Copy(rgbValues, 0, ptr, bytes);

                // Unlock the bits.
                bmpOutput.UnlockBits(bmpData);

                // Convert Bitmap to BitmapImage
                MemoryStream str = new MemoryStream();
                bmpOutput.Save(str, ImageFormat.Bmp);
                str.Seek(0, SeekOrigin.Begin);
                BitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                mainImage.Source = bdc.Frames[0];

                String messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
                MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    MainWindow.noChange = false;
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Histogram Equalization [YUV]
        private void histogramEqualizationYUV_Click(object sender, RoutedEventArgs e)
        {
            if (InputFilename == String.Empty || bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Int32 i;
                Int32 j;
                Int32 k = 0;
                Byte[,] ImageR = new Byte[bmpOutput.Width, bmpOutput.Height];
                Byte[,] ImageG = new Byte[bmpOutput.Width, bmpOutput.Height];
                Byte[,] ImageB = new Byte[bmpOutput.Width, bmpOutput.Height];
                Int32[] HistogramY = new Int32[256];
                Double[] SumHistogramEqualizationY = new Double[256];
                Double[] SumHistogramY = new Double[256];

                // Lock the bitmap's bits.  
                BitmapData bmpData = bmpOutput.LockBits(new System.Drawing.Rectangle(0, 0, bmpOutput.Width, bmpOutput.Height), ImageLockMode.ReadWrite, bmpOutput.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. 
                Int32 bytes = Math.Abs(bmpData.Stride) * bmpOutput.Height;
                Byte[] rgbValues = new Byte[bytes];
                Double[] Red = new Double[bytes];
                Double[] Green = new Double[bytes];
                Double[] Blue = new Double[bytes];
                Double[] LuminanceY = new Double[bytes];
                Double[] ChrominanceU = new Double[bytes];
                Double[] ChrominanceV = new Double[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(ptr, rgbValues, 0, bytes);

                Stopwatch watch = Stopwatch.StartNew();

                for (i = 0; i < 256; i++)
                {
                    HistogramY[i] = 0;
                }

                for (i = 0; i < bmpOutput.Width; i++)
                {
                    for (j = 0; j < bmpOutput.Height; j++)
                    {
                        int index = (bmpData.Stride * j) + (i * 3);

                        Blue[index] = (Double)rgbValues[index];
                        Blue[index] = Blue[index] / 255.0;
                        Green[index + 1] = (Double)rgbValues[index + 1];
                        Green[index + 1] = Green[index + 1] / 255.0;
                        Red[index + 2] = (Double)rgbValues[index + 2];
                        Red[index + 2] = Red[index + 2] / 255.0;

                        LuminanceY[index] = (0.299 * Red[index + 2]) + (0.587 * Green[index + 1]) + (0.114 * Blue[index]);
                        ChrominanceU[index + 1] = (-0.14713 * Red[index + 2]) - (0.28886 * Green[index + 1]) + (0.436 * Blue[index]);
                        ChrominanceV[index + 2] = (0.615 * Red[index + 2]) - (0.51499 * Green[index + 1]) - (0.10001 * Blue[index]);

                        LuminanceY[index] = LuminanceY[index] * 255.0;
                        if (LuminanceY[index] > 255.0)
                        {
                            LuminanceY[index] = 255.0;
                        }

                        if (LuminanceY[index] < 0.0)
                        {
                            LuminanceY[index] = 0.0;
                        }

                        k = (Int32)LuminanceY[index];
                        HistogramY[k]++;
                    }
                }

                for (i = 0; i < 256; i++)
                {
                    SumHistogramEqualizationY[i] = 0.0;
                }

                for (i = 0; i < 256; i++)
                {
                    SumHistogramEqualizationY[i] = (Double)HistogramY[i] / (Double)(bmpOutput.Width * bmpOutput.Height);
                }

                SumHistogramY[0] = SumHistogramEqualizationY[0];
                for (i = 1; i < 256; i++)
                {
                    SumHistogramY[i] = SumHistogramY[i - 1] + SumHistogramEqualizationY[i];
                }

                for (i = 0; i < bmpOutput.Width; i++)
                {
                    for (j = 0; j < bmpOutput.Height; j++)
                    {
                        int index = (bmpData.Stride * j) + (i * 3);

                        k = (Int32)LuminanceY[index];
                        LuminanceY[index] = (Byte)Math.Round(SumHistogramY[k] * 255.0);
                        LuminanceY[index] = LuminanceY[index] / 255;

                        Red[index + 2] = LuminanceY[index] + (0.0 * ChrominanceU[index + 1]) + (1.13983 * ChrominanceV[index + 2]);
                        Green[index + 1] = LuminanceY[index] + (-0.39465 * ChrominanceU[index + 1]) + (-0.58060 * ChrominanceV[index + 2]);
                        Blue[index] = LuminanceY[index] + (2.03211 * ChrominanceU[index + 1]) + (0.0 * ChrominanceV[index + 2]);

                        Red[index + 2] = Red[index + 2] * 255.0;
                        Green[index + 1] = Green[index + 1] * 255.0;
                        Blue[index] = Blue[index] * 255.0;

                        if (Red[index + 2] > 255.0)
                        {
                            Red[index + 2] = 255.0;
                        }

                        if (Red[index + 2] < 0.0)
                        {
                            Red[index + 2] = 0.0;
                        }

                        if (Green[index + 1] > 255.0)
                        {
                            Green[index + 1] = 255.0;
                        }

                        if (Green[index + 1] < 0.0)
                        {
                            Green[index + 1] = 0.0;
                        }

                        if (Blue[index] > 255.0)
                        {
                            Blue[index] = 255.0;
                        }

                        if (Blue[index] < 0.0)
                        {
                            Blue[index] = 0.0;
                        }

                        rgbValues[index + 2] = (Byte)Red[index + 2];
                        rgbValues[index + 1] = (Byte)Green[index + 1];
                        rgbValues[index] = (Byte)Blue[index];
                    }
                }

                watch.Stop();
                TimeSpan elapsedTime = watch.Elapsed;

                // Copy the RGB values back to the bitmap
                Marshal.Copy(rgbValues, 0, ptr, bytes);

                // Unlock the bits.
                bmpOutput.UnlockBits(bmpData);

                // Convert Bitmap to BitmapImage
                MemoryStream str = new MemoryStream();
                bmpOutput.Save(str, ImageFormat.Bmp);
                str.Seek(0, SeekOrigin.Begin);
                BitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                mainImage.Source = bdc.Frames[0];

                String messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
                MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    MainWindow.noChange = false;
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Image Summarization
        private void imageSummarization_Click(object sender, RoutedEventArgs e)
        {
            if (InputFilename == String.Empty || bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Int32 B;
                Int32 G;
                Int32 R;

                // Lock the bitmap's bits.  
                BitmapData bmpData = bmpOutput.LockBits(new System.Drawing.Rectangle(0, 0, bmpOutput.Width, bmpOutput.Height), ImageLockMode.ReadWrite, bmpOutput.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. 
                Int32 bytes = Math.Abs(bmpData.Stride) * bmpOutput.Height;
                Byte[] rgbValues = new Byte[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(ptr, rgbValues, 0, bytes);

                Stopwatch watch = Stopwatch.StartNew();

                for (int i = 0; i < bmpOutput.Width; i++)
                {
                    for (int j = 0; j < bmpOutput.Height; j++)
                    {
                        int index = (j * bmpData.Stride) + (i * 3);
                        R = rgbValues[index + 2] + rgbValues[index + 2]; // R
                        G = rgbValues[index + 1] + rgbValues[index + 1]; // G
                        B = rgbValues[index] + rgbValues[index]; // B

                        if (R > 255)
                        {
                            R = 255;
                        }

                        if (G > 255)
                        {
                            G = 255;
                        }

                        if (B > 255)
                        {
                            B = 255;
                        }

                        rgbValues[index + 2] = (Byte)R; // R
                        rgbValues[index + 1] = (Byte)G; // G
                        rgbValues[index] = (Byte)B; // B
                    }
                }

                watch.Stop();
                TimeSpan elapsedTime = watch.Elapsed;

                // Copy the RGB values back to the bitmap
                Marshal.Copy(rgbValues, 0, ptr, bytes);

                // Unlock the bits.
                bmpOutput.UnlockBits(bmpData);

                // Convert Bitmap to BitmapImage
                MemoryStream str = new MemoryStream();
                bmpOutput.Save(str, ImageFormat.Bmp);
                str.Seek(0, SeekOrigin.Begin);
                BitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                mainImage.Source = bdc.Frames[0];

                String messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
                MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    MainWindow.noChange = false;
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Image Subtraction
        private void imageSubtraction_Click(object sender, RoutedEventArgs e)
        {
            if (InputFilename == String.Empty || bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Int32 B;
                Int32 G;
                Int32 R;

                // Lock the bitmap's bits.  
                BitmapData bmpData = bmpOutput.LockBits(new System.Drawing.Rectangle(0, 0, bmpOutput.Width, bmpOutput.Height), ImageLockMode.ReadWrite, bmpOutput.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. 
                Int32 bytes = Math.Abs(bmpData.Stride) * bmpOutput.Height;
                Byte[] rgbValues = new Byte[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(ptr, rgbValues, 0, bytes);

                Stopwatch watch = Stopwatch.StartNew();

                for (int i = 0; i < bmpOutput.Width; i++)
                {
                    for (int j = 0; j < bmpOutput.Height; j++)
                    {
                        int index = (j * bmpData.Stride) + (i * 3);

                        R = rgbValues[index + 2] - rgbValues[index + 2]; // R
                        G = rgbValues[index + 1] - rgbValues[index + 1]; // G
                        B = rgbValues[index] - rgbValues[index]; // B

                        rgbValues[index + 2] = (Byte)R; // R
                        rgbValues[index + 1] = (Byte)G; // G
                        rgbValues[index] = (Byte)B; // B
                    }
                }

                watch.Stop();
                TimeSpan elapsedTime = watch.Elapsed;

                // Copy the RGB values back to the bitmap
                Marshal.Copy(rgbValues, 0, ptr, bytes);

                // Unlock the bits.
                bmpOutput.UnlockBits(bmpData);

                // Convert Bitmap to BitmapImage
                MemoryStream str = new MemoryStream();
                bmpOutput.Save(str, ImageFormat.Bmp);
                str.Seek(0, SeekOrigin.Begin);
                BitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                mainImage.Source = bdc.Frames[0];

                String messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
                MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    MainWindow.noChange = false;
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Image Convolution [2D]
        private void imageConvolution2d_Click(object sender, RoutedEventArgs e)
        {
            if (InputFilename == String.Empty || bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                ImageConvolution2D imageConvolution2dWindow = new ImageConvolution2D(InputFilename, bmpOutput);
                imageConvolution2dWindow.Owner = this;
                imageConvolution2dWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (noChange == false)
            {
                MessageBoxResult result = MessageBox.Show("Quit without saving?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}