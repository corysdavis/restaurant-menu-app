/*********************************************
* Name: Cory Davis
* Date: 12/7/2025
* Purpose:
* Demonstrates inheritance from MenuItem and represents a drink item with temperature
**********************************************/

public class DrinkItem : MenuItemBase
{
    private bool IsCold { get; set; }

    // parameterized constructor
    public DrinkItem(string name, decimal price, bool isCold) 
        : base(name, price)
    {
        IsCold = isCold;
    }

    // default constructor
    public DrinkItem() : base("Unknown Drink", 0.0m)
    {
        IsCold = true;
    }

    public override void Display()
    {
        Console.WriteLine($"{Name} - ${Price:F2} | {(IsCold ? "Cold" : "Hot")} Drink");
    }
}