﻿using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }
        public AuthSenderOptions Options { get; } //set only via Secret Manager
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }
        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGrid.SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                //THIS MUST MATCH A VERIFIED EMAIL ACCOUNT IN SENDGRID
                From = new EmailAddress("ryandownard@mail.weber.edu", Options.SendGridUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(true, true);
            return client.SendEmailAsync(msg);
        }
    }

}
