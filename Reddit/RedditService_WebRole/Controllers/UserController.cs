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
        private const string SecurityKey = "this_is_a_long_and_secure_security_key_used_for_jwt_token"; // Kljuc samo za proveru
        TokenService tokenservice = new TokenService(SecurityKey);
        static string clientToken;
        public static string username;
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveToken(string token)
        {
            clientToken = token;
            if (token != null)
            {
                username = tokenservice.GetUsernameFromToken(clientToken);
                Debug.WriteLine("Token" + token);
                Debug.WriteLine("Username: " + username);
            }

            return View("UserPage");
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