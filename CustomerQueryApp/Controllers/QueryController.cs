using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CustomerQueryApp.Helpers;
using CustomerQueryWebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace CustomerQueryApp.Controllers
{
    //[Route("[controller]")]
    public class QueryController : Controller
    {

        CustomerWebAPI api = new CustomerWebAPI();

        public QueryController()
        {
        }

        // View to enter details to Post a New Query
        [HttpGet]
        public async Task<IActionResult> NewQuery(int id = 0)
        {
            QueryMasterListModel qvm = null;
           
            if (id == 0)
            {
                ViewData["SubmitBtnTxt"] = "Create Customer";
                qvm = new QueryMasterListModel();
                qvm.CustomerId = 1;
                qvm.QueryId = 0;
                qvm.QueryDate = DateTime.Today;
                qvm.Status = CustomerQueryData.Models.QueryStatus.New;

                // Retrieve  Products for Drop down
                HttpClient client = api.Initial();
                HttpResponseMessage res = await client.GetAsync("api/product/getProducts");
                List<ProductViewModel> pvmList = new List<ProductViewModel>();
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    pvmList = JsonConvert.DeserializeObject<List<ProductViewModel>>(result);
                }

                ViewData["ProductId"] = new SelectList(pvmList, "ProductId", "Description", qvm.ProductId);
                ViewData["DeptId"] = new SelectList(await api.PopulateDeptDropDown(), "DeptId", "DeptName", "Select Department");

                return View(qvm);
            }
            return View();
        }

        /// <summary>
        /// Add a new Query Master.
        /// Cannot Edit an existing Query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewQuery([Bind("QueryId, CustomerId, ProductId,DeptId, Title,Message,QueryDate,Status")] QueryMasterListModel query)
        {
            string error = "";
            
            if (ModelState.IsValid)
            {
                if (query.QueryId != 0)
                {
                    return Unauthorized();  // ("Existing Query cannot be modified");
                }

                using (HttpClient client = api.Initial())
                {
                    // POST
                    using (HttpResponseMessage res = await client.PostAsJsonAsync<QueryMasterListModel>("api/query/newQuery", query))
                    {
                        var result = res.Content.ReadAsStringAsync().Result;
                        if (res.IsSuccessStatusCode)
                        {
                            // To get the Query ID
                            query = JsonConvert.DeserializeObject<QueryMasterListModel>(result);
                            // NEED TO BE ON SAME PAGE & DISPLAY THE QUERY ID
                            ViewData["AlertMsg"] = query.QueryId;
                            return View("QueryAddedConfirmed");

                            //return RedirectToAction("Index", "customer");  
                        } else
                        {
                            error = res.StatusCode + ": " + res.ReasonPhrase;
                        }
                    }
                }
            }

            //ModelState.AddModelError("Error", error);
            return View(query);
        }

        [HttpGet]
        [Route("[controller]/viewQuery/{queryId:int}")]
        public async Task<IActionResult> ViewQuery(int queryId)
        {
            ViewData["Error"] = "";
            // Get Query Master
            // Get All QueryAssigns of the Master
            QueryMasterListModel qVM = null;
            using (HttpClient client = api.Initial() )
            {
                using (HttpResponseMessage res = await client.GetAsync("api/query/getQueryDetails/" + queryId))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var result = res.Content.ReadAsStringAsync().Result;
                        qVM = JsonConvert.DeserializeObject<QueryMasterListModel>(result);
                        if (qVM != null)
                        {
                            if (qVM.QueryAssigns == null)
                                qVM.QueryAssigns = new List<QueryAssignViewModel>();
                        } else
                        {
                            ViewData["Error"] = "Not Found: Invalid Query Id";
                        }
                    }
                }
            }

            return View(qVM);
        }

        [HttpGet]
        [Route("[controller]/viewQueryAjaxTest/{queryId:int}")]
        public async Task<PartialViewResult> ViewQueryAjaxTest(int queryId)
        {
            // Get Query Master
            // Get All QueryAssigns of the Master
            QueryMasterListModel qVM = null;
            using (HttpClient client = api.Initial())
            {
                using (HttpResponseMessage res = await client.GetAsync("api/query/getQueryDetails/" + queryId))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var result = res.Content.ReadAsStringAsync().Result;
                        qVM = JsonConvert.DeserializeObject<QueryMasterListModel>(result);
                        if (qVM.QueryAssigns == null)
                            qVM.QueryAssigns = new List<QueryAssignViewModel>();
                    }
                }
            }

            return PartialView("ViewQuery", qVM);
        }

        // For Employee ONLY
        [HttpGet]
        [Route("[controller]/addSolution/{queryId:int:min(1)}", Name = "AddQuerySolution")]
        public async Task<IActionResult> QuerySolution(int queryId)
        {
            QueryAssignViewModel qaVM = new QueryAssignViewModel();

            using (HttpClient client = api.Initial())
            {
                using (HttpResponseMessage res = await client.GetAsync("api/query/getShortQuery/" + queryId))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var result = res.Content.ReadAsStringAsync().Result;
                        // Need original query to show Question, QryDate, Product Details
                        QueryMasterListModel qVM = JsonConvert.DeserializeObject<QueryMasterListModel>(result);
                        qaVM.QueryId = queryId;
                        qaVM.ResponseDate = DateTime.Now;
                        qaVM.FromCustOrEmp = "Emp";
                        qaVM.CustomerId = qVM.CustomerId;
                        qaVM.CustomerName = qVM.CustomerName;
                        qaVM.QueryDate = qVM.QueryDate;
                        qaVM.QueryTitle = qVM.Title;
                        qaVM.QueryQuestion = qVM.Message;
                        qaVM.Product = qVM.Product;
                        qaVM.EmployeeId = 3;
                    }
                }


            }
            return View(qaVM);

        }

       // [Microsoft.AspNetCore.Authorization.Authorize("Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/postSolution", Name = "PostQuerySolution")]
        public async Task<IActionResult> PostQuerySolution([Bind("QueryId, CustomerId,EmployeeId,ResponseDate,Message, FromCustOrEmp")] QueryAssignViewModel qaVM)
        {
            string error = "";

            if (ModelState.IsValid)
            {
                if (qaVM.QueryId == 0)
                {
                    return BadRequest();  
                }

                using (HttpClient client = api.Initial())
                {
                    // POST
                    using (HttpResponseMessage res = await client.PostAsJsonAsync<QueryAssignViewModel>("api/query/postQueryAssign", qaVM))
                    {
                        var result = res.Content.ReadAsStringAsync().Result;
                        if (res.IsSuccessStatusCode)
                        {
                            // To get the Query ID
                            //query = JsonConvert.DeserializeObject<QueryMasterListModel>(result);
                            // NEED TO BE ON SAME PAGE & DISPLAY THE QUERY ID
                            //return View(query);
                            return RedirectToAction("queries", "employee");
                        } else
                        {
                            error = res.StatusCode + ": " + res.ReasonPhrase;
                        }
                   }
                }

            }

            return View(qaVM);
        }


    }
}