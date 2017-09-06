using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ZUtility
{
    public class DataCommands
    {
        private static RoutedUICommand refresh;
        static DataCommands()
        {
            // Initialize the command.
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R"));
            refresh = new RoutedUICommand(
            "Refresh", "Refresh", typeof(DataCommands), inputs);
        }
        public static RoutedUICommand Refresh
        {
            get { return refresh; }
        }
    }
}
