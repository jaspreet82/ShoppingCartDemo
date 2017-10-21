using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartDemo.Models
{

    public interface IOrderRepository
    {
        Order Create(Customer customer);

        void Update(Order order);
    }
}
