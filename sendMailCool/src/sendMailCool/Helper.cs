using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.IO;

namespace sendMailCool
{
    public static class Helper
    {
        public static int SendEmail(string from, string fromTitle, string to, string toTitle, string subject, string bodyContent, string path, string yourEmail, string yourPass) {
            try
            {                             
                var body = new TextPart("plain")
                {
                    Text = bodyContent
                };

                // Smtp Server
                string SmtpServer = "smtp.gmail.com";
                // Smtp Port Number
                int SmtpPortNumber = 587;
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(fromTitle, from));
                mimeMessage.To.Add(new MailboxAddress(toTitle, to));
                mimeMessage.Subject = subject;                           
                

                // now create the multipart/mixed container to hold the message text and the
                // image attachment
                var multipart = new Multipart("mixed");
                multipart.Add(body);

                // add attackment
                // create an image attachment for the file located at path
                if (path != "")
                {
                    var attachment = new MimePart()
                    {
                        ContentObject = new ContentObject(System.IO.File.OpenRead(path), ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(path)
                    };
                    multipart.Add(attachment);
                }              

                // now set the multipart/mixed as the message body
                mimeMessage.Body = multipart;

                using (var client = new SmtpClient())
                {
                    client.Connect(SmtpServer, SmtpPortNumber, false);
                    //client.Authenticate("your email", "your pass");

                    client.Send(mimeMessage);
                    client.Disconnect(true);

                }
                return 1;
            }
            catch
            {
                return 0;
            }            

        }



    }
}
