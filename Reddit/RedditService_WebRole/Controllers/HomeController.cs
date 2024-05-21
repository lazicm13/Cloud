using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserService_Data;
using static UserService_Data.TokenService;

namespace RedditService_WebRole.Controllers
{
    public class HomeController : Controller
    {
        TokenService tokenservice = new TokenService();
        static string clientToken;
        static string username;

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
            else
                ViewBag.Username = "Not loaded";
            return Json(new { success = true });
        }

        public ActionResult Index()
        {
            ViewBag.Username = username;
            Debug.WriteLine("ViewBag username:" + username);
            return View();
        }

        
    }
}