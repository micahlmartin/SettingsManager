using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettingsManager.ViewModels
{
    public class ConnectionStringViewModel : KeyValueViewModel
    {
        private string _provider;
        public string Provider
        {
            get { return _provider; }
            set
            {
                if (_provider == value) return;

                _provider = value;

                OnPropertyChanged("Provider");
            }
        }
    }
}
