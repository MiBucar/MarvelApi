using MailKit;
using MarvelApi_Mvc.Models.DTOs.UserDTOs;
using MarvelApi_Mvc.Services.IServices;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Mail;

namespace MarvelApi_Mvc.Services.Implementation
{
    public class MailReceiverService : IMailReceiverService
    {
        private const string EmailAdress = "mislavbucar@gmail.com";

        public async Task SendEmailAsync(MailDTO mailDto)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(mailDto.Name, mailDto.Email));
            email.To.Add(MailboxAddress.Parse(EmailAdress));
            email.Subject = mailDto.Subject;
            email.Body = new TextPart("plain")
            {
                Text = $"{mailDto.Email} \n {mailDto.Comment}"
            };

            await ConnectAndSendEmailAsync(email);
        }

        private async Task ConnectAndSendEmailAsync(MimeMessage email)
        {
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    smtp.Authenticate(EmailAdress, "kotf vwfj uasv daoy");
                    await smtp.SendAsync(email);
                    smtp.Disconnect(true);
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}
