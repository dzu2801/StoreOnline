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
            var tenkh = collection["Name"];
            var email = collection["Email"];
            var diachi = collection["Add"];
            var dienthoai = collection["Phone"];
            var gioitinh = collection["Sex"];
            var matkhau = collection["Password"];
            var matkhaunl = collection["Confirm Password"];
            if (Convert.ToString(taikhoan) == "Username")
            {
                ViewData["loi1"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(tenkh) == "Name")
            {
                ViewData["loi2"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(email) == "Email")
            {
                ViewData["loi3"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(diachi) == "Add")
            {
                ViewData["loi4"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(dienthoai) == "Phone")
            {
                ViewData["loi5"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(gioitinh) == "Sex")
            {
                ViewData["loi6"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(matkhau) == "Password")
            {
                ViewData["loi7"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(matkhaunl) == "Confirm Password")
            {
                ViewData["loi8"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(matkhau) != Convert.ToString(matkhaunl))
            {
                ViewData["loi9"] = "Mật khẩu nhập lại phải trùng trước đó";
            }
            else
            {
                kh.TK = taikhoan;
                kh.MK = matkhau;
                kh.EMAILKH = email;
                kh.TENKH = tenkh;
                kh.GIOITINH = gioitinh;
                kh.DIACHIKH = diachi;
                kh.DTKH = dienthoai;
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
            if (Session["TK"] == null)
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
            var matkhau = collection["Password"];
            var matkhaunl = collection["Confirm Password"];
            if (String.IsNullOrEmpty(taikhoan))
            {
                ViewData["loi1"] = "Tên tài khoản không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(mail))
            {
                ViewData["loi2"] = "Email không được bỏ trống";
            }
            if (Convert.ToString(matkhau) == "Password")
            {
                ViewData["loi3"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(matkhaunl) == "Confirm Password")
            {
                ViewData["loi4"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(matkhau) != Convert.ToString(matkhaunl))
            {
                ViewData["loi5"] = "Mật khẩu nhập lại phải trùng trước đó";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.Where(n => n.TK == taikhoan && n.EMAILKH == mail).Single();
                if (kh != null)
                {
                    kh.MK = matkhau;
                    db.SubmitChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.Thongbao = "Tài khoản hoặc Email không đúng";
                }
            }
            return View();
        }

        //[HttpGet]
        //public ActionResult Resetpassword()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Resetpassword(FormCollection collection, KHACHHANG kh)
        //{
        //    var matkhau = collection["Password"];
        //    var matkhaunl = collection["Confirm Password"];
        //    if (Convert.ToString(matkhau) == "Password")
        //    {
        //        ViewData["loi1"] = "Không được bỏ trống";
        //    }
        //    else if (Convert.ToString(matkhaunl) == "Confirm Password")
        //    {
        //        ViewData["loi2"] = "Không được bỏ trống";
        //    }
        //    else if (Convert.ToString(matkhau) != Convert.ToString(matkhaunl))
        //    {
        //        ViewData["loi3"] = "Mật khẩu nhập lại phải trùng trước đó";
        //    }
        //    else
        //    {
        //        kh = (KHACHHANG)Session["tkrs"];    
        //        kh.MK = matkhau;
        //        db.KHACHHANGs.InsertOnSubmit(kh);
        //        db.SubmitChanges();
        //        return RedirectToAction("Login");
        //    }
        //    return View();
        //}

        public ActionResult Logout()
        {
            Session["TK"] = null;
            return RedirectToAction("Index","Shoponline");
            
        }

        public ActionResult Info()
        {
            if (Session["TK"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }
    }
}