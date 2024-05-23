using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltweb_.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult TrangChu()
        {
            return View();
        }
        public ActionResult TrangTimKiem()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult Terms()
        {
            return View();
        }
        public ActionResult Community()
        {
            return View();
        }
        public ActionResult HuongDanThanhToan()
        {
            return View();
        }
        public ActionResult CommonQuestion()
        {
            return View();
        }
        public ActionResult ChinhSachBaoMat()
        {
            return View();
        }
        public ActionResult HuongDanDatVe()
        {
            return View();
        }
    }
}