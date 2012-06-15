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
using Microsoft.Practices.Prism.Events;
using SettingsManager.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace SettingsManager.Views
{
    /// <summary>
    /// Interaction logic for ManageSettingsView.xaml
    /// </summary>
    public partial class ManageSettingsView : Window
    {
        private ManageSettingsViewModel _viewModel;

        public ManageSettingsView()
        {
            InitializeComponent();

            Name = ViewNames.ManageSettingsView;

            this.Loaded += (sender, e) =>
            {
                _viewModel = ServiceLocator.Current.GetInstance<ManageSettingsViewModel>();
                DataContext = _viewModel;
            };
        }

        private void AppSettings_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            ((KeyValueViewModel)e.Row.Item).BeginEdit();
        }

        private void AppSettings_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            _viewModel.EditAppSetting((KeyValueViewModel)e.Row.Item);
        }

        private void AppSettings_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var grid = (DataGrid)sender;

                if (grid.SelectedItems.Count > 0)
                {
                    if (!_viewModel.DeleteAppSetting((KeyValueViewModel)grid.SelectedItems[0]))
                        e.Handled = true;
                }
            }
        }

        private void ConnectionStrings_Beginning(object sender, DataGridBeginningEditEventArgs e)
        {
            ((ConnectionStringViewModel)e.Row.Item).BeginEdit();
        }

        private void ConnectionStrings_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            _viewModel.EditConnectionString((ConnectionStringViewModel)e.Row.Item);
        }

        private void ConnectionStrings_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var grid = (DataGrid)sender;

                if (grid.SelectedItems.Count > 0)
                {
                    if (!_viewModel.DeleteConnectionString((ConnectionStringViewModel)grid.SelectedItems[0]))
                        e.Handled = true;
                }
            }
        }
    }
}
