using CustomerQueryData.Models;
using System;
using System.Threading.Tasks;

namespace CustomerQueryData.Interfaces
{
    public interface ICustomer : IDisposable
    {
        bool CustomerExists(int custId);
        Customer GetCustomer(int custId);
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);

        Task StartQueryAsync(QueryMaster query);


    }
}
