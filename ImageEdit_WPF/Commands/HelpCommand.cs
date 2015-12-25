using System.Windows.Input;

namespace ImageEdit_WPF.Commands {
    public class HelpCommand {
        private static readonly RoutedUICommand _help;

        public static RoutedUICommand Help {
            get { return _help; }
        }

        static HelpCommand() {
            InputGestureCollection gestures = new InputGestureCollection();
            gestures.Add(new KeyGesture(Key.F1, ModifierKeys.None, "F1"));
            _help = new RoutedUICommand("Help", "Help", typeof (HelpCommand), gestures);
        }
    }
}