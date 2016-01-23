using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace ImageEdit_WPF.HelperClasses {
    public class ViewModel : INotifyPropertyChanged {
        /// <summary>
        /// Image that binds to the GUI.
        /// </summary>
        private ImageSource m_bitmapBind = null;

        /// <summary>
        /// Progress bar at main window.
        /// </summary>
        private ProgressBar m_progress = null;

        /// <summary>
        /// Points that represent the values of the histogram.
        /// </summary>
        private PointCollection m_histogramPoints = null;

        public ImageSource M_bitmapBind {
            get { return m_bitmapBind; }
            set {
                m_bitmapBind = value;
                OnPropertyChanged("M_bitmapBind");
            }
        }

        public ProgressBar M_progress {
            get { return m_progress; }
            set {
                m_progress = value;
                OnPropertyChanged("M_progress");
            }
        }

        /// <summary>
        /// Get or set histogram's points. Checking if we have a different set of points to show.
        /// </summary>
        public PointCollection M_histogramPoints {
            get { return m_histogramPoints; }
            set {
                m_histogramPoints = value;
                OnPropertyChanged("M_histogramPoints");
            }
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