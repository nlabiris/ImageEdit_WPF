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
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows;

namespace ImageEdit_WPF.Windows {
    /// <summary>
    /// Interaction logic for Contrast.xaml
    /// </summary>
    public partial class Contrast : Window {
        private ImageData m_data = null;
        private ViewModel m_vm = null;
        private BackgroundWorker m_backgroundWorker = null;
        private TimeSpan elapsedTime = TimeSpan.Zero;

        /// <summary>
        /// Contrast <c>constructor</c>
        /// Here we initialiaze the images and also we set the focus at the textBox being used.
        /// </summary>
        public Contrast(ImageData data, ViewModel vm) {
            m_data = data;
            m_vm = vm;

            InitializeComponent();
            textboxContrast.Focus();

            m_backgroundWorker = new BackgroundWorker();
            m_backgroundWorker.WorkerReportsProgress = true;
            m_backgroundWorker.WorkerSupportsCancellation = false;
            m_backgroundWorker.DoWork += backgroundWorker_DoWork;
            m_backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            m_backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
        }

        /// <summary>
        /// Implementation of the Contrast algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ok_Click(object sender, RoutedEventArgs e) {
            double contrast = 0;

            try {
                contrast = double.Parse(textboxContrast.Text, new CultureInfo("el-GR"));
                //if (brightness > 255 || brightness < 0)
                //{
                //    String message = "Wrong range" + Environment.NewLine + Environment.NewLine + "Give a number between 0 and 255";
                //    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}
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
            m_backgroundWorker.RunWorkerAsync(contrast);
            Close();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            double contrast = (double)e.Argument;

            // Apply algorithm and return execution time
            elapsedTime = Algorithms.Contrast(m_data, contrast, m_backgroundWorker);
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            m_vm.M_progress.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            string messageOperation = "Done!\r\n\r\nElapsed time (HH:MM:SS.MS): " + elapsedTime;
            MessageBoxResult result = MessageBoxResult.None;

            if (e.Error != null) {
                MessageBox.Show(e.Error.Message, "Error");
            }

            result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK) {
                m_vm.M_progress.Value = 0;
                m_vm.M_bitmapBind = m_data.M_bitmap.BitmapToBitmapSource(); // Set main image
                m_data.M_noChange = false;
                m_data.M_bmpUndoRedo = m_data.M_bitmap.Clone() as Bitmap;
                m_data.M_undoStack.Push(m_data.M_bmpUndoRedo);
                foreach (Window mainWindow in Application.Current.Windows) {
                    if (mainWindow.GetType() == typeof (MainWindow)) {
                        ((MainWindow)mainWindow).undo.IsEnabled = true;
                        ((MainWindow)mainWindow).redo.IsEnabled = false;
                    }
                }
                m_data.M_redoStack.Clear();
            }
        }
    }
}