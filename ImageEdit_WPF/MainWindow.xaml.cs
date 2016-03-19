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


using ImageAlgorithms;
using ImageAlgorithms.Algorithms;
using ImageEdit_WPF.HelperClasses;
using ImageEdit_WPF.Windows;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

// BUG: Undo/Redo
// PENDING: Canny Edge detection algorithm
// PENDING: Image croping
// PENDING: Rotation algorithm
// PENDING: Resize
// PENDING: HDR
// PENDING: Thermal
// PENDING: Pixelization
// PENDING: Better encoder (I used Magick.NET but I have to deal with some problems to use it again)
// PENDING: Preferences window


namespace ImageEdit_WPF {
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// Here we have all the components that appear on the main window.
    /// </summary>
    public partial class MainWindow : Window {
        private ImageData m_data = null;
        private ViewModel m_vm = null;
        private BackgroundWorker m_backgroundWorker = null;
        private TimeSpan elapsedTime = TimeSpan.Zero;

        #region MainWindow constructor
        /// <summary>
        /// Main Window constructor. Here we initialize the state of some menu items
        /// as well as checking the visibility of the status bar.
        /// </summary>
        public MainWindow() {
            m_data = new ImageData();
            m_vm = new ViewModel();

            InitializeComponent();
            DataContext = m_vm;

            m_backgroundWorker = new BackgroundWorker();
            m_backgroundWorker.WorkerReportsProgress = false;
            m_backgroundWorker.WorkerSupportsCancellation = false;
            m_backgroundWorker.DoWork += backgroundWorker_DoWork;
            m_backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            undo.IsEnabled = false;
            redo.IsEnabled = false;
            preferences.IsEnabled = false;

            if (statusBar.Visibility == Visibility.Visible) {
                statusBar.Visibility = Visibility.Visible;
                statusBarShowHide.IsChecked = true;
            } else {
                statusBar.Visibility = Visibility.Collapsed;
                statusBarShowHide.IsChecked = false;
            }
        }
        #endregion

