using System.ComponentModel.DataAnnotations;

namespace CustomerQueryData.Models
{
    public class Survey
    {
        [Key]
        public int SurveyId { get; set; }

        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int? DeptId { get; set; }
        public virtual Department Department { get; set; }

        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [Required]
        [Range(1, 5)]
        public int Ratings { get; set; }

        public int? QueryId { get; set; }
    }
}
