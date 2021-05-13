/**
 * Author:    Jantzen Allphin
 * Partner:   None
 * Date:      October 3, 2020
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Jantzen Allphin - This work may not be copied for use in Academic Coursework.
 *
 * I, Jantzen Allphin, certify that I wrote this code (below) from scratch and did
 * not copy it in part or whole from another source.  Any references used
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents
 *
 *    Contains C# code for the EmailSender controller logic using SendGrid.
 */

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using URC.Area.Identity.Services;

namespace URC.Identity.Services
{
    /// <summary>
    /// Represents an EmailSender.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        /// <summary>
        /// Constructs an EmailSender.
        /// </summary>
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        /// <summary>
        /// Sends an email for register confirmation.
        /// </summary>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        /// <summary>
        /// Execute registration confirmation through email logic.
        /// </summary>
        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("u0893643@utah.edu", Options.SendGridUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}