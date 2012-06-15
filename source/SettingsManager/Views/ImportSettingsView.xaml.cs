using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SettingsManager.ViewModels;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Events;

namespace SettingsManager.Views
{
    /// <summary>
    /// Interaction logic for ImportSettingsView.xaml
    /// </summary>
    public partial class ImportSettingsView : Window
    {
        public ImportSettingsView()
        {
            InitializeComponent();

            var events = ServiceLocator.Current.GetInstance<IEventAggregator>();

            Name = ViewNames.ImportView;

            this.Loaded += (sender, e) => { DataContext = ServiceLocator.Current.GetInstance<ImportSettingsViewModel>();  };
        }
    }
}
