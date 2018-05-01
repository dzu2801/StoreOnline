using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreOnline.Models;

namespace StoreOnline.Controllers
{
    public class AdminController : Controller
    {
        dbStoreOnlineDataContext db = new dbStoreOnlineDataContext();

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var userad = collection["Userad"];
            var passad = collection["Passad"];
            if (String.IsNullOrEmpty(userad))
            {
                ViewData["loi1"] = "Tài khoản không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(passad))
            {
                ViewData["loi2"] = "Mật khẩu không được bỏ trống";
            }
            else
            {
                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.USERADMIN == userad && n.PASSADMIN == passad);
                if (ad != null)
                {
                    Session["Admin"] = userad;
                    ViewBag.Thongbao = "Đăng nhập Admin thành công";
                    //return RedirectToAction("Index", "Admin");

                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
    }
}