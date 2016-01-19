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