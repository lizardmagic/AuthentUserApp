using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RCInvigilator.Models
{
    public class Student
    {
        public int StudentId { get; set; }
		[DisplayName("Student Number")]
        public string StudentNumber { get; set; }
		[DisplayName("First Name")]
        public string FirstName { get; set; }
		[DisplayName("Last Name")]
        public string LastName { get; set; }
		[DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
		[DisplayName("Date of Birth")]
        public DateTime DateOfBirth { get; set; }
		public bool Authenticated { get; set; }
		public string PicId { get; set; }
    }
}