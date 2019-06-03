using System.ComponentModel.DataAnnotations;

namespace CustomerQueryWebAPI.ViewModels
{
    public class QueryFAQViewModel
    {
        public QueryFAQViewModel()
        {       
        }

        public QueryFAQViewModel(int deptId, string question, string solution)
        {
            this.DeptId = deptId;
            this.Question = question;
            this.Solution = solution;
        }

        [Required]
        public int DeptId { get; set; }
        public string Question { get; set; }
        public string Solution { get; set; }
    }
}
