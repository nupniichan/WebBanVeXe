using ltweb_.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltweb_.Controllers
{
    public class DashboardController : Controller
    {
        DBQuanLyXeEntities dbContext = new DBQuanLyXeEntities();
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QuanLyVe()
        {
            // Lấy danh sách tất cả các vé hoặc thực hiện tìm kiếm vé tại đây
            var danhSachVe = dbContext.VeXe.ToList(); // Thay thế bằng truy vấn tìm kiếm thực tế

            return View(danhSachVe);
        }
        public ActionResult QuanLyChuyenDi()
        {
            var chuyenDiList = dbContext.ChuyenDi
                .Include("NhanVien")
                .Include("XeKhach")
                .ToList();

            return View(chuyenDiList);
        }

        public ActionResult QuanLyNhanVien()
        {
            var nhanViens = dbContext.NhanVien.Include(n => n.HangXe);
            return View(nhanViens.ToList());
        }

        public ActionResult ThongKe()
        {
            return View();
        }

        public ActionResult QuanLyXe()
        {
            var xeKhachList = dbContext.XeKhach.ToList();
            return View(xeKhachList);
        }

        public ActionResult GioiThieu()
        {
            return View();
        }
        // =============================================== Xe Khách ===============================================
        public ActionResult CreateXeKhach()
        {
            // Lấy idhangxe từ Session hoặc từ nơi bạn đã lưu nó khi người dùng đăng nhập
            int? idHangXe = Session["IdHangXe"] as int?;

            if (idHangXe.HasValue)
            {
                // Tạo một đối tượng XeKhach với idhangxe đã lấy được
                XeKhach model = new XeKhach
                {
                    IdHangXe = idHangXe.Value
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("Error"); // Hoặc xử lý lỗi nếu không có idhangxe hợp lệ
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateXeKhach(XeKhach model)
        {
            if (ModelState.IsValid)
            {
                if (IsIdHangXeValid(model.IdHangXe.GetValueOrDefault()))
                {
                    if (IsBienSoXeExists(model.BienSoXe))
                    {
                        ModelState.AddModelError("BienSoXe", "Biển số xe đã tồn tại.");
                    }
                    else
                    {
                        dbContext.XeKhach.Add(model);
                        dbContext.SaveChanges();

                        return RedirectToAction("QuanLyXe");
                    }
                }
                else
                {
                    ModelState.AddModelError("idhangxe", "Invalid IdHangXe value.");
                }
            }
            return View(model);
        }
        private bool IsBienSoXeExists(string bienSoXe)
        {
            // Kiểm tra xem biển số xe đã tồn tại trong cơ sở dữ liệu chưa
            return dbContext.XeKhach.Any(x => x.BienSoXe == bienSoXe);
        }
        private bool IsIdHangXeValid(int idHangXe)
        {
            // Thực hiện kiểm tra tính hợp lệ của idHangXe ở đây (ví dụ: kiểm tra trong cơ sở dữ liệu)
            // Trả về true nếu hợp lệ, ngược lại trả về false
            // Đây chỉ là ví dụ, bạn cần thay đổi để phù hợp với ứng dụng của bạn
            return dbContext.HangXe.Any(h => h.IdHangXe == idHangXe);
        }

        public ActionResult EditXeKhach(int id)
        {
            var xeKhach = dbContext.XeKhach.Find(id);
            if (xeKhach == null)
            {
                return HttpNotFound();
            }

            return View(xeKhach);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditXeKhach(XeKhach model)
        {
            if (ModelState.IsValid)
            {
                var existingXeKhach = dbContext.XeKhach.Find(model.IdXeKhach);
                if (existingXeKhach != null)
                {
                    // Cập nhật thông tin xe từ model
                    existingXeKhach.BienSoXe = model.BienSoXe;
                    existingXeKhach.LoaiXe = model.LoaiXe;
                    existingXeKhach.SoGhe = model.SoGhe;
                    existingXeKhach.TenLoaiXe = model.TenLoaiXe;

                    dbContext.SaveChanges();

                    return RedirectToAction("QuanLyXe");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Không tìm thấy xe.");
                }
            }

            return View(model);
        }

        public ActionResult RemoveXeKhach(int id)
        {
            var xeKhach = dbContext.XeKhach.Find(id);
            if (xeKhach == null)
            {
                return HttpNotFound();
            }

            return View(xeKhach);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveXeKhachConfirmed(int id)
        {
            var xeKhach = dbContext.XeKhach.Find(id);
            if (xeKhach == null)
            {
                return HttpNotFound();
            }

            dbContext.XeKhach.Remove(xeKhach);
            dbContext.SaveChanges();

            return RedirectToAction("QuanLyXe");
        }
        // =============================================== Nhân Viên ===============================================
        public ActionResult CreateNhanVien()
        {
            ViewBag.IdHangXe = new SelectList(dbContext.HangXe, "IdHangXe", "TenHangXe");
            return View();
        }

        // Xử lý tạo mới nhân viên
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNhanVien(NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                dbContext.NhanVien.Add(nhanVien);
                dbContext.SaveChanges();
                return RedirectToAction("QuanLyNhanVien");
            }

            ViewBag.IdHangXe = new SelectList(dbContext.HangXe, "IdHangXe", "TenHangXe", nhanVien.IdHangXe);
            return View(nhanVien);
        }
        public ActionResult EditNhanVien(int id)
        {
            var nhanVien = dbContext.NhanVien.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdHangXe = new SelectList(dbContext.HangXe, "IdHangXe", "TenHangXe", nhanVien.IdHangXe);
            return View(nhanVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditNhanVien(NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                dbContext.Entry(nhanVien).State = EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("QuanLyNhanVien");
            }

            ViewBag.IdHangXe = new SelectList(dbContext.HangXe, "IdHangXe", "TenHangXe", nhanVien.IdHangXe);
            return View(nhanVien);
        }

        public ActionResult RemoveNhanVien(int id)
        {
            var nhanVien = dbContext.NhanVien.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }

            return View(nhanVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveNhanVienConfirmed(int id)
        {
            var nhanVien = dbContext.NhanVien.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }

            dbContext.NhanVien.Remove(nhanVien);
            dbContext.SaveChanges();

            return RedirectToAction("QuanLyNhanVien");
        }
        // =============================================== ChuyenDi ===============================================
        public ActionResult CreateChuyenDi()
        {
            // Lấy danh sách nhân viên và xe khách từ cơ sở dữ liệu
            var nhanVienList = dbContext.NhanVien.ToList();
            var xeKhachList = dbContext.XeKhach.ToList();

            // Chuyển danh sách nhân viên và xe khách thành SelectList
            ViewBag.NhanVienList = new SelectList(nhanVienList, "IdNhanVien", "HoTen");
            ViewBag.XeKhachList = new SelectList(xeKhachList, "IdXeKhach", "BienSoXe");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateChuyenDi(ChuyenDi model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Kiểm tra xem các trường bắt buộc có được điền đầy đủ không
                    if (string.IsNullOrEmpty(model.DiemXuatPhat) || string.IsNullOrEmpty(model.DiemDen) ||
                        model.ThoiGianDi == null || model.ThoiGianDen == null || model.Gia == null ||
                        model.IdNhanVien == null || model.IdXeKhach == null)
                    {
                        ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin.");
                        // Điều hướng quay lại view tạo
                        return View(model);
                    }

                    // Tạo một đối tượng LichTrinh mới
                    // Tạo một đối tượng LichTrinh mới
                    LichTrinh lichTrinh = new LichTrinh
                    {
                        NgayDi = model.ThoiGianDi.Value.Date, // Đặt NgayDi của LichTrinh bằng NgayDi của ChuyenDi, chỉ lấy phần ngày
/*                        GioDi = new DateTime(
                            model.ThoiGianDi.Value.Year,
                            model.ThoiGianDi.Value.Month,
                            model.ThoiGianDi.Value.Day,
                            model.ThoiGianDi.Value.Hour,
                            model.ThoiGianDi.Value.Minute,
                            0), // Đặt GioDi của LichTrinh bằng NgayDi của ChuyenDi và giờ phút, đặt giây là 0*/
                        IdVeXe = null, // Bạn có thể đặt IdVeXe bằng giá trị thích hợp
                    };

                    // Lưu LichTrinh vào cơ sở dữ liệu
                    dbContext.LichTrinh.Add(lichTrinh);
                    dbContext.SaveChanges();

                    // Gán IdLichTrinh của ChuyenDi bằng IdLichTrinh vừa tạo
                    model.IdLichTrinh = lichTrinh.IdLichTrinh;

                    // Lưu ChuyenDi vào cơ sở dữ liệu
                    dbContext.ChuyenDi.Add(model);
                    dbContext.SaveChanges();

                    return RedirectToAction("QuanLyChuyenDi");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi: " + ex.Message);
                return View(model);
            }

            return View(model);
        }


        public ActionResult EditChuyenDi(int id)
        {
            var chuyenDi = dbContext.ChuyenDi.Find(id);
            if (chuyenDi == null)
            {
                return HttpNotFound();
            }

            // Tạo SelectList cho danh sách nhân viên và xe khách
            ViewBag.NhanVienList = new SelectList(dbContext.NhanVien, "IdNhanVien", "HoTen");
            ViewBag.XeKhachList = new SelectList(dbContext.XeKhach, "IdXeKhach", "BienSoXe");

            return View(chuyenDi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditChuyenDi(ChuyenDi model)
        {
            if (ModelState.IsValid)
            {
                dbContext.Entry(model).State = EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("QuanLyChuyenDi");
            }

            ViewBag.NhanVienList = new SelectList(dbContext.NhanVien, "IdNhanVien", "HoTen");
            ViewBag.XeKhachList = new SelectList(dbContext.XeKhach, "IdXeKhach", "BienSoXe");

            return View(model);
        }
        public ActionResult DeleteChuyenDi(int id)
        {
            var chuyenDi = dbContext.ChuyenDi.Find(id);
            if (chuyenDi == null)
            {
                return HttpNotFound();
            }

            dbContext.ChuyenDi.Remove(chuyenDi);
            dbContext.SaveChanges();

            return RedirectToAction("QuanLyChuyenDi");
        }

        // =============================================== VeXe ===============================================
        public ActionResult TaoVeXe(int idLichTrinh, int soLuongVe)
        {
            try
            {
                // Kiểm tra nếu idLichTrinh không hợp lệ hoặc số lượng vé không hợp lệ
                if (idLichTrinh <= 0 || soLuongVe <= 0)
                {
                    ModelState.AddModelError("", "Dữ liệu không hợp lệ.");
                    return RedirectToAction("QuanLyChuyenDi");
                }

                // Lấy thông tin lịch trình
                var lichTrinh = dbContext.LichTrinh.FirstOrDefault(lt => lt.IdLichTrinh == idLichTrinh);

                if (lichTrinh == null)
                {
                    ModelState.AddModelError("", "Lịch trình không tồn tại.");
                    return RedirectToAction("QuanLyChuyenDi");
                }

                // Lấy giá từ đối tượng ChuyenDi tương ứng với IdLichTrinh
                decimal giaVe = dbContext.ChuyenDi.FirstOrDefault(cd => cd.IdLichTrinh == idLichTrinh)?.Gia ?? 0;

                for (int i = 0; i < soLuongVe; i++)
                {
                    VeXe veXe = new VeXe
                    {
                        ThanhTien = giaVe * soLuongVe, // Tính giá vé bằng cách nhân giá của ChuyenDi với số lượng vé
                        SoLuongVe = 1, // Mỗi vé có số lượng là 1
                        NgayDat = DateTime.Now, // Ngày đặt vé
                        TrangThai = "Chưa thanh toán", // Trạng thái mặc định
                        IdLichTrinh = idLichTrinh,
                        IdKhachHang = null, // Chưa có thông tin khách hàng khi tạo vé
                        GhiChu = ""
                    };

                    dbContext.VeXe.Add(veXe);
                }

                dbContext.SaveChanges();

                return RedirectToAction("QuanLyChuyenDi");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi: " + ex.Message);
                return RedirectToAction("QuanLyChuyenDi");
            }
        }

    }
}
