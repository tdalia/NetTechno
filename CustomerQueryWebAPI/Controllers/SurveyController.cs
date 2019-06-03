using CustomerQueryData.Interfaces;
using CustomerQueryData.Models;
using CustomerQueryWebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerQueryWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly ISurvey surveyContext;
        private readonly ICommon commonContext;
        private readonly ICustomer customerContext;
        private readonly IEmployee employeeContext;

        public SurveyController(ISurvey survey, ICommon common, ICustomer customer, IEmployee employee)
        {
            surveyContext = survey;
            commonContext = common;
            customerContext = customer;
            employeeContext = employee;
        }

        // Post New
        [HttpPost]
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(201)] // CreatedAtAction
        [Route("newSurvery", Name = "PostNewSurvey")]
        public async Task<IActionResult> PostNewSurvey(SurveyViewModel survey)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            // Convert SurveyViewModel to Survey
            Survey surveyEnt = survey.ConvertToSurvey();
            bool added = await surveyContext.AddSurvey(surveyEnt);

            if (added)
                return CreatedAtAction("PostNewSurvey", "Survey Added Successfully");
            else
                return BadRequest("Error occured while saving the Survey.");
        }

        //  GetAllSurveys()
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<SurveyViewModel>))]
        [Route("getAllSurvery", Name = "GetAllSurveys")]
        public async Task<IActionResult> GetAllSurveys()
        {
            List<Survey> surveys = await surveyContext.GetAllSurveys();

            // Convert all Survey Ent to SurveyViewModels
            List<SurveyViewModel> surveyVMList =  surveys.ConvertAll<SurveyViewModel>(s => new SurveyViewModel(s));

            surveys = null;

            return Ok(surveyVMList);
        }

        // GetDepartmentSurveys(int deptId = 0)
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(List<SurveyViewModel>))]
        [Route("getDeptSurvery/{deptId:int:min(1)}", Name = "GetSurveysForDept")]
        public async Task<IActionResult> GetSurveysForDept(int deptId = 0)
        {
            if (!commonContext.DeptExists(deptId))
                return BadRequest("Invalid. Department Id doesn't exists !");

            List<Survey> surveys = await surveyContext.GetDepartmentSurveys(deptId);

            // Convert all Survey Ent to SurveyViewModels
            List<SurveyViewModel> surveyVMList = surveys.ConvertAll<SurveyViewModel>(s => new SurveyViewModel(s));

            surveys = null;

            return Ok(surveyVMList);
        }

        // GetCustomerSurveys(int custId = 0)
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(List<SurveyViewModel>))]
        [Route("getSurveryFromCustomer/{customerId:int:min(1)}", Name = "GetSurveysFromCustomer")]
        public async Task<IActionResult> GetSurveysFromCustomer(int customerId = 0)
        {
            if (! customerContext.CustomerExists(customerId) )
                return BadRequest("Invalid. Customer Id doesn't exists !");

            List<Survey> surveys = await surveyContext.GetCustomerSurveys(customerId);

            // Convert all Survey Ent to SurveyViewModels
            List<SurveyViewModel> surveyVMList = surveys.ConvertAll<SurveyViewModel>(s => new SurveyViewModel(s));

            surveys = null;

            return Ok(surveyVMList);
        }

        // GetEmployeeSurveys(int empId = 0)
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(List<SurveyViewModel>))]
        [Route("getEmployeeSurvey/{employeeId:int:min(1)}", Name = "GetSurveysForEmployee")]
        public async Task<IActionResult> GetSurveysForEmployee(int employeeId = 0)
        {
            if (! employeeContext.EmployeeExists(employeeId) )
                return BadRequest("Invalid. Employee Id doesn't exists !");

            List<Survey> surveys = await surveyContext.GetEmployeeSurveys(employeeId);

            // Convert all Survey Ent to SurveyViewModels
            List<SurveyViewModel> surveyVMList = surveys.ConvertAll<SurveyViewModel>(s => new SurveyViewModel(s));

            surveys = null;

            return Ok(surveyVMList);
        }

        // Post New Rating for Employee's Solution
        [HttpPost]
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(201)] // CreatedAtAction
        [Route("postNewEmpSurvery", Name = "PostNewEmpSurvey")]
        public async Task<IActionResult> PostNewEmpSurvey(SurveyViewModel survey)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            // Convert SurveyViewModel to Survey
            Survey surveyEnt = survey.ConvertToSurvey();
            surveyEnt.Employee = employeeContext.GetEmployee((int)survey.EmployeeId);
            bool added = await surveyContext.AddEmpSurvey(surveyEnt);

            // Update Emp Rating (
            if (added)
                return CreatedAtAction("PostNewEmpSurvey", "Survey Added Successfully");
            else
                return BadRequest("Error occured while saving the Survey.");
        }


        // Dispose
        public void Dispose()
        {
            surveyContext.Dispose();
            commonContext.Dispose();
            customerContext.Dispose();
            employeeContext.Dispose();
        }

    }
}