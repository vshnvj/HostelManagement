using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
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

        //[HttpGet]
        //[Route("TrackRent")]
        //public List<User> TrackRent()
        //{
        //    //var l = ;
        //    return db.Users.Include(x => x.Allocations ).Include(c=>c.Payments).ToList();
        //}


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
            string con = "data source=(localdb)\\ProjectsV13;initial catalog=HostelDatabase;integrated security=True";
            SqlConnection conn = new SqlConnection(con);
            conn.Open();
            user.Status = 0;
            user.Role_id = 0;
            SqlCommand command = new SqlCommand(@"INSERT INTO [dbo].[Users] ( [Name], [Gender], [Mobile], [Email], [Address], [Status], [Password], [Role_id]) 
                                                                     values  (@u1, @u2,@u3,@u4,@u5,@u6, @u7, @u8)", conn);

            command.Parameters.AddWithValue("@u1", user.Name);
            command.Parameters.AddWithValue("@u2", user.Gender);
            command.Parameters.AddWithValue("@u3", user.Mobile);
            command.Parameters.AddWithValue("@u4", user.Email);
            command.Parameters.AddWithValue("@u5", user.Address);
            command.Parameters.AddWithValue("@u6", 0);
            command.Parameters.AddWithValue("@u7", user.Password);
            command.Parameters.AddWithValue("@u8", 0); 
            int r;
           
                r = command.ExecuteNonQuery();
            
                if (r > 0)
            {
                
                return Ok(db.Users.FirstOrDefault(x=>x.Email==user.Email));
            }
            return BadRequest();

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