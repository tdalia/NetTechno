using CustomerQueryApp.Helpers;
using CustomerQueryApp.ViewModels;
using CustomerQueryApp.ViewModels.Customer;
using CustomerQueryData.Interfaces;
using CustomerQueryWebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CustomerQueryApp.Controllers
{
    [Route("[controller]")]
    public class CustomerController : Controller
    {

        CustomerWebAPI api = new CustomerWebAPI();

        public CustomerController()
        {
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            /** Direct Implementation
            List<CustomerIndexListingModel> cmList =  CustomerIndexListingModel
                .ConvertQueryMasterToListModel(await queryContext.GetUnResolvedQueryOfCustomerAsync(1));
            List<CustomerIndexListingModel> resolvedList = CustomerIndexListingModel
                .ConvertQueryMasterToListModel(await queryContext.GetRecentResolvedQueryOfCustomerAsync(1));
            
            CustomerIndexModel cim = new CustomerIndexModel()
            {
                UnresolvedQuery = cmList,
                RecentResolvedQuery = resolvedList
            };

            return View(cim);*/

            int custId = 1;
            List<QueryMasterListModel> listModels = new List<QueryMasterListModel>();
            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/customer/"+custId+"/unresolvedQueries");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                listModels = JsonConvert.DeserializeObject<List<QueryMasterListModel>>(result);
            }

            List<QueryMasterListModel> resolvedlistModels = new List<QueryMasterListModel>();
            
            client = api.Initial();
           // string uri = Url.Link("GetRecentResolvedQuerys", new { customerId = custId });
            res = await client.GetAsync("api/customer/" + custId + "/recentresolvedQueries");
            // (uri);
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                resolvedlistModels = JsonConvert.DeserializeObject<List<QueryMasterListModel>>(result);
            } else if (res.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ViewData["Error"] = "Invalid Customer Id";
            } else if (res.StatusCode == HttpStatusCode.NotFound)
                ViewData["Error"] = "Not Found. Or Wrong Parameter.";

            CustomerIndexModel cim = new CustomerIndexModel()
            {
                CustomerUnResolvedQuerys  = listModels,
                CustomerRecentResolvedQuerys = resolvedlistModels
            };

            return View(cim);
        }

        [HttpGet]
        [Route("getCustomer/{id:int?}")]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            CustomerViewModel cvm = new CustomerViewModel();

            if (id == 0)
            {
                ViewData["SubmitBtnTxt"] = "Create Customer";
                return View(cvm);
            } else
            {
                using (HttpClient client = api.Initial())
                {
                    using (HttpResponseMessage res = await client.GetAsync("api/customer/" + id))
                    {
                        var result = res.Content.ReadAsStringAsync().Result;
                       if (res.IsSuccessStatusCode)
                        {
                            cvm = JsonConvert.DeserializeObject<CustomerViewModel>(result);
                            ViewData["SubmitBtnTxt"] = "Update Profile";
                            return View(cvm);
                        }
                        else if (res.StatusCode == HttpStatusCode.NotFound)
                        {  // 404
                            NotFoundObjectResult nf = JsonConvert.DeserializeObject<NotFoundObjectResult>(result);
                            ViewData["Error"] = nf.Value;
                            return View(cvm);
                        }
                    }
                }
            }

            /*  else
              {
                  CustomerQueryData.Models.Customer customer = _context.GetCustomer(id);
                  if (customer != null)
                  {
                      CustomerModel cm = new CustomerModel(customer);
                      ViewData["SubmitBtnTxt"] = "Update Profile";
                      return View(cm);
                  } 

              }
              return View("Not Found");  
              */
            return BadRequest();

        }

        [HttpPost]
        [Route("addEditCustomer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("CustomerId,FirstName,LastName,Email,UserName,Password")] CustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {

                using (HttpClient client = api.Initial())
                {
                    if (customer.CustomerId == 0)
                    {
                        // POST
                        using (HttpResponseMessage res = await client.PostAsJsonAsync< CustomerViewModel>("api/customer/add", customer))
                        {
                            var result = res.Content.ReadAsStringAsync().Result;
                            if (res.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Index");
                            }
                        }
                    }
                    else
                    {
                        // PUT
                        using (HttpResponseMessage res = await client.PutAsJsonAsync<CustomerViewModel>("api/customer/update", customer))
                        {
                            var result = res.Content.ReadAsStringAsync().Result;
                            if (res.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Index");
                            }
                        }
                    }
                }

                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return View(customer);
            }

            return View(customer);
        }

        [HttpGet]
        [Route("searchQuery")]
        public IActionResult SearchQuery()
        {
            return View();
        }

        [HttpPost]
        [Route("searchQuery")]
        public IActionResult SearchQuery([Bind("QueryId")] SearchQueryModel sqm)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("viewQuery", "query", new { queryId = sqm.QueryId } );
            }

            return View(sqm);
        }

        [HttpGet]
        [Route("queryFaq", Name = "QueryFAQs")]
        public async Task<IActionResult> QueryFAQ()
        {
            QueryFaqExtdViewModel faqVM = new QueryFaqExtdViewModel();

            faqVM.FAQVMList = null;
            List<DeptViewModel> depts = await api.PopulateDeptDropDown();
            DeptViewModel dvm = new DeptViewModel();
            dvm.DeptId = 0;
            dvm.DeptName = "Select Department";
            depts.Insert(0, dvm);
            ViewData["DeptId"] = new SelectList(depts, "DeptId", "DeptName", dvm.DeptName);

            return View(faqVM);
        }

        [HttpPost]
        [Route("queryFaqDept", Name = "PQueryFAQs")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostQueryFAQ([Bind("DeptId")] QueryFaqExtdViewModel faqVM)
        {
            // Get All Dept's
            ViewData["DeptId"] = new SelectList(await api.PopulateDeptDropDown(), "DeptId", "DeptName", faqVM.DeptId);
            
            // Retrieve FAQ's of selected Dept
            using (HttpClient client = api.Initial())
            {
                using (HttpResponseMessage res = await client.GetAsync("api/query/getQueryFAQ/" + faqVM.DeptId))
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    if (res.IsSuccessStatusCode)
                    {
                        faqVM.FAQVMList = JsonConvert.DeserializeObject<List<QueryFAQViewModel>>(result);
                    }
                    else
                    {
                        ViewData["Error"] = res.ReasonPhrase + " - " + result;
                    }
                }
            }

            return View("QueryFAQ", faqVM);
        }

 /*
        [HttpGet]
        [Route("empSurvey")]
        public IActionResult EmpSurvey()
        {
            SurveyViewModel svm = new SurveyViewModel();

            return PartialView("EmpSurveyModalPartial", svm);
        }


        [HttpGet]
        [Route("respondToAjax")]
       // [ProducesResponseType(200,Type = typeof(JsonResult) )]
        public IActionResult RespondToAjax(int queryId)
        {
            return Json("Hello World ! QueryId is " + queryId);
        }

       
        private async Task<List<DeptViewModel>> PopulateDeptDropDown()
        {
            List<DeptViewModel> pvmList = null;

            using (HttpClient client = api.Initial())
            {
                using (HttpResponseMessage res = await client.GetAsync("api/department/getDepartments"))
                {
                    pvmList = new List<DeptViewModel>();
                    if (res.IsSuccessStatusCode)
                    {
                        var result = res.Content.ReadAsStringAsync().Result;
                        pvmList = JsonConvert.DeserializeObject<List<DeptViewModel>>(result);
                    }
                }
            }
            return pvmList;
        }
        */


    }
}