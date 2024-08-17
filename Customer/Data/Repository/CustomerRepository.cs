using Customer.Data.Models;

namespace Customer.Data.Repository
{
    public static class CustomerRepository
    {
        public static List<CustomerModel> Customers = new List<CustomerModel>
        {
        new CustomerModel { Id = 1, FirstName = "Jalak", LastName = "Khan" },
        new CustomerModel { Id = 2, FirstName = "Omair", LastName = "Ali" },
        new CustomerModel { Id = 3, FirstName = "Bilal", LastName = "Khan" },
        new CustomerModel { Id = 4, FirstName = "Ammad", LastName = "Ali" },
        new CustomerModel { Id = 4, FirstName = "Uzair", LastName = "Anwar" }
        };
    }
}
