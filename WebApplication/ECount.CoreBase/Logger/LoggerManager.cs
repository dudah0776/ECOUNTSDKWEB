using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECount.CoreBase
{
    public class LoggerManager
    {
        public static string type;
        public static ILogger LoggerFactory(string type)
        {
            if(type.ToLower() == LoggerType.console.ToString())
            {
                return new ConsoleLogger();
            }
            if (type.ToLower() == LoggerType.file.ToString())
            {
                return new FileLogger();
            }
            if (type.ToLower() == LoggerType.db.ToString())
            {
                return new DBLogger();
            }
            throw new Exception("type 정보에 일치하는 클래스가 없습니다.");
        }
    }
    public enum LoggerType{
        console,
        file,
        db
    }
}
