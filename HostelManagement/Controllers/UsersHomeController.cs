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
                if (GetRoom() != null)
                {
                    ViewBag.Room = GetRoom();
                }
            
            List<complaint> r_list = new List<complaint>();
            string uri = "http://localhost:64533/GetComplaintsList?userid=" + Session["id"];

            var response1 = client.GetAsync(uri);
            response1.Wait();
            var test1 = response1.Result;
            if (test1.IsSuccessStatusCode)
            {
                var employees = test1.Content.ReadAsAsync<List<complaint>>();
               r_list = employees.Result;
                ViewBag.FeedbackList = r_list;
            }



            return View(r_list);
     }
        public ActionResult Edit()
        {
            return RedirectToAction("Edit", "Users", new { id =Session["id"]});
        }

        public ActionResult Feedback()
        {
             return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Feedback(string sub, string details)
        {
            complaint complaint = new complaint();
            complaint.sub = sub;
            complaint.details = details;
            complaint.user = int.Parse(Session["id"].ToString());
            HttpClient client = new HttpClient();
            var response = client.PostAsJsonAsync<complaint>("http://localhost:64533/api/complaintapi/", complaint);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                
                return RedirectToAction("Index", "UsersHome", new { id =Session["id"]});

            }
            return View();
        }

        public ActionResult TrackRent()
        {

            var response = client.GetAsync("http://localhost:64533/api/paymentsapi");
            List<Payment> li = new List<Payment>();
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var pays = test.Content.ReadAsAsync<List<Payment>>();
                pays.Wait();
                li = pays.Result;
                li = li.FindAll(x => x.User_id == int.Parse(Session["id"].ToString()));
            }
            ViewBag.List = li;


           
            return View();

        }

        public ActionResult Requests()
        {


            var response = client.GetAsync("http://localhost:64533/requestsent?id=" + Session["id"].ToString());
            response.Wait();
            User user1 = new User();
            var test = response.Result;
            string s = "";
            if (test.IsSuccessStatusCode)
            {
                var re = test.Content.ReadAsAsync<User>();
                user1 = re.Result;
                ViewData["success"] = "Request sent successfully";
                s = "Request sent successfully";
            }

            //return Content("<script>alert('Request sent successfully')</script>");
            return View(user1);

        }

        public ActionResult SeeRequests()
        {
            HttpClient client = new HttpClient();

            var response = client.GetAsync("http://localhost:64533/api/usersapi/" + Session["id"].ToString());
            response.Wait();
            var test = response.Result;
            User u = new User();
            if (test.IsSuccessStatusCode)
            {
                var re = test.Content.ReadAsAsync<User>();
                u = re.Result;
                if (u.Status == 2)
                {
                    ViewData["Status"] = "Approved";
                }
                if (u.Status == 3)
                {
                    ViewData["Status"] = "Deallocated";
                }


                if (u.Status == 1)
                {
                    ViewData["Status"] = "Applied";
                }
            }



            HttpClient client1 = new HttpClient();


            string uri1 = "http://localhost:64533/api/AllocationsApi/" + u.Id;
            var response1 = client.GetAsync(uri1);
            response.Wait();
            var test1 = response1.Result;



            if (test.IsSuccessStatusCode)
            {

                var employees1 = test1.Content.ReadAsAsync<Allocation>();
                employees1.Wait();
                if (employees1.Result != null)
                {
                    Allocation al = (Allocation)employees1.Result;
                    if (al != null)
                        ViewData["room"] = al.Room_no;
                }





            }




            return View(u);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }
        public Room GetRoom()
        {
            HttpClient client = new HttpClient();

            var response = client.GetAsync("http://localhost:64533/api/allocationsapi/" + Session["id"].ToString());
            response.Wait();
            var test = response.Result;
            Allocation all = new Allocation();
            User u = new User();
            if (test.IsSuccessStatusCode)
            {
                var re = test.Content.ReadAsAsync<Allocation>();
                return re.Result.Room;
            }

            return null;
        }

    }

    }