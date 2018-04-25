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
        public ActionResult Index()
        {
            //var SpHot = SanPhamHot(5);
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}