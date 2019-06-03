using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CustomerQueryData.Models;
using CustomerQueryData.Interfaces;
using CustomerQueryApp.Helpers;
using CustomerQueryWebAPI.ViewModels;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using CustomerQueryApp.ViewModels;
using Microsoft.AspNetCore.Http;

namespace CustomerQueryApp.Controllers
{
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IEmployee _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private LoginViewModel user;

        CustomerWebAPI api = new CustomerWebAPI();

        public EmployeeController(IEmployee context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            user = _session.GetObjectFromJson<LoginViewModel>("LoggedUser");
        }

        /*
        private async Task<List<DeptViewModel>> PopulateDeptDropDown()
        {
            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/department/getDepartments");
            List<DeptViewModel> pvmList = new List<DeptViewModel>();
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                pvmList = JsonConvert.DeserializeObject<List<DeptViewModel>>(result);
            }

            return pvmList;
        }
*/

        // GET: Employee
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {

            /**
            List<EmployeeViewModel> list = new List<EmployeeViewModel>();
            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/employee/getAllEmployees");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(result);
            }
            return View(list);
             */

            //  LoginViewModel user = HttpContext.Session.GetObjectFromJson<LoginViewModel>("LoggedUser");

            ViewData["LoggedId"] = user.EmployeeId;
            ViewData["LoggedUser"] = user;
            /*
            var dataContext = await _context.GetEmployeesAsync();  

            var listingResult = dataContext
                .Select(result => new EmployeeViewModel()
                {
                    EmployeeId = result.EmployeeId,
                    FirstName = result.FirstName, 
                    LastName = result.LastName,
                    Email = result.Email,
                    UserName = result.UserName,
                    EmpAvgRating = result.EmpAvgRating,
                    DeptName = result.Department.DeptName
                });
            return View(listingResult);
            */
            return View();
        }

        //[Authorize("Admin")]
        // GET: Employee/Create
        //[HttpGet]
        [Route("[controller]/AddOrEdit")]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            TempData["LoggedUser"] = user;

            // Populate drop downs
            if (id == 0)
            {
                // New Employee
                // Check for Admin Rights
                if (user.RoleName.StartsWith("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    ViewData["DeptId"] = new SelectList(await api.PopulateDeptDropDown(), "DeptId", "DeptName");
                    ViewData["RoleId"] = new SelectList(await api.PopulateRoleDropDown(), "RoleId", "RoleName");
                    return View(new Employee());
                }
                else
                    return Forbid();    // Access Denied
            } else
            {
                ViewData["DeptId"] = new SelectList(await api.PopulateDeptDropDown(), "DeptId", "DeptName");
                ViewData["RoleId"] = new SelectList(await api.PopulateRoleDropDown(), "RoleId", "RoleName");
                Employee emp = _context.GetEmployee(id);
                return View(emp);
            }
        }

         // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/AddOrEdit")]
        public async Task<IActionResult> AddOrEdit([Bind("EmployeeId,FirstName,LastName,Email,UserName,Password,EmpAvgRating,DeptId, RoleId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.EmployeeId == 0)
                    await _context.AddEmployeeAsync(employee);
                else
                    await _context.UpdateEmployeeAsync(employee);

                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptId"] = new SelectList(await api.PopulateDeptDropDown(), "DeptId", "DeptName", employee.DeptId);
            ViewData["RoleId"] = new SelectList(await api.PopulateRoleDropDown(), "RoleId", "RoleName", employee.RoleId);
            return View(employee);
        }

        // GET: Employee/Delete/5
        //[Authorize("Admin")]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int empId = Convert.ToInt32(id);

            var employee = _context.GetEmployee(empId);
            if (employee == null)
            {
                return NotFound();
            } else
                await _context.DeleteEmployeeAsync(employee);

            return RedirectToAction(nameof(Index));
        }

        // Show List of Unresolved Queries
        [HttpGet]
        [Route("queries")]
        public async Task<IActionResult> QueryIndex()
        {

            int deptId = user.DeptId;
            List<QueryMasterListModel> listModels = new List<QueryMasterListModel>();

            using (HttpClient client = api.Initial())
            {
                using (HttpResponseMessage res = await client.GetAsync("api/query/getDeptunresolvedQueries/"+deptId))
                {
                    if(res.IsSuccessStatusCode)
                    {
                        var result = res.Content.ReadAsStringAsync().Result;
                        listModels = JsonConvert.DeserializeObject<List<QueryMasterListModel>>(result);
                    }
                }
            }

            return View(listModels);   
        }

        [HttpGet]
        [Route("ratings")]
        public async Task<IActionResult> Ratings()
        {
            int empId = 3;

            // Show His Rating
            // Show Rating of other emp in dept
            // Show other Dept's Ratings

            RatingViewModel rvm = new RatingViewModel();
            using (HttpClient client = api.Initial())
            {
                using (HttpResponseMessage res = await client.GetAsync("api/employee/ratings/" + empId))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var result = res.Content.ReadAsStringAsync().Result;
                        rvm = JsonConvert.DeserializeObject<RatingViewModel>(result);
                    }
                }
            }

            return View(rvm);
        }


    }
}
