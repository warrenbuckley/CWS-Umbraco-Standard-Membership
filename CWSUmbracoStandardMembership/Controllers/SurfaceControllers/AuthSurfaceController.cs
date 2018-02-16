using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web.Mvc;
using CWSUmbracoStandardMembership.Models;
using CWSUmbracoStandardMembership.Code;
using System.Threading.Tasks;

namespace CWSUmbracoStandardMembership.Controllers.SurfaceControllers
{
    public class AuthSurfaceController : SurfaceController
    {
        #region Sign-in/out

        /// <summary>
        /// Renders the Login view
        /// @Html.Action("RenderLogin","AuthSurface");
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
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
            if (!ModelState.IsValid) return PartialView("Login", model);

            //Member already logged in - redirect to home
            if (User?.Identity.IsAuthenticated == true) return Redirect("/");

            //Lets TRY to log the user in
            try
            {
                //Try and login the user... (IS THIS NESSECARY?, model should be validated by .net)
                if (Membership.ValidateUser(model.EmailAddress, model.Password))
                {
                    var ms = Services.MemberService;

                    //Get the member from their email address
                    var checkMember = ms.GetByEmail(model.EmailAddress);

                    //Check the member exists
                    if (checkMember != null)
                    {
                        //Let's check they have verified their email address
                        if (checkMember.GetValue<bool>("hasVerifiedEmail"))
                        {
                            //Update number of logins counter
                            int noLogins = checkMember.GetValue<int>("numberOfLogins");

                            //Update the counter
                            checkMember.SetValue("numberOfLogins", noLogins + 1);

                            //Update label with last login date to now
                            checkMember.SetValue("lastLoggedIn", DateTime.Now.ToString("dd/MM/yyyy @ HH:mm:ss"));

                            //Update label with last logged in IP address & Host Name
                            string hostName = Dns.GetHostName();
                            string clientIPAddress = Dns.GetHostAddresses(hostName).GetValue(0).ToString();

                            checkMember.SetValue("hostNameOfLastLogin", hostName);
                            checkMember.SetValue("iPofLastLogin", clientIPAddress);

                            //Save the details
                            ms.Save(checkMember);

                            //If they have verified then lets log them in
                            //Set Auth cookie
                            FormsAuthentication.SetAuthCookie(model.EmailAddress, true);

                            //Once logged in - redirect them back to the return URL
                            return new RedirectResult(model.ReturnUrl);
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
            if (User?.Identity.IsAuthenticated == true)
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

        #endregion

        #region Forgotten/Reset Password

        /// <summary>
        /// Renders the Forgotten Password view
        /// @Html.Action("RenderForgottenPassword","AuthSurface");
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult RenderForgottenPassword()
        {
            return PartialView("ForgottenPassword", new ForgottenPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> HandleForgottenPassword(ForgottenPasswordViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("ForgottenPassword", model);

            //Find the member with the email address
            var findMember = Services.MemberService.GetByEmail(model.EmailAddress);

            if (findMember != null)
            {
                //Set expiry date to 
                DateTime expiryTime = DateTime.Now.AddMinutes(15);

                //Lets update resetGUID property
                findMember.SetValue("resetGUID", expiryTime.ToString("ddMMyyyyHHmmssFFFF"));

                Services.MemberService.Save(findMember);

                //Send user an email to reset password with GUID in it
                EmailHelper email = new EmailHelper();
                await email.SendResetPasswordEmail(findMember.Email, expiryTime.ToString("ddMMyyyyHHmmssFFFF"));
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
        [ChildActionOnly]
        public ActionResult RenderResetPassword()
        {
            return PartialView("ResetPassword", new ResetPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("ResetPassword", model);

            //Get member from email
            var resetMember = Services.MemberService.GetByEmail(model.EmailAddress);

            //Ensure we have that member
            if (resetMember != null)
            {
                //Get the querystring GUID
                var resetQS = Request.QueryString["resetGUID"];

                //Ensure we have a value in QS
                if (!string.IsNullOrEmpty(resetQS))
                {
                    //See if the QS matches the value on the member property
                    if (resetMember.GetValue<string>("resetGUID") == resetQS)
                    {
                        //Got a match, now check to see if the 15min window hasnt expired
                        DateTime expiryTime = DateTime.ParseExact(resetQS, "ddMMyyyyHHmmssFFFF", null);

                        //Check the current time is less than the expiry time
                        DateTime currentTime = DateTime.Now;

                        //Check if date has NOT expired (been and gone)
                        if (currentTime.CompareTo(expiryTime) < 0)
                        {
                            //Got a match, we can allow user to update password
                            Services.MemberService.SavePassword(resetMember, model.Password);

                            //Remove the resetGUID value
                            resetMember.SetValue("resetGUID", string.Empty);

                            //Save the member
                            Services.MemberService.Save(resetMember);

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

        #endregion

        /// <summary>
        /// Renders the Register View
        /// @Html.Action("RenderRegister","AuthSurface");
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult RenderRegister()
        {
            return PartialView("Register", new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> HandleRegister(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("Register", model);

            var ms = Services.MemberService;

            //Model valid let's create the member
            try
            {
                var createMember = ms.CreateMember(model.EmailAddress, model.EmailAddress, model.Name, "Member");

                //Set the verified email to false
                createMember.SetValue("hasVerifiedEmail", false);

                //Set the profile URL to be the member ID, so they have a unqie profile ID, until they go to set it
                createMember.SetValue("profileURL", createMember.Id);

                //Save the changes & password
                ms.Save(createMember);
                ms.SavePassword(createMember, model.Password);
            }
            catch (Exception ex)
            {
                //TODO-1: Handle any errors here and inform the member
                //TODO-2: Duplicate email address - already exists
                throw;
            }

            //If nothing failed while saving the member, we continue the onboarding:
            //Create temporary GUID
            var tempGUID = Guid.NewGuid();

            //Fetch our new member we created by their email
            var updateMember = ms.GetByEmail(model.EmailAddress);
            if (updateMember != null)
            {
                //Set the verification email GUID value on the member
                updateMember.SetValue("emailVerifyGUID", tempGUID.ToString());

                //Set the Joined Date label on the member
                updateMember.SetValue("joinedDate", DateTime.Now.ToString("dd/MM/yyyy @ HH:mm:ss"));

                //Save changes
                ms.Save(updateMember);
            }

            //Send out verification email, with GUID in it
            EmailHelper email = new EmailHelper();
            await email.SendVerifyEmail(model.EmailAddress, tempGUID.ToString());

            //TODO: Return a success message and perhaps redirect the user instead.
            //Return the view...
            return PartialView("Register", new RegisterViewModel());
        }

        /// <summary>
        /// Renders the Verify Email
        /// @Html.Action("RenderVerifyEmail","AuthSurface");
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult RenderVerifyEmail(string verifyGUID)
        {
            //Auto binds and gets guid from the querystring
            var findMember = Services.MemberService.GetMembersByPropertyValue("emailVerifyGuid", verifyGUID).FirstOrDefault();

            //Ensure we find a member with the verifyGUID
            if (findMember != null)
            {
                //We got the member, so let's update the verify email checkbox
                findMember.SetValue("hasVerifiedEmail", true);

                //Save the member
                Services.MemberService.Save(findMember);
            }
            else
            {
                //Couldn't find them - most likely invalid GUID
                return Redirect("/");
            }

            //Just in case...
            return Redirect("/");
        }


        #region REMOTE Validation

        /// <summary>
        /// Used with jQuery Validate to check when user registers that email address not already used
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public JsonResult CheckEmailIsUsed(string emailAddress)
        {
            //Try and get member by email typed in
            var checkEmail = Services.MemberService.GetByEmail(emailAddress);
            if (checkEmail != null)
            {
                return Json(String.Format("The email address '{0}' is already in use.", emailAddress), JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
