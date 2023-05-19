using ECount.CoreBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECount.CoreBase
{
    public interface ILogger
    {
        void Write<T>(T data, string AddInfo)where T:IBase;
    }
}
