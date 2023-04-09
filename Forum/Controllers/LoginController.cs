using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using Forum.Common;

namespace Forum.Controllers
{
    public class LoginController : Controller
    {
        private ForumDB db = new ForumDB();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Remove("id-user");
            Session.Remove("user_name");
            Session.Remove("role");
            Session.Clear();
            return RedirectToAction("Index", "Home");

        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string user_name, string pass)
        {
            Encryption encryption = new Encryption();
            var md5_pass = encryption.GetMD5(pass);

            try
            {
                var data = db.Users.Where(s => s.user_name.Equals(user_name) && s.pass.Equals(md5_pass)).SingleOrDefault();
                if (data != null)
                {
                    //add session
                    if (data.flag.Equals(false))
                    {
                        Session["id-user"] = data.id_user;
                        Session["user_name"] = data.user_name;
                        Session["name"] = data.name;
                        Session["role"] = data.role;
                        Session["role-details"] = data.role_detail;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Deleted"] = "Deleted";
                        return View();
                    }
                }
                else
                {
                    TempData["Invalid"] = "Invalid";
                    return View();
                }
            }
            catch (Exception)
            {
                TempData["Invalid"] = "Invalid";
                return View();
            }
            return View();
        }
    }
}