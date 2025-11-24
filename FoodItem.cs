/*********************************************
* Name: Cory Davis
* Date: 11/23/2025
* Purpose:
* Demonstrates inheritance from MenuItem and represents a food item with calories.
**********************************************/

public class FoodItem : MenuItem
{
    public int Calories { get; set; }

    public FoodItem(string name, decimal price, int calories)
        : base(name, price)
    {
        Calories = calories;
    }

    // Polymorphism: Overriding Display()
    public override void Display()
    {
        Console.WriteLine($"{Name} - ${Price:F2} | {Calories} cal");
    }
}