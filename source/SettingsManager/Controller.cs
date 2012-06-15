using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using Microsoft.Data.ConnectionUI;
using Microsoft.Practices.Prism.Events;
using System.Windows;
using SettingsManager.Views;
using SettingsManager.Core;
using SettingsManager.ViewModels;

namespace SettingsManager
{
    public class Controller
    {
        private IEventAggregator _events;

        public Controller(IEventAggregator events)
        {
            _events = events;

            _events.GetEvent<OpenImportViewEvent>().Subscribe(e => { ShowImport(); });
            _events.GetEvent<ImportSettingsCompleteEvent>().Subscribe(e => { CloseImportView(); });
            _events.GetEvent<SelectedConfigChangedEvent>().Subscribe(e => { SelectConfiguration(e); });
            _events.GetEvent<OpenManageSettingsViewEvent>().Subscribe(e => { ShowManageSettings(); });
            _events.GetEvent<OpenCreateConfigurationViewEvent>().Subscribe(e => { ShowCreateConfiguration(); });
            _events.GetEvent<NewConfigurationCreatedEvent>().Subscribe(e => { CloseCreateConfigurationView(); });
            _events.GetEvent<OpenCreateAppSettingViewEvent>().Subscribe(e => { ShowCreateAppSettingConfiguration(e); });
            _events.GetEvent<AppSettingCreatedEvent>().Subscribe(e => { CloseCreateppSettingConfiguration(e); });
        }

        private void CloseCreateConfigurationView()
        {
            var win = TryGetView(ViewNames.CreateConfigurationView);
            if (win != null)
            {
                if (System.Windows.Interop.ComponentDispatcher.IsThreadModal)
                    win.DialogResult = true;
                else
                    win.Close();
            }
        }

        private void CloseImportView()
        {
            var win = TryGetView(ViewNames.ImportView);
            if (win != null)
            {
                if (System.Windows.Interop.ComponentDispatcher.IsThreadModal)
                    win.DialogResult = true;
                else
                    win.Close();
            }
        }

        private void CloseCreateppSettingConfiguration(AppSettingsCreatedEventArgs e)
        {
            var win = TryGetView(ViewNames.CreateAppSettingView);
            if (win != null)
            {
                if (System.Windows.Interop.ComponentDispatcher.IsThreadModal)
                    win.DialogResult = true;
                else
                    win.Close();
            }
        }

        private void ShowCreateAppSettingConfiguration(OpenCreateAppSettingsViewEventArgs e)
        {
            var win = TryGetView(ViewNames.CreateAppSettingView);
            if (win != null)
            {
                win.Activate();
                return;
            }

            var vm = new NewAppSettingViewModel();
            vm.ConfigurationName = e.ConfugurationName;
            
            win = new AddAppSettingView();
            win.DataContext = vm;

            if (e.ShowAsDialog)
                win.ShowDialog();
            else
                win.Show();
        }

        public void SelectConfiguration(string configName)
        {
            ConfigurationManager.SetCurrentConfiguration(configName);            
        }

        private void UpdateSettings(IEnumerable<System.IO.FileInfo> filesToUpdate, ProfileConfiguration config)
        {
            foreach (var file in filesToUpdate)
            {
                var configFile = System.Configuration.ConfigurationManager.OpenMappedMachineConfiguration(new System.Configuration.ConfigurationFileMap(file.FullName));
                configFile.AppSettings.Settings.Clear();
                configFile.ConnectionStrings.ConnectionStrings.Clear();

                foreach (var appSetting in config.AppSettings)
                    configFile.AppSettings.Settings.Add(appSetting.Name, appSetting.Value);
                foreach (var connectionString in config.ConnectionStrings)
                    configFile.ConnectionStrings.ConnectionStrings.Add(new System.Configuration.ConnectionStringSettings(connectionString.Name, connectionString.Value, connectionString.Provider));

                configFile.Save();
            }
            
        }

        public void ShowImport()
        {
            var win = TryGetView(ViewNames.ImportView);
            if (win != null)
            {
                win.Activate();
                return;
            }

            win = new ImportSettingsView();
            win.Show();
        }

        public void ShowManageSettings()
        {
            var win = TryGetView(ViewNames.ManageSettingsView);
            if (win != null)
            {
                win.Activate();
                return;
            }

            win = new ManageSettingsView();
            win.Show();
        }

        public void ShowCreateConfiguration()
        {
            var win = TryGetView(ViewNames.CreateConfigurationView);
            if (win != null)
            {
                win.Activate();
                return;
            }

            win = new NewConfigurationView();
            win.ShowDialog();
        }

        private Window TryGetView(string viewName)
        {
            for (int i = 0; i < App.Current.Windows.Count; i++)
            {
                var win = App.Current.Windows[i];
                if (win.Name == viewName)
                    return win;
            }

            return null;
        }

        public static string GetConnectionStringFromDialog()
        {
            DataConnectionDialog dcd = new DataConnectionDialog();

            if (DataConnectionDialog.Show(dcd) == DialogResult.OK)
            {
                return dcd.ConnectionString;
            }

            return string.Empty;
        }
    }
}
