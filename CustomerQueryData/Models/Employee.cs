using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerQueryData.Models
{
    public class Employee
    {
        public Employee()
        {
            Surveys = new HashSet<Survey>();
        }

        [Key]
        public int EmployeeId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("E-Mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Rating")]
        public int? EmpAvgRating { get; set; }

        [Required]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Department")]
        public int DeptId { get; set; }
        public virtual Department Department { get; set; }

        public virtual ICollection<Survey> Surveys { get; set; }


    }
}
