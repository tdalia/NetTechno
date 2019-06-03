 using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerQueryData.Interfaces;
using CustomerQueryData.Models;
using CustomerQueryWebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;


/**
 * 
 */
namespace CustomerQueryWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomer _context;
        private readonly IQuery queryContext;
        private readonly ISurvey surveyContext;

        public CustomerController(ICustomer context, IQuery query, ISurvey survey)
        {
            _context = context;
            queryContext = query;
            surveyContext = survey;
        }

        [HttpGet]
        [Route("{customerId:int:min(1)}/unresolvedQueries", Name = "GetUnResolvedQuerys")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(List<QueryMasterListModel>))]
        public async Task<List<QueryMasterListModel>> GetUnResolvedQuerysAsync(int customerId = 0)
        {
             List<QueryMasterListModel> list = QueryMasterListModel
                .ConvertQueryMasterToListModel(await queryContext.GetUnResolvedQueryOfCustomerAsync(customerId) );
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{customerId:int:min(1)}/recentresolvedQueries", Name = "GetRecentResolvedQuerys")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(List<QueryMasterListModel>))]
        public async Task<IActionResult> GetRecentResolvedQuerysAsync(int customerId = 0)
        {
            if (customerId <= 0)
                return BadRequest("Customer ID can't be 0");    // 400

            List<QueryMasterListModel> list = QueryMasterListModel
               .ConvertQueryMasterToListModel(await queryContext.GetRecentResolvedQueryOfCustomerAsync(customerId));

            foreach(QueryMasterListModel qm in list)
                qm.isQuerySolutionRated = await surveyContext.IsQueryRated(qm.QueryId);

            return Ok(list);
        }

 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{customerId:int:min(1)}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CustomerViewModel))]
        public IActionResult GetCustomer(int customerId = 0)
        {
            Customer customer = _context.GetCustomer(customerId);
            CustomerViewModel cvm = null;
            if (customer != null)
            {
                cvm = new CustomerViewModel(customer);
                return Ok(cvm);
            }
            else
                return NotFound("Customer with Id: {customerId} not found. Provide valid Id of Customer");
        }


        [HttpPost]
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(201)] // CreatedAtAction
        [Route("add", Name = "AddCustomer")]
        public async Task<IActionResult> PostNewCustomer(CustomerViewModel customerVM)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            // CONVERT CustomerModel TO Customer
            Customer cust = customerVM.ConvertToCustomer();
            await _context.AddCustomerAsync(cust);

            return CreatedAtAction("PostNewCustomer", "Created");
        }

        [HttpPut]
        [Route("update", Name = "UpdateCustomer")]
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(404)] // Not Found
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdateCustomer(CustomerViewModel customerVM)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            if (customerVM == null)
                return BadRequest("Object cannot be null");

            if (_context.GetCustomer(customerVM.CustomerId) == null)
                return NotFound("Customer Not Found");

            // CONVERT CustomerModel TO Customer
            Customer cust = customerVM.ConvertToCustomer();
            await _context.UpdateCustomerAsync(cust);

            return Ok("Customer Updated");
        }

        // GET: /<controller>/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return Ok("Index Action Test");
        }

        protected void Dispose()
        {
            _context.Dispose();
            queryContext.Dispose();
            surveyContext.Dispose();
        }
    }
}
