using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using VeterinaryManagementSystem.Models;
using VeterinaryManagementSystem.Helpers;

namespace VeterinaryManagementSystem.Views.Inventory
{
    public partial class ProductDialog : Window, INotifyPropertyChanged
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly bool _isEditMode;
        private Product _product;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsEditMode => _isEditMode;

        public Product Product
        {
            get => _product;
            private set
            {
                _product = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Product)));
            }
        }

        public ObservableCollection<ProductCategory> Categories { get; private set; }
        public ObservableCollection<Supplier> Suppliers { get; private set; }

        public ProductDialog(DatabaseHelper dbHelper, Product product = null)
        {
            _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
            _isEditMode = product != null;
            Product = product ?? new Product
            {
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            InitializeComponent();
            DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var categories = _dbHelper.GetCategories();
                Categories = new ObservableCollection<ProductCategory>(categories);

                var suppliers = _dbHelper.GetSuppliers();
                Suppliers = new ObservableCollection<Supplier>(suppliers);

                // Set default category for new products
                if (!IsEditMode && Categories.Any())
                {
                    Product.Category = Categories.First();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateProduct())
            {
                DialogResult = true;
                Close();
            }
        }

        private bool ValidateProduct()
        {
            if (string.IsNullOrWhiteSpace(Product.SKU))
            {
                ShowError("El SKU es requerido.");
                SkuTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(Product.Name))
            {
                ShowError("El nombre del producto es requerido.");
                NameTextBox.Focus();
                return false;
            }

            if (Product.Category == null)
            {
                ShowError("Por favor seleccione una categoría.");
                CategoryComboBox.Focus();
                return false;
            }

            if (Product.Price <= 0)
            {
                ShowError("El precio debe ser mayor a cero.");
                PriceTextBox.Focus();
                return false;
            }

            if (Product.Cost <= 0)
            {
                ShowError("El costo debe ser mayor a cero.");
                CostTextBox.Focus();
                return false;
            }

            if (Product.Stock < 0)
            {
                ShowError("Las existencias no pueden ser negativas.");
                StockTextBox.Focus();
                return false;
            }

            // Validate unique SKU for new products or changed SKUs
            if (!IsEditMode || (IsEditMode && HasSkuChanged()))
            {
                try
                {
                    if (_dbHelper.IsSkuInUse(Product.SKU, Product.Id))
                    {
                        ShowError("Este SKU ya está en uso.");
                        SkuTextBox.Focus();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Error al validar SKU: {ex.Message}");
                    return false;
                }
            }

            return true;
        }

        private bool HasSkuChanged()
        {
            try
            {
                var originalProduct = _dbHelper.GetProductById(Product.Id);
                return originalProduct?.SKU != Product.SKU;
            }
            catch
            {
                return true; // If error, assume changed to force validation
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error de Validación",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}

