using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerQueryData.Models
{
    public class Warranty
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int CustomerId { get; set; }

        [Required]
        public DateTime WarrantyStartDate { get; set; }

        [Required]
        public DateTime WarrantyDueDate { get; set; }
        
    }
}
