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
    public class InvigilatorSessionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/InvigilatorSessions
        public IQueryable<InvigilatorSession> GetInvigilatorSessions()
        {
            return db.InvigilatorSessions.Include(invigSess => invigSess.Invigilator).Include(invigSess => invigSess.Session);
        }

        // GET: api/InvigilatorSessions/5
        [ResponseType(typeof(InvigilatorSession))]
        public IHttpActionResult GetInvigilatorSession(int id)
        {
            InvigilatorSession invigilatorSession = db.InvigilatorSessions.Find(id);
            if (invigilatorSession == null)
            {
                return NotFound();
            }

            return Ok(invigilatorSession);
        }

        // PUT: api/InvigilatorSessions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInvigilatorSession(int id, InvigilatorSession invigilatorSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invigilatorSession.InvigilatorSessionId)
            {
                return BadRequest();
            }

            db.Entry(invigilatorSession).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvigilatorSessionExists(id))
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

        // POST: api/InvigilatorSessions
        [ResponseType(typeof(InvigilatorSession))]
        public IHttpActionResult PostInvigilatorSession(InvigilatorSession invigilatorSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.InvigilatorSessions.Add(invigilatorSession);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = invigilatorSession.InvigilatorSessionId }, invigilatorSession);
        }

        // DELETE: api/InvigilatorSessions/5
        [ResponseType(typeof(InvigilatorSession))]
        public IHttpActionResult DeleteInvigilatorSession(int id)
        {
            InvigilatorSession invigilatorSession = db.InvigilatorSessions.Find(id);
            if (invigilatorSession == null)
            {
                return NotFound();
            }

            db.InvigilatorSessions.Remove(invigilatorSession);
            db.SaveChanges();

            return Ok(invigilatorSession);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InvigilatorSessionExists(int id)
        {
            return db.InvigilatorSessions.Count(e => e.InvigilatorSessionId == id) > 0;
        }
    }
}