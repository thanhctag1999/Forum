using Forum.Models;
using System.Web.Mvc;
using Forum.Common;
using System.Linq;

namespace Forum.Controllers
{
    public class RegisterController : Controller
    {
        private ForumDB db = new ForumDB();

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterUser reuser)
        {
            Encryption encryption = new Encryption();
            var data = db.Users.Where(s => s.user_name.Equals(reuser.user_name)).ToList();
            if (reuser.user_name == null || reuser.name == null || reuser.pass == null || reuser.ConfirmPassword == null)
            {
                TempData["CheckInformation"] = "Vui lòng điền đầy đủ thông tin";
            }
            else if (reuser.pass != reuser.ConfirmPassword)
            {
                TempData["ConfirmPass"] = "Mật khẩu không khớp";
            }
            else if (data.Count > 0)
            {
                TempData["Login"] = "Tài khoản đã tồn tại";
            }
            else
            {
                User user = new User();
                user.user_name = reuser.user_name;
                user.name = reuser.name;
                user.pass = encryption.GetMD5(reuser.pass);
                db.Configuration.ValidateOnSaveEnabled = false;
                user.flag = false;
                user.role_detail = reuser.role_detail;
                user.area = reuser.area;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login", "Login");
            }
            return RedirectToAction("Register", "Register");
        }
    }
}