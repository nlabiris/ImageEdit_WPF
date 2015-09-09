/*
Basic image processing software
<https://github.com/nlabiris/ImageEdit>

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
    /// Interaction logic for SaltPepperNoiseColor.xaml
    /// </summary>
    public partial class SaltPepperNoiseColor : Window
    {
        private String filename;
        private Bitmap bmpOutput = null;
        private Bitmap bmpUndoRedo = null;

        public SaltPepperNoiseColor(String fname, Bitmap bmpO, Bitmap bmpUR)
        {
            InitializeComponent();

            filename = fname;
            bmpOutput = bmpO;
            bmpUndoRedo = bmpUR;

            textboxNoiseColor.Focus();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            Int32 i = 0;
            Int32 j = 0;
            Double probability;
            Int32 data;
            Int32 data1;
            Int32 data2;
            Random rand = new Random();

            try
            {
                probability = Double.Parse(textboxNoiseColor.Text);
                data = (Int32)(probability * 32768 / 2);
                data1 = data + 16384;
                data2 = 16384 - data;
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

            for (i = 0; i < bmpOutput.Width; i++)
            {
                for (j = 0; j < bmpOutput.Height; j++)
                {
                    int index = (j * bmpData.Stride) + (i * 3);

                    data = rand.Next(32768);
                    if (data >= 16384 && data < data1)
                    {
                        rgbValues[index + 2] = 0;
                    }
                    if (data >= data2 && data <= 16384)
                    {
                        rgbValues[index + 2] = 255;
                    }

                    data = rand.Next(32768);
                    if (data >= 16384 && data < data1)
                    {
                        rgbValues[index + 1] = 0;
                    }
                    if (data >= data2 && data <= 16384)
                    {
                        rgbValues[index + 1] = 255;
                    }

                    data = rand.Next(32768);
                    if (data >= 16384 && data < data1)
                    {
                        rgbValues[index] = 0;
                    }
                    if (data >= data2 && data <= 16384)
                    {
                        rgbValues[index] = 255;
                    }
                }
            }

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmpOutput.UnlockBits(bmpData);

            // Convert Bitmap to BitmapImage
            BitmapToBitmapImage();

            String messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
            MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                MainWindow.noChange = false;
                MainWindow.Action = ActionType.ImageConvolution;
                bmpUndoRedo = bmpOutput.Clone() as System.Drawing.Bitmap;
                MainWindow.undoStack.Push(bmpUndoRedo);
                MainWindow.redoStack.Clear();
                foreach (Window mainWindow in Application.Current.Windows)
                {
                    if (mainWindow.GetType() == typeof(MainWindow))
                    {
                        (mainWindow as MainWindow).undo.IsEnabled = true;
                    }
                }
                this.Close();
            }
        }

        public void BitmapToBitmapImage()
        {
            MemoryStream str = new MemoryStream();
            bmpOutput.Save(str, ImageFormat.Bmp);
            str.Seek(0, SeekOrigin.Begin);
            BmpBitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

            foreach (Window mainWindow in Application.Current.Windows)
            {
                if (mainWindow.GetType() == typeof(MainWindow))
                {
                    (mainWindow as MainWindow).mainImage.Source = bdc.Frames[0];
                }
            }
        }
    }
}
