using ltweb_.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltweb_.Controllers
{
    public class SearchController : Controller
    {
        DBQuanLyXeEntities dbContext = new DBQuanLyXeEntities();
        public ActionResult Search(string diemXuatPhat, string diemDen, string ngayDi)
        {
            if (!string.IsNullOrEmpty(ngayDi))
            {
                DateTime ngayDiParsed;
                if (DateTime.TryParseExact(ngayDi, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngayDiParsed))
                {
                    var chuyenDi = dbContext.ChuyenDi
                        .Where(cd => cd.DiemXuatPhat == diemXuatPhat && cd.DiemDen == diemDen)
                        .Where(cd => cd.ThoiGianDi.HasValue &&
                            cd.ThoiGianDi.Value.Year == ngayDiParsed.Year &&
                            cd.ThoiGianDi.Value.Month == ngayDiParsed.Month &&
                            cd.ThoiGianDi.Value.Day == ngayDiParsed.Day)
                            .Select(cd => new ChuyenDiViewModel
                            {
                                IdChuyenDi = cd.IdChuyenDi,
                                DiemXuatPhat = cd.DiemXuatPhat,
                                DiemDiCuThe = cd.DiemDiCuThe,
                                DiemDen = cd.DiemDen,
                                DiemDenCuThe = cd.DiemDenCuThe,
                                Gia = cd.Gia,
                                GioDuKien = cd.ThoiGianDuKien.HasValue ? (byte)cd.ThoiGianDuKien.Value : (byte)0,
                                ThoiGianDi = cd.ThoiGianDi,
                                ThoiGianDen = cd.ThoiGianDen,
                                IdNhanVien = cd.IdNhanVien,
                                IdXeKhach = cd.IdXeKhach,
                                NhanVien = cd.NhanVien,
                                XeKhach = cd.XeKhach,
                                IdLichTrinh = cd.IdLichTrinh,
                                HangXe = dbContext.HangXe.FirstOrDefault(hx => hx.IdHangXe == cd.XeKhach.HangXe.IdHangXe)
                            })
                            .ToList();

                    if (chuyenDi.Any())
                    {
                        // Nếu có dữ liệu phù hợp, trả về view tương ứng
                        return View("Search", chuyenDi);
                    }
                }
            }

            // Xử lý trường hợp không có dữ liệu phù hợp
            return View("NoData");
        }

    }

}