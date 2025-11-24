/*********************************************
* Name: Cory Davis
* Date: 11/23/2025
* Purpose:
*   Demonstrates composition.
*   An Order HAS a Customer and HAS MANY MenuItems.
*   Uses polymorphism when displaying items.
**********************************************/

public class Order : IDisplayable
{
    public Customer CustomerInfo { get; set; }
    public List<MenuItem> Items { get; set; }

    public Order(Customer customer)
    {
        CustomerInfo = customer;
        Items = new List<MenuItem>();
    }

    public void AddItem(MenuItem item)
    {
        Items.Add(item);
    }

    // order details
    public void Display()
    {
        Console.WriteLine($"\nOrder Summary for {CustomerInfo.Name}");
        Console.WriteLine("------------------------------------");
        foreach (var item in Items)
        {
            item.Display();
        }
    }
}