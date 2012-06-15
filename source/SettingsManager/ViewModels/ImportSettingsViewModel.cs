using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using SettingsManager.Core;
using System.Windows;
using Microsoft.Practices.Prism.Events;

namespace SettingsManager.ViewModels
{
    public class ImportSettingsViewModel : BaseViewModel
    {
        private IEventAggregator _events;

        public ImportSettingsViewModel(IEventAggregator events)
        {
            _events = events;

            LoadConfigurationCommand = new RelayCommand(LoadConfiguration, () => { return !string.IsNullOrWhiteSpace(SelectedMachineConfigPath); });
            ImportSettingsCommand = new RelayCommand(ImportSettings, () => { return !string.IsNullOrWhiteSpace(SelectedMachineConfigPath) && !string.IsNullOrWhiteSpace(ConfigurationName) && _isLoaded; });
            MachineConfigList.First().IsSelected = true;
        }

        private ObservableCollection<Option<string>> _machineConfigListInternal;
        private ObservableCollection<Option<string>> MachineConfigListInternal
        {
            get
            {
                if (_machineConfigListInternal == null)
                    _machineConfigListInternal = new ObservableCollection<Option<string>>(GetAllMachineConfigPaths());

                return _machineConfigListInternal;
            }
        }
        private ReadOnlyObservableCollection<Option<string>> _machineConfigList;
        public ReadOnlyObservableCollection<Option<string>> MachineConfigList
        {
            get
            {
                if (_machineConfigList == null)
                    _machineConfigList = new ReadOnlyObservableCollection<Option<string>>(MachineConfigListInternal);

                return _machineConfigList;
            }
        }

        private IEnumerable<Option<string>> GetAllMachineConfigPaths()
        {
            return Utilities.GetAllMachineConfigPaths().Select(x => new Option<string> { DisplayText = x.FullName, Value = x.FullName });
        }

        public RelayCommand LoadConfigurationCommand { get; private set; }

        private string SelectedMachineConfigPath
        {
            get { return MachineConfigList.First(x => x.IsSelected).Value; }
        }

        private bool _isLoaded;
        public void LoadConfiguration()
        {
            var config = System.Configuration.ConfigurationManager.OpenMappedMachineConfiguration(new System.Configuration.ConfigurationFileMap(SelectedMachineConfigPath));

            AppSettingsListInternal.Clear();
            ConnectionStringListInternal.Clear();

            foreach (System.Configuration.KeyValueConfigurationElement setting in config.AppSettings.Settings)
                AppSettingsListInternal.Add(new KeyValueViewModel { Key = setting.Key, Value = setting.Value });

            foreach (System.Configuration.ConnectionStringSettings connectionString in config.ConnectionStrings.ConnectionStrings)
                ConnectionStringListInternal.Add(new ConnectionStringViewModel { Key = connectionString.Name, Value = connectionString.ConnectionString, Provider = connectionString.ProviderName });

            _isLoaded = true;

            ImportSettingsCommand.CanExecute(null);
        }

        private ObservableCollection<KeyValueViewModel> _appSettingsListInternal;
        private ObservableCollection<KeyValueViewModel> AppSettingsListInternal
        {
            get
            {
                if (_appSettingsListInternal == null)
                    _appSettingsListInternal = new ObservableCollection<KeyValueViewModel>();

                return _appSettingsListInternal;
            }
        }
        private ReadOnlyObservableCollection<KeyValueViewModel> _appSettingsList;
        public ReadOnlyObservableCollection<KeyValueViewModel> AppSettingsList
        {
            get
            {
                if (_appSettingsList == null)
                    _appSettingsList = new ReadOnlyObservableCollection<KeyValueViewModel>(AppSettingsListInternal);

                return _appSettingsList;
            }
        }

        private ObservableCollection<ConnectionStringViewModel> _connectionStringListInternal;
        private ObservableCollection<ConnectionStringViewModel> ConnectionStringListInternal
        {
            get
            {
                if (_connectionStringListInternal == null)
                    _connectionStringListInternal = new ObservableCollection<ConnectionStringViewModel>();

                return _connectionStringListInternal;
            }
        }
        private ReadOnlyObservableCollection<ConnectionStringViewModel> _connectionStringList;
        public ReadOnlyObservableCollection<ConnectionStringViewModel> ConnectionStringList
        {
            get
            {
                if (_connectionStringList == null)
                    _connectionStringList = new ReadOnlyObservableCollection<ConnectionStringViewModel>(ConnectionStringListInternal);

                return _connectionStringList;
            }
        }

        public RelayCommand ImportSettingsCommand { get; private set; }
        public void ImportSettings()
        {
            var config = Configuration.Load();

            if (config.Contains(ConfigurationName))
            {
                MessageBox.Show("Failed to import settings. A configuration named '" + ConfigurationName + "' already exists. Please rename it and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var profileConfig = new ProfileConfiguration();
            profileConfig.Name = ConfigurationName;

            foreach (var connectionString in ConnectionStringList)
                profileConfig.AddConnectionString(connectionString.Key, connectionString.Value, connectionString.Provider);
            foreach (var setting in AppSettingsList)
                profileConfig.AddAppSetting(setting.Key, setting.Value);

            config.AddConfiguration(profileConfig);

            config.Save();

            _events.GetEvent<ImportSettingsCompleteEvent>().Publish(ConfigurationName);
        }

        private string _configurationName;
        public string ConfigurationName
        {
            get { return _configurationName; }
            set
            {
                if (_configurationName == value) return;

                _configurationName = value;

                OnPropertyChanged("ConfigurationName");

                ImportSettingsCommand.CanExecute(null);
            }
        }
    }
}
