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
    /// Interaction logic for AddAppSettingView.xaml
    /// </summary>
    public partial class AddAppSettingView : Window
    {
        public AddAppSettingView()
        {
            InitializeComponent();

            Name = ViewNames.CreateAppSettingView;

            var events = ServiceLocator.Current.GetInstance<IEventAggregator>();
            //events.GetEvent<AppSettingCreatedEvent>().Subscribe(e => { this.DialogResult = true; });
        }
    }
}
