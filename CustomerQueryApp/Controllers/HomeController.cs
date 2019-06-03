using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CustomerQueryData.Models;
using CustomerQueryWebAPI.ViewModels;
using CustomerQueryData.Interfaces;
using CustomerQueryApp;
using CustomerQueryApp.ViewModels;

namespace CustomerQueryData.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployee empContext;

        public HomeController(IEmployee context)
        {
            empContext = context;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View("LoginEmp");
        }

        [HttpPost]
        [Route("[controller]/loginEmployee")]
        public IActionResult Login([Bind("UserName,Password")] EmployeeViewModel evm)
        {
            // Check Login Credentials
            Employee emp = empContext.GetEmployee(evm.UserName, evm.Password);
            EmployeeViewModel uservm = new EmployeeViewModel(emp);
            //(username, password);
            if (emp != null)
            {
                LoginViewModel user = new LoginViewModel(uservm);
                
                HttpContext.Session.SetObjectAsJson("LoggedUser", user);
                
                ViewData["LoggedUsed"] = user;
                ViewData["LoggedId"] = user.EmployeeId;

                /*
                // Rediect to respective page - Emp or Admin
                if (emp.RoleId == 1)
                {
                    // Admin
                    return RedirectToAction("Index", "Admin");
                } else */
                if (emp.RoleId == 1 || emp.RoleId == 2)
                {
                    // Employee
                    return RedirectToAction("Index", "Employee");
                }

                return Ok(emp);
            }

            return View("LoginEmp");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
