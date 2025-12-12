/*********************************************
* Name: Cory Davis
* Date: 12/7/2025
* Purpose:
* Demonstrates inheritance from MenuItem and represents a food item with calories.
**********************************************/

public class FoodItem : MenuItemBase
{
    private int Calories { get; set; }

    // parameterized constructor
    public FoodItem(string name, decimal price, int calories) 
        : base(name, price)
    {
        Calories = calories;
    }

    // copy constructor
    public FoodItem(FoodItem original) 
        : base(original.Name, original.Price)
    {
        Calories = original.Calories;
    }

    public override void Display()
    {
        Console.WriteLine($"{Name} - ${Price:F2} | {Calories} cal");
    }
}