using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcountSDK;

namespace SimpleERPWeb.Models
{
    public class InventoryManager
    {
        public IinventorySDK sdk;

        public InventoryManager(string StoreType)
        {
            if (StoreType == "db")
            {
                sdk = new DBInventorySDK();
            }
            else if (StoreType == "memory")
            {
                sdk = new MemoryInventorySDK();
            }
            else
            {
                sdk = null;
            }
        }
    }
}