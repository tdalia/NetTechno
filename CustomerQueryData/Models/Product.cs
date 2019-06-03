using System.ComponentModel.DataAnnotations;

namespace CustomerQueryData.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public int DeptId { get; set; }
        public virtual Department Department { get; set; }

        public string Description { get; set; }

        public string Company { get; set; }
    }
}
