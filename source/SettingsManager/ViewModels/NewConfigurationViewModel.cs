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
    public class NewConfigurationViewModel : BaseViewModel
    {
        private IEventAggregator _events;

        public NewConfigurationViewModel()
        {
            _events = ServiceLocator.Current.GetInstance<IEventAggregator>();

            CreateConfigurationCommand = new RelayCommand(CreateConfiguration, () => { return !string.IsNullOrWhiteSpace(Name); });
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;

                _name = value;

                OnPropertyChanged("Name");

                CreateConfigurationCommand.CanExecute(null);
            }
        }

        public RelayCommand CreateConfigurationCommand { get; private set; }
        public void CreateConfiguration()
        {
            var config = Configuration.Load();
            if (config.Contains(Name))
            {
                MessageBox.Show("A configuration with the name " + Name + " already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            config.AddConfiguration(new ProfileConfiguration{ Name = Name });

            config.Save();

            _events.GetEvent<NewConfigurationCreatedEvent>().Publish(Name);
        }
    }
}
