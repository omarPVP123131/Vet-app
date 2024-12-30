using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using VeterinaryManagementSystem.Services;
using VeterinaryManagementSystem.Commands;
using VeterinaryManagementSystem.Views;
using VeterinaryManagementSystem.Models;
using System.Windows.Navigation;

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
        private ICommand _loginCommand;


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
                SetProperty(ref _name, value);
                (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged();
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

        public ICommand LoginCommand => _loginCommand ??= new RelayCommand(
            async param => await LoginAsync(),
            param => CanLogin()
        );

        public LoginViewModel(IAuthenticationService authService)
        {
            _authService = authService;
        }

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
