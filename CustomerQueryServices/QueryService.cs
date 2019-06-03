using CustomerQueryData;
using CustomerQueryData.Interfaces;
using CustomerQueryData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerQueryServices
{
    public class QueryService : IQuery
    {
        private readonly DataContext _context;
        //private delegate Predicate whereClause;
        // 	Java -	Predicate<Enroll> p = (e) -> e.getStudentId() == studId && e.getCourseName() == courseName;
        /*
         * Predicate<string> isUpper = delegate(string s) { return s.Equals(s.ToUpper());};
    bool result = isUpper("hello world!!");

            Predicate<string> isUpper = s => s.Equals(s.ToUpper());
    bool result = isUpper("hello world!!");
         */

        public QueryService(DataContext context)
        {
            _context = context;
        }

        // Get ALL Unresolved Queries
        public async Task<List<QueryMaster>> GetAllUnResolvedQueryAsync()
        {
            List<QueryMaster> unresolvedQueries = await _context.QueryMasters
                .Include(prod => prod.Product)
                .Include(d => d.Department)
                .Where(q => q.Status != QueryStatus.Resolved)
                .OrderByDescending(d => d.QueryDate)
                .ToListAsync<QueryMaster>();

            return unresolvedQueries;
        }

        // https://www.intertech.com/Blog/using-lambda-expressions-for-predicate-func-and-action-arguments-in-generic-list-methods/

        // Get ALL Unresolved Queries of specified Customer
        public async Task<List<QueryMaster>> GetUnResolvedQueryOfCustomerAsync(int custId)
        {
            if (custId == 0)
                return new List<QueryMaster>();

            List<QueryMaster> unresolvedQueries = await _context.QueryMasters
                .Include(prod => prod.Product)  
                .Include(d => d.Department)
                .Where(q => q.CustomerId == custId && q.Status != QueryStatus.Resolved)                
                .OrderByDescending(d => d.QueryDate)
                .ToListAsync<QueryMaster>();

            Func<QueryMaster, bool> whereClause = delegate (QueryMaster q)
            {
                return q.CustomerId == custId && q.Status != QueryStatus.Resolved;
            };

           // var call2Task = GetQueryMasters(whereClause);

            return unresolvedQueries;
        }

        // Get Recent RESOLVED query (in last 30 days)
        public async Task<List<QueryMaster>> GetRecentResolvedQueryOfCustomerAsync(int custId)
        {
            if (custId == 0)
                return new List<QueryMaster>();
             
            List<QueryMaster> unresolvedQueries = await _context.QueryMasters
                .Include(prod => prod.Product)
                .Include(d => d.Department)
                .Where(q => q.CustomerId == custId && q.Status == QueryStatus.Resolved)
                .OrderByDescending(d => d.QueryDate)
                .ToListAsync<QueryMaster>();

           //(q.QueryAssigns.OrderByDescending(qa => qa.ResponseDate).First()).ResponseDate > DateTime.Now.AddDays(-30) )                 



            return unresolvedQueries;
        }

        // TRIAL - PREDICATE
        private List<QueryMaster> GetQueryMastersOfCustomerAsync(Func<QueryMaster, bool> whereClause)
        {
            List<QueryMaster> result = _context.QueryMasters
                .Include(prod => prod.Product)
                .Include(d => d.Department)
                .Where(q => q.CustomerId == 1 && q.Status != QueryStatus.Resolved)
               // .Where(whereClause)
               // Can't use ToListAsync whith whereClause  ??? 
                .OrderByDescending(d => d.QueryDate)
                .ToList<QueryMaster>();

            return result;
        }

        public async Task<QueryMaster> GetQueryMaster(int queryId)
        {
            QueryMaster qm = null;  // = new QueryMaster();

            // Check if queryId Exists
            if (_context.QueryMasters.Any(q => q.QueryId == queryId) )
            {
                qm = _context.QueryMasters
                    .Include(c => c.Customer)
                    .Include(p => p.Product)
                    .Include(d => d.Department)
                    .Where(q => q.QueryId == queryId).ToList().FirstOrDefault();

                qm.QueryAssigns = await GetQueryAssignsAsync(queryId);
            }

            return qm;
        }

        public async Task<QueryMaster> GetOnlyQuestionOfQueryMaster(int queryId)
        {
            QueryMaster qm = null;  // = new QueryMaster();

            // Check if queryId Exists
            if (_context.QueryMasters.Any(q => q.QueryId == queryId))
            {
                qm = (await _context.QueryMasters
                    .Include(c => c.Customer)
                    .Include(p => p.Product)
                    .Include(d => d.Department)
                    .Where(q => q.QueryId == queryId)
                    .ToListAsync())                 
                    .FirstOrDefault();
            }

            return qm;
        }

        // Returns QueryAssigns of Specified queryId
        public async Task<List<QueryAssign>> GetQueryAssignsAsync(int queryId)
        {
            List<QueryAssign> qa = null;
            if (_context.QueryAssigns.Any(i => i.QueryId == queryId))
            {
                qa = await _context.QueryAssigns
                    .Include(c => c.Customer)
                    .Include(e => e.Employee)
                    .Where(q => q.QueryId == queryId)
                    .ToListAsync();
            }

            return qa;

        }



        // Returns ALL QueryMaster of Specified Customer
        public async Task<List<QueryMaster>> GetQueryMastersOfCustomerAsync(int custId)
        {
            List<QueryMaster> result = await _context.QueryMasters
                 .Include(prod => prod.Product)
                 .Include(d => d.Department)
                 .Where(q => q.CustomerId == custId)
                 .OrderByDescending(d => d.QueryDate)
                 .ToListAsync<QueryMaster>();

            return result;
        }

        public async Task<List<QueryMaster>> GetUnResolvedQuerysOfDeptAsync(int deptId = 0)
        {
            List<QueryMaster> queryMasters = await _context.QueryMasters
                .Include(q => q.Customer)
                .Include(p => p.Product)
                .Include(de => de.Department)
                .Where(d => d.Product.DeptId == deptId && d.Status != QueryStatus.Resolved)
                .ToListAsync();

            return queryMasters;
        }

        public async Task<List<QueryMaster>> GetResolvedQuerysOfDeptAsync(int deptId = 0)
        {
            List<QueryMaster> queryMasters = await _context.QueryMasters
                .Include(de => de.Department)
                .Include(a => a.QueryAssigns)
                .Where(d => d.Product.DeptId == deptId && d.Status == QueryStatus.Resolved)
                .ToListAsync();

            return queryMasters;
        }

        public async Task<bool> IsQueryRatedByCustomer(int queryId)
        {
            bool rated = false;

            rated = await _context.Surveys.AnyAsync(s => s.QueryId == queryId);
            return rated;
        }


        #region "Add Query Master/Assigns"

        // Add New Query Master
        public async Task<bool> AddNewQuery(QueryMaster queryMaster)
        {
            EntityEntry<QueryMaster> track = await _context.QueryMasters.AddAsync(queryMaster);
            Console.WriteLine("QM ADDED : " + track.Entity.QueryId);
            int x = await _context.SaveChangesAsync();

            return x > 0 ? true : false;
        }

        public async Task<bool> AddNewQueryAssign(int queryId, QueryAssign queryAssign)
        {
            EntityEntry<QueryAssign> track = await _context.QueryAssigns.AddAsync(queryAssign);
            Console.WriteLine("QA ADDED : " + track.Entity.Id);
            int x = await _context.SaveChangesAsync();

            QueryMaster qm = await _context.QueryMasters.FindAsync(queryId);
            qm.Status = QueryStatus.Resolved;
            x = await _context.SaveChangesAsync();

            return x > 0 ? true : false;
        }

        #endregion


        public bool ExistsQueryId(int queryId)
        {
            return _context.QueryMasters.Any(q => q.QueryId == queryId);
        }

        // Get Employee of who provided Solution to the provided queryID
        public async Task<Employee> GetEmployeeProvidedSolutionForAsync(int queryId)
        {
            Employee employee = null;

            // Get QueryMast of the queryId
            QueryMaster qm = await _context.QueryMasters
                 .Include(a => a.QueryAssigns)
                 .Where(d => d.QueryId == queryId && d.Status == QueryStatus.Resolved)
                 .FirstOrDefaultAsync();
            
            if (qm != null)
            {
                if (qm.QueryAssigns != null && (qm.QueryAssigns != null && qm.QueryAssigns.Count > 0))
                {
                    // Get the latest QueryAssign 
                    QueryAssign qa = qm.QueryAssigns.OrderByDescending(q => q.ResponseDate).FirstOrDefault();
                    if (qa != null)
                    {
                        // Retrieve Employee details 
                        employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == qa.EmployeeId);
                    }
                    qa = null;
                }
            }
            qm = null;

            return employee;
        }

        public void Dispose()
        {
            _context.Dispose();
        }






        /*
Predicate<QueryMaster> predicate = (q) =
List<QueryMaster> result = await GetUnResolvedQuerysOfDeptAsync(deptId, );

List<QueryMaster> result = await GetQueryMastersOfCustomerAsync(1);
result.FindAll(predicate);*/


        /*
        public async Task<List<QueryMaster>> GetUnResolvedQuerysOfDeptAsync(int deptId = 0)
        {
            List<QueryMaster> result = await GetQueryMastersOfCustomerAsync(1);
            result.FindAll(predicate);

            return result;
        }*/

    }
}
