using System.Collections.Generic;

namespace CustomerQueryWebAPI.ViewModels
{
    public class RatingViewModel
    {
        public RatingViewModel()
        {        }

        public RatingViewModel(int empRating, List<EmpRateModel> empDeptList, List<DeptRateModel> deptList)
        {
            EmployeeRating = empRating;
            EmployeesSameDeptList = empDeptList;
            DeptViewModelList = deptList;
        }

        public int EmployeeRating { get; set; }

        public List<EmpRateModel> EmployeesSameDeptList { get; set; }

        public List<DeptRateModel> DeptViewModelList { get; set; }
    }
}
