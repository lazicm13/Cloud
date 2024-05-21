using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RedditService_WebRole.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserPage()
        {
            return View("UserPage");
        }

        public ActionResult ProfilePage()
        {
            return View("Profile");
        }

        public ActionResult ProfileLogin(string Email)
        {
            return View("Profile");
        }

        [HttpPost]
        public ActionResult Update(string Ime, string Prezime, string Adresa, string Grad, string Drzava, string Broj_telefona, string Email, string Password, HttpPostedFileBase file)
        {
            
            
            if(Ime != "")
            {

            }



           return View("UserPage");
        }
    }
}