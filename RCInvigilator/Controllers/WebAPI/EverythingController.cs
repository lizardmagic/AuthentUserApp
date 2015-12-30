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
    public class EverythingController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Everything
        public IQueryable<Everything> GetEverything()
        {
            return db.Everything.Include(e => e.InvigilatorSession.Invigilator).Include(e => e.InvigilatorSession.Session)
								.Include(e => e.ExamStudent.Exam).Include(e => e.ExamStudent.Student);
        }

        // GET: api/Everything/5
        [ResponseType(typeof(Everything))]
        public IHttpActionResult GetEverything(int id)
        {
            Everything everything = db.Everything.Find(id);
            if (everything == null)
            {
                return NotFound();
            }

            return Ok(everything);
        }

        // PUT: api/Everything/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEverything(int id, Everything everything)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != everything.Id)
            {
                return BadRequest();
            }

            db.Entry(everything).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EverythingExists(id))
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

        // POST: api/Everything
        [ResponseType(typeof(Everything))]
        public IHttpActionResult PostEverything(Everything everything)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Everything.Add(everything);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = everything.Id }, everything);
        }

        // DELETE: api/Everything/5
        [ResponseType(typeof(Everything))]
        public IHttpActionResult DeleteEverything(int id)
        {
            Everything everything = db.Everything.Find(id);
            if (everything == null)
            {
                return NotFound();
            }

            db.Everything.Remove(everything);
            db.SaveChanges();

            return Ok(everything);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EverythingExists(int id)
        {
            return db.Everything.Count(e => e.Id == id) > 0;
        }
    }
}