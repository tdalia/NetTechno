using CustomerQueryData.Models;
using System;
using System.Collections.Generic;

namespace CustomerQueryData.Interfaces
{
    public interface ICommon : IDisposable
    {
        bool DeptExists(int deptId);
        List<Department> GetAllDepartments();
        List<Role> GetAllRoles();
    }
}
