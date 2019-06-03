
using CustomerQueryData.Models;
using System.ComponentModel.DataAnnotations;

namespace CustomerQueryWebAPI.ViewModels
{
    public class SurveyViewModel
    {
        public SurveyViewModel()
        {        }

        public SurveyViewModel(Survey survey)
        {
            SurveyId = survey.SurveyId;
            CustomerId = survey.CustomerId;
            EmployeeId = survey.EmployeeId;
            DeptId = survey.DeptId;
            Ratings = survey.Ratings;
            QueryId = survey.QueryId;
        }

        [Key]
        public int SurveyId { get; set; }

        [Display(Name = "Customer Name")]
        public int? CustomerId { get; set; }

        [Display(Name = "Department Name")]
        public int? DeptId { get; set; }

        [Display(Name = "Employee Name")]
        public int? EmployeeId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Ratings { get; set; }

        public int? QueryId { get; set; }

        public Survey ConvertToSurvey()
        {
            Survey survey = new Survey();
            survey.CustomerId = this.CustomerId;
            survey.DeptId = this.DeptId;
            survey.EmployeeId = this.EmployeeId;
            survey.Ratings = this.Ratings;
            survey.QueryId = this.QueryId;

            return survey;
        }
    }
}
