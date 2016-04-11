using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ImageEdit_WPF.UndoRedoSystem.Command {
    public class RedoCommand : ICommand {
        public event EventHandler CanExecuteChanged;

        public RedoCommand() {
            CommandStateManager.Instance.PropertyChanged += new PropertyChangedEventHandler(Instance_PropertyChanged);
        }

        void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            CanExecuteChanged(this, e);
        }

        public bool CanExecute(object parameter) {
            return CommandStateManager.Instance.CanRedo;
        }

        public void Execute(object parameter) {
            CommandStateManager.Instance.Redo();
        }
    }
}
