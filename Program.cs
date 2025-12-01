/*********************************************
* Name: Cory Davis
* Date: 11/30/2025
* Purpose:
* Main entry point for Week 2 project.
**********************************************/

using System;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        Console.Title = "Restaurant Ordering System - Week 3";

        Console.WriteLine("Cory Davis - Week 3: Abstraction, Constructors, Access Specifiers");
        Console.WriteLine("Welcome to the Restaurant Ordering System! \n");
        Console.WriteLine("Press ENTER to continue...");
        Console.ReadLine();

        // customers
        Customer customer = new Customer("Cory Davis");

        // menu items
        FoodItem burger = new FoodItem("Classic Burger", 8.99m, 850);
        FoodItem fries = new FoodItem("French Fries", 3.49m, 420);
        DrinkItem cola = new DrinkItem("Cola", 2.49m, true);

        // copy constructor
        FoodItem burgerCopy = new FoodItem(burger);

        // create the order
        Order order = new Order(customer);
        order.AddItem(burger);
        order.AddItem(fries);
        order.AddItem(cola);

        // display menu items
        List<MenuItemBase> menuItems = new List<MenuItemBase>() { burger, fries, cola };
        Console.WriteLine("Menu Items:\n");
        foreach (var item in menuItems)
        {
            item.Display(); // Polymorphism via abstract base
        }

        // display order
        Console.WriteLine("\n--------------------------------------");
        order.Display();

        Console.WriteLine("\nPress ENTER to exit.");
        Console.ReadLine();
    }
}