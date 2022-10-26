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
using System.Net.Http;

namespace HostelManagement.Controllers
{
    public class AllocationsController : Controller
    {
        private HostelDatabaseEntities2 db = new HostelDatabaseEntities2();

        // GET: Allocations
        //public async Task<ActionResult> Index()
        //{
        //    var allocations = db.Allocations.Include(a => a.Room).Include(a => a.User);
        //    return RedirectToAction("AllocatedRooms","AdminHome", new { list = allocations.ToList() });
        //}

        //// GET: Allocations/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Allocation allocation = await db.Allocations.FindAsync(id);
        //    if (allocation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(allocation);
        //}

        // GET: Allocations/Create
        public ActionResult Create(int? id)
        {
            List<Room> li = new List<Room>();
            li = db.Rooms.ToList().FindAll(r => r.available > 0);
            if (li == null)
            {
                ViewData["notavailable"] = "rooms not available";
                return RedirectToAction("Index", "AdminHome");
            }
            ViewBag.Room_no = new SelectList(li, "Room_no", "Room_no");
            ViewBag.UserId = id;
            return View();
        }

        // POST: Allocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Room_no,User_id,Id")] Allocation allocation)
        {
            allocation.Date_of_allocation = System.DateTime.Now;

            HttpClient client = new HttpClient();
            string uri = "http://localhost:64533/api/AllocationsApi/";
            var response = client.PostAsJsonAsync<Allocation>(uri, allocation);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                //return RedirectToAction("Index");
                return RedirectToAction("AllocatedRooms", "AdminHome");

            }
            ViewBag.Room_no = new SelectList(db.Rooms, "Room_no", "Room_no");
            ViewBag.User_id = allocation.User_id;
            return RedirectToAction("AllocatedRooms", "AdminHome");
        }

        // GET: Allocations/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Allocation allocation = await db.Allocations.FindAsync(id);
        //    if (allocation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Room_no = new SelectList(db.Rooms, "Room_no", "Room_no", allocation.Room_no);
        //    ViewBag.User_id = new SelectList(db.Users, "Id", "Name", allocation.User_id);
        //    return View(allocation);
        //}

        // POST: Allocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Room_no,User_id,Date_of_allocation,Id")] Allocation allocation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(allocation).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Room_no = new SelectList(db.Rooms, "Room_no", "Room_no", allocation.Room_no);
        //    ViewBag.User_id = new SelectList(db.Users, "Id", "Name", allocation.User_id);
        //    return View(allocation);
        //}

        // GET: Allocations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpClient client = new HttpClient();
            string uri = "http://localhost:64533/api/AllocationsApi/";
            var response = client.DeleteAsync(uri+id.ToString());
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                //return RedirectToAction("Index");
                return RedirectToAction("AllocatedRooms", "AdminHome");

            }

            return View();
        }

        // POST: Allocations/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Allocation allocation = await db.Allocations.FindAsync(id);
        //    db.Allocations.Remove(allocation);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
