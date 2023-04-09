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

namespace Forum.Areas.Admin.Controllers
{
    public class TopicsController : Controller
    {
        private ForumDB db = new ForumDB();

        // GET: Admin/Topics
        public async Task<ActionResult> Index()
        {
            // Check Logined
            if (Session["user_name"] == null) 
            {
                TempData["Error"] = "You have not permission to access";
                return RedirectToAction("Login", "Login", new { Area = "" });
            }
            //Check Admin
            if (Session["role"].ToString() != "True")
            {
                TempData["Error"] = "You have not permission to access";
                return RedirectToAction("Index", "Home", new { Area = "" });
            }

            return View(await db.Topics.OrderByDescending(x => x.id_topic).ToListAsync());

        }

        // GET: Admin/Topics/Create
        public ActionResult Create()
        {
            // Check Logined
            if (Session["user_name"] == null)
            {
                TempData["Error"] = "You have not permission to access";
                return RedirectToAction("Login", "Login", new { Area = "" });
            }
            //Check Admin
            if (Session["role"].ToString() != "True")
            {
                TempData["Error"] = "You have not permission to access";
                return RedirectToAction("Index", "Home", new { Area = "" });
            }

            return View();
        }

        // POST: Admin/Topics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id_topic, topic_name, flag")] Topic topic)
        {
            // Check Logined
            if (Session["user_name"] == null)
            {
                TempData["Error"] = "You have not permission to access";
                return RedirectToAction("Login", "Login", new { Area = "" });
            }
            //Check Admin
            if (Session["role"].ToString() != "True")
            {
                TempData["Error"] = "You have not permission to access";
                return RedirectToAction("Index", "Home", new { Area = "" });
            }

            if (ModelState.IsValid)
            {
                var listTopic = db.Topics;
                if (listTopic.Where(t => t.topic_name.Equals(topic.topic_name)).Count() == 0)
                {
                    db.Topics.Add(topic);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = String.Format("Topic <strong>{0}</strong> has already been taken.", topic.topic_name);
                }
            }
            return View(topic);
        }

        // GET: Admin/Topics/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            // Check Logined
            if (Session["user_name"] == null)
            {
                TempData["Error"] = "You have not permission to access";
                return RedirectToAction("Login", "Login", new { Area = "" });
            }
            //Check Admin
            if (Session["role"].ToString() != "True")
            {
                TempData["Error"] = "You have not permission to access";
                return RedirectToAction("Index", "Home", new { Area = "" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Admin/Topics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id_topic, topic_name, flag")] Topic topic)
        {
            // Check Logined
            if (Session["user_name"] == null)
            {
                TempData["Error"] = "You have not permission to access";
                return RedirectToAction("Login", "Login", new { Area = "" });
            }
            //Check Admin
            if (Session["role"].ToString() != "True")
            {
                TempData["Error"] = "You have not permission to access";
                return RedirectToAction("Index", "Home", new { Area = "" });
            }

            if (ModelState.IsValid)
            {
                var listTopic = db.Topics;
                if (listTopic.Where(t => t.topic_name.Equals(topic.topic_name) && t.id_topic != topic.id_topic).Count() == 0)
                {
                    db.Entry(topic).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = String.Format("Topic <strong>{0}</strong> has already been taken.", topic.topic_name);
                }
            }
            return View(topic);
        }

        // POST: Admin/Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            // Check Logined
            if (Session["user_name"] == null)
            {
                TempData["Error"] = "You have not permission to access";
                return RedirectToAction("Login", "Login", new { Area = "" });
            }
            //Check Admin
            if (Session["role"].ToString() != "True")
            {
                TempData["Error"] = "You have not permission to access";
                return RedirectToAction("Index", "Home", new { Area = "" });
            }

            Topic topic = await db.Topics.FindAsync(id);
            if (topic.Posts.Count() == 0)
            {
                db.Topics.Remove(topic);
                TempData["Message"] = String.Format("Topic <strong>{0}</strong> deleted.", topic.topic_name);
            }
            else
            {
                topic.flag = 0;
                TempData["Message"] = String.Format("Topic <strong>{0}</strong> Inactive, because topic has posts.", topic.topic_name);
            }
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
