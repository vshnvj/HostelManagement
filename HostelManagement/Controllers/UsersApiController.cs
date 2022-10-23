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
    public class UsersApiController : ApiController
    {
        private HostelDatabaseEntities2 db = new HostelDatabaseEntities2();

        // GET: api/Users
        public List<User> GetUsers()
        {
            
            return db.Users.ToList();
        }
        [Route("GetUserByEmail")]
        public async Task<IHttpActionResult> GetUserByEmail(string email)
        {
            User u=db.Users.FirstOrDefault(x => x.Email == email);
            if(u==null)
            {
                return NotFound();
            }
            return Ok(u);
        }


        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet]
        [Route("RequestSent")]
        public async Task<IHttpActionResult> RequestSent(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.Status = 1;
            db.Entry(user).State=EntityState.Modified;
            db.SaveChanges();
            return Ok(user);
        }


        // PUT: api/Users/5
        [Route("PutUser")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User user)
        {
            user.Id = db.Users.ToList().Count + 1;
            user.Status = 0;
            user.Role_id = 0;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.Users.Add(user);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(user);
                //CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }



    }
}