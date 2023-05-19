using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EcountSDK
{
    public class MemoryInventorySDK:IinventorySDK
    {
        private List<Purchase> _purchases = new List<Purchase>();
        private List<Sale> _sales = new List<Sale>();
        Dictionary<string, int> inventory = null;
        
        public void Refresh(List<Purchase> purchases, List<Sale> sales)
        {
            inventory = new Dictionary<string, int>();
            foreach (var item in purchases)
            {
                int quantity = 0;
                //중복 제거
                _purchases = purchases.FindAll(x => x.Product.Code == item.Product.Code);
                _sales = sales.FindAll(x => x.Product.Code == item.Product.Code);   
                foreach (var purchase in _purchases)
                {
                    quantity += purchase.Quantity;
                }
                foreach (var sale in _sales)
                {
                    quantity -= sale.Quantity;
                }
                //중복 키 검색
                if (!inventory.ContainsKey(item.Product.Code))
                {
                    inventory.Add(item.Product.Code, quantity);
                }                                
            }
            _purchases = new List<Purchase>();
            foreach(var item in purchases)
            {
                _purchases.Add(new Purchase(item.Product, item.Quantity, item.date));
            }
        }
        
        public List<Purchase> GetPurchases(DateTime date)
        {
            MemoryPurchaseSDK pursdk = new MemoryPurchaseSDK();
            List<Purchase> saved_purchase = pursdk.GetPurchaseList();
            List<Purchase> result = null;//날짜로 필터링한 구매정보
            if (saved_purchase != null && date!=null)
            {
                result = new List<Purchase>();
                foreach(var item in saved_purchase)
                {
                    if(DateTime.Compare(item.date, date) <= 0)
                    {
                        result.Add(new Purchase(item.Product, item.Quantity, item.date, item.customer));
                    }
                }
            }
            return result;
        }

        public List<Sale> GetSales(DateTime date)
        {
            MemorySaleSDK salesdk = new MemorySaleSDK();
            List<Sale> saved_sale = salesdk.GetSaleList();
            List<Sale> result = null;
            if (saved_sale != null && date!=null)
            {
                result = new List<Sale>();
                foreach (var item in saved_sale)
                {
                    if (DateTime.Compare(item.date, date) <= 0)
                    {
                        result.Add(new Sale(item.Product, item.Quantity, item.date, item.customer));
                    }
                }
            }
            return result;
        }
        public List<Inventory> GetStatus(DateTime date)
        {
            List<Purchase> purchases = GetPurchases(date);
            List<Sale> sales = GetSales(date);
            List<Inventory> inventories = null;
            if (purchases != null && sales != null)
            {
                inventories = new List<Inventory>();
                Refresh(purchases, sales);
                foreach (KeyValuePair<string, int> item in inventory)
                {
                    Purchase pur = _purchases.Find(x => x.Product.Code == item.Key);
                    Inventory inven = new Inventory();
                    inven.Product = new Product();
                    //품목 코드와 이름 분리 ex)cd001 콜라
                    string key = item.Key;
                    string[] keyarr = key.Split(' ');                    
                    inven.Product.Code = keyarr[0];
                    inven.Product.Name = keyarr[1];
                    inven.Quantity = item.Value;
                    inventories.Add(inven);
                }
            }
            return inventories;
        }
        public List<Inventory> GetStatus(string code, DateTime date)
        {
            List<Purchase> purchases = GetPurchases(date);
            List<Sale> sales = GetSales(date);
            List<Inventory> inventories = null;
            if (purchases != null && sales != null)
            {
                inventories = new List<Inventory>();
                Refresh(purchases, sales);
                foreach (KeyValuePair<string, int> item in inventory)
                {
                    Purchase pur = _purchases.Find(x => x.Product.Code == item.Key);
                    Inventory inven = new Inventory();
                    inven.Product = new Product();
                    string key = item.Key;
                    string[] keyarr = key.Split(' ');
                    //코드로 검색
                    if (keyarr[0].Contains(code))
                    {
                        inven.Product.Code = keyarr[0];
                        inven.Product.Name = keyarr[1];
                        inven.Quantity = item.Value;
                        inventories.Add(inven);
                    }
                }
            }
            return inventories;
        }
         
        public List<Purchase> GetHistory(DateTime date)
        {
            List<Purchase> result = null;
            List<Purchase> purchases = GetPurchases(date);
            List<Sale> sales = GetSales(date);
            if (purchases != null && sales != null)
            {
                Refresh(purchases, sales);
                result = new List<Purchase>();
                foreach (KeyValuePair<string, int> item in inventory)
                {
                    Purchase pur = _purchases.Find(x => x.Product.Code == item.Key);
                    result.Add(new Purchase(pur.Product, item.Value, pur.date));                    
                }
            }
            return result;
        }  
    }
}
