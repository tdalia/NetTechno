using CustomerQueryData.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CustomerQueryWebAPI.ViewModels
{
    public class EmployeeViewModel
    {

        public EmployeeViewModel()
        {        }

        public EmployeeViewModel(Employee emp)
        {
            EmployeeId = emp.EmployeeId;
            FirstName = emp.FirstName;
            LastName = emp.LastName;
            Email = emp.Email;
            UserName = emp.UserName;
            Password = emp.Password;
            EmpAvgRating = emp.EmpAvgRating;
            DeptId = emp.DeptId;
            Department = emp.Department;
            RoleId = emp.RoleId;
            Role = emp.Role;
        }

        public int EmployeeId { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("E-Mail")]
        public string Email { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Rating")]
        public int? EmpAvgRating { get; set; }
        [DisplayName("Department")]
        public string DeptName { get; set; }

        public int DeptId { get; set; }
        public Department Department { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public Employee ConvertToEmployee()
        {
            Employee employee = new Employee();
            employee.EmployeeId = EmployeeId;
            employee.FirstName = FirstName;
            employee.LastName = LastName;
            employee.Email = Email;
            employee.UserName = UserName;
            employee.Password = Password;
            employee.DeptId = DeptId;
            employee.Department = Department;
            employee.Role = Role;
            employee.RoleId = RoleId;
            employee.EmpAvgRating = EmpAvgRating;

            return employee;
        }
    }
}
