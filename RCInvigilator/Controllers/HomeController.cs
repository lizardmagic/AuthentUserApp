using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RCInvigilator.Models;

namespace RCInvigilator.Controllers
{
	[Authorize]
    public class HomeController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
			ExamsController ec = new ExamsController();
            ViewBag.Message = "Adminstration Portal.";
			List<Exam> exams = db.Exams.ToList();
			string[] labels = new String[exams.Count];
			int[] inExams = new int[exams.Count];
			int[] authenticated = new int[exams.Count];
			for (var i = 0; i < labels.Length; ++i)
			{
				labels[i] = exams[i].CourseCode;
				inExams[i] = Exam.CountEnrolled(exams[i].ExamId);
				authenticated[i] = Exam.CountAuthenticated(exams[i].ExamId);
			}
			ViewBag.chartLabels = labels;
			ViewBag.inExams = inExams;
			ViewBag.authenticated = authenticated;
			
            return View(db.Sessions.ToList());
        }
    }
}