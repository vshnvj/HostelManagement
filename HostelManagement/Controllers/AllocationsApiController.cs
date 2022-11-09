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


        [Route("GetAllTrackRent")]
        [ResponseType(typeof(Allocation))]
        public async Task<IHttpActionResult> GetAllTrackRent(string month,string year)
        {
            

            var temp = db.Payments.ToList().FindAll(x => x.Date_of_payment.Value.Month.ToString() == month
                                                         && x.Date_of_payment.Value.Year.ToString()==year);

           
                                        
var allrents = from all in db.Allocations.Include(x => x.Room).Include(x => x.User).ToList()
                           join pay in temp.ToList()
                           on all.User_id equals pay.User_id
                           into data_A
                           from data_B in data_A.DefaultIfEmpty(new Payment())
                           select new
                           {
                               all.Room_no,
                               all.User_id,
                               all.User.Name,
                               all.User.Mobile,
                               Rent = int.Parse(all.Room.Rent.ToString()),
                               Date_of_payment = data_B.Date_of_payment,
                               Amount = data_B.Amount
                           };
            //return Ok(allrents);



            string con = "data source=(localdb)\\ProjectsV13;initial catalog=HostelDatabase;integrated security=True";
            SqlConnection conn = new SqlConnection(con);
            conn.Open();
            SqlCommand command = new SqlCommand(@"select d.room_no,d.User_id,d.name,d.mobile,d.rent,
                                                     p.amount ,p.date_of_payment   
                                                        from deallocation d ,payment p
                                                    where d.User_id=p.User_id and Month(p.date_of_payment)=@mon", conn);

            List<ViewModel> v = new List<ViewModel>();
            command.Parameters.AddWithValue("@mon", month);
            //v.AddRange((IEnumerable<ViewModel>)allrents);
            SqlDataReader r = command.ExecuteReader();
            while (r.Read())
            {

                int room_no = int.Parse(r[0].ToString());

                int uid = int.Parse(r[1].ToString());

                ViewModel view = new ViewModel();
                view.Room_no = room_no;
                view.User_id = uid;
                view.Name = r[2].ToString();
                view.Mobile = r[3].ToString();
                view.Rent = int.Parse(r[4].ToString());
                view.Amount = int.Parse(r[5].ToString());
                view.Date_of_payment = DateTime.Parse(r[6].ToString());
                  v.Add(view);

            }
            //List<ViewModel> l =(List<ViewModel>) allrents;
            Tuple< dynamic, List <ViewModel>> t = new Tuple< dynamic,List<ViewModel>>(allrents, v);
            return Ok(t);

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
            //db.Allocations.Add(allocation);
        

            User u=db.Users.Find(allocation.User_id);
            u.Status = 2;
            u.Allocations.Add(allocation);

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
            //return Ok(all);


            string con = "data source=(localdb)\\ProjectsV13;initial catalog=HostelDatabase;integrated security=True";
            SqlConnection conn = new SqlConnection(con);
            conn.Open();
            SqlCommand command = new SqlCommand(@"insert into deallocation(room_no,user_id,name,mobile,rent,date_of_allocation)
            values(@r,@uid,@name,@mob,@rent,@doa)", conn);
            command.Parameters.AddWithValue("@r", room.Room_no);
            command.Parameters.AddWithValue("@uid", u.Id);

            command.Parameters.AddWithValue("@name", u.Name);
            command.Parameters.AddWithValue("@mob", u.Mobile);
            command.Parameters.AddWithValue("@rent", room.Rent);
            command.Parameters.AddWithValue("@doa", all.Date_of_allocation);

            int r = command.ExecuteNonQuery();
            if (r > 0)
            {
                return Ok(all);
            }
            return BadRequest();
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