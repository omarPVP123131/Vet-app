using System.Data.SQLite;
using System.IO;
using VeterinaryManagementSystem.Models;

namespace VeterinaryManagementSystem.Helpers
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString) || !connectionString.Contains("Data Source"))
            {
                throw new ArgumentException("La cadena de conexión debe contener 'Data Source'.");
            }
            _connectionString = connectionString;
        }


        public void InitializeDatabase()
        {
            var dbPath = GetDatabasePathFromConnectionString();
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath); // Crear la base de datos si no existe
            }
            CreateTables();
        }

        public SQLiteConnection GetConnection()
        {
            try
            {
                // Verificar si la cadena de conexión contiene 'Data Source'
                if (string.IsNullOrEmpty(_connectionString) || !_connectionString.Contains("Data Source"))
                {
                    throw new ArgumentException("La cadena de conexión debe contener 'Data Source' con el nombre del archivo .db.");
                }

                // Crear y devolver la conexión
                return new SQLiteConnection(_connectionString);
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y proporcionar un mensaje de error detallado
                Console.WriteLine($"Error al crear la conexión: {ex.Message}");
                throw;
            }
        }

        private string GetDatabasePathFromConnectionString()
        {
            var builder = new SQLiteConnectionStringBuilder(_connectionString);
            return builder.DataSource; // Obtener la ruta de la base de datos
        }

        private void CreateTables()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
CREATE TABLE IF NOT EXISTS Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL,
    Password TEXT NOT NULL,
    Role TEXT NOT NULL,
    Email TEXT NOT NULL,
    IsActive INTEGER NOT NULL,
    CreatedAt DATETIME NOT NULL,
    LastLogin DATETIME
);

CREATE TABLE IF NOT EXISTS ProductCategories (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Description TEXT
);

CREATE TABLE IF NOT EXISTS Products (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CategoryId INTEGER NOT NULL,
    Name TEXT NOT NULL,
    Description TEXT,
    SKU TEXT UNIQUE,
    Price REAL NOT NULL,
    Cost REAL NOT NULL,
    Stock INTEGER NOT NULL,
    MinimumStock INTEGER NOT NULL,
    IsActive INTEGER NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME,
    FOREIGN KEY (CategoryId) REFERENCES ProductCategories(Id)
);

CREATE TABLE IF NOT EXISTS Suppliers (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CompanyName TEXT NOT NULL,
    ContactName TEXT,
    Email TEXT,
    Phone TEXT,
    Address TEXT,
    TaxId TEXT,
    IsActive INTEGER NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME
);

