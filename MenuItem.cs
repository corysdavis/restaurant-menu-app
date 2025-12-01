/*********************************************
* Name: Cory Davis
* Date: 11/30/2025
* Purpose:
* Thease class for all menu items and implements the IDisplayable interface.
**********************************************/

public class MenuItem : IDisplayable
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public MenuItem(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public virtual void Display()
    {
        Console.WriteLine($"{Name} - ${Price:F2}");
    }
}