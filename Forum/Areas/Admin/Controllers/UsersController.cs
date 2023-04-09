using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Forum.Models;
using Forum.Common;

namespace Forum.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private ForumDB db = new ForumDB();

        // GET: Admin/Users
        public ActionResult Index()
        {
            if (Session["user_name"] == null)
            { 
                return RedirectToAction("Login", "Login", new { Area = "" });
            }
            return View(db.Users.OrderByDescending(x => x.id_user).ToList());
        }
		
        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_user,user_name,pass,role,name,flag")] User user)
        {
            if (ModelState.IsValid)
            {
                        var model = db.Users.Where(x => x.user_name.Equals(user.user_name)).ToList();
                        if(model.Count() > 0)
                        {
                            ViewBag.Message = "Username already exist";
                        }
                        else
                        {
                            Encryption encryption = new Encryption();
                            var md5_pass = encryption.GetMD5(user.pass);
                            user.pass = md5_pass;
                            user.flag = false;
                            db.Users.Add(user);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
            }  

            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_user,user_name,pass,role,name,flag")] User user)
        {
            if (ModelState.IsValid)
            {
                var model = db.Users.AsNoTracking().Where(x => x.pass.Equals(user.pass)).ToList();
                if (model.Count() > 0)
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Encryption encryption = new Encryption();
                    var md5_pass = encryption.GetMD5(user.pass);
                    user.pass = md5_pass;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var checkUsers = db.Posts.AsNoTracking().Where(x => x.id_user.Equals(id)).ToList();
            if (checkUsers.Count() > 0)
            {
                Session["Message"] = "User Id: " + id + " already exists in Posts";
                return RedirectToAction("Index");
            }
            else {
                var user = db.Users.AsNoTracking().Where(x => x.id_user.Equals(id)).SingleOrDefault();
                user.flag = true;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
}
