using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltweb_.Models
{
    public class ChuyenDiViewModel
    {
        public int IdChuyenDi { get; set; }
        public string DiemXuatPhat { get; set; }
        public string DiemDen { get; set; }
        public decimal? Gia { get; set; }
        public DateTime? ThoiGianDi { get; set; }
        public DateTime? ThoiGianDen { get; set; }
        public int? IdNhanVien { get; set; }
        public int? IdXeKhach { get; set; }
        public NhanVien NhanVien { get; set; }
        public XeKhach XeKhach { get; set; }
        public int? IdLichTrinh { get; set; }
        public HangXe HangXe { get; set; }
        public string DiemDiCuThe { get; set; }
        public string DiemDenCuThe { get; set; }
        public byte GioDuKien { get; set; }

    }
}