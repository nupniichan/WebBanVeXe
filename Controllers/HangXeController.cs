using System;
using System.Linq;
using System.Web.Mvc;
using ltweb_.Models;

public class HangXeController : Controller
{
    private DBQuanLyXeEntities db = new DBQuanLyXeEntities();

    public ActionResult BecomeOurPartner()
    {
        return View();
    }
    public bool IsLoggedIn
    {
        get
        {
            return Session["UserId"] != null; // Kiểm tra xem người dùng đã đăng nhập hay chưa
        }
    }

    [HttpGet]
    public ActionResult RegisterForHangXeOnly()
    {
        if (IsLoggedIn)
        {
            return RedirectToAction("Index", "Dashboard");
        }
        return View();
    }

    [HttpPost]
    public ActionResult RegisterForHangXeOnly(HangXe hangXe)
    {
        if (ModelState.IsValid)
        {
            // Kiểm tra xem tên đăng nhập đã tồn tại chưa
            if (db.HangXe.Any(x => x.TenDangNhap == hangXe.TenDangNhap))
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại.");
            }
            else
            {
                // Kiểm tra xem mật khẩu nhập lại khớp với mật khẩu hay không
                if (hangXe.MatKhau != hangXe.NhapLaiMatKhau)
                {
                    ModelState.AddModelError("NhapLaiMatKhau", "Mật khẩu nhập lại không khớp.");
                }
                else
                {
                    try
                    {
                        db.HangXe.Add(hangXe);
                        db.SaveChanges();
                        return RedirectToAction("Login");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Lỗi trong quá trình lưu hãng xe.");
                    }
                }
            }
        }
        return View();
    }

    public ActionResult Login()
    {
        if (IsLoggedIn)
        {
            return RedirectToAction("Index", "Dashboard");
        }
        return View();
    }

    [HttpPost]
    public ActionResult Login(string TenDangNhap, string MatKhau)
    {
        var hangXe = db.HangXe.FirstOrDefault(x => x.TenDangNhap == TenDangNhap && x.MatKhau == MatKhau);
        if (hangXe != null)
        {
            Session["UserId"] = hangXe.IdHangXe;
            Session["TenHangXe"] = hangXe.TenHangXe;
            // Lưu IdHangXe vào Session
            Session["IdHangXe"] = hangXe.IdHangXe;
            return RedirectToAction("Index", "Dashboard");
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
        return RedirectToAction("Login");
    }

    public ActionResult Dashboard()
    {
        if (!IsLoggedIn)
        {
            return RedirectToAction("Login");
        }

        return View(("~/Views/Dashboard/Index.cshtml"));
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
