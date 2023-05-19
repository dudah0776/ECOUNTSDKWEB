using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ECount.CoreBase;
namespace EcountSDK
{
    public class DBPurchaseSDK : IPurchaseSDK
    {        
        ILogger log;
        Log logger = new Log();
        SqlConnectHelper sqlhelper = new SqlConnectHelper();
        DBProductSDK prodsdk = new DBProductSDK();
        DBCustomerSDK custsdk = new DBCustomerSDK();
        public DBPurchaseSDK()
        {
            string logType = LoggerManager.type;
            log = LoggerManager.LoggerFactory(logType);
        }
        public int DeletePurchase(string pid)
        {
            string sql = "EXEC DELETE_PURCHASE_KYM @PID";
            SqlCommand cmd = sqlhelper.GetCommand(sql);
            cmd.Parameters.AddWithValue("@PID", pid);
            var count = cmd.ExecuteNonQuery();
            return count;
        }

        public List<Purchase> GetPurchaseList()
        {
            List<Purchase> purchaselist = new List<Purchase>();
            string sql = "EXEC SELECT_ALL_PURCHASE_KYM";
            SqlDataReader reader = sqlhelper.GetDataReader(sql);
            while (reader.Read())
            {
                var purchase = new Purchase();
                purchase.pid = (int)reader["PID"];
                //purchase.Product.Code = reader["PRODUCTCODE"] as string;
                var product = prodsdk.GetProduct(reader["PRODUCTCODE"] as string);
                purchase.Product = product;
                //purchase.customer.Code = reader["CUSTOMERCODE"] as string;
                var customer = custsdk.GetCustomer(reader["CUSTOMERCODE"] as string);
                purchase.customer = customer;
                purchase.Quantity = (int)reader["QUANTITY"];                
                purchase.date = (DateTime)reader["DATE"];
                purchaselist.Add(purchase);
            }
            reader.Close();
            return purchaselist;
        }

        public List<Purchase> GetSearchedList(string pCode, string cCode)
        {
            if (pCode == null) pCode = "";
            if (cCode == null) cCode = "";
            List<Purchase> purchaselist = new List<Purchase>();
            //sql 인젝션 방지를 위해 parameter addwithvalue사용
            string sql = "EXEC SEARCH_PURCHASE_KYM @PRODUCTCODE, @CUSTOMERCODE";
            SqlCommand cmd = sqlhelper.GetCommand(sql);
            cmd.Parameters.AddWithValue("@PRODUCTCODE", pCode);
            cmd.Parameters.AddWithValue("@CUSTOMERCODE", cCode);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var purchase = new Purchase();
                if (purchase.Product == null)
                {
                    purchase.Product = new Product();
                }
                if (purchase.customer == null)
                {
                    purchase.customer = new Customer();
                }
                purchase.pid = (int)reader["PID"];
                purchase.Product.Code = reader["PRODUCTCODE"] as string;
                purchase.customer.Code = reader["CUSTOMERCODE"] as string;
                purchase.Quantity = (int)reader["QUANTITY"];
                //예외 발생 가능성 있음
                purchase.date = (DateTime)reader["DATE"];
                purchaselist.Add(purchase);
            }
            reader.Close();
            return purchaselist;
        }

        public int Insert(Purchase purchase)
        {
            string sql = "EXEC INSERT_PURCHASE_KYM @PRODUCTCODE, @CUSTOMERCODE, @QUANTITY, @DATE";
            SqlCommand cmd = sqlhelper.GetCommand(sql);
            cmd.Parameters.AddWithValue("@PRODUCTCODE", purchase.Product.Code);
            cmd.Parameters.AddWithValue("@CUSTOMERCODE", purchase.customer.Code);
            cmd.Parameters.AddWithValue("@QUANTITY", purchase.Quantity);
            cmd.Parameters.AddWithValue("@DATE", purchase.date.ToString("yyyy-MM-dd"));
            var result = cmd.ExecuteNonQuery();
            var product = prodsdk.GetProduct(purchase.Product.Code);
            var customer = custsdk.GetCustomer(purchase.customer.Code);
            purchase.Product = product;
            purchase.customer = customer;

            var logger = new Log(purchase.Product.Code, purchase.Product.Name, purchase.Product.Type);            
            log.Write<Log>(logger, "구매입력");
            return result;
        }
        public int ModifyPurchase(Purchase purchase)
        {
            string sql = "EXEC MODIFY_PURCHASE_KYM @PID, @PRODUCTCODE, @CUSTOMERCODE, @QUANTITY, @DATE";
            SqlCommand cmd = sqlhelper.GetCommand(sql);
            cmd.Parameters.AddWithValue("@PID", purchase.pid);
            cmd.Parameters.AddWithValue("@PRODUCTCODE", purchase.Product.Code);
            cmd.Parameters.AddWithValue("@CUSTOMERCODE", purchase.customer.Code);
            cmd.Parameters.AddWithValue("@QUANTITY", purchase.Quantity);
            cmd.Parameters.AddWithValue("@DATE", purchase.date.ToString("yyyy-MM-dd"));
            var result = cmd.ExecuteNonQuery();
            return result;
        }
        public int PurchaseCount(string code, DateTime date)
        {
            var invensdk = new DBInventorySDK();
            var purchaseList = invensdk.GetPurchases(date);
            var list = purchaseList.Where(x => x.Product.Code.Equals(code)).ToList();
            int result = 0;
            foreach (var item in list)
            {
                result += item.Quantity;
            }
            return result;
        }

        //구매 품목에 상품코드가 있는지 확인
        public bool isPurchaseCode(string code)
        {
            var purchaseList = GetPurchaseList();
            bool isCodeIncluded = purchaseList.Any(item => item.Product.Code.Contains(code));
            return isCodeIncluded;
        }
        //구매 품목에 거래처코드가 있는지 확인
        public bool isPurchaseCustCode(string code)
        {
            var purchaseList = GetPurchaseList();
            bool isCodeIncluded = purchaseList.Any(item => item.customer.Code.Contains(code));
            return isCodeIncluded;
        }
    }
}
