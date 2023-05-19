using EcountSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleERPWeb.Models
{
    public class SaleManager
    {
        public ISaleSDK sdk;

        public SaleManager(string StoreType)
        {
            if (StoreType == "db")
            {
                sdk = new DBSaleSDK();
            }
            else if (StoreType == "memory")
            {
                sdk = new MemorySaleSDK();
            }
            else
            {
                sdk = null;
            }
        }
    }
}