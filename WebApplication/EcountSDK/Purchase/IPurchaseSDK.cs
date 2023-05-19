using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    public interface IPurchaseSDK
    {
        int PurchaseCount(string code, DateTime date);
        bool isPurchaseCode(string code);
        bool isPurchaseCustCode(string code);
        int Insert(Purchase purchase);
        List<Purchase> GetPurchaseList();
        List<Purchase> GetSearchedList(string pCode, string cCode);
        int ModifyPurchase(Purchase purchase);
        int DeletePurchase(string pid);
    }
}
