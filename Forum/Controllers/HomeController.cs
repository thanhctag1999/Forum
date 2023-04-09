using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using Forum.Models;

namespace Forum.Controllers
{
    public class HomeController : Controller
    {
        private ForumDB db = new ForumDB();
        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.User).Include(p => p.Topic);
            return View(posts.Where(s => s.flag.Equals("1")).OrderByDescending(s => s.id_post));
        }
        public ActionResult Detail(int id)
        {
            var post = db.Posts.Find(id);
            ViewBag.report = new SelectList(db.Reports, "id", "reason");
            var comments = db.Comments.AsNoTracking().Where(x => x.id_post.Equals(id)).Where(x => x.status != "hidden").OrderByDescending(x => x.id_comment).ToList();
            @ViewData["Comment"] = comments;
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Comment([Bind(Include = "id_comment,content_comment,id_post,id_user,create_at")] Comment comment)
        {
            if (Session["id-user"] != null)
            {
                comment.id_user = (int)Session["id-user"];
                comment.created_at = DateTime.Now;
                db.Comments.Add(comment);
                await db.SaveChangesAsync();
                return RedirectToAction("Detail", new { id = comment.id_post });
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            /*ViewBag.id_topic = new SelectList(db.Topics, "id_topic", "topic_name", post.id_topic);*/
            /*ViewBag.id_user = new SelectList(db.Users, "id_user", "user_name", post.id_user);*/
            /*return View(post);*/
        }

        [HttpPost]
        public ActionResult Search(String strSearch)
        {
            try
            {
                var model = db.Posts.Where(x => x.post_name.Contains(strSearch)).SingleOrDefault();
                if (model != null)
                {
                    int id = Int32.Parse(model.id_post.ToString());
                    var posts = db.Posts.Include(p => p.User);
                    return View(posts.Where(s => s.id_post.Equals(id)));
                }
                else
                {
                    return View();

                }
            }
            catch (Exception)
            {
                var posts = db.Posts.Include(p => p.User);
                return View(posts.Where(s => s.flag.Equals("1")).OrderByDescending(s => s.id_post));
            }
        }

        // POST: Home/Report/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Report(int? id, string reason, Report report)
        {
            Comment comment = db.Comments.Find(id);
           /* db.Database.ExecuteSqlCommand(
                    "UPDATE dbo.Comments SET status = 'report' WHERE id_comment = " + comment.id_comment);*/
            db.Database.ExecuteSqlCommand(
                   "INSERT INTO dbo.Reports(id, reason) VALUES ('" + id + "','" + reason + "')");
            var status = "success";
            return Json(status);
        }

        // POST: Home/Report/5
        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Report(string comment_id, string reason)
         {
             Comment comment = db.Comments.Find(comment_id);
             var text_reason = reason;
             db.Database.ExecuteSqlCommand(
                     "UPDATE dbo.Comments SET status = 'report' WHERE id_comment = " + comment.id_comment);
             var status = "success";
             return Json(status);
         }*/

        // GET: Home/Like/5
        public ActionResult Like(int? id)
        {
           
            Comment comment = db.Comments.Find(id);
            if (comment.like == null)
            {
                var like = 0;
                like = like + 1;
                db.Database.ExecuteSqlCommand(
                    "UPDATE dbo.Comments SET [like] = 1 WHERE id_comment = " + comment.id_comment);
                return RedirectToAction("Detail", new { id = comment.id_post });
            }
            else
            {
                var like = comment.like;
                like = like + 1;
                db.Database.ExecuteSqlCommand(
                        "UPDATE dbo.Comments SET [like] = " + like + " WHERE id_comment = " + comment.id_comment);
                return RedirectToAction("Detail", new { id = comment.id_post });
            }
           

        }
    }
}