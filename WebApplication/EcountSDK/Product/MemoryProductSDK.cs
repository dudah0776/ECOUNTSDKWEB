using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using ECount.CoreBase;

namespace EcountSDK
{
    public class MemoryProductSDK:IProductSDK
    {
        private static List<Product> _products = new List<Product>();

        public List<Product> GetProductList()
        {
            return _products;
        }
        public List<Product> GetSearchedList(string code, string type)
        {       
            if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(type))
            {
                return _products;
            }
            else if (string.IsNullOrEmpty(code))
            {
                // code가 비어있는 경우
                return _products.Where(p => p.Type.Contains(type)).ToList();
            }
            else if (string.IsNullOrEmpty(type))
            {
                // type이 비어있는 경우
                return _products.Where(p => p.Code.Contains(code)).ToList();
            }
            else
            {
                // code와 type 모두 비어있지 않은 경우
                return _products.Where(p => p.Code.Contains(code) && p.Type.Contains(type)).ToList();
            }
        }
        public int InsertProduct(Product product)
        {
            try
            {
                //중복 검사               
                if (_products != null)
                {
                    foreach (var item in _products)
                    {
                        if (item.Code.Equals(product.Code))
                        {
                            return 0;
                        }
                    }
                }
                _products.Add(product);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public int ModifyProduct(Product product)
        {
            try
            {
                Product prod = _products.Find(x => x.Code.Equals(product.Code));
                prod.Name = product.Name;
                prod.Type = product.Type;
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public int DeleteProduct(string code)
        {
            try
            {
                Product prod = _products.Find(x => x.Code.Equals(code));
                _products.Remove(prod);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public void Create(string code, string name, string type)
        {
            var formatter = new BinaryFormatter();
            //파일이 존재하지 않을 시
            if (!File.Exists("product.dat"))
            {
                List<Product> result = new List<Product>();
                result.Add(new Product(code, name, type));
                FileStore.Save(result);
            }
            //파일이 존재할 시
            else
            {
                bool code_exist = false;
                List<Product> saved_products = null;
                using (var readStream = File.OpenRead("product.dat"))
                {
                    saved_products = (List<Product>)formatter.Deserialize(readStream);
                    readStream.Close();
                }
                //중복 검사               
                if (saved_products != null)
                {
                    foreach (var item in saved_products)
                    {
                        if (item.Code.Equals(code))
                        {
                            code_exist = true;
                            break;
                        }
                    }
                }
                //중복값이 없을 시 저장
                if (!code_exist)
                {
                    saved_products.Add(new Product(code, name, type));
                    FileStore.Save(saved_products);
                }
            }

        }

        public List<Product> Getproductlist()
        {
            return FileStore.Getproductlist();
        }
        public Product GetProduct(string code)
        {
            return _products.Find(x => x.Code == code);
        }

        public List<Product> GetproductByName(string Name)
        {
            return _products.FindAll(x => x.Name == Name);
        }
        public List<Product> GetproductByType(string Type)
        {
            return _products.FindAll(x => x.Type == Type);
        }
    }


}
