using CustomerQueryWebAPI.ViewModels;

namespace CustomerQueryApp.ViewModels
{
    public class LoginViewModel
    {

        public LoginViewModel()
        {        }

        public LoginViewModel(EmployeeViewModel emp)
        {
            EmployeeId = emp.EmployeeId;
            FirstName = emp.FirstName;
            LastName = emp.LastName;
            Email = emp.Email;
            UserName = emp.UserName;
            Password = emp.Password;
            EmpAvgRating = emp.EmpAvgRating;
            DeptId = emp.DeptId;
            DeptName = emp.Department.DeptName;
            RoleId = emp.RoleId;
            RoleName = emp.Role.RoleName;
        }

        public int EmployeeId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? EmpAvgRating { get; set; }
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
