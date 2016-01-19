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

using System.Collections.Generic;
using System.Drawing;

namespace ImageEdit_WPF.HelperClasses {
    /// <summary>
    /// <c>ActionType</c> enumeration is used at the Undo/Redo sytem (not now).
    /// </summary>
    public enum ActionType {
        Unknown = -1,
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
        ImageEqualizationRGB = 10,
        ImageEqualizationHSV = 11,
        ImageEqualizationYUV = 12,
        GaussianBlur = 13,
        Sharpen = 14,
        SobelEdgeDetection = 15,
        SaltPepperNoiseBW = 16,
        SaltPepperNoiseColor = 17,
        NoiseReductionMean = 18,
        NoiseReductionMedian = 19
    }

    public class ImageData {
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
        /// Check if the image has been modified.
        /// </summary>
        private bool m_noChange = true;

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
        private ActionType m_action;

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

        public Bitmap M_bitmap {
            get { return m_bitmap; }
            set { m_bitmap = value; }
        }

        public int M_height {
            get { return m_bitmap.Height; }
        }

        public int M_width {
            get { return m_bitmap.Width; }
        }

        public Stack<Bitmap> M_undoStack {
            get { return m_undoStack; }
            set { m_undoStack = value; }
        }

        public Stack<Bitmap> M_redoStack {
            get { return m_redoStack; }
            set { m_redoStack = value; }
        }

        public ActionType M_action {
            get { return m_action; }
            set { m_action = value; }
        }

        public Bitmap M_bmpUndoRedo {
            get { return m_bmpUndoRedo; }
            set { m_bmpUndoRedo = value; }
        }
        #endregion
    }
}