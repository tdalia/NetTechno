using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerQueryData.Models
{
    public class QueryAssign
    {
        public QueryAssign()
        {
            //Employees = new HashSet<Employee>();
        }

        [Key]
        public int Id { get; set; }

        // Same as QueryMaster
        [Required]
        public int QueryId { get; set; }

       // [ForeignKey("QueryMasterFK")]
       public virtual QueryMaster Query { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [Required]
        public DateTime ResponseDate { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string FromCustOrEmp { get; set; }

    }
}
