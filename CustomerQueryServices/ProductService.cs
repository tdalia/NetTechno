using CustomerQueryData;
using CustomerQueryData.Interfaces;
using CustomerQueryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerQueryServices
{
    public class ProductService : IProduct
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }


        public async Task<List<Product>> GetAllProducts()
        {
            List<Product> products = await _context.Products
                .Include(dept => dept.Department)
                .ToListAsync();

            return products;
        }

        public async Task<Product> GetProduct(int productId)
        {
            List<Product> prds = await GetAllProducts();
            return prds.FirstOrDefault(p => p.ProductId == productId);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
