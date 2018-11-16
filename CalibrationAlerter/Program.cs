using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PMAlerter
{
    class Receiver
    {
        public String Email { get; set; }
        public int Level { get; set; }
    }
    class Program
    {

        static void Main(string[] args)
        {


            DateTime reportingDate = DateTime.Now;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday || ((DateTime.Now.Month == 10) && (DateTime.Now.Day == 2)))
                return;

            List<Receiver> Receivers = new List<Receiver>();

            Receivers.Add(new Receiver { Email = "Keerthikumar.BH@mann-hummel.com", Level = 1 });
            Receivers.Add(new Receiver { Email = "Kiran.BS@mann-hummel.com", Level = 1 });
            Receivers.Add(new Receiver { Email = "Mustafa.Shaikh@mann-hummel.com", Level = 2 });
            Receivers.Add(new Receiver { Email = "Prashant.Gaitonde@mann-hummel.com", Level = 3 });
            Receivers.Add(new Receiver { Email = "ideonics.in@gmail.com", Level = 1 });

            List<Asset> Critical = new List<Asset>();
            List<Asset> High = new List<Asset>();
            List<Asset> Normal = new List<Asset>();

            String MailBody = String.Empty;
           

            using (var db = new AMSDB())
            {
                var assets = from a in db.Assets
                             where a.Department == "Quality"
                             select a;

                foreach (Asset a in assets)
                {
                    if ((a.AttendedOn.Value.AddDays(a.AttentionInterval.Value + 1) - DateTime.Now).Days < 0)
                    {
                        Critical.Add(a);
                    }



                    else if ((a.AttendedOn.Value.AddDays(a.AttentionInterval.Value + 1) - DateTime.Now).Days < 7)
                    {
                        High.Add(a);

                    }

                    else if ((a.AttendedOn.Value.AddDays(a.AttentionInterval.Value + 1) - DateTime.Now).Days < a.AlertInterval.Value)
                    {


                        Normal.Add(a);
                    }


                }

                if (Critical.Count > 0)
                {
                    List<Receiver> rx = new List<Receiver>();
                    MailBody = "<h1> CRITICAL PRIORITY </h1>" +
                      " <table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + ">" +
                      "<tr bgcolor='#4da6ff'><th>Machine No</th><th> Location </th><th>Due Date</th></tr>";

                    foreach (Asset a in Critical)
                    {
                        MailBody += "<tr><td>" + "  " + a.Tag + "</td>"
                           + "<td>  " + "  " + a.Location + "</td>"
                           + "<td>  " + "  " + a.AttendedOn.Value.AddDays(a.AttentionInterval.Value + 1).ToShortDateString() + "</td>"
                           + "</tr>";
                    }

                    foreach (Receiver r in Receivers)
                    {
                        if (r.Level == 3)
                        {
                            rx.Add(r);
                        }

                    }

                    MailBody += "</table>";

                    if (rx.Count > 0)
                        SendMail(MailBody, rx);
                }

                if (High.Count > 0)
                {
                    List<Receiver> rx = new List<Receiver>();
                    MailBody += "<h1> High PRIORITY </h1>" +
                      " <table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + ">" +
                      "<tr bgcolor='#4da6ff'><th>Machine No</th><th> Location </th><th>Due Date</th></tr>";

                    foreach (Asset a in High)
                    {
                        MailBody += "<tr><td>" + "  " + a.Tag + "</td>"
                           + "<td>  " + "  " + a.Location + "</td>"
                           + "<td>  " + "  " + a.AttendedOn.Value.AddDays(a.AttentionInterval.Value + 1).ToShortDateString() + "</td>"
                           + "</tr>";
                    }

                    foreach (Receiver r in Receivers)
                    {
                        if (r.Level == 2)
                        {
                            rx.Add(r);
                        }

                    }
                    MailBody += "</table>";
                    if (rx.Count > 0)
                        SendMail(MailBody, rx);
                }

                if (Normal.Count > 0)
                {

                    List<Receiver> rx = new List<Receiver>();
                    MailBody += "<h1> PRIORITY </h1>" +
                      " <table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + ">" +
                      "<tr bgcolor='#4da6ff'><th>Machine No</th><th> Location </th><th>Due Date</th></tr>";

                    foreach (Asset a in Normal)
                    {
                        MailBody += "<tr><td>" + "  " + a.Tag + "</td>"
                           + "<td>  " + "  " + a.Location + "</td>"
                           + "<td>  " + "  " + a.AttendedOn.Value.AddDays(a.AttentionInterval.Value + 1).ToShortDateString() + "</td>"
                           + "</tr>";
                    }

                    foreach (Receiver r in Receivers)
                    {
                        if (r.Level == 1)
                        {
                            rx.Add(r);
                        }

                    }

                    MailBody += "</table>";
                    if (rx.Count > 0)
                        SendMail(MailBody, rx);

                }

            }
        }
        static void SendMail(string mailBody, List<Receiver> receivers)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("mhin.andon@gmail.com");

            foreach (Receiver r in receivers)
                mail.To.Add(r.Email);
            mail.Subject = "Calibration Alert";
            mail.Body = mailBody;
            mail.IsBodyHtml = true;

            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");




            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("mhin.andon@gmail.com", "mhin@123");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

        }
    }
}


