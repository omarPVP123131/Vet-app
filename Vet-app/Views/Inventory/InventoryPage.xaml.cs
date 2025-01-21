using System.Windows.Controls;
using VeterinaryManagementSystem.Helpers;
using VeterinaryManagementSystem.ViewModels;

namespace VeterinaryManagementSystem.Views.Inventory
{
    public partial class InventoryPage : Page
    {
        public InventoryPage(DatabaseHelper dbHelper)
        {
            InitializeComponent();
            DataContext = new InventoryViewModel(dbHelper);
        }

    }
}