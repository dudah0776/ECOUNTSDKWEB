using ECount.CoreBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    public interface ICommonSDK
    {
        int CommonDelete(string code, string query);

        List<T> GetCommonList<T>(string sql) where T : CodeBase, new();
        int InsertCommon<T>(T temp, string query) where T : CodeBase;
        int ModifyCommon<T>(T temp, string query) where T : CodeBase;
    }
}
