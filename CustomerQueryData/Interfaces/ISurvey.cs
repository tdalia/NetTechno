using CustomerQueryData.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerQueryData.Interfaces
{
    public interface ISurvey : IDisposable
    {
        Task<bool> AddSurvey(Survey survey);

        Task<bool> AddEmpSurvey(Survey survey);

        Task<List<Survey>> GetAllSurveys();

        Task<List<Survey>> GetCustomerSurveys(int custId = 0);

        Task<List<Survey>> GetDepartmentSurveys(int deptId = 0);

        Task<List<Survey>> GetEmployeeSurveys(int empId = 0);

        Task<bool> IsQueryRated(int queryId);
    }
}
