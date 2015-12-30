using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RCInvigilator.Models
{
    public class Exam
    {
        public int ExamId { get; set; }
		[DisplayName("Course Title")]
        public string CourseTitle { get; set; }
		[DisplayName("Course Code")]
        public string CourseCode { get; set; }
        [DisplayName("Start Time")]
        [DisplayFormat(DataFormatString = "{0:t}")]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }
        [DisplayName("End Time")]
        [DisplayFormat(DataFormatString = "{0:t}")]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }
        public int SessionId { get; set; }
		[ForeignKey("SessionId")]
        public Session Session { get; set; }

		static public int CountEnrolled(int id)
		{
			ApplicationDbContext db = new ApplicationDbContext();
			return db.ExamStudents.Where(es => es.ExamId == id).Count();
		}

		static public int CountAuthenticated(int id)
		{
			ApplicationDbContext db = new ApplicationDbContext();
			return db.ExamStudents.Where(es => es.ExamId == id && es.Student.Authenticated).Count();
		}
    }
}