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
    public class InvigilatorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invigilators
        public ActionResult Index()
        {
            return View(db.Invigilators.ToList());
        }

        // GET: Invigilators/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invigilator invigilator = db.Invigilators.Find(id);
            if (invigilator == null)
            {
                return HttpNotFound();
            }
            return View(invigilator);
        }

        public ActionResult CreateWithSession(int? sessionId)
        {
            ViewData["sessionId"] = sessionId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWithSession([Bind(Include = "InvigilatorId,Username,FirstName,LastName,EmailAddress,PhoneNumber")] Invigilator invigilator)
        {
            if (ModelState.IsValid)
            {
                db.Invigilators.Add(invigilator);
                db.SaveChanges();
                var sessionId = Convert.ToInt32(Request["SessionId"]);
                InvigilatorSession invigSess = new InvigilatorSession();
                {
                    invigSess.InvigilatorId = invigilator.InvigilatorId;
                    invigSess.SessionId = sessionId;
                }
                db.InvigilatorSessions.Add(invigSess);
                db.SaveChanges();
				var examStudents = db.ExamStudents.ToList<ExamStudent>();
				var matchedId = -1;
				foreach (ExamStudent es in examStudents)
				{
					if (db.Exams.Find(es.ExamId).SessionId == sessionId)
					{
						matchedId = es.ExamStudentId;
						break;
					}

				}
				if (matchedId > 0)
				{
					Everything everything = new Everything();
					{
						everything.ExamStudentId = matchedId;
						everything.InvigilatorSessionId = invigSess.InvigilatorSessionId;
					}
					db.Everything.Add(everything);
					db.SaveChanges();
				}
                return RedirectToAction("Index");
            }

            return View(invigilator);
        }

        // GET: Invigilators/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Invigilators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvigilatorId,Username,FirstName,LastName,EmailAddress,PhoneNumber")] Invigilator invigilator)
        {
            if (ModelState.IsValid)
            {
                db.Invigilators.Add(invigilator);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invigilator);
        }
       
        // GET: Invigilators/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invigilator invigilator = db.Invigilators.Find(id);
            if (invigilator == null)
            {
                return HttpNotFound();
            }
            return View(invigilator);
        }

        // POST: Invigilators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvigilatorId,Username,FirstName,LastName,EmailAddress,PhoneNumber")] Invigilator invigilator)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invigilator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invigilator);
        }

        // GET: Invigilators/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invigilator invigilator = db.Invigilators.Find(id);
            if (invigilator == null)
            {
                return HttpNotFound();
            }
            return View(invigilator);
        }

        // POST: Invigilators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invigilator invigilator = db.Invigilators.Find(id);
			List<InvigilatorSession> invigSessions = db.InvigilatorSessions.Where(invigSess => invigSess.InvigilatorId == id).ToList();
			foreach (var invig in invigSessions)
			{
				db.InvigilatorSessions.Remove(invig);
			}
			db.SaveChanges();
            db.Invigilators.Remove(invigilator);
            db.SaveChanges();
			return RedirectToAction("Dashboard", "Home", null);
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
