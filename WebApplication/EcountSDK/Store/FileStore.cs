using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
namespace EcountSDK
{
    public class FileStore
    {      
        // 파일명 : product.dat
        public static void Save(List<Product> products)
        {
            var formatter = new BinaryFormatter();
            using (var writeStream = File.OpenWrite("product.dat"))
            {
                formatter.Serialize(writeStream, products);
                writeStream.Close();
            }
        }

        // 파일명 : purchase.dat
        public static void Save(List<Purchase> purchases)
        {
            var formatter = new BinaryFormatter();
            using (var writeStream = File.OpenWrite("purchases.dat"))
            {
                formatter.Serialize(writeStream, purchases);
                writeStream.Close();
            }
        }

        // 파일명 : sale.dat
        public static void Save(List<Sale> sales)
        {
            var formatter = new BinaryFormatter();
            using (var writeStream = File.OpenWrite("sale.dat"))
            {
                formatter.Serialize(writeStream, sales);
                writeStream.Close();
            }
        }


        // 파일명 : product.dat
        public static List<Product> Getproductlist()
        {
            var formatter = new BinaryFormatter();
            List<Product> saved_products = null;
            using (var readStream = File.OpenRead("product.dat"))
            {
                saved_products = (List<Product>)formatter.Deserialize(readStream);
                readStream.Close();
            }
            return saved_products;
        }

        // 파일명 : purchases.dat
        public static List<Purchase> getpurchaselist()
        {
            var formatter = new BinaryFormatter();
            List<Purchase> saved_purchase = null;
            using (var readStream = File.OpenRead("purchases.dat"))
            {
                saved_purchase = (List<Purchase>)formatter.Deserialize(readStream);
                readStream.Close();
            }
            return saved_purchase;            
        }

        // 파일명 : sale.dat
        public static List<Sale> getsalelist()
        {
            var formatter = new BinaryFormatter();
            List<Sale> saved_sale = null;
            using (var readStream = File.OpenRead("sale.dat"))
            {
                saved_sale = (List<Sale>)formatter.Deserialize(readStream);
                readStream.Close();
            }
            return saved_sale;
        }
    }
}
