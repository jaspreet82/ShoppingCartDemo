using ShoppingCartDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;
using System.Web.Http.Dependencies;
using Unity.Exceptions;
using Unity.AspNet.WebApi;

namespace ShoppingCartDemo
{
    public static class WebApiConfig
    {
        private class TestCustomerRepo : ICustomerRepository
        {
            public Customer Get(int id)
            {
                return new Customer
                {
                    ID = id
                };
            }
        }

        private class TestItemRepo : IItemRepository
        {
            public Item Get(int id)
            {
                return new Item
                {
                    ID = id
                };
            }
        }

        private class TestOrderRepo : IOrderRepository
        {
            public Order Create(Customer customer)
            {
                return new Order(customer, 1);
            }

            public Order Get(int orderID)
            {
                return new Order(new Customer { ID = 1 }, 1);
            }

            public void Update(Order order)
            {
            }
        }

        public static void Register(HttpConfiguration config)
        {
            // TODO: remove this when live!
            var container = new UnityContainer();
            container.RegisterInstance<ICustomerRepository>(new TestCustomerRepo());
            container.RegisterInstance<IItemRepository>(new TestItemRepo());
            container.RegisterInstance<IOrderRepository>(new TestOrderRepo());
            config.DependencyResolver = new UnityDependencyResolver(container);


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }


    }
}

