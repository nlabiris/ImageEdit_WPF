﻿/*
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

namespace ImageEdit_WPF
{
    /// <summary>
    /// Interaction logic for NoiseReductionMean.xaml
    /// </summary>
    public partial class NoiseReductionMean : Window
    {
        /// <summary>
        /// Output image.
        /// </summary>
        private readonly Bitmap _bmpOutput = null;

        /// <summary>
        /// Image used at the Undo/Redo system.
        /// </summary>
        private Bitmap _bmpUndoRedo = null;

        /// <summary>
        /// Size of the kernel.
        /// </summary>
        private int _sizeMask = 0;

        /// <summary>
        /// Noise Reduction (Mean filter) <c>constructor</c>.
        /// Here we initialiaze the images and also we set the default kernel.
        /// </summary>
        /// <param name="bmpO">Output image.</param>
        /// <param name="bmpUR">Image used at the Undo/Redo system.</param>
        public NoiseReductionMean(Bitmap bmpO, Bitmap bmpUR)
        {
            InitializeComponent();

            _bmpOutput = bmpO;
            _bmpUndoRedo = bmpUR;

            three.IsChecked = true;
        }

        /// <summary>
        /// If 3x3 radioBox is checked, set kernel's size.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void three_Checked(object sender, RoutedEventArgs e)
        {
            _sizeMask = 3;
        }

        /// <summary>
        /// If 5x5 radioBox is checked, set kernel's size.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void five_Checked(object sender, RoutedEventArgs e)
        {
            _sizeMask = 5;
        }

        /// <summary>
        /// If 7x7 radioBox is checked, set kernel's size.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seven_Checked(object sender, RoutedEventArgs e)
        {
            _sizeMask = 7;
        }

        /// <summary>
        /// Implementation of the Noise Reduction (Mean filter) algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            int sumR = 0;
            int sumG = 0;
            int sumB = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = _bmpOutput.LockBits(new Rectangle(0, 0, _bmpOutput.Width, _bmpOutput.Height), ImageLockMode.ReadWrite, _bmpOutput.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride) * _bmpOutput.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            if (_sizeMask == 3)
            {
                for (i = _sizeMask / 2; i < _bmpOutput.Width - _sizeMask / 2; i++)
                {
                    for (j = _sizeMask / 2; j < _bmpOutput.Height - _sizeMask / 2; j++)
                    {
                        int index;

                        sumR = 0;
                        sumG = 0;
                        sumB = 0;

                        for (k = 0; k < _sizeMask; k++)
                        {
                            for (l = 0; l < _sizeMask; l++)
                            {
                                index = ((j + l - 1) * bmpData.Stride) + ((i + k - 1) * 3);
                                sumR = sumR + rgbValues[index + 2];
                                sumG = sumG + rgbValues[index + 1];
                                sumB = sumB + rgbValues[index];
                            }
                        }

                        index = (j * bmpData.Stride) + (i * 3);

                        rgbValues[index + 2] = (byte)(sumR / (_sizeMask * _sizeMask));
                        rgbValues[index + 1] = (byte)(sumG / (_sizeMask * _sizeMask));
                        rgbValues[index] = (byte)(sumB / (_sizeMask * _sizeMask));
                    }
                }
            }
            else if (_sizeMask == 5)
            {
                for (i = _sizeMask / 2; i < _bmpOutput.Width - _sizeMask / 2; i++)
                {
                    for (j = _sizeMask / 2; j < _bmpOutput.Height - _sizeMask / 2; j++)
                    {
                        int index;

                        sumR = 0;
                        sumG = 0;
                        sumB = 0;

                        for (k = 0; k < _sizeMask; k++)
                        {
                            for (l = 0; l < _sizeMask; l++)
                            {
                                index = ((j + l - 1) * bmpData.Stride) + ((i + k - 1) * 3);
                                sumR = sumR + rgbValues[index + 2];
                                sumG = sumG + rgbValues[index + 1];
                                sumB = sumB + rgbValues[index];
                            }
                        }

                        index = (j * bmpData.Stride) + (i * 3);

                        rgbValues[index + 2] = (byte)(sumR / (_sizeMask * _sizeMask));
                        rgbValues[index + 1] = (byte)(sumG / (_sizeMask * _sizeMask));
                        rgbValues[index] = (byte)(sumB / (_sizeMask * _sizeMask));
                    }
                }
            }
            else if (_sizeMask == 7)
            {
                for (i = _sizeMask / 2; i < _bmpOutput.Width - _sizeMask / 2; i++)
                {
                    for (j = _sizeMask / 2; j < _bmpOutput.Height - _sizeMask / 2; j++)
                    {
                        int index;

                        sumR = 0;
                        sumG = 0;
                        sumB = 0;

                        for (k = 0; k < _sizeMask; k++)
                        {
                            for (l = 0; l < _sizeMask; l++)
                            {
                                index = ((j + l - 1) * bmpData.Stride) + ((i + k - 1) * 3);
                                sumR = sumR + rgbValues[index + 2];
                                sumG = sumG + rgbValues[index + 1];
                                sumB = sumB + rgbValues[index];
                            }
                        }

                        index = (j * bmpData.Stride) + (i * 3);

                        rgbValues[index + 2] = (byte)(sumR / (_sizeMask * _sizeMask));
                        rgbValues[index + 1] = (byte)(sumG / (_sizeMask * _sizeMask));
                        rgbValues[index] = (byte)(sumB / (_sizeMask * _sizeMask));
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
            if (result == MessageBoxResult.OK)
            {
                MainWindow.NoChange = false;
                MainWindow.Action = ActionType.ImageConvolution;
                _bmpUndoRedo = _bmpOutput.Clone() as Bitmap;
                MainWindow.UndoStack.Push(_bmpUndoRedo);
                MainWindow.RedoStack.Clear();
                foreach (Window mainWindow in Application.Current.Windows)
                {
                    if (mainWindow.GetType() == typeof(MainWindow))
                    {
                        ((MainWindow) mainWindow).undo.IsEnabled = true;
                        ((MainWindow) mainWindow).redo.IsEnabled = false;
                    }
                }
                this.Close();
            }
        }

        /// <summary>
        /// <c>Bitmap</c> to <c>BitmpaImage</c> conversion method in order to show the edited image at the main window.
        /// </summary>
        public void BitmapToBitmapImage()
        {
            MemoryStream str = new MemoryStream();
            _bmpOutput.Save(str, ImageFormat.Bmp);
            str.Seek(0, SeekOrigin.Begin);
            BmpBitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

            foreach (Window mainWindow in Application.Current.Windows)
            {
                if (mainWindow.GetType() == typeof(MainWindow))
                {
                    ((MainWindow) mainWindow).mainImage.Source = bdc.Frames[0];
                }
            }
        }
    }
}
