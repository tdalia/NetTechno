using CustomerQueryData;
using CustomerQueryData.Interfaces;
using CustomerQueryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerQueryServices
{
    public class SurveyService : ISurvey
    {
        private readonly DataContext _context;

        public SurveyService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> AddSurvey(Survey survey)
        {
            _context.Surveys.Add(survey);
            int x = await _context.SaveChangesAsync();

            // If For Dept
            // Get all Dept Count
            int countRecords = await _context.Surveys.Where(d => d.DeptId == survey.DeptId).CountAsync();

            // Find the sum of Rate
            int sumRating = await _context.Surveys.Where(d => d.DeptId == survey.DeptId).SumAsync(r => r.Ratings);

            // Avg = sum / countOfRecods
            int avg = (int)sumRating / countRecords;

           // Get Dept
            Department dept = _context.Departments.Where(d => d.DeptId == survey.DeptId).FirstOrDefault();
            Console.WriteLine("AddSurvey: Count Records = " +  countRecords + " SumRat: " + sumRating +  "Avg :" + avg + " Prev Avg :" + dept.DeptAvgRating );
            dept.DeptAvgRating = avg;

            // Update 
            _context.Departments.Update(dept);
            x = await _context.SaveChangesAsync();

            return x > 0 ? true : false;
        }

        public async Task<bool> AddEmpSurvey(Survey survey)
        {
            if (survey.EmployeeId.HasValue)
            {
                _context.Surveys.Add(survey);
                int x = await _context.SaveChangesAsync();

                // Get all Emps records
                List<Survey> sList = await _context.Surveys
                    .Where(e => e.EmployeeId.HasValue && e.EmployeeId == survey.EmployeeId).ToListAsync<Survey>();
                int countRecords = sList.Count();

                // Find the sum of Rate
                int sumRating = sList.Sum(r => r.Ratings);
               
                // Avg = sum / countOfRecods
                int avg = (int)sumRating / countRecords;

                // Get Employee
                Employee employee = _context.Employees.Where(e => e.EmployeeId == survey.EmployeeId).FirstOrDefault();
                employee.EmpAvgRating = avg;
                // Update 
                _context.Employees.Update(employee);
                x = await _context.SaveChangesAsync();

                return x > 0 ? true : false;
            }

            return false;
        }

        public async Task<List<Survey>> GetAllSurveys()
        {
            List<Survey> surveys = await _context.Surveys
                .Include(c => c.Customer)
                .Include(d => d.Department)
                .Include(e => e.Employee)
                .ToListAsync();

            return surveys;
        }

        public async Task<List<Survey>> GetDepartmentSurveys(int deptId = 0)
        {
            List<Survey> surveys = await GetAllSurveys();
            surveys = surveys
                .Where(s => s.DeptId == deptId)
                .ToList();

            return surveys;
        }

        public async Task<List<Survey>> GetCustomerSurveys(int custId = 0)
        {
            List<Survey> surveys = await GetAllSurveys();
            surveys = surveys
                .Where(s => s.CustomerId == custId)
                .ToList();

            return surveys;
        }

        public async Task<List<Survey>> GetEmployeeSurveys(int empId = 0)
        {
            List<Survey> surveys = await GetAllSurveys();
            surveys = surveys
                .Where(s => s.EmployeeId == empId)
                .ToList();

            return surveys;
        }

        public async Task<bool> IsQueryRated(int queryId)
        {
            return await _context.Surveys.AnyAsync(s => s.QueryId == queryId);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
