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
        SubscriptionRepository subRepo = new SubscriptionRepository();
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

        public ActionResult LoadThemes(string sortOrder)
        {
            try
            {
                var themes = repo.RetrieveAllThemes().ToList();

                if (sortOrder == null || sortOrder.ToLower() == "asc")
                {

                    themes = themes.OrderBy(t => t.Title).ToList();
                }
                else
                {
                    themes = themes.OrderByDescending(t => t.Title).ToList();
                }

                return Json(new { success = true, themes }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message + e.StackTrace);
                return Json(new { success = false, message = "Failed to load themes" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteTheme(string themeId, string token)
        {
            try
            {
                if (!tokenService.ValidateJwtToken(token))
                    return Json(new { success = false, message = "Token validation failed" }, JsonRequestBehavior.AllowGet);
                Debug.WriteLine("rowkey provera: " + themeId);

                var themes = repo.RetrieveAllThemes().ToList();
                foreach (var t in themes)
                {
                    if (t.RowKey.Equals(themeId))
                    {
                        repo.DeleteTheme(t);
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message + "\n" + e.StackTrace);
                return Json(new { success = false, message = "Post deleting failed" }, JsonRequestBehavior.AllowGet);
            }

            

            return Json(new { success = false, message = "Post deleting failed" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddSubscriber(string themeId, string token)
        {
            try
            {
                if (!tokenService.ValidateJwtToken(token))
                    return Json(new { success = false, message = "Token validation failed" }, JsonRequestBehavior.AllowGet);

                var email = tokenService.GetEmailFromToken(token);

                Subscription sub = new Subscription();

                sub.ThemeId = themeId;
                sub.UserId = email;

                Debug.WriteLine("Provera: " + sub.UserId + sub.ThemeId + sub.RowKey);

                var subs = subRepo.RetrieveAllSubscriptions().ToList();
                if (!subRepo.SubscriptionExists(themeId, email)) 
                    subRepo.AddSubscription(sub);
                else
                    return Json(new { success = false, message = "You are already subscribed to this post." }, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, message = "Subscription added." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message + "\n" + e.StackTrace);
                return Json(new { success = false, message = "Post subscription failed" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteSubscriber(string themeId, string token)
        {
            try
            {
                if (!tokenService.ValidateJwtToken(token))
                    return Json(new { success = false, message = "Token validation failed" }, JsonRequestBehavior.AllowGet);

                var email = tokenService.GetEmailFromToken(token);

                var subs = subRepo.RetrieveAllSubscriptions().ToList();

                Debug.WriteLine("Check email and themeId: " + email + themeId);
                foreach(var s in subs)
                {
                    if(s.ThemeId.Equals(themeId) && s.UserId.Equals(email))
                    {
                        subRepo.DeleteSubscription(s);
                        return Json(new { success = true, message = "Subscription deleted." }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { success = false, message = "You are not subscribed to this post." }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message + "\n" + e.StackTrace);
                return Json(new { success = false, message = "Post unsubscription failed" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetAllSubscriptions()
        {
            try
            {
                var subs = subRepo.RetrieveAllSubscriptions().ToList();
                Debug.WriteLine("broj elemenata u sub:" + subs.Count.ToString());
                return Json(new { success = true, subs}, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(new { success = false, message = e.StackTrace + e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
