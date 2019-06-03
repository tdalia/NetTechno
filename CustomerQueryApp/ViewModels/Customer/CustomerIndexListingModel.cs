using CustomerQueryData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerQueryApp.ViewModels.Customer
{
    public class CustomerIndexListingModel
    {
        public int CustomerId { get; set; }
        public int QueryId { get; set; }

        public string Title { get; set; }
        public string QueryDate { get; set; }
        public QueryStatus Status { get; set; }
        public Product Product { get; set; }

        public static CustomerIndexListingModel ConvertQueryMasterToListModel(QueryMaster qm)
        {
            CustomerIndexListingModel cm = new CustomerIndexListingModel();
            cm.CustomerId = qm.CustomerId;
            cm.QueryId = qm.QueryId;
            cm.Title = qm.Title;
            cm.Product = qm.Product;
            cm.Status = qm.Status;
            // Format Date
            cm.QueryDate = qm.QueryDate.ToString("MM/dd/yyyy");

            return cm;
        }

        public static List<CustomerIndexListingModel> ConvertQueryMasterToListModel(List<QueryMaster> qm)
        {
            List<CustomerIndexListingModel> cmList = new List<CustomerIndexListingModel>();

            if (qm.FirstOrDefault() == null)
                return cmList;

            foreach (QueryMaster query in qm)
                cmList.Add(ConvertQueryMasterToListModel(query));

            return cmList;
        }


    }
}
