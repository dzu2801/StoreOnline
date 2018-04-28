using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreOnline.Models
{
    public class Giohang
    {
        //Tao doi tuong data chua dữ liệu từ model dbStoreOnline đã tạo. 
         dbStoreOnlineDataContext data = new dbStoreOnlineDataContext();
        public string iMasp { set; get; }
        public string sTensp { set; get; }
        public string sAnhsp { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }

        }
        //Khoi tao gio hàng theo Masp duoc truyen vao voi Soluong mac dinh la 1
        public Giohang(string Masach)
        {
            iMasp = Masach;
            SANPHAM sp = data.SANPHAMs.Single(n => n.MASP == iMasp);
            sTensp = sp.TENSP;
            sAnhsp = sp.HINHANH;
            dDongia = double.Parse(sp.GIABAN.ToString());
            iSoluong = 1;
        }
    }
}