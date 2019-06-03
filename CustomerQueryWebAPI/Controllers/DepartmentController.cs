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
    public class DepartmentController : ControllerBase
    {
        private readonly ICommon commonContext;
        public DepartmentController(ICommon commonCtx)
        {
            commonContext = commonCtx;
        }

        [HttpGet]
        [Route("getDepartments", Name = "GetDepartments")]
        [ProducesResponseType(200, Type = typeof(List<DeptViewModel>))]
        public List<DeptViewModel> GetDepartments()
        {
            List<Department> depts = commonContext.GetAllDepartments();
            // await Task.Run(() => commonContext.GetAllDepartments() );
            List<DeptViewModel> deptViewModels = depts.Select(p => new DeptViewModel(p)).ToList();

            return deptViewModels;
        }

    }
}