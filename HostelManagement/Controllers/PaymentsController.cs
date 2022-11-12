using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HostelManagement.Models;
using System.Data.SqlClient;
using System.Net.Http;

namespace HostelManagement.Controllers
{
    public class PaymentsController : Controller
    {
        private HostelDatabaseEntities2 db = new HostelDatabaseEntities2();
        HttpClient client = new HttpClient();
        // GET: Payments
        DateTime doa;
        int Rent;
        
          
        public async Task<ActionResult> Index()
        {
            var response = client.GetAsync("http://localhost:64533/api/paymentsapi/");
            List<Payment> li = new List<Payment>();
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var pays = test.Content.ReadAsAsync<List<Payment>>();
                pays.Wait();
                li = pays.Result;
            }
            return View(li);
        }

        // GET: Payments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = await db.Payments.FindAsync(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }


        // GET: Payments/Create
        public ActionResult Create(int? id)
        {
            ViewBag.User_id = id;
            string uri = "http://localhost:64533/api/AllocationsApi/";
            var response = client.GetAsync(uri + id.ToString());
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                
                var a = test.Content.ReadAsAsync<Allocation>();
                var all = a.Result;
                doa = (DateTime)all.Date_of_allocation;
                Rent = (int)all.Room.Rent;
                TempData["User_id"] = id;
                TempData["doa"] = doa;
                TempData["Rent"] = Rent;


            }

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,User_id,Amount,Date_of_payment")] Payment payment)
        {

            if (TempData.ContainsKey("Rent"))
                Rent= Convert.ToInt32(TempData.Peek("Rent").ToString());

            if (TempData.ContainsKey("User_id"))
                payment.User_id = Convert.ToInt32(TempData.Peek("User_id").ToString());


            if (TempData.ContainsKey("doa"))
                doa= DateTime.Parse(TempData.Peek("doa").ToString());


            if (payment.Amount > Rent)
            {
                ModelState.AddModelError("rent", "Amount is more than the rent");
                return View();
            }
            if (payment.Date_of_payment < doa)
            {
                ModelState.AddModelError("Doa", "Guest was not allocated in this month");
                return View();
            }

            var response = client.PostAsJsonAsync<Payment>("http://localhost:64533/api/paymentsapi/", payment);
            response.Wait();
            var test = response.Result;
            
            if (test.IsSuccessStatusCode)
            {
               
                return RedirectToAction("TrackRent", "AdminHome");
            }
            return View();
           
          
        }

        // GET: Payments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = await db.Payments.FindAsync(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_id = new SelectList(db.Users, "Id", "Name", payment.User_id);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,User_id,Amount,Date_of_payment")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.User_id = new SelectList(db.Users, "Id", "Name", payment.User_id);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = await db.Payments.FindAsync(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Payment payment = await db.Payments.FindAsync(id);
            db.Payments.Remove(payment);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        





        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
