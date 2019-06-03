using System.ComponentModel;

namespace CustomerQueryApp.ViewModels.Employee
{
    public class EmployeeIndexListingModel
    {
        public int EmployeeId { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("E-Mail")]
        public string Email { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DisplayName("Rating")]
        public int? EmpAvgRating { get; set; }
        [DisplayName("Department")]
        public string DeptName { get; set; }
    }
}
