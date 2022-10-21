using HostelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HostelManagement.Controllers
{
    public class UsersHomeController : Controller
    {
        // GET: UsersHome
        public ActionResult Index(User u)
        {
            return View(u);
        }
    }
}