using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        public ActionResult Update(User user, HttpPostedFileBase file, string OldPassword)
        {    
            User old_user = repo.RetrieveAllUsers().FirstOrDefault(s => s.Password == OldPassword);
            if (file != null && file.ContentLength > 0)
            {
              
                string uploadsFolderPath = Server.MapPath("~/Uploads"); 
                string fileName = Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolderPath, fileName);

      
                file.SaveAs(filePath);

               
                old_user.PhotoUrl = Url.Content("~/Uploads/" + fileName); 
            }
            if (user.Password!=null)
            {
                old_user.Password = user.Password;
            }
            if(user.Ime!= null && user.Prezime!=null && user.Email!=null && user.Drzava!=null && user.Broj_telefona!=null && user.Adresa!=null && user.Grad!=null )
            {
                old_user.Ime = user.Ime;
                old_user.Prezime = user.Prezime;
                old_user.Grad = user.Grad;
                old_user.Drzava = user.Drzava;
                old_user.Adresa = user.Adresa;
                old_user.Email = user.Email;
                old_user.Broj_telefona = user.Broj_telefona;
               
            }
            repo.UpdateUser(old_user);


            return View("UserProfile");
        }
        [HttpPost]
        public ActionResult RedirectToEdit(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { error = "Token not provided" });
            }
             string username = tokenservice.GetUsernameFromToken(token);
            User user = repo.RetrieveAllUsers().FirstOrDefault(s => s.Ime == username);
            return Json(new { success = true, email = user.Email });
        }


        public ActionResult UpdateUser(string email)
        {
            User user = repo.RetrieveAllUsers().FirstOrDefault(s => s.Email == email);
            return View(user);
        }
    }
}