using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECount.CoreBase
{
    public static class ConditionExtentions
    {
        public static int vIndexOf(this string source, char value)
        {
            if (source == null)
            {
                return 0;
            }
            return source.IndexOf(value);
        }

        #region [Bool Type]
        public static bool vIsNull(this bool? source)
        {
            if (source == null) return true;
            
            else    return false;       
        }

        public static bool vIsEmpty(this bool? source)
        {
            return source.vIsNull();
        }

        public static bool vIsNotEmpty(this bool? source)
        {            
            return !source.vIsNull();         
        }

        public static bool vIsDefault(this bool source)
        {
            if (default(bool) == source)    return true;            
            else return false;        
        }
        #endregion

        #region [String Type]
        public static bool vIsNull(this string source)
        {
            if (source == null) return true;
            else return false;
        }

        public static bool vIsEmpty(this string source)
        {
            if (source == "") return true;
            else return false;
        }

        public static bool vIsNotEmpty(this string source)
        {
            return !source.vIsNotEmpty();                
        }

        public static bool vIsDefault(this string source)
        {            
            return source.vIsNull();
        }
        #endregion

        #region [Int Type]
        public static bool vIsNull(this int? source)
        {            
            return source.HasValue;
        }

        public static bool vIsEmpty(this int? source)
        {
            return source.vIsNull();
        }

        public static bool vIsNotEmpty(this int? source)
        {            
            return source.vIsNull();
        }

        public static bool vIsDefault(this int source)
        {
            if (source == 0) return true;
            else return false;            
        }
        #endregion
    }
}
