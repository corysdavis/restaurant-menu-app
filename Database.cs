/*********************************************
* Name: Cory Davis
* Date: 12/7/2025
* Purpose:
* Database class
**********************************************/

using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class Database
{
    private readonly string _connectionString;

    public Database(string databaseFile = "restaurant.db")
    {
        _connectionString = $"Data Source={databaseFile}";
        InitializeDatabase();
    }

    /// <summary>
    /// Create tables if they do not exist.
    /// Tables:
    /// - Customers: Id, Name
    /// - MenuItems: Id, Name, Price, Type ('Food'|'Drink'), Calories (nullable), IsCold (nullable, 0/1)
    /// - Orders: Id, CustomerId, OrderDate
    /// - OrderItems: Id, OrderId, MenuItemId, Quantity
    /// </summary>
    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Customers (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS MenuItems (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Price REAL NOT NULL,
                Type TEXT NOT NULL,
                Calories INTEGER,
                IsCold INTEGER
            );

            CREATE TABLE IF NOT EXISTS Orders (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CustomerId INTEGER NOT NULL,
                OrderDate TEXT NOT NULL,
                FOREIGN KEY(CustomerId) REFERENCES Customers(Id)
            );

            CREATE TABLE IF NOT EXISTS OrderItems (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                OrderId INTEGER NOT NULL,
                MenuItemId INTEGER NOT NULL,
                Quantity INTEGER NOT NULL,
                FOREIGN KEY(OrderId) REFERENCES Orders(Id),
                FOREIGN KEY(MenuItemId) REFERENCES MenuItems(Id)
            );
        ";
        cmd.ExecuteNonQuery();
    }

    // customer CRUD
    public int AddCustomer(string name)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "INSERT INTO Customers(Name) VALUES($name); SELECT last_insert_rowid();";
        cmd.Parameters.AddWithValue("$name", name);
        var id = (long)cmd.ExecuteScalar();
        return (int)id;
    }

    public List<(int Id, string Name)> GetAllCustomers()
    {
        var list = new List<(int, string)>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Id, Name FROM Customers ORDER BY Name;";
        using var rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            list.Add((rdr.GetInt32(0), rdr.GetString(1)));
        }
        return list;
    }

    public (int Id, string Name)? GetCustomerById(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Id, Name FROM Customers WHERE Id = $id;";
        cmd.Parameters.AddWithValue("$id", id);
        using var rdr = cmd.ExecuteReader();
        if (rdr.Read())
            return (rdr.GetInt32(0), rdr.GetString(1));
        return null;
    }

    public bool UpdateCustomer(int id, string newName)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "UPDATE Customers SET Name = $name WHERE Id = $id;";
        cmd.Parameters.AddWithValue("$name", newName);
        cmd.Parameters.AddWithValue("$id", id);
        return cmd.ExecuteNonQuery() > 0;
    }

    public bool DeleteCustomer(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cleanup1 = connection.CreateCommand();
        cleanup1.CommandText = @"
            DELETE FROM OrderItems WHERE OrderId IN (SELECT Id FROM Orders WHERE CustomerId = $id);
            DELETE FROM Orders WHERE CustomerId = $id;";
        cleanup1.Parameters.AddWithValue("$id", id);
        cleanup1.ExecuteNonQuery();

        var cmd = connection.CreateCommand();
        cmd.CommandText = "DELETE FROM Customers WHERE Id = $id;";
        cmd.Parameters.AddWithValue("$id", id);
        return cmd.ExecuteNonQuery() > 0;
    }

    // MenuItem CRUD 
    public int AddMenuItem(string name, decimal price, string type, int? calories = null, bool? isCold = null)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO MenuItems (Name, Price, Type, Calories, IsCold)
            VALUES ($name, $price, $type, $calories, $isCold);
            SELECT last_insert_rowid();";
        cmd.Parameters.AddWithValue("$name", name);
        cmd.Parameters.AddWithValue("$price", Convert.ToDouble(price));
        cmd.Parameters.AddWithValue("$type", type);
        cmd.Parameters.AddWithValue("$calories", calories.HasValue ? (object)calories.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$isCold", isCold.HasValue ? (object)(isCold.Value ? 1 : 0) : DBNull.Value);
        var id = (long)cmd.ExecuteScalar();
        return (int)id;
    }

    public List<(int Id, string Name, decimal Price, string Type, int? Calories, bool? IsCold)> GetAllMenuItems()
    {
        var list = new List<(int, string, decimal, string, int?, bool?)>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Id, Name, Price, Type, Calories, IsCold FROM MenuItems ORDER BY Type, Name;";
        using var rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            int id = rdr.GetInt32(0);
            string name = rdr.GetString(1);
            decimal price = Convert.ToDecimal(rdr.GetDouble(2));
            string type = rdr.GetString(3);
            int? calories = rdr.IsDBNull(4) ? null : rdr.GetInt32(4);
            bool? isCold = rdr.IsDBNull(5) ? null : (rdr.GetInt32(5) == 1);
            list.Add((id, name, price, type, calories, isCold));
        }
        return list;
    }

    public bool UpdateMenuItem(int id, string name, decimal price, string type, int? calories, bool? isCold)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            UPDATE MenuItems
            SET Name = $name, Price = $price, Type = $type, Calories = $calories, IsCold = $isCold
            WHERE Id = $id;";
        cmd.Parameters.AddWithValue("$name", name);
        cmd.Parameters.AddWithValue("$price", Convert.ToDouble(price));
        cmd.Parameters.AddWithValue("$type", type);
        cmd.Parameters.AddWithValue("$calories", calories.HasValue ? (object)calories.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$isCold", isCold.HasValue ? (object)(isCold.Value ? 1 : 0) : DBNull.Value);
        cmd.Parameters.AddWithValue("$id", id);
        return cmd.ExecuteNonQuery() > 0;
    }

    public bool DeleteMenuItem(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        var cleanup = connection.CreateCommand();
        cleanup.CommandText = "DELETE FROM OrderItems WHERE MenuItemId = $id;";
        cleanup.Parameters.AddWithValue("$id", id);
        cleanup.ExecuteNonQuery();

        var cmd = connection.CreateCommand();
        cmd.CommandText = "DELETE FROM MenuItems WHERE Id = $id;";
        cmd.Parameters.AddWithValue("$id", id);
        return cmd.ExecuteNonQuery() > 0;
    }

    // orders CRUD
    public int CreateOrder(int customerId, Dictionary<int, int> menuItemIdToQuantity)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();

        var cmdInsertOrder = connection.CreateCommand();
        cmdInsertOrder.CommandText = "INSERT INTO Orders (CustomerId, OrderDate) VALUES ($cid, $odate); SELECT last_insert_rowid();";
        cmdInsertOrder.Parameters.AddWithValue("$cid", customerId);
        cmdInsertOrder.Parameters.AddWithValue("$odate", DateTime.UtcNow.ToString("o"));
        var orderId = (long)cmdInsertOrder.ExecuteScalar();

        foreach (var kvp in menuItemIdToQuantity)
        {
            var insertItem = connection.CreateCommand();
            insertItem.CommandText = "INSERT INTO OrderItems (OrderId, MenuItemId, Quantity) VALUES ($oid, $mid, $qty);";
            insertItem.Parameters.AddWithValue("$oid", orderId);
            insertItem.Parameters.AddWithValue("$mid", kvp.Key);
            insertItem.Parameters.AddWithValue("$qty", kvp.Value);
            insertItem.ExecuteNonQuery();
        }

        transaction.Commit();
        return (int)orderId;
    }

    // read orders
    public List<(int OrderId, int CustomerId, DateTime OrderDate, List<(int MenuItemId, int Quantity)>)> GetAllOrders()
    {
        var result = new List<(int, int, DateTime, List<(int, int)>)>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Id, CustomerId, OrderDate FROM Orders ORDER BY OrderDate DESC;";
        using var rdr = cmd.ExecuteReader();
        var orders = new List<(int, int, DateTime)>();
        while (rdr.Read())
        {
            orders.Add((rdr.GetInt32(0), rdr.GetInt32(1), DateTime.Parse(rdr.GetString(2))));
        }

        foreach (var o in orders)
        {
            var items = new List<(int, int)>();
            var cmdItems = connection.CreateCommand();
            cmdItems.CommandText = "SELECT MenuItemId, Quantity FROM OrderItems WHERE OrderId = $oid;";
            cmdItems.Parameters.AddWithValue("$oid", o.Item1);
            using var rdr2 = cmdItems.ExecuteReader();
            while (rdr2.Read())
            {
                items.Add((rdr2.GetInt32(0), rdr2.GetInt32(1)));
            }
            result.Add((o.Item1, o.Item2, o.Item3, items));
        }

        return result;
    }

    public bool DeleteOrder(int orderId)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cleanup = connection.CreateCommand();
        cleanup.CommandText = "DELETE FROM OrderItems WHERE OrderId = $oid;";
        cleanup.Parameters.AddWithValue("$oid", orderId);
        cleanup.ExecuteNonQuery();

        var cmd = connection.CreateCommand();
        cmd.CommandText = "DELETE FROM Orders WHERE Id = $oid;";
        cmd.Parameters.AddWithValue("$oid", orderId);
        return cmd.ExecuteNonQuery() > 0;
    }

    // Simple update: replace whole set of items for an order
    public bool UpdateOrderItems(int orderId, Dictionary<int, int> menuItemIdToQuantity)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();
        var del = connection.CreateCommand();
        del.CommandText = "DELETE FROM OrderItems WHERE OrderId = $oid;";
        del.Parameters.AddWithValue("$oid", orderId);
        del.ExecuteNonQuery();

        foreach (var kvp in menuItemIdToQuantity)
        {
            var insertItem = connection.CreateCommand();
            insertItem.CommandText = "INSERT INTO OrderItems (OrderId, MenuItemId, Quantity) VALUES ($oid, $mid, $qty);";
            insertItem.Parameters.AddWithValue("$oid", orderId);
            insertItem.Parameters.AddWithValue("$mid", kvp.Key);
            insertItem.Parameters.AddWithValue("$qty", kvp.Value);
            insertItem.ExecuteNonQuery();
        }
        transaction.Commit();
        return true;
    }
}