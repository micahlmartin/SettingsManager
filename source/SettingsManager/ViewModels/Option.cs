using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettingsManager.ViewModels
{
    public class Option<T> : BaseViewModel
    {
        public string DisplayText { get; set; }
        public T Value { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;

                _isSelected = value;

                OnPropertyChanged("IsSelected");
            }
        }
    }
}
