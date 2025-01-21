using System.Windows;
using VeterinaryManagementSystem.Models;

namespace VeterinaryManagementSystem.Views.Inventory
{
    public partial class StockAdjustmentDialog : Window
    {
        public int NewStockLevel { get; private set; }

        public StockAdjustmentDialog(Product product)
        {
            InitializeComponent();
            // Asume que el producto ya tiene un stock actual que puedes mostrar si es necesario
            NewStockLevel = product.Stock;
        }

        // Manejador de evento para el botón "Cancel"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Cierra la ventana sin hacer ningún cambio
            this.DialogResult = false;
            this.Close();
        }

        // Manejador de evento para el botón "Save"
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Usa la propiedad NewStockLevel que ya está vinculada con el TextBox
            if (NewStockLevel >= 0)  // Asegúrate de que el nivel de stock sea válido
            {
                this.DialogResult = true;  // Se confirma el cambio
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid stock level.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
