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
using System.Windows;

namespace ImageEdit_WPF.Windows {
    /// <summary>
    /// Interaction logic for ShiftBits.xaml
    /// </summary>
    public partial class ShiftBits : Window {
        private ImageData m_data = null;

        /// <summary>
        /// Shift Bits <c>constructor</c>.
        /// Here we initialiaze the images and also we set the focus at the textBox being used.
        /// </summary>
        public ShiftBits(ImageData data) {
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
                    string message = "Wrong range\r\n\r\nGive a number between 0 and 7";
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

            // Apply algorithm and return execution time
            TimeSpan elapsedTime = Algorithms.ShiftBits(m_data, bits);

            // Set main image
            m_data.M_bitmapBind = m_data.M_bitmap.BitmapToBitmapSource();

            string messageOperation = "Done!\r\n\r\nElapsed time (HH:MM:SS.MS): " + elapsedTime;
            MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);

            m_data.M_noChange = false;
            m_data.M_action = ActionType.ShiftBits;
            m_data.M_bmpUndoRedo = m_data.M_bitmap.Clone() as Bitmap;
            m_data.M_undoStack.Push(m_data.M_bmpUndoRedo);
            m_data.M_redoStack.Clear();
            foreach (Window mainWindow in Application.Current.Windows) {
                if (mainWindow.GetType() == typeof (MainWindow)) {
                    ((MainWindow)mainWindow).undo.IsEnabled = true;
                    ((MainWindow)mainWindow).redo.IsEnabled = false;
                }
            }
            Close();
        }
    }
}