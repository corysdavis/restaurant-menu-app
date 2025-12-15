Project Overview

The Restaurant Ordering System is a console-based application that allows users to manage customers, menu items, and orders. Data is persisted using a SQLite database.

Features

Add, update, list, and delete customers

Add, update, list, and delete menu items (Food and Drinks)

Create orders by selecting menu items and quantities

View order summaries for each customer

Persistent storage with SQLite

Class Structure

Customer: Represents a customer with a name and display functionality.

MenuItemBase: Abstract base class for menu items; defines name, price, and abstract Display method.

FoodItem: Inherits from MenuItemBase, adds calories, and implements display.

DrinkItem: Inherits from MenuItemBase, adds cold/hot status, and implements display.

Order: Represents a customer’s order with multiple menu items and display functionality.

Database: Handles SQLite operations for customers, menu items, orders, and order items.

IDisplayable: Interface for classes that can display themselves in the console.

Program: Main class that provides the console menu and handles user interactions.
