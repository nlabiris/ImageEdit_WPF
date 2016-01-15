using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media;

namespace ImageEdit_WPF.HelperClasses {
    /// <summary>
    /// <c>ActionType</c> enumeration is used at the Undo/Redo sytem (not now).
    /// </summary>
    [Obsolete]
    public enum ActionType {
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

    public class ImageEditData : INotifyPropertyChanged {
        #region Private fields
        /// <summary>
        /// Input filename of the image.
        /// </summary>
        private string m_inputFilename = string.Empty;

        /// <summary>
        /// Output filename of the image.
        /// </summary>
        private string m_outputFilename = string.Empty;

        /// <summary>
        /// Check if the image has been modified
        /// </summary>
        private bool m_noChange = true;

        /// <summary>
        /// Image that binds to the GUI.
        /// </summary>
        private ImageSource m_bitmapBind = null;

        /// <summary>
        /// Output image that carries all changes until saved.
        /// </summary>
        private Bitmap m_bitmap = null;

        /// <summary>
        /// Stack that contains all undone actions.
        /// </summary>
        private Stack<Bitmap> m_undoStack = new Stack<Bitmap>();

        /// <summary>
        /// Stack that contains actions to be redone.
        /// </summary>
        private Stack<Bitmap> m_redoStack = new Stack<Bitmap>();

        /// <summary>
        /// Type of action (which algorithm used).
        /// </summary>
        /// <remarks>This field is not used.</remarks>
        [Obsolete] private ActionType m_action;

        /// <summary>
        /// Image used at the Undo/Redo system.
        /// </summary>
        private Bitmap m_bmpUndoRedo = null;
        #endregion

        #region Public properties
        public string M_inputFilename {
            get { return m_inputFilename; }
            set { m_inputFilename = value; }
        }

        public string M_outputFilename {
            get { return m_outputFilename; }
            set { m_outputFilename = value; }
        }

        public bool M_noChange {
            get { return m_noChange; }
            set { m_noChange = value; }
        }

        public ImageSource M_bitmapBind {
            get { return m_bitmapBind; }
            set {
                m_bitmapBind = value;
                OnPropertyChanged("M_bitmapBind");
            }
        }

        public Bitmap M_bitmap {
            get { return m_bitmap; }
            set { m_bitmap = value; }
        }

        public Stack<Bitmap> M_undoStack {
            get { return m_undoStack; }
            set { m_undoStack = value; }
        }

        public Stack<Bitmap> M_redoStack {
            get { return m_redoStack; }
            set { m_redoStack = value; }
        }

        [Obsolete]
        public ActionType M_action {
            get { return m_action; }
            set { m_action = value; }
        }

        public Bitmap M_bmpUndoRedo {
            get { return m_bmpUndoRedo; }
            set { m_bmpUndoRedo = value; }
        }
        #endregion

        public static void PreEdit(ImageEditData data) {
        }

        public static void PostEdit(ImageEditData data) {
        }


        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) {
            var handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}