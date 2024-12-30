using System.Windows;
using System.Windows.Input;
using VeterinaryManagementSystem.ViewModels;
using VeterinaryManagementSystem.Services;

namespace VeterinaryManagementSystem.Views
{

    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            // Focus en el campo de usuario al iniciar
            Loaded += (s, e) => NameTextBox.Focus();

            // Enable window dragging
            this.MouseDown += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            };
        }


        private void OpenRegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var registrationWindow = new UserRegistrationWindow();
                registrationWindow.Owner = this;
                registrationWindow.ShowDialog();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error al abrir la ventana de registro: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ForgotPassword_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Función de recuperación de contraseña en desarrollo",
                          "Recuperar Contraseña",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
        }
    }
}