using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RCInvigilator.Models
{
    public class Everything
    {
        public int Id { get; set; }
        public int InvigilatorSessionId { get; set; }
		[ForeignKey("InvigilatorSessionId")]
		public InvigilatorSession InvigilatorSession { get; set; }
        public int ExamStudentId { get; set; }
		[ForeignKey("ExamStudentId")]
		public ExamStudent ExamStudent { get; set; }
    }
}