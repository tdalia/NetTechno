using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerQueryData;
using CustomerQueryData.Interfaces;
using CustomerQueryData.Models;

namespace CustomerQueryServices
{
    public class CustomerService : ICustomer
    {
        private readonly DataContext _context;

        public CustomerService(DataContext context)
        {
            _context = context;
        }

        #region Cust Exists, GetCust

        public bool CustomerExists(int custId)
        {
            return _context.Customers.Any(c => c.CustomerId == custId);
        }

        public Customer GetCustomer(int custId)
        {
            // NEED TO INCLUDE ALL SURVEYS & QUERIES
            return _context.Customers
                .FirstOrDefault(c => c.CustomerId == custId);                
        }

        #endregion

        #region Add, Update

        public async Task AddCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            //_context.Update(customer);
            Customer c = GetCustomer(customer.CustomerId);
            if (c != null)
            {
                c.FirstName = customer.FirstName;
                c.LastName = customer.LastName;
                c.Email = customer.Email;
                c.UserName = customer.UserName;
                c.Password = customer.Password;
                await _context.SaveChangesAsync();
            }
        }

        #endregion

        // Start Query
        public async Task StartQueryAsync(QueryMaster query)
        {
            query.Status = QueryStatus.New;
            await _context.QueryMasters.AddAsync(query);
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
