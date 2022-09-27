using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiBackend.Models;

namespace WebApiBackend.Controllers
{
    public class CentresController : ApiController
    {
        private bookingdbEntities db;

        public CentresController()
        {
            db = new bookingdbEntities();
            db.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Centres
        public IQueryable<Centre> GetCentres()
        {
            //db.Database.SqlQuery
           
            return db.Centres;
        }

        // GET: api/Centres/5
        [ResponseType(typeof(Centre))]
        public IHttpActionResult GetCentre(int id)
        {
            Centre centre = db.Centres.Find(id);
            if (centre == null)
            {
                return NotFound();
            }

            return Ok(centre);
        }

        // PUT: api/Centres/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCentre(int id, Centre centre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != centre.Id)
            {
                return BadRequest();
            }

            db.Entry(centre).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CentreExists(id))
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

        // POST: api/Centres
        [ResponseType(typeof(Centre))]
        public IHttpActionResult PostCentre(Centre centre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<Centre> centres = db.Centres.Where(c => c.Name == centre.Name).ToList();
            if ((centres != null) && (centres.Any()))
            {
                return Conflict();
            }

            db.Centres.Add(centre);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = centre.Id }, centre);
        }

        // DELETE: api/Centres/5
        [ResponseType(typeof(Centre))]
        public IHttpActionResult DeleteCentre(int id)
        {
            Centre centre = db.Centres.Find(id);
            if (centre == null)
            {
                return NotFound();
            }

            db.Centres.Remove(centre);
            db.SaveChanges();

            return Ok(centre);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CentreExists(int id)
        {
            return db.Centres.Count(e => e.Id == id) > 0;
        }
    }
}