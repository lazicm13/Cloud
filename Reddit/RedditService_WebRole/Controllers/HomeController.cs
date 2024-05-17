using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserService_Data;

namespace RedditService_WebRole.Controllers
{
    public class HomeController : Controller
    {
        UserDataRepository repo = new UserDataRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public ActionResult Create(string Ime, string Prezime, string Adresa, string Grad, string Drzava, string Broj_telefona, string Email, string Password, HttpPostedFileBase file)
        {
            try
            {
                string RowKey = Email;
                if (repo.Exists(RowKey))
                {
                    return View("Error");
                }

                string uniqueBlobName = string.Format("image_{0}", RowKey);
                var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
                CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobStorage.GetContainerReference("vezba");

                container.CreateIfNotExists();

                CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
                blob.Properties.ContentType = file.ContentType;

                file.InputStream.Seek(0, SeekOrigin.Begin);

                blob.UploadFromStream(file.InputStream);

                User entry = new User(RowKey)
                {
                    Ime = Ime,
                    Prezime = Prezime,
                    Adresa = Adresa,
                    Grad = Grad,
                    Drzava = Drzava,
                    Broj_telefona = Broj_telefona,
                    Email = Email,
                    Password = Password,
                    PhotoUrl = blob.Uri.ToString()
                };
                repo.AddUser(entry);

                CloudQueue queue = QueueHelper.GetQueueReference("vezba");
                queue.AddMessage(new CloudQueueMessage(RowKey), null, TimeSpan.FromMilliseconds(30));

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging purposes
                System.Diagnostics.Debug.WriteLine("An error occurred: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    System.Diagnostics.Debug.WriteLine("Inner Stack Trace: " + ex.InnerException.StackTrace);
                }

                return View("Register");
            }
        }





    }
}