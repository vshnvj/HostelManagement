using System;
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
    public class PaymentsApiController : ApiController
    {
        private HostelDatabaseEntities2 db = new HostelDatabaseEntities2();

        // GET: api/PaymentsApi
        public IQueryable<Payment> GetPayments()
        {
            var payments = db.Payments.Include(p => p.User);
            return payments;
        }
        [HttpGet]
        [Route("AllocationPayments")]
        public IQueryable<Allocation> AllocationPayments()
        {
            var payments = db.Payments.Include(x => x.Amount).Join(db.Allocations, p => p.User, a => a.User, (p, a) => a).Include(x => x.User).Include(x => x.Room);

            return payments;
        }

        // GET: api/PaymentsApi/5
        [ResponseType(typeof(Payment))]
        public async Task<IHttpActionResult> GetPayment(int id)
        {
            Payment payment = await db.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        // PUT: api/PaymentsApi/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPayment(int id, Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != payment.Id)
            {
                return BadRequest();
            }

            db.Entry(payment).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
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

        // POST: api/PaymentsApi
        [ResponseType(typeof(Payment))]
        public async Task<IHttpActionResult> PostPayment(Payment payment)
        {
            string con = "data source=(localdb)\\ProjectsV13;initial catalog=HostelDatabase;integrated security=True";
            SqlConnection conn = new SqlConnection(con);
            conn.Open();
            SqlCommand command = new SqlCommand("insert into Payment(User_id,Amount,Date_of_payment) values  (@uid, @amount, @date)", conn);

            command.Parameters.AddWithValue("@uid", payment.User_id);
            command.Parameters.AddWithValue("@amount", payment.Amount);
            command.Parameters.AddWithValue("@date", payment.Date_of_payment);
            int r = command.ExecuteNonQuery();
            if (r > 0)
            {
                //return RedirectToAction("TrackRent", "AdminHome");
                return Ok(r);
            }



            return BadRequest();
        }

        // DELETE: api/PaymentsApi/5
        [ResponseType(typeof(Payment))]
        public async Task<IHttpActionResult> DeletePayment(int id)
        {
            Payment payment = await db.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            db.Payments.Remove(payment);
            await db.SaveChangesAsync();

            return Ok(payment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaymentExists(int id)
        {
            return db.Payments.Count(e => e.Id == id) > 0;
        }
    }
}