using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SettingsManager.Core
{
    [DataContract]
    public class ProfileConfiguration
    {
        private IList<ConnectionStringSetting> _connectionStrings;
        private IList<AppSetting> _appSettings;

        public ProfileConfiguration()
        {
            _connectionStrings = new List<ConnectionStringSetting>();
            _appSettings = new List<AppSetting>();
        }

        [DataMember]
        public IEnumerable<ConnectionStringSetting> ConnectionStrings
        {
            get
            {
                if (_connectionStrings == null)
                    _connectionStrings = new List<ConnectionStringSetting>();

                return _connectionStrings;
            }
            set { _connectionStrings = new List<ConnectionStringSetting>(value); }
        }

        [DataMember]
        public IEnumerable<AppSetting> AppSettings
        {
            get
            {
                if (_appSettings == null)
                    _appSettings = new List<AppSetting>();

                return _appSettings;
            }
            set { _appSettings = new List<AppSetting>(value); }
        }

        [DataMember]
        public string Name { get; set; }

        public AppSetting GetAppSetting(string name)
        {
            return _appSettings.FirstOrDefault(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant());
        }
        public void AddAppSetting(AppSetting setting)
        {
            if (string.IsNullOrWhiteSpace(setting.Name)) throw new ArgumentNullException("Name must be specified.");
            AppSetting newSetting = GetAppSetting(setting.Name);
            if (newSetting == null)
            {
                newSetting = setting;
                _appSettings.Add(newSetting);
            }
            else
                newSetting.Value = setting.Value;

        }
        public void AddAppSetting(string name, string value)
        {
            AddAppSetting(new AppSetting { Name = name, Value = value });
        }

        public ConnectionStringSetting GetConnectionString(string name)
        {
            return _connectionStrings.FirstOrDefault(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant());
        }
        public void AddConnectionString(ConnectionStringSetting setting)
        {
            if (string.IsNullOrWhiteSpace(setting.Name)) throw new ArgumentNullException("Name must be specified.");

            ConnectionStringSetting newSetting = GetConnectionString(setting.Name);
            if (newSetting == null)
            {
                newSetting = setting;
                _connectionStrings.Add(newSetting);
            }
            else
            {
                newSetting.Provider = setting.Provider;
                newSetting.Value = setting.Value;
            }
        }
        public void AddConnectionString(string name, string value, string provider)
        {
            AddConnectionString(new ConnectionStringSetting { Name = name, Value = value, Provider = provider });
        }

        public void DeleteAppSetting(string name)
        {
            var setting = _appSettings.FirstOrDefault(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant());
            if(setting != null)
                _appSettings.Remove(setting);
        }

        internal void DeleteConnectionSTring(string name)
        {
            var setting = _connectionStrings.FirstOrDefault(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant());
            if(setting != null)
                _connectionStrings.Remove(setting);
        }
    }
}
