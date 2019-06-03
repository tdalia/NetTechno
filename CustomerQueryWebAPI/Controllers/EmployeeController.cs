using CustomerQueryData.Interfaces;
using CustomerQueryData.Models;
using CustomerQueryWebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerQueryWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee empContext;
        private readonly ICommon commonContext;

        public EmployeeController(IEmployee context, ICommon common)
        {
            empContext = context;
            commonContext = common;
        }

        /*
       // [HttpGet]
        [Route("loginEmployee")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(EmployeeViewModel))]
        public IActionResult LoginEmployee(string username, string password)
        {
            Employee emp = empContext.GetEmployee(username, password);
                //(evm.UserName, evm.Password);
            if (emp != null)
                return Ok(emp);
            else
                return NotFound();
        }*/


        /// <summary>
        /// Get Employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{employeeId:int:min(1)}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(EmployeeViewModel))]
        public IActionResult GetEmployee(int employeeId = 0)
        {
            Employee employee = empContext.GetEmployee(employeeId);
            EmployeeViewModel evm = null;
            if (employee != null)
            {
                evm = new EmployeeViewModel(employee);
                return Ok(evm);
            }
            else
                return NotFound("Employee with Id: {employeeId} not found. Provide valid Id of Employee");
        }

        [HttpGet]
        [Route("getAllEmployees", Name = "GetAllEmployees")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(List<EmployeeViewModel>))]
        public async Task<List<EmployeeViewModel>> GetAllEmployees()
        {
            List<Employee> employee = await empContext.GetEmployeesAsync();
            List<EmployeeViewModel> list = new List<EmployeeViewModel>();
            foreach(Employee emp in employee)
            {
                list.Add(new EmployeeViewModel(emp));
            }
 
            return list;           
        }

        // Add Employee
        [HttpPost]
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(201)] // CreatedAtAction
        [Route("addEmp", Name = "AddEmployee")]
        public async Task<IActionResult> PostNewEmployee(EmployeeViewModel evm)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            // CONVERT EmployeeViewModel TO Employee
            Employee emp = evm.ConvertToEmployee();
            if (await empContext.AddEmployeeAsync(emp) == true)
                return CreatedAtAction("PostNewCustomer", "Created");
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed due to some problem at backend.");
        }


        // Edit Employee
        [HttpPut]
        [Route("update", Name = "UpdateEmployee")]
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(404)] // Not Found
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdateEmployee(EmployeeViewModel evm)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            if (evm == null)
                return BadRequest("Object cannot be null");

            if (empContext.EmployeeExists(evm.EmployeeId) == false)
                return NotFound("Employee Not Found");

            // CONVERT CustomerModel TO Customer
            Employee emp = evm.ConvertToEmployee();
            if (await empContext.UpdateEmployeeAsync(emp) == true)
                return Ok("Employee Updated");
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed due to some problem at backend.");

        }

        // Delete Employee
        [HttpDelete]
        [Route("delete/{employeeId:int:min(1)}", Name = "DeleteEmployee")]
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(404)] // Not Found
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteCustomer(int employeeId)
        {
            // Check for Existence
            Employee employee = empContext.GetEmployee(employeeId);
            if (employee == null)
                return NotFound("Employee Not Found");

            if (await empContext.DeleteEmployeeAsync(employee) == true)
                return Ok("Employee Deleted");
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed due to some problem at backend.");
        }

        [HttpGet]
        [Route("ratings/{empId:int}")]
        public async Task<IActionResult> GetRatings(int empId)
        {
            Employee employee = empContext.GetEmployee(empId);
            int empRate = 0;
            if (employee != null)
            {
                RatingViewModel rVM = null;

                if (employee.EmpAvgRating.HasValue)
                    empRate = (int)employee.EmpAvgRating;

                // Get all Emp of his dept
                List<Employee> empsOfDept = await empContext.GetEmployeesOfDeptAsync(employee.DeptId);
                List<EmpRateModel> empVMDepts = new List<EmpRateModel>();
                    //empsOfDept.ConvertAll<EmployeeViewModel>(e => new EmployeeViewModel(e));
                foreach (Employee e in empsOfDept)
                {
                    empVMDepts.Add(new EmpRateModel()
                    {
                        EmployeeId = e.EmployeeId,
                        EmpName = e.FirstName + " " + e.LastName,
                        Email = e.Email,
                        EmpAvgRating = e.EmpAvgRating,
                        DeptId = e.DeptId
                    });
                }

                // Get all Dept Ratings
                List<Department> depts =  commonContext.GetAllDepartments();
                List<DeptRateModel> deptVMList = new List<DeptRateModel>(); //depts.ConvertAll<DeptViewModel>(d => new DeptViewModel(d));
                foreach (Department d in depts)
                {
                    deptVMList.Add(new DeptRateModel()
                    {
                         DeptId = d.DeptId,
                         DeptName = d.DeptName,
                         DeptAvgRating = d.DeptAvgRating
                    });
                }

                empsOfDept = null;
                depts = null;

                rVM = new RatingViewModel(empRate, empVMDepts, deptVMList);
                return Ok(rVM);
            }
            else
                return NotFound("Employee with Id: {employeeId} not found. Provide valid Id of Employee");
        }

        public void Dispose()
        {
            empContext.Dispose();
            commonContext.Dispose();
        }
    }
}