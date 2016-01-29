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
using System.Windows;

namespace ImageEdit_WPF.Windows {
    /// <summary>
    /// Interaction logic for GaussianBlur.xaml
    /// </summary>
    public partial class GaussianBlur : Window {
        private ImageData m_data = null;
        private ViewModel m_vm = null;
        private BackgroundWorker m_backgroundWorker = null;
        private TimeSpan elapsedTime = TimeSpan.Zero;

        /// <summary>
        /// Size of the kernel.
        /// </summary>
        private int m_sizeMask = 0;

        /// <summary>
        /// Kernel.
        /// </summary>
        private int[,] maskX = null;

        /// <summary>
        /// Gaussian Blur <c>constructor</c>.
        /// Here we initialiaze the images and also we set the focus
        /// at the 'OK' button and at one of the three radio boxes (kernel size).
        /// </summary>
        public GaussianBlur(ImageData data, ViewModel vm) {
            m_data = data;
            m_vm = vm;

            InitializeComponent();
            three.IsChecked = true;
            ok.Focus();

            m_backgroundWorker = new BackgroundWorker();
            m_backgroundWorker.WorkerReportsProgress = false;
            m_backgroundWorker.WorkerSupportsCancellation = false;
            m_backgroundWorker.DoWork += backgroundWorker_DoWork;
            m_backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
        }

        /// <summary>
        /// If kernel's size is 3x3, the following attributes are set.
        /// <list type="bullet">
        ///     <item>
        ///     <description>
        ///         Kernel size.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Height of the window.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Width of the window.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box width.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box height.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         OK button attributes.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Text Boxes visibility and values.
        ///     </description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// As for the textBoxes that hold the values of the kernel,
        /// some of them are not visible because is not needed to.
        /// They exist only for bigger kernels.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void three_Checked(object sender, RoutedEventArgs e) {
            m_sizeMask = 3;

            Height = 270;
            Width = 190;

            groupBox.Width = 120;
            groupBox.Height = 100;

            ok.Margin = new Thickness(50, 10, 50, 10);

            tbx1.Text = "1";
            tbx2.Text = "2";
            tbx3.Text = "1";
            tbx8.Text = "2";
            tbx9.Text = "4";
            tbx10.Text = "2";
            tbx15.Text = "1";
            tbx16.Text = "2";
            tbx17.Text = "1";

            tbx1.Visibility = Visibility.Visible;
            tbx2.Visibility = Visibility.Visible;
            tbx3.Visibility = Visibility.Visible;
            tbx4.Visibility = Visibility.Collapsed;
            tbx5.Visibility = Visibility.Collapsed;
            tbx6.Visibility = Visibility.Collapsed;
            tbx7.Visibility = Visibility.Collapsed;
            tbx8.Visibility = Visibility.Visible;
            tbx9.Visibility = Visibility.Visible;
            tbx10.Visibility = Visibility.Visible;
            tbx11.Visibility = Visibility.Collapsed;
            tbx12.Visibility = Visibility.Collapsed;
            tbx13.Visibility = Visibility.Collapsed;
            tbx14.Visibility = Visibility.Collapsed;
            tbx15.Visibility = Visibility.Visible;
            tbx16.Visibility = Visibility.Visible;
            tbx17.Visibility = Visibility.Visible;
            tbx18.Visibility = Visibility.Collapsed;
            tbx19.Visibility = Visibility.Collapsed;
            tbx20.Visibility = Visibility.Collapsed;
            tbx21.Visibility = Visibility.Collapsed;
            tbx22.Visibility = Visibility.Collapsed;
            tbx23.Visibility = Visibility.Collapsed;
            tbx24.Visibility = Visibility.Collapsed;
            tbx25.Visibility = Visibility.Collapsed;
            tbx26.Visibility = Visibility.Collapsed;
            tbx27.Visibility = Visibility.Collapsed;
            tbx28.Visibility = Visibility.Collapsed;
            tbx29.Visibility = Visibility.Collapsed;
            tbx30.Visibility = Visibility.Collapsed;
            tbx31.Visibility = Visibility.Collapsed;
            tbx32.Visibility = Visibility.Collapsed;
            tbx33.Visibility = Visibility.Collapsed;
            tbx34.Visibility = Visibility.Collapsed;
            tbx35.Visibility = Visibility.Collapsed;
            tbx36.Visibility = Visibility.Collapsed;
            tbx37.Visibility = Visibility.Collapsed;
            tbx38.Visibility = Visibility.Collapsed;
            tbx39.Visibility = Visibility.Collapsed;
            tbx40.Visibility = Visibility.Collapsed;
            tbx41.Visibility = Visibility.Collapsed;
            tbx42.Visibility = Visibility.Collapsed;
            tbx43.Visibility = Visibility.Collapsed;
            tbx44.Visibility = Visibility.Collapsed;
            tbx45.Visibility = Visibility.Collapsed;
            tbx46.Visibility = Visibility.Collapsed;
            tbx47.Visibility = Visibility.Collapsed;
            tbx48.Visibility = Visibility.Collapsed;
            tbx49.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If kernel's size is 5x5, the following attributes are set.
        /// <list type="bullet">
        ///     <item>
        ///     <description>
        ///         Kernel size.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Height of the window.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Width of the window.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box width.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box height.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         OK button attributes.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Text Boxes visibility and values.
        ///     </description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// As for the textBoxes that hold the values of the kernel,
        /// some of them are not visible because is not needed to.
        /// They exist only for bigger kernels.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void five_Checked(object sender, RoutedEventArgs e) {
            m_sizeMask = 5;

