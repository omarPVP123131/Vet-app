using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using VeterinaryManagementSystem.Services;
using VeterinaryManagementSystem.ViewModels;
using VeterinaryManagementSystem.Views;
using VeterinaryManagementSystem.Helpers;
using VeterinaryManagementSystem.Config;

namespace VeterinaryManagementSystem
{
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();

            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            // Usar la conexión centralizada de DatabaseConfig
            var dbHelper = new DatabaseHelper(DatabaseConfig.ConnectionString);
            dbHelper.InitializeDatabase();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Registrar DatabaseHelper con la conexión centralizada
            services.AddSingleton(sp => new DatabaseHelper(DatabaseConfig.ConnectionString));

            // Registrar servicios
            services.AddSingleton<IAuthenticationService>(sp =>
                new AuthenticationService(DatabaseConfig.ConnectionString));

            services.AddSingleton<INavigationService, NavigationService>();

            // Registrar ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainViewModel>();

            // Registrar ventanas
            services.AddTransient<LoginWindow>();
            services.AddTransient<MainWindow>();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loginWindow = serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
            Current.MainWindow = loginWindow;
        }

        public static ServiceProvider ServiceProvider =>
            (Current as App)?.serviceProvider;
    }
}
