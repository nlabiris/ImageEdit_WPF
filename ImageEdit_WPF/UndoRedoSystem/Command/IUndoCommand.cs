using System.Windows.Input;

namespace ImageEdit_WPF.UndoRedoSystem.Command {
    public interface IUndoCommand : ICommand {
        void Undo();
    }
}
