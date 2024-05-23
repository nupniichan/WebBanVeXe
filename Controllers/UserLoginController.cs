using System.Linq;
using System.Web.Mvc;
using ltweb_.Models;

public class AccountController : Controller
{
    private DBQuanLyXeEntities db = new DBQuanLyXeEntities(); // Thay YourDbContext bằng tên của DbContext thực sự

    public bool IsLoggedIn
    {
        get
        {
            return Session["UserId"] != null; // Kiểm tra xem người dùng đã đăng nhập hay chưa
        }
    }

    [HttpGet]
    public ActionResult Register()
    {
        if (IsLoggedIn)
        {
            return RedirectToAction("TrangChu", "Home"); // Chuyển đến trang chính nếu đã đăng nhập
        }
        return View();
    }

    [HttpPost]
    public ActionResult Register(KhachHang khachHang)
    {
        if (ModelState.IsValid)
        {
            // Kiểm tra xem tên đăng nhập đã tồn tại chưa
            if (db.KhachHang.Any(x => x.TenDangNhap == khachHang.TenDangNhap))
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại.");
            }
            else
            {
                // Kiểm tra xem mật khẩu nhập lại khớp với mật khẩu hay không
                if (khachHang.MatKhau != khachHang.NhapLaiMatKhau)
                {
                    ModelState.AddModelError("NhapLaiMatKhau", "Mật khẩu nhập lại không khớp.");
                }
                else
                {
                    // Hash mật khẩu trước khi lưu vào CSDL (đảm bảo rằng bạn sử dụng thư viện hash mật khẩu, ví dụ: BCrypt)
                    // Ví dụ: khachHang.MatKhau = BCrypt.HashPassword(khachHang.MatKhau);

                    db.KhachHang.Add(khachHang);
                    db.SaveChanges();
                    return RedirectToAction("TrangChu", "Home");
                }
            }
        }
        return View();
    }

    public ActionResult Login()
    {
        if (IsLoggedIn)
        {
            return RedirectToAction("TrangChu", "Home"); // Chuyển đến trang chính nếu đã đăng nhập
        }
        return View();
    }

    [HttpPost]
    public ActionResult Login(string TenDangNhap, string MatKhau)
    {
        var khachHang = db.KhachHang.FirstOrDefault(x => x.TenDangNhap == TenDangNhap && x.MatKhau == MatKhau);
        if (khachHang != null)
        {
            // Đăng nhập thành công, lưu thông tin đăng nhập vào Session
            Session["UserId"] = khachHang.IdKhachHang;
            Session["UserName"] = khachHang.TenDangNhap;
            return RedirectToAction("TrangChu", "Home");
        }
        ModelState.AddModelError("LoginError", "Tên đăng nhập hoặc mật khẩu không đúng.");
        return View();
    }
    public ActionResult Logout()
    {
        return View();
    }

    [HttpPost]
    public ActionResult ConfirmLogout()
    {
        Session.Clear();
        return RedirectToAction("TrangChu", "Home");
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            db.Dispose();
        }
        base.Dispose(disposing);
    }
}
