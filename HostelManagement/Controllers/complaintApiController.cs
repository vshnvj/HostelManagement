using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HostelManagement.Models;

namespace HostelManagement.Controllers
{
    public class complaintApiController : ApiController
    {
        private HostelDatabaseEntities6 db = new HostelDatabaseEntities6();

        // GET: api/complaintApi
        public IQueryable<complaint> Getcomplaints()
        {
            return db.complaints;
        }

        // GET: api/complaintApi/5
        [ResponseType(typeof(complaint))]
        public async Task<IHttpActionResult> Getcomplaint(int id)
        {
            complaint complaint = await db.complaints.FindAsync(id);
            if (complaint == null)
            {
                return NotFound();
            }

            return Ok(complaint);
        }

        // PUT: api/complaintApi/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putcomplaint(int id, complaint complaint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != complaint.Id)
            {
                return BadRequest();
            }

            db.Entry(complaint).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!complaintExists(id))
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

        // POST: api/complaintApi
        [ResponseType(typeof(complaint))]
        public async Task<IHttpActionResult> Postcomplaint(complaint complaint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.complaints.Add(complaint);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (complaintExists(complaint.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = complaint.Id }, complaint);
        }

        // DELETE: api/complaintApi/5
        [ResponseType(typeof(complaint))]
        public async Task<IHttpActionResult> Deletecomplaint(int id)
        {
            complaint complaint = await db.complaints.FindAsync(id);
            if (complaint == null)
            {
                return NotFound();
            }

            db.complaints.Remove(complaint);
            await db.SaveChangesAsync();

            return Ok(complaint);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool complaintExists(int id)
        {
            return db.complaints.Count(e => e.Id == id) > 0;
        }
    }
}