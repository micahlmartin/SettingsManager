using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettingsManager.Core;
using System.Windows;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;

namespace SettingsManager.ViewModels
{
    public class NewAppSettingViewModel : KeyValueViewModel
    {
        private Configuration _config;
        private IEventAggregator _events;

        public NewAppSettingViewModel()
        {
            //_events = ServiceLocator.Current.GetInstance<IEventAggregator>();

            //_config = Configuration.Load();

            //CreateSettingCommand = new RelayCommand(CreateSetting, () => { return !string.IsNullOrWhiteSpace(Key) && !string.IsNullOrWhiteSpace(Value); });

            //this.PropertyChanged += (sender, e) => { CreateSettingCommand.CanExecute(null); };
        }

        public string ConfigurationName { get; set; }

        private bool _applyToAll = true;
        public bool ApplyToAll
        {
            get { return _applyToAll; }
            set
            {
                if (_applyToAll == value) return;

                _applyToAll = value;

                OnPropertyChanged("ApplyToAll");
            }
        }

        //public RelayCommand CreateSettingCommand { get; private set; }
        //public void CreateSetting()
        //{
        //    if (_config.ProfileConfigurations[ConfigurationName].AppSettings.ContainsKey(Key))
        //    {
        //        MessageBox.Show("A setting with the name " + Key + " already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //    if (ApplyToAll)
        //    {
        //        foreach (var profileConfig in _config.ProfileConfigurations)
        //        {
        //            if (!profileConfig.Value.AppSettings.ContainsKey(Key))
        //                profileConfig.Value.AppSettings.Add(Key, Value);
        //        }
        //    }
        //    else
        //        _config.ProfileConfigurations[ConfigurationName].AppSettings.Add(Key, Value);

        //    _config.Save();

        //    _events.GetEvent<AppSettingCreatedEvent>().Publish(new AppSettingsCreatedEventArgs
        //    {
        //        ConfigurationName = ConfigurationName,
        //        Key = Key,
        //        Value = Value,
        //        ApplyToAll = ApplyToAll
        //    });
        //}
    }
}
