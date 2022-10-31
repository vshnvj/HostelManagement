﻿using HostelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace HostelManagement.Controllers
{
    public class AdminHomeController : Controller
    {
        // GET: AdminHome
        HttpClient client = new HttpClient();


        string allocationUri = "api/AllocationsApi/";
        public ActionResult Index()
        {
            HttpClient client = new HttpClient();
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
                li = li.FindAll(x => x.Status == 1);
                return View(li);

         
        }

        public ActionResult Feedback()
        {
            return RedirectToAction("Index", "complaint");
        }

        public ActionResult Users(string search=null)
        {
           
            HttpClient client = new HttpClient();
            var response = client.GetAsync("http://localhost:64533/api/usersapi");
            List<User> li = new List<User>();
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var rooms = test.Content.ReadAsAsync<List<User>>();
                rooms.Wait();
                li = rooms.Result;
                if (search != null)
                {
                    li = li.FindAll(x => x.Name.ToLower() == search.ToLower());

                }
            }
            li = li.FindAll(x => x.Status == 2 && x.Role_id!=1);
            return View(li);


        }

        public ActionResult Allocate(int? id)
        {
            return RedirectToAction("Create", "Allocations", new { id = id });
        }
        public ActionResult Deallocate(int? id)
        {
            return RedirectToAction("Delete", "Allocations", new { id = id });
        }
        
        public ActionResult AllocatedRooms()
        {
            HttpClient client = new HttpClient();


            string uri = "http://localhost:64533/api/AllocationsApi/";
            var response = client.GetAsync(uri);
            response.Wait();
            var test = response.Result;
            List<Allocation> list = new List<Allocation>();
            if (test.IsSuccessStatusCode)
            {
                var employees = test.Content.ReadAsAsync<List<Allocation>>();
                employees.Wait();
                list = employees.Result;

            }
            return View(list);
        }
        

        public ActionResult Rooms()
        {
            string uri = "http://localhost:64533/api/roomsapi/";
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

        public ActionResult Edit(int? id)
        {
            return RedirectToAction("Edit", "Rooms", new { id = id });
        }
        public ActionResult Details(int? id)
        {
            return RedirectToAction("Details", "Rooms", new { id = id });
        }
        public ActionResult Delete(int? id)
        {
            return RedirectToAction("Delete", "Rooms", new { id = id });
        }

        public ActionResult TrackRent()
        {
            //List<Payment> pays = AllRents();
            //string uri = "http://localhost:64533/api/AllocationsApi/";
            //var response = client.GetAsync(uri);
            //response.Wait();
            //var test = response.Result;
            //List<Allocation> list = new List<Allocation>();
            //if (test.IsSuccessStatusCode)
            //{
            //    var employees = test.Content.ReadAsAsync<List<Allocation>>();
            //    employees.Wait();
            //    list = employees.Result;


            //    //var res = pays.FindAll(x => x.User.Union(list.User));

            //}
            var list = AllRents();


            return View(list);
        }


        public List<Payment> AllRents()
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
            return li;
        }

        public ActionResult AddRent(int? id)
        {
            return RedirectToAction("Create", "Payments", new { id = id });
        }
    }
}