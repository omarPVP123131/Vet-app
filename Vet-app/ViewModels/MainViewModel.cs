using System.Windows.Input;
using System.Windows;
using VeterinaryManagementSystem.Commands;
using VeterinaryManagementSystem.Models;
using VeterinaryManagementSystem.Services;
using VeterinaryManagementSystem.ViewModels;
using VeterinaryManagementSystem.Views;
using VeterinaryManagementSystem;

public class MainViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authService;
    private User _currentUser;
    private ICommand _logoutCommand;

    public User CurrentUser
    {
        get => _currentUser;
        set => SetProperty(ref _currentUser, value);
    }

    public ICommand LogoutCommand => _logoutCommand ??= new RelayCommand(ExecuteLogout);

    public MainViewModel(IAuthenticationService authService)
    {
        _authService = authService;
        // Aquí podrías inicializar datos necesarios
    }

    private void ExecuteLogout(object parameter)
    {
        var result = MessageBox.Show("¿Está seguro que desea cerrar sesión?",
                                       "Cerrar Sesión",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            // Crear LoginViewModel con el servicio de autenticación
            var loginViewModel = new LoginViewModel(_authService);
            var loginWindow = new LoginWindow(loginViewModel);
            loginWindow.Show();

            // Cerrar MainWindow
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainWindow)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
