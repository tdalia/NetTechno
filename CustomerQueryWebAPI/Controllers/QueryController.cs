using CustomerQueryData.Interfaces;
using CustomerQueryData.Models;
using CustomerQueryWebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerQueryWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly IQuery queryContext;
        private readonly ICommon commonContext;

        public QueryController(IQuery context, ICommon common)
        {
            queryContext = context;
            commonContext = common;
        }

        [HttpPost]
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(201)] // CreatedAtAction
        [Route("newQuery", Name = "PostNewQuery")]
        public async Task<IActionResult> PostNewQueryMaster(QueryMasterListModel queryVM)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            // CONVERT CustomerModel TO Customer
            QueryMaster qm = queryVM.ConvertToQueryMaster();
            bool added = await queryContext.AddNewQuery(qm);

            if (added)
            {
                queryVM.QueryId = qm.QueryId;
                return CreatedAtAction("PostNewQueryMaster", new { queryId = qm.QueryId }, queryVM);    // new { queryId = qm.QueryId }, queryVM);
            }
            else
                return BadRequest("Server Error: Failed to add New Query");
        }

        [HttpGet]
        [Route("getQueryDetails/{queryId:int:min(1)}", Name = "GetQueryDetails")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(QueryMasterListModel))]
        public async Task<QueryMasterListModel> GetQueryDetailsAsync(int queryId)
        {
            if (!queryContext.ExistsQueryId(queryId))
                return null;

            QueryMaster qm = await queryContext.GetQueryMaster(queryId);

            QueryMasterListModel qlm = QueryMasterListModel.ConvertQueryMasterToListModel(qm);

            // Get if query is Rated by Customer or not
            qlm.isQuerySolutionRated = await queryContext.IsQueryRatedByCustomer(queryId);

            return qlm;
        }

        [HttpGet]
        [Route("getShortQuery/{queryId:int:min(1)}", Name = "GetShortQuery")]
        public async Task<QueryMasterListModel> GetShortQueryMaster(int queryId)
        {
            QueryMasterListModel qmLM = await GetQueryDetailsAsync(queryId);
            qmLM.QueryAssigns = null;

            return qmLM;
        }

        [HttpPost]
        [ProducesResponseType(201)] // CreatedAtAction
        [Route("postQueryAssign", Name = "PostNewQueryAssign")]
        public async Task<IActionResult> PostNewQueryAssign(QueryAssignViewModel qaVM)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            // Convert VM to QueryAssign Model
            QueryAssign qa = qaVM.ConvertToQueryAssign();
            bool added = await queryContext.AddNewQueryAssign(qa.QueryId, qa);

            if (added)
            {
                return CreatedAtAction("PostNewQueryAssign", "Created");
            }
            else
                return BadRequest("Server Error: Failed to add New Assign to Query");
        }


        [HttpGet]
        [Route("getDeptunresolvedQueries/{deptId:int:min(1)}", Name = "GetDeptUnResolvedQuerys")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(List<QueryMasterListModel>))]
        public async Task<List<QueryMasterListModel>> GetUnResolvedQuerysAsync(int deptId = 0)
        {
            List<QueryMasterListModel> list = QueryMasterListModel
               .ConvertQueryMasterToListModel(await queryContext.GetUnResolvedQuerysOfDeptAsync(deptId));
            return list;
        }

        [HttpGet]
        [Route("getQueryFAQ/{deptId:int:min(1)}")]
        [ProducesResponseType(400, Type = typeof(BadRequestObjectResult))] 
        [ProducesResponseType(404, Type = typeof(NotFoundObjectResult))]
        [ProducesResponseType(200, Type = typeof(List<QueryFAQViewModel>))]
        public async Task<IActionResult> GetQueryFAQ(int deptId = 0)
        {
            if (deptId < 1)
                return null;

            // Check for Dept Id
            if (!commonContext.DeptExists(deptId))
                return BadRequest("Invalid or Department doesn't exists.");

            List<QueryFAQViewModel> faqList = new List<QueryFAQViewModel>();

            // Get QueryMasters of specific Dept
            List<QueryMaster> queryMasters = await queryContext.GetResolvedQuerysOfDeptAsync(deptId);

            if (queryMasters.Count > 0)
            {
                // From QueryAssign, get the Resolved assign
                foreach (QueryMaster qm in queryMasters)
                {
                    if (qm.QueryAssigns != null && (qm.QueryAssigns != null && qm.QueryAssigns.Count > 0))
                    {
                        QueryAssign qa = qm.QueryAssigns.OrderByDescending(q => q.ResponseDate).FirstOrDefault();
                        if (qa != null)
                            faqList.Add(new QueryFAQViewModel(deptId, qm.Message, qa.Message));
                    }
                }

                queryMasters = null;
                return Ok(faqList);
            } else
            {
                return NotFound("No Query's found for provided Department");
            }

        }

        [HttpGet]
        [Route("getQueryAssignForQuery/{queryId:int:min(1)}")]
        [ProducesResponseType(400, Type = typeof(BadRequestObjectResult))]
        [ProducesResponseType(404, Type = typeof(NotFoundObjectResult))]
        [ProducesResponseType(200, Type = typeof(QueryAssignViewModel))]
        public async Task<IActionResult> GetQuerySolutionForQuery(int queryId = 0)
        {
            if (queryId < 1)
                return BadRequest("Invalid Query Id.");

            if (! queryContext.ExistsQueryId(queryId))
                return NotFound("Query doesn't exists.");

            // Get QueryMaster of specific Dept
            QueryMaster queryMaster = await queryContext.GetQueryMaster(queryId);

            if (queryMaster.QueryAssigns.Count == 0)
                return BadRequest("Query doesn't contain any Solution");

            QueryAssign qa = queryMaster.QueryAssigns.OrderByDescending(q => q.ResponseDate).FirstOrDefault();
            if (qa != null)
            {
                EmployeeViewModel evm = new EmployeeViewModel() {
                    EmployeeId =  qa.EmployeeId,
                    FirstName = qa.Employee.FirstName, 
                    LastName = qa.Employee.LastName
                    
                };
                return Ok(new QueryAssignViewModel(qa));
            }
            else
                return NotFound("No Solution found for Query Id : " + queryId);


        }

        [HttpGet]
        [ProducesResponseType(400, Type = typeof(BadRequestObjectResult))]
        [ProducesResponseType(404, Type = typeof(NotFoundObjectResult))]
        [ProducesResponseType(200, Type = typeof(EmployeeViewModel))]
        [Route("getEmployeeProvidedSolutionFor/{queryId:int:min(1)}")]
        public async Task<IActionResult> GetEmployeeProvidedSolutionFor(int queryId)
        {
            if (!queryContext.ExistsQueryId(queryId))
                return NotFound("Query doesn't exists.");

            Employee emp = await queryContext.GetEmployeeProvidedSolutionForAsync(queryId);
            if (emp != null)
            {
                EmployeeViewModel evm = new EmployeeViewModel()
                {
                    EmployeeId = emp.EmployeeId,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName
                };
                return Ok(evm);
            }

            return BadRequest("Problem occured while retrieving information.");            
        }


        public void Dispose()
        {
            queryContext.Dispose();
            commonContext.Dispose();
        }
    }
}