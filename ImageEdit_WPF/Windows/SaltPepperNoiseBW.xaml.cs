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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageEdit_WPF.Windows {
    /// <summary>
    /// Interaction logic for SaltPepperNoiseBW.xaml
    /// </summary>
    public partial class SaltPepperNoiseBW : Window {
        /// <summary>
        /// Output image.
        /// </summary>
        private readonly Bitmap _bmpOutput = null;

        /// <summary>
        /// Image used at the Undo/Redo system.
        /// </summary>
        private Bitmap _bmpUndoRedo = null;

        /// <summary>
        /// Check if the image has been modified
        /// </summary>
        private bool _nochange;

        /// <summary>
        /// Salt-and-Pepper Noise generator (Black/White) <c>constructor</c>.
        /// Here we initialiaze the images and also we set the focus at the textBox being used.
        /// </summary>
        /// <param name="bmpO">Output image.</param>
        /// <param name="bmpUR">Image used at the Undo/Redo system.</param>
        public SaltPepperNoiseBW(Bitmap bmpO, Bitmap bmpUR, ref bool nochange) {
            InitializeComponent();

            _bmpOutput = bmpO;
            _bmpUndoRedo = bmpUR;
            _nochange = nochange;

            textboxNoiseBW.Focus();
        }

        /// <summary>
        /// Implementation of the Salt-and-Pepper Noise generator (Black/White) algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ok_Click(object sender, RoutedEventArgs e) {
            int i = 0;
            int j = 0;
            double probability;
            int data;
            int data1;
            int data2;
            Random rand = new Random();

            try {
                probability = double.Parse(textboxNoiseBW.Text);
                data = (int)(probability*32768/2);
                data1 = data + 16384;
                data2 = 16384 - data;
            } catch (ArgumentNullException ex) {
                MessageBox.Show(ex.Message, "ArgumentNullException", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            } catch (FormatException ex) {
                MessageBox.Show(ex.Message, "FormatException", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            } catch (OverflowException ex) {
                MessageBox.Show(ex.Message, "OverflowException", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            // Lock the bitmap's bits.  
            BitmapData bmpData = _bmpOutput.LockBits(new Rectangle(0, 0, _bmpOutput.Width, _bmpOutput.Height), ImageLockMode.ReadWrite, _bmpOutput.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*_bmpOutput.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            for (i = 0; i < _bmpOutput.Width; i++) {
                for (j = 0; j < _bmpOutput.Height; j++) {
                    int index = (j*bmpData.Stride) + (i*3);

                    data = rand.Next(32768);
                    if (data >= 16384 && data < data1) {
                        rgbValues[index + 2] = 0;
                        rgbValues[index + 1] = 0;
                        rgbValues[index] = 0;
                    }
                    if (data >= data2 && data <= 16384) {
                        rgbValues[index + 2] = 255;
                        rgbValues[index + 1] = 255;
                        rgbValues[index] = 255;
                    }
                }
            }

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            _bmpOutput.UnlockBits(bmpData);

            // Convert Bitmap to BitmapImage
            BitmapToBitmapImage();

            string messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
            MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK) {
                _nochange = false;
                MainWindow.Action = ActionType.ImageConvolution;
                _bmpUndoRedo = _bmpOutput.Clone() as Bitmap;
                MainWindow.UndoStack.Push(_bmpUndoRedo);
                MainWindow.RedoStack.Clear();
                foreach (Window mainWindow in Application.Current.Windows) {
                    if (mainWindow.GetType() == typeof (MainWindow)) {
                        ((MainWindow)mainWindow).undo.IsEnabled = true;
                        ((MainWindow)mainWindow).redo.IsEnabled = false;
                    }
                }
                this.Close();
            }
        }

        /// <summary>
        /// <c>Bitmap</c> to <c>BitmpaImage</c> conversion method in order to show the edited image at the main window.
        /// </summary>
        public void BitmapToBitmapImage() {
            MemoryStream str = new MemoryStream();
            _bmpOutput.Save(str, ImageFormat.Bmp);
            str.Seek(0, SeekOrigin.Begin);
            BmpBitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

            foreach (Window mainWindow in Application.Current.Windows) {
                if (mainWindow.GetType() == typeof (MainWindow)) {
                    ((MainWindow)mainWindow).mainImage.Source = bdc.Frames[0];
                }
            }
        }
    }
}