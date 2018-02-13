using System;
using System.Threading.Tasks;
using System.Web;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CWSUmbracoStandardMembership.Code
{
    public class EmailHelper
    {
        private const string SendGridApiKey = "someapikey";
        private const string EmailFromAddress = "you@yoursite.com";

        /// <summary>
        /// Send a reset email to member
        /// </summary>
        /// <param name="memberEmail"></param>
        /// <param name="resetGUID"></param>
        /// <returns></returns>
        public async Task SendResetPasswordEmail(string memberEmail, string resetGUID)
        {
            // Create the email client first, then add the properties.
            var client = new SendGridClient(SendGridApiKey);

            // Add the message properties.
            var from = new EmailAddress(EmailFromAddress);
            var to = new EmailAddress(memberEmail);

            //Subject
            string subject = "Example.com - Reset Your Password";

            //Reset link
            string baseURL = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
            var resetURL = baseURL + "/reset-password?resetGUID=" + resetGUID;

            //HTML Message
            string htmlContent = string.Format(
                                "<h3>Reset Your Password</h3>" +
                                "<p>You have requested to reset your password<br/>" +
                                "If you have not requested to reste your password, simply ignore this email and delete it</p>" +
                                "<p><a href='{0}'>Reset your password</a></p>",
                                resetURL);

            //PlainText Message
            string plainTextContent = string.Format(
                                "Reset your password" + Environment.NewLine +
                                "You have requested to reset your password" + Environment.NewLine +
                                "If you have not requested to reste your password, simply ignore this email and delete it" +
                                Environment.NewLine + Environment.NewLine +
                                "Reset your password: {0}",
                                resetURL);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        /// <summary>
        /// Send a reset email to member
        /// </summary>
        /// <param name="memberEmail"></param>
        /// <param name="verifyGUID"></param>
        /// <returns></returns>
        public async Task SendVerifyEmail(string memberEmail, string verifyGUID)
        {
            // Create the email client first, then add the properties.
            var client = new SendGridClient(SendGridApiKey);

            // Add the message properties.
            var from = new EmailAddress(EmailFromAddress);
            var to = new EmailAddress(memberEmail);

            //Subject
            string subject = "Example.com - Verify Your Email";

            //Verify link
            string baseURL = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
            var verifyURL = baseURL + "/verify-email?verifyGUID=" + verifyGUID;

            //PlainText Message
            string plainTextContent = string.Format(
                                "Verify Your Email" + Environment.NewLine +
                                "Click here to verify your email address and active your account today" + Environment.NewLine +
                                Environment.NewLine + Environment.NewLine +
                                "{0}",
                                verifyURL);

            //HTML Message
            string htmlContent = string.Format(
                                "<h3>Verify Your Email</h3>" +
                                "<p>Click here to verify your email address and active your account today</p>" +
                                "<p><a href='{0}'>Verify your email & active your account</a></p>",
                                verifyURL);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}