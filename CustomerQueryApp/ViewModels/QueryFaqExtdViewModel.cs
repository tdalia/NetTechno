using CustomerQueryWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerQueryApp.ViewModels
{
    public class QueryFaqExtdViewModel
    {
        [Required(ErrorMessage = "Select Department")]
        [Display(Name = "Department")]
        public int DeptId { get; set; }
        public List<QueryFAQViewModel> FAQVMList { get; set; }
    }
}
