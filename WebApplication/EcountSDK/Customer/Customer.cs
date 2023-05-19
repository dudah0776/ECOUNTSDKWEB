using ECount.CoreBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    [Serializable]
    public class Customer:CodeBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public Customer() { }

        public Customer(string Code, string Name)
        {
            this.Code = Code;
            this.Name = Name;
        }
    }
}
