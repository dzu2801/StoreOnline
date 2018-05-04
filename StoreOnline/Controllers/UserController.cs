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
            var gioitinh = collection["gender"];
            var matkhau = collection["Password"];
            var matkhaunl = collection["Confirm Password"];
            if (Convert.ToString(taikhoan) == "Username")
            {
                ViewData["loi1"] = "Không được bỏ trống";
            }
            else
            {
                KHACHHANG k = db.KHACHHANGs.SingleOrDefault(n => n.TK == taikhoan);
                if (k != null)
                {
                    ViewData["loi1"] = "Tài Khoản đã tồn tại";
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


                //chổ cần sửa giới tính
                else if (Convert.ToString(gioitinh) == null)
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
                    ViewData["loi8"] = "Mật khẩu nhập lại phải trùng trước đó";
                }
                else
                {
                    try
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
                    catch
                    {
                        ViewData["loi8"] = "Đã xảy ra lỗi!!! Cập nhập tài khoản không thành công!!!";
                        return this.Register();
                    }
                }
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
                    Session["Taikhoan"] = kh;
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
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TK == taikhoan && n.EMAILKH == mail);
                if (kh != null)
                {
                    kh.MK = matkhau;
                    db.SubmitChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.Thongbao = "Tài khoản hoặc Email không đúng";
                    return View();
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["TK"] = null;
            return RedirectToAction("Index","Shoponline");
            
        }
        [HttpGet]
        public ActionResult Info()
        {
            if (Session["TK"] == null || Session["TK"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            var kh = from s in db.KHACHHANGs
                     where s.TK == Session["TK"].ToString()
                     select s;
            return View(kh.Single());
        }
        [HttpPost]
        public ActionResult Info(FormCollection collection)
        {           
            var tenkh = collection["Tenkh"];
            var email = collection["Email"];
            var diachi = collection["Diachi"];
            var dienthoai = collection["Dienthoai"];
            var matk = collection["Matk"];
            var gioitinh = collection["gender"];
            var chondoimk = collection["doimatkhau"];
            var matkhaucu = collection["Old Password"];
            var matkhau = collection["Password"];
            var matkhaunl = collection["Confirm Password"];

            if (Convert.ToString(tenkh) == "Họ Tên")
            {
                ViewData["loi2"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(email) == "Email")
            {
                ViewData["loi3"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(diachi) == "Địa Chỉ")
            {
                ViewData["loi4"] = "Không được bỏ trống";
            }
            else if (Convert.ToString(dienthoai) == "Số Điện Thoại")
            {
                ViewData["loi5"] = "Không được bỏ trống";
            }
            else
            {
                try
                {
                    KHACHHANG kh = db.KHACHHANGs.Where(s => s.MAKH == matk.ToString()).Single();
                    kh.EMAILKH = email;
                    kh.TENKH = tenkh;
                    kh.DIACHIKH = diachi;
                    kh.DTKH = dienthoai;
                    kh.GIOITINH = gioitinh;
                    if (Convert.ToString(chondoimk) == "checked")
                    {
                        if (Convert.ToString(matkhaucu) == "Nhập Lại Mật Khẩu Mới")
                        {
                            ViewData["loi7"] = "Không được bỏ trống";
                        }
                        else if (Convert.ToString(matkhau) == "Nhập Lại Mật Khẩu Mới")
                        {
                            ViewData["loi8"] = "Không được bỏ trống";
                        }
                        else if (Convert.ToString(matkhaunl) == "Nhập Lại Mật Khẩu Mới")
                        {
                            ViewData["loi9"] = "Không được bỏ trống";
                        }
                        else if(Convert.ToString(matkhau)!=Convert.ToString(matkhaunl))
                        {
                            ViewData["loi9"] = "Mật khẩu nhập lại phải trùng trước đó";
                        }
                        else if (matkhaucu.ToString() == kh.MK.ToString())
                        {
                            kh.MK = matkhau;
                            db.SubmitChanges();
                            ViewData["loi2"] = "Cập nhập thành công";
                            return this.Info();
                        }
                        else
                        {
                            ViewData["loi7"] = "Mật Khẩu cũ không chính xác";
                        }
                        ViewData["loi2"] = "Cập nhập không thành công";
                        return this.Info();                        
                    }
                    db.SubmitChanges();
                    ViewData["loi2"] = "Cập nhập thành công";
                    return this.Info();
                }
                catch
                {
                    ViewData["loi2"] = "Cập nhập không thành công";
                    return this.Info();
                }
            }
            ViewData["loi2"] = "Cập nhập không thành công";
            return this.Info();
        }
        public ActionResult Lichsudathang()
        {
            if (Session["Taikhoan"] == null)
            {
                return this.Login();
            }
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            var ddh=db.DONDATHANGs.OrderByDescending(a => a.NGAYDAT).Where(m=>m.MAKH==kh.MAKH.ToString()).ToList();
            return View(ddh);
        }
        public ActionResult Chitietdondathang(string mahd)
        {
            if (Session["TK"] == null || Session["TK"].ToString() == null)
            {
                return this.Login();
            }
            var ddh = db.CTDONDATHANGs.Where(m => m.MAHD == mahd).ToList();
            return View(ddh);
        }
    }
}