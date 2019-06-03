using CustomerQueryData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomerQueryApp.ViewModels.Query
{
    public class QueryMasterViewModel
    {
        public int QueryId { get; set; }

        public int CustomerId { get; set; }
       // public virtual Customer Customer { get; set; }
       
        [Display(Name = "Product Name")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public string Title { get; set; }

        public string Message { get; set; }

        [Required]
        public DateTime QueryDate { get; set; }

        public QueryStatus Status { get; set; }

        //public List<Department> Departments { get; set; }

        public List<Product> Products { get; set; }
                                           // public virtual ICollection<QueryAssign> QueryAssigns { get; set; }

    }
}
