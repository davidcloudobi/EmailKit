using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using MimeKit;
using Org.BouncyCastle.Asn1.Ocsp;

namespace EmailService
{
  public  class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public bool SendEmail(MessageDTO message)
        {
         

            var newMessage = new Message(message.To, message.Subject, message.Content);

            var emailMessage = CreateEmailMessage(newMessage);

            if (emailMessage != null)
            {
                Send(emailMessage);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SendEmailAsync(MessageDTO message)
        {
            //var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();

            // var message = new Message(new string[] { "codemazetest@mailinator.com" }, "Test mail with Attachments", "This is the content from our mail with attachments.", files);
            //  await _emailSender.SendEmailAsync(message);


            var newMessage = new Message(message.To, message.Subject, message.Content);
           // var newMessage = new Message(message.To, message.Subject, message.Content, file);

            var emailMessage = CreateEmailMessage(newMessage);

            if (emailMessage != null)
            {
                
                await SendAsync(emailMessage);
                return true;
            }
            else
            {
                return false;
            }

           
        }


        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
           // emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) };

            return emailMessage;


            //var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) };

            //if (message.Attachments != null && message.Attachments.Any())
            //{
            //    byte[] fileBytes;
            //    foreach (var attachment in message.Attachments)
            //    {
            //        using (var ms = new MemoryStream())
            //        {
            //            attachment.CopyTo(ms);
            //            fileBytes = ms.ToArray();
            //        }

            //        bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
            //    }
            //}

            //emailMessage.Body = bodyBuilder.ToMessageBody();

           // return emailMessage;




        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }


        }


        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

    }




}

