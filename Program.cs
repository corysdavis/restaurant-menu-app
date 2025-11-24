/*********************************************
* Name: Cory Davis
* Date: 11/23/2025
* Purpose:
* Main entry point for Week 2 project.
**********************************************/

using System;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        Console.Title = "Restaurant Ordering System";

        Console.WriteLine("      Week 2 Project - Restaurant Ordering System");
        Console.WriteLine("           By: Cory Davis");
        Console.WriteLine("Welcome to the Restaurant Ordering System!");
        Console.WriteLine("Press ENTER to begin.");
        Console.ReadLine();

        // customer
        Customer customer = new Customer("Cory Davis");

        // menu items
        MenuItem burger = new FoodItem("Classic Burger", 8.99m, 850);
        MenuItem fries = new FoodItem("French Fries", 3.49m, 420);
        MenuItem cola = new DrinkItem("Cola", 2.49m, true);

        // create order
        Order order = new Order(customer);
        order.AddItem(burger);
        order.AddItem(fries);
        order.AddItem(cola);

        // interface list
        Console.WriteLine("Menu Items:\n");

        List<IDisplayable> displayObjects = new List<IDisplayable>()
        {
            burger, fries, cola
        };

        foreach (var obj in displayObjects)
        {
            obj.Display(); 
        }

        // display full order
        Console.WriteLine("\n--------------------------------------");
        order.Display();

        Console.WriteLine("\nPress ENTER to exit.");
        Console.ReadLine();
    }
}