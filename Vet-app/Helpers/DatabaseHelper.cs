using System;
using System.Data.SQLite;
using System.IO;

namespace VeterinaryManagementSystem.Helpers
{
    public class DatabaseHelper
    {
        private readonly string _dbPath;

        public DatabaseHelper(string dbPath)
        {
            _dbPath = dbPath;
        }

        public void InitializeDatabase()
        {
            // Si no existe el archivo de la base de datos, se crea
            if (!File.Exists(_dbPath))
            {
                SQLiteConnection.CreateFile(_dbPath); // Crear el archivo de la base de datos
            }

            CreateTables(); // Crear las tablas necesarias
        }

        // Método para obtener la conexión SQLite
        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection($"Data Source={_dbPath};Version=3;");
        }


        private void CreateTables()
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
            {
                connection.Open();
                var command = connection.CreateCommand();


                // Crear la tabla de usuarios si no existe
                command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL,
                Password TEXT NOT NULL,
                Role TEXT NOT NULL,
                Email TEXT NOT NULL,
                IsActive INTEGER NOT NULL,
                CreatedAt DATETIME NOT NULL,
                ProfileImage BLOB
            );
        ";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
