using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserService_Data;

namespace RedditService_WebRole.Controllers
{
    public class UserController : Controller
    {
        UserDataRepository repo = new UserDataRepository();
        private const string SecurityKey = "this_is_a_long_and_secure_security_key_used_for_jwt_token";
        TokenService tokenservice = new TokenService(SecurityKey);
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserPage()
        {
            var user = TempData["User"] as User;
            ViewBag.User = user;
            // Pass user information to the view
            TempData.Keep("User");
            if (user != null)
            {
                ViewBag.IsAuthenticated = true;
            }
            else
            {
                ViewBag.IsAuthenticated = false;
            }
            return View();
        }

        public ActionResult ProfileTO()
        {
            return View("UserProfile");
        }

        [HttpPost]
        public ActionResult ProfilePage(string token)
        {
            Debug.WriteLine("Ulazak u akciju Profile Page");
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { error = "Token not provided" });
                }

                //if (!tokenservice.ValidateToken(token))
                //{
                //    return Json(new { error = "Invalid token" });
                //}

                string username = tokenservice.GetUsernameFromToken(token);
                User currentUser = repo.RetrieveAllUsers().FirstOrDefault(s => s.Ime == username);

                Debug.WriteLine(currentUser.Ime + " " + currentUser.Prezime);
                if (currentUser == null)
                {
                    Debug.WriteLine("Korisnik nije pronadjen");
                    return Json(new { error = "User not found" });
                }

                return Json(currentUser);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace + e.Message);
                return Json(new { error = e.Message });
            }
        }
        public ActionResult UserProfile()
        {
            return View();
        }


        public ActionResult ProfileLogin(string Email)
        {
            return View("UserProfile");
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