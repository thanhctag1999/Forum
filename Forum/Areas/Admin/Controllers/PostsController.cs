using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Forum.Models;

namespace Forum.Areas.Admin.Controllers
{
    public class PostsController : Controller
    {
        private ForumDB db = new ForumDB();

        // GET: Admin/Posts
        public ActionResult Index()
        {
            if (Session["user_name"] == null)
            {
                return RedirectToAction("Login", "Login", new { Area = "" });
            }
            var posts = db.Posts.Include(p => p.Topic).Include(p => p.User);
            return View(posts.OrderByDescending(p => p.id_post).ToList());
           // return null;       
        }

        // GET: Admin/Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Admin/Posts/Create
        public ActionResult Create()
        {
            ViewBag.id_topic = new SelectList(db.Topics, "id_topic", "topic_name");
            ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name");
            return View();
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_post,post_name,content_post,created_at,id_user,flag,id_topic")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.id_topic = new SelectList(db.Topics, "id_topic", "topic_name", post.id_topic);
            ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name", post.id_user);
            return View(post);
        }

        // GET: Admin/Posts/Edit/5
        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }         
            return null;
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id)
        {

            Post post = db.Posts.Find(id);
            db.Database.ExecuteSqlCommand(
                    "UPDATE dbo.Posts SET flag = '1' WHERE id_post = " + post.id_post);
            return RedirectToAction("Index");

        }

        // GET: Admin/Posts/Delete/5
        public ActionResult Reject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        // POST: Admin/Posts/Delete/5
        [HttpPost, ActionName("Reject")]
        [ValidateAntiForgeryToken]
        public ActionResult RejectConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Database.ExecuteSqlCommand(
                    "UPDATE dbo.Posts SET flag = '2' WHERE id_post = " + post.id_post);
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
