using System.Windows;
using System.Windows.Controls;
using VeterinaryManagementSystem.Config;
using VeterinaryManagementSystem.Helpers;
using VeterinaryManagementSystem.ViewModels;

namespace VeterinaryManagementSystem.Views.Proveedores
{
    public partial class SupplierView : Page
    {
        private SuppliersViewModel _viewModel;

        public SupplierView(DatabaseHelper databaseHelper)
        {
            InitializeComponent();
            _viewModel = new SuppliersViewModel(databaseHelper);
            DataContext = _viewModel;
        }
    }
}