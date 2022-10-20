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
    public class UsersController : Controller
    {
        private HostelDatabaseEntities2 db = new HostelDatabaseEntities2();

        // GET: Users
        HttpClient client = new HttpClient();
        string uri = "http://localhost:64533/api/usersapi";
        public async Task<ActionResult> Index()
        {

            //

            var response = client.GetAsync("http://localhost:64533/api/usersapi");
            List<User> li = new List<User>();
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var rooms = test.Content.ReadAsAsync<List<User>>();
                rooms.Wait();
                li = rooms.Result;
            }
            return View(li);
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            var response = client.GetAsync("http://localhost:64533/api/usersapi/" + id.ToString());
            User li = new User();
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var r = test.Content.ReadAsAsync<User>();
                //r.Wait();
                li = r.Result;
            }
            return View(li);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.Role_id = new SelectList(db.Roles, "Role_id", "Role1");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Gender,Mobile,Email,Address,Status,Password,Role_id")] User user)
        {
            var response = client.PostAsJsonAsync<User>("http://localhost:64533/api/usersapi", user);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var response = client.GetAsync(uri+"/" + id.ToString());

            var test = response.Result;
            var res = test.Content.ReadAsAsync<User>();
            res.Wait();
            User user = res.Result;


            if (user== null)
            {
                return HttpNotFound();
            }
            return View(user);
           
           // ViewBag.Role_id = new SelectList(db.Roles, "Role_id", "Role1", user.Role_id);
           
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Gender,Mobile,Email,Address,Status,Password,Role_id")] User user)
        {
            
            var response = client.PutAsJsonAsync<User>("http://localhost:64533/putuser?id=" + user.Id.ToString(), user);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var response = client.GetAsync(uri + "/" + id.ToString());

            var test = response.Result;
            var res = test.Content.ReadAsAsync<User>();
            res.Wait();
            User user = res.Result;


            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {

            var response = client.DeleteAsync("http://localhost:64533/api/usersapi/" + id.ToString());
            response.Wait();
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
