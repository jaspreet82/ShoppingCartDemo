# ShoppingCartDemo

Some notes:
- I haven't given too much thought to the HTTP verbs on the CartController methods (I only made them "GET" so I could test using a browser rather than Postman or a similar tool). I've seen some places that use just GET and POST and others that also use other verbs- I don't have strong feelings either way.
- I had a loose idea to use "Cart" before checkout, for orders that had not been finalized and "Order" for those that had. There are many pieces of data (such as delivery date, and payment information) that you might attach to an Order but don't make sense to attach to a cart. I didn't follow through with this idea fully though (I'd need to create a Cart repository and some sort of conversion method to properly implement it). I'm still not sure whether the distinction is that useful.
- I'm not sure it makes sense that a customer might have multiple carts on the go at once. I think this might add confusion for consumers of the API, though there may also be some contexts in which this is a good idea (such as if the API was being used by other systems rather than customers directly).
- I've added some classes/interfaces (such as ICustomerRepository, Customer, etc.,) into the ShoppingCartDemo project that clearly don't belong there, it just saved me creating some extra projects for the sake of this demo.
