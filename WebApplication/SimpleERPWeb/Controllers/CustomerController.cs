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
    public class CustomerController : Controller
    {
        string storeType = ConfigurationManager.AppSettings["StoreType"];
        ICustomerSDK sdk;
        ICommonSDK commonsdk;
        public CustomerController()
        {
            CustomerManager manager = new CustomerManager(storeType);
            commonsdk = manager.csdk;
        }
        public ActionResult Index()
        {
            ViewBag.custlist = commonsdk.GetCommonList<Customer>(SQL.GetCustomerSP);
            return View();
        }
        public ActionResult InsertExec(Customer customer)
        {            
            if (customer == null || customer.Code == null || customer.Name == null)
            {
                //실패에 대한 응답
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            int result = commonsdk.InsertCommon<Customer>(customer,SQL.InsertCustomerSP);
            if (result <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
        }
        public JsonResult GetCustomerList()
        {
            var custlist = commonsdk.GetCommonList<Customer>(SQL.GetCustomerSP);            
            return Json(custlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSearchedList(string code)
        {
            var custlist = sdk.GetSearchedList(code);
            return Json(custlist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ModifyCustomer(Customer customer)
        {
            var result = commonsdk.ModifyCommon<Customer>(customer, SQL.ModifyCustomerSP);
            if (result == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
        }
        [HttpPost]
        public ActionResult DeleteCustomer(string Code)
        {
            var result = commonsdk.CommonDelete(Code, SQL.DeleteCustomerSP);
            if (result == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
        }
    }
}