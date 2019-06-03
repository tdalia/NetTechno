using CustomerQueryData.Models;

namespace CustomerQueryWebAPI.ViewModels
{
    public class RoleViewModel
    {
        public RoleViewModel(Role r)
        {
            if (r != null)
            {
                RoleId = r.RoleId;
                RoleName = r.RoleName;
            }
        }

        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }
}
