using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Forum.Models;
using System.Windows.Forms;


namespace Forum.Controllers
{
    public class PostController : Controller
    {
        private ForumDB db = new ForumDB();

        public object SelectTopic { get; private set; }

        // GET: Post
        public async Task<ActionResult> Index()
        {
            var posts = db.Posts.Include(p => p.Topic).Include(p => p.User);
            return View(await posts.ToListAsync());
        }

        // GET: Post/Details/5
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

        // GET: Post/Create
        public ActionResult Post()
        {
            var user = Session["role-details"];
            if(user != null)
            {
                if (user.Equals("Nhà Nông"))
                {
                    ViewBag.id_topic = new SelectList(db.Topics.Where(t => t.flag.Equals(1) && t.id_topic.Equals(3) || t.id_topic.Equals(4)), "id_topic", "topic_name");
                    ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name");
                    if (Session["id-user"] != null)
                    {
                        return View();
                    }
                    else
                    {
                        TempData["Login"] = "Invalid";
                        return RedirectToAction("Login", "Login");
                    }
                }
                if (user.Equals("Nhà Doanh Nghiệp") || user.Equals("Nhà Doanh Nghi?p"))
                {
                    ViewBag.id_topic = new SelectList(db.Topics.Where(t => t.flag.Equals(1) && t.id_topic.Equals(1) || t.id_topic.Equals(4) || t.id_topic.Equals(9)), "id_topic", "topic_name");
                    ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name");
                    if (Session["id-user"] != null)
                    {
                        return View();
                    }
                    else
                    {
                        TempData["Login"] = "Invalid";
                        return RedirectToAction("Login", "Login");
                    }
                }
                if (user.Equals("Nhà Khoa Học") || user.Equals("Nhà Khoa H?c"))
                {
                    ViewBag.id_topic = new SelectList(db.Topics.Where(t => t.flag.Equals(1) && t.id_topic.Equals(2) || t.id_topic.Equals(5) || t.id_topic.Equals(7) || t.id_topic.Equals(8)), "id_topic", "topic_name");
                    ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name");
                    if (Session["id-user"] != null)
                    {
                        return View();
                    }
                    else
                    {
                        TempData["Login"] = "Invalid";
                        return RedirectToAction("Login", "Login");
                    }
                }
                else
                {
                    ViewBag.id_topic = new SelectList(db.Topics.Where(t => t.flag.Equals(1)), "id_topic", "topic_name");
                    ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name");
                    if (Session["id-user"] != null)
                    {
                        return View();
                    }
                    else
                    {
                        TempData["Login"] = "Invalid";
                        return RedirectToAction("Login", "Login");
                    }
                }
            }
            else
            {
                ViewBag.id_topic = new SelectList(db.Topics.Where(t => t.flag.Equals(1)), "id_topic", "topic_name");
                ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name");
                if (Session["id-user"] != null)
                {
                    return View();
                }
                else
                {
                    TempData["Login"] = "Invalid";
                    return RedirectToAction("Login", "Login");
                }
            } 
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Post([Bind(Include = "id_post,post_name,content_post,id_user,flag,id_topic,created_at,image")] Post post)
        {
            post.id_user = (int)Session["id-user"];
            post.flag = "0";
            post.created_at = DateTime.Now;
            db.Posts.Add(post);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
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
