﻿using Serilog;
using System.Net.Mail;
using System.Net;
using System.Security.Policy;
using static System.Net.WebRequestMethods;

namespace Uniware_PandoIntegration.API
{
    public class Emailtrigger
    {
        private static IConfiguration _configuration;
        public Emailtrigger(IConfiguration configuration) { 
             _configuration = configuration;

        }
        public static void SendEmailToAdmin(string apiname,string Reason/*, string content*/)
        {
            try
            {
               var username= _configuration.GetSection("SendmailCredentilas:UserName").Value;
               var Password= _configuration.GetSection("SendmailCredentilas:Password").Value;
               var Emails= _configuration.GetSection("EmailIds:Id").Value;
                string currentDirectory = Directory.GetCurrentDirectory();
                string folderName = "Template";
                var fullPath = Path.Combine(currentDirectory, folderName, "Retrigger.txt");
                
                string content = System.IO.File.ReadAllText(fullPath);
                content= content.Replace("{Urladdress}", "https://uniwarepandointegration.azurewebsites.net/");
                content = content.Replace("{CompanyName}", "Duroflex Private Limited.");
                content = content.Replace("{Apiname}", apiname);               
                content = content.Replace("{Reason}", Reason);               
                content = content.Replace("{SubCompanyName}", "DuroConnect");
                content = content.Replace("{TeamName}", "DuroConnect Team");
                content = content.Replace("{Address}", "Duroflex Private Limited. #30/6, NR Trident Tec Park, Hosur Main Road, HSR Layout, Sector 6,Bengaluru, Karnataka, India 560068");
                content = content.Replace("{Country}", "India");

                string emailId = Emails;
                 //emailId = "Asad.khan@duroflexworld.com,vivek.acharya@duroflexworld.com,mukul.bansal@duroflexworld.com";               
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(username);
                    mail.To.Add(emailId);
                   // mail.Bcc.Add(emailIds);
                    mail.Subject = "!Failed Records";
                    mail.Body = content;
                    mail.IsBodyHtml = true;
                    //  mail.Attachments.Add(new Attachment("C:\\file.zip"));

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential(username, Password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception Ex)
            {
                //log.Error($"Excetion at :", Ex);
                throw Ex;
            }
        }
    }
}
