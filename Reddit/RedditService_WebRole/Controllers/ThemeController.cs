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

namespace RedditService_WebRole.Controllers
{
    public class ThemeController : Controller
    {
        TokenService tokenService = new TokenService();
        ThemeDataRepository repo = new ThemeDataRepository();
        // GET: Theme
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewTheme()
        {
            return View("NewTheme");
        }

        [HttpPost]
        public ActionResult SaveTheme(string title, string content, HttpPostedFileBase image, string token)
        {
            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(content) && image != null)
            {
                try
                {
                    Topic topic = new Topic();
                    string uniqueBlobName = string.Format("image_{0}", topic.RowKey);
                    var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
                    CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobStorage.GetContainerReference("vezba");

                    container.CreateIfNotExists();

                    CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
                    blob.Properties.ContentType = image.ContentType;

                    image.InputStream.Seek(0, SeekOrigin.Begin);

                    blob.UploadFromStream(image.InputStream);

                    topic.Title = title;
                    topic.Content = content;
                    topic.PhotoUrl = blob.Uri.ToString();
                    topic.Publisher = tokenService.GetEmailFromToken(token);
                    topic.Time_published = DateTime.Now;
                    topic.Upvote = 0;
                    topic.Downvote = 0;
                    topic.Comments = new List<Comment>();

                    repo.AddTheme(topic);

                    CloudQueue queue = QueueHelper.GetQueueReference("vezba");
                    queue.AddMessage(new CloudQueueMessage(topic.RowKey), null, TimeSpan.FromMilliseconds(30));
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.Message + e.StackTrace);
                }
                
                return Json(new { success = true, message = "Topic is successfully saved" });
            }
            else
            {
                return Json(new { success = false, message = "You have not sent all the required information." });
            }
        }

        public ActionResult LoadThemes()
        {
            try
            {
                var themes = repo.RetrieveAllThemes().ToList();
                return Json(new { success = true, themes }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message + e.StackTrace);
                return Json(new { success = false, message = "Failed to load themes" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
