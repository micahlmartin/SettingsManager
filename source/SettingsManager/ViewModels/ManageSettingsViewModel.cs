using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using System.Collections.ObjectModel;
using SettingsManager.Core;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;

namespace SettingsManager.ViewModels
{
    public class ManageSettingsViewModel : BaseViewModel
    {
        private IEventAggregator _events;

        public ManageSettingsViewModel(IEventAggregator events)
        {
            _events = events;
            _events.GetEvent<NewConfigurationCreatedEvent>().Subscribe(ConfigurationCreatedHandler);
            //_events.GetEvent<AppSettingCreatedEvent>().Subscribe(AppSettingCreatedHandler);

            if (ProfileConfigs.Any())
                ProfileConfigs.First().IsSelected = true;

            DeleteConfigurationCommand = new RelayCommand(DeleteConfiguration, () => { return SelectedConfig != null; });
            CreateConfigurationCommand = new RelayCommand(CreateConfiguration);
            AddAppSettingCommand = new RelayCommand(AddAppSetting);
        }

        private ObservableCollection<ProfileConfigViewModel> _profileConfigsInternal;
        private ObservableCollection<ProfileConfigViewModel> ProfileConfigsInternal
        {
            get
            {
                if (_profileConfigsInternal == null)
                {
                    _profileConfigsInternal = new ObservableCollection<ProfileConfigViewModel>();
                    ReloadConfigs();
                }

                return _profileConfigsInternal;
            }
        }
        private ReadOnlyObservableCollection<ProfileConfigViewModel> _profileConfigs;
        public ReadOnlyObservableCollection<ProfileConfigViewModel> ProfileConfigs
        {
            get
            {
                if (_profileConfigs == null)
                    _profileConfigs = new ReadOnlyObservableCollection<ProfileConfigViewModel>(ProfileConfigsInternal);

                return _profileConfigs;
            }
        }
        private void ReloadConfigs()
        {
            ProfileConfigsInternal.Clear();

            var configuration = Configuration.Load();

            foreach (var config in configuration.ProfileConfigurations.Select(x => new ProfileConfigViewModel(x)))
                ProfileConfigsInternal.Add(config);

            OnPropertyChanged("ProfileConfigs");
        }

        private BindingList<KeyValueViewModel> test;

        public ProfileConfigViewModel SelectedConfig
        {
            get
            {
                return ProfileConfigs.FirstOrDefault(x => x.IsSelected);
            }
        }

        public RelayCommand DeleteConfigurationCommand { get; private set; }
        public void DeleteConfiguration(object parameter)
        {
            var config = (ProfileConfigViewModel)parameter;


            if (MessageBox.Show("Are you sure you want to delete this configuration?", "Confirm Delete", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                ConfigurationManager.DeleteProfileConfiguration(config.Name);

                ProfileConfigsInternal.Remove(config);

                _events.GetEvent<ConfigurationDeletedEvent>().Publish(config.Name);

                OnPropertyChanged("ProfileConfigs");
            }
        }

        public RelayCommand CreateConfigurationCommand { get; private set; }
        public void CreateConfiguration()
        {
            _events.GetEvent<OpenCreateConfigurationViewEvent>().Publish(true);
        }
        private void ConfigurationCreatedHandler(string configurationName)
        {
            var config = Configuration.Load();

            var newConfig = config[configurationName];
            ProfileConfigsInternal.Add(new ProfileConfigViewModel(newConfig));

            OnPropertyChanged("ProfileConfigs");
        }

        public RelayCommand AddAppSettingCommand { get; private set; }
        public void AddAppSetting()
        {
            _events.GetEvent<OpenCreateAppSettingViewEvent>().Publish(new OpenCreateAppSettingsViewEventArgs { ConfugurationName = SelectedConfig.Name, ShowAsDialog = true });
        }
        //private void AppSettingCreatedHandler(AppSettingsCreatedEventArgs e)
        //{
        //    var config = Configuration.Load();
            
        //    var newSetting = config.ProfileConfigurations[e.ConfigurationName].AppSettings[e.Key];

        //    ProfileConfigsInternal.First(x => x.Name == e.ConfigurationName).AppSettings.Add(new KeyValueViewModel{ Key = e.Key, Value = e.Value, IsSelected = true});
        //}

        public bool DeleteAppSetting(KeyValueViewModel vm)
        {
            if (MessageBox.Show("Are you sure you want to delete this setting?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return false;

            ConfigurationManager.DeleteAppSetting(SelectedConfig.Name, vm.Key);

            SelectedConfig.AppSettings.Remove(vm);

            return true;
        }

        public bool EditAppSetting(KeyValueViewModel vm)
        {
            if (!string.IsNullOrWhiteSpace(vm.Error))
            {
                vm.CancelEdit();
                return false;
            }

            vm.EndEdit();

            ConfigurationManager.SaveAppSetting(SelectedConfig.Name, new AppSetting { Name = vm.Key, Value = vm.Value });

            return true;
        }

        internal bool EditConnectionString(ConnectionStringViewModel vm)
        {
            if (!string.IsNullOrWhiteSpace(vm.Error))
            {
                vm.CancelEdit();
                return false;
            }

            vm.EndEdit();

            ConfigurationManager.SaveConnectionString(SelectedConfig.Name, new ConnectionStringSetting { Name = vm.Key, Provider = vm.Provider, Value = vm.Value });

            return true;
        }

        internal bool DeleteConnectionString(ConnectionStringViewModel vm)
        {
            if (MessageBox.Show("Are you sure you want to delete this connection string?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return false;

            ConfigurationManager.DeleteConnectionString(SelectedConfig.Name, vm.Key);

            SelectedConfig.ConnectionStrings.Remove(vm);

            return true;
        }
    }
}