            Height = 310;
            Width = 230;

            groupBox.Width = 180;
            groupBox.Height = 140;

            ok.Margin = new Thickness(70, 10, 70, 10);

            tbx1.Text = "2";
            tbx2.Text = "7";
            tbx3.Text = "12";
            tbx4.Text = "7";
            tbx5.Text = "2";
            tbx8.Text = "7";
            tbx9.Text = "31";
            tbx10.Text = "52";
            tbx11.Text = "31";
            tbx12.Text = "7";
            tbx15.Text = "12";
            tbx16.Text = "52";
            tbx17.Text = "127";
            tbx18.Text = "52";
            tbx19.Text = "12";
            tbx22.Text = "7";
            tbx23.Text = "31";
            tbx24.Text = "52";
            tbx25.Text = "31";
            tbx26.Text = "7";
            tbx29.Text = "2";
            tbx30.Text = "7";
            tbx31.Text = "12";
            tbx32.Text = "7";
            tbx33.Text = "2";

            tbx1.Visibility = Visibility.Visible;
            tbx2.Visibility = Visibility.Visible;
            tbx3.Visibility = Visibility.Visible;
            tbx4.Visibility = Visibility.Visible;
            tbx5.Visibility = Visibility.Visible;
            tbx6.Visibility = Visibility.Collapsed;
            tbx7.Visibility = Visibility.Collapsed;
            tbx8.Visibility = Visibility.Visible;
            tbx9.Visibility = Visibility.Visible;
            tbx10.Visibility = Visibility.Visible;
            tbx11.Visibility = Visibility.Visible;
            tbx12.Visibility = Visibility.Visible;
            tbx13.Visibility = Visibility.Collapsed;
            tbx14.Visibility = Visibility.Collapsed;
            tbx15.Visibility = Visibility.Visible;
            tbx16.Visibility = Visibility.Visible;
            tbx17.Visibility = Visibility.Visible;
            tbx18.Visibility = Visibility.Visible;
            tbx19.Visibility = Visibility.Visible;
            tbx20.Visibility = Visibility.Collapsed;
            tbx21.Visibility = Visibility.Collapsed;
            tbx22.Visibility = Visibility.Visible;
            tbx23.Visibility = Visibility.Visible;
            tbx24.Visibility = Visibility.Visible;
            tbx25.Visibility = Visibility.Visible;
            tbx26.Visibility = Visibility.Visible;
            tbx27.Visibility = Visibility.Collapsed;
            tbx28.Visibility = Visibility.Collapsed;
            tbx29.Visibility = Visibility.Visible;
            tbx30.Visibility = Visibility.Visible;
            tbx31.Visibility = Visibility.Visible;
            tbx32.Visibility = Visibility.Visible;
            tbx33.Visibility = Visibility.Visible;
            tbx34.Visibility = Visibility.Collapsed;
            tbx35.Visibility = Visibility.Collapsed;
            tbx36.Visibility = Visibility.Collapsed;
            tbx37.Visibility = Visibility.Collapsed;
            tbx38.Visibility = Visibility.Collapsed;
            tbx39.Visibility = Visibility.Collapsed;
            tbx40.Visibility = Visibility.Collapsed;
            tbx41.Visibility = Visibility.Collapsed;
            tbx42.Visibility = Visibility.Collapsed;
            tbx43.Visibility = Visibility.Collapsed;
            tbx44.Visibility = Visibility.Collapsed;
            tbx45.Visibility = Visibility.Collapsed;
            tbx46.Visibility = Visibility.Collapsed;
            tbx47.Visibility = Visibility.Collapsed;
            tbx48.Visibility = Visibility.Collapsed;
            tbx49.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If kernel's size is 7x7, the following attributes are set.
        /// <list type="bullet">
        ///     <item>
        ///     <description>
        ///         Kernel size.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Height of the window.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Width of the window.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box width.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Group box height.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         OK button attributes.
        ///     </description>
        ///     </item>
        ///     <item>
        ///     <description>
        ///         Text Boxes visibility and values.
        ///     </description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seven_Checked(object sender, RoutedEventArgs e) {
            m_sizeMask = 7;

