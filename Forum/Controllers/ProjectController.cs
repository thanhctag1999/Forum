using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Forum.Models;

namespace Forum.Controllers
{
    public class ProjectController : Controller
    {
        private ForumDB db = new ForumDB();

        // GET: Project
        public async Task<ActionResult> Index()
        {
            var posts = db.Posts.Include(p => p.Topic).Include(p => p.User).Include(p => p.User1).Include(p => p.User2).OrderByDescending(p => p.id_post); ;
            return View(await posts.ToListAsync());
        }

        // GET: Project/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = await db.Posts.FindAsync(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            ViewBag.id_topic = new SelectList(db.Topics, "id_topic", "topic_name");
            ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name");
            ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name");
            ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name");
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id_post,post_name,id_user,flag,id_topic,created_at,content_post,image")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.id_topic = new SelectList(db.Topics, "id_topic", "topic_name", post.id_topic);
            ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name", post.id_user);
            ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name", post.id_user);
            ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name", post.id_user);
            return View(post);
        }

        // GET: Project/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = await db.Posts.FindAsync(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_topic = new SelectList(db.Topics, "id_topic", "topic_name", post.id_topic);
            ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name", post.id_user);
            ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name", post.id_user);
            ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name", post.id_user);
            return View(post);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id_post,post_name,id_user,flag,id_topic,created_at,content_post,image")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.id_topic = new SelectList(db.Topics, "id_topic", "topic_name", post.id_topic);
            ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name", post.id_user);
            ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name", post.id_user);
            ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name", post.id_user);
            return View(post);
        }

        // GET: Project/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = await db.Posts.FindAsync(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Post post = await db.Posts.FindAsync(id);
            db.Posts.Remove(post);
            await db.SaveChangesAsync();
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
