using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using VeterinaryManagementSystem.Helpers;
using VeterinaryManagementSystem.Models;
using VeterinaryManagementSystem.Views.Inventory;

namespace VeterinaryManagementSystem.ViewModels
{
    public partial class InventoryViewModel : ObservableObject
    {
        private readonly DatabaseHelper _dbHelper;

        [ObservableProperty]
        private ObservableCollection<Product> products;

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private ProductCategory selectedCategory;

        [ObservableProperty]
        private string selectedStockFilter;

        public ObservableCollection<ProductCategory> Categories { get; private set; }

        public ObservableCollection<string> StockFilters { get; } = new ObservableCollection<string>
    {
        "All",
        "Low Stock",
        "Out of Stock",
        "In Stock"
    };

        public IEnumerable<Product> FilteredProducts => FilterProducts();

        public InventoryViewModel(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
            LoadData();

            // Subscribe to property changes for filtering
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SearchText) ||
                    e.PropertyName == nameof(SelectedCategory) ||
                    e.PropertyName == nameof(SelectedStockFilter))
                {
                    OnPropertyChanged(nameof(FilteredProducts));
                }
            };
        }

        private IEnumerable<Product> FilterProducts()
        {
            var query = Products.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(p =>
                    p.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    p.SKU.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            if (SelectedCategory != null)
            {
                query = query.Where(p => p.Category.Id == SelectedCategory.Id);
            }

            switch (SelectedStockFilter)
            {
                case "Sin Stock":
                    query = query.Where(p => p.Stock == 0);
                    break;
                case "Casi Sin Stock":
                    query = query.Where(p => p.Stock == 5);
                    break;
                case "En Stock":
                    query = query.Where(p => p.Stock > p.Stock);
                    break;
            }

            return query.ToList();
        }

        private void LoadData()
        {
            try
            {
                Products = new ObservableCollection<Product>(_dbHelper.GetProducts());
                Categories = new ObservableCollection<ProductCategory>(_dbHelper.GetCategories());
                SelectedStockFilter = "All"; // Set default filter
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void ManageCategories()
        {
            var dialog = new CategoryManagementDialog(Categories, _dbHelper);
            if (dialog.ShowDialog() == true)
            {
                // Refresh categories
                Categories = new ObservableCollection<ProductCategory>(_dbHelper.GetCategories());
                OnPropertyChanged(nameof(Categories));
            }
        }


        [RelayCommand]
        private void AddProduct()
        {
            try
            {
                var dialog = new ProductDialog(_dbHelper);
                if (dialog.ShowDialog() == true)
                {
                    var product = dialog.Product;
                    _dbHelper.AddProduct(product);
                    Products.Add(product);
                    MessageBox.Show("Product added successfully", "Success");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product: {ex.Message}", "Error");
            }
        }

        [RelayCommand]
        private void EditProduct(Product product)
        {
            try
            {
                var dialog = new ProductDialog(_dbHelper, product);
                if (dialog.ShowDialog() == true)
                {
                    var updatedProduct = dialog.Product;
                    _dbHelper.UpdateProduct(updatedProduct);

                    var index = Products.IndexOf(product);
                    Products[index] = updatedProduct;

                    MessageBox.Show("Product updated successfully", "success");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}", "error");
            }
        }

        [RelayCommand]
        private void DeleteProduct(Product product)
        {
            try
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete {product.Name}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _dbHelper.DeleteProduct(product.Id);
                    Products.Remove(product);
                    MessageBox.Show("Product deleted successfully", "success");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting product: {ex.Message}", "error");
            }
        }

        [RelayCommand]
        private void AdjustStock(Product product)
        {
            try
            {
                var dialog = new StockAdjustmentDialog(product);
                if (dialog.ShowDialog() == true)
                {
                    var newStock = dialog.NewStockLevel;
                    product.Stock = newStock;
                    _dbHelper.UpdateProduct(product);

                    var index = Products.IndexOf(product);
                    Products[index] = product;

                    MessageBox.Show("Stock adjusted successfully", "success");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adjusting stock: {ex.Message}", "error");
            }
        }
    }
}