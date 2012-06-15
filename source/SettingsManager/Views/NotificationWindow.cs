using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Practices.Prism.Events;
using SettingsManager.Core;

namespace SettingsManager.Views
{
    public class NotificationWindow : Window
    {
        private NotifyIcon _notifyIcon;
        private ContextMenuStrip _trayMenu;
        private IEventAggregator _events;
        private IList<ToolStripItem> _configItems;

        public NotificationWindow(IEventAggregator events)
        {
            Name = ViewNames.NotificationView;

            _events = events;
            _events.GetEvent<ImportSettingsCompleteEvent>().Subscribe(SettingsImportedHandler);
            _events.GetEvent<ConfigurationDeletedEvent>().Subscribe(ConfigurationDeletedHandler);
            _events.GetEvent<NewConfigurationCreatedEvent>().Subscribe(NewConfigCreatedHandler);

            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = new Icon("Resources/icon.ico");
            _notifyIcon.Visible = true;

            BuildMenu();
        }
        private void BuildMenu()
        {
            _trayMenu = new ContextMenuStrip();
            _trayMenu.Opening += (sender, e) => { SetSelectedConfig(); };
            _configItems = new List<ToolStripItem>();

            _trayMenu.Items.Add("&Create Configuration", null, (sender, e) => { _events.GetEvent<OpenCreateConfigurationViewEvent>().Publish(false); });
            _trayMenu.Items.Add("&Import Settings", new Bitmap("Resources/Import.png"), (sender, e) => { _events.GetEvent<OpenImportViewEvent>().Publish(EventArgs.Empty); });
            _trayMenu.Items.Add("&Manage Settings", null, (sender, e) => { _events.GetEvent<OpenManageSettingsViewEvent>().Publish(EventArgs.Empty); });
            _trayMenu.Items.Add("&Exit", null, (sender, e) => { Exit(); });
            
            _notifyIcon.ContextMenuStrip = _trayMenu;

            BuildConfigItems();
        }

        private void SetSelectedConfig()
        {
            var currentConfig = Configuration.Load().CurrentConfiguration;

            foreach (var item in _configItems)
            {
                var toolstripItem = item as ToolStripMenuItem;
                if (toolstripItem != null)
                {
                    toolstripItem.Checked = toolstripItem.Tag.ToString() == currentConfig;
                }
            }
        }

        private void BuildConfigItems()
        {
            ClearConfigItems();

            var config = Configuration.Load();

            if (config.ProfileConfigurations.Any())
            {
                var menuItem = new ToolStripSeparator();
                _configItems.Add(menuItem);
                _trayMenu.Items.Add(menuItem);
            }

            foreach (var profileConfig in config.ProfileConfigurations)
            {
                var isSelected = profileConfig.Name.ToLowerInvariant() == config.CurrentConfiguration.ToLowerInvariant();
                var menuItem = new ToolStripMenuItem(profileConfig.Name);
                menuItem.CheckOnClick = true;
                menuItem.Checked = isSelected;
                menuItem.Tag = profileConfig.Name;
                menuItem.Click += (sender, e) => { SelectConfiguration(((ToolStripMenuItem)sender).Tag.ToString()); };
                _configItems.Add(menuItem);
                _trayMenu.Items.Add(menuItem);
            }
        }

        private void SelectConfiguration(string configuration)
        {
            _events.GetEvent<SelectedConfigChangedEvent>().Publish(configuration);

            Notify(null, "Switched to the '" + configuration + "' configuration", ToolTipIcon.Info);
        }

        private void ClearConfigItems()
        {
            foreach (var item in _configItems)
            {
                _trayMenu.Items.Remove(item);
            }
        }
        private void Notify(string title, string message, ToolTipIcon icon)
        {
            _notifyIcon.BalloonTipIcon = icon;
            _notifyIcon.BalloonTipText = message;
            _notifyIcon.BalloonTipTitle = title;
            _notifyIcon.ShowBalloonTip(500);
        }
        private void Exit()
        {
            App.Current.Shutdown();
            _notifyIcon.Dispose();
        }


        public void SettingsImportedHandler(string settingName)
        {
            Notify("Import Complete", "Settings were successfully imported", ToolTipIcon.Info);
            BuildConfigItems();
        }

        public void ConfigurationDeletedHandler(string configurationName)
        {
            BuildConfigItems();
        }

        public void NewConfigCreatedHandler(string configurationName)
        {
            Notify("Created Configuration", "The new configuration '" + configurationName + "' was created successfully", ToolTipIcon.Info);
            BuildConfigItems();
        }
    }
}
