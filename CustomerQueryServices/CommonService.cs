using CustomerQueryData;
using CustomerQueryData.Interfaces;
using CustomerQueryData.Models;
using System.Collections.Generic;
using System.Linq;

namespace CustomerQueryServices
{
    public class CommonService : ICommon
    {
        private readonly DataContext _context;

        public CommonService(DataContext context)
        {
            _context = context;
        }

        public bool DeptExists(int deptId)
        {
            return _context.Departments.Any(d => d.DeptId == deptId);
        }

        public List<Department> GetAllDepartments()
        {
            return _context.Departments.ToList();
        }

        public List<Role> GetAllRoles()
        {
            return _context.Roles.ToList();
        }   
        
        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
