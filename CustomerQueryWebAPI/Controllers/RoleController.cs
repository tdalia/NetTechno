using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerQueryData.Interfaces;
using CustomerQueryWebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerQueryWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ICommon commonContext;

        public RoleController(ICommon commonCtx)
        {
            commonContext = commonCtx;
        }

        [HttpGet]
        [Route("getRoles", Name = "GetRoles")]
        [ProducesResponseType(200, Type = typeof(List<RoleViewModel>))]
        public List<RoleViewModel> GetDepartments()
        {
            List<RoleViewModel> roles = commonContext.GetAllRoles().Select(p => new RoleViewModel(p)).ToList();

            return roles;
        }

    }
}