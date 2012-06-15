using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace SettingsManager.Core
{
    public static class Settings
    {
        public static string SettingsFilePath = Path.Combine(Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SettingsManager"), "settings.xml");
    }
}
