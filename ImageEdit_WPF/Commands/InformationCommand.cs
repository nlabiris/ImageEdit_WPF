using System.Windows.Input;

namespace ImageEdit_WPF.Commands
{
    public class InformationCommand
    {
        private static readonly RoutedUICommand _information;

        public static RoutedUICommand Information
        {
            get
            {
                return _information;
            }
        }

        static InformationCommand()
        {
            InputGestureCollection gestures = new InputGestureCollection();
            gestures.Add(new KeyGesture(Key.I, ModifierKeys.Control, "Ctrl+I"));
            _information = new RoutedUICommand("Information", "Information", typeof (InformationCommand), gestures);
        }
    }
}
