using CustomerQueryData.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerQueryData.Interfaces
{
    public interface IEmployee : IDisposable
    {
        bool EmployeeExists(string username, string password);
        Employee GetEmployee(string username, string password);

        Task<List<Employee>> GetEmployeesAsync();
        Task<List<Employee>> GetEmployeesOfDeptAsync(int deptId);
        Employee GetEmployee(int id);
        Task<bool> AddEmployeeAsync(Employee employee);
        Task<bool> UpdateEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(Employee employee);
        bool EmployeeExists(int id);
        IEnumerable<Department> GetDepartments();
        IEnumerable<Role> GetRoles();
    }
}
