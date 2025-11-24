/*********************************************
* Name: Cory Davis
* Date: 11/23/2025
* Purpose:
* Dmonstrates inheritance from MenuItem and represents a drink item with temperature.
**********************************************/

public class DrinkItem : MenuItem
{
    public bool IsCold { get; set; }

    public DrinkItem(string name, decimal price, bool isCold)
        : base(name, price)
    {
        IsCold = isCold;
    }

    // Polymorphism: Overriding Display()
    public override void Display()
    {
        Console.WriteLine($"{Name} - ${Price:F2} | {(IsCold ? "Cold" : "Hot")} Drink");
    }
}