using ShoppingCartDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShoppingCartDemo.Exceptions;

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
        
        [HttpPost]
        public void AddToCart(int cartId, int itemId, int quantityToAdd)
        {
            var cart = _orderRepository.Get(cartId);
            var item = _itemRepository.Get(itemId);

            if (cart.Items.TryGetValue(item, out int quantityAlreadyInOrder))
                cart.Items[item] = quantityAlreadyInOrder + quantityToAdd;
            else
                cart.Items.Add(item, quantityToAdd);

            _orderRepository.Update(cart);
        }

        [HttpPost]
        public void RemoveFromCart(int cartId, int itemId, int quantityToRemove)
        {
            var cart = _orderRepository.Get(cartId);
            var item = _itemRepository.Get(itemId);

            if (cart.Items.TryGetValue(item, out int quantityAlreadyInOrder))
            {
                if (quantityAlreadyInOrder > quantityToRemove)
                {
                    cart.Items[item] = quantityAlreadyInOrder - quantityToRemove;
                    _orderRepository.Update(cart);
                }
                else if (quantityAlreadyInOrder == quantityToRemove)
                {
                    cart.Items.Remove(item);
                    _orderRepository.Update(cart);
                }
                else
                    throw new TryingToRemoveTooManyItems($"Trying to remove {quantityToRemove} of item {itemId} from {cartId} but only {quantityAlreadyInOrder} in cart");
            }
            else
                throw new TryingToRemoveTooManyItems($"Item with ID {itemId} is not in cart {cartId} at all");
        }

        [HttpPost]
        public int CreateCart(int customerID)
        {
            var customer = _customerRepository.Get(customerID);
            var order = _orderRepository.Create(customer);
            return order.ID;
        }

        [HttpPost]
        public void ClearCart(int cartId)
        {
            var cart = _orderRepository.Get(cartId);
            cart.Items.Clear();
            _orderRepository.Update(cart);
        }

        [HttpGet]
        public IEnumerable<Order> GetOrdersForCustomer(int customerID)
        {
            var customer = _customerRepository.Get(customerID);
            return (_orderRepository.GetAllOrders(customer));
        }
    }
}
