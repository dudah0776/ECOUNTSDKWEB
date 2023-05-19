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
    public class ProductController : Controller
    {
        string StoreType = ConfigurationManager.AppSettings["StoreType"];
        IProductSDK sdk;
        ICommonSDK commonsdk;
        public ProductController()
        {
            ProductManager manager = new ProductManager(StoreType);            
            commonsdk = manager.csdk;
        }

        public ActionResult Index()
        {
            Type enumType = typeof(EcountSDK.ProductType);
            var enumValues = Enum.GetValues(enumType);
            ViewBag.prodType = enumValues;
            ViewBag.prodlist = commonsdk.GetCommonList<Product>(SQL.GetProductSP);
            return View();
        }
        public ActionResult InsertExec(string Code, string Name, string Type)
        {
            var prod = new Product(Code, Name, Type);
            if (prod == null || prod.Code == null || prod.Name == null || prod.Type == null)
            {
                //실패에 대한 응답
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            int result = commonsdk.InsertCommon<Product>(prod, SQL.InsertProductSP);
            if (result <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
        }
        public JsonResult GetProductList()
        {
            var prodlist = commonsdk.GetCommonList<Product>(SQL.GetProductSP);
            return Json(prodlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSearchedList(string code, string type)
        {            
            var prodlist = sdk.GetSearchedList(code, type);
            return Json(prodlist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ModifyProduct(string Code, string Name, string Type)
        {
            Product product = new Product(Code, Name, Type);
            var result = commonsdk.ModifyCommon<Product>(product, SQL.ModifyProductSP);
            if (result == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
        }
        [HttpPost]
        public ActionResult DeleteProduct(string Code)
        {
            var result = commonsdk.CommonDelete(Code, SQL.DeleteProductSP);
            if (result == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
        }
    }
}