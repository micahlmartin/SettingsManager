using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SettingsManager.Core
{
    [DataContract]
    public class Configuration
    {
        private IList<ProfileConfiguration> _profileConfigurations;
        private string _currentConfiguration;


        //[DataMember]
        //public IDictionary<string, ProfileConfiguration> ProfileConfigurations
        //{
        //    get
        //    {
        //        if (_profileConfigurations == null)
        //            _profileConfigurations = new Dictionary<string, ProfileConfiguration>();

        //        return _profileConfigurations;
        //    }
        //    set { _profileConfigurations = value; }
        //}

        [DataMember]
        public IEnumerable<ProfileConfiguration> ProfileConfigurations
        {
            get
            {
                if (_profileConfigurations == null)
                    _profileConfigurations = new List<ProfileConfiguration>();

                return _profileConfigurations;
            }
            set
            {
                if (value != null)
                    _profileConfigurations = value.ToList();
            }
        }

        [DataMember]
        public string CurrentConfiguration
        {
            get { return _currentConfiguration + ""; }
            set
            {
                _currentConfiguration = value;
            }
        }

        public ProfileConfiguration this[string configurationName]
        {
            get
            {
                return ProfileConfigurations.FirstOrDefault(x => x.Name.ToLowerInvariant() == configurationName.ToLowerInvariant());
            }
        }

        public void RemoveConfiguration(string name)
        {
            var itemToRemove = this[name.ToLowerInvariant()];
            if (itemToRemove != null)
                _profileConfigurations.Remove(itemToRemove);
        }

        public void AddConfiguration(ProfileConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(configuration.Name)) throw new ArgumentNullException("Configuration name must be specified");

            if (this[configuration.Name] != null)
                throw new ArgumentException("There is already a configuration named " + configuration.Name);

            _profileConfigurations.Add(configuration);
        }

        public void Save()
        {
            ConfigurationManager.SaveConfiguration(this, Settings.SettingsFilePath);
        }

        public static Configuration Load()
        {
            return ConfigurationManager.LoadConfiguration(Settings.SettingsFilePath);
        }

        public bool Contains(string name)
        {
            return this[name] != null;
        }
    }
}
