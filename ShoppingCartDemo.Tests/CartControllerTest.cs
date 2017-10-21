using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingCartDemo.Models;
using Moq;
using System.Collections.Generic;
using System.Linq;
using ShoppingCartDemo.Controllers;

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
                Orders.Single(o => o.ID == order.ID).Items = order.Items;
            }
        }

        private FakeOrderRepository _fakeRepository = new FakeOrderRepository();
        private CartController _cartController;

        private Customer customer1 = new Customer { ID = 1 };
        private Customer customer2 = new Customer { ID = 2 };
        private Item item1 = new Item { ID = 1 };
        private Item item2 = new Item { ID = 2 };

        [TestInitialize]
        public void Initialize()
        {
            var customerRepository = new Mock<ICustomerRepository>();
            customerRepository.Setup(c => c.Get(1)).Returns(customer1);
            customerRepository.Setup(c => c.Get(2)).Returns(customer2);

            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(c => c.Get(1)).Returns(item1);
            itemRepository.Setup(c => c.Get(2)).Returns(item2);

            _cartController = new CartController(_fakeRepository, customerRepository.Object, itemRepository.Object);                     
        }

        [TestMethod]
        public void CreateEmptyCart()
        {
            var newOrderID = _cartController.CreateCart(customer1.ID);
            Assert.AreEqual(1, _fakeRepository.Orders.Count());
            Assert.AreEqual(0, _fakeRepository.Orders.Single(o => o.ID == newOrderID).Items);
            Assert.AreEqual(customer1.ID, _fakeRepository.Orders.Single(o => o.ID == newOrderID).Customer.ID);
        }

        [TestMethod]
        public void AddItemsToCart()
        {
            var newOrderID = _cartController.CreateCart(customer1.ID);
            _cartController.AddToCart(newOrderID, item1.ID, 10);
            Assert.AreEqual(1, _fakeRepository.Orders.Count());
            Assert.AreEqual(10, _fakeRepository.Orders.Single(o => o.ID == newOrderID).Items);

            _cartController.AddToCart(newOrderID, item1.ID, 10);
            Assert.AreEqual(20, _fakeRepository.Orders.Single(o => o.ID == newOrderID).Items);
        }

        [TestMethod]
        public void RemoveItemsFromCart()
        {
            var newOrderID = _cartController.CreateCart(customer1.ID);
            _cartController.AddToCart(newOrderID, item1.ID, 10);
            Assert.AreEqual(1, _fakeRepository.Orders.Count());
            Assert.AreEqual(10, _fakeRepository.Orders.Single(o => o.ID == newOrderID).Items);

            _cartController.RemoveFromCart(newOrderID, item1.ID, 5);
            Assert.AreEqual(5, _fakeRepository.Orders.Single(o => o.ID == newOrderID).Items);
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void RemoveTooManyItemsFromCart()
        {

        }

        [TestMethod]
        public void ClearItemsFromCart()
        {

        }

        [TestMethod]
        public void CreateMultipleCartsAddToBothOutOfSequence()
        {

        }

        [TestMethod]
        public void MultipleCustomersCreatingCartsAndUpdatingThem()
        {

        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void RemoveItemFromCartThatIsntThere()
        {

        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void AddToCartThatDoesntExist()
        {

        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void ClearItemsFromCartThatDoesntExist()
        {

        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void CartUpdateToRepositoryFails()
        {

        }
    }
}
