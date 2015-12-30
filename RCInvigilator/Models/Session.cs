using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RCInvigilator.Models
{
    public class Session
    {
        [Key]
        public int SessionId { get; set; }
        public string Campus { get; set; }
        public string Building { get; set; }
        public string Room { get; set; }
        [DisplayName("Date of Session")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime SessionDate { get; set; }
    }
}