using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Exceptions
{
    public abstract class NotFoundException(string message) : Exception(message)
    {
        // C# 12
        // Primary CTOR
    }

    public class ProductNotFoundException(int id) : NotFoundException($"Product with Id {id} Not Found")
    {
    }
    public class BasketNotFoundException(string id) : NotFoundException($"Basket with Id {id} Not Found")
    {
    }
}
