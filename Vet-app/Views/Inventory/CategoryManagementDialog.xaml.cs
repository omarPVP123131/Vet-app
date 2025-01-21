using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using VeterinaryManagementSystem.Models;
using VeterinaryManagementSystem.Helpers;

namespace VeterinaryManagementSystem.Views.Inventory
{
    public partial class CategoryManagementDialog : Window
    {
        private readonly ObservableCollection<ProductCategory> _categories;
        private readonly DatabaseHelper _dbHelper;

        public CategoryManagementDialog(ObservableCollection<ProductCategory> categories, DatabaseHelper dbHelper)
        {
            InitializeComponent();
            _categories = categories;
            _dbHelper = dbHelper;
            CategoriesList.ItemsSource = _categories;
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var categoryName = NewCategoryTextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(categoryName))
                {
                    MessageBox.Show("Please enter a category name.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_categories.Any(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("A category with this name already exists.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newCategory = new ProductCategory { Name = categoryName };
                _dbHelper.AddCategory(newCategory);
                _categories.Add(newCategory);
                NewCategoryTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding category: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = (Button)sender;
                var category = (ProductCategory)button.Tag;
                var dialog = new EditCategoryDialog(category.Name);

                if (dialog.ShowDialog() == true)
                {
                    var newName = dialog.CategoryName.Trim();
                    if (string.IsNullOrWhiteSpace(newName))
                    {
                        MessageBox.Show("Category name cannot be empty.", "Validation Error",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (_categories.Any(c => c.Id != category.Id &&
                        c.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)))
                    {
                        MessageBox.Show("A category with this name already exists.", "Validation Error",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    category.Name = newName;
                    _dbHelper.UpdateCategory(category);
                    CategoriesList.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing category: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = (Button)sender;
                var category = (ProductCategory)button.Tag;

                // Check if category is in use
                if (_dbHelper.IsCategoryInUse(category.Id))
                {
                    MessageBox.Show("Cannot delete this category as it is being used by one or more products.",
                        "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Are you sure you want to delete the category '{category.Name}'?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _dbHelper.DeleteCategory(category.Id);
                    _categories.Remove(category);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting category: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}