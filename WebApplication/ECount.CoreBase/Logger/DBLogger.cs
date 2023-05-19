using ECount.CoreBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECount.CoreBase
{
    class DBLogger : ILogger
    {

        public void Write<T>(T data, string AddInfo) where T : IBase
        {
            string log = string.Format("{0} - {1} {2}전표가 {3}되었습니다.",data.Code,data.Name,data.Type,AddInfo);
            string connectString = "Server=10.10.9.241,25111;Database=ACCT_AC;uid=ecountdev;Pwd=acct@0000;";            
            string sql = "INSERT INTO CORELOG(LOG) VALUES('log');";
            sql = sql.Replace("log", log);
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            var count = cmd.ExecuteNonQuery();

        }
    }
}
