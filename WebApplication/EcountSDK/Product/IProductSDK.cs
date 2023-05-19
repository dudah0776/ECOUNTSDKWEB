using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    public interface IProductSDK
    {
        List<Product> GetProductList();
        List<Product> GetSearchedList(string code, string type);
        int InsertProduct(Product product);
        int ModifyProduct(Product product);
        int DeleteProduct(string code);
    }
}
