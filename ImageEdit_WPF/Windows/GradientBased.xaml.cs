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
using System.Globalization;
using System.Windows;

namespace ImageEdit_WPF.Windows {
    /// <summary>
    /// Interaction logic for GradientBased.xaml
    /// </summary>
    public partial class GradientBased : Window {
        private ImageData m_data = null;
        private ViewModel m_vm = null;
        private BackgroundWorker m_backgroundWorker = null;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private EdgeFilterType filterType = EdgeFilterType.EdgeDetectMono;
        private DerivativeLevel derivativeLevel = DerivativeLevel.FirstDerivative;
        private byte threshold = 0;
        private double bFactor = 0.0;
        private double gFactor = 0.0;
        private double rFactor = 0.0;

        public GradientBased(ImageData data, ViewModel vm) {
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
            filters.Add("Edge Detect Mono");
            filters.Add("Edge Detect Gradient");
            filters.Add("Sharpen");
            filters.Add("Sharpen Gradient");
            cmbFilters.ItemsSource = filters;
            cmbFilters.SelectedIndex = 0;
            rdbFirstDerivative.IsChecked = true;
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e) {
            try {
                switch (cmbFilters.SelectionBoxItem.ToString()) {
                    case "Edge Detect Mono":
                        filterType = EdgeFilterType.EdgeDetectMono;
                        break;
                    case "Edge Detect Gradient":
                        filterType = EdgeFilterType.EdgeDetectGradient;
                        break;
                    case "Sharpen":
                        filterType = EdgeFilterType.Sharpen;
                        break;
                    case "Sharpen Gradient":
                        filterType = EdgeFilterType.SharpenGradient;
                        break;
                }

                if (rdbFirstDerivative.IsChecked == true) {
                    derivativeLevel = DerivativeLevel.FirstDerivative;
                } else if (rdbSecondDerivative.IsChecked == true) {
                    derivativeLevel = DerivativeLevel.SecondDerivative;
                }

                threshold = byte.Parse(txbThreshold.Text);
                if (threshold > 255 || threshold < 0) {
                    string message = "Wrong range\r\n\r\nGive a number between 0 and 255";
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (txbBlueFactor.Text.Contains(".")) {
                    bFactor = double.Parse(txbBlueFactor.Text, new CultureInfo("en-US"));
                    if (bFactor > 1.0 || bFactor < 0.0) {
                        string message = "Wrong range\r\n\r\nGive a number between 0.0 and 1.0";
                        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                } else if (txbBlueFactor.Text.Contains(",")) {
                    bFactor = double.Parse(txbBlueFactor.Text, new CultureInfo("el-GR"));
                    if (bFactor > 1.0 || bFactor < 0.0) {
                        string message = "Wrong range\r\n\r\nGive a number between 0,0 and 1,0";
                        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                if (txbGreenFactor.Text.Contains(".")) {
                    gFactor = double.Parse(txbGreenFactor.Text, new CultureInfo("en-US"));
                    if (gFactor > 1.0 || gFactor < 0.0) {
                        string message = "Wrong range\r\n\r\nGive a number between 0.0 and 1.0";
                        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                } else if (txbGreenFactor.Text.Contains(",")) {
                    gFactor = double.Parse(txbGreenFactor.Text, new CultureInfo("el-GR"));
                    if (gFactor > 1.0 || gFactor < 0.0) {
                        string message = "Wrong range\r\n\r\nGive a number between 0,0 and 1,0";
                        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                if (txbRedFactor.Text.Contains(".")) {
                    rFactor = double.Parse(txbRedFactor.Text, new CultureInfo("en-US"));
                    if (rFactor > 1.0 || rFactor < 0.0) {
                        string message = "Wrong range\r\n\r\nGive a number between 0.0 and 1.0";
                        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                } else if (txbRedFactor.Text.Contains(",")) {
                    rFactor = double.Parse(txbRedFactor.Text, new CultureInfo("el-GR"));
                    if (rFactor > 1.0 || rFactor < 0.0) {
                        string message = "Wrong range\r\n\r\nGive a number between 0,0 and 1,0";
                        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
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
            elapsedTime = Algorithms.EdgeDetection_GradientBased(m_data, filterType, derivativeLevel, rFactor, gFactor, bFactor, threshold);
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
