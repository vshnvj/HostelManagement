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

            var allocations = db.Allocations.Include(a=>a.User).Include(x=>x.Room);
           
            return Ok(allocations);
        }


        // GET: api/AllocationsApi/5
        [ResponseType(typeof(Allocation))]
        public async Task<IHttpActionResult> GetAllocation(int id)
        {
            var allocation = db.Allocations.Include(x => x.User).Include(x => x.Room).ToList();
            Allocation all = allocation.Find(x => x.User_id == id);

            if (all == null)
            {
                return NotFound();
            }

            return Ok(all);
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
            var allocation = db.Allocations.Include(x => x.User).Include(x=>x.Room).ToList();
            Allocation all = allocation.Find(x => x.User_id == id);

            
            if (allocation == null)
            {
                return NotFound();
            }

           


            User u = all.User;
            u.Status = 3;

            Room room = all.Room;
            room.available = room.available + 1;

            db.Entry(u).State = EntityState.Modified;
            db.Entry(room).State = EntityState.Modified;
            db.Allocations.Remove(all);
            await db.SaveChangesAsync();

            return Ok(all);
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