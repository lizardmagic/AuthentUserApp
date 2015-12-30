using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RCInvigilator.Models
{
    public class ExamStudent
    {
        public int ExamStudentId { get; set; }
        
        public int ExamId { get; set; }
		[ForeignKey("ExamId")]
        public Exam Exam { get; set; }
        
        public int StudentId { get; set; }
		[ForeignKey("StudentId")]
        public Student Student { get; set; }
    }
}