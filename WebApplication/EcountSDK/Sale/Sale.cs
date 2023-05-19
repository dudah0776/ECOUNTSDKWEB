using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    [Serializable]
    public class Sale
    {
        public int sid { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime date { get; set; }
        public Customer customer { get; set; }

        public Sale() { }
        public Sale(Product product, int quantity, DateTime date)
        {
            this.Product = product;
            this.Quantity = quantity;
            this.date = date;
        }
        public Sale(Product product, int quantity, DateTime date, Customer customer)
        {
            this.Product = product;
            this.Quantity = quantity;
            this.date = date;
            this.customer = customer;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}",
                this.Product.Code, this.Product.Name, this.Quantity, this.date);
        }
    }
}
