using System.Windows;
using System.Windows.Controls;
using VeterinaryManagementSystem.Config;
using VeterinaryManagementSystem.Helpers;
using VeterinaryManagementSystem.ViewModels;

namespace VeterinaryManagementSystem.Views.Proveedores
{
    public partial class SupplierView : Page
    {
        public SuppliersViewModel ViewModel { get; private set; }

        public SupplierView(DatabaseHelper databaseHelper)
        {
            InitializeComponent();
            ViewModel = new SuppliersViewModel(databaseHelper);
            this.DataContext = ViewModel;
        }
    }
}