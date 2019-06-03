using CustomerQueryData.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerQueryData
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<QueryAssign> QueryAssigns { get; set; }
        public DbSet<QueryMaster> QueryMasters { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Warranty> Warranties { get; set; }

    }
}
