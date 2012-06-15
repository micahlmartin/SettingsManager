using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace SettingsManager.Core
{
    public static class ConfigurationManager
    {
        private static object LockObject = new object();

        public static Configuration LoadConfiguration(string path)
        {
            lock (LockObject)
            {
                return LoadConfigurationInternal(path);
            }
        }
        private static Configuration LoadConfigurationInternal(string path)
        {
            if (!File.Exists(path))
                return new Configuration();

            using (var reader = XmlReader.Create(path))
            {
                var serializer = new DataContractSerializer(typeof(Configuration));
                return (Configuration)serializer.ReadObject(reader);
            }
        }

        public static void SaveConfiguration(Configuration configuration, string path)
        {
            lock (LockObject)
            {
                SaveConfigurationInternal(configuration, path);
            }
        }
        private static void SaveConfigurationInternal(Configuration configuration, string path)
        {
            EnsureFolder(path);
            using (var writer = XmlWriter.Create(path, new XmlWriterSettings { Indent = true }))
            {
                var serializer = new DataContractSerializer(configuration.GetType());
                serializer.WriteObject(writer, configuration);
            }
        }

        private static void EnsureFolder(string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public static void SaveProfileConfiguration(string name, ProfileConfiguration config)
        {
            lock (LockObject)
            {
                var configuration = LoadConfigurationInternal(Settings.SettingsFilePath);
                configuration.RemoveConfiguration(name);

                configuration.AddConfiguration(config);

                SaveConfigurationInternal(configuration, Settings.SettingsFilePath);
            }
        }

        public static void DeleteProfileConfiguration(string name)
        {
            lock (LockObject)
            {
                var configuration = LoadConfigurationInternal(Settings.SettingsFilePath);
                configuration.RemoveConfiguration(name);
                SaveConfigurationInternal(configuration, Settings.SettingsFilePath);
            }
        }

        public static void SetCurrentConfiguration(string name)
        {
            lock (LockObject)
            {
                var configuration = LoadConfigurationInternal(Settings.SettingsFilePath);
                configuration.CurrentConfiguration = name;
                SaveConfigurationInternal(configuration, Settings.SettingsFilePath);

                var filesToUpdate = Utilities.GetAllMachineConfigPaths();

                UpdateSettings(filesToUpdate, configuration[name]);
            }
        }
        private static void UpdateSettings(IEnumerable<System.IO.FileInfo> filesToUpdate, ProfileConfiguration config)
        {
            foreach (var file in filesToUpdate)
            {
                var configFile = System.Configuration.ConfigurationManager.OpenMappedMachineConfiguration(new System.Configuration.ConfigurationFileMap(file.FullName));
                configFile.AppSettings.Settings.Clear();
                configFile.ConnectionStrings.ConnectionStrings.Clear();

                foreach (var appSetting in config.AppSettings)
                    configFile.AppSettings.Settings.Add(appSetting.Name, appSetting.Value);
                foreach (var connectionString in config.ConnectionStrings)
                {
                    var connectionConfig = new System.Configuration.ConnectionStringSettings(connectionString.Name, connectionString.Value);
                    if (!string.IsNullOrWhiteSpace(connectionString.Provider))
                        connectionConfig.ProviderName = connectionString.Provider;

                    configFile.ConnectionStrings.ConnectionStrings.Add(connectionConfig);
                }

                configFile.Save();
            }

        }

        public static ProfileConfiguration GetCurrentConfiguration()
        {
            lock (LockObject)
            {
                var configuration = LoadConfigurationInternal(Settings.SettingsFilePath);
                return configuration[configuration.CurrentConfiguration];
            }
        }

        public static void DeleteAppSetting(string configurationName, string settingName)
        {
            lock (LockObject)
            {
                var configuration = LoadConfigurationInternal(Settings.SettingsFilePath);
                var config = configuration[configurationName];
                if (config != null)
                    config.DeleteAppSetting(settingName);

                SaveConfigurationInternal(configuration, Settings.SettingsFilePath);
            }
        }

        public static void SaveAppSetting(string configurationName, AppSetting setting)
        {
            lock (LockObject)
            {
                var configuration = LoadConfiguration(Settings.SettingsFilePath);
                var config = configuration[configurationName];
                config.AddAppSetting(setting);
                SaveConfigurationInternal(configuration, Settings.SettingsFilePath);
            }
        }

        public static void SaveConnectionString(string configurationName, ConnectionStringSetting connectionString)
        {
            lock (LockObject)
            {
                var configuration = LoadConfigurationInternal(Settings.SettingsFilePath);
                var config = configuration[configurationName];
                config.AddConnectionString(connectionString);
                SaveConfigurationInternal(configuration, Settings.SettingsFilePath);
            }
        }

        internal static void DeleteConnectionString(string configurationName, string connectionStringName)
        {
            lock (LockObject)
            {
                var configuration = LoadConfigurationInternal(Settings.SettingsFilePath);
                var config = configuration[configurationName];
                if (config != null)
                    config.DeleteConnectionSTring(connectionStringName);

                SaveConfigurationInternal(configuration, Settings.SettingsFilePath);
            }
        }
    }
}
