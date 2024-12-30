using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using VeterinaryManagementSystem.Services;
using VeterinaryManagementSystem.ViewModels;
using VeterinaryManagementSystem.Views;
using VeterinaryManagementSystem.Helpers;

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
            // Ruta correcta para la base de datos SQLite
            var dbPath = "VeterinaryDb.db"; // Asegúrate de que el archivo .db esté en el directorio correcto

            // Crea una instancia de la clase DatabaseHelper
            var dbHelper = new DatabaseHelper(dbPath);

            // Llamamos al método para crear la base de datos y las tablas
            dbHelper.InitializeDatabase();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Registrar el servicio de autenticación con la base de datos SQLite
            services.AddSingleton<IAuthenticationService>(sp => new AuthenticationService("VeterinaryDb.db"));

            // Registrar otros servicios
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

            // Obtener la ventana de login con DI
            var loginWindow = serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
            Current.MainWindow = loginWindow;
        }

        public static ServiceProvider ServiceProvider =>
            (Current as App)?.serviceProvider;
    }
}
