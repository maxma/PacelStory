using PacelStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacelStory.Repositories
{
    interface ICustomerRepository
    {
        IEnumerable<Customer> GetAllCustomers(int pageNumber);
        Customer GetSpecifiedCustomerById(long id);
        Customer GetSpecifiedCustomerByMoble(string moble);
        long CreateCustomer(Customer item);
        int RemoveCustomer(long customerId);
        long UpdateCustomer(Customer item);
    }
}
