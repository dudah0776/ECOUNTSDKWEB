using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    public class DBCustomerSDK : ICustomerSDK
    {
        private SqlConnectHelper sqlhelper = new SqlConnectHelper();
        public int DeleteCustomer(string code)
        {
            var pursdk = new DBPurchaseSDK();
            var salesdk = new DBSaleSDK();
            if (pursdk.isPurchaseCustCode(code) || salesdk.isSaleCustCode(code))
            {
                return 0;
            }
            string sql = "EXEC DELETE_CUSTOMER_KYM @Code";
            SqlCommand cmd = sqlhelper.GetCommand(sql);
            cmd.Parameters.AddWithValue("@Code", code);
            var count = cmd.ExecuteNonQuery();
            return count;
        }

        public List<Customer> GetCustomerList()
        {
            List<Customer> custlist = new List<Customer>();
            string sql = "SELECT_ALL_CUSTOMER_KYM";
            SqlDataReader reader = sqlhelper.GetDataReader(sql);
            while (reader.Read())
            {
                var customer = new Customer();
                customer.Code = reader["CODE"] as string;
                customer.Name = reader["NAME"] as string;
                custlist.Add(customer);
            }
            reader.Close();
            return custlist;
        }
        public Customer GetCustomer(string code)
        {
            string sql = "SELECT * FROM CUSTOMER_KYM WHERE CODE=@Code";
            SqlCommand cmd = sqlhelper.GetCommand(sql);
            cmd.Parameters.AddWithValue("@Code", code);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var customer = new Customer();
                    customer.Code = reader["CODE"] as string;
                    customer.Name = reader["NAME"] as string;
                    return customer;
                }
                else
                {
                    return null;
                }
            }
        }
        public List<Customer> GetSearchedList(string code)
        {
            if (code == null) code = "";
            List<Customer> custlist = new List<Customer>();
            //sql 인젝션 방지를 위해 parameter addwithvalue사용
            string sql = "EXEC SEARCH_CUSTOMER_KYM @Code";
            SqlCommand cmd = sqlhelper.GetCommand(sql);
            cmd.Parameters.AddWithValue("@Code", code);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var customer = new Customer();
                customer.Code = reader["CODE"] as string;
                customer.Name = reader["NAME"] as string;
                custlist.Add(customer);
            }
            reader.Close();
            return custlist;
        }

        public int InsertCustomer(Customer customer)
        {
            string sql = string.Format("EXEC INSERT_CUSTOMER_KYM {0}, {1}",
                customer.Code, customer.Name);
            int result = sqlhelper.ExecuteQuery(sql);
            return result;
        }

        public int ModifyCustomer(Customer customer)
        {
            var pursdk = new DBPurchaseSDK();
            var salesdk = new DBSaleSDK();
            if (pursdk.isPurchaseCustCode(customer.Code) || salesdk.isSaleCustCode(customer.Code))
            {
                return 0;
            }
            string sql = string.Format("EXEC MODIFY_CUSTOMER_KYM {0}, {1}",
                customer.Code, customer.Name);
            int result = sqlhelper.ExecuteQuery(sql);
            return result;
        }
    }
}
