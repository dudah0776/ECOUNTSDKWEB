using ECount.CoreBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    [Serializable]
    public class Product:CodeBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
        //public ProductType Type;
        public string Type { get; set; }
        public Product(){}
        public Product(string code, string name, string Type)
        {
            this.Code = code;
            this.Name = name;
            this.Type = Type;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", this.Code, this.Name, this.Type);

        }
    }
}
