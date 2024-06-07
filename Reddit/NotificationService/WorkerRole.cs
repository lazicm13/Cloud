using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using UserService_Data;

namespace NotificationService
{
    public class WorkerRole : RoleEntryPoint
    {
        UserDataRepository userRepo = new UserDataRepository();
        SubscriptionRepository subRepo = new SubscriptionRepository();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
    


        public override void Run()
        {
            CloudQueue queue = QueueHelper.GetQueueReference("notification");

            Trace.TraceInformation("NotificationService is running");

            while (true)
            {
                CloudQueueMessage message = queue.GetMessage();
                if (message == null)
                {
                    Trace.TraceInformation("Trenutno ne postoji poruka u redu.", "Information");
                }
                else
                {
                    Trace.TraceInformation(String.Format("Poruka glasi: {0}", message.AsString), "Information");

                    if (message.DequeueCount > 3)
                    {
                        queue.DeleteMessage(message);
                    }

                    string messageContent = message.AsString;
                    string[] parts = messageContent.Split('/');

                    List<string> subscribers;

                    if (parts.Length == 2)
                    {
                        string themeId = parts[0];   // Prvi deo je themeId
                        string content = parts[1]; // Drugi deo je content


                        subscribers = GetUserSubscriptions(themeId);

                        foreach (string userEmail in subscribers)
                        {
                            string subject = "Novi komentar na temu!";
                            string body = $"Novi komentar: {content}";

                            SendEmail("smtp.gmail.com", 587, "milanovicd891@gmail.com", "mnthazsotnutxkwr", userEmail, subject, body);
                        }

                        // Evidencija informacija o poslatim notifikacijama
                        RecordNotificationInfo(themeId, content, subscribers.Count);

                        queue.DeleteMessage(message);
                    }
                    else
                    {
                        Trace.TraceInformation("Poruka nije u ocekivanom formatu", "Information");
                    }

                    Trace.TraceInformation(String.Format("Poruka procesuirana: {0}", message.AsString), "Information");
                }

                Thread.Sleep(5000);
                Trace.TraceInformation("Working", "Information");
            }

        }
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("NotificationService has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("NotificationService is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("NotificationService has stopped");
        }

        private List<string> GetUserSubscriptions(string postId)
        {
            List<Subscription> subs = subRepo.RetrieveAllSubscriptions().ToList();
            List<string> users = new List<string>();


            foreach (var sub in subs)
            {
                if (sub.ThemeId == postId)
                {
                    users.Add(sub.UserId);
                }
            }

            return users;
        }

        public void SendEmail(string smtpServer, int smtpPort, string senderEmail, string senderPassword, string recipientEmail, string subject, string body)
        {
            try
            {
                // Kreiranje SMTP klijenta
                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);

                smtpClient.EnableSsl = true; // Ako vaš SMTP server zahteva SSL enkripciju
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);


                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(senderEmail);
                mailMessage.To.Add(recipientEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;

                smtpClient.Send(mailMessage);

                Trace.TraceInformation("Email successfully sent.");
            }
            catch (Exception ex)
            {
                // Ako slanje nije uspelo, uhvatite i tretirajte izuzetak
                Trace.TraceError($"Error sending email: {ex.Message}");
            }
        }

        private void RecordNotificationInfo(string themeId, string commentContent, int emailCount)
        {
            
            string record = $"{DateTime.UtcNow},{themeId}/{commentContent},{emailCount}";
            Trace.TraceInformation($"Notification record: {record}");
        }
    }
}
