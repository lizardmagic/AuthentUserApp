using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RCInvigilator.Models;

namespace RCInvigilator.Controllers.WebAPI
{
    public class ExamStudentsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ExamStudents
        public IQueryable<ExamStudent> GetExamStudents()
        {
            return db.ExamStudents.Include(es => es.Exam).Include(es => es.Exam.Session).Include(es => es.Student);
        }

        // GET: api/ExamStudents/5
        [ResponseType(typeof(ExamStudent))]
        public IHttpActionResult GetExamStudent(int id)
        {
            ExamStudent examStudent = db.ExamStudents.Find(id);
            if (examStudent == null)
            {
                return NotFound();
            }

            return Ok(examStudent);
        }

        // PUT: api/ExamStudents/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutExamStudent(int id, ExamStudent examStudent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != examStudent.ExamStudentId)
            {
                return BadRequest();
            }

            db.Entry(examStudent).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamStudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ExamStudents
        [ResponseType(typeof(ExamStudent))]
        public IHttpActionResult PostExamStudent(ExamStudent examStudent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ExamStudents.Add(examStudent);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = examStudent.ExamStudentId }, examStudent);
        }

        // DELETE: api/ExamStudents/5
        [ResponseType(typeof(ExamStudent))]
        public IHttpActionResult DeleteExamStudent(int id)
        {
            ExamStudent examStudent = db.ExamStudents.Find(id);
            if (examStudent == null)
            {
                return NotFound();
            }

            db.ExamStudents.Remove(examStudent);
            db.SaveChanges();

            return Ok(examStudent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExamStudentExists(int id)
        {
            return db.ExamStudents.Count(e => e.ExamStudentId == id) > 0;
        }
    }
}