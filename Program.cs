/*********************************************
* Name: Cory Davis
* Date: 12/7/2025
* Purpose:
* Restaurant Ordering System using SQLite
**********************************************/


using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    private static Database db;

    public static void Main(string[] args)
    {
        Console.Title = "Restaurant Ordering System - Week 4";

        Console.WriteLine("Cory Davis - Week 4 Project");
        Console.WriteLine();
        Console.WriteLine("Welcome!");
        Console.WriteLine("Enter the number of the option you want and press ENTER.");
        Console.WriteLine();

        // initialize database
        db = new Database();

        var shouldExit = false;
        while (!shouldExit)
        {
            ShowMainMenu();
            var input = Console.ReadLine();
            switch (input)
            {
                case "1": CreateCustomer(); break;
                case "2": ListCustomers(); break;
                case "3": UpdateCustomer(); break;
                case "4": DeleteCustomer(); break;

                case "10": CreateMenuItem(); break;
                case "11": ListMenuItems(); break;
                case "12": UpdateMenuItem(); break;
                case "13": DeleteMenuItem(); break;

                case "20": CreateOrder(); break;
                case "21": ListOrders(); break;
                case "22": UpdateOrderItems(); break;
                case "23": DeleteOrder(); break;

                case "0": shouldExit = true; break;
                default:
                    Console.WriteLine("Unknown option. Try again.");
                    break;
            }
            Console.WriteLine();
        }

        Console.WriteLine("Goodbye!");
    }

    private static void ShowMainMenu()
    {
        Console.WriteLine("---------- Main Menu ----------");
        Console.WriteLine("Customers");
        Console.WriteLine(" 1) Add Customer");
        Console.WriteLine(" 2) List Customers");
        Console.WriteLine(" 3) Update Customer");
        Console.WriteLine(" 4) Delete Customer");
        Console.WriteLine();
        Console.WriteLine("Menu Items");
        Console.WriteLine("10) Add Menu Item");
        Console.WriteLine("11) List Menu Items");
        Console.WriteLine("12) Update Menu Item");
        Console.WriteLine("13) Delete Menu Item");
        Console.WriteLine();
        Console.WriteLine("Orders");
        Console.WriteLine("20) Create Order");
        Console.WriteLine("21) List Orders");
        Console.WriteLine("22) Update Order Items");
        Console.WriteLine("23) Delete Order");
        Console.WriteLine();
        Console.WriteLine("0) Exit");
        Console.Write("Select an option: ");
    }

    // customer handlers
    private static void CreateCustomer()
    {
        Console.Write("Enter customer name: ");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("Name cannot be empty."); return; }
        var id = db.AddCustomer(name.Trim());
        Console.WriteLine($"Customer added with Id {id}");
    }

    private static void ListCustomers()
    {
        var customers = db.GetAllCustomers();
        if (!customers.Any()) { Console.WriteLine("No customers found."); return; }
        Console.WriteLine("Customers:");
        foreach (var c in customers) Console.WriteLine($" {c.Id}) {c.Name}");
    }

    private static void UpdateCustomer()
    {
        ListCustomers();
        Console.Write("Enter customer Id to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid Id."); return; }
        var c = db.GetCustomerById(id);
        if (c == null) { Console.WriteLine("Customer not found."); return; }
        Console.Write($"Enter new name for {c.Value.Name}: ");
        var newName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newName)) { Console.WriteLine("Name cannot be empty."); return; }
        if (db.UpdateCustomer(id, newName.Trim())) Console.WriteLine("Customer updated.");
        else Console.WriteLine("Update failed.");
    }

    private static void DeleteCustomer()
    {
        ListCustomers();
        Console.Write("Enter customer Id to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid Id."); return; }
        if (db.DeleteCustomer(id)) Console.WriteLine("Customer deleted (and related orders cleaned).");
        else Console.WriteLine("Delete failed.");
    }

    // Menu Item handlers
    private static void CreateMenuItem()
    {
        Console.Write("Name: ");
        var name = Console.ReadLine();
        Console.Write("Price (e.g., 4.99): ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price)) { Console.WriteLine("Invalid price."); return; }
        Console.Write("Type (Food/Drink): ");
        var type = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(type)) { Console.WriteLine("Type required."); return; }
        type = type.Trim();
        if (type.Equals("Food", StringComparison.OrdinalIgnoreCase))
        {
            Console.Write("Calories (integer): ");
            if (!int.TryParse(Console.ReadLine(), out int cal)) { Console.WriteLine("Invalid calories."); return; }
            var id = db.AddMenuItem(name.Trim(), price, "Food", cal, null);
            Console.WriteLine($"Food item added with Id {id}");
        }
        else if (type.Equals("Drink", StringComparison.OrdinalIgnoreCase))
        {
            Console.Write("Is it cold? (y/n): ");
            var cold = Console.ReadLine();
            bool isCold = cold?.Trim().ToLower() == "y";
            var id = db.AddMenuItem(name.Trim(), price, "Drink", null, isCold);
            Console.WriteLine($"Drink item added with Id {id}");
        }
        else
        {
            Console.WriteLine("Type must be Food or Drink.");
        }
    }

    private static void ListMenuItems()
    {
        var items = db.GetAllMenuItems();
        if (!items.Any()) { Console.WriteLine("No menu items."); return; }
        Console.WriteLine("Menu Items:");
        foreach (var it in items)
        {
            string extra = it.Type == "Food" ? $"{it.Calories} cal" : (it.IsCold.HasValue ? (it.IsCold.Value ? "Cold" : "Hot") : "");
            Console.WriteLine($" {it.Id}) {it.Name} - ${it.Price:F2} [{it.Type}] {extra}");
        }
    }

    private static void UpdateMenuItem()
    {
        ListMenuItems();
        Console.Write("Enter menu item Id to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid Id."); return; }
        var items = db.GetAllMenuItems();
        var item = items.FirstOrDefault(i => i.Id == id);
        if (item.Id == 0) { Console.WriteLine("Item not found."); return; }

        Console.Write($"New name (blank to keep '{item.Name}'): ");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name)) name = item.Name;

        Console.Write($"New price (current {item.Price:F2}): ");
        var priceInput = Console.ReadLine();
        decimal price = item.Price;
        if (!string.IsNullOrWhiteSpace(priceInput) && !decimal.TryParse(priceInput, out price)) { Console.WriteLine("Invalid price."); return; }

        Console.Write("Type (Food/Drink) (blank to keep): ");
        var type = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(type)) type = item.Type;

        int? calories = item.Calories;
        bool? isCold = item.IsCold;
        if (type.Equals("Food", StringComparison.OrdinalIgnoreCase))
        {
            Console.Write($"Calories (current {(item.Calories.HasValue ? item.Calories.Value.ToString() : "none")}): ");
            var cIn = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(cIn) && int.TryParse(cIn, out int cVal)) calories = cVal;
        }
        else
        {
            Console.Write("Is it cold? (y/n/blank to keep): ");
            var cold = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(cold))
            {
                isCold = cold.Trim().ToLower() == "y";
            }
        }

        if (db.UpdateMenuItem(id, name.Trim(), price, type.Trim(), calories, isCold)) Console.WriteLine("Menu item updated.");
        else Console.WriteLine("Update failed.");
    }

    private static void DeleteMenuItem()
    {
        ListMenuItems();
        Console.Write("Enter menu item Id to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid Id."); return; }
        if (db.DeleteMenuItem(id)) Console.WriteLine("Menu item deleted.");
        else Console.WriteLine("Delete failed.");
    }

    // order handlers
    private static void CreateOrder()
    {
        var customers = db.GetAllCustomers();
        if (!customers.Any()) { Console.WriteLine("No customers - add one first."); return; }
        Console.WriteLine("Customers:");
        foreach (var c in customers) Console.WriteLine($" {c.Id}) {c.Name}");
        Console.Write("Enter customer Id for this order: ");
        if (!int.TryParse(Console.ReadLine(), out int custId)) { Console.WriteLine("Invalid Id."); return; }
        var ci = db.GetCustomerById(custId);
        if (ci == null) { Console.WriteLine("Customer not found."); return; }

        var menu = db.GetAllMenuItems();
        if (!menu.Any()) { Console.WriteLine("No menu items - add some first."); return; }
        Console.WriteLine("Menu Items:");
        foreach (var it in menu) Console.WriteLine($" {it.Id}) {it.Name} - ${it.Price:F2}");

        var selection = new Dictionary<int, int>();
        while (true)
        {
            Console.Write("Enter MenuItem Id to add (or blank to finish): ");
            var inStr = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(inStr)) break;
            if (!int.TryParse(inStr, out int mid)) { Console.WriteLine("Invalid Id."); continue; }
            if (!menu.Any(m => m.Id == mid)) { Console.WriteLine("Menu item not found."); continue; }
            Console.Write("Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0) { Console.WriteLine("Invalid quantity."); continue; }
            if (selection.ContainsKey(mid)) selection[mid] += qty; else selection[mid] = qty;
        }

        if (!selection.Any()) { Console.WriteLine("No items selected."); return; }

        var orderId = db.CreateOrder(custId, selection);
        Console.WriteLine($"Order created with Id {orderId}.");
    }

    private static void ListOrders()
    {
        var orders = db.GetAllOrders();
        if (!orders.Any()) { Console.WriteLine("No orders."); return; }
        var menu = db.GetAllMenuItems().ToDictionary(m => m.Id);
        var customers = db.GetAllCustomers().ToDictionary(c => c.Id, c => c.Name);

        foreach (var o in orders)
        {
            string custName = customers.ContainsKey(o.CustomerId) ? customers[o.CustomerId] : $"Customer {o.CustomerId}";
            Console.WriteLine($"Order {o.OrderId} for {custName} at {o.OrderDate.ToLocalTime():g}");
            foreach (var it in o.Item4)
            {
                var mid = it.MenuItemId;
                var qty = it.Quantity;
                if (menu.ContainsKey(mid))
                {
                    var m = menu[mid];
                    Console.WriteLine($"  - {m.Name} x{qty} @ ${m.Price:F2}");
                }
                else
                {
                    Console.WriteLine($"  - Item {mid} x{qty}");
                }
            }
            Console.WriteLine();
        }
    }

    private static void UpdateOrderItems()
    {
        ListOrders();
        Console.Write("Enter order Id to update: ");
        if (!int.TryParse(Console.ReadLine(), out int oid)) { Console.WriteLine("Invalid Id."); return; }
        var allOrders = db.GetAllOrders();
        if (!allOrders.Any(o => o.OrderId == oid)) { Console.WriteLine("Order not found."); return; }

        var menu = db.GetAllMenuItems();
        var selection = new Dictionary<int, int>();
        while (true)
        {
            Console.Write("Enter MenuItem Id to add to order (or blank to finish): ");
            var inStr = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(inStr)) break;
            if (!int.TryParse(inStr, out int mid)) { Console.WriteLine("Invalid Id."); continue; }
            if (!menu.Any(m => m.Id == mid)) { Console.WriteLine("Menu item not found."); continue; }
            Console.Write("Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0) { Console.WriteLine("Invalid quantity."); continue; }
            if (selection.ContainsKey(mid)) selection[mid] += qty; else selection[mid] = qty;
        }

        if (!selection.Any()) { Console.WriteLine("No items selected - abort."); return; }
        db.UpdateOrderItems(oid, selection);
        Console.WriteLine("Order items updated.");
    }

    private static void DeleteOrder()
    {
        ListOrders();
        Console.Write("Enter order Id to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int oid)) { Console.WriteLine("Invalid Id."); return; }
        if (db.DeleteOrder(oid)) Console.WriteLine("Order deleted.");
        else Console.WriteLine("Delete failed.");
    }
}