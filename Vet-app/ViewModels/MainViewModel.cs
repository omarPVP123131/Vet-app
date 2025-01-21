using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;
using VeterinaryManagementSystem.Models;
using VeterinaryManagementSystem.Services;
using VeterinaryManagementSystem.Views;

namespace VeterinaryManagementSystem.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly IAuthenticationService _authService;
        private readonly INavigationService _navigationService;
        private User _currentUser;

        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public ICommand LogoutCommand { get; }

        public MainViewModel(IAuthenticationService authService, INavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;

            // Inicializamos el comando de logout
            LogoutCommand = new RelayCommand(ExecuteLogout);
        }

        private void ExecuteLogout()
        {
            var result = MessageBox.Show("¿Está seguro que desea cerrar sesión?",
                                         "Cerrar Sesión",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Crear LoginViewModel con el servicio de autenticación y navegación
                var loginViewModel = new LoginViewModel(_authService, _navigationService);
                var loginWindow = new LoginWindow(loginViewModel); // Pasando LoginViewModel al constructor
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
}
