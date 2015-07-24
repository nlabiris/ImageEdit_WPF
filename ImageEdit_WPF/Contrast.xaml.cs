﻿using System;
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
using System.Globalization;

namespace ImageEdit_WPF
{
    /// <summary>
    /// Interaction logic for Contrast.xaml
    /// </summary>
    public partial class Contrast : Window
    {
        private String filename;
        private Bitmap bmpOutput = null;

        public Contrast(String fname, Bitmap bmpO)
        {
            InitializeComponent();

            filename = fname;
            bmpOutput = bmpO;

            textboxContrast.Focus();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            Double contrast = 0;
            Double R = 0;
            Double G = 0;
            Double B = 0;

            try
            {
                contrast = Double.Parse(textboxContrast.Text, new CultureInfo("el-GR"));
                //if (brightness > 255 || brightness < 0)
                //{
                //    String message = "Wrong range" + Environment.NewLine + Environment.NewLine + "Give a number between 0 and 255";
                //    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentNullException", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "FormatException", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
            catch (OverflowException ex)
            {
                MessageBox.Show(ex.Message, "OverflowException", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

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

                    R = rgbValues[index + 2] * contrast;
                    G = rgbValues[index + 1] * contrast;
                    B = rgbValues[index] * contrast;

                    if (R > 255.0)
                    {
                        R = 255.0;
                    }
                    else if (R < 0.0)
                    {
                        R = 0.0;
                    }

                    if (G > 255.0)
                    {
                        G = 255.0;
                    }
                    else if (G < 0.0)
                    {
                        G = 0.0;
                    }

                    if (B > 255.0)
                    {
                        B = 255.0;
                    }
                    else if (B < 0.0)
                    {
                        B = 0.0;
                    }

                    rgbValues[index + 2] = (Byte)R;
                    rgbValues[index + 1] = (Byte)G;
                    rgbValues[index] = (Byte)B;
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

            foreach (Window mainWindow in Application.Current.Windows)
            {
                if (mainWindow.GetType() == typeof(MainWindow))
                {
                    (mainWindow as MainWindow).mainImage.Source = bdc.Frames[0];
                }
            }

            String messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
            MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                MainWindow.noChange = false;
                this.Close();
            }
        }
    }
}