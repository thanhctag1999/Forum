using Forum.Models;
using System.Web.Mvc;
using Forum.Common;
using System.Linq;
using System.Data.Entity;

namespace Forum.Controllers
{
    public class ChangePasswordController : Controller
    {
        private ForumDB db = new ForumDB();

        // GET: ChangePassword
        public ActionResult ChangePassword()
        {
            if (Session["id-user"] != null)
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string CurrentPassword, string NewPassword, string ConfirmPassword)
        {
            Encryption encryption = new Encryption();
            if (CurrentPassword != null && NewPassword != null)
            {
                var md5_pass_old = encryption.GetMD5(CurrentPassword);
                var md5_pass_new = encryption.GetMD5(NewPassword);
                if (Session["user_name"] != null)
                {
                    string userName = Session["user_name"].ToString();
                    var data = db.Users.Where(s => s.user_name.Equals(userName) && s.pass.Equals(md5_pass_old));

                    if (data.Count() > 0)
                    {
                        User user = data.FirstOrDefault();
                        user.pass = md5_pass_new;
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();

                        Session.Remove("id-user");
                        Session.Remove("user_name");
                        Session.Remove("role");
                        ViewBag.Message = "Congratulations! Your password has been changed successfully.";
                        return RedirectToAction("Login", "Login");
                    }
                    else { ViewBag.Message = "Current password is incorrect!"; }
                }
                else { ViewBag.Message = "You are logged out!"; }
            }
            return View();
        }
    }
}
