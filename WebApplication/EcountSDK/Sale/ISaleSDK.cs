using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    public interface ISaleSDK
    {
        int SaleCount(string code, DateTime date);
        bool isSaleCode(string code);
        bool isSaleCustCode(string code);
        int Insert(Sale sale);
        List<Sale> GetSaleList();
        List<Sale> GetSearchedList(string pCode, string cCode);
        int ModifySale(Sale sale);
        int DeleteSale(string sid);
    }
}
