using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace EcountSDK
{
    public class MemorySaleSDK:ISaleSDK
    {
        private static List<Sale> _sales = new List<Sale>();
        private static int sid = 0;

        public int SaleCount(string code, DateTime date)
        {
            var invensdk = new MemoryInventorySDK();
            var saleList = invensdk.GetSales(date);
            var list = saleList.Where(x => x.Product.Code.Equals(code)).ToList();
            int result = 0;
            foreach (var item in list)
            {
                result += item.Quantity;
            }
            return result;
        }
        public bool isSaleCode(string code)
        {
            var saleList = GetSaleList();
            bool isCodeIncluded = saleList.Any(item => item.Product.Code.Contains(code));
            return isCodeIncluded;
        }
        public bool isSaleCustCode(string code)
        {
            var saleList = GetSaleList();
            bool isCodeIncluded = saleList.Any(item => item.customer.Code.Contains(code));
            return isCodeIncluded;
        }
        public int Insert(Sale sale)
        {


            try
            {
                MemoryPurchaseSDK sdk = new MemoryPurchaseSDK();
                int saleCount = SaleCount(sale.Product.Code, sale.date) + sale.Quantity;
                if (saleCount > sdk.PurchaseCount(sale.Product.Code, sale.date))
                {
                    return 0;
                }
                sid++;
                sale.sid = sid;
                _sales.Add(sale);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }

        }
        public List<Sale> GetSaleList()
        {


            return _sales;

        }
        public List<Sale> GetSearchedList(string pCode, string cCode)
        {


            if (string.IsNullOrEmpty(pCode) && string.IsNullOrEmpty(cCode))
            {
                return _sales;
            }
            else if (string.IsNullOrEmpty(pCode))
            {
                // code가 비어있는 경우
                return _sales.Where(p => p.customer.Code.Contains(cCode)).ToList();
            }
            else if (string.IsNullOrEmpty(cCode))
            {
                // type이 비어있는 경우
                return _sales.Where(p => p.Product.Code.Contains(pCode)).ToList();
            }
            else
            {
                // code와 type 모두 비어있지 않은 경우
                return _sales.Where(p => p.Product.Code.Contains(pCode) && p.customer.Code.Contains(cCode)).ToList();
            }

        }
        public int ModifySale(Sale sale)
        {



            try
            {
                Sale s = _sales.Find(x => x.sid.Equals(sale.sid));
                s.Product.Code = sale.Product.Code;
                s.customer.Code = sale.customer.Code;
                s.Quantity = sale.Quantity;
                s.date = sale.date;
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }

        }
        public int DeleteSale(string sid)
        {


            Sale s = _sales.Find(x => x.sid.Equals(int.Parse(sid)));
            _sales.Remove(s);
            return 1;

        }

        public void Create(string code, int quantity, DateTime date)
        {
            //code로 받은 상품 정보 product.dat에서 가져오기
            var formatter = new BinaryFormatter();
            List<Product> products = null;
            using (var readStream = File.OpenRead("product.dat"))
            {
                products = (List<Product>)formatter.Deserialize(readStream);
                readStream.Close();
            }
            Product target_product = products.Find(x => x.Code == code);
            //Console.WriteLine("구매:"+target_product.ToString());
            //구매하고자 하는 품목이 상품 품목에 있을 때
            if (target_product != null)
            {
                //구매정보 없을때
                if (!File.Exists("sale.dat"))
                {
                    List<Sale> result = new List<Sale>();
                    result.Add(new Sale(target_product, quantity, date));
                    FileStore.Save(result);
                    //Console.WriteLine("구매 품목 파일 생성 후 저장완료");
                }
                //구매정보 있을때
                else
                {
                    List<Sale> saved_sale = null;
                    using (var readStream = File.OpenRead("sale.dat"))
                    {
                        saved_sale = (List<Sale>)formatter.Deserialize(readStream);
                        readStream.Close();
                    }
                    saved_sale.Add(new Sale(target_product, quantity, date));
                    //저장
                    FileStore.Save(saved_sale);
                    //Console.WriteLine("구매 품목 저장 완료");
                }
            }
        }

        public List<Sale> GetHistory()
        {
            return FileStore.getsalelist();
        }
        public List<Sale> GetHistoryByCode(string code)
        {
            return _sales.FindAll(x => x.Product.Code == code);
        }

        public List<Sale> GetHistoryByName(string name)
        {
            return _sales.FindAll(x => x.Product.Name == name);
        }

        public List<Sale> GetHistoryByType(string type)
        {
            return _sales.FindAll(x => x.Product.Type == type);
        }
    }
}
