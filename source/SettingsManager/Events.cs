using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;

namespace SettingsManager
{
    public class OpenImportViewEvent : CompositePresentationEvent<EventArgs> { }
    public class OpenManageSettingsViewEvent : CompositePresentationEvent<EventArgs> { }
    public class ImportSettingsCompleteEvent : CompositePresentationEvent<string> { }
    public class UpdateSettingsCompleteEvent : CompositePresentationEvent<string> { }
    public class ConfigurationDeletedEvent : CompositePresentationEvent<string> { }
    public class SelectedConfigChangedEvent : CompositePresentationEvent<string> { }
    public class OpenCreateConfigurationViewEvent : CompositePresentationEvent<bool> { }
    public class NewConfigurationCreatedEvent : CompositePresentationEvent<string> { }



    public class OpenCreateAppSettingViewEvent : CompositePresentationEvent<OpenCreateAppSettingsViewEventArgs> { }
    public class OpenCreateAppSettingsViewEventArgs : EventArgs
    {
        public bool ShowAsDialog { get; set; }
        public string ConfugurationName { get; set; }
    }

    public class AppSettingCreatedEvent : CompositePresentationEvent<AppSettingsCreatedEventArgs> { }
    public class AppSettingsCreatedEventArgs : EventArgs
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string ConfigurationName { get; set; }
        public bool ApplyToAll { get; set; }
    }
}
