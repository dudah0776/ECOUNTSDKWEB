using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    public static class SQL
    {
        public static string DeleteCustomerSP = "EXEC DELETE_CUSTOMER_KYM @Code";
        public static string GetCustomerSP = "EXEC SELECT_ALL_CUSTOMER_KYM";
        public static string InsertCustomerSP = "EXEC INSERT_CUSTOMER_KYM @Code, @Name";
        public static string ModifyCustomerSP = "EXEC MODIFY_CUSTOMER_KYM @Code, @Name";

        public static string DeleteProductSP = "EXEC DELETE_PRODUCT_KYM @Code";
        public static string GetProductSP = "EXEC SELECT_ALL_PRODUCT_KYM";
        public static string InsertProductSP = "EXEC INSERT_PRODUCT_KYM @Code, @Name, @Type";
        public static string ModifyProductSP = "EXEC MODIFY_PRODUCT_KYM @Code, @Name, @Type";

    }
}
