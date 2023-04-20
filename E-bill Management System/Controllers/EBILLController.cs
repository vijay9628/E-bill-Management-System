using E_bill_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_bill_Management_System.Repository;
namespace E_bill_Management_System.Controllers
{
    public class EBILLController : Controller
    {
        // GET: EBILL
        public ActionResult Index()
        {
            Data data = new Data();
            var list = data.GetAllDetails();

            return View(list);
        }

        public ActionResult create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult create(BillDetails bd)
        {
            Data data = new Data();
            data.saveBillDetails(bd);
            ModelState.Clear();
            return View();
        }

        public ActionResult CreateItem(Items items)
        {
            return PartialView("_CreateItem",items);
        }

        public ActionResult viewBill(int Id)
        {
            Data data = new Data();
            var detail = data.GetDetails(Id);
            return View(detail);
        }


        public ActionResult changecolumn()
        {
            return View();
        }
    }
}