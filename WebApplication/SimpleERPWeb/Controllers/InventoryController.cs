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
    public class InventoryController : Controller
    {
        string storeType = ConfigurationManager.AppSettings["StoreType"];
        IinventorySDK sdk;
        public InventoryController()
        {
            InventoryManager manager = new InventoryManager(storeType);
            sdk = manager.sdk;
        }
        public ActionResult Index()
        {
            DateTime currentTime = DateTime.Now;
            ViewBag.inventorylist = sdk.GetStatus(currentTime);
            return View();
        }
        [HttpPost]
        public JsonResult GetRead(DateTime date, string code)
        {
            List<Inventory> invenlist = null;
            if (code == null)
            {
                invenlist = sdk.GetStatus(date);
            }
            else
            {
                invenlist = sdk.GetStatus(code, date);
            }            
            return Json(invenlist, JsonRequestBehavior.AllowGet);
        }
    }
}