using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreOnline.Models;

namespace StoreOnline.Controllers
{
    public class UserController : Controller
    {
        dbStoreOnlineDataContext db = new dbStoreOnlineDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection collection, KHACHHANG kh)
        {
            var taikhoan = collection["Username"];
            var email = collection["Email"];
            var matkhau = collection["Password"];
            var matkhaunl = collection["Confirm Password"];
            if (Convert.ToString(taikhoan) == "Username")
            {
                ViewData["loi1"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(email) == "Email")
            {
                ViewData["loi2"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(matkhau) == "Password")
            {
                ViewData["loi3"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(matkhaunl) == "Confirm Password")
            {
                ViewData["loi4"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(matkhau) != Convert.ToString(matkhaunl))
            {
                ViewData["loi4"] = "Mật khẩu nhập lại phải trùng trước đó";
            }
            else
            {
                kh.TK = taikhoan;
                kh.MK = matkhau;
                kh.EMAILKH = email;
                kh.TENKH = null;
                kh.GIOITINH = null;
                kh.DIACHIKH = null;
                kh.DTKH = null;
                kh.MAKH = "";
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Login");
            }

            return this.Register();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var taikhoan = collection["Username"];
            var matkhau = collection["Password"];
            if (String.IsNullOrEmpty(taikhoan))
            {
                ViewData["loi1"] = "Tài khoản không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["loi2"] = "Mật khẩu không được bỏ trống";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TK == taikhoan && n.MK == matkhau);
                if (kh != null)
                {
                    Session["TK"] = taikhoan;
                    return RedirectToAction("Index", "Shoponline");

                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }   
            return View();
        }

        public ActionResult Wishlist()
        {
            if (Session["tentk"] == null || Session["Taikhoan"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(FormCollection collection)
        {
            var taikhoan = collection["Username"];
            var mail = collection["Email"];
            if (String.IsNullOrEmpty(taikhoan))
            {
                ViewData["loi1"] = "Tên tài khoản không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(mail))
            {
                ViewData["loi2"] = "Email không được bỏ trống";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TK == taikhoan && n.EMAILKH == mail);
                if (kh != null)
                {
                    Session["Taikhoan"] = taikhoan;
                    return RedirectToAction("Resetpassword", "User");
                }
                else
                {
                    ViewBag.Thongbao = "Tài khoản hoặc Email không đúng";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Resetpassword()
        {
            return View();
        }
    }
}