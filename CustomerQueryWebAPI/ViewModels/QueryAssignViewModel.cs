using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CustomerQueryData.Models;

namespace CustomerQueryWebAPI.ViewModels
{
    public class QueryAssignViewModel
    {
        public QueryAssignViewModel()
        {        }

        public QueryAssignViewModel(QueryAssign qa)
        {
            Id = qa.Id;
            QueryId = qa.QueryId;
            CustomerId = qa.CustomerId;
            EmployeeId = qa.EmployeeId;
            ResponseDate = qa.ResponseDate;
            ResponseDateStr = qa.ResponseDate != null ? qa.ResponseDate.ToString("MM/dd/yyyy") : "";
            Message = qa.Message;
            FromCustOrEmp = qa.FromCustOrEmp;

            if (qa.Query != null)
            {
                CustomerName = qa.Query.Customer.FirstName + " " + qa.Query.Customer.LastName;
                QueryDate = qa.Query.QueryDate; //.ToString("MM/dd/yyyy");
                QueryTitle = qa.Query.Title;
                QueryQuestion = qa.Query.Message;
                Product = qa.Query.Product;
            }
        }

        public int Id {  get; set; }

        // Same as QueryMaster
        [Required]
        public int QueryId { get; set; }

        public int CustomerId { get; set; }

        public int EmployeeId { get; set; }
       // public virtual Employee Employee { get; set; }

       // [Required]
        public int ForDeptId { get; set; }

        [Required]
        public DateTime ResponseDate{  get; set;   }

        [Display(Name = "Response Date")]
        public string ResponseDateStr{   get;  set;        }

        [Required(ErrorMessage = "Solution can't be blank")]
        [Display(Name = "Solution")]
        public string Message { get; set; }

        [Required]
        public string FromCustOrEmp { get; set; }

        // From QueryMaster
        [Display(Name = "Query Title")]
        public string QueryTitle { get; set; }
        [Display(Name = "Query")]
        public string QueryQuestion { get; set; }
        [Display(Name = "Product Name")]
        public Product Product { get; set; }

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Query Date")]
        public DateTime QueryDate { get; set; }


        public QueryAssign ConvertToQueryAssign()
        {
            return ConvertToQueryAssign(this);
        }

        public static QueryAssign ConvertToQueryAssign(QueryAssignViewModel assign)
        {
            QueryAssign qa = new QueryAssign();
            qa.Id = assign.Id;
            qa.QueryId = assign.QueryId;
            qa.CustomerId = assign.CustomerId;
            qa.EmployeeId = assign.EmployeeId;
            qa.ResponseDate = assign.ResponseDate;
            qa.Message = assign.Message;
            qa.FromCustOrEmp = assign.FromCustOrEmp;
            return qa;
        }

        public static ICollection<QueryAssign> ConverToQueryAssignList(ICollection<QueryAssignViewModel> qvmList)
        {
            List<QueryAssign> qaList = new List<QueryAssign>();
            foreach(QueryAssignViewModel qvm in qvmList)
            {
                qaList.Add(QueryAssignViewModel.ConvertToQueryAssign(qvm));
            }
            return qaList;
        }

    }
}
