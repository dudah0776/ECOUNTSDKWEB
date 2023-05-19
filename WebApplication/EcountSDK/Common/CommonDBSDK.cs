using ECount.CoreBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    public class CommonDBSDK:ICommonSDK
    {
        private SqlConnectHelper sqlhelper = new SqlConnectHelper();

        public int CommonDelete(string code, string query)
        {
            var pursdk = new DBPurchaseSDK();
            var salesdk = new DBSaleSDK();
            if (pursdk.isPurchaseCustCode(code) || salesdk.isSaleCustCode(code))
            {
                return 0;
            }
            string sql = query;
            SqlCommand cmd = sqlhelper.GetCommand(sql);
            cmd.Parameters.AddWithValue("@Code", code);
            var count = cmd.ExecuteNonQuery();
            return count;
        }

        public List<T> GetCommonList<T>(string sql)
            where T:CodeBase, new() 
        {
            List<T> templist = new List<T>();                       
            SqlDataReader reader = sqlhelper.GetDataReader(sql);
            if(reader!=null && reader.HasRows)
            {
                var schemaTable = reader.GetSchemaTable();
                bool hasTypeColumn = schemaTable.Columns.Contains("TYPE");
                while (reader.Read())
                {
                    string name = typeof(T).Name;
                    var item = new T();
                    item.Code = reader["CODE"] as string;
                    item.Name = reader["NAME"] as string;
                    if (name=="Product")
                    {
                        item.Type = reader["TYPE"] as string;
                    }
                    templist.Add(item);
                }
            }            
            reader.Close();
            return templist;
        }
        public int InsertCommon<T>(T temp, string query)
            where T:CodeBase
        {                        
            string sql = query;
            SqlCommand cmd = sqlhelper.GetCommand(sql);                        
            cmd.Parameters.AddWithValue("@Code", temp.Code);
            cmd.Parameters.AddWithValue("@Name", temp.Name);
            if (temp.Type != null)
            {
                cmd.Parameters.AddWithValue("@Type", temp.Type);
            }
            var result = cmd.ExecuteNonQuery();
            return result;
        }
        public int ModifyCommon<T>(T temp, string query)
            where T:CodeBase
        {
            var pursdk = new DBPurchaseSDK();
            var salesdk = new DBSaleSDK();
            if (pursdk.isPurchaseCustCode(temp.Code) || salesdk.isSaleCustCode(temp.Code)|| salesdk.isSaleCode(temp.Code))
            {
                return 0;
            }
            string sql = query;
            SqlCommand cmd = sqlhelper.GetCommand(sql);
            cmd.Parameters.AddWithValue("@Code", temp.Code);
            cmd.Parameters.AddWithValue("@Name", temp.Name);
            if (temp.Type != null)
            {
                cmd.Parameters.AddWithValue("@Type", temp.Type);
            }
            var result = cmd.ExecuteNonQuery();
            return result;
        }
    }
}
