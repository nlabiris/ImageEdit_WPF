using System.Windows.Input;

namespace ImageEdit_WPF.Commands {
    public class UndoCommand {
        private static readonly RoutedUICommand _undo;

        public static RoutedUICommand Undo {
            get { return _undo; }
        }

        static UndoCommand() {
            InputGestureCollection gestures = new InputGestureCollection();
            gestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+Z"));
            _undo = new RoutedUICommand("Undo", "Undo", typeof (HelpCommand), gestures);
        }
    }
}