using CustomerQueryData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerQueryWebAPI.ViewModels
{
    public class QueryListViewModel
    {
        [Display(Name = "Query Id")]
        public int QueryId { get; set; }

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime QueryDate { get; set; }

        [Display(Name = "Query Date")]
        public string QueryDateStr { get; set; }
        public QueryStatus Status { get; set; }
        [Display(Name = "Product Name")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
