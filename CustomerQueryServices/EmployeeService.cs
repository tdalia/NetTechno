using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CustomerQueryData;
using CustomerQueryData.Interfaces;
using CustomerQueryData.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace CustomerQueryServices
{
    public class EmployeeService: IEmployee
    {
        private readonly DataContext _context;

        public EmployeeService(DataContext context)
        {
            _context = context;
        }

        #region LOGIN

        public bool EmployeeExists(string username, string password)
        {
            return _context.Employees.Any(e => e.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                e.Password.Equals(password) );
        }

        public Employee GetEmployee(string username, string password)
        {
            Employee emp = null;

            if (EmployeeExists(username, password))
            {
                emp = _context.Employees
                        .Include(dept => dept.Department)
                        .Include(role => role.Role)
                        .FirstOrDefault(e => e.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                                        e.Password.Equals(password));
            }

            return emp;
        }

        #endregion


        public async Task<bool> AddEmployeeAsync(Employee employee)
        {
            EntityEntry<Employee> e = await _context.AddAsync(employee);
            await _context.SaveChangesAsync();
            return e.State == EntityState.Added ? true : false;
        }

        public async Task<bool> DeleteEmployeeAsync(Employee employee)
        {
            EntityEntry<Employee> e = _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return e.State == EntityState.Deleted ? true : false;
        }

        public bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }

        public Employee GetEmployee(int id)
        {
            return _context.Employees
                .Include(dept => dept.Department)
                .Include(role => role.Role)
                .FirstOrDefault(emp => emp.EmployeeId == id);
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _context.Employees.Include(e => e.Department)
                .Include(r => r.Role)
                .ToListAsync<Employee>();
        }

        public async Task<List<Employee>> GetEmployeesOfDeptAsync(int deptId)
        {
            return await _context.Employees.Include(e => e.Department)
                .Include(r => r.Role)
                .Where(e => e.DeptId == deptId)
                .ToListAsync<Employee>();
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            EntityEntry<Employee> e = _context.Update(employee);
            await _context.SaveChangesAsync();

            return e.State == EntityState.Added ? true : false;
        }

        public IEnumerable<Department> GetDepartments()
        {
            return _context.Departments;
        }

        public IEnumerable<Role> GetRoles()
        {
            return _context.Roles;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
