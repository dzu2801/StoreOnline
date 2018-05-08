using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreOnline.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

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

        public ActionResult Sanpham(int ?page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(db.SANPHAMs.ToList().OrderBy(n => n.MASP).ToPagedList(pageNumber, pageSize));
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
                    return RedirectToAction("Dondathang", "Admin");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();

        }

        [HttpGet]
        public ActionResult Themsanpham()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            ViewBag.LOAI = new SelectList(db.LOAIs.ToList().OrderBy(n => n.LOAI1), "LOAI", "LOAI1");
            ViewBag.MANCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(n => n.TENNCC), "MANCC", "TENNCC");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themsanpham(SANPHAM sanpham, HttpPostedFileBase fileUpload)
        {
            ViewBag.MANCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(n => n.TENNCC), "MANCC", "TENNCC");
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/sanpham/"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    sanpham.MASP = "";
                    sanpham.HINHANH = fileName;
                    db.SANPHAMs.InsertOnSubmit(sanpham);
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("Sanpham");
        }

        public ActionResult DetailsSP(string id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            SANPHAM sanpham = db.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            ViewBag.MASP = sanpham.MASP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }

        [HttpGet]
        public ActionResult DeleteSP(string id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            SANPHAM sanpham = db.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            ViewBag.MASP = sanpham.MASP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }
        [HttpPost, ActionName("DeleteSP")]
        public ActionResult Xacnhanxoa(string id)
        {

            SANPHAM sanpham = db.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            ViewBag.MASP = sanpham.MASP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SANPHAMs.DeleteOnSubmit(sanpham);
            db.SubmitChanges();
            return RedirectToAction("Sanpham");
        }
        [HttpGet]
        public ActionResult EditSP(string id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            SANPHAM sanpham = db.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MANCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(n => n.TENNCC), "MANCC", "TENNCC");
            return View(sanpham);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditSP(SANPHAM sanpham, HttpPostedFileBase fileUpload)
        {
            ViewBag.MANCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(n => n.TENNCC), "MANCC", "TENNCC");
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/sanpham"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    sanpham.HINHANH = fileName;
                    UpdateModel(sanpham);
                    db.SubmitChanges();
                }
                return RedirectToAction("Sanpham");
            }
        }
        public ActionResult Nhacungcap(int ?page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(db.NHACUNGCAPs.ToList().OrderBy(n => n.MANCC).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult CreateNCC()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateNCC(NHACUNGCAP ncc, HttpPostedFileBase fileUpload)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            ncc.MANCC = "";
            db.NHACUNGCAPs.InsertOnSubmit(ncc);
            db.SubmitChanges();
            return RedirectToAction("Nhacungcap");
        }

        public ActionResult DetailsNCC(string id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            NHACUNGCAP ncc = db.NHACUNGCAPs.SingleOrDefault(n => n.MANCC == id);
            ViewBag.MANCC = ncc.MANCC;
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);
        }
        [HttpGet]
        public ActionResult DeleteNCC(string id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            NHACUNGCAP ncc = db.NHACUNGCAPs.SingleOrDefault(n => n.MANCC == id);
            ViewBag.MANCC = ncc.MANCC;
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);
        }
        [HttpPost, ActionName("DeleteNCC")]
        public ActionResult XacnhanxoaNCC(string id)
        {
            NHACUNGCAP ncc = db.NHACUNGCAPs.SingleOrDefault(n => n.MANCC == id);
            ViewBag.MANCC = ncc.MANCC;
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NHACUNGCAPs.DeleteOnSubmit(ncc);
            db.SubmitChanges();
            return RedirectToAction("Nhacungcap");
        }

        [HttpGet]
        public ActionResult EditNCC(string id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            Session["mancc"] = id;
            NHACUNGCAP ncc = db.NHACUNGCAPs.SingleOrDefault(n => n.MANCC == id);
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);
        }
        [HttpPost]
        public ActionResult EditNCC(FormCollection collection)
        {
            var id = Session["mancc"];
            var ten = collection["Ten"];
            var diachi = collection["Diachi"];
            var dt = collection["Dienthoai"];
            var email = collection["Email"];
            var mast = collection["mast"];
            var ghichu = collection["ghichu"];

            NHACUNGCAP ncc = db.NHACUNGCAPs.SingleOrDefault(n => n.MANCC == id.ToString());
            ncc.TENNCC = ten;
            ncc.DIACHINCC = diachi;
            ncc.DTNCC = dt;
            ncc.GHICHU = ghichu;
            ncc.MASOTHUE = mast;
            ncc.EMAILNCC = email;
            db.SubmitChanges();
          
            return RedirectToAction("Nhacungcap");
        }
        [HttpGet]
        public ActionResult Dondathang()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            List<DONDATHANG> ddh = db.DONDATHANGs.OrderByDescending(a => a.NGAYDAT).ToList();
            return View(ddh);
        }
        public ActionResult Chitietdondathang(string mahd, string makh)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            List<CTDONDATHANG> sp = db.CTDONDATHANGs.Where(a=>a.MAHD==mahd).ToList();
            List<CTDONDATHANG> kh = db.CTDONDATHANGs.Where(a => a.DONDATHANG.MAKH == makh).ToList();

            return View(sp.ToList());
        }

        public ActionResult Khachhang(int ?page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(db.KHACHHANGs.ToList().OrderBy(n => n.MAKH).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult DetailsKH(string id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MAKH == id);
            ViewBag.MAKH = kh.MAKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }
        [HttpGet]
        public ActionResult DeleteKH(string id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MAKH == id);
            ViewBag.MAKH = kh.MAKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }
        [HttpPost, ActionName("DeleteKH")]
        public ActionResult XacnhanxoaKH(string id)
        {
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MAKH == id);
            ViewBag.MAKH = kh.MAKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.KHACHHANGs.DeleteOnSubmit(kh);
            db.SubmitChanges();
            return RedirectToAction("Khachhang");
        }
    }
}