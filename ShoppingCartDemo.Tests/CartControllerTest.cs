using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingCartDemo.Models;
using Moq;
using System.Collections.Generic;
using System.Linq;
using ShoppingCartDemo.Controllers;
using ShoppingCartDemo.Exceptions;

namespace ShoppingCartDemo.Tests
{
    [TestClass]
    public class CartControllerTest
    {
        private class FakeOrderRepository : IOrderRepository
        {
            public List<Order> Orders = new List<Order>();
            int id = 1;

            Order IOrderRepository.Create(Customer customer)
            {
                var newOrder = new Order(customer, id);
                Orders.Add(newOrder);
                id++;
                return newOrder;
            }

            void IOrderRepository.Update(Order order)
            {
                var orderToUpdate = Orders.FirstOrDefault(o => o.ID == order.ID);
                if (orderToUpdate == null)
                    throw new CartDoesNotExistException($"No cart matches ID {order.ID}");

                orderToUpdate.Items = order.Items;
            }

            Order IOrderRepository.Get(int orderID)
            {
                var order =  Orders.FirstOrDefault(o => o.ID == orderID);
                if (order == null)
                    throw new CartDoesNotExistException($"No cart matches ID {orderID}");
                return order;
            }
        }

        private FakeOrderRepository _fakeRepository = new FakeOrderRepository();
        private CartController _cartController;
        private Mock<ICustomerRepository> _customerRepository;
        private Mock<IItemRepository> _itemRepository;

        private Customer customer1 = new Customer { ID = 1 };
        private Customer customer2 = new Customer { ID = 2 };
        private Item item1 = new Item { ID = 1 };
        private Item item2 = new Item { ID = 2 };

        [TestInitialize]
        public void Initialize()
        {
            _customerRepository = new Mock<ICustomerRepository>();
            _customerRepository.Setup(c => c.Get(1)).Returns(customer1);
            _customerRepository.Setup(c => c.Get(2)).Returns(customer2);

            _itemRepository = new Mock<IItemRepository>();
            _itemRepository.Setup(c => c.Get(1)).Returns(item1);
            _itemRepository.Setup(c => c.Get(2)).Returns(item2);
            _itemRepository.Setup(c => c.Get(3)).Throws(new ItemDoesNotExistException("No item exists matching ID 3"));

            _cartController = new CartController(_fakeRepository, _customerRepository.Object, _itemRepository.Object);                     
        }

        [TestMethod]
        public void CreateEmptyCart()
        {
            var newOrderID = _cartController.CreateCart(customer1.ID);
            Assert.AreEqual(1, _fakeRepository.Orders.Count());
            Assert.AreEqual(0, _fakeRepository.Orders.Single(o => o.ID == newOrderID).Items.Count);
            Assert.AreEqual(customer1.ID, _fakeRepository.Orders.Single(o => o.ID == newOrderID).Customer.ID);
        }

        [TestMethod]
        public void AddItemsToCart()
        {
            var newOrderID = _cartController.CreateCart(customer1.ID);
            _cartController.AddToCart(newOrderID, item1.ID, 10);
            Assert.AreEqual(1, _fakeRepository.Orders.Count());
            Assert.AreEqual(10, _fakeRepository.Orders.Single(o => o.ID == newOrderID).Items[item1]);

            _cartController.AddToCart(newOrderID, item1.ID, 10);

            Assert.AreEqual(20, _fakeRepository.Orders.Single(o => o.ID == newOrderID).Items[item1]);
        }

        [TestMethod]
        public void RemoveSomeItemsFromCart()
        {
            var newOrderID = _cartController.CreateCart(customer1.ID);
            _cartController.AddToCart(newOrderID, item1.ID, 10);
            Assert.AreEqual(1, _fakeRepository.Orders.Count());
            Assert.AreEqual(10, _fakeRepository.Orders.Single(o => o.ID == newOrderID).Items[item1]);

            _cartController.RemoveFromCart(newOrderID, item1.ID, 5);
            Assert.AreEqual(5, _fakeRepository.Orders.Single(o => o.ID == newOrderID).Items[item1]);
        }

