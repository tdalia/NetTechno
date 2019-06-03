using CustomerQueryData.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerQueryData.Interfaces
{
    public interface IQuery : IDisposable
    {
        Task<List<QueryMaster>> GetAllUnResolvedQueryAsync();
        Task<QueryMaster> GetQueryMaster(int queryId);
        Task<QueryMaster> GetOnlyQuestionOfQueryMaster(int queryId);
        Task<List<QueryAssign>> GetQueryAssignsAsync(int queryId);
        Task<bool> IsQueryRatedByCustomer(int queryId);

        // CUSTOMER SPECIFIC
        Task<List<QueryMaster>> GetUnResolvedQueryOfCustomerAsync(int custId);

        Task<List<QueryMaster>> GetRecentResolvedQueryOfCustomerAsync(int custId);


        Task<List<QueryMaster>> GetQueryMastersOfCustomerAsync(int custId);

        // DEPARTMENT SPECIFIC
        Task<List<QueryMaster>> GetUnResolvedQuerysOfDeptAsync(int deptId = 0);
        Task<List<QueryMaster>> GetResolvedQuerysOfDeptAsync(int deptId = 0);

        // ADD
        Task<bool> AddNewQuery(QueryMaster queryMaster);
        Task<bool> AddNewQueryAssign(int queryId, QueryAssign queryAssign);

        bool ExistsQueryId(int queryId);

        Task<Employee> GetEmployeeProvidedSolutionForAsync(int queryId);

    }
}
