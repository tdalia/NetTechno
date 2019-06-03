using System.Collections.Generic;

namespace CustomerQueryApp.ViewModels.Employee
{
    public class EmployeeIndexModel
    {
        public IEnumerable<EmployeeIndexListingModel> Employees { get; set; }
    }
}
