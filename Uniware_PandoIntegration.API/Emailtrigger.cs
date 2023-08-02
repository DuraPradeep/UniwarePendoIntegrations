using Serilog;
using System.Net.Mail;
using System.Net;
using System.Security.Policy;
using static System.Net.WebRequestMethods;

namespace Uniware_PandoIntegration.API
{
    public class Emailtrigger
    {
        public static void SendEmailToAdmin(string apiname/*, string content*/)
        {
            try
            {
                //CONTENT

                //string content;

                //content = "Triggered Sale Order API Failed, Failed Records Avaliable in Website Please Retrigger.";
                string currentDirectory = Directory.GetCurrentDirectory();
                string folderName = "Template";
                var fullPath = Path.Combine(currentDirectory, folderName, "Retrigger.txt");
                
                string content = System.IO.File.ReadAllText(fullPath);
                content= content.Replace("{Urladdress}", "3.7.240.18:8088");
                content = content.Replace("{CompanyName}", "Duroflex Private Limited.");
                content = content.Replace("{Apiname}", apiname);               
                content = content.Replace("{SubCompanyName}", "DuroConnect");
                content = content.Replace("{TeamName}", "DuroConnect Team");
                content = content.Replace("{Address}", "Duroflex Private Limited. #30/6, NR Trident Tec Park, Hosur Main Road, HSR Layout, Sector 6,Bengaluru, Karnataka, India 560068");
                content = content.Replace("{Country}", "India");

                string emailId;
                emailId = "cw.ajay@duroflexworld.com";
                //var emailIds = "Asad.khan@duroflexworld.com,cw.pradeep@duroflexworld.com";
                var emailIds = "cw.keerti@duroflexworld.com";
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("itsupport@duroflexworld.com");
                    mail.To.Add(emailId);
                    mail.Bcc.Add(emailIds);
                    mail.Subject = "!Failed Records";
                    mail.Body = content;
                    mail.IsBodyHtml = true;
                    //  mail.Attachments.Add(new Attachment("C:\\file.zip"));

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("itsupport@duroflexworld.com", "duro@123");
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
