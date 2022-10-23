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
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using HostelManagement.Models;

namespace HostelManagement.Controllers
{
    public class AllocationsApiController : ApiController
    {
        private HostelDatabaseEntities2 db = new HostelDatabaseEntities2();

        // GET: api/AllocationsApi
        public async Task<IHttpActionResult> GetAllocations()
        {

            var allocations = db.Allocations.ToList();
            foreach(var a in allocations)
            {
                a.User = db.Users.Find(a.User_id);
            }
            return Ok(allocations);
        }


        // GET: api/AllocationsApi/5
        [ResponseType(typeof(Allocation))]
        public async Task<IHttpActionResult> GetAllocation(int id)
        {
            Allocation allocation = await db.Allocations.FindAsync(id);
            int a =(int) allocation.User_id;
            allocation.User = db.Users.Find(allocation.User_id);
            if (allocation == null)
            {
                return NotFound();
            }

            return Ok(allocation);
        }

        // PUT: api/AllocationsApi/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAllocation(int id, Allocation allocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != allocation.Id)
            {
                return BadRequest();
            }

            db.Entry(allocation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AllocationExists(id))
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

        // POST: api/AllocationsApi
        [ResponseType(typeof(Allocation))]
        public async Task<IHttpActionResult> PostAllocation(Allocation allocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            allocation.Room = db.Rooms.Find(allocation.Room_no);
            allocation.User = db.Users.Find(allocation.User_id);
            db.Allocations.Add(allocation);
        

            User u=db.Users.Find(allocation.User_id);
            u.Status = 2;
            
            Room room = db.Rooms.Find(allocation.Room_no);
            room.available=room.available- 1;

            db.Entry(u).State = EntityState.Modified;
            db.Entry(room).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AllocationExists(allocation.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            //return CreatedAtRoute("DefaultApi", new { id = allocation.Id }, allocation);
            //var allocations = db.Allocations.Include(a => a.Room).Include(a => a.User);
            return Ok(db.Allocations); 
                

        }

        // DELETE: api/AllocationsApi/5
        [ResponseType(typeof(Allocation))]
        public async Task<IHttpActionResult> DeleteAllocation(int id)
        {
            Allocation allocation = await db.Allocations.FindAsync(id);
            if (allocation == null)
            {
                return NotFound();
            }

            db.Allocations.Remove(allocation);
            await db.SaveChangesAsync();

            return Ok(allocation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AllocationExists(int id)
        {
            return db.Allocations.Count(e => e.Id == id) > 0;
        }
    }
}