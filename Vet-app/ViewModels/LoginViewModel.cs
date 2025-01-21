using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VeterinaryManagementSystem.Services;
using VeterinaryManagementSystem.Models;
using System.Threading.Tasks;

namespace VeterinaryManagementSystem.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authService;
        private readonly INavigationService _navigationService;
        private string _name;
        private string _password;
        private string _errorMessage;
        private bool _isLoading;
        private User _currentUser;
        private RelayCommand _loginCommand;  // Declara la variable _loginCommand

        public LoginViewModel(IAuthenticationService authService, INavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;
        }

        public bool IsAuthenticated => _currentUser != null;

        public User CurrentUser
        {
            get => _currentUser;
            private set
            {
                SetProperty(ref _currentUser, value);
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    LoginCommand.NotifyCanExecuteChanged(); // Llamar después de cambiar la propiedad
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                {
                    LoginCommand.NotifyCanExecuteChanged(); // Llamar después de cambiar la propiedad
                }
            }
        }


        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public RelayCommand LoginCommand => _loginCommand ??= new RelayCommand(
            async () => await LoginAsync(),
            () => CanLogin()
        );

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !IsLoading;
        }

        public async Task<bool> LoginAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var (success, message, user) = await _authService.LoginAsync(Name, Password);

                if (!success)
                {
                    ErrorMessage = message;
                    return false;
                }

                CurrentUser = user;
                _navigationService.NavigateToMain();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error al iniciar sesión: " + ex.Message;
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
