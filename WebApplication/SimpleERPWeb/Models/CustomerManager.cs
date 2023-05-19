using EcountSDK;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SimpleERPWeb.Models
{
    public class CustomerManager
    {
        public ICustomerSDK sdk;
        public ICommonSDK csdk;

        public CustomerManager(string StoreType)
        {
            if (StoreType == "db")
            {
                //csdk = new CommonDBSDK();
                sdk = new DBCustomerSDK();
            }
            else if (StoreType == "memory")
            {
                sdk = new MemoryCustomerSDK();
            }
            else
            {
                sdk = null;
            }
        }
    }
}