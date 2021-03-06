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

using ImageAlgorithms;
using ImageAlgorithms.Algorithms;
using ImageEdit_WPF.HelperClasses;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;

namespace ImageEdit_WPF.Windows {
    /// <summary>
    /// Interaction logic for Sobel.xaml
    /// </summary>
    public partial class Sobel : Window {
        private ImageData m_data = null;
        private ViewModel m_vm = null;
        private BackgroundWorker m_backgroundWorker = null;
        private TimeSpan elapsedTime = TimeSpan.Zero;

        /// <summary>
        /// Size of the kernels.
        /// </summary>
        private int m_kernelSize = 0;

        /// <summary>
        /// Sobel <c>constructor</c>.
        /// Here we initialiaze the images and also we set the focus
        /// at the 'OK' button and at one of the three radio boxes (kernel size).
        /// </summary>
        public Sobel(ImageData data, ViewModel vm) {
            m_data = data;
            m_vm = vm;

            InitializeComponent();
            three.IsChecked = true;

            m_backgroundWorker = new BackgroundWorker();
            m_backgroundWorker.WorkerReportsProgress = false;
            m_backgroundWorker.WorkerSupportsCancellation = false;
            m_backgroundWorker.DoWork += backgroundWorker_DoWork;
            m_backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
        }

        /// <summary>
        /// Set kernel's size to 3.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void three_Checked(object sender, RoutedEventArgs e) {
            m_kernelSize = 3;
        }

        /// <summary>
        /// Set kernel's size 5.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void five_Checked(object sender, RoutedEventArgs e) {
            m_kernelSize = 5;
        }

        /// <summary>
        /// Set kernel's size to 7.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seven_Checked(object sender, RoutedEventArgs e) {
            m_kernelSize = 7;
        }

        /// <summary>
        /// Implementation of the Sobel (Edge detector) algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ok_Click(object sender, RoutedEventArgs e) {
            m_backgroundWorker.RunWorkerAsync();
            Close();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            switch (m_kernelSize) {
                case 3:
                    // Apply algorithm and return execution time
                    elapsedTime = Algorithms.EdgeDetection_Sobel(m_data, m_kernelSize, Kernel.M_Sobel3x3_X, Kernel.M_Sobel3x3_Y);
                    break;
                case 5:
                    // Apply algorithm and return execution time
                    elapsedTime = Algorithms.EdgeDetection_Sobel(m_data, m_kernelSize, Kernel.M_Sobel5x5_X, Kernel.M_Sobel5x5_Y);
                    break;
                case 7:
                    // Apply algorithm and return execution time
                    elapsedTime = Algorithms.EdgeDetection_Sobel(m_data, m_kernelSize, Kernel.M_Sobel7x7_X, Kernel.M_Sobel7x7_Y);
                    break;
            }
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
