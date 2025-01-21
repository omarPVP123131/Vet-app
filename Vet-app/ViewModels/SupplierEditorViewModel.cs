using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System
    .Windows;

public class SupplierEditorViewModel : INotifyPropertyChanged, IDataErrorInfo
{
    private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();
    private string _errorMessage;

    public Supplier Supplier { get; set; }
    public string Title { get; set; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    private readonly Action<Supplier> _onSave;

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
            OnPropertyChanged(nameof(HasError));
        }
    }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public bool IsValid => string.IsNullOrEmpty(Error) && !_errors.Any();

    public string Error => null;

    public string this[string columnName]
    {
        get
        {
            string error = null;
            _errors.Remove(columnName);

            switch (columnName)
            {
                case nameof(Supplier.CompanyName):
                    if (string.IsNullOrWhiteSpace(Supplier?.CompanyName))
                    {
                        error = "El nombre de la empresa es requerido.";
                    }
                    break;

                case nameof(Supplier.ContactName):
                    if (string.IsNullOrWhiteSpace(Supplier?.ContactName))
                    {
                        error = "El nombre del contacto es requerido.";
                    }
                    break;

                case nameof(Supplier.Phone):
                    if (string.IsNullOrWhiteSpace(Supplier?.Phone))
                    {
                        error = "El teléfono es requerido.";
                    }
                    else if (!ValidatePhoneNumber(Supplier.Phone))
                    {
                        error = "El formato del teléfono no es válido.";
                    }
                    break;

                case nameof(Supplier.Email):
                    if (!string.IsNullOrWhiteSpace(Supplier?.Email) && !ValidateEmail(Supplier.Email))
                    {
                        error = "El formato del correo electrónico no es válido.";
                    }
                    break;
            }

            if (error != null)
            {
                _errors[columnName] = error;
            }

            ErrorMessage = _errors.FirstOrDefault().Value;
            OnPropertyChanged(nameof(IsValid));
            return error;
        }
    }

    public SupplierEditorViewModel(Supplier supplier, string title, Action<Supplier> onSave)
    {
        Supplier = supplier ?? new Supplier();
        Title = title;
        _onSave = onSave;

        SaveCommand = new RelayCommand(Save, () => IsValid);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void Save()
    {
        if (!IsValid)
        {
            ErrorMessage = "Por favor, corrija los errores antes de guardar.";
            return;
        }

        try
        {
            _onSave?.Invoke(Supplier);
            CloseWindow();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error al guardar: {ex.Message}";
        }
    }

    private void Cancel() => CloseWindow();

    private void CloseWindow()
    {
        foreach (Window window in Application.Current.Windows)
        {
            if (window.DataContext == this)
            {
                window.Close();
                break;
            }
        }
    }

    private bool ValidatePhoneNumber(string phone)
    {
        // Acepta formatos: (123) 456-7890 o 123-456-7890 o 1234567890
        string pattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
        return Regex.IsMatch(phone, pattern);
    }

    private bool ValidateEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}