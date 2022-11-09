using HostelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace HostelManagement.Controllers
{
    public class UsersHomeController : Controller
    {
        // GET: UsersHome
        User user;
        HttpClient client = new HttpClient();
        public ActionResult Index()
        {
            var response = client.GetAsync("http://localhost:64533/api/usersapi/" + Session["username"].ToString());
            User li = new User();
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var r = test.Content.ReadAsAsync<User>();
                //r.Wait();
                user= r.Result;
            }
            return View(user);
           
        }

        public ActionResult Edit()
        {
            return RedirectToAction("Edit", "Users", new { id = Session["username"] } );
        }

        public ActionResult Feedback()
        {
            var response = client.GetAsync("http://localhost:64533/api/usersapi/" + Session["username"].ToString());

            user = new User();
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var r = test.Content.ReadAsAsync<User>();
                //r.Wait();
                user = r.Result;
            }

            return View(user);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Feedback( string sub, string details)
        {
            complaint complaint = new complaint();
            complaint.sub = sub;
            complaint.details = details;
            HttpClient client = new HttpClient();
            var response = client.PostAsJsonAsync<complaint>("http://localhost:64533/api/complaintapi/", complaint);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                //return RedirectToAction("Index");


                return RedirectToAction("Index", "UsersHome", new { id = Session["username"] });

            }
            return View();
        }

        public ActionResult TrackRent()
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
                li = li.FindAll(x=>x.User_id == int.Parse(Session["username"].ToString()));
            }
            ViewBag.List = li;


            var response1 = client.GetAsync("http://localhost:64533/api/usersapi/" + Session["username"].ToString());
            User l = new User();
            response.Wait();
            var test1 = response1.Result;
            if (test1.IsSuccessStatusCode)
            {
                var r = test1.Content.ReadAsAsync<User>();
                //r.Wait();
                l = r.Result;
            }
            return View(l);

    }

    public ActionResult Requests()
        {
           
           
            var response = client.GetAsync("http://localhost:64533/requestsent?id=" + Session["username"].ToString());
            response.Wait();
            User user1=new User();
            var test = response.Result;
            string s = "";
            if (test.IsSuccessStatusCode)
            {
                var re = test.Content.ReadAsAsync<User>();
                 user1 = re.Result;
                ViewData["success"] = "Request sent successfully";
                  s= "Request sent successfully";
            }

            //return Content("<script>alert('Request sent successfully')</script>");
            return View(user1);
            
        }

        public ActionResult SeeRequests()
        {
            HttpClient client = new HttpClient();

            var response = client.GetAsync("http://localhost:64533/api/usersapi/" + Session["username"].ToString());
            response.Wait();
            var test = response.Result;
            User u = new User();
            if (test.IsSuccessStatusCode)
            {
                var re = test.Content.ReadAsAsync<User>();
                u = re.Result;
                if (u.Status == 2)
                   ViewData["Status"]="Approved";
                if (u.Status == 3)
                    ViewData["Status"] = "Deallocated";
                else
                if (u.Status == 1)
                    ViewData["Status"] = "Applied";
                
            }
            return  View(u);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }

    }

    }