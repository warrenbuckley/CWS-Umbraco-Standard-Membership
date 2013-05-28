﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web.Mvc;
using CWSUmbracoStandardMembership.Models;
using CWSUmbracoStandardMembership.Code;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.member;

namespace CWSUmbracoStandardMembership.Controllers.SurfaceControllers
{
    public class AuthSurfaceController : SurfaceController
    {

        /// <summary>
        /// Renders the Login view
        /// @Html.Action("RenderLogin","AuthSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderLogin()
        {
            LoginViewModel loginModel = new LoginViewModel();


            if (string.IsNullOrEmpty(HttpContext.Request["ReturnUrl"]))
            {
                //If returnURL is empty then set it to /
                loginModel.ReturnUrl = "/";
            }
            else
            {
                //Lets use the return URL in the querystring or form post
                loginModel.ReturnUrl = HttpContext.Request["ReturnUrl"];
            }

            return PartialView("Login", loginModel);
        }


        /// <summary>
        /// Handles the login form when user posts the form/attempts to login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleLogin(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //return RedirectToCurrentUmbracoPage();
                return PartialView("Login", model);
            }

            //Member already logged in - redirect to home
            if (Member.IsLoggedOn())
            {
                return Redirect("/");
            }

            //Lets TRY to log the user in
            try
            {
                //Try and login the user...
                if (Membership.ValidateUser(model.EmailAddress, model.Password))
                {
                    //Valid credentials

                    //Get the member from their email address
                    var checkMember = Member.GetMemberFromEmail(model.EmailAddress);

                    //Check the member exists
                    if (checkMember != null)
                    {
                        //Let's check they have verified their email address
                        if (Convert.ToBoolean(checkMember.getProperty("hasVerifiedEmail").Value))
                        {
                            //Update number of logins counter
                            int noLogins = 0;
                            if (int.TryParse(checkMember.getProperty("numberOfLogins").Value.ToString(), out noLogins))
                            {
                                //Managed to parse it to a number
                                //Don't need to do anything as we have default value of 0
                            }

                            //Update the counter
                            checkMember.getProperty("numberOfLogins").Value = noLogins + 1;

                            //Update label with last login date to now
                            checkMember.getProperty("lastLoggedIn").Value = DateTime.Now.ToString("dd/MM/yyyy @ HH:mm:ss");

                            //Update label with last logged in IP address & Host Name
                            string hostName         = Dns.GetHostName();
                            string clientIPAddress  = Dns.GetHostAddresses(hostName).GetValue(0).ToString();

                            checkMember.getProperty("hostNameOfLastLogin").Value    = hostName;
                            checkMember.getProperty("iPOfLastLogin").Value          = clientIPAddress;

                            //Save the details
                            checkMember.Save();

                            //If they have verified then lets log them in
                            //Set Auth cookie
                            FormsAuthentication.SetAuthCookie(model.EmailAddress, true);

                            //Once logged in - redirect them back to the return URL
                            if (!string.IsNullOrEmpty(model.ReturnUrl))
                            {
                                return new RedirectResult(model.ReturnUrl);
                            }
                            else
                            {
                                return new RedirectResult("/");
                            }
                        }
                        else
                        {
                            //User has not verified their email yet
                            ModelState.AddModelError("LoginForm.", "Email account has not been verified");
                            return PartialView("Login", model);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("LoginForm.", "Invalid details");
                    return PartialView("Login", model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("LoginForm.", "Error: " + ex.ToString());
                return PartialView("Login", model);
            }

            return PartialView("Login", model);
        }

        //Used with an ActionLink
        //@Html.ActionLink("Logout", "Logout", "AuthSurface")
        public ActionResult Logout()
        {
            //Member already logged in, lets log them out and redirect them home
            if (Member.IsLoggedOn())
            {
                //Log member out
                FormsAuthentication.SignOut();

                //Redirect home
                return Redirect("/");
            }
            else
            {
                //Redirect home
                return Redirect("/");
            }
        }


        /// <summary>
        /// Renders the Forgotten Password view
        /// @Html.Action("RenderForgottenPassword","AuthSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderForgottenPassword()
        {
            return PartialView("ForgottenPassword", new ForgottenPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleForgottenPassword(ForgottenPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("ForgottenPassword", model);
            }

            //Find the member with the email address
            var findMember = Member.GetMemberFromEmail(model.EmailAddress);

            if (findMember != null)
            {
                //We found the member with that email

                //Set expiry date to 
                DateTime expiryTime = DateTime.Now.AddMinutes(15);

                //Lets update resetGUID property
                findMember.getProperty("resetGUID").Value = expiryTime.ToString("ddMMyyyyHHmmssFFFF");

                //Save the member with the up[dated property value
                findMember.Save();

                //Send user an email to reset password with GUID in it
                EmailHelper email = new EmailHelper();
                email.SendResetPasswordEmail(findMember.Email, expiryTime.ToString("ddMMyyyyHHmmssFFFF"));
            }
            else
            {
                ModelState.AddModelError("ForgottenPasswordForm.", "No member found");
                return PartialView("ForgottenPassword", model);
            }

            return PartialView("ForgottenPassword", model);
        }


        /// <summary>
        /// Renders the Reset Password View
        /// @Html.Action("RenderResetPassword","AuthSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderResetPassword()
        {
            return PartialView("ResetPassword", new ResetPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("ResetPassword", model);
            }

            //Get member from email
            var resetMember = Member.GetMemberFromEmail(model.EmailAddress);

            //Ensure we have that member
            if (resetMember != null)
            {
                //Get the querystring GUID
                var resetQS = Request.QueryString["resetGUID"];

                //Ensure we have a vlaue in QS
                if (!string.IsNullOrEmpty(resetQS))
                {
                    //See if the QS matches the value on the member property
                    if (resetMember.getProperty("resetGUID").Value.ToString() == resetQS)
                    {

                        //Got a match, now check to see if the 15min window hasnt expired
                        DateTime expiryTime = DateTime.ParseExact(resetQS, "ddMMyyyyHHmmssFFFF", null);

                        //Check the current time is less than the expiry time
                        DateTime currentTime = DateTime.Now;

                        //Check if date has NOT expired (been and gone)
                        if (currentTime.CompareTo(expiryTime) < 0)
                        {

                            //Got a match, we can allow user to update password
                            resetMember.Password = model.Password;

                            //Remove the resetGUID value
                            resetMember.getProperty("resetGUID").Value = string.Empty;

                            //Save the member
                            resetMember.Save();

                            return Redirect("/login");
                        }
                        else
                        {
                            //ERROR: Reset GUID has expired
                            ModelState.AddModelError("ResetPasswordForm.", "Reset GUID has expired");
                            return PartialView("ResetPassword", model);
                        }
                    }
                    else
                    {
                        //ERROR: QS does not match what is stored on member property
                        //Invalid GUID
                        ModelState.AddModelError("ResetPasswordForm.", "Invalid GUID");
                        return PartialView("ResetPassword", model);
                    }
                }
                else
                {
                    //ERROR: No QS present
                    //Invalid GUID
                    ModelState.AddModelError("ResetPasswordForm.", "Invalid GUID");
                    return PartialView("ResetPassword", model);
                }
            }

            return PartialView("ResetPassword", model);
        }


        /// <summary>
        /// Renders the Register View
        /// @Html.Action("RenderRegister","AuthSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderRegister()
        {
            return PartialView("Register", new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleRegister(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("Register", model);
            }

            //Member Type
            MemberType umbJobMemberType = MemberType.GetByAlias("Member");

            //Umbraco Admin User (The Umbraco back office username who will create the member via the API)
            User umbUser = new User("Admin");

            //Model valid let's create the member
            try
            {
                Member createMember = Member.MakeNew(model.Name, model.EmailAddress, model.EmailAddress, umbJobMemberType, umbUser);

                //Set password on the newly created member
                createMember.Password = model.Password;

                //Set the verified email to false
                createMember.getProperty("hasVerifiedEmail").Value = false;

                //Set the profile URL to be the member ID, so they have a unqie profile ID, until they go to set it
                createMember.getProperty("profileURL").Value = createMember.Id;

                //Save the changes
                createMember.Save();
            }
            catch (Exception ex)
            {
                //EG: Duplicate email address - already exists
                throw;
            }


            //Create temporary GUID
            var tempGUID = Guid.NewGuid();

            //Fetch our new member we created by their email
            var updateMember = Member.GetMemberFromEmail(model.EmailAddress);

            //Just to be sure...
            if (updateMember != null)
            {
                //Set the verification email GUID value on the member
                updateMember.getProperty("emailVerifyGUID").Value = tempGUID.ToString();

                //Set the Joined Date label on the member
                updateMember.getProperty("joinedDate").Value = DateTime.Now.ToString("dd/MM/yyyy @ HH:mm:ss");
                
                //Save changes
                updateMember.Save();
            }

            //Send out verification email, with GUID in it
            EmailHelper email = new EmailHelper();
            email.SendVerifyEmail(model.EmailAddress, tempGUID.ToString());

            //Return the view...
            return PartialView("Register", new RegisterViewModel());

        }

        /// <summary>
        /// Renders the Verify Email
        /// @Html.Action("RenderVerifyEmail","AuthSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderVerifyEmail(string verifyGUID)
        {
            //Auto binds and gets guid from the querystring
            Member findMember = Member.GetAllAsList().SingleOrDefault(x => x.getProperty("emailVerifyGUID").Value.ToString() == verifyGUID);

            //Ensure we find a member with the verifyGUID
            if (findMember != null)
            {
                //We got the member, so let's update the verify email checkbox
                findMember.getProperty("hasVerifiedEmail").Value = true;

                //Save the member
                findMember.Save();
            }
            else
            {
                //Couldn't find them - most likely invalid GUID
                return Redirect("/");
            }

            //Just in case...
            return Redirect("/");
        }


        //REMOTE Validation
        /// <summary>
        /// Used with jQuery Validate to check when user registers that email address not already used
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public JsonResult CheckEmailIsUsed(string emailAddress)
        {
            //Try and get member by email typed in
            var checkEmail = Member.GetMemberFromEmail(emailAddress);

            if (checkEmail != null)
            {
                return Json(String.Format("The email address '{0}' is already in use.", emailAddress), JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
