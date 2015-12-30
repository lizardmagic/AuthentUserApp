using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RCInvigilator.Models;

namespace RCInvigilator.Controllers
{
	[Authorize]
    public class SessionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Sessions
		public ActionResult Index()
		{
			return RedirectToAction("Dashboard", "Home", null);
		}

        public ActionResult AddExam(int? id)
        {
            return RedirectToAction("CreateWithSession", "Exams", new { id = id });
        }

        public ActionResult AddInvigilator(int? id)
        {
            return RedirectToAction("CreateWithSession", "Invigilators", new { id = id });
        }

		public ActionResult ViewExams(int? id)
		{
			return RedirectToAction("Index", "Exams", new { sessionId = id });
		}

        // GET: Sessions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
			ViewData["Exams"] = db.Exams.Where(e => e.SessionId == id).ToList();
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // GET: Sessions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SessionId,Campus,Building,Room,SessionDate")] Session session)
        {
            if (ModelState.IsValid)
            {
                db.Sessions.Add(session);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(session);
        }

        // GET: Sessions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SessionId,Campus,Building,Room,SessionDate")] Session session)
        {
            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(session);
        }

        // GET: Sessions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Session session = db.Sessions.Find(id);
			List<Exam> exams = db.Exams.Where(e => e.SessionId == id).ToList();
			foreach (var e in exams)
			{
				List<ExamStudent> examStudents = db.ExamStudents.Where(es => es.ExamId == e.ExamId).ToList();
				foreach (var es in examStudents)
				{
					db.ExamStudents.Remove(es);
				}
				db.SaveChanges();
				db.Exams.Remove(e);
			}
			db.SaveChanges();
            db.Sessions.Remove(session);
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
