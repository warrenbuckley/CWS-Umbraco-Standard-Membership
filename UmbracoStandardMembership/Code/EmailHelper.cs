using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using SendGridMail;
using SendGridMail.Transport;

namespace UmbracoStandardMembership.Code
{
    public class EmailHelper
    {
        public void SendResetPasswordEmail(string memberEmail, string resetGUID)
        {
            //Send a reset email to member
            // Create the email object first, then add the properties.
            var myMessage = SendGrid.GetInstance();

            // Add the message properties.
            myMessage.From = new MailAddress("support@umbjobs.com");

            //Send to the member's email address
            myMessage.AddTo(memberEmail);

            //Subject
            myMessage.Subject = "Umb Jobs - Reset Your Password";

            //Reset link
            var resetURL = HttpContext.Current.Request.Url.ToString() + "/reset-password?guid=" + resetGUID;

            //HTML Message
            myMessage.Html = string.Format(
                                "<h3>Reset Your Password</h3>" +
                                "<p>You have requested to reset your password on UmbJobs.com<br/>" +
                                "If you have not requested to reste your password, simply ignore this email and delete it</p>" +
                                "<p><a href='{0}'>Reset your password</a></p>",
                                resetURL);


            //PlainText Message
            myMessage.Text = string.Format(
                                "Reset your password" + Environment.NewLine +
                                "You have requested to reset your password on UmbJobs.com" + Environment.NewLine +
                                "If you have not requested to reste your password, simply ignore this email and delete it" +
                                Environment.NewLine + Environment.NewLine +
                                "Reset your password: {0}",
                                resetURL);

            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential("sendgridUsername", "sendgridPassword");

            // Create an SMTP transport for sending email.
            var transportSMTP = SMTP.GetInstance(credentials);

            // Send the email.
            transportSMTP.Deliver(myMessage);
        }
    }
}