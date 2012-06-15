using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SettingsManager.Core
{
    public static class Utilities
    {
        public static IEnumerable<FileInfo> GetAllMachineConfigPaths()
        {
            var searchRoot = new DirectoryInfo(new DirectoryInfo(new DirectoryInfo(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory()).Parent.FullName).Parent.FullName);
            return searchRoot.GetFiles("machine.config", SearchOption.AllDirectories);
        }
    }
}
