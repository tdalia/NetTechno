using CustomerQueryData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerQueryWebAPI.ViewModels
{
    public class DeptViewModel
    {
        public DeptViewModel()
        {        }

        public DeptViewModel(Department dept)
        {
            DeptId = dept.DeptId;
            DeptName = dept.DeptName;
            DeptAvgRating = DeptAvgRating;
            Employees = dept.Employees;
            Surveys = dept.Surveys;
        }

        public int DeptId { get; set; }

        [Required]
        public string DeptName { get; set; }

        public int? DeptAvgRating { get; set; }

        // There may be multiple Emp's in 1 Dept; but only 1 dept per emp
        public ICollection<Employee> Employees { get; set; }

        public ICollection<Survey> Surveys { get; set; }
    }
}
