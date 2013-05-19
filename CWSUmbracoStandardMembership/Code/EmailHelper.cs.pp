using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using SendGridMail;
using SendGridMail.Transport;

namespace $rootnamespace$.Code
{
    public class EmailHelper
    {
        private const string SendGridUsername   = "sendGridUsername";
        private const string SendGridPassword   = "sendGridPassword";
        private const string EmailFromAddress   = "you@yoursite.com";

        public void SendResetPasswordEmail(string memberEmail, string resetGUID)
        {
            //Send a reset email to member
            // Create the email object first, then add the properties.
            var myMessage = SendGrid.GetInstance();

            // Add the message properties.
            myMessage.From = new MailAddress(EmailFromAddress);

            //Send to the member's email address
            myMessage.AddTo(memberEmail);

            //Subject
            myMessage.Subject = "Umb Jobs - Reset Your Password";

            //Reset link
            string baseURL = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
            var resetURL = baseURL + "/reset-password?resetGUID=" + resetGUID;

            //HTML Message
            myMessage.Html = string.Format(
                                "<h3>Reset Your Password</h3>" +
                                "<p>You have requested to reset your password<br/>" +
                                "If you have not requested to reste your password, simply ignore this email and delete it</p>" +
                                "<p><a href='{0}'>Reset your password</a></p>",
                                resetURL);


            //PlainText Message
            myMessage.Text = string.Format(
                                "Reset your password" + Environment.NewLine +
                                "You have requested to reset your password" + Environment.NewLine +
                                "If you have not requested to reste your password, simply ignore this email and delete it" +
                                Environment.NewLine + Environment.NewLine +
                                "Reset your password: {0}",
                                resetURL);

            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential(SendGridUsername, SendGridPassword);

            // Create an SMTP transport for sending email.
            var transportSMTP = SMTP.GetInstance(credentials);

            // Send the email.
            transportSMTP.Deliver(myMessage);
        }

        public void SendVerifyEmail(string memberEmail, string verifyGUID)
        {
            //Send a reset email to member
            // Create the email object first, then add the properties.
            var myMessage = SendGrid.GetInstance();

            // Add the message properties.
            myMessage.From = new MailAddress(EmailFromAddress);

            //Send to the member's email address
            myMessage.AddTo(memberEmail);

            //Subject
            myMessage.Subject = "Umb Jobs - Verify Your Email";

            //Verify link
            string baseURL = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
            var verifyURL = baseURL + "/verify-email?verifyGUID=" + verifyGUID;

            //HTML Message
            myMessage.Html = string.Format(
                                "<h3>Verify Your Email</h3>" +
                                "<p>Click here to verify your email address and active your account today</p>" +
                                "<p><a href='{0}'>Verify your email & active your account</a></p>",
                                verifyURL);


            //PlainText Message
            myMessage.Text = string.Format(
                                "Verify Your Email" + Environment.NewLine +
                                "Click here to verify your email address and active your account today" + Environment.NewLine +
                                Environment.NewLine + Environment.NewLine +
                                "{0}",
                                verifyURL);

            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential(SendGridUsername, SendGridPassword);

            // Create an SMTP transport for sending email.
            var transportSMTP = SMTP.GetInstance(credentials);

            // Send the email.
            transportSMTP.Deliver(myMessage);
        }
    }
}
