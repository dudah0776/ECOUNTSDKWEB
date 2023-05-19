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
    public class SaleController : Controller
    {
        string storeType = ConfigurationManager.AppSettings["StoreType"];
        ISaleSDK sdk;
        IProductSDK psdk;
        ICustomerSDK csdk;
        public SaleController()
        {
            SaleManager salemanager = new SaleManager(storeType);
            sdk = salemanager.sdk;
            ProductManager productmanager = new ProductManager(storeType);
            psdk = productmanager.sdk;
            CustomerManager customerManager = new CustomerManager(storeType);
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
            ViewBag.salelist = sdk.GetSaleList();
            return View();
        }
        public ActionResult InsertExec(Sale sale)
        {            
            if (sale.Product.Code == null || sale.customer.Code == null || sale.Quantity == null ||sale.date==null)
            {
                //실패에 대한 응답
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            int result = sdk.Insert(sale);
            if (result <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
        }
        public JsonResult GetSaleList()
        {
            var salelist = sdk.GetSaleList();            
            return Json(salelist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSearchedList(string pCode, string cCode)
        {
            var salelist = sdk.GetSearchedList(pCode, cCode);
            return Json(salelist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ModifySale(Sale sale)
        {
            var result = sdk.ModifySale(sale);
            if (result == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
        }

        [HttpPost]
        public ActionResult DeleteSale(string sid)
        {
            var result = sdk.DeleteSale(sid);
            if (result == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
        }
    }
}