            Height = 350;
            Width = 280;

            groupBox.Width = 240;
            groupBox.Height = 180;

            ok.Margin = new Thickness(90, 10, 90, 10);

            tbx1.Text = "1";
            tbx2.Text = "1";
            tbx3.Text = "2";
            tbx4.Text = "2";
            tbx5.Text = "2";
            tbx6.Text = "1";
            tbx7.Text = "1";
            tbx8.Text = "1";
            tbx9.Text = "3";
            tbx10.Text = "4";
            tbx11.Text = "5";
            tbx12.Text = "4";
            tbx13.Text = "3";
            tbx14.Text = "1";
            tbx15.Text = "2";
            tbx16.Text = "4";
            tbx17.Text = "7";
            tbx18.Text = "8";
            tbx19.Text = "7";
            tbx20.Text = "4";
            tbx21.Text = "2";
            tbx22.Text = "2";
            tbx23.Text = "5";
            tbx24.Text = "8";
            tbx25.Text = "10";
            tbx26.Text = "8";
            tbx27.Text = "5";
            tbx28.Text = "2";
            tbx29.Text = "2";
            tbx30.Text = "4";
            tbx31.Text = "7";
            tbx32.Text = "8";
            tbx33.Text = "7";
            tbx34.Text = "4";
            tbx35.Text = "2";
            tbx36.Text = "1";
            tbx37.Text = "3";
            tbx38.Text = "4";
            tbx39.Text = "5";
            tbx40.Text = "4";
            tbx41.Text = "3";
            tbx42.Text = "1";
            tbx43.Text = "1";
            tbx44.Text = "1";
            tbx45.Text = "2";
            tbx46.Text = "2";
            tbx47.Text = "2";
            tbx48.Text = "1";
            tbx49.Text = "1";

