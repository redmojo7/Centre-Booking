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
using WebApiBackend.Models;

namespace WebApiBackend.Controllers
{
    public class BookingsController : ApiController
    {
        private bookingdbEntities db;

        public BookingsController()
        {
            db = new bookingdbEntities();
            db.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Bookings
        public IHttpActionResult GetBookings()
        {
           // var a =  db.Bookings.Include(e => e.Centre).ToList();
            List<Booking> bookings = db.Bookings.Include(e => e.Centre).ToList();
            /*
            foreach (var b in bookings)
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(results);
            {
                Centre centre = centres.FirstOrDefault(c => c.Id == b.CentreId);
                centre.Bookings = null;
                b.Centre = centre;
            }*/
            return Ok(bookings);
        }

        // GET: api/Bookings/5
        [ResponseType(typeof(Booking))]
        public IHttpActionResult GetBooking(int id)
        {
            Booking booking = db.Bookings.Include(e => e.Centre).FirstOrDefault(b => b.Id == id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        // PUT: api/Bookings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBooking(int id, Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != booking.Id)
            {
                return BadRequest();
            }

            db.Entry(booking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        // POST: api/Bookings
        [ResponseType(typeof(Booking))]
        public IHttpActionResult PostBooking(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Booking> bookings = db.Bookings.Where(b => b.CentreId == booking.CentreId).ToList();
            foreach (var b in bookings)
            {
                bool overlap = booking.StartDate <= b.EndDate && b.StartDate <= booking.EndDate;
                if (overlap)
                {
                    return Conflict();
                }
            }
            db.Bookings.Add(booking);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = booking.Id }, booking);
        }

        // DELETE: api/Bookings/5
        [ResponseType(typeof(Booking))]
        public IHttpActionResult DeleteBooking(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            db.Bookings.Remove(booking);
            db.SaveChanges();

            return Ok(booking);
        }

        // GET: api/AvaliableDate/1
        [ResponseType(typeof(DateTime))]
        [HttpGet]
        [Route("api/Bookings/nextAvaliableDate/{centreId}")]
        public IHttpActionResult NextAvaliableDate(int centreId)
        {
            if (centreId == 0) 
            {
                return BadRequest("centreId cannot be empty.");
            }
            List<Booking> bookings = db.Bookings.Where(b => b.CentreId == centreId).ToList();
            DateTime avaliable = DateTime.Today.AddDays(1);

            if (bookings == null)
            {
                return Ok(avaliable);
            }

            DateTime tody = DateTime.Today;
            // firstly searching
            foreach (var b in bookings) 
            {
                DateTime start = b.StartDate;
                DateTime end = b.EndDate;
                if (tody.Subtract(start).TotalDays >= 0 && tody.Subtract(end).TotalDays <= 0)  // Between
                {
                    avaliable = new[] { end.AddDays(1), avaliable }.Max();
                }
            }
            // second seaching
            foreach (var b in bookings) 
            {
                DateTime start = b.StartDate;
                DateTime end = b.EndDate;
                if (tody.Subtract(start).TotalDays <= 0 && tody.Subtract(end).TotalDays <= 0)
                {
                    if (avaliable.Subtract(start).TotalDays >= 0 && avaliable.Subtract(end).TotalDays <= 0) // Between
                    {
                        avaliable = end.AddDays(1);
                    }
                }
            }
            return Ok(avaliable);
        }

        
        [HttpGet]
        [Route("api/bookings/incentre/{centreId}")]
        // GET: api/Bookings
        public IHttpActionResult inCentre(int centreId)
        {
            List<Booking> bookings = db.Bookings.Include(e => e.Centre).Where(a => a.CentreId==centreId).ToList();
            return Ok(bookings);
        }


    protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookingExists(int id)
        {
            return db.Bookings.Count(e => e.Id == id) > 0;
        }
    }
}