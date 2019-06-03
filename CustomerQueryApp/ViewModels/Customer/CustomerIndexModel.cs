using CustomerQueryWebAPI.ViewModels;
using System.Collections.Generic;

namespace CustomerQueryApp.ViewModels.Customer
{
    public class CustomerIndexModel
    {
        public IEnumerable<CustomerIndexListingModel> UnresolvedQuery { get; set; }

        public IEnumerable<CustomerIndexListingModel> RecentResolvedQuery { get; set; }

        public IEnumerable<QueryMasterListModel> CustomerUnResolvedQuerys { get; set; }

        public IEnumerable<QueryMasterListModel> CustomerRecentResolvedQuerys { get; set; }

    }
}
