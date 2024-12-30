using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using VeterinaryManagementSystem.Models;
using VeterinaryManagementSystem.Views;
using VeterinaryManagementSystem.ViewModels;

namespace VeterinaryManagementSystem.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IAuthenticationService _authService;

        public NavigationService(IAuthenticationService authService)
        {
            _authService = authService;
        }

        public void NavigateToMain()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = App.ServiceProvider.GetService<MainWindow>();
                var mainViewModel = App.ServiceProvider.GetService<MainViewModel>();

                if (mainViewModel != null)
                {
                    mainViewModel.CurrentUser = _authService.CurrentUser;
                    mainWindow.DataContext = mainViewModel;
                }

                mainWindow.Show();
                Application.Current.MainWindow?.Close();
                Application.Current.MainWindow = mainWindow;
            });
        }

        public void NavigateToLogin()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var loginWindow = App.ServiceProvider.GetService<LoginWindow>();
                var loginViewModel = App.ServiceProvider.GetService<LoginViewModel>();

                if (loginViewModel != null)
                {
                    loginWindow.DataContext = loginViewModel;
                }

                loginWindow.Show();
                Application.Current.MainWindow?.Close();
                Application.Current.MainWindow = loginWindow;
            });
        }
    }
}