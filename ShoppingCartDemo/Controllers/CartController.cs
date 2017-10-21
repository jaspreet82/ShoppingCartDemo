using ShoppingCartDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShoppingCartDemo.Controllers
{
    public class CartController : ApiController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IItemRepository _itemRepository;

        public CartController(IOrderRepository orderRepository, ICustomerRepository customerRepository, IItemRepository itemRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _itemRepository = itemRepository;
        }
        
        public void AddToCart(int cartId, int itemId, int quantity)
        {

        }

        public void RemoveFromCart(int cartId, int itemId, int quantity)
        {

        }

        public int CreateCart(int customerID)
        {
            var customer = _customerRepository.Get(customerID);
            var order = _orderRepository.Create(customer);
            return order.ID;
        }

        public void ClearCart(int cartId)
        {
        }
    }
}