            tbx1.Visibility = Visibility.Visible;
            tbx2.Visibility = Visibility.Visible;
            tbx3.Visibility = Visibility.Visible;
            tbx4.Visibility = Visibility.Visible;
            tbx5.Visibility = Visibility.Visible;
            tbx6.Visibility = Visibility.Visible;
            tbx7.Visibility = Visibility.Visible;
            tbx8.Visibility = Visibility.Visible;
            tbx9.Visibility = Visibility.Visible;
            tbx10.Visibility = Visibility.Visible;
            tbx11.Visibility = Visibility.Visible;
            tbx12.Visibility = Visibility.Visible;
            tbx13.Visibility = Visibility.Visible;
            tbx14.Visibility = Visibility.Visible;
            tbx15.Visibility = Visibility.Visible;
            tbx16.Visibility = Visibility.Visible;
            tbx17.Visibility = Visibility.Visible;
            tbx18.Visibility = Visibility.Visible;
            tbx19.Visibility = Visibility.Visible;
            tbx20.Visibility = Visibility.Visible;
            tbx21.Visibility = Visibility.Visible;
            tbx22.Visibility = Visibility.Visible;
            tbx23.Visibility = Visibility.Visible;
            tbx24.Visibility = Visibility.Visible;
            tbx25.Visibility = Visibility.Visible;
            tbx26.Visibility = Visibility.Visible;
            tbx27.Visibility = Visibility.Visible;
            tbx28.Visibility = Visibility.Visible;
            tbx29.Visibility = Visibility.Visible;
            tbx30.Visibility = Visibility.Visible;
            tbx31.Visibility = Visibility.Visible;
            tbx32.Visibility = Visibility.Visible;
            tbx33.Visibility = Visibility.Visible;
            tbx34.Visibility = Visibility.Visible;
            tbx35.Visibility = Visibility.Visible;
            tbx36.Visibility = Visibility.Visible;
            tbx37.Visibility = Visibility.Visible;
            tbx38.Visibility = Visibility.Visible;
            tbx39.Visibility = Visibility.Visible;
            tbx40.Visibility = Visibility.Visible;
            tbx41.Visibility = Visibility.Visible;
            tbx42.Visibility = Visibility.Visible;
            tbx43.Visibility = Visibility.Visible;
            tbx44.Visibility = Visibility.Visible;
            tbx45.Visibility = Visibility.Visible;
            tbx46.Visibility = Visibility.Visible;
            tbx47.Visibility = Visibility.Visible;
            tbx48.Visibility = Visibility.Visible;
            tbx49.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Implementation of the Gaussian Blur algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ok_Click(object sender, RoutedEventArgs e) {
            switch (m_sizeMask) {
                case 3:
                    maskX = new int[3, 3] {
                        {int.Parse(tbx1.Text), int.Parse(tbx2.Text), int.Parse(tbx3.Text)},
                        {int.Parse(tbx8.Text), int.Parse(tbx9.Text), int.Parse(tbx10.Text)},
                        {int.Parse(tbx15.Text), int.Parse(tbx16.Text), int.Parse(tbx17.Text)}
                    };
                    break;
                case 5:
                    maskX = new int[5, 5] {
                        {int.Parse(tbx1.Text), int.Parse(tbx2.Text), int.Parse(tbx3.Text), int.Parse(tbx4.Text), int.Parse(tbx5.Text)},
                        {int.Parse(tbx8.Text), int.Parse(tbx9.Text), int.Parse(tbx10.Text), int.Parse(tbx11.Text), int.Parse(tbx12.Text)},
                        {int.Parse(tbx15.Text), int.Parse(tbx16.Text), int.Parse(tbx17.Text), int.Parse(tbx18.Text), int.Parse(tbx19.Text)},
                        {int.Parse(tbx22.Text), int.Parse(tbx23.Text), int.Parse(tbx24.Text), int.Parse(tbx25.Text), int.Parse(tbx26.Text)},
                        {int.Parse(tbx29.Text), int.Parse(tbx30.Text), int.Parse(tbx31.Text), int.Parse(tbx32.Text), int.Parse(tbx33.Text)}
                    };
                    break;
                case 7:
                    maskX = new int[7, 7] {
                        {int.Parse(tbx1.Text), int.Parse(tbx2.Text), int.Parse(tbx3.Text), int.Parse(tbx4.Text), int.Parse(tbx5.Text), int.Parse(tbx6.Text), int.Parse(tbx7.Text)},
                        {int.Parse(tbx8.Text), int.Parse(tbx9.Text), int.Parse(tbx10.Text), int.Parse(tbx11.Text), int.Parse(tbx12.Text), int.Parse(tbx13.Text), int.Parse(tbx14.Text)},
                        {int.Parse(tbx15.Text), int.Parse(tbx16.Text), int.Parse(tbx17.Text), int.Parse(tbx18.Text), int.Parse(tbx19.Text), int.Parse(tbx20.Text), int.Parse(tbx21.Text)},
                        {int.Parse(tbx22.Text), int.Parse(tbx23.Text), int.Parse(tbx24.Text), int.Parse(tbx25.Text), int.Parse(tbx26.Text), int.Parse(tbx27.Text), int.Parse(tbx28.Text)},
                        {int.Parse(tbx29.Text), int.Parse(tbx30.Text), int.Parse(tbx31.Text), int.Parse(tbx32.Text), int.Parse(tbx33.Text), int.Parse(tbx34.Text), int.Parse(tbx35.Text)},
                        {int.Parse(tbx36.Text), int.Parse(tbx37.Text), int.Parse(tbx38.Text), int.Parse(tbx39.Text), int.Parse(tbx40.Text), int.Parse(tbx41.Text), int.Parse(tbx42.Text)},
                        {int.Parse(tbx43.Text), int.Parse(tbx44.Text), int.Parse(tbx45.Text), int.Parse(tbx46.Text), int.Parse(tbx47.Text), int.Parse(tbx48.Text), int.Parse(tbx49.Text)}
                    };
                    break;
            }
            m_backgroundWorker.RunWorkerAsync();
            Close();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            // Apply algorithm and return execution time
            elapsedTime = Algorithms.GaussianBlur(m_data, m_sizeMask, maskX);
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
