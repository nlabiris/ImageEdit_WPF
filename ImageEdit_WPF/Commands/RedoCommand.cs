/*
Basic image processing software
<https://github.com/nlabiris/ImageEdit_WPF>

Copyright (C) 2015  Nikos Labiris

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.Windows.Input;

namespace ImageEdit_WPF.Commands {
    public class RedoCommand {
        private static readonly RoutedUICommand m_redo;

        public static RoutedUICommand Redo {
            get { return m_redo; }
        }

        static RedoCommand() {
            InputGestureCollection gestures = new InputGestureCollection();
            gestures.Add(new KeyGesture(Key.Y, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+Y"));
            m_redo = new RoutedUICommand("Redo", "Redo", typeof (RedoCommand), gestures);
        }
    }
}