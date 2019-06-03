using System.ComponentModel;

namespace CustomerQueryApp.ViewModels.Customer
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }

        public CustomerModel()
        {        }

        public CustomerModel(CustomerQueryData.Models.Customer customer)
        {
            this.CustomerId = customer.CustomerId;
            this.FirstName = customer.FirstName;
            this.LastName = customer.LastName;
            this.Email = customer.Email;
            this.UserName = customer.UserName;
            this.Password = customer.Password;
        }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("E-Mail")]
        public string Email { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }

        public CustomerQueryData.Models.Customer ConvertToCustomer()
        {
            CustomerQueryData.Models.Customer domainCustomer = new CustomerQueryData.Models.Customer();

            // 
            if (this.CustomerId != 0)
                // If Edit mode, get customerId
                domainCustomer.CustomerId = this.CustomerId;

            domainCustomer.FirstName = this.FirstName;
            domainCustomer.LastName = this.LastName;
            domainCustomer.Email = this.Email;
            domainCustomer.UserName = this.UserName;
            domainCustomer.Password = this.Password;

            return domainCustomer;
        }

        
    }
}
