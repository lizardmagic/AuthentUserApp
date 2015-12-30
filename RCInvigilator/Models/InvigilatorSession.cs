using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RCInvigilator.Models
{
    public class InvigilatorSession
    {
        public int InvigilatorSessionId { get; set; }
        
        public int InvigilatorId { get; set; }
		[ForeignKey("InvigilatorId")]
        public Invigilator Invigilator { get; set; }
        
        public int SessionId { get; set; }
		[ForeignKey("SessionId")]
        public Session Session { get; set; }
    }
}