/*********************************************
* Name: Cory Davis
* Date: 11/30/2025
* Purpose:
*   Demonstrates composition.
*   An Order HAS a Customer and HAS MANY MenuItems.
*   Uses polymorphism when displaying items.
**********************************************/

public class Order : IDisplayable
{
    public Customer CustomerInfo { get; set; }
    public List<MenuItemBase> Items { get; set; } 
    public Order(Customer customer)
    {
        CustomerInfo = customer;
        Items = new List<MenuItemBase>();
    }

    public void AddItem(MenuItemBase item) 
    {
        Items.Add(item);
    }

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