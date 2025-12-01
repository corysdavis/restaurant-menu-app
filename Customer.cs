/*********************************************
* Name: Cory Davis
* Date: 11/30/2025
* Purpose:
* Represents the customer placing the order.
**********************************************/

public class Customer : IDisplayable
{
    public string Name { get; set; }

    public Customer(string name)
    {
        Name = name;
    }

    public void Display()
    {
        Console.WriteLine($"Customer: {Name}");
    }
}