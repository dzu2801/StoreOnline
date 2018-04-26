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
        private List<SANPHAM> Laysanpham(int count)
        {
            //Sắp xếp sách theo ngày cập nhật, sau đó lấy top @count 
            return db.SANPHAMs.OrderByDescending(a => a.TENSP).Take(count).ToList();
        }
        public ActionResult Index(int ? page)
        {
            int pageSize = 12;
            int pageNum = (page ?? 1);
            //Lấy top 5 Album bán chạy nhất
            var sanpham = Laysanpham(30);
            return View(sanpham.ToPagedList(pageNum, pageSize));
        }        
        public ActionResult Contact()
        {
            return View();
        }
    }
}