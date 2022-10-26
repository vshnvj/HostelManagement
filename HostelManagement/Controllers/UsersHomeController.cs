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
        public ActionResult Index(int id)
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

        public ActionResult Edit(int? id)
        {
            return RedirectToAction("Edit", "Users", new { id = id });
        }

        public ActionResult Requests(int? id)
        {
           
            var response = client.GetAsync("http://localhost:64533/requestsent?id=" + id.ToString());
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
            return View((object)s);
        }

        public ActionResult SeeRequests(int? id)
        {
            HttpClient client = new HttpClient();

            var response = client.GetAsync("http://localhost:64533/api/usersapi/" + id.ToString());
            response.Wait();
            var test = response.Result;
            User u = new User();
            if (test.IsSuccessStatusCode)
            {
                var re = test.Content.ReadAsAsync<User>();
                u = re.Result;
                if (u.Status == 2)
                   ViewData["Status"]="Approved";
                else
                if (u.Status == 1)
                    ViewData["Status"] = "Applied";
                
            }
            return  View();
        }
        }
    }