using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    public class MemoryCustomerSDK:ICustomerSDK
    {
        private static List<Customer> _custlist = new List<Customer>();
        public List<Customer> GetCustomerList()
        {
                return _custlist;      
        }
        public List<Customer> GetSearchedList(string code)
        {
                if (string.IsNullOrEmpty(code))
                {
                    // code가 비어있는 경우
                    return _custlist;
                }
                else
                {                    
                    return _custlist.Where(p => p.Code.Contains(code)).ToList();
                }
        }
        public int InsertCustomer(Customer customer)
        {
                try
                {
                    //중복 검사               
                    if (_custlist != null)
                    {
                        foreach (var item in _custlist)
                        {
                            if (item.Code.Equals(customer.Code))
                            {
                                return 0;
                            }
                        }
                    }
                    _custlist.Add(customer);
                    return 1;
                }catch(Exception e)
                {
                    return 0;
                }
        }
        public int ModifyCustomer(Customer customer)
        {            
                var pursdk = new MemoryPurchaseSDK();
                var salesdk = new MemorySaleSDK();
                if (pursdk.isPurchaseCustCode(customer.Code) || salesdk.isSaleCustCode(customer.Code))
                {
                    return 0;
                }
                try
                {
                    Customer cust = _custlist.Find(x => x.Code.Equals(customer.Code));
                    cust.Code = customer.Code;
                    cust.Name = customer.Name;
                    return 1;
                }
                catch (Exception e)
                {
                    return -1;
                }
        }
        public int DeleteCustomer(string code)
        {
                var pursdk = new MemoryPurchaseSDK();
                var salesdk = new MemorySaleSDK();
                if (pursdk.isPurchaseCustCode(code) || salesdk.isSaleCustCode(code))
                {
                    return 0;
                }
                try
                {
                    Customer cust = _custlist.Find(x => x.Code.Equals(code));
                    _custlist.Remove(cust);
                    return 1;
                }
                catch (Exception e)
                {
                    return 0;
                }
        }
    }
}
