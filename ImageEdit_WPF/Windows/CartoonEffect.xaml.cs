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
using ImageEdit_WPF.HelperClasses.Algorithms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows;

namespace ImageEdit_WPF.Windows {
    /// <summary>
    /// Interaction logic for CartoonEffect.xaml
    /// </summary>
    public partial class CartoonEffect : Window {
        private ImageData m_data = null;
        private ViewModel m_vm = null;
        private BackgroundWorker m_backgroundWorker = null;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private byte threshold = 0;
        private KernelType kernelType = KernelType.None;

        public CartoonEffect(ImageData data, ViewModel vm) {
            m_data = data;
            m_vm = vm;

            InitializeComponent();

            m_backgroundWorker = new BackgroundWorker();
            m_backgroundWorker.WorkerReportsProgress = false;
            m_backgroundWorker.WorkerSupportsCancellation = false;
            m_backgroundWorker.DoWork += backgroundWorker_DoWork;
            m_backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            // Fill list of filters
            List<string> filters = new List<string>();
            filters.Add("None");
            filters.Add("Gaussian (3x3)");
            filters.Add("Gaussian (5x5)");
            filters.Add("Gaussian (7x7)");
            filters.Add("Mean (3x3)");
            filters.Add("Mean (5x5)");
            filters.Add("Mean (7x7)");
            filters.Add("Median (3x3)");
            filters.Add("Median (5x5)");
            filters.Add("Median (7x7)");
            filters.Add("Median (9x9)");
            filters.Add("Low pass (3x3)");
            filters.Add("Low pass (5x5)");
            filters.Add("Sharpen (3x3)");
            filters.Add("Sharpen (5x5)");
            filters.Add("Sharpen (7x7)");
            cmbFilters.ItemsSource = filters;
            cmbFilters.SelectedIndex = 0;
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e) {
            try {
                switch (cmbFilters.SelectionBoxItem.ToString()) {
                    case "Gaussian (3x3)":
                        kernelType = KernelType.Gaussian3x3;
                        break;
                    case "Gaussian (5x5)":
                        kernelType = KernelType.Gaussian5x5;
                        break;
                    case "Gaussian (7x7)":
                        kernelType = KernelType.Gaussian7x7;
                        break;
                    case "Mean (3x3)":
                        kernelType = KernelType.Mean3x3;
                        break;
                    case "Mean (5x5)":
                        kernelType = KernelType.Mean5x5;
                        break;
                    case "Mean (7x7)":
                        kernelType = KernelType.Mean7x7;
                        break;
                    case "Median (3x3)":
                        kernelType = KernelType.Median3x3;
                        break;
                    case "Median (5x5)":
                        kernelType = KernelType.Median5x5;
                        break;
                    case "Median (7x7)":
                        kernelType = KernelType.Median7x7;
                        break;
                    case "Median (9x9)":
                        kernelType = KernelType.Median9x9;
                        break;
                    case "Low pass (3x3)":
                        kernelType = KernelType.LowPass3x3;
                        break;
                    case "Low pass (5x5)":
                        kernelType = KernelType.LowPass5x5;
                        break;
                    case "Sharpen (3x3)":
                        kernelType = KernelType.Sharpen3x3;
                        break;
                    case "Sharpen (5x5)":
                        kernelType = KernelType.Sharpen5x5;
                        break;
                    case "Sharpen (7x7)":
                        kernelType = KernelType.Sharpen7x7;
                        break;
                    case "None":
                        kernelType = KernelType.None;
                        break;
                }

                threshold = byte.Parse(txbThreshold.Text);
                if (threshold > 255 || threshold < 0) {
                    string message = "Wrong range\r\n\r\nGive a number between 0 and 255";
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
            m_backgroundWorker.RunWorkerAsync();
            Close();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            // Apply algorithm and return execution time
            elapsedTime = Algorithms.CartoonEffect(m_data, threshold, kernelType);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            string messageOperation = "Done!\r\n\r\nElapsed time (HH:MM:SS.MS): " + elapsedTime;
            MessageBoxResult result = MessageBoxResult.None;

            if (e.Error != null) {
                MessageBox.Show(e.Error.Message, "Error");
            }

            result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK) {
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
