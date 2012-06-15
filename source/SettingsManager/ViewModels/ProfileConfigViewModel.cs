using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SettingsManager.Core;
using System.ComponentModel;

namespace SettingsManager.ViewModels
{
    public class ProfileConfigViewModel : SelectableViewModel
    {
        private ProfileConfiguration _config;

        public ProfileConfigViewModel(ProfileConfiguration config)
        {
            _config = config;

            AppSettings.AddingNew += (sender, e) => { e.NewObject = new KeyValueViewModel(); };
            AppSettings.ListChanged += (sender, e) => 
            {
                if (e.ListChangedType == ListChangedType.ItemAdded)
                {

                }

            };
        }

        void AppSettings_AddingNew(object sender, AddingNewEventArgs e)
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { return _config.Name; }
            set
            {
                if (_config.Name == value) return;

                _config.Name = value;

                OnPropertyChanged("Name");
            }
        }

        private ObservableCollection<ConnectionStringViewModel> _connectionStrings;
        public ObservableCollection<ConnectionStringViewModel> ConnectionStrings
        {
            get
            {
                if (_connectionStrings == null)
                    _connectionStrings = new ObservableCollection<ConnectionStringViewModel>(_config.ConnectionStrings.Select(x => new ConnectionStringViewModel { Key = x.Name, Value = x.Value, Provider = x.Provider }));

                return _connectionStrings;
            }
        }

        private BindingList<KeyValueViewModel> _appSettings;
        public BindingList<KeyValueViewModel> AppSettings
        {
            get
            {
                if (_appSettings == null)
                    _appSettings = new BindingList<KeyValueViewModel>(_config.AppSettings.Select(x => new KeyValueViewModel { Key = x.Name, Value = x.Value }).ToList());

                return _appSettings;
            }
        }
    }
}
