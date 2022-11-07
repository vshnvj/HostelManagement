using HostelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace HostelManagement.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            if (Session["username"] != null)
            {

                if (Session["username"].ToString() == "1")
                {

                    return RedirectToAction("Index", "AdminHome");
                }

                else
                {
                    return RedirectToAction("Index", "UsersHome", new { id = Session["username"].ToString() });
                }
            }
            else {
                ViewBag.Title = "Home Page";

                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(string email,string password)
        {
            string Email = email;
            string Password = password;
            HttpClient client = new HttpClient();
            var response=client.GetAsync("http://localhost:64533/GetUserByEmail?email=" + email);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var pass = test.Content.ReadAsAsync<User>();
                pass.Wait();
                User u = pass.Result;
                if (u.Password == password)
                {
                    if (u.Role_id == 1)
                    {
                        Session["username"] = u.Id;
                        return RedirectToAction("Index", "AdminHome");
                    }

                    else
                    {
                        Session["username"] = u.Id;
                        return RedirectToAction("Index", "UsersHome", new { id = Session["username"].ToString() });
                    }

                }

                else
                {
                    TempData["failed"] = "Invalid username or password.";
            } 
            }

            return View();
        }

    }
}
