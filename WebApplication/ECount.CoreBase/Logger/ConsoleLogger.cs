using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECount.CoreBase;

namespace ECount.CoreBase
{
    public class ConsoleLogger : ILogger
    {
        public void Write<T>(T data, string AddInfo) where T : IBase
        {
            throw new NotImplementedException();
        }
    }
}
