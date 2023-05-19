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
    public class MemoryPurchaseSDK:IPurchaseSDK
    {
        private static List<Purchase> _purchases = new List<Purchase>();
        private static int pid = 0;


        public int PurchaseCount(string code, DateTime date)
        {
            var invensdk = new MemoryInventorySDK();
            var purchaseList = invensdk.GetPurchases(date);
            var list = purchaseList.Where(x => x.Product.Code.Equals(code)).ToList();
            int result = 0;
            foreach (var item in list)
            {
                result += item.Quantity;
            }
            return result;
        }

        //구매 품목에 상품코드가 있는지 확인
        public bool isPurchaseCode(string code)
        {
            var purchaseList = GetPurchaseList();
            bool isCodeIncluded = purchaseList.Any(item => item.Product.Code.Contains(code));
            return isCodeIncluded;
        }
        //구매 품목에 거래처코드가 있는지 확인
        public bool isPurchaseCustCode(string code)
        {
            var purchaseList = GetPurchaseList();
            bool isCodeIncluded = purchaseList.Any(item => item.customer.Code.Contains(code));
            return isCodeIncluded;
        }

        public int Insert(Purchase purchase)
        {


            try
            {
                pid++;
                purchase.pid = pid;
                _purchases.Add(purchase);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }

        }
        public List<Purchase> GetPurchaseList()
        {
            return _purchases;
        }

        public List<Purchase> GetSearchedList(string pCode, string cCode)
        {


            if (string.IsNullOrEmpty(pCode) && string.IsNullOrEmpty(cCode))
            {
                return _purchases;
            }
            else if (string.IsNullOrEmpty(pCode))
            {
                // code가 비어있는 경우
                return _purchases.Where(p => p.customer.Code.Contains(cCode)).ToList();
            }
            else if (string.IsNullOrEmpty(cCode))
            {
                // type이 비어있는 경우
                return _purchases.Where(p => p.Product.Code.Contains(pCode)).ToList();
            }
            else
            {
                // code와 type 모두 비어있지 않은 경우
                return _purchases.Where(p => p.Product.Code.Contains(pCode) && p.customer.Code.Contains(cCode)).ToList();
            }

        }
        public int ModifyPurchase(Purchase purchase)
        {

            try
            {
                Purchase pur = _purchases.Find(x => x.pid.Equals(purchase.pid));
                pur.Product.Code = purchase.Product.Code;
                pur.customer.Code = purchase.customer.Code;
                pur.Quantity = purchase.Quantity;
                pur.date = purchase.date;
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }

        }
        public int DeletePurchase(string pid)
        {



            try
            {
                Purchase purchase = _purchases.Find(x => x.pid.Equals(int.Parse(pid)));
                _purchases.Remove(purchase);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }

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
                if (!File.Exists("purchases.dat"))
                {
                    List<Purchase> result = new List<Purchase>();
                    result.Add(new Purchase(target_product, quantity, date));
                    FileStore.Save(result);
                    //Console.WriteLine("구매 품목 파일 생성 후 저장완료");
                }
                //구매정보 있을때
                else
                {
                    List<Purchase> saved_purchase = null;
                    using (var readStream = File.OpenRead("purchases.dat"))
                    {
                        saved_purchase = (List<Purchase>)formatter.Deserialize(readStream);
                        readStream.Close();
                    }
                    saved_purchase.Add(new Purchase(target_product, quantity, date));
                    //저장
                    FileStore.Save(saved_purchase);
                    //Console.WriteLine("구매 품목 저장 완료");
                }
            }

        }

        public List<Purchase> GetHistory()
        {
            return FileStore.getpurchaselist();
        }

        public List<Purchase> GetHistory(string code)
        {
            return _purchases.FindAll(x => x.Product.Code == code);
        }


        public List<Purchase> GetHistoryByName(string name)
        {
            return _purchases.FindAll(x => x.Product.Name == name);
        }


        public List<Purchase> GetHistoryByType(string type)
        {
            return _purchases.FindAll(x => x.Product.Type == type);
        }
    }
}
