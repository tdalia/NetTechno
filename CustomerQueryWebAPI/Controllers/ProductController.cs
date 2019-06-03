using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerQueryData.Interfaces;
using CustomerQueryData.Models;
using CustomerQueryWebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerQueryWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct productContext;

        public ProductController(IProduct product)
        {
            productContext = product;
        }


        [HttpGet]
        [Route("getProducts", Name = "GetProducts")]
        [ProducesResponseType(200, Type = typeof(List<ProductViewModel>))]
        public async Task<List<ProductViewModel>> GetProducts()
        {
            List<Product> products = await productContext.GetAllProducts();
            List<ProductViewModel> productViewModels = products.Select(p => new ProductViewModel(p)).ToList();

            return productViewModels;
        }


    }
}