using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcountSDK;

namespace SimpleERPWeb.Models
{
    public class PurchaseManager
    {
        public IPurchaseSDK sdk;

        public PurchaseManager(string StoreType)
        {
            if (StoreType == "db")
            {
                sdk = new DBPurchaseSDK();
            }
            else if (StoreType == "memory")
            {
                sdk = new MemoryPurchaseSDK();
            }
            else
            {
                sdk = null;
            }
        }
    }
}