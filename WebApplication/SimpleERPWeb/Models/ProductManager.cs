using EcountSDK;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SimpleERPWeb.Models
{
    public class ProductManager
    {
        public IProductSDK sdk;
        public ICommonSDK csdk;
        public ProductManager(string StoreType)
        {
            if (StoreType == "db")
            {                
                //csdk = new CommonDBSDK();
                sdk = new DBProductSDK();
            }
            else if (StoreType == "memory")
            {
                sdk = new MemoryProductSDK();
            }
            else
            {
                sdk = null;
            }
        }
    }
}