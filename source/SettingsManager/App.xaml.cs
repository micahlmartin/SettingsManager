using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Microsoft.Practices.ServiceLocation;

namespace SettingsManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new BootStrapper();
            bootstrapper.Run();

            Controller = ServiceLocator.Current.GetInstance<Controller>();
        }

        public Controller Controller { get; set; }
    }
}
