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

namespace Forum.Controllers
{
    public class ProfileController : Controller
    {
        private ForumDB db = new ForumDB();

        // GET: Profile
        public ActionResult Index()
        {
            if(Session["id-user"] != null){
                var user_id = Session["id-user"];
                User user = db.Users.Find(user_id);
                return View(user);
            }
            else{
                return RedirectToAction("Index","Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "id_user,user_name,pass,role,name,flag,role_detail")] User user)
        {
            try
            {
                user.id_user = Convert.ToInt32(Session["id-user"]);
                user.user_name = Convert.ToString(Session["user_name"]);
                user.role = Convert.ToBoolean(Session["role"]);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                var user_id = user.id_user;
                User users = db.Users.Find(user_id);
                Session["name"] = users.name;
                Session["user-name"] = users.name;
                Session["user-username"] = users.user_name;
                Session["user-roledetail"] = users.role_detail;
                Session["user-area"] = users.area;
                return RedirectToAction("Index", "Profile");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Profile");
            }
        }
        // GET: Profile/Detail/5
        public ActionResult Detail(int? id)
        {

            if (id != null)
            {
                Session.Remove("user-name");
                Session.Remove("user-username");
                Session.Remove("user-roledetail");
                var user_id = id;
                User user = db.Users.Find(user_id);
                Session["user-name"] = user.name;
                Session["user-username"] = user.user_name;
                Session["user-roledetail"] = user.role_detail;
                Session["user-area"] = user.area;
                return RedirectToAction("Index", "Profile");
            }
            else
            {
                return RedirectToAction("Index", "Profile");
            }
                              }

        // GET: Profile/Create
        public ActionResult Create()
        {
           
            return View();
        }

        // POST: Profile/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_user,user_name,pass,role,name,image")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Profile/Edit/5
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

        // POST: Profile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_user,user_name,pass,role,name,image,role_detail,role_detail")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Profile/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Profile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
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
