using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartDemo.Models
{
    public interface IItemRepository
    {
        Item Get(int id);
    }
}
