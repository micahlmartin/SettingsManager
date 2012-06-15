using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettingsManager.ViewModels
{
    public class SelectableViewModel : BaseViewModel
    {
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
