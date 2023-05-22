using EcountSDK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EcountSDK
{
    public class SqlConnectHelper
    {
        private string connectString;

        public SqlConnectHelper()
        {
            this.connectString = "**********************************";
        }
        public SqlConnectHelper(string connectString)
        {
            this.connectString = connectString;
        }
        public SqlCommand GetCommand(string sql)
        {
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            return cmd;
        }
        //select문 실행할때
        public SqlDataReader GetDataReader(string sql)
        {
            SqlCommand cmd = GetCommand(sql);
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return rdr;
        }      
        //insert, update, delete문 할때
        public int ExecuteQuery(string sql)
        {
            SqlCommand cmd = GetCommand(sql);
            var count = cmd.ExecuteNonQuery();
            return count;
        }
    }
}
