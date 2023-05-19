using SimpleERPWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcountSDK;
using System.Net;
using System.Configuration;

namespace SimpleERPWeb.Controllers
{
    public class PurchaseController : Controller
    {
        string StoreType = ConfigurationManager.AppSettings["StoreType"];
        IPurchaseSDK sdk;
        IProductSDK psdk;
        ICustomerSDK csdk;

        public PurchaseController()
        {
            ProductManager productmanager = new ProductManager(StoreType);
            psdk = productmanager.sdk;
            PurchaseManager purchasemanager = new PurchaseManager(StoreType);
            sdk = purchasemanager.sdk;
            CustomerManager customerManager = new CustomerManager(StoreType);
            csdk = customerManager.sdk;            
        }
        public ActionResult Index()
        {
            var prodlist = psdk.GetProductList();
            var custlist = csdk.GetCustomerList();
            List<string> prodlistinfo = new List<string>();
            List<string> custlisttinfo = new List<string>();
            foreach (var item in prodlist)
            {
                string prodinfo = item.Code + " " + item.Name;
                prodlistinfo.Add(prodinfo);
            }
            foreach(var item in custlist)
            {
                string custinfo = item.Code + " " + item.Name;
                custlisttinfo.Add(custinfo);
            }

            ViewBag.custlistinfo = custlisttinfo;
            ViewBag.prodlistinfo = prodlistinfo;
            ViewBag.purchaselist = sdk.GetPurchaseList();
            return View();
        }
        public ActionResult InsertExec(Purchase purchase)
        {           
            if (purchase.Product.Code == null || purchase.customer.Code == null || purchase.Quantity == null ||purchase.date==null)
            {
                //실패에 대한 응답
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            int result = sdk.Insert(purchase);
            if (result <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
        }
        public JsonResult GetPurchaseList()
        {
            var purchaselist = sdk.GetPurchaseList();            
            return Json(purchaselist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSearchedList(string pCode, string cCode)
        {
            var purchaselist = sdk.GetSearchedList(pCode, cCode);
            return Json(purchaselist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ModifyPurchase(Purchase purchase)
        {
            var result = sdk.ModifyPurchase(purchase);
            if (result == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
        }

        [HttpPost]
        public ActionResult DeletePurchase(string pid)
        {
            var result = sdk.DeletePurchase(pid);
            if (result == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
        }
    }
}