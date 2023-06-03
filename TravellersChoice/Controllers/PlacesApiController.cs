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
using TravellersChoice.Models;

namespace TravellersChoice.Controllers
{
    public class PlacesApiController : ApiController
    {
        private TravellerContext db = new TravellerContext();


        PlacesApiController() 
        {
            db.Configuration.ProxyCreationEnabled = false;

        }
        // GET: api/PlacesApi
        public IQueryable<Place> GetPlace()
        {
            return db.Place;
        }

        // GET: api/PlacesApi/5
        [ResponseType(typeof(Place))]
        public IHttpActionResult GetPlace(int id)
        {
            Place place = db.Place.Find(id);
            if (place == null)
            {
                return NotFound();
            }

            return Ok(place);
        }

        // PUT: api/PlacesApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPlace(int id, Place place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != place.PlaceId)
            {
                return BadRequest();
            }

            db.Entry(place).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaceExists(id))
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

        // POST: api/PlacesApi
        [ResponseType(typeof(Place))]
        public IHttpActionResult PostPlace(Place place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Place.Add(place);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = place.PlaceId }, place);
        }

        // DELETE: api/PlacesApi/5
        [ResponseType(typeof(Place))]
        public IHttpActionResult DeletePlace(int id)
        {
            Place place = db.Place.Find(id);
            if (place == null)
            {
                return NotFound();
            }

            db.Place.Remove(place);
            db.SaveChanges();

            return Ok(place);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlaceExists(int id)
        {
            return db.Place.Count(e => e.PlaceId == id) > 0;
        }
    }
}