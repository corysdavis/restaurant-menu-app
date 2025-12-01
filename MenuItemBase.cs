/*********************************************
* Name: Cory Davis
* Date: 11/30/2025
* Assignment: Project
* Abstract base class for all menu items -- Demonstrates abstraction and access specifiers
*********************************************/
public abstract class MenuItemBase : IDisplayable
{
    protected string Name { get; set; }
    protected decimal Price { get; set; }

    // parameterized constructor
    protected MenuItemBase(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    // abstract method --- forces derived classes to implement Display
    public abstract void Display();
}