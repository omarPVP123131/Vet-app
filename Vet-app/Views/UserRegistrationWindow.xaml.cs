using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using Dapper;
using VeterinaryManagementSystem.Helpers;
using VeterinaryManagementSystem.Models;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.IO;

namespace VeterinaryManagementSystem.Views
{
    public partial class UserRegistrationWindow : Window
    {
        private byte[] _profileImageData;

        public UserRegistrationWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string email = EmailTextBox.Text;
            string role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string password = PasswordBox.Password; // Ensure PasswordBox is the name of the control

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("El correo electrónico no es válido.");
                return;
            }

            // Utilizamos la contraseña tal cual, sin hacer hashing
            string plainPassword = password;

            using (var connection = new SQLiteConnection($"Data Source=VeterinaryDb.db;Version=3;"))
            {
                connection.Open();
                try
                {
                    // Incluir la imagen en la consulta de inserción
                    connection.Execute(@"
                INSERT INTO Users (Username, Password, Email, Role, IsActive, CreatedAt, ProfileImage)
                VALUES (@Username, @Password, @Email, @Role, @IsActive, @CreatedAt, @ProfileImage)",
                        new
                        {
                            Username = username,
                            Password = plainPassword, // La contraseña en texto claro
                            Email = email,
                            Role = role,
                            IsActive = 1,
                            CreatedAt = DateTime.UtcNow,
                            ProfileImage = _profileImageData.Length == 0 ? DBNull.Value : (object)_profileImageData
                        });
                    MessageBox.Show("Usuario registrado exitosamente.");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al registrar el usuario: {ex.Message}");
                }
            }
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Imagenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Mostrar la imagen seleccionada en el control Image
                var image = new BitmapImage(new Uri(openFileDialog.FileName));
                ProfileImage.Source = image;

                // Convertir la imagen a un arreglo de bytes
                _profileImageData = File.ReadAllBytes(openFileDialog.FileName);
            }
        }


        private void UpdatePlaceholderVisibility()
        {
            var usernamePlaceholder = (TextBlock)FindName("UsernamePlaceholder");
            var emailPlaceholder = (TextBlock)FindName("EmailPlaceholder");
            var passwordPlaceholder = (TextBlock)FindName("PasswordPlaceholder");

            if (usernamePlaceholder != null)
            {
                usernamePlaceholder.Visibility = string.IsNullOrEmpty(UsernameTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
            }
            if (emailPlaceholder != null)
            {
                emailPlaceholder.Visibility = string.IsNullOrEmpty(EmailTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
            }
            if (passwordPlaceholder != null)
            {
                passwordPlaceholder.Visibility = string.IsNullOrEmpty(PasswordBox.Password) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private bool IsValidEmail(string email)
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

        // Aquí podrías agregar la lógica de validación para Username, si es necesario
    }
}
