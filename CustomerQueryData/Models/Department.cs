using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomerQueryData.Models
{
    public class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
            Surveys = new HashSet<Survey>();
        }

        [Key]
        public int DeptId { get; set; }

        [Required]
        public string DeptName { get; set; }

        public int? DeptAvgRating { get; set; }

        // There may be multiple Emp's in 1 Dept; but only 1 dept per emp
        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<Survey> Surveys { get; set; }
    }
}
