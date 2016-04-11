using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ImageEdit_WPF.UndoRedoSystem.Command {
    public class UndoCommand : ICommand {
        public event EventHandler CanExecuteChanged;

        public UndoCommand() {
            CommandStateManager.Instance.PropertyChanged += new PropertyChangedEventHandler(Instance_PropertyChanged);
        }

        void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            CanExecuteChanged(this, e);
        }

        public bool CanExecute(object parameter) {
            return CommandStateManager.Instance.CanUndo;
        }

        public void Execute(object parameter) {
            CommandStateManager.Instance.Undo();
        }
    }
}
