using CustomerQueryApp.Helpers;
using CustomerQueryWebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CustomerQueryApp.Controllers
{
    [Route("[controller]")]
    public class SurveyController : Controller
    {

        CustomerWebAPI api = new CustomerWebAPI();

        // View to enter Survey for Dept from Customer
        [HttpGet]
        [Route("sendDeptSurvey", Name = "SendDeptSurvey")]
        public async Task<IActionResult> sendSurvey(int deptId = 0)
        {
            SurveyViewModel svm = new SurveyViewModel();
            svm.CustomerId = 1;
            svm.EmployeeId = 0;

            ViewData["DeptId"] = new SelectList(await api.PopulateDeptDropDown(), "DeptId", "DeptName");

            return View("sendSurveyForDept", svm);
        }

        [HttpPost]
        [Route("postDeptSurvey", Name = "PostDeptSurvey")]
        public async Task<IActionResult> sendSurvey([Bind("Ratings, DeptId")] SurveyViewModel svm)
        {

            svm.CustomerId = 1;
        
            // call API
            using (HttpClient client = api.Initial())
            {
                using(HttpResponseMessage res = await client.PostAsJsonAsync< SurveyViewModel>("api/survey/newSurvery", svm))
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    if (res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "customer");
                    } else
                    {
                        ViewData["Error"] = result;
                        return View("sendSurveyForDept", svm);
                    }
                }
            }
        }

        [HttpGet]
        [Route("sendSolutionSurvey/{queryId:int:min(1)}", Name = "SendSolutionSurvey")]
        public async Task<IActionResult> sendSolutionSurveyAsync(int queryId = 0)
        {
            SurveyViewModel svm = new SurveyViewModel();
            svm.CustomerId = 1;
            svm.QueryId = queryId;

            // Retrieve Emp ID & Emp Name from Query who provided solution 
            svm.EmployeeId = 0;

            string empName = "Emp Name";
            
            using (HttpClient client = api.Initial())
            {
                using (HttpResponseMessage res = await client.GetAsync("api/query/getEmployeeProvidedSolutionFor/" + queryId))
                {
                    EmployeeViewModel evm = null;
                    if (res.IsSuccessStatusCode)
                    {
                        var result = res.Content.ReadAsStringAsync().Result;
                        evm = JsonConvert.DeserializeObject<EmployeeViewModel>(result);
                        if (evm != null)
                        {
                            svm.EmployeeId = evm.EmployeeId;
                            empName = evm.FirstName;
                        }
                    }
                }
            }


            ViewData["EmpName"] = empName;

            return View("sendSolutionSurvey", svm);
        }

        [HttpPost]
        [Route("postSolutionSurvey", Name = "PostSolutionSurvey")]
        public async Task<IActionResult> postSolutionSurvey([Bind("CustomerId,EmployeeId,QueryId,Ratings")] SurveyViewModel svm)
        {
            // Have to post Survey for Emp
            // Have cust Id, Eemp Id, Query Id & Ratings

            // call API
            using (HttpClient client = api.Initial())
            {
                using (HttpResponseMessage res = await client.PostAsJsonAsync<SurveyViewModel>("api/survey/postNewEmpSurvery", svm))
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    if (res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "customer");
                    }
                    else
                    {
                        ViewData["Error"] = result;
                        return View("sendSolutionSurvey", svm);
                    }
                }
            }
        }


    }
}