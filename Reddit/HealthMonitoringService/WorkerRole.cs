using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using UserService_Data;

namespace HealthMonitoringService
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private static HttpClient _httpClient = new HttpClient();
        HealthCheckDataRepository health_repo = new HealthCheckDataRepository();
        UserDataRepository user_repo = new UserDataRepository();

        public override void Run()
        {
            Trace.WriteLine("HealthMonitoringService is running", "Information");

            while (true)
            {
                CheckHealth().Wait();

                Thread.Sleep(5000);
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("HealthMonitoringService has been started");

            return result;
        }

        private async Task CheckHealth()
        {
            var redditServiceUrl = "http://localhost:81/User/UserPage";
            //Connect();
            //bool notificationServiceUrl = proxy.IAmAlive();
            Trace.TraceInformation("Service is alive.");
            var redditServiceStatus = await CheckServiceHealth(redditServiceUrl);


            LogHealthStatus("RedditService", redditServiceStatus);
            //LogHealthStatus("NotificationService", notificationServiceUrl);

            if (!redditServiceStatus || false) //!notificationServiceUrl)
            {
                SendEmailNotification(redditServiceStatus, false);//notificationServiceUrl);
            }
        }


        private async Task<bool> CheckServiceHealth(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error checking health of {url}: {ex.Message}");
                return false;
            }
        }


        private void LogHealthStatus(string serviceName, bool isHealthy)
        {

            HealthCheck healthCheck = new HealthCheck()
            {
                ServiceName = serviceName,
                isHealthy = isHealthy ? "OK" : "NOT_OK",
                TimeOfCheck = DateTime.Now
            };

            health_repo.AddCheck(healthCheck);


        }

        private void SendEmailNotification(bool redditServiceStatus, bool notificationServiceStatus)
        {
            string subject = "Health Check Alert";
            string body = "The following services have issues:\n";
            if (!redditServiceStatus)
                body += "- RedditService\n";
            if (!notificationServiceStatus)
                body += "- NotificationService\n";

            // Implement email sending logic here, e.g., using SendGrid or SMTP client
            SendEmail("milanovicd891@gmail.com", subject, body);
        }


        public void SendEmail(string adminEmail, string subject, string body)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;

            List<User> users = user_repo.RetrieveAllUsers().ToList();

            try
            {
                // Kreiranje SMTP klijenta
                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);

                // Postavljanje potrebnih parametara za SMTP klijenta
                smtpClient.EnableSsl = true; // Ako vaš SMTP server zahteva SSL enkripciju
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(adminEmail, "mnthazsotnutxkwr");

                // Kreiranje email poruke
                foreach (User u in users)
                {
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(adminEmail);
                    mailMessage.To.Add(u.Email);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    smtpClient.Send(mailMessage);
                }

                Trace.TraceInformation("Email successfully sent.");
            }
            catch (Exception ex)
            {
                // Ako slanje nije uspelo, uhvatite i tretirajte izuzetak
                Trace.TraceError($"Error sending email: {ex.Message}");
            }
        }


        public override void OnStop()
        {
            Trace.TraceInformation("HealthMonitoringService is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("HealthMonitoringService has stopped");
        }

       
    }
}
