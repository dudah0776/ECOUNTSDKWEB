using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    public class DBSaleSDK : ISaleSDK
    {
        SqlConnectHelper sqlhelper = new SqlConnectHelper();
        public int DeleteSale(string sid)
        {
            string sql = "EXEC DELETE_SALE_KYM @SID";
            SqlCommand cmd = sqlhelper.GetCommand(sql);
            cmd.Parameters.AddWithValue("@SID", sid);
            var count = cmd.ExecuteNonQuery();
            return count;
        }

        public List<Sale> GetSaleList()
        {
            List<Sale> salelist = new List<Sale>();
            string sql = "EXEC SELECT_ALL_SALE_KYM";
            SqlDataReader reader = sqlhelper.GetDataReader(sql);
            while (reader.Read())
            {
                var sale = new Sale();
                if (sale.Product == null)
                {
                    sale.Product = new Product();
                }
                if (sale.customer == null)
                {
                    sale.customer = new Customer();
                }
                sale.sid = (int)reader["SID"];
                sale.Product.Code = reader["PRODUCTCODE"] as string;
                sale.customer.Code = reader["CUSTOMERCODE"] as string;
                sale.Quantity = (int)reader["QUANTITY"];
                //예외 발생 가능성 있음
                sale.date = (DateTime)reader["DATE"];
                salelist.Add(sale);
            }
            reader.Close();
            return salelist;
        }

        public List<Sale> GetSearchedList(string pCode, string cCode)
        {
            if (pCode == null) pCode = "";
            if (cCode == null) cCode = "";
            List<Sale> salelist = new List<Sale>();
            //sql 인젝션 방지를 위해 parameter addwithvalue사용
            string sql = "EXEC SEARCH_SALE_KYM @PRODUCTCODE, @CUSTOMERCODE";
            SqlCommand cmd = sqlhelper.GetCommand(sql);
            cmd.Parameters.AddWithValue("@PRODUCTCODE", pCode);
            cmd.Parameters.AddWithValue("@CUSTOMERCODE", cCode);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var sale = new Sale();
                if (sale.Product == null)
                {
                    sale.Product = new Product();
                }
                if (sale.customer == null)
                {
                    sale.customer = new Customer();
                }
                sale.sid = (int)reader["SID"];
                sale.Product.Code = reader["PRODUCTCODE"] as string;
                sale.customer.Code = reader["CUSTOMERCODE"] as string;
                sale.Quantity = (int)reader["QUANTITY"];
                //예외 발생 가능성 있음
                sale.date = (DateTime)reader["DATE"];
                salelist.Add(sale);
            }
            reader.Close();
            return salelist;
        }

        public int Insert(Sale sale)
        {
            DBPurchaseSDK sdk = new DBPurchaseSDK();
            int saleCount = SaleCount(sale.Product.Code, sale.date);
            if (saleCount > sdk.PurchaseCount(sale.Product.Code, sale.date))
            {
                return 0;
            }
            string sql = "EXEC INSERT_SALE_KYM @PRODUCTCODE, @CUSTOMERCODE, @QUANTITY, @DATE";
            SqlCommand cmd = sqlhelper.GetCommand(sql);
            cmd.Parameters.AddWithValue("@PRODUCTCODE", sale.Product.Code);
            cmd.Parameters.AddWithValue("@CUSTOMERCODE", sale.customer.Code);
            cmd.Parameters.AddWithValue("@QUANTITY", sale.Quantity);
            cmd.Parameters.AddWithValue("@DATE", sale.date.ToString("yyyy-MM-dd"));
            var result = cmd.ExecuteNonQuery();
            return result;
        }
        public int ModifySale(Sale sale)
        {
            string sql = "EXEC MODIFY_SALE_KYM @SID, @PRODUCTCODE, @CUSTOMERCODE, @QUANTITY, @DATE";
            SqlCommand cmd = sqlhelper.GetCommand(sql);
            cmd.Parameters.AddWithValue("@SID", sale.sid);
            cmd.Parameters.AddWithValue("@PRODUCTCODE", sale.Product.Code);
            cmd.Parameters.AddWithValue("@CUSTOMERCODE", sale.customer.Code);
            cmd.Parameters.AddWithValue("@QUANTITY", sale.Quantity);
            cmd.Parameters.AddWithValue("@DATE", sale.date.ToString("yyyy-MM-dd"));
            var result = cmd.ExecuteNonQuery();
            return result;
        }


        public int SaleCount(string code, DateTime date)
        {
            var invensdk = new DBInventorySDK();
            var saleList = invensdk.GetSales(date);
            var list = saleList.Where(x => x.Product.Code.Equals(code)).ToList();
            int result = 0;
            foreach (var item in list)
            {
                result += item.Quantity;
            }
            return result;
        }
        public bool isSaleCode(string code)
        {
            var saleList = GetSaleList();
            bool isCodeIncluded = saleList.Any(item => item.Product.Code.Contains(code));
            return isCodeIncluded;
        }
        public bool isSaleCustCode(string code)
        {
            var saleList = GetSaleList();
            bool isCodeIncluded = saleList.Any(item => item.customer.Code.Contains(code));
            return isCodeIncluded;
        }
    }
}
