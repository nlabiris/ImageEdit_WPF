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



using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;


// BUG:  Undo/Redo (!!)
// TODO: Color to Grayscale algorithm
// TODO: Canny Edge detection algorithm
// TODO: Noise reduction algorithm
// TODO: Image croping
// TODO: Rotation algorithm
// TODO: Resize
// TODO: HDR
// TODO: Thermal
// TODO: Pixelization
// TODO: null action in enum
// TODO: Better encoder (I used Magick.NET but I have to deal with some problems to use it again)
// TODO: Check sum of kernel windows
// TODO: Progress bar on every algorithm window
// TODO: Preferences window


namespace ImageEdit_WPF
{
    /// <summary>
    /// <c>ActionType</c> enumeration is used at the Undo/Redo sytem (not now).
    /// </summary>
    public enum ActionType
    {
        ShiftBits = 0,
        Threshold = 1,
        AutoThreshold = 2,
        Negative = 3,
        SquareRoot = 4,
        ContrastEnhancement = 5,
        Brightness = 6,
        Contrast = 7,
        ImageSummarization = 8,
        ImageSubtraction = 9,
        ImageConvolution = 10,
        ImageEqualizationRGB = 11,
        ImageEqualizationHSV = 12,
        ImageEqualizationYUV = 13
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// Here we have all the main components that appear on the main window.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Input filename of the image.
        /// </summary>
        private string _inputFilename = string.Empty;

        /// <summary>
        /// Output filename of the image.
        /// </summary>
        private string _outputFilename = string.Empty;

        /// <summary>
        /// Check if the image has been modified
        /// </summary>
        public static bool NoChange = true;

        /// <summary>
        /// Input image used only for displaying purposes.
        /// </summary>
        private BitmapImage _bmpInput = null;

        /// <summary>
        /// Output image that carries all changes until saved.
        /// </summary>
        private System.Drawing.Bitmap _bmpOutput = null;

        /// <summary>
        /// Stack that contains all undone actions.
        /// </summary>
        public static Stack<System.Drawing.Bitmap> UndoStack = new Stack<System.Drawing.Bitmap>();

        /// <summary>
        /// Stack that contains actions to be redone.
        /// </summary>
        public static Stack<System.Drawing.Bitmap> RedoStack = new Stack<System.Drawing.Bitmap>();

        /// <summary>
        /// Type of action (which algorithm used).
        /// </summary>
        public static ActionType Action;

        /// <summary>
        /// Image used at the Undo/Redo system.
        /// </summary>
        public Bitmap BmpUndoRedo = null;



        #region MainWindow constructor
        /// <summary>
        /// Main Window constructor. Here we initialize the state of some menu items
        /// as well as checking the visibility of the status bar.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            undo.IsEnabled = false;
            redo.IsEnabled = false;
            preferences.IsEnabled = false;

            if (menuBar.Visibility == Visibility.Visible)
            {
                menuBarShowHide.IsChecked = true;
            }
            else
            {
                menuBarShowHide.IsChecked = false;
            }

