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
    public class ExamsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Exams
        public ActionResult Index(int? sessionId)
        {
			ViewData["sessionId"] = sessionId;
			if (sessionId == null)
			{

			}
            return View(db.Exams.Where(e => e.SessionId == sessionId).ToList());
        }

        // GET: Exams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = db.Exams.Find(id);
			List<ExamStudent> examStudents = db.ExamStudents.Where(es => es.ExamId == id).ToList();
			List<Student> students = new List<Student>();
			foreach (var es in examStudents)
			{
				students.Add(db.Students.Where(s => s.StudentId == es.StudentId).Single());
			}
            if (exam == null)
            {
                return HttpNotFound();
            }
			ViewData["Students"] = students;
            return View(exam);
        }

        // GET: Exams/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Exams/CreateWithSession/1
        public ActionResult CreateWithSession(int id)
        {
            ViewData["sessionId"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWithSession([Bind(Include = "ExamId,CourseTitle,CourseCode,StartTime,EndTime,SessionId")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                db.Exams.Add(exam);
                db.SaveChanges();
				return RedirectToAction("Details", "Exams", new { id = exam.ExamId });
            }

            return View(exam);
        }

		public ActionResult FinishAddingToSession()
		{
			return RedirectToAction("Index", "Sessions", new object { });
		}

        // POST: Exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExamId,CourseTitle,CourseCode,StartTime,EndTime")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                db.Exams.Add(exam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(exam);
        }

        public ActionResult ImportStudents(int? id)
        {
            ViewData["ExamID"] = id;
            return View();
        }

        //POST: Exams/Import
        [HttpPost, ActionName("Import")]
        [ValidateAntiForgeryToken]
        public ActionResult Import(HttpPostedFileBase students)
        {
            string fileName = DateTime.Now.ToString("dd-mm-yyyy") + ".csv";
            string pathToSave = Server.MapPath("~/Content/");
			var examId = Convert.ToInt32(Request["ExamID"]);
            if (students.ContentLength > 0)
            {
                students.SaveAs(System.IO.Path.Combine(pathToSave, fileName));
            }
            else
            {
                throw new NullReferenceException();
            }
            var lines = System.IO.File.ReadAllLines(Server.MapPath("~/Content/" + fileName));
            foreach (var CSVStudent in lines)
            {
				var student = db.Students.Where(s => s.StudentNumber == CSVStudent).Single();
                
                ExamStudent es = new ExamStudent();
                {
                    es.StudentId = student.StudentId;
                    es.ExamId = examId;
                }
                db.ExamStudents.Add(es);
                db.SaveChanges();
				try
				{
					var invigSess = db.InvigilatorSessions.Find(es.Exam.SessionId);
					Everything everything = new Everything();
					{
						everything.ExamStudentId = es.ExamStudentId;
						everything.InvigilatorSessionId = invigSess.InvigilatorSessionId;
					}
					db.Everything.Add(everything);
					db.SaveChanges();
				}
				catch (NullReferenceException)
				{

				}
				
            }

			return RedirectToAction("Details", "Exams", new { id = examId });
        }

        // GET: Exams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = db.Exams.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        // POST: Exams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExamId,CourseTitle,CourseCode,StartTime,EndTime")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(exam);
        }

        // GET: Exams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = db.Exams.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        // POST: Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Exam exam = db.Exams.Find(id);
			var sessionId = exam.SessionId;
			List<ExamStudent> examStudents = db.ExamStudents.Where(es => es.ExamId == id).ToList();
			foreach (var es in examStudents)
			{
				db.ExamStudents.Remove(es);
			}
			db.SaveChanges();
            db.Exams.Remove(exam);
            db.SaveChanges();
			return RedirectToAction("Details", "Sessions", new { id = sessionId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
		public int session { get; set; }
	}
}
