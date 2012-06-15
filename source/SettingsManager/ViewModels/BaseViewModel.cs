using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SettingsManager.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var propertyChangedEvent = PropertyChanged;
            if (propertyChangedEvent != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
