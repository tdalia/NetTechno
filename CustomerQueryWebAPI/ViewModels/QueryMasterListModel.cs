using CustomerQueryData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CustomerQueryWebAPI.ViewModels
{
    public class QueryMasterListModel
    {
        public QueryMasterListModel()
        {
            QueryAssigns = new List<QueryAssignViewModel>();
        }

        public int CustomerId { get; set; }
        [Display(Name = "Query Id")]
        public int QueryId { get; set; }

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime QueryDate { get; set; }

        [Display(Name = "Query Date")]
        public string QueryDateStr { get; set; }
        public QueryStatus Status { get; set; }
        [Display(Name = "Product Name")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public string Message { get; set; }

        [Display(Name = "Department")]
        public int DeptId { get; set; }
        public string DepartmentName { get; set; }

        public List<Product> Products { get; set; }

        public bool isQuerySolutionRated { get; set; } = false;

        public virtual ICollection<QueryAssignViewModel> QueryAssigns { get; set; }


        #region "Conversion Methods"

        public static QueryMasterListModel ConvertQueryMasterToListModel(QueryMaster qm)
        {
            QueryMasterListModel cm = new QueryMasterListModel();
            cm.CustomerId = qm.CustomerId;
            if (qm.Customer != null)
            {
                cm.CustomerName = ((qm.Customer.FirstName == null) ? "" : qm.Customer.FirstName)
                    + " "
                    + ((qm.Customer.LastName == null) ? "" : qm.Customer.LastName);
            }
            cm.QueryId = qm.QueryId;
            cm.Title = qm.Title;
            cm.Product = qm.Product;
            cm.DeptId = qm.DeptId;
            cm.DepartmentName = qm.Department.DeptName;
            cm.Status = qm.Status;
            cm.Message = qm.Message;
            // Format Date
            cm.QueryDate = qm.QueryDate;
              cm.QueryDateStr = qm.QueryDate.ToString("MM/dd/yyyy");

            cm.QueryAssigns = qm.QueryAssigns != null ?
                qm.QueryAssigns.Select(qa => new QueryAssignViewModel(qa)).ToList() :
                null;

            return cm;
        }

        public static List<QueryMasterListModel> ConvertQueryMasterToListModel(List<QueryMaster> qm)
        {
            List<QueryMasterListModel> cmList = new List<QueryMasterListModel>();

            if (qm.FirstOrDefault() == null)
                return cmList;

            // Convert all QueryMaster to QueryMasterListModel
            cmList = qm.ConvertAll(q => ConvertQueryMasterToListModel(q));

            return cmList;
        }

        public QueryMaster ConvertToQueryMaster()
        {
            QueryMaster qm = new QueryMaster();
            qm.QueryId = this.QueryId;
            qm.CustomerId = this.CustomerId;
            qm.ProductId = this.ProductId;
            qm.DeptId = this.DeptId;
            qm.QueryDate = this.QueryDate;
            qm.Status = this.Status;
            qm.Product = this.Product;
            qm.Title = this.Title;
            qm.Message = this.Message;

            qm.QueryAssigns = QueryAssignViewModel.ConverToQueryAssignList(QueryAssigns);

            return qm;
        }

        #endregion

    }
}
