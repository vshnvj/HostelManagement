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
    public class complaintController : Controller
    {
        private HostelDatabaseEntities2 db = new HostelDatabaseEntities2();

        // GET: complaint
        string uri = "http://localhost:64533/api/complaintapi/";
        public async Task<ActionResult> Index()
        {
            //return View(await db.complaints.ToListAsync());
          

            
                HttpClient client = new HttpClient();
                List<complaint> r_list = new List<complaint>();
                var response = client.GetAsync(uri);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var employees = test.Content.ReadAsAsync<List<complaint>>();
                    employees.Wait();
                    r_list = employees.Result;
                }
                return View(r_list);
          

        }

        // GET: complaint/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            complaint complaint = await db.complaints.FindAsync(id);
            if (complaint == null)
            {
                return HttpNotFound();
            }
            return View(complaint);
        }

        // GET: complaint/Create
        public ActionResult Create(int? id)
        {
            HttpClient client = new HttpClient();

            var response = client.GetAsync("http://localhost:64533/api/usersapi/" + Session["id"].ToString());

            User user = new User();
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var r = test.Content.ReadAsAsync<User>();
                //r.Wait();
                user = r.Result;
            }
            return View(user);
            //return View();
        }

        // POST: complaint/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int? id,string sub,string details)
        {
            complaint complaint = new complaint();
            complaint.sub = sub;
            complaint.details = details;
            HttpClient client = new HttpClient();
            var response = client.PostAsJsonAsync<complaint>(uri, complaint);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                //return RedirectToAction("Index");


                return RedirectToAction("Index", "UsersHome",new { id = id});

            }
            return View();
        }



        public async Task<ActionResult> Delete(int? id)
        {

            HttpClient client = new HttpClient();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
       var response = client.DeleteAsync(uri + id);
        response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Complaint");
            }
            return View();
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