CREATE TABLE IF NOT EXISTS Purchases (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    SupplierId INTEGER NOT NULL,
    UserId INTEGER NOT NULL,
    PurchaseDate DATETIME NOT NULL,
    Total REAL NOT NULL,
    Status TEXT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE IF NOT EXISTS PurchaseDetails (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    PurchaseId INTEGER NOT NULL,
    ProductId INTEGER NOT NULL,
    Quantity INTEGER NOT NULL,
    UnitPrice REAL NOT NULL,
    FOREIGN KEY (PurchaseId) REFERENCES Purchases(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

CREATE TABLE IF NOT EXISTS Sales (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL,
    SaleDate DATETIME NOT NULL,
    Subtotal REAL NOT NULL,
    Tax REAL NOT NULL,
    Total REAL NOT NULL,
    PaymentMethod TEXT NOT NULL,
    Status TEXT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE IF NOT EXISTS SaleDetails (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    SaleId INTEGER NOT NULL,
    ProductId INTEGER NOT NULL,
    Quantity INTEGER NOT NULL,
    UnitPrice REAL NOT NULL,
    Discount REAL DEFAULT 0,
    FOREIGN KEY (SaleId) REFERENCES Sales(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);";

                    // Ejecutar el comando
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear las tablas: {ex.Message}");
            }
        }

        public IEnumerable<Product> GetProducts()
        {
            try
            {
                var products = new List<Product>();

                using (var connection = GetConnection())
                {
                    connection.Open();
                    var command = new SQLiteCommand("SELECT p.Id, p.Name, p.Description, p.SKU, p.Price, p.Cost, p.Stock, p.MinimumStock, p.IsActive, p.CreatedAt, p.UpdatedAt, p.CategoryId, c.Name AS CategoryName " +
                                                    "FROM Products p " +
                                                    "INNER JOIN ProductCategories c ON p.CategoryId = c.Id", connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = new Product
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                SKU = reader.GetString(reader.GetOrdinal("SKU")),
                                Price = reader.GetDouble(reader.GetOrdinal("Price")),
                                Cost = reader.GetDouble(reader.GetOrdinal("Cost")),
                                Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                                IsActive = reader.GetInt32(reader.GetOrdinal("IsActive")) == 1,
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                                Category = new ProductCategory
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                    Name = reader.GetString(reader.GetOrdinal("CategoryName"))
                                }
                            };

                            products.Add(product);
                        }
                    }
                }

                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los productos: {ex.Message}");
                return new List<Product>();  // Retorna una lista vacía en caso de error
            }
        }

        public IEnumerable<ProductCategory> GetCategories()
        {
            try
            {
                var categories = new List<ProductCategory>();

                using (var connection = GetConnection())
                {
                    connection.Open();
                    var command = new SQLiteCommand("SELECT Id, Name, Description, Type FROM ProductCategories", connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var category = new ProductCategory
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                Type = reader.GetString(reader.GetOrdinal("Type"))
                            };

                            categories.Add(category);
                        }
                    }
                }

                return categories;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las categorías: {ex.Message}");
                return new List<ProductCategory>();  // Retorna una lista vacía en caso de error
            }
        }


        public void AddProduct(Product product)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand("INSERT INTO Products (Name, Description, SKU, Price, Stock, MinimumStock, CategoryId, CreatedAt) VALUES (@Name, @Description, @SKU, @Price, @Stock, @MinimumStock, @CategoryId, @CreatedAt)", connection);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@SKU", product.SKU);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Stock", product.Stock);
                command.Parameters.AddWithValue("@CategoryId", product.Category.Id);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand("UPDATE Products SET Name = @Name, Description = @Description, SKU = @SKU, Price = @Price, Stock = @Stock, MinimumStock = @MinimumStock, CategoryId = @CategoryId, UpdatedAt = @UpdatedAt WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", product.Id);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@SKU", product.SKU);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Stock", product.Stock);
                command.Parameters.AddWithValue("@CategoryId", product.Category.Id);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(int productId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand("DELETE FROM Products WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", productId);
                command.ExecuteNonQuery();
            }
        }
        public void AddCategory(ProductCategory category)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand(@"
            INSERT INTO ProductCategories (Name, Description, Type) 
            VALUES (@Name, @Description, @Type)", connection);

                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@Description", category.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Type", category.Type ?? "VETERINARY");

                command.ExecuteNonQuery();

                // Obtener el ID generado
                command.CommandText = "SELECT last_insert_rowid()";
                category.Id = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void UpdateCategory(ProductCategory category)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand(@"
            UPDATE ProductCategories 
            SET Name = @Name, 
                Description = @Description,
                Type = @Type
            WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", category.Id);
                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@Description", category.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Type", category.Type ?? "VETERINARY");

                command.ExecuteNonQuery();
            }
        }

        public bool IsCategoryInUse(int categoryId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand(@"
            SELECT COUNT(*) 
            FROM Products 
            WHERE CategoryId = @CategoryId", connection);

                command.Parameters.AddWithValue("@CategoryId", categoryId);

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        public void DeleteCategory(int categoryId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand(@"
            DELETE FROM ProductCategories 
            WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", categoryId);
                command.ExecuteNonQuery();
            }
        }

        public bool IsSkuInUse(string sku, int excludeProductId = 0)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand(@"
            SELECT COUNT(*) 
            FROM Products 
            WHERE SKU = @SKU 
            AND Id != @ExcludeId", connection);

                command.Parameters.AddWithValue("@SKU", sku);
                command.Parameters.AddWithValue("@ExcludeId", excludeProductId);

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        public Product GetProductById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand(@"
            SELECT p.*, c.Name AS CategoryName
            FROM Products p
            INNER JOIN ProductCategories c ON p.CategoryId = c.Id
            WHERE p.Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ?
                                null : reader.GetString(reader.GetOrdinal("Description")),
                            SKU = reader.GetString(reader.GetOrdinal("SKU")),
                            Price = reader.GetDouble(reader.GetOrdinal("Price")),
                            Cost = reader.GetDouble(reader.GetOrdinal("Cost")),
                            Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                            Category = new ProductCategory
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                Name = reader.GetString(reader.GetOrdinal("CategoryName"))
                            }
                        };
                    }
                    return null;
                }
            }
        }

        public void AddSupplier(Supplier supplier)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand(@"
            INSERT INTO Suppliers 
            (CompanyName, ContactName, Email, Phone, Address, TaxId, IsActive, CreatedAt, UpdatedAt) 
            VALUES 
            (@CompanyName, @ContactName, @Email, @Phone, @Address, @TaxId, @IsActive, @CreatedAt, @UpdatedAt);", connection);

                command.Parameters.AddWithValue("@CompanyName", supplier.CompanyName);
                command.Parameters.AddWithValue("@ContactName", supplier.ContactName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", supplier.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Phone", supplier.Phone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Address", supplier.Address ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TaxId", supplier.TaxId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", supplier.IsActive ? 1 : 0);
                command.Parameters.AddWithValue("@CreatedAt", supplier.CreatedAt);
                command.Parameters.AddWithValue("@UpdatedAt", supplier.UpdatedAt ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateSupplier(Supplier supplier)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand(@"
            UPDATE Suppliers 
            SET 
                CompanyName = @CompanyName,
                ContactName = @ContactName,
                Email = @Email,
                Phone = @Phone,
                Address = @Address,
                TaxId = @TaxId,
                IsActive = @IsActive,
                UpdatedAt = @UpdatedAt
            WHERE 
                Id = @Id;", connection);

                command.Parameters.AddWithValue("@Id", supplier.Id);
                command.Parameters.AddWithValue("@CompanyName", supplier.CompanyName);
                command.Parameters.AddWithValue("@ContactName", supplier.ContactName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", supplier.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Phone", supplier.Phone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Address", supplier.Address ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TaxId", supplier.TaxId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", supplier.IsActive ? 1 : 0);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteSupplier(int supplierId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand("DELETE FROM Suppliers WHERE Id = @Id;", connection);
                command.Parameters.AddWithValue("@Id", supplierId);
                command.ExecuteNonQuery();
            }
        }


        public IEnumerable<Supplier> GetSuppliers()
        {
            var suppliers = new List<Supplier>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Suppliers;", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        suppliers.Add(new Supplier
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                            ContactName = reader.IsDBNull(reader.GetOrdinal("ContactName")) ? null : reader.GetString(reader.GetOrdinal("ContactName")),
                            Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                            Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                            Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                            TaxId = reader.IsDBNull(reader.GetOrdinal("TaxId")) ? null : reader.GetString(reader.GetOrdinal("TaxId")),
                            IsActive = reader.GetInt32(reader.GetOrdinal("IsActive")) == 1,
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                        });
                    }
                }
            }

            return suppliers;
        }

        public IEnumerable<Client> GetClients()
        {
            try
            {
                var clients = new List<Client>();

                using (var connection = GetConnection())
                {
                    connection.Open();
                    var command = new SQLiteCommand("SELECT * FROM Clients", connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var client = new Client
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DocumentNumber = reader.GetString(reader.GetOrdinal("DocumentNumber")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                                IsActive = reader.GetInt32(reader.GetOrdinal("IsActive")) == 1,
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                            };

                            clients.Add(client);
                        }
                    }
                }

                return clients;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los clientes: {ex.Message}");
                return new List<Client>();
            }
        }

        public void AddClient(Client client)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand(@"
            INSERT INTO Clients (FirstName, LastName, DocumentNumber, Email, Phone, Address, IsActive, CreatedAt)
            VALUES (@FirstName, @LastName, @DocumentNumber, @Email, @Phone, @Address, @IsActive, @CreatedAt)", connection);

                command.Parameters.AddWithValue("@FirstName", client.FirstName);
                command.Parameters.AddWithValue("@LastName", client.LastName);
                command.Parameters.AddWithValue("@DocumentNumber", client.DocumentNumber);
                command.Parameters.AddWithValue("@Email", client.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Phone", client.Phone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Address", client.Address ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", client.IsActive ? 1 : 0);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                command.ExecuteNonQuery();

                // Get the ID of the newly inserted client
                command.CommandText = "SELECT last_insert_rowid()";
                client.Id = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void UpdateClient(Client client)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand(@"
            UPDATE Clients 
            SET FirstName = @FirstName,
                LastName = @LastName,
                DocumentNumber = @DocumentNumber,
                Email = @Email,
                Phone = @Phone,
                Address = @Address,
                IsActive = @IsActive,
                UpdatedAt = @UpdatedAt
            WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", client.Id);
                command.Parameters.AddWithValue("@FirstName", client.FirstName);
                command.Parameters.AddWithValue("@LastName", client.LastName);
                command.Parameters.AddWithValue("@DocumentNumber", client.DocumentNumber);
                command.Parameters.AddWithValue("@Email", client.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Phone", client.Phone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Address", client.Address ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", client.IsActive ? 1 : 0);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteClient(int clientId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand("DELETE FROM Clients WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", clientId);
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Client> SearchClients(string searchText)
        {
            try
            {
                var clients = new List<Client>();
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var command = new SQLiteCommand(@"
                SELECT * FROM Clients 
                WHERE FirstName LIKE @SearchText 
                OR LastName LIKE @SearchText 
                OR DocumentNumber LIKE @SearchText 
                OR Email LIKE @SearchText 
                OR Phone LIKE @SearchText", connection);

                    command.Parameters.AddWithValue("@SearchText", $"%{searchText}%");

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var client = new Client
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DocumentNumber = reader.GetString(reader.GetOrdinal("DocumentNumber")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                                IsActive = reader.GetInt32(reader.GetOrdinal("IsActive")) == 1
                            };

                            clients.Add(client);
                        }
                    }
                }

                return clients;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar clientes: {ex.Message}");
                return new List<Client>();
            }
        }
    }
}