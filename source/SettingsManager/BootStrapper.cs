using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.UnityExtensions;
using SettingsManager.Views;
using System.Windows;

namespace SettingsManager
{
    public class BootStrapper : UnityBootstrapper
    {

        protected override System.Windows.DependencyObject CreateShell()
        {
            return Container.TryResolve<NotificationWindow>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            App.Current.MainWindow = (Window)this.Shell;    
        }
    }
}
