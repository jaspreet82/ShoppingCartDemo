using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ShoppingCartDemo.Client
{
    public class ClientExample
    {
        /// <summary>
        /// Log-in or create a new user either way, they should then have a customer ID  
        /// For our purposes, return a dummy one (pardon the pun).
        /// </summary>
        /// <returns></returns>
        public int LoginOrSignup()
        {
            return 1;
        }

        public int CreateCart(HttpClient client, int customerID)
        {
            return int.Parse(client.GetAsync($@"api/createCart/{customerID}").Result.Content.ToString());
        }

        public void ExampleJourney()
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53963/");

            int customerID = LoginOrSignup();

            int cartID = CreateCart(client, customerID);

            bool checkOut = false;

            while (!checkOut)
            {
                // Allow more items to be added to the cart
                client.GetAsync($@"api/RemoveFromCart/{cartID}/{1}/{new Random().Next(5)}");
            }

            // Now handle checkout process
        }
    }
}
