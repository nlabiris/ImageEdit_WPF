using System;
using System.Collections.Generic;
using System.ComponentModel;
using ImageEdit_WPF.UndoRedoSystem.Command;

namespace ImageEdit_WPF.UndoRedoSystem {
    public class CommandStateManager : INotifyPropertyChanged {
        private static readonly Lazy<CommandStateManager> _instance = new Lazy<CommandStateManager>(() => new CommandStateManager());

        private Stack<IUndoCommand> _undos = new Stack<IUndoCommand>();
        private Stack<IUndoCommand> _redos = new Stack<IUndoCommand>();

        public static CommandStateManager Instance {
            get { return _instance.Value; }
        }

        public bool CanUndo {
            get { return _undos.Count > 0; }
        }

        public bool CanRedo {
            get { return _redos.Count > 0; }
        }

        private CommandStateManager() {
        }

        public void Executed(IUndoCommand command) {
            _undos.Push(command);
            OnNotifyPropertyChanged("CanUndo");
        }

        public void Undo() {
            if (CanUndo) {
                IUndoCommand command = _undos.Pop();
                _redos.Push(command);
                command.Undo();
                OnNotifyPropertyChanged("CanRedo");
            }
        }

        public void Redo() {
            if (CanRedo) {
                IUndoCommand command = _redos.Pop();
                _undos.Push(command);
                command.Execute(null);
                OnNotifyPropertyChanged("CanUndo");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnNotifyPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
