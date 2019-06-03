namespace CustomerQueryWebAPI.ViewModels
{
    public class EmpRateModel
    {
        public int EmployeeId { get; set; }
        public string EmpName { get; set; }
        public string Email { get; set; }
        public int? EmpAvgRating { get; set; }
        public int DeptId { get; set; }
    }
}
