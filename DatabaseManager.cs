#nullable disable
using Microsoft.Data.Sqlite;
using System.IO;
using System.Collections.Generic;

namespace ModernCafePOS;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
}

public static class DatabaseManager
{
    private static string dbPath = "pos.db";
    private static string connectionString = $"Data Source={dbPath}";

    public static void InitializeDatabase()
    {
        bool isNew = !File.Exists(dbPath);

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Categories (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CategoryId INTEGER,
                    Name TEXT NOT NULL,
                    Price DECIMAL NOT NULL,
                    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
                );
                CREATE TABLE IF NOT EXISTS Tables (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    IsOpen BOOLEAN NOT NULL DEFAULT 0
                );
                CREATE TABLE IF NOT EXISTS Orders (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    TableId INTEGER,
                    TotalAmount DECIMAL,
                    IsPaid BOOLEAN NOT NULL DEFAULT 0,
                    OrderDate DATETIME DEFAULT CURRENT_TIMESTAMP
                );
                CREATE TABLE IF NOT EXISTS OrderDetails (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    OrderId INTEGER,
                    ProductId INTEGER,
                    Quantity INTEGER,
                    Price DECIMAL
                );
                CREATE TABLE IF NOT EXISTS Settings (
                    Key TEXT PRIMARY KEY,
                    Value TEXT
                );
                INSERT OR IGNORE INTO Settings (Key, Value) VALUES ('InitialCash', '0');
                
                -- Başlangıçta hiç masa yoksa 20 tane ekle
                INSERT INTO Tables (Name, IsOpen) 
                SELECT 'MASA ' || n, 0 
                FROM (SELECT 1 as n UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9 UNION SELECT 10 UNION SELECT 11 UNION SELECT 12 UNION SELECT 13 UNION SELECT 14 UNION SELECT 15 UNION SELECT 16 UNION SELECT 17 UNION SELECT 18 UNION SELECT 19 UNION SELECT 20)
                WHERE NOT EXISTS (SELECT 1 FROM Tables);
            ";
            command.ExecuteNonQuery();

            if (isNew)
            {
                SeedData(connection);
            }
            else
            {
                // Mevcut DB için Dondurma kategorisi ve ürünlerini ekle (yoksa)
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = @"
                    INSERT INTO Categories (Name) SELECT 'Dondurma' WHERE NOT EXISTS (SELECT 1 FROM Categories WHERE Name = 'Dondurma');
                    
                    INSERT INTO Products (CategoryId, Name, Price) 
                    SELECT (SELECT Id FROM Categories WHERE Name = 'Dondurma'), 'Çikolatalı Dondurma', 55.00
                    WHERE NOT EXISTS (SELECT 1 FROM Products WHERE Name = 'Çikolatalı Dondurma');
                    
                    INSERT INTO Products (CategoryId, Name, Price) 
                    SELECT (SELECT Id FROM Categories WHERE Name = 'Dondurma'), 'Vanilyalı Dondurma', 55.00
                    WHERE NOT EXISTS (SELECT 1 FROM Products WHERE Name = 'Vanilyalı Dondurma');

                    INSERT INTO Products (CategoryId, Name, Price) 
                    SELECT (SELECT Id FROM Categories WHERE Name = 'Dondurma'), 'Çilekli Limonlu Dondurma', 60.00
                    WHERE NOT EXISTS (SELECT 1 FROM Products WHERE Name = 'Çilekli Limonlu Dondurma');

                    INSERT INTO Products (CategoryId, Name, Price) 
                    SELECT (SELECT Id FROM Categories WHERE Name = 'Dondurma'), 'Fıstıklı Dondurma', 70.00
                    WHERE NOT EXISTS (SELECT 1 FROM Products WHERE Name = 'Fıstıklı Dondurma');

                    INSERT INTO Products (CategoryId, Name, Price) 
                    SELECT (SELECT Id FROM Categories WHERE Name = 'Dondurma'), 'Karışık Dondurma (3 Top)', 80.00
                    WHERE NOT EXISTS (SELECT 1 FROM Products WHERE Name = 'Karışık Dondurma (3 Top)');
                ";
                checkCmd.ExecuteNonQuery();
            }
        }
    }

    private static void SeedData(SqliteConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Categories (Name) VALUES ('Sıcak İçecekler'), ('Soğuk İçecekler'), ('Tatlılar'), ('Ana Yemekler'), ('Aperatifler');
            
            INSERT INTO Products (CategoryId, Name, Price) VALUES 
            -- Sıcak İçecekler (1)
            (1, 'Bardak Çay', 15.00),
            (1, 'Fincan Çay', 25.00),
            (1, 'Filtre Kahve', 45.00),
            (1, 'Türk Kahvesi', 40.00),
            (1, 'Latte', 55.00),
            (1, 'Cappuccino', 55.00),
            (1, 'Espresso', 40.00),
            (1, 'Americano', 50.00),
            (1, 'Mocha', 60.00),
            (1, 'Sıcak Çikolata', 55.00),
            (1, 'Bitki Çayı', 35.00),

            -- Soğuk İçecekler (2)
            (2, 'Limonata', 50.00),
            (2, 'Çilekli Limonata', 60.00),
            (2, 'Ice Latte', 65.00),
            (2, 'Ice Mocha', 70.00),
            (2, 'Kutu Kola', 35.00),
            (2, 'Fanta', 35.00),
            (2, 'Sprite', 35.00),
            (2, 'Buzlu Çay (Şeft)', 40.00),
            (2, 'Buzlu Çay (Limon)', 40.00),
            (2, 'Sıkma Portakal', 75.00),
            (2, 'Su (0.5L)', 15.00),
            (2, 'Maden Suyu', 20.00),

            -- Tatlılar (3)
            (3, 'Tiramisu', 90.00),
            (3, 'San Sebastian', 120.00),
            (3, 'Limonlu Cheesecake', 100.00),
            (3, 'Orman Meyveli Kek', 110.00),
            (3, 'Brownie', 85.00),
            (3, 'Profiterol', 95.00),
            (3, 'Sufle', 105.00),
            (3, 'Marlenka', 90.00),

            -- Ana Yemekler (4)
            (4, 'Köri Soslu Tavuk', 180.00),
            (4, 'Cafe de Paris Tavuk', 195.00),
            (4, 'Spagetti Bolonez', 160.00),
            (4, 'Penne Arrabbiata', 150.00),
            (4, 'Fettuccine Alfredo', 170.00),
            (4, 'Izgara Köfte', 210.00),
            (4, 'Hamburger Menü', 240.00),
            (4, 'Cheeseburger Menü', 260.00),

            -- Aperatifler (5)
            (5, 'Patates Kızartması', 80.00),
            (5, 'Soğan Halkası (8li)', 70.00),
            (5, 'Çıtır Tavuk Sepeti', 140.00),
            (5, 'Sosis Tabağı', 120.00),
            (5, 'Kaşarlı Tost', 75.00),
            (5, 'Karışık Tost', 90.00),
            (5, 'Sigara Böreği', 60.00);
        ";
        command.ExecuteNonQuery();
    }
    
    public static List<Product> GetProducts()
    {
        var products = new List<Product>();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT p.Id, p.Name, p.Price, c.Name as Category 
                FROM Products p 
                JOIN Categories c ON p.CategoryId = c.Id";
            
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Price = reader.GetDecimal(2),
                        Category = reader.GetString(3)
                    });
                }
            }
        }
        return products;
    }

    public static void SaveOrder(string tableName, decimal totalAmount)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Orders (TableId, TotalAmount, IsPaid, OrderDate) VALUES (@tId, @amount, 1, datetime('now','localtime'))";
            
            // "MASA 5" içinden sadece 5 rakamını alalım
            int tId = 0;
            string numStr = tableName.Replace("MASA", "").Trim();
            int.TryParse(numStr, out tId);

            command.Parameters.AddWithValue("@tId", tId);
            command.Parameters.AddWithValue("@amount", totalAmount);
            command.ExecuteNonQuery();
        }
    }

    public static decimal GetDailyTotal()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            // Aynı gün içindeki siparişlerin tutarını topla
            command.CommandText = "SELECT SUM(TotalAmount) FROM Orders WHERE date(OrderDate) = date('now','localtime')";
            
            var result = command.ExecuteScalar();
            if (result != DBNull.Value && result != null)
            {
                return Convert.ToDecimal(result);
            }
        }
        return 0m;
    }

    public static decimal GetTotalRevenue()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            // Tüm zamanların toplamını al
            command.CommandText = "SELECT SUM(TotalAmount) FROM Orders";
            
            var result = command.ExecuteScalar();
            if (result != DBNull.Value && result != null)
            {
                return Convert.ToDecimal(result);
            }
        }
        return 0m;
    }

    public static List<OrderLog> GetRecentOrders()
    {
        var orders = new List<OrderLog>();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, TableId, TotalAmount, OrderDate FROM Orders ORDER BY OrderDate DESC LIMIT 20";
            
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    orders.Add(new OrderLog
                    {
                        Id = reader.GetInt32(0),
                        TableId = reader.GetInt32(1),
                        TotalAmount = reader.GetDecimal(2),
                        OrderDate = reader.GetString(3)
                    });
                }
            }
        }
        return orders;
    }

    public static int GetDailyOrderCount()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Orders WHERE date(OrderDate) = date('now','localtime')";
            
            var result = command.ExecuteScalar();
            if (result != DBNull.Value && result != null)
            {
                return Convert.ToInt32(result);
            }
        }
        return 0;
    }
    public static decimal GetInitialCash()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Value FROM Settings WHERE Key = 'InitialCash'";
            var result = command.ExecuteScalar();
            if (result != null) return decimal.Parse(result.ToString());
        }
        return 0m;
    }

    public static void UpdateInitialCash(decimal amount)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Settings SET Value = @val WHERE Key = 'InitialCash'";
            command.Parameters.AddWithValue("@val", amount.ToString());
            command.ExecuteNonQuery();
        }
    }

    public static int GetTableCount()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Tables";
            return Convert.ToInt32(command.ExecuteScalar());
        }
    }

    public static void AddTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            int nextId = GetTableCount() + 1;
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Tables (Name, IsOpen) VALUES (@name, 0)";
            command.Parameters.AddWithValue("@name", "MASA " + nextId);
            command.ExecuteNonQuery();
        }
    }

    public static void RemoveLastTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            int currentCount = GetTableCount();
            if (currentCount <= 1) return; // En az 1 masa kalmalı

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Tables WHERE Id = (SELECT MAX(Id) FROM Tables)";
            command.ExecuteNonQuery();
        }
    }
    public static List<string> GetCategoryNames()
    {
        var cats = new List<string>();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Name FROM Categories";
            using (var reader = command.ExecuteReader())
                while (reader.Read()) cats.Add(reader.GetString(0));
        }
        return cats;
    }

    public static void SaveProduct(string name, decimal price, string category)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Products (CategoryId, Name, Price) VALUES ((SELECT Id FROM Categories WHERE Name = @cat), @name, @price)";
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@price", price);
            command.Parameters.AddWithValue("@cat", category);
            command.ExecuteNonQuery();
        }
    }

    public static void UpdateProductPrice(int id, decimal price)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Products SET Price = @price WHERE Id = @id";
            command.Parameters.AddWithValue("@price", price);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }

    public static void DeleteProduct(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Products WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }

    public static void AddCategory(string name)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Categories (Name) VALUES (@name)";
            command.Parameters.AddWithValue("@name", name);
            command.ExecuteNonQuery();
        }
    }
}

public class OrderLog
{
    public int Id { get; set; }
    public int TableId { get; set; }
    public decimal TotalAmount { get; set; }
    public string OrderDate { get; set; }
}