            if (statusBar.Visibility == Visibility.Visible)
            {
                statusBarShowHide.IsChecked = true;
            }
            else
            {
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
        private void open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Command implementation of Open menu item. Executing command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "JPEG/JPG - JPG Files|*.jpeg;*.jpg|BMP - Windows Bitmap|*.bmp|PNG - Portable Network Graphics|*.png";

                if (openFile.ShowDialog() == true)
                {
                    _inputFilename = openFile.FileName;
                    _bmpInput = new BitmapImage(new Uri(_inputFilename, UriKind.Absolute));
                    _bmpOutput = new System.Drawing.Bitmap(_inputFilename);
                    BmpUndoRedo = new System.Drawing.Bitmap(_inputFilename);
                    mainImage.Source = _bmpInput;

                    statusBar.Visibility = Visibility.Visible;

                    int bpp = System.Drawing.Image.GetPixelFormatSize(_bmpOutput.PixelFormat);
                    string resolution = _bmpOutput.Width + " x " + _bmpOutput.Height + " x " + bpp + " bpp";
                    string size = string.Empty;
                    FileInfo filesize = new FileInfo(_inputFilename);
                    switch (bpp)
                    {
                        case 8:
                            size = filesize.Length / 1000 + " KB" + " / " + (_bmpOutput.Width * _bmpOutput.Height * 1) / 1000000 + " MB";
                            break;
                        case 16:
                            size = filesize.Length / 1000 + " KB" + " / " + (_bmpOutput.Width * _bmpOutput.Height * 2) / 1000000 + " MB";
                            break;
                        case 24:
                            size = filesize.Length / 1000 + " KB" + " / " + (_bmpOutput.Width * _bmpOutput.Height * 3) / 1000000 + " MB";
                            break;
                        case 32:
                            size = filesize.Length / 1000 + " KB" + " / " + (_bmpOutput.Width * _bmpOutput.Height * 4) / 1000000 + " MB";
                            break;
                    }

                    imageResolution.Text = resolution;
                    imageSize.Text = size;
                    separator.Visibility = Visibility.Visible;

                    UndoStack.Clear();
                    UndoStack.Push(BmpUndoRedo);
                    RedoStack.Clear();
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (UriFormatException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void reopen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_inputFilename != string.Empty)
                {
                    Uri uri = new Uri(_inputFilename, UriKind.Absolute);
                    _bmpInput = new BitmapImage(uri);
                    _bmpOutput = new System.Drawing.Bitmap(_inputFilename);
                    BmpUndoRedo = new System.Drawing.Bitmap(_inputFilename);
                    mainImage.Source = _bmpInput;

                    UndoStack.Clear();
                    UndoStack.Push(BmpUndoRedo);
                    RedoStack.Clear();
                }
                else
                {
                    MessageBox.Show("Open image first!", "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (UriFormatException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Command implementation of Save menu item. Executing command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (_outputFilename != string.Empty)
                {

                    string extension = Path.GetExtension(_outputFilename);

                    ImageCodecInfo codec;
                    System.Drawing.Imaging.Encoder quality;
                    EncoderParameters encoderArray;
                    EncoderParameter encoder;

                    switch (extension.ToLower())
                    {
                        case ".jpg":
                            //MagickImage image = new MagickImage(bmpOutput);
                            //image.Quality = 100;
                            //image.Format = MagickFormat.Jpeg;
                            //image.CompressionMethod = CompressionMethod.JPEG;
                            //image.Write(OutputFilename);
                            codec = GetEncoder(ImageFormat.Jpeg);
                            quality = System.Drawing.Imaging.Encoder.Quality;
                            encoderArray = new EncoderParameters(1);
                            encoder = new EncoderParameter(quality, 85L);
                            encoderArray.Param[0] = encoder;

                            _bmpOutput.Save(_outputFilename, codec, encoderArray);

                            break;
                        case ".jpeg":
                            //MagickImage image = new MagickImage(bmpOutput);
                            //image.Quality = 100;
                            //image.Format = MagickFormat.Jpeg;
                            //image.CompressionMethod = CompressionMethod.JPEG;
                            //image.Write(OutputFilename);
                            codec = GetEncoder(ImageFormat.Jpeg);
                            quality = System.Drawing.Imaging.Encoder.Quality;
                            encoderArray = new EncoderParameters(1);
                            encoder = new EncoderParameter(quality, 85L);
                            encoderArray.Param[0] = encoder;

                            _bmpOutput.Save(_outputFilename, codec, encoderArray);

                            break;
                        case ".png":
                            //MagickImage image = new MagickImage(bmpOutput);
                            //image.Quality = 100;
                            //image.Format = MagickFormat.Jpeg;
                            //image.CompressionMethod = CompressionMethod.JPEG;
                            //image.Write(OutputFilename);
                            codec = GetEncoder(ImageFormat.Png);
                            quality = System.Drawing.Imaging.Encoder.Quality;
                            encoderArray = new EncoderParameters(1);
                            encoder = new EncoderParameter(quality, 85L);
                            encoderArray.Param[0] = encoder;

                            _bmpOutput.Save(_outputFilename, codec, encoderArray);

                            break;
                        case ".bmp":
                            //MagickImage image = new MagickImage(bmpOutput);
                            //image.Quality = 100;
                            //image.Format = MagickFormat.Jpeg;
                            //image.CompressionMethod = CompressionMethod.JPEG;
                            //image.Write(OutputFilename);
                            codec = GetEncoder(ImageFormat.Bmp);
                            quality = System.Drawing.Imaging.Encoder.Quality;
                            encoderArray = new EncoderParameters(1);
                            encoder = new EncoderParameter(quality, 85L);
                            encoderArray.Param[0] = encoder;

                            _bmpOutput.Save(_outputFilename, codec, encoderArray);

                            break;
                        default:
                            throw new ArgumentOutOfRangeException(extension);
                    }
                }
                else
                {
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.Filter = "JPEG/JPG - JPG Files|*.jpeg;*.jpg|BMP - Windows Bitmap|*.bmp|PNG - Portable Network Graphics|*.png";

                    if (saveFile.ShowDialog() == true)
                    {
                        _outputFilename = saveFile.FileName;
                        string extension = Path.GetExtension(_outputFilename);

                        ImageCodecInfo codec;
                        System.Drawing.Imaging.Encoder quality;
                        EncoderParameters encoderArray;
                        EncoderParameter encoder;

                        switch (extension.ToLower())
                        {
                            case ".jpg":
                                //MagickImage image = new MagickImage(bmpOutput);
                                //image.Quality = 100;
                                //image.Format = MagickFormat.Jpeg;
                                //image.CompressionMethod = CompressionMethod.JPEG;
                                //image.Write(OutputFilename);
                                codec = GetEncoder(ImageFormat.Jpeg);
                                quality = System.Drawing.Imaging.Encoder.Quality;
                                encoderArray = new EncoderParameters(1);
                                encoder = new EncoderParameter(quality, 85L);
                                encoderArray.Param[0] = encoder;

                                _bmpOutput.Save(_outputFilename, codec, encoderArray);

                                break;
                            case ".jpeg":
                                //MagickImage image = new MagickImage(bmpOutput);
                                //image.Quality = 100;
                                //image.Format = MagickFormat.Jpeg;
                                //image.CompressionMethod = CompressionMethod.JPEG;
                                //image.Write(OutputFilename);
                                codec = GetEncoder(ImageFormat.Jpeg);
                                quality = System.Drawing.Imaging.Encoder.Quality;
                                encoderArray = new EncoderParameters(1);
                                encoder = new EncoderParameter(quality, 85L);
                                encoderArray.Param[0] = encoder;

                                _bmpOutput.Save(_outputFilename, codec, encoderArray);

                                break;
                            case ".png":
                                //MagickImage image = new MagickImage(bmpOutput);
                                //image.Quality = 100;
                                //image.Format = MagickFormat.Jpeg;
                                //image.CompressionMethod = CompressionMethod.JPEG;
                                //image.Write(OutputFilename);
                                codec = GetEncoder(ImageFormat.Png);
                                quality = System.Drawing.Imaging.Encoder.Quality;
                                encoderArray = new EncoderParameters(1);
                                encoder = new EncoderParameter(quality, 85L);
                                encoderArray.Param[0] = encoder;

                                _bmpOutput.Save(_outputFilename, codec, encoderArray);

                                break;
                            case ".bmp":
                                //MagickImage image = new MagickImage(bmpOutput);
                                //image.Quality = 100;
                                //image.Format = MagickFormat.Jpeg;
                                //image.CompressionMethod = CompressionMethod.JPEG;
                                //image.Write(OutputFilename);
                                codec = GetEncoder(ImageFormat.Bmp);
                                quality = System.Drawing.Imaging.Encoder.Quality;
                                encoderArray = new EncoderParameters(1);
                                encoder = new EncoderParameter(quality, 85L);
                                encoderArray.Param[0] = encoder;

                                _bmpOutput.Save(_outputFilename, codec, encoderArray);

                                break;
                            default:
                                throw new ArgumentOutOfRangeException(extension);
                        }
                    }
                }

                NoChange = true;
            }
            catch (EncoderFallbackException ex)
            {
                MessageBox.Show(ex.Message, "EncoderFallbackException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentNullException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ExternalException ex)
            {
                MessageBox.Show(ex.Message, "ExternalException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void saveAs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "JPEG/JPG - JPG Files|*.jpeg;*.jpg|BMP - Windows Bitmap|*.bmp|PNG - Portable Network Graphics|*.png";

                if (saveFile.ShowDialog() == true)
                {
                    _outputFilename = saveFile.FileName;
                    string extension = Path.GetExtension(_outputFilename);

                    ImageCodecInfo codec;
                    System.Drawing.Imaging.Encoder quality;
                    EncoderParameters encoderArray;
                    EncoderParameter encoder;

                    switch (extension.ToLower())
                    {
                        case ".jpg":
                            //MagickImage image = new MagickImage(bmpOutput);
                            //image.Quality = 100;
                            //image.Format = MagickFormat.Jpeg;
                            //image.CompressionMethod = CompressionMethod.JPEG;
                            //image.Write(OutputFilename);
                            codec = GetEncoder(ImageFormat.Jpeg);
                            quality = System.Drawing.Imaging.Encoder.Quality;
                            encoderArray = new EncoderParameters(1);
                            encoder = new EncoderParameter(quality, 85L);
                            encoderArray.Param[0] = encoder;

                            _bmpOutput.Save(_outputFilename, codec, encoderArray);

                            break;
                        case ".jpeg":
                            //MagickImage image = new MagickImage(bmpOutput);
                            //image.Quality = 100;
                            //image.Format = MagickFormat.Jpeg;
                            //image.CompressionMethod = CompressionMethod.JPEG;
                            //image.Write(OutputFilename);
                            codec = GetEncoder(ImageFormat.Jpeg);
                            quality = System.Drawing.Imaging.Encoder.Quality;
                            encoderArray = new EncoderParameters(1);
                            encoder = new EncoderParameter(quality, 85L);
                            encoderArray.Param[0] = encoder;

                            _bmpOutput.Save(_outputFilename, codec, encoderArray);

                            break;
                        case ".png":
                            //MagickImage image = new MagickImage(bmpOutput);
                            //image.Quality = 100;
                            //image.Format = MagickFormat.Jpeg;
                            //image.CompressionMethod = CompressionMethod.JPEG;
                            //image.Write(OutputFilename);
                            codec = GetEncoder(ImageFormat.Png);
                            quality = System.Drawing.Imaging.Encoder.Quality;
                            encoderArray = new EncoderParameters(1);
                            encoder = new EncoderParameter(quality, 85L);
                            encoderArray.Param[0] = encoder;

                            _bmpOutput.Save(_outputFilename, codec, encoderArray);

                            break;
                        case ".bmp":
                            //MagickImage image = new MagickImage(bmpOutput);
                            //image.Quality = 100;
                            //image.Format = MagickFormat.Jpeg;
                            //image.CompressionMethod = CompressionMethod.JPEG;
                            //image.Write(OutputFilename);
                            codec = GetEncoder(ImageFormat.Bmp);
                            quality = System.Drawing.Imaging.Encoder.Quality;
                            encoderArray = new EncoderParameters(1);
                            encoder = new EncoderParameter(quality, 85L);
                            encoderArray.Param[0] = encoder;

                            _bmpOutput.Save(_outputFilename, codec, encoderArray);

                            break;
                        default:
                            throw new ArgumentOutOfRangeException(extension);
                    }
                }

                NoChange = true;
            }
            catch (EncoderFallbackException ex)
            {
                MessageBox.Show(ex.Message, "EncoderFallbackException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentNullException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ExternalException ex)
            {
                MessageBox.Show(ex.Message, "ExternalException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Command implementation of Undo menu item. Executing command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (UndoStack.Count <= 1)
            {
                undo.IsEnabled = false;
                return;
            }

            if (UndoStack.Count == 2)
            {
                undo.IsEnabled = false;
            }

            BmpUndoRedo = UndoStack.Pop();
            RedoStack.Push(BmpUndoRedo);
            redo.IsEnabled = true;
            _bmpOutput = UndoStack.Peek();
            BitmapToBitmapImage();
        }
        #endregion

        #region Redo
        /// <summary>
        /// Command implementation of Redo menu item. Check if the command can execute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Command implementation of Redo menu item. Executing command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (RedoStack.Count == 0)
            {
                redo.IsEnabled = false;
                return;
            }

            if (RedoStack.Count == 1)
            {
                redo.IsEnabled = false;
            }

            _bmpOutput = BmpUndoRedo = RedoStack.Pop();
            UndoStack.Push(BmpUndoRedo);
            BitmapToBitmapImage();

            if (UndoStack.Count > 1)
            {
                undo.IsEnabled = true;
            }
        }
        #endregion

        #region Menu bar
        /// <summary>
        /// Get and set the visibility of menu bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuBarShowHide_Click(object sender, RoutedEventArgs e)
        {
            if (menuBar.Visibility == Visibility.Collapsed)
            {
                menuBar.Visibility = Visibility.Visible;
                menuBarShowHide.IsChecked = true;
            }
            else
            {
                menuBar.Visibility = Visibility.Collapsed;
                menuBarShowHide.IsChecked = false;
            }
        }
        #endregion

        #region Status bar
        /// <summary>
        /// Get and set the visibility of status bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statusBarShowHide_Click(object sender, RoutedEventArgs e)
        {
            if (statusBar.Visibility == Visibility.Collapsed)
            {
                statusBar.Visibility = Visibility.Visible;
                statusBarShowHide.IsChecked = true;
            }
            else
            {
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
        private void Help_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Command implementation of Help menu item. Executing command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("This is the help window", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region About
        /// <summary>
        /// About window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void about_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ImageEdit v0.27.53 beta", "Version", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Information
        /// <summary>
        /// Command implementation of Information menu item. Check if the command can execute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Information_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Command implementation of Information menu item. Executing command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Information_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Information informationWindow = new Information(_inputFilename, _bmpOutput);
                informationWindow.Owner = this;
                informationWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void shiftBits_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                ShiftBits shiftBitsWindow = new ShiftBits(_bmpOutput, BmpUndoRedo);
                shiftBitsWindow.Owner = this;
                shiftBitsWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void threshold_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Threshold thresholdWindow = new Threshold(_bmpOutput, BmpUndoRedo);
                thresholdWindow.Owner = this;
                thresholdWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void autoThreshold_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                AutoThreshold autoThresholdWindow = new AutoThreshold(_bmpOutput, BmpUndoRedo);
                autoThresholdWindow.Owner = this;
                autoThresholdWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void negative_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Lock the bitmap's bits.  
                BitmapData bmpData = _bmpOutput.LockBits(new System.Drawing.Rectangle(0, 0, _bmpOutput.Width, _bmpOutput.Height), ImageLockMode.ReadWrite, _bmpOutput.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. 
                int bytes = Math.Abs(bmpData.Stride) * _bmpOutput.Height;
                byte[] rgbValues = new byte[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(ptr, rgbValues, 0, bytes);

                Stopwatch watch = Stopwatch.StartNew();

                for (int i = 0; i < _bmpOutput.Width; i++)
                {
                    for (int j = 0; j < _bmpOutput.Height; j++)
                    {
                        int index = (j * bmpData.Stride) + (i * 3);
                        rgbValues[index + 2] = (byte)(255 - rgbValues[index + 2]); // R
                        rgbValues[index + 1] = (byte)(255 - rgbValues[index + 1]); // G
                        rgbValues[index] = (byte)(255 - rgbValues[index]); // B
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
                    NoChange = false;
                    Action = ActionType.Negative;
                    BmpUndoRedo = _bmpOutput.Clone() as Bitmap;
                    UndoStack.Push(BmpUndoRedo);
                    undo.IsEnabled = true;
                    redo.IsEnabled = false;
                    RedoStack.Clear();
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void squareRoot_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Lock the bitmap's bits.  
                BitmapData bmpData = _bmpOutput.LockBits(new System.Drawing.Rectangle(0, 0, _bmpOutput.Width, _bmpOutput.Height), ImageLockMode.ReadWrite, _bmpOutput.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. 
                int bytes = Math.Abs(bmpData.Stride) * _bmpOutput.Height;
                byte[] rgbValues = new byte[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(ptr, rgbValues, 0, bytes);

                Stopwatch watch = Stopwatch.StartNew();

                for (int i = 0; i < _bmpOutput.Width; i++)
                {
                    for (int j = 0; j < _bmpOutput.Height; j++)
                    {
                        int index = (j * bmpData.Stride) + (i * 3);
                        rgbValues[index + 2] = (byte)Math.Sqrt(rgbValues[index + 2] * 255); // R
                        rgbValues[index + 1] = (byte)Math.Sqrt(rgbValues[index + 1] * 255); // G
                        rgbValues[index] = (byte)Math.Sqrt(rgbValues[index] * 255); // B
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
                    NoChange = false;
                    Action = ActionType.SquareRoot;
                    BmpUndoRedo = _bmpOutput.Clone() as System.Drawing.Bitmap;
                    UndoStack.Push(BmpUndoRedo);
                    undo.IsEnabled = true;
                    redo.IsEnabled = false;
                    RedoStack.Clear();
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void contrastEnhancement_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                ContrastEnhancement enhancement = new ContrastEnhancement(_bmpOutput, BmpUndoRedo);
                enhancement.Owner = this;
                enhancement.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void brightness_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Brightness brightness1 = new Brightness(_bmpOutput, BmpUndoRedo);
                brightness1.Owner = this;
                brightness1.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Contrast
        /// <summary>
        /// Contrast algorithm. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contrast_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Contrast contrastWindow = new Contrast(_bmpOutput, BmpUndoRedo);
                contrastWindow.Owner = this;
                contrastWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void histogram_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Histogram histogramWindow = new Histogram(_bmpOutput);
                histogramWindow.Owner = this;
                histogramWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void histogramEqualizationRGB_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                int b;
                int g;
                int r;
                double[] possibilityR = new double[256];
                double[] possibilityG = new double[256];
                double[] possibilityB = new double[256];
                int[] histogramR = new int[256];
                int[] histogramG = new int[256];
                int[] histogramB = new int[256];
                double[] histogramEqR = new double[256];
                double[] histogramEqG = new double[256];
                double[] histogramEqB = new double[256];

                // Lock the bitmap's bits.  
                BitmapData bmpData = _bmpOutput.LockBits(new System.Drawing.Rectangle(0, 0, _bmpOutput.Width, _bmpOutput.Height), ImageLockMode.ReadWrite, _bmpOutput.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. 
                int bytes = Math.Abs(bmpData.Stride) * _bmpOutput.Height;
                byte[] rgbValues = new byte[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(ptr, rgbValues, 0, bytes);

                Stopwatch watch = Stopwatch.StartNew();

                for (int i = 0; i < 256; i++)
                {
                    histogramR[i] = 0;
                    histogramG[i] = 0;
                    histogramB[i] = 0;
                }

                for (int i = 0; i < _bmpOutput.Width; i++)
                {
                    for (int j = 0; j < _bmpOutput.Height; j++)
                    {
                        int index = (j * bmpData.Stride) + (i * 3);


                        b = rgbValues[index + 2];
                        histogramB[b]++;
                        g = rgbValues[index + 1];
                        histogramG[g]++;
                        r = rgbValues[index];
                        histogramR[r]++;
                    }
                }

                for (int i = 0; i < 256; i++)
                {
                    possibilityB[i] = histogramB[i] / (double)(_bmpOutput.Width * _bmpOutput.Height);
                    possibilityG[i] = histogramG[i] / (double)(_bmpOutput.Width * _bmpOutput.Height);
                    possibilityR[i] = histogramR[i] / (double)(_bmpOutput.Width * _bmpOutput.Height);
                }

                histogramEqB[0] = possibilityB[0];
                histogramEqG[0] = possibilityG[0];
                histogramEqR[0] = possibilityR[0];
                for (int i = 1; i < 256; i++)
                {
                    histogramEqB[i] = histogramEqB[i - 1] + possibilityB[i];
                    histogramEqG[i] = histogramEqG[i - 1] + possibilityG[i];
                    histogramEqR[i] = histogramEqR[i - 1] + possibilityR[i];
                }

                for (int i = 0; i < _bmpOutput.Width; i++)
                {
                    for (int j = 0; j < _bmpOutput.Height; j++)
                    {
                        int index = (j * bmpData.Stride) + (i * 3);

                        b = rgbValues[index + 2];
                        b = (int)Math.Round(histogramEqB[b] * 255);
                        g = rgbValues[index + 1];
                        g = (int)Math.Round(histogramEqG[g] * 255);
                        r = rgbValues[index];
                        r = (int)Math.Round(histogramEqR[r] * 255);

                        if (b > 255)
                        {
                            b = 255;
                        }
                        else if (b < 0)
                        {
                            b = 0;
                        }

                        if (g > 255)
                        {
                            g = 255;
                        }
                        else if (g < 0)
                        {
                            g = 0;
                        }

                        if (r > 255)
                        {
                            r = 255;
                        }
                        else if (r < 0)
                        {
                            r = 0;
                        }

                        rgbValues[index + 2] = (byte)b;
                        rgbValues[index + 1] = (byte)g;
                        rgbValues[index] = (byte)r;
                    }
                }

                watch.Stop();
                TimeSpan elapsedTime = watch.Elapsed;

                // Copy the RGB values back to the bitmap
                Marshal.Copy(rgbValues, 0, ptr, bytes);

                // Unlock the bits.
                _bmpOutput.UnlockBits(bmpData);

                // Convert Bitmap to BitmapImage
                MemoryStream str = new MemoryStream();
                _bmpOutput.Save(str, ImageFormat.Bmp);
                str.Seek(0, SeekOrigin.Begin);
                BitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                mainImage.Source = bdc.Frames[0];

                string messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
                MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    NoChange = false;
                    Action = ActionType.ImageEqualizationRGB;
                    BmpUndoRedo = _bmpOutput.Clone() as System.Drawing.Bitmap;
                    UndoStack.Push(BmpUndoRedo);
                    undo.IsEnabled = true;
                    redo.IsEnabled = false;
                    RedoStack.Clear();
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void histogramEqualizationHSV_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                int i;
                int j;
                int k = 0;
                double max = 0.0;
                double min = 0.0;
                double chroma = 0.0;
                double c = 0.0;
                double x = 0.0;
                double m = 0.0;
                int[] histogramV = new int[256];
                double[] sumHistogramEqualizationV = new double[256];
                double[] sumHistogramV = new double[256];

                // Lock the bitmap's bits.  
                BitmapData bmpData = _bmpOutput.LockBits(new System.Drawing.Rectangle(0, 0, _bmpOutput.Width, _bmpOutput.Height), ImageLockMode.ReadWrite, _bmpOutput.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. 
                int bytes = Math.Abs(bmpData.Stride) * _bmpOutput.Height;
                byte[] rgbValues = new byte[bytes];
                double[] red = new double[bytes];
                double[] green = new double[bytes];
                double[] blue = new double[bytes];
                double[] hue = new double[bytes];
                double[] value = new double[bytes];
                double[] saturation = new double[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(ptr, rgbValues, 0, bytes);

                Stopwatch watch = Stopwatch.StartNew();

                for (i = 0; i < 256; i++)
                {
                    histogramV[i] = 0;
                    sumHistogramEqualizationV[i] = 0.0;
                }

                for (i = 0; i < _bmpOutput.Width; i++)
                {
                    for (j = 0; j < _bmpOutput.Height; j++)
                    {
                        int index = (bmpData.Stride * j) + (i * 3);

                        blue[index] = rgbValues[index];
                        blue[index] = blue[index] / 255.0;
                        green[index + 1] = rgbValues[index + 1];
                        green[index + 1] = green[index + 1] / 255.0;
                        red[index + 2] = rgbValues[index + 2];
                        red[index + 2] = red[index + 2] / 255.0;


                        min = Math.Min(red[index + 2], Math.Min(green[index + 1], blue[index]));
                        max = Math.Max(red[index + 2], Math.Max(green[index + 1], blue[index]));
                        chroma = max - min;
                        hue[index] = 0.0;
                        saturation[index + 1] = 0.0;
                        if (chroma != 0.0)
                        {
                            if (Math.Abs(red[index + 2] - max) < 0.00001)
                            {
                                hue[index] = ((green[index + 1] - blue[index]) / chroma);
                                hue[index] = hue[index] % 6.0;
                            }
                            else if (Math.Abs(green[index + 1] - max) < 0.00001)
                            {
                                hue[index] = ((blue[index] - red[index + 2]) / chroma) + 2;
                            }
                            else
                            {
                                hue[index] = ((red[index + 2] - green[index + 1]) / chroma) + 4;
                            }

                            hue[index] = hue[index] * 60.0;
                            if (hue[index] < 0.0)
                            {
                                hue[index] = hue[index] + 360.0;
                            }
                            saturation[index + 1] = chroma / max;
                        }
                        value[index + 2] = max;

                        value[index + 2] = value[index + 2] * 255.0;

                        if (value[index + 2] > 255.0)
                        {
                            value[index + 2] = 255.0;
                        }
                        if (value[index + 2] < 0.0)
                        {
                            value[index + 2] = 0.0;
                        }

                        k = (int)value[index + 2];
                        histogramV[k]++;
                    }
                }

                for (i = 0; i < 256; i++)
                {
                    sumHistogramEqualizationV[i] = histogramV[i] / (double)(_bmpOutput.Width * _bmpOutput.Height);
                }

                sumHistogramV[0] = sumHistogramEqualizationV[0];
                for (i = 1; i < 256; i++)
                {
                    sumHistogramV[i] = sumHistogramV[i - 1] + sumHistogramEqualizationV[i];
                }

                for (i = 0; i < _bmpOutput.Width; i++)
                {
                    for (j = 0; j < _bmpOutput.Height; j++)
                    {
                        int index = (bmpData.Stride * j) + (i * 3);

                        k = (int)value[index + 2];
                        value[index + 2] = (byte)Math.Round(sumHistogramV[k] * 255.0);
                        value[index + 2] = value[index + 2] / 255;

                        c = value[index + 2] * saturation[index + 1];
                        hue[index] = hue[index] / 60.0;
                        hue[index] = hue[index] % 2;
                        x = c * (1.0 - Math.Abs(hue[index] - 1.0));
                        m = value[index + 2] - c;

                        if (hue[index] >= 0.0 && hue[index] < 60.0)
                        {
                            red[index + 2] = c;
                            green[index + 1] = x;
                            blue[index] = 0;
                        }
                        else if (hue[index] >= 60.0 && hue[index] < 120.0)
                        {
                            red[index + 2] = x;
                            green[index + 1] = c;
                            blue[index] = 0;
                        }
                        else if (hue[index] >= 120.0 && hue[index] < 180.0)
                        {
                            red[index + 2] = 0;
                            green[index + 1] = c;
                            blue[index] = x;
                        }
                        else if (hue[index] >= 180.0 && hue[index] < 240.0)
                        {
                            red[index + 2] = 0;
                            green[index + 1] = x;
                            blue[index] = c;
                        }
                        else if (hue[index] >= 240.0 && hue[index] < 300.0)
                        {
                            red[index + 2] = x;
                            green[index + 1] = 0;
                            blue[index] = c;
                        }
                        else if (hue[index] >= 300.0 && hue[index] < 360.0)
                        {
                            red[index + 2] = c;
                            green[index + 1] = 0;
                            blue[index] = x;
                        }

                        red[index + 2] = red[index + 2] + m;
                        green[index + 1] = green[index + 1] + m;
                        blue[index] = blue[index] + m;

                        red[index + 2] = red[index + 2] * 255.0;
                        green[index + 1] = green[index + 1] * 255.0;
                        blue[index] = blue[index] * 255.0;

                        if (red[index + 2] > 255.0)
                        {
                            red[index + 2] = 255.0;
                        }

                        if (red[index + 2] < 0.0)
                        {
                            red[index + 2] = 0.0;
                        }

                        if (green[index + 1] > 255.0)
                        {
                            green[index + 1] = 255.0;
                        }

                        if (green[index + 1] < 0.0)
                        {
                            green[index + 1] = 0.0;
                        }

                        if (blue[index] > 255.0)
                        {
                            blue[index] = 255.0;
                        }

                        if (blue[index] < 0.0)
                        {
                            blue[index] = 0.0;
                        }

                        rgbValues[index + 2] = (byte)red[index + 2];
                        rgbValues[index + 1] = (byte)green[index + 1];
                        rgbValues[index] = (byte)blue[index];
                    }
                }

                watch.Stop();
                TimeSpan elapsedTime = watch.Elapsed;

                // Copy the RGB values back to the bitmap
                Marshal.Copy(rgbValues, 0, ptr, bytes);

                // Unlock the bits.
                _bmpOutput.UnlockBits(bmpData);

                // Convert Bitmap to BitmapImage
                MemoryStream str = new MemoryStream();
                _bmpOutput.Save(str, ImageFormat.Bmp);
                str.Seek(0, SeekOrigin.Begin);
                BitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                mainImage.Source = bdc.Frames[0];

                string messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
                MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    NoChange = false;
                    Action = ActionType.ImageEqualizationHSV;
                    BmpUndoRedo = _bmpOutput.Clone() as System.Drawing.Bitmap;
                    UndoStack.Push(BmpUndoRedo);
                    undo.IsEnabled = true;
                    redo.IsEnabled = false;
                    RedoStack.Clear();
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void histogramEqualizationYUV_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                int i;
                int j;
                int k = 0;
                int[] histogramY = new int[256];
                double[] sumHistogramEqualizationY = new double[256];
                double[] sumHistogramY = new double[256];

                // Lock the bitmap's bits.  
                BitmapData bmpData = _bmpOutput.LockBits(new System.Drawing.Rectangle(0, 0, _bmpOutput.Width, _bmpOutput.Height), ImageLockMode.ReadWrite, _bmpOutput.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. 
                int bytes = Math.Abs(bmpData.Stride) * _bmpOutput.Height;
                byte[] rgbValues = new byte[bytes];
                double[] red = new double[bytes];
                double[] green = new double[bytes];
                double[] blue = new double[bytes];
                double[] luminanceY = new double[bytes];
                double[] chrominanceU = new double[bytes];
                double[] chrominanceV = new double[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(ptr, rgbValues, 0, bytes);

                Stopwatch watch = Stopwatch.StartNew();

                for (i = 0; i < 256; i++)
                {
                    histogramY[i] = 0;
                }

                for (i = 0; i < _bmpOutput.Width; i++)
                {
                    for (j = 0; j < _bmpOutput.Height; j++)
                    {
                        int index = (bmpData.Stride * j) + (i * 3);

                        blue[index] = rgbValues[index];
                        blue[index] = blue[index] / 255.0;
                        green[index + 1] = rgbValues[index + 1];
                        green[index + 1] = green[index + 1] / 255.0;
                        red[index + 2] = rgbValues[index + 2];
                        red[index + 2] = red[index + 2] / 255.0;

                        luminanceY[index] = (0.299 * red[index + 2]) + (0.587 * green[index + 1]) + (0.114 * blue[index]);
                        chrominanceU[index + 1] = (-0.14713 * red[index + 2]) - (0.28886 * green[index + 1]) + (0.436 * blue[index]);
                        chrominanceV[index + 2] = (0.615 * red[index + 2]) - (0.51499 * green[index + 1]) - (0.10001 * blue[index]);

                        luminanceY[index] = luminanceY[index] * 255.0;
                        if (luminanceY[index] > 255.0)
                        {
                            luminanceY[index] = 255.0;
                        }

                        if (luminanceY[index] < 0.0)
                        {
                            luminanceY[index] = 0.0;
                        }

                        k = (int)luminanceY[index];
                        histogramY[k]++;
                    }
                }

                for (i = 0; i < 256; i++)
                {
                    sumHistogramEqualizationY[i] = 0.0;
                }

                for (i = 0; i < 256; i++)
                {
                    sumHistogramEqualizationY[i] = histogramY[i] / (double)(_bmpOutput.Width * _bmpOutput.Height);
                }

                sumHistogramY[0] = sumHistogramEqualizationY[0];
                for (i = 1; i < 256; i++)
                {
                    sumHistogramY[i] = sumHistogramY[i - 1] + sumHistogramEqualizationY[i];
                }

                for (i = 0; i < _bmpOutput.Width; i++)
                {
                    for (j = 0; j < _bmpOutput.Height; j++)
                    {
                        int index = (bmpData.Stride * j) + (i * 3);

                        k = (int)luminanceY[index];
                        luminanceY[index] = (byte)Math.Round(sumHistogramY[k] * 255.0);
                        luminanceY[index] = luminanceY[index] / 255;

                        red[index + 2] = luminanceY[index] + (0.0 * chrominanceU[index + 1]) + (1.13983 * chrominanceV[index + 2]);
                        green[index + 1] = luminanceY[index] + (-0.39465 * chrominanceU[index + 1]) + (-0.58060 * chrominanceV[index + 2]);
                        blue[index] = luminanceY[index] + (2.03211 * chrominanceU[index + 1]) + (0.0 * chrominanceV[index + 2]);

                        red[index + 2] = red[index + 2] * 255.0;
                        green[index + 1] = green[index + 1] * 255.0;
                        blue[index] = blue[index] * 255.0;

                        if (red[index + 2] > 255.0)
                        {
                            red[index + 2] = 255.0;
                        }

                        if (red[index + 2] < 0.0)
                        {
                            red[index + 2] = 0.0;
                        }

                        if (green[index + 1] > 255.0)
                        {
                            green[index + 1] = 255.0;
                        }

                        if (green[index + 1] < 0.0)
                        {
                            green[index + 1] = 0.0;
                        }

                        if (blue[index] > 255.0)
                        {
                            blue[index] = 255.0;
                        }

                        if (blue[index] < 0.0)
                        {
                            blue[index] = 0.0;
                        }

                        rgbValues[index + 2] = (byte)red[index + 2];
                        rgbValues[index + 1] = (byte)green[index + 1];
                        rgbValues[index] = (byte)blue[index];
                    }
                }

                watch.Stop();
                TimeSpan elapsedTime = watch.Elapsed;

                // Copy the RGB values back to the bitmap
                Marshal.Copy(rgbValues, 0, ptr, bytes);

                // Unlock the bits.
                _bmpOutput.UnlockBits(bmpData);

                // Convert Bitmap to BitmapImage
                MemoryStream str = new MemoryStream();
                _bmpOutput.Save(str, ImageFormat.Bmp);
                str.Seek(0, SeekOrigin.Begin);
                BitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                mainImage.Source = bdc.Frames[0];

                string messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
                MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    NoChange = false;
                    Action = ActionType.ImageEqualizationYUV;
                    BmpUndoRedo = _bmpOutput.Clone() as System.Drawing.Bitmap;
                    UndoStack.Push(BmpUndoRedo);
                    undo.IsEnabled = true;
                    redo.IsEnabled = false;
                    RedoStack.Clear();
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Image Summarization
        /// <summary>
        /// Image Summarization algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageSummarization_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                int b;
                int g;
                int r;

                // Lock the bitmap's bits.  
                BitmapData bmpData = _bmpOutput.LockBits(new System.Drawing.Rectangle(0, 0, _bmpOutput.Width, _bmpOutput.Height), ImageLockMode.ReadWrite, _bmpOutput.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. 
                int bytes = Math.Abs(bmpData.Stride) * _bmpOutput.Height;
                byte[] rgbValues = new byte[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(ptr, rgbValues, 0, bytes);

                Stopwatch watch = Stopwatch.StartNew();

                for (int i = 0; i < _bmpOutput.Width; i++)
                {
                    for (int j = 0; j < _bmpOutput.Height; j++)
                    {
                        int index = (j * bmpData.Stride) + (i * 3);
                        r = rgbValues[index + 2] + rgbValues[index + 2]; // R
                        g = rgbValues[index + 1] + rgbValues[index + 1]; // G
                        b = rgbValues[index] + rgbValues[index]; // B

                        if (r > 255)
                        {
                            r = 255;
                        }

                        if (g > 255)
                        {
                            g = 255;
                        }

                        if (b > 255)
                        {
                            b = 255;
                        }

                        rgbValues[index + 2] = (byte)r; // R
                        rgbValues[index + 1] = (byte)g; // G
                        rgbValues[index] = (byte)b; // B
                    }
                }

                watch.Stop();
                TimeSpan elapsedTime = watch.Elapsed;

                // Copy the RGB values back to the bitmap
                Marshal.Copy(rgbValues, 0, ptr, bytes);

                // Unlock the bits.
                _bmpOutput.UnlockBits(bmpData);

                // Convert Bitmap to BitmapImage
                MemoryStream str = new MemoryStream();
                _bmpOutput.Save(str, ImageFormat.Bmp);
                str.Seek(0, SeekOrigin.Begin);
                BitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                mainImage.Source = bdc.Frames[0];

                string messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
                MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    NoChange = false;
                    Action = ActionType.ImageSummarization;
                    BmpUndoRedo = _bmpOutput.Clone() as System.Drawing.Bitmap;
                    UndoStack.Push(BmpUndoRedo);
                    undo.IsEnabled = true;
                    redo.IsEnabled = false;
                    RedoStack.Clear();
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Image Subtraction
        /// <summary>
        /// Image Subtraction algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageSubtraction_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                int b;
                int g;
                int r;

                // Lock the bitmap's bits.  
                BitmapData bmpData = _bmpOutput.LockBits(new System.Drawing.Rectangle(0, 0, _bmpOutput.Width, _bmpOutput.Height), ImageLockMode.ReadWrite, _bmpOutput.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. 
                int bytes = Math.Abs(bmpData.Stride) * _bmpOutput.Height;
                byte[] rgbValues = new byte[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(ptr, rgbValues, 0, bytes);

                Stopwatch watch = Stopwatch.StartNew();

                for (int i = 0; i < _bmpOutput.Width; i++)
                {
                    for (int j = 0; j < _bmpOutput.Height; j++)
                    {
                        int index = (j * bmpData.Stride) + (i * 3);

                        r = rgbValues[index + 2] - rgbValues[index + 2]; // R
                        g = rgbValues[index + 1] - rgbValues[index + 1]; // G
                        b = rgbValues[index] - rgbValues[index]; // B

                        rgbValues[index + 2] = (byte)r; // R
                        rgbValues[index + 1] = (byte)g; // G
                        rgbValues[index] = (byte)b; // B
                    }
                }

                watch.Stop();
                TimeSpan elapsedTime = watch.Elapsed;

                // Copy the RGB values back to the bitmap
                Marshal.Copy(rgbValues, 0, ptr, bytes);

                // Unlock the bits.
                _bmpOutput.UnlockBits(bmpData);

                // Convert Bitmap to BitmapImage
                MemoryStream str = new MemoryStream();
                _bmpOutput.Save(str, ImageFormat.Bmp);
                str.Seek(0, SeekOrigin.Begin);
                BitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                mainImage.Source = bdc.Frames[0];

                string messageOperation = "Done!" + Environment.NewLine + Environment.NewLine + "Elapsed time (HH:MM:SS.MS): " + elapsedTime.ToString();
                MessageBoxResult result = MessageBox.Show(messageOperation, "Elapsed time", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    NoChange = false;
                    Action = ActionType.ImageSubtraction;
                    BmpUndoRedo = _bmpOutput.Clone() as System.Drawing.Bitmap;
                    UndoStack.Push(BmpUndoRedo);
                    undo.IsEnabled = true;
                    redo.IsEnabled = false;
                    RedoStack.Clear();
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Edge detection (Sobel)
        /// <summary>
        /// Edge Detection algorithm. Here we create a new window from where we implement the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sobel_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Sobel sobelWindow = new Sobel(_bmpOutput, BmpUndoRedo);
                sobelWindow.Owner = this;
                sobelWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void gaussianBlur_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                GaussianBlur gaussianBlurWindow = new GaussianBlur(_bmpOutput, BmpUndoRedo);
                gaussianBlurWindow.Owner = this;
                gaussianBlurWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void sharpen_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Sharpen sharpenWindow = new Sharpen(_bmpOutput, BmpUndoRedo);
                sharpenWindow.Owner = this;
                sharpenWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void noiseColor_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                SaltPepperNoiseColor saltPepperNoiseColorWindow = new SaltPepperNoiseColor(_bmpOutput, BmpUndoRedo);
                saltPepperNoiseColorWindow.Owner = this;
                saltPepperNoiseColorWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void noiseBW_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                SaltPepperNoiseBW saltPepperNoiseBwWindow = new SaltPepperNoiseBW(_bmpOutput, BmpUndoRedo);
                saltPepperNoiseBwWindow.Owner = this;
                saltPepperNoiseBwWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void noiseReductionMean_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                NoiseReductionMean noiseReductionMeanWindow = new NoiseReductionMean(_bmpOutput, BmpUndoRedo);
                noiseReductionMeanWindow.Owner = this;
                noiseReductionMeanWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
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
        private void noiseReductionMedian_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFilename == string.Empty || _bmpOutput == null)
            {
                MessageBox.Show("Open image first!", "ArgumentsNull", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                NoiseReductionMedian noiseReductionMedianWindow = new NoiseReductionMedian(_bmpOutput, BmpUndoRedo);
                noiseReductionMedianWindow.Owner = this;
                noiseReductionMedianWindow.Show();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "FileNotFoundException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "IndexOutOfRangeException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Bitmap To BitmapImage
        /// <summary>
        /// <c>Bitmap</c> to <c>BitmpaImage</c> conversion method in order to show the edited image at the main window.
        /// </summary>
        public void BitmapToBitmapImage()
        {
            // Convert Bitmap to BitmapImage
            MemoryStream str = new MemoryStream();
            _bmpOutput.Save(str, ImageFormat.Bmp);
            str.Seek(0, SeekOrigin.Begin);
            BmpBitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

            mainImage.Source = bdc.Frames[0];
        }
        #endregion

        #region GetEncoder
        /// <summary>
        /// Get the encoder info in order to use it at <c>Save</c> or <c>Save as...</c> method.
        /// </summary>
        /// <param name="format">Format of the image</param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        #endregion

        #region Window_Closing
        /// <summary>
        /// Event that fires when we are trying to close the window.
        /// It is used to check if there are any unsaved changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (NoChange == false)
            {
                MessageBoxResult result = MessageBox.Show("Quit without saving?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
        #endregion
    }
}