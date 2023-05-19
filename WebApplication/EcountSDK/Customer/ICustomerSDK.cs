using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    public interface ICustomerSDK
    {
        List<Customer> GetCustomerList();
        List<Customer> GetSearchedList(string code);
        int InsertCustomer(Customer customer);
        int ModifyCustomer(Customer customer);
        int DeleteCustomer(string code);
    }
}
