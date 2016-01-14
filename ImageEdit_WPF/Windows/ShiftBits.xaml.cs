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
using ImageEdit_WPF.HelperClasses;

namespace ImageEdit_WPF.Windows {
    /// <summary>
    /// Interaction logic for ShiftBits.xaml
    /// </summary>
    public partial class ShiftBits : Window {
        private ImageEditData m_data = null;

        /// <summary>
        /// Shift Bits <c>constructor</c>.
        /// Here we initialiaze the images and also we set the focus at the textBox being used.
        /// </summary>
        public ShiftBits(ImageEditData data) {
            m_data = data;

            InitializeComponent();
            textboxBits.Focus();
        }

        /// <summary>
        /// Implementation of the Shift bits algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ok_Click(object sender, RoutedEventArgs e) {
            int bits = 0;

            try {
                bits = int.Parse(textboxBits.Text);
                if (bits > 7 || bits < 0) {
                    string message = "Wrong range" + Environment.NewLine + Environment.NewLine + "Give a number between 0 and 7";
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            } catch (ArgumentNullException ex) {
                MessageBox.Show(ex.Message, "ArgumentNullException", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            } catch (FormatException ex) {
                MessageBox.Show(ex.Message, "FormatException", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            } catch (OverflowException ex) {
                MessageBox.Show(ex.Message, "OverflowException", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            // Lock the bitmap's bits.  
            BitmapData bmpData = m_data.M_bmpOutput.LockBits(new Rectangle(0, 0, m_data.M_bmpOutput.Width, m_data.M_bmpOutput.Height), ImageLockMode.ReadWrite, m_data.M_bmpOutput.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int bytes = Math.Abs(bmpData.Stride)*m_data.M_bmpOutput.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            for (int i = 0; i < m_data.M_bmpOutput.Width; i++) {
                for (int j = 0; j < m_data.M_bmpOutput.Height; j++) {
                    int index = (j*bmpData.Stride) + (i*3);

                    rgbValues[index + 2] = (byte)(rgbValues[index + 2] << bits); // R
                    rgbValues[index + 1] = (byte)(rgbValues[index + 1] << bits); // G
                    rgbValues[index] = (byte)(rgbValues[index] << bits); // B
                }
            }

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            m_data.M_bmpOutput.UnlockBits(bmpData);
            
            string messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime;
            MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK) {
                m_data.M_noChange = false;
                m_data.M_action = ActionType.ShiftBits;
                m_data.M_bmpUndoRedo = m_data.M_bmpOutput.Clone() as Bitmap;
                m_data.M_undoStack.Push(m_data.M_bmpUndoRedo);
                m_data.M_redoStack.Clear();
                foreach (Window mainWindow in Application.Current.Windows) {
                    if (mainWindow.GetType() == typeof (MainWindow)) {
                        ((MainWindow)mainWindow).undo.IsEnabled = true;
                        ((MainWindow)mainWindow).redo.IsEnabled = false;
                        ((MainWindow)mainWindow).mainImage.Source = m_data.M_bmpOutput.BitmapToBitmapImage();
                    }
                }
                Close();
            }
        }
    }
}