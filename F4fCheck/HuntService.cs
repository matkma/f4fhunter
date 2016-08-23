using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace F4fCheck
{
    public class HuntService
    {
        private static int spamFiltersIndex;
        public static string[] Filters 
        { 
            get
            {
                return new string[] { "Islandi", "Sri Lank", "Malezj", "Singapur", "Kuala Lumpur", "Reykjavik", "Colombo" };
            }
        }

        private static string[] spamFilters;


        public static void Hunt(object sender, System.Timers.ElapsedEventArgs args)
        {
            InitSpamFilters();
            using (WebClient web = new WebClient())
            {
                try
                {
                    string rawSource = web.DownloadString(@"http://www.fly4free.pl/");
                    string source = CutSource(rawSource);
                    var results = FilterSource(source);

                    if (results.Length > 0)
                    {
                        SendEmail("kmakmateusz@gmail.com", "f4fHS - " + string.Join(", ", results), "SPRAWDZAJ SZYBKO");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException);
                }
            }
        }

        private static string CutSource(string source)
        {
            int start = source.IndexOf(@"entries");
            int end = source.IndexOf(@"/entries");
            return source.Substring(start, end - start);
        }

        private static void InitSpamFilters()
        {
            if (spamFilters == null)
            {
                spamFiltersIndex = 0;
                spamFilters = new string[20];
            }
        }

        private static string[] FilterSource(string source)
        {
            List<string> results = new List<string>();
            
            foreach(string filter in Filters)
            {
                if (source.Contains(filter))
                {
                    int startIndex = source.IndexOf(filter) - 20;
                    if (startIndex < 0) startIndex = 0;
                    int value = 40;
                    if (startIndex + value >= source.Length) value = source.Length - startIndex - 1;

                    string spamFilter = source.Substring(startIndex, value);

                    if (spamFilters.Where(c => c != null && c == spamFilter).Count() > 0)
                    {
                        continue;
                    }
                    else
                    {
                        string notification = (filter == Filters[0] || filter == Filters[1] || filter == Filters[2]) ? filter + "a" : filter;
                        results.Add(notification);
                        spamFilters[spamFiltersIndex % 20] = spamFilter;
                        spamFiltersIndex++;
                    }
                }
            }

            return results.ToArray();
        }

        private static void SendEmail(string toAddress, string subject, string body)  
        {  
            string host = ConfigurationManager.AppSettings["Host"].ToString();  
            string fromAddress = ConfigurationManager.AppSettings["FromMail"].ToString();  
            string fromPassword = ConfigurationManager.AppSettings["Password"].ToString();  

            var smtp = new SmtpClient
            {
                Host = host,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            
            smtp.Send(message);
        }  
    }
}
