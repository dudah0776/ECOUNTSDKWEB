using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    public class DBProductSDK : IProductSDK
    {
        private SqlConnectHelper sqlhelper = new SqlConnectHelper();

        public int DeleteProduct(string code)
        {
            var pursdk = new DBPurchaseSDK();
            var salesdk = new DBSaleSDK();
            if (pursdk.isPurchaseCode(code) || salesdk.isSaleCode(code))
            {
                return 0;
            }
            string sql = "EXEC DELETE_PRODUCT_KYM @Code";
            SqlCommand cmd = sqlhelper.GetCommand(sql);
            cmd.Parameters.AddWithValue("@Code", code);
            var count = cmd.ExecuteNonQuery();
            return count;
        }

        public List<Product> GetProductList()
        {
            List<Product> prodlist = new List<Product>();
            string sql = "SELECT_ALL_PRODUCT_KYM";
            SqlDataReader reader = sqlhelper.GetDataReader(sql);
            while (reader.Read())
            {
                var product = new Product();
                product.Code = reader["CODE"] as string;
                product.Name = reader["NAME"] as string;
                product.Type = reader["TYPE"] as string;
                prodlist.Add(product);
            }
            reader.Close();
            return prodlist;
        }
        public Product GetProduct(string code)
        {
            string sql = "SELECT * FROM PRODUCT_KYM WHERE CODE=@Code";
            SqlCommand cmd = sqlhelper.GetCommand(sql);
            cmd.Parameters.AddWithValue("@Code", code);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var product = new Product();
                    product.Code = reader["CODE"] as string;
                    product.Name = reader["NAME"] as string;
                    product.Type = reader["TYPE"] as string;
                    return product;
                }
                else
                {
                    return null;
                }                
            }                       
        }

        public List<Product> GetSearchedList(string code, string type)
        {
            if (code == null) code = "";
            if (type == null) type = "";
            List<Product> prodlist = new List<Product>();
            //sql 인젝션 방지를 위해 parameter addwithvalue사용
            string sql = "EXEC SEARCH_PRODUCT_KYM @Code, @Type";
            SqlCommand cmd = sqlhelper.GetCommand(sql);
            cmd.Parameters.AddWithValue("@Code", code);
            cmd.Parameters.AddWithValue("@Type", type);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var product = new Product();
                product.Code = reader["CODE"] as string;
                product.Name = reader["NAME"] as string;
                product.Type = reader["TYPE"] as string;
                prodlist.Add(product);
            }
            reader.Close();
            return prodlist;
        }

        public int InsertProduct(Product product)
        {
            string sql = string.Format("EXEC INSERT_PRODUCT_KYM {0}, {1}, {2}",
                product.Code, product.Name, product.Type);
            int result = sqlhelper.ExecuteQuery(sql);
            return result;
        }

        public int ModifyProduct(Product product)
        {
            var pursdk = new DBPurchaseSDK();
            var salesdk = new DBSaleSDK();
            if (pursdk.isPurchaseCode(product.Code) || salesdk.isSaleCode(product.Code))
            {
                return 0;
            }
            string sql = string.Format("EXEC MODIFY_PRODUCT_KYM {0}, {1}, {2}",
            product.Code, product.Name, product.Type);
            int result = sqlhelper.ExecuteQuery(sql);
            return result;
        }
    }
}
