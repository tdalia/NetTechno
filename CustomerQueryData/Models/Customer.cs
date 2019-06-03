using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomerQueryData.Models
{
    public class Customer
    {
        public Customer()
        {
            QueryMasters = new HashSet<QueryMaster>();
            Surveys = new HashSet<Survey>();
        }

        //[Column(TypeName = "datetime2")]
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual ICollection<QueryMaster> QueryMasters { get; set; }

        public virtual ICollection<Survey> Surveys { get; set; }
    }
}
