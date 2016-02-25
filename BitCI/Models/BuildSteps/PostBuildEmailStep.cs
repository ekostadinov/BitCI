using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace BitCI.Models.BuildSteps
{
    public class PostBuildEmailStep : IBuildStep
    {
        public int Id { get; set; }
        public int BuildId { get; set; }
        public Build Build { get; set; }
        public StepStatus Status { get; set; }
        public string Value { get; set; }

        public void Execute()
        {
            //update log
            object locker = new Object();
            lock (locker)
            {
                using (FileStream file = new FileStream(Build.Log, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
                {
                    writer.WriteLine();
                    writer.WriteLine("Step 4:");
                    writer.WriteLine("Send email to - ");
                }
            }

            try
            {
                SmtpClient mailServer = new SmtpClient("smtp.gmail.com", 587);
                mailServer.EnableSsl = true;
                mailServer.Credentials = new System.Net.NetworkCredential("evgenikostadinov@gmail.com", "dummy-pass");

                string from = "evgenikostadinov@gmail.com";
                string to = Value;
                MailMessage msg = new MailMessage(from, to)
                {
                    Subject = "BitCI test report",
                    Body = "BitCI notifys You for a triggered build."
                };
                msg.Attachments.Add(new Attachment(Build.Log));
                mailServer.Send(msg);
            }
            catch (Exception ex)
            {
                lock (locker)
                {
                    using (FileStream file = new FileStream(Build.Log, FileMode.Append, FileAccess.Write, FileShare.Read))
                    using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
                    {
                        writer.WriteLine("Unable to send email to " + Value + ". Error : " + ex.Message);
                    }
                }
            }
        }
    }
}