        #region Open
        /// <summary>
        /// Command implementation of Open menu item. Check if the command can execute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void open_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        /// <summary>
        /// Command implementation of Open menu item. Executing command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void open_Executed(object sender, ExecutedRoutedEventArgs e) {
            try {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "JPEG/JPG - JPG Files|*.jpeg;*.jpg|BMP - Windows Bitmap|*.bmp|PNG - Portable Network Graphics|*.png";

                if (openFile.ShowDialog() == true) {
                    m_data.M_inputFilename = openFile.FileName;
                    m_data.M_bitmap = new Bitmap(m_data.M_inputFilename);
                    m_data.M_bmpUndoRedo = new Bitmap(m_data.M_inputFilename);
                    m_vm.M_bitmapBind = m_data.M_bitmap.BitmapToBitmapSource();

                    CalculateData_StatusBar();

                    m_data.M_undoStack.Clear();
                    m_data.M_undoStack.Push(m_data.M_bmpUndoRedo);
                    m_data.M_redoStack.Clear();
                }
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (UriFormatException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Reopen
        /// <summary>
        /// Reopen last image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reopen_Click(object sender, RoutedEventArgs e) {
            try {
                if (m_data.M_inputFilename != string.Empty) {
                    m_data.M_bitmap = new Bitmap(m_data.M_inputFilename);
                    m_data.M_bmpUndoRedo = new Bitmap(m_data.M_inputFilename);
                    m_vm.M_bitmapBind = m_data.M_bitmap.BitmapToBitmapSource();

                    m_data.M_undoStack.Clear();
                    m_data.M_undoStack.Push(m_data.M_bmpUndoRedo);
                    m_data.M_redoStack.Clear();
                } else {
                    MessageBox.Show("Open image first!", "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (UriFormatException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Command implementation of Save menu item. Check if the command can execute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        /// <summary>
        /// Command implementation of Save menu item. Executing command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_Executed(object sender, ExecutedRoutedEventArgs e) {
            try {
                if (m_data.M_outputFilename != string.Empty) {
                    switch (Path.GetExtension(m_data.M_outputFilename).ToLower()) {
                        case ".jpg":
                            SaveImage.Save(ImageFormat.Jpeg, m_data);
                            break;
                        case ".jpeg":
                            SaveImage.Save(ImageFormat.Jpeg, m_data);
                            break;
                        case ".png":
                            SaveImage.Save(ImageFormat.Png, m_data);
                            break;
                        case ".bmp":
                            SaveImage.Save(ImageFormat.Bmp, m_data);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                } else {
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.Filter = "JPEG/JPG - JPG Files|*.jpeg;*.jpg|BMP - Windows Bitmap|*.bmp|PNG - Portable Network Graphics|*.png";

                    if (saveFile.ShowDialog() == true) {
                        m_data.M_outputFilename = saveFile.FileName;

                        switch (Path.GetExtension(m_data.M_outputFilename).ToLower()) {
                            case ".jpg":
                                SaveImage.Save(ImageFormat.Jpeg, m_data);
                                break;
                            case ".jpeg":
                                SaveImage.Save(ImageFormat.Jpeg, m_data);
                                break;
                            case ".png":
                                SaveImage.Save(ImageFormat.Png, m_data);
                                break;
                            case ".bmp":
                                SaveImage.Save(ImageFormat.Bmp, m_data);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }

                m_data.M_noChange = true;
            } catch (EncoderFallbackException ex) {
                MessageBox.Show(ex.Message, "EncoderFallbackException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentNullException ex) {
                MessageBox.Show(ex.Message, "ArgumentNullException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ExternalException ex) {
                MessageBox.Show(ex.Message, "ExternalException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Save as...
        /// <summary>
        /// Save image at a preferred format.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAs_Click(object sender, RoutedEventArgs e) {
            try {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "JPEG/JPG - JPG Files|*.jpeg;*.jpg|BMP - Windows Bitmap|*.bmp|PNG - Portable Network Graphics|*.png";

                if (saveFile.ShowDialog() == true) {
                    m_data.M_outputFilename = saveFile.FileName;

                    switch (Path.GetExtension(m_data.M_outputFilename).ToLower()) {
                        case ".jpg":
                            SaveImage.Save(ImageFormat.Jpeg, m_data);
                            break;
                        case ".jpeg":
                            SaveImage.Save(ImageFormat.Jpeg, m_data);
                            break;
                        case ".png":
                            SaveImage.Save(ImageFormat.Png, m_data);
                            break;
                        case ".bmp":
                            SaveImage.Save(ImageFormat.Bmp, m_data);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                m_data.M_noChange = true;
            } catch (EncoderFallbackException ex) {
                MessageBox.Show(ex.Message, "EncoderFallbackException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentNullException ex) {
                MessageBox.Show(ex.Message, "ArgumentNullException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ExternalException ex) {
                MessageBox.Show(ex.Message, "ExternalException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Undo
        /// <summary>
        /// Command implementation of Undo menu item. Check if the command can execute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        /// <summary>
        /// Command implementation of Undo menu item. Executing command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Undo_Executed(object sender, ExecutedRoutedEventArgs e) {
            if (m_data.M_undoStack.Count <= 1) {
                undo.IsEnabled = false;
                return;
            }

            if (m_data.M_undoStack.Count == 2) {
                undo.IsEnabled = false;
            }

            m_data.M_bmpUndoRedo = m_data.M_undoStack.Pop();
            m_data.M_redoStack.Push(m_data.M_bmpUndoRedo);
            redo.IsEnabled = true;
            m_data.M_bitmap = m_data.M_undoStack.Peek();
            mainImage.Source = m_data.M_bitmap.BitmapToBitmapSource();
        }
        #endregion

        #region Redo
        /// <summary>
        /// Command implementation of Redo menu item. Check if the command can execute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        /// <summary>
        /// Command implementation of Redo menu item. Executing command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Redo_Executed(object sender, ExecutedRoutedEventArgs e) {
            if (m_data.M_redoStack.Count == 0) {
                redo.IsEnabled = false;
                return;
            }

            if (m_data.M_redoStack.Count == 1) {
                redo.IsEnabled = false;
            }

            m_data.M_bitmap = m_data.M_bmpUndoRedo = m_data.M_redoStack.Pop();
            m_data.M_undoStack.Push(m_data.M_bmpUndoRedo);
            mainImage.Source = m_data.M_bitmap.BitmapToBitmapSource();

            if (m_data.M_undoStack.Count > 1) {
                undo.IsEnabled = true;
            }
        }
        #endregion

        #region Status bar
        /// <summary>
        /// Get and set the visibility of status bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statusBarShowHide_Click(object sender, RoutedEventArgs e) {
            if (statusBar.Visibility == Visibility.Collapsed) {
                statusBar.Visibility = Visibility.Visible;
                statusBarShowHide.IsChecked = true;
            } else {
                statusBar.Visibility = Visibility.Collapsed;
                statusBarShowHide.IsChecked = false;
            }
        }
        #endregion

        #region Help
        /// <summary>
        /// Command implementation of Help menu item. Check if the command can execute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        /// <summary>
        /// Command implementation of Help menu item. Executing command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_Executed(object sender, ExecutedRoutedEventArgs e) {
            MessageBox.Show("This is the help window", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region About
        /// <summary>
        /// About window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void about_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("ImageEdit v0.9 beta", "Version", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Information
        /// <summary>
        /// Command implementation of Information menu item. Check if the command can execute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Information_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        /// <summary>
        /// Command implementation of Information menu item. Executing command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Information_Executed(object sender, ExecutedRoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                Information informationWindow = new Information(m_data);
                informationWindow.Owner = this;
                informationWindow.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Shift Bits
        /// <summary>
        /// Shift Bits algorithm. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shiftBits_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                ShiftBits shiftBitsWindow = new ShiftBits(m_data, m_vm);
                shiftBitsWindow.Owner = this;
                shiftBitsWindow.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Threshold
        /// <summary>
        /// Threshold algorithm. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void threshold_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                Threshold thresholdWindow = new Threshold(m_data, m_vm);
                thresholdWindow.Owner = this;
                thresholdWindow.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Auto Threshold
        /// <summary>
        /// Auto Threshold algorithm. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoThreshold_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                AutoThreshold autoThresholdWindow = new AutoThreshold(m_data, m_vm);
                autoThresholdWindow.Owner = this;
                autoThresholdWindow.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Negative
        /// <summary>
        /// Negative algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void negative_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                m_data.M_action = ActionType.Negative;
                m_backgroundWorker.RunWorkerAsync();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Square root
        /// <summary>
        /// Square root algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void squareRoot_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                m_data.M_action = ActionType.SquareRoot;
                m_backgroundWorker.RunWorkerAsync();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Contrast Enhancement
        /// <summary>
        /// Contrast Enhancement algorithm. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contrastEnhancement_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                ContrastEnhancement enhancement = new ContrastEnhancement(m_data, m_vm);
                enhancement.Owner = this;
                enhancement.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Brightness
        /// <summary>
        /// Brightness algorithm. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void brightness_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                Brightness brightness1 = new Brightness(m_data, m_vm);
                brightness1.Owner = this;
                brightness1.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Contrast
        /// <summary>
        /// Contrast algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contrast_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                Contrast contrastWindow = new Contrast(m_data, m_vm);
                contrastWindow.Owner = this;
                contrastWindow.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Convert to grayscale
        /// <summary>
        /// Convert a colored image to grayscale algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grayscale_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                m_data.M_action = ActionType.Grayscale;
                m_backgroundWorker.RunWorkerAsync();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Sepia tone
        /// <summary>
        /// Sepia tone.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sepia_OnClick(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                m_data.M_action = ActionType.Sepia;
                m_backgroundWorker.RunWorkerAsync();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Cartoon effect
        /// <summary>
        /// Cartoon effect. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CartoonEffect_OnClick(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                CartoonEffect cartoonEffectWindow = new CartoonEffect(m_data, m_vm);
                cartoonEffectWindow.Owner = this;
                cartoonEffectWindow.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Oil Paint effect
        /// <summary>
        /// Oil paint effect.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OilPaintEffect_OnClick(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                OilPaintEffect oilPaintEffectWindow = new OilPaintEffect(m_data, m_vm);
                oilPaintEffectWindow.Owner = this;
                oilPaintEffectWindow.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Histogram
        /// <summary>
        /// Histogram algorithm. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void histogram_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                Histogram histogramWindow = new Histogram(m_data, m_vm);
                histogramWindow.Owner = this;
                histogramWindow.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Histogram Equalization [RGB]
        /// <summary>
        /// Histogram Equalization algorithm for the RGB color space.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void histogramEqualizationRGB_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                m_data.M_action = ActionType.ImageEqualizationRGB;
                m_backgroundWorker.RunWorkerAsync();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Histogram Equalization [HSV]
        /// <summary>
        /// Histogram Equalization algorithm for the HSV color space.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void histogramEqualizationHSV_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                m_data.M_action = ActionType.ImageEqualizationHSV;
                m_backgroundWorker.RunWorkerAsync();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Histogram Equalization [YUV]
        /// <summary>
        /// Histogram Equalization algorithm for the YUV color space.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void histogramEqualizationYUV_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                m_data.M_action = ActionType.ImageEqualizationYUV;
                m_backgroundWorker.RunWorkerAsync();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Edge detection (Sobel)
        /// <summary>
        /// Edge detection using the Sobel algorithm. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sobel_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                Sobel sobelWindow = new Sobel(m_data, m_vm);
                sobelWindow.Owner = this;
                sobelWindow.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Edge detection (Gradient based)
        /// <summary>
        /// Edge detection using a gradient based method. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GradientBased_OnClick(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                GradientBased gradientBasedWindow = new GradientBased(m_data, m_vm);
                gradientBasedWindow.Owner = this;
                gradientBasedWindow.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Gaussian Blur
        /// <summary>
        /// Gaussian Blur algorithm. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gaussianBlur_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                GaussianBlur gaussianBlurWindow = new GaussianBlur(m_data, m_vm);
                gaussianBlurWindow.Owner = this;
                gaussianBlurWindow.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Sharpen
        /// <summary>
        /// Sharpen algorithm. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sharpen_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                Sharpen sharpenWindow = new Sharpen(m_data, m_vm);
                sharpenWindow.Owner = this;
                sharpenWindow.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Salt-and-Pepper Noise (Color)
        /// <summary>
        /// Colored Salt-and-Pepper Noise algorithm. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noiseColor_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                SaltPepperNoiseColor saltPepperNoiseColorWindow = new SaltPepperNoiseColor(m_data, m_vm);
                saltPepperNoiseColorWindow.Owner = this;
                saltPepperNoiseColorWindow.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Salt-and-Pepper Noise (Black/White)
        /// <summary>
        /// Black and White Salt-and-Pepper Noise algorithm. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noiseBW_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                SaltPepperNoiseBW saltPepperNoiseBwWindow = new SaltPepperNoiseBW(m_data, m_vm);
                saltPepperNoiseBwWindow.Owner = this;
                saltPepperNoiseBwWindow.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Noise reduction (Mean)
        /// <summary>
        /// Noise Reduction (Mean filter) algorithm. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noiseReductionMean_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                NoiseReductionMean noiseReductionMeanWindow = new NoiseReductionMean(m_data, m_vm);
                noiseReductionMeanWindow.Owner = this;
                noiseReductionMeanWindow.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Noise reduction (Median)
        /// <summary>
        /// Noise Reduction (Median filter) algorithm. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noiseReductionMedian_Click(object sender, RoutedEventArgs e) {
            if (m_data.M_inputFilename == string.Empty || m_data.M_bitmap == null) {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                NoiseReductionMedian noiseReductionMedianWindow = new NoiseReductionMedian(m_data, m_vm);
                noiseReductionMedianWindow.Owner = this;
                noiseReductionMedianWindow.Show();
            } catch (FileNotFoundException ex) {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (IndexOutOfRangeException ex) {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Calculate Data for status bar
        public void CalculateData_StatusBar() {
            int bpp = Image.GetPixelFormatSize(m_data.M_bitmap.PixelFormat);
            string resolution = m_data.M_bitmap.Width + " x " + m_data.M_bitmap.Height + " x " + bpp + " bpp";
            string size = string.Empty;
            FileInfo filesize = new FileInfo(m_data.M_inputFilename);
            switch (bpp) {
                case 8:
                    size = filesize.Length/1000 + " KB" + " / " + (m_data.M_bitmap.Width*m_data.M_bitmap.Height*1)/1000000 + " MB";
                    break;
                case 16:
                    size = filesize.Length/1000 + " KB" + " / " + (m_data.M_bitmap.Width*m_data.M_bitmap.Height*2)/1000000 + " MB";
                    break;
                case 24:
                    size = filesize.Length/1000 + " KB" + " / " + (m_data.M_bitmap.Width*m_data.M_bitmap.Height*3)/1000000 + " MB";
                    break;
                case 32:
                    size = filesize.Length/1000 + " KB" + " / " + (m_data.M_bitmap.Width*m_data.M_bitmap.Height*4)/1000000 + " MB";
                    break;
            }

            imageResolution.Text = resolution;
            imageSize.Text = size;
            separatorFirst.Visibility = Visibility.Visible;
        }
        #endregion

        #region Window_Closing
        /// <summary>
        /// Event that fires when we are trying to close the window.
        /// It is used to check if there are any unsaved changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e) {
            if (m_data.M_noChange == false) {
                MessageBoxResult result = MessageBox.Show("Quit without saving?", "Quit", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No) {
                    e.Cancel = true;
                }
            }
        }
        #endregion

        #region BackgroundWorker
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            switch (m_data.M_action) {
                case ActionType.Negative:
                    // Apply algorithm and return execution time
                    elapsedTime = Algorithms.Negative(m_data);
                    break;
                case ActionType.SquareRoot:
                    // Apply algorithm and return execution time
                    elapsedTime = Algorithms.SquareRoot(m_data);
                    break;
                case ActionType.ImageEqualizationRGB:
                    // Apply algorithm and return execution time
                    elapsedTime = Algorithms.HistogramEqualization_RGB(m_data);
                    break;
                case ActionType.ImageEqualizationHSV:
                    // Apply algorithm and return execution time
                    elapsedTime = Algorithms.HistogramEqualization_HSV(m_data);
                    break;
                case ActionType.ImageEqualizationYUV:
                    // Apply algorithm and return execution time
                    elapsedTime = Algorithms.HistogramEqualization_YUV(m_data);
                    break;
                case ActionType.Grayscale:
                    // Apply algorithm and return execution time
                    elapsedTime = Algorithms.ConvertToGrayscale(m_data);
                    break;
                case ActionType.Sepia:
                    // Apply algorithm and return execution time
                    elapsedTime = Algorithms.Sepia(m_data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
                undo.IsEnabled = true;
                redo.IsEnabled = false;
                m_data.M_redoStack.Clear();
            }
        }
        #endregion
    }
}
