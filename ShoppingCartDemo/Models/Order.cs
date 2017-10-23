using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartDemo.Models
{
    public class Order
    {
        public readonly int ID;

        public readonly Customer Customer;

        public Dictionary<Item, int> Items;

        public Order(Customer customer, int id)
        {
            Customer = customer;
            ID = id;
            Items = new Dictionary<Item, int>();
        }
    }
}