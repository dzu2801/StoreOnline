using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreOnline.Models;

namespace StoreOnline.Controllers
{
    public class ShoponlineController : Controller
    {
        dbStoreOnlineDataContext db = new dbStoreOnlineDataContext();

        //private List<SANPHAM> SanPhamHot(int count)
        //{
        //    return db.SANPHAMs.OrderByDescending(a => a.DONDATHANGs).Take(count).ToList();
        //}

        // GET: Shoponline
        //Tao doi tuong daya chưa dữ liệu từ model dbBansach đã tạo. 
        private List<SANPHAM> Laysanpham(int count)
        {
            //Sắp xếp sách theo ngày cập nhật, sau đó lấy top @count 
            return db.SANPHAMs.OrderByDescending(a => a.TENSP).Take(count).ToList();
        }
        public ActionResult Index()
        {
            //Lấy top 5 Album bán chạy nhất
            var sanpham = Laysanpham(30);
            return View(sanpham);
        }        
        public ActionResult Contact()
        {
            return View();
        }
    }
}