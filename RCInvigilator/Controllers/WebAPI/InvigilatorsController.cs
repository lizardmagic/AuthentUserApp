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
    public class InvigilatorsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Invigilators
        public IQueryable<Invigilator> GetInvigilators()
        {
            return db.Invigilators;
        }

        // GET: api/Invigilators/5
        [ResponseType(typeof(Invigilator))]
        public IHttpActionResult GetInvigilator(int id)
        {
            Invigilator invigilator = db.Invigilators.Find(id);
            if (invigilator == null)
            {
                return NotFound();
            }

            return Ok(invigilator);
        }

        // PUT: api/Invigilators/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInvigilator(int id, Invigilator invigilator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invigilator.InvigilatorId)
            {
                return BadRequest();
            }

            db.Entry(invigilator).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvigilatorExists(id))
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

        // POST: api/Invigilators
        [ResponseType(typeof(Invigilator))]
        public IHttpActionResult PostInvigilator(Invigilator invigilator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Invigilators.Add(invigilator);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = invigilator.InvigilatorId }, invigilator);
        }

        // DELETE: api/Invigilators/5
        [ResponseType(typeof(Invigilator))]
        public IHttpActionResult DeleteInvigilator(int id)
        {
            Invigilator invigilator = db.Invigilators.Find(id);
            if (invigilator == null)
            {
                return NotFound();
            }

            db.Invigilators.Remove(invigilator);
            db.SaveChanges();

            return Ok(invigilator);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InvigilatorExists(int id)
        {
            return db.Invigilators.Count(e => e.InvigilatorId == id) > 0;
        }
    }
}