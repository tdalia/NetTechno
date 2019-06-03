using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomerQueryData.Models
{
    public enum QueryStatus
    {
        New = 0,            // Customer asked initially
        Asked = 1,          // Cust questioned again in Conver
        Replied = 2,        // Emp replied
        InProcess = 3,      // 
        Resolved = 4        // Resolved
    }

    public class QueryMaster
    {
        public QueryMaster()
        {
            QueryAssigns = new HashSet<QueryAssign>();
        }

        [Key]
        public int QueryId { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int DeptId { get; set; }
        public virtual Department Department { get; set; }

        [Required]
        public string Title { get; set; }

        public string Message { get; set; }

        [Required]
        public DateTime QueryDate { get; set; }

        public QueryStatus Status { get; set; }

        public virtual ICollection<QueryAssign> QueryAssigns { get; set; }

    }
}
