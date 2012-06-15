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
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Events;
using SettingsManager.ViewModels;

namespace SettingsManager.Views
{
    /// <summary>
    /// Interaction logic for NewConfigurationView.xaml
    /// </summary>
    public partial class NewConfigurationView : Window
    {
        public NewConfigurationView()
        {
            InitializeComponent();

            Name = ViewNames.CreateConfigurationView;

            this.Loaded += (sender, e) => { DataContext = ServiceLocator.Current.GetInstance<NewConfigurationViewModel>(); };
        }
    }
}
