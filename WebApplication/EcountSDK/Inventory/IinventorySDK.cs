using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    public interface IinventorySDK
    {
        void Refresh(List<Purchase> purchases, List<Sale> sales);
        List<Purchase> GetPurchases(DateTime date);
        List<Sale> GetSales(DateTime date);
        List<Inventory> GetStatus(string code, DateTime date);
        List<Inventory> GetStatus(DateTime date);
    }
}
