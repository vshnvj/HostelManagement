﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
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
        private HostelDatabaseEntities2 db = new HostelDatabaseEntities2();

        // GET: api/complaintApi
        public IQueryable<complaint> Getcomplaints()
        {
            return db.complaints;
        }

        [Route("GetComplaintsList")]
        public List<complaint> GetComplaintsList(int userid)
        {
            return db.complaints.ToList().FindAll(x =>x.user==userid);
        }

        // GET: api/complaintApi/5
        //[ResponseType(typeof(complaint))]
        //public async Task<IHttpActionResult> Getcomplaint(int id)
        //{
        //    complaint complaint = await db.complaints.FindAsync(id);
        //    if (complaint == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(complaint);
        //}

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
           
            string con = "data source=(localdb)\\ProjectsV13;initial catalog=HostelDatabase;integrated security=True";
            SqlConnection conn = new SqlConnection(con);
            conn.Open();
            SqlCommand command = new SqlCommand(@"INSERT INTO [dbo].[complaint] ( [sub], [details], [user],[seen]) 
                                                                     values  (@u1, @u2, @u3,@u4)", conn);

            command.Parameters.AddWithValue("@u1", complaint.sub);
            command.Parameters.AddWithValue("@u2",complaint.details);
            command.Parameters.AddWithValue("@u3", complaint.user);
            command.Parameters.AddWithValue("@u4",0);

            int r;

            r = command.ExecuteNonQuery();

            if (r > 0)
            {

                return Ok();
            }
            return BadRequest();
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
            if(complaint.Seen == 0)
            {
                complaint.Seen = 1;
                db.Entry(complaint).State=EntityState.Modified;
                db.SaveChanges();
            }

            
            //db.complaints.Remove(complaint);
           // await db.SaveChangesAsync();

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