using CustomerQueryData.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerQueryData.Interfaces
{
    public interface IProduct : IDisposable
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProduct(int productId);
    }
}
