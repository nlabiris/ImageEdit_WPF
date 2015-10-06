using System.Windows.Input;

namespace ImageEdit_WPF
{
    public class RedoCommand
    {
        private static readonly RoutedUICommand _redo;

        public static RoutedUICommand Redo
        {
            get
            {
                return _redo;
            }
        }

        static RedoCommand()
        {
            InputGestureCollection gestures = new InputGestureCollection();
            gestures.Add(new KeyGesture(Key.Y, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+Y"));
            _redo = new RoutedUICommand("Redo", "Redo", typeof (RedoCommand), gestures);
        }
    }
}
