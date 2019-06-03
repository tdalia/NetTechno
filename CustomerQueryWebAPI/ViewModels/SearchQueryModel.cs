using System.ComponentModel.DataAnnotations;

namespace CustomerQueryWebAPI.ViewModels
{
    public class SearchQueryModel
    {
        [Required(ErrorMessage = "Enter Valid Query ID")]
        [Display(Name = "Query Id")]
        public int QueryId { get; set; }
    }
}
