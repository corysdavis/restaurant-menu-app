/*********************************************
* Name: Cory Davis
* Date: 11/16/2025
* Assignment: Week 1 Project
* Description:
*   The classes and Main application for a
*   Restaurant Ordering System 
**********************************************/

using System;
using System.Collections.Generic;

public class MenuItem
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public MenuItem(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Name} - ${Price:F2}";
    }
}

// Inheritance -- FoodItem is a MenuItem
public class FoodItem : MenuItem
{
    public int Calories { get; set; }

    public FoodItem(string name, decimal price, int calories)
        : base(name, price)
    {
        Calories = calories;
    }

    public override string ToString()
    {
        return $"{Name} - ${Price:F2} | {Calories} cal";
    }
}

// Inheritance -- DrinkItem is a MenuItem
public class DrinkItem : MenuItem
{
    public bool IsCold { get; set; }

    public DrinkItem(string name, decimal price, bool isCold)
        : base(name, price)
    {
        IsCold = isCold;
    }

    public override string ToString()
    {
        return $"{Name} - ${Price:F2} | {(IsCold ? "Cold" : "Hot")} Drink";
    }
}

// Composition -- Order has a list of MenuItems
public class Order
{
    public string CustomerName { get; set; }

    public List<MenuItem> Items { get; set; }

    public Order(string customerName)
    {
        CustomerName = customerName;
        Items = new List<MenuItem>();
    }

    public void AddItem(MenuItem item)
    {
        Items.Add(item);
    }

    public override string ToString()
    {
        string output = $"Order for {CustomerName}\n";
        output += "--------------------------\n";

        foreach (var item in Items)
        {
            output += item + "\n";
        }

        return output;
    }
}

// main application

public class Program
{
    public static void Main(string[] args)
    {
        Console.Title = "Restaurant Ordering System";

        Console.WriteLine(" Cory Davis - SDC320L Project");
        Console.WriteLine(" Restaurant Ordering System");

        Console.WriteLine("Welcome!");
  
        Console.WriteLine("Press ENTER to continue...");
        Console.ReadLine();

        FoodItem burger = new FoodItem("Classic Burger", 8.99m, 850);
        DrinkItem cola = new DrinkItem("Cola", 2.49m, true);
        FoodItem fries = new FoodItem("French Fries", 3.49m, 420);

        Order customerOrder = new Order("John Doe");
        customerOrder.AddItem(burger);
        customerOrder.AddItem(cola);
        customerOrder.AddItem(fries);

        // display info
        Console.WriteLine("Menu Items:\n");
        Console.WriteLine(burger);
        Console.WriteLine(cola);
        Console.WriteLine(fries);

        Console.WriteLine("\n----------------------------------------------");
        Console.WriteLine("Full Order:\n");
        Console.WriteLine(customerOrder);

        Console.WriteLine("Press ENTER to exit.");
        Console.ReadLine();
    }
}