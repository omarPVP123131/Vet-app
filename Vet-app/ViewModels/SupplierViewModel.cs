using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using VeterinaryManagementSystem.Helpers;
using VeterinaryManagementSystem.Models;
using CommunityToolkit.Mvvm.Input;
using VeterinaryManagementSystem.Views.Proveedores;
using System.Windows;
using VeterinaryManagementSystem.ViewModels;
using VeterinaryManagementSystem.Services;

namespace VeterinaryManagementSystem.ViewModels
{
    public class SuppliersViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly DialogService _dialogService;
        private readonly AnimationService _animationService;

        public SuppliersViewModel(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
            _animationService = new AnimationService();
            _dialogService = new DialogService(_animationService);
            LoadSuppliers();

            // Initialize commands
            SearchCommand = new RelayCommand(SearchSuppliers);
            AddSupplierCommand = new RelayCommand(AddSupplier);
            UpdateSupplierCommand = new RelayCommand(UpdateSupplier, () => SelectedSupplier != null);
            DeleteSupplierCommand = new RelayCommand(DeleteSupplier, () => SelectedSupplier != null);
        }

        #region Properties

        public ObservableCollection<Supplier> Suppliers { get; set; } = new ObservableCollection<Supplier>();

        private Supplier _selectedSupplier;
        public Supplier SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                if (_selectedSupplier != value)
                {
                    _selectedSupplier = value;
                    OnPropertyChanged(nameof(SelectedSupplier));
                    OnPropertyChanged(nameof(CanEditOrDelete));
                    (UpdateSupplierCommand as RelayCommand)?.NotifyCanExecuteChanged();
                    (DeleteSupplierCommand as RelayCommand)?.NotifyCanExecuteChanged();
                }
            }
        }

        public string SearchText { get; set; }

        public bool CanEditOrDelete => SelectedSupplier != null;

        #endregion

        #region Commands

        public ICommand SearchCommand { get; }
        public ICommand AddSupplierCommand { get; }
        public ICommand UpdateSupplierCommand { get; }
        public ICommand DeleteSupplierCommand { get; }

        #endregion

        #region Methods

        private void LoadSuppliers()
        {
            Suppliers.Clear();
            foreach (var supplier in _databaseHelper.GetSuppliers())
            {
                Suppliers.Add(supplier);
            }
        }

        private void SearchSuppliers()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadSuppliers();
                return;
            }

            Suppliers.Clear();
            foreach (var supplier in _databaseHelper.GetSuppliers().Where(s =>
                s.CompanyName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                s.ContactName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                s.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase)))
            {
                Suppliers.Add(supplier);
            }
        }

        private void AddSupplier()
        {
            var newSupplier = new Supplier
            {
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            OpenEditor(newSupplier, "Agregar Proveedor", supplier =>
            {
                _databaseHelper.AddSupplier(supplier);
                Suppliers.Add(supplier);
            });
        }

        private void UpdateSupplier()
        {
            if (SelectedSupplier != null)
            {
                OpenEditor(SelectedSupplier, "Editar Proveedor", supplier =>
                {
                    _databaseHelper.UpdateSupplier(supplier);
                    LoadSuppliers();
                });
            }
        }

        private void OpenEditor(Supplier supplier, string title, Action<Supplier> onSave)
        {
            var editorViewModel = new SupplierEditorViewModel(supplier, title, onSave, _dialogService, _animationService);
            var editorView = new SupplierEditorView
            {
                DataContext = editorViewModel,
                Owner = Application.Current.MainWindow
            };

            editorView.ShowDialog();
        }

        private void DeleteSupplier()
        {
            if (SelectedSupplier != null)
            {
                _databaseHelper.DeleteSupplier(SelectedSupplier.Id);
                Suppliers.Remove(SelectedSupplier);
                SelectedSupplier = null;
            }
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}