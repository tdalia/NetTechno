using CustomerQueryData.Models;
using System.ComponentModel.DataAnnotations;

namespace CustomerQueryWebAPI.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {        }

        public ProductViewModel(Product product)
        {
            ProductId = product.ProductId;
            DeptId = product.DeptId;
            Department = product.Department;
            Description = product.Description;
            Company = product.Company;
        }

        public int ProductId { get; set; }

        [Required]
        public int DeptId { get; set; }
        public virtual Department Department { get; set; }

        public string Description { get; set; }

        public string Company { get; set; }

        //public static ProductViewModel ConvertFromProduct()

    }
}
