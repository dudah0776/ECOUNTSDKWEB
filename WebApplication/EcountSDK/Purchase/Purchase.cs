using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    [Serializable]
    public class Purchase
    {
        public int pid { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime date { get; set; }
        public Customer customer { get; set; }
        public Purchase() { }
        
        public Purchase(Product product, int quantity, DateTime date)
        {
            this.Product = product;
            this.Quantity = quantity;
            this.date = date;
        }
        public Purchase(Product product, int quantity, DateTime date, Customer customer)
        {
            this.Product = product;
            this.Quantity = quantity;
            this.date = date;
            this.customer = customer;
        }
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", 
                this.Product.Code, this.Product.Name, 
                this.Product.Type, this.Quantity, this.date);
        }
    }
}
