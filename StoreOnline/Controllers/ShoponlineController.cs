using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreOnline.Models;
using PagedList;
using PagedList.Mvc;

namespace StoreOnline.Controllers
{
    public class ShoponlineController : Controller
    {
        dbStoreOnlineDataContext db = new dbStoreOnlineDataContext();

        // GET: Shoponline
        //Tao doi tuong daya chưa dữ liệu từ model dbBansach đã tạo. 
        private List<SANPHAM> Laysanpham(string ten)
        {
            //Sắp xếp sách theo tên
            if(ten==null || ten=="Search")
                return db.SANPHAMs.OrderByDescending(a => a.TENSP).ToList();
            else
                return db.SANPHAMs.OrderByDescending(a => a.TENSP).Where(a=>a.TENSP.Contains(ten)).ToList();
        }
        public ActionResult Index(FormCollection collection, int ? page)
        {
            string ten = collection["Search"];
            int pageSize = 12;
            int pageNum = (page ?? 1);
            //Lấy sản phẩm
            var sanpham = Laysanpham(ten);
            return View(sanpham.ToPagedList(pageNum, pageSize));
        }        
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Details(string masp)
        {
            var sp = from s in db.SANPHAMs
                     where s.MASP == masp
                     select s;
            return View(sp.Single());
        }
    }
}