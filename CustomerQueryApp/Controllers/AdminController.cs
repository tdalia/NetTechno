using CustomerQueryApp.ViewModels;
using CustomerQueryData.Interfaces;
using CustomerQueryWebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerQueryApp.Controllers
{
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly IEmployee _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private LoginViewModel user;

        public AdminController(IEmployee context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
           // LoginViewModel lvm = _session.GetObjectFromJson<LoginViewModel>("LoggedUser");
            user =_session.GetObjectFromJson<LoginViewModel>("LoggedUser");

        }

        public async Task<IActionResult> Index()
        {
            if (user.RoleId != 1)
                return Forbid();

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
        }
    }
}