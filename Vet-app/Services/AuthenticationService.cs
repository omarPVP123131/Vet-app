using System.Data.SQLite;
using System.Threading.Tasks;
using VeterinaryManagementSystem.Models;
using VeterinaryManagementSystem.Helpers;

namespace VeterinaryManagementSystem.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly string _connectionString;
        private User _currentUser;

        public AuthenticationService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool IsAuthenticated => _currentUser != null;

        public User CurrentUser => _currentUser;

        public async Task<(bool success, string message, User user)> LoginAsync(string username, string password)
        {
            // Simular un pequeño retraso para emular la operación de red
            await Task.Delay(500);

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return (false, "Por favor ingrese nombre y contraseña", null);

            // Comprobar las credenciales contra la base de datos SQLite
            using (var connection = new SQLiteConnection(_connectionString))  // Corregido aquí
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Password, Id, Role, Email FROM Users WHERE Username = @Username";
                command.Parameters.AddWithValue("@Username", username);

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var storedPassword = reader.GetString(0); // Recuperar la contraseña almacenada
                    if (password == storedPassword) // Comparar contraseñas en texto claro
                    {
                        _currentUser = new User
                        {
                            Id = reader.GetInt32(1),
                            Username = username,
                            Role = reader.GetString(2),
                            Email = reader.GetString(3),
                            IsActive = true
                        };

                        return (true, "Login exitoso", _currentUser);
                    }
                }
            }

            return (false, "Usuario o contraseña incorrectos", null);
        }

        public Task<bool> LogoutAsync()
        {
            _currentUser = null;
            return Task.FromResult(true);
        }
    }
}