        [TestMethod]
        public void RemoveAllItemsFromCart()
        {
            var newOrderID = _cartController.CreateCart(customer1.ID);
            _cartController.AddToCart(newOrderID, item1.ID, 10);
            _cartController.RemoveFromCart(newOrderID, item1.ID, 10);
            Assert.AreEqual(0, _fakeRepository.Orders.Single(o => o.ID == newOrderID).Items.Count);
        }


        [TestMethod, ExpectedException(typeof(TryingToRemoveTooManyItems))]
        public void RemoveTooManyItemsFromCart()
        {
            var newOrderID = _cartController.CreateCart(customer1.ID);
            _cartController.AddToCart(newOrderID, item1.ID, 10);

            _cartController.RemoveFromCart(newOrderID, item1.ID, 20);
        }

        [TestMethod]
        public void ClearItemsFromCart()
        {
            var newOrderID = _cartController.CreateCart(customer1.ID);
            _cartController.AddToCart(newOrderID, item1.ID, 10);
            _cartController.AddToCart(newOrderID, item2.ID, 10);

            _cartController.ClearCart(newOrderID);
            Assert.AreEqual(0, _fakeRepository.Orders.Single(o => o.ID == newOrderID).Items.Count);
        }

        [TestMethod]
        public void MultipleCustomersCreatingCartsAndUpdatingThem()
        {
            var newCart1ID = _cartController.CreateCart(customer1.ID);
            var newCart2ID = _cartController.CreateCart(customer1.ID);
            var newCart3ID = _cartController.CreateCart(customer2.ID);

            _cartController.AddToCart(newCart3ID, item1.ID, 5);
            _cartController.AddToCart(newCart1ID, item1.ID, 10);
            _cartController.AddToCart(newCart2ID, item2.ID, 10);
            _cartController.AddToCart(newCart1ID, item1.ID, 10);
            _cartController.AddToCart(newCart2ID, item2.ID, 5);

            Assert.AreEqual(3, _fakeRepository.Orders.Count);
            Assert.AreEqual(20, _fakeRepository.Orders.Single(o => o.ID == newCart1ID).Items[item1]);
            Assert.AreEqual(15, _fakeRepository.Orders.Single(o => o.ID == newCart2ID).Items[item2]);
            Assert.AreEqual(5, _fakeRepository.Orders.Single(o => o.ID == newCart3ID).Items[item1]);
        }

        [TestMethod]
        public void 


        [TestMethod, ExpectedException(typeof(CartDoesNotExistException))]
        public void RemoveItemFromCartThatIsntThere()
        {
            _cartController.RemoveFromCart(3, item1.ID, 20);
        }

        [TestMethod, ExpectedException(typeof(ItemDoesNotExistException))]
        public void RemoveItemThatDoesNotExist()
        {
            var newOrderID = _cartController.CreateCart(customer1.ID);
            _cartController.RemoveFromCart(newOrderID, 3, 20);
        }

        [TestMethod, ExpectedException(typeof(CartDoesNotExistException))]
        public void AddToCartThatDoesntExist()
        {
            _cartController.AddToCart(1, item1.ID, 5);
        }

        [TestMethod, ExpectedException(typeof(CartDoesNotExistException))]
        public void ClearItemsFromCartThatDoesntExist()
        {
            _cartController.ClearCart(1);
        }

        [TestMethod, ExpectedException(typeof(RepositoryUpdateException))]
        public void CartUpdateToRepositoryFails()
        {
            var failingRepo = new Mock<IOrderRepository>();
            failingRepo.Setup(r => r.Create(It.IsAny<Customer>())).Returns(new Order(new Customer(), 1));
            failingRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(new Order(new Customer(), 1));
            failingRepo.Setup(r => r.Update(It.IsAny<Order>())).Throws(new RepositoryUpdateException("Exception thrown by repository"));

            _cartController = new CartController(failingRepo.Object, _customerRepository.Object, _itemRepository.Object);

            _cartController.AddToCart(1, item1.ID, 5);
        }
    }
}
