using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SettingsManager
{
    public class RelayCommand : ICommand
    {
        private Action _handler;
        private Action<object> _handler2;
        private Func<bool> _canExecute;
        private bool _isEnabled;

        public RelayCommand(Action handler) : this(handler, () => { return true; }) { }
        public RelayCommand(Action handler, Func<bool> canExecute)
        {
            _handler = handler;
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object> handler) : this(handler, () => { return true; }) { }
        public RelayCommand(Action<object> handler, Func<bool> canExecute)
        {
            _handler2 = handler;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            IsEnabled = _canExecute();
            return IsEnabled;
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    if (CanExecuteChanged != null)
                        CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (_handler != null)
                _handler();
            else
                _handler2(parameter);
        }
    }
}
