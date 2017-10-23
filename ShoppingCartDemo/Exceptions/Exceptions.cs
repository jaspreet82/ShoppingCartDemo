using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartDemo.Exceptions
{
    public class TryingToRemoveTooManyItems : Exception
    {
        public TryingToRemoveTooManyItems(string message) : base(message) { }
    }
    
    public class CartDoesNotExistException : Exception
    {
        public CartDoesNotExistException(string message) : base(message) { }
    }

    public class ItemDoesNotExistException: Exception
    {
        public ItemDoesNotExistException(string message) : base(message) { }
    }

    public class RepositoryUpdateException : Exception
    {
        public RepositoryUpdateException(string message) : base(message) { }
    }
}