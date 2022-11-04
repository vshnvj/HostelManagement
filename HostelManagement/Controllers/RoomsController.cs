using HostelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace HostelManagement.Controllers
{
    public class RoomsController : Controller
    {

        string uri = "http://localhost:64533/api/roomsapi/";

        // GET: RoomsMvc
        public ActionResult Index()
        {
            HttpClient client = new HttpClient();
            List<Room> r_list = new List<Room>();
                var response = client.GetAsync(uri);
              response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var employees = test.Content.ReadAsAsync<List<Room>>();
                employees.Wait();
                r_list = employees.Result;
            }
            return View(r_list);
        }



        // GET: RoomsMvc/Details/5
        public ActionResult Details(int? id)
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync("http://localhost:64533/GetRoomGuets?id=" + id.ToString());
            List<Allocation> list = new List<Allocation>();
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var r = test.Content.ReadAsAsync<List<Allocation>>();
                //r.Wait();
                list= r.Result;
            }
            return View(list);
        }



        // GET: RoomsMvc/Create
        public ActionResult Create()
        {
            return View();
        }



        // POST: RoomsMvc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Room_no,Capacity,available,Rent")] Room room)
        {
            HttpClient client = new HttpClient();
            var response = client.PostAsJsonAsync<Room>(uri, room);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                //return RedirectToAction("Index");
                return RedirectToAction("Rooms","AdminHome");

            }
            return View();
        }



        // GET: RoomsMvc/Edit/5
        public ActionResult Edit(int? id)
        {
            HttpClient client = new HttpClient();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var response = client.GetAsync(uri+ id.ToString());



            var test = response.Result;
            var res = test.Content.ReadAsAsync<Room>();
            res.Wait();
            Room room1 = res.Result;






            if (room1 == null)
            {
                return HttpNotFound();
            }
            return View(room1);



        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "Room_no,Capacity,available,Rent")] Room room)
        {
            HttpClient client = new HttpClient();
            var response = client.PutAsJsonAsync<Room>(uri + id.ToString(), room);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                //return RedirectToAction("Index");

                return RedirectToAction("Rooms", "AdminHome");

            }
            return View();
        }



        // GET: RoomsMvc/Delete/5
        public ActionResult Delete(int? id)
        {
           
            HttpClient client = new HttpClient();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var response = client.GetAsync(uri + id.ToString());



            var test = response.Result;
            var res = test.Content.ReadAsAsync<Room>();
            res.Wait();
            Room room1 = res.Result;






            if (room1 == null)
            {
                return HttpNotFound();
            }
            return View(room1);
        }



        // POST: RoomsMvc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HttpClient client = new HttpClient();



            var response = client.DeleteAsync(uri + id);



            response.Wait();

            //return RedirectToAction("Index");
            return RedirectToAction("Rooms", "AdminHome");

        }




    }
}
    