using System;
using System.Linq;
using System.Web.Mvc;
using CWSUmbracoStandardMembership.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web;
using System.Web.Security;

namespace CWSUmbracoStandardMembership.Controllers.SurfaceControllers
{
    [Authorize]
    public class MemberEditController : SurfaceController
    {
        /// <summary>
        /// Gets the logged in user, populates the model and returns it
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult RenderEditProfile()
        {
            // Get the current member
            var membershipHelper = new Umbraco.Web.Security.MembershipHelper(UmbracoContext.Current);
            var currentMember = membershipHelper.GetCurrentMember();

            ProfileViewModel profileModel = new ProfileViewModel();

            profileModel.Name           = currentMember.Name;
            profileModel.EmailAddress   = currentMember.GetPropertyValue<string>("Email");
            profileModel.MemberID       = currentMember.Id;
            profileModel.Description    = currentMember.GetPropertyValue<string>("description");
            profileModel.ProfileURL     = currentMember.GetPropertyValue<string>("profileURL");
            profileModel.Twitter        = currentMember.GetPropertyValue<string>("twitter");
            profileModel.LinkedIn       = currentMember.GetPropertyValue<string>("linkedIn");
            profileModel.Skype          = currentMember.GetPropertyValue<string>("skype");
                
            //Pass the model to the view
            return PartialView("EditProfile", profileModel);
        }

        /// <summary>
        /// Update the member with our data & save it down
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleEditProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("EditProfile", model);

            var updateMember = Services.MemberService.GetById(model.MemberID);

            updateMember.Name = model.Name;
            updateMember.Email = model.EmailAddress;
            updateMember.SetValue("description", model.Description);
            updateMember.SetValue("profileURL", model.ProfileURL);
            updateMember.SetValue("twitter", model.Twitter);
            updateMember.SetValue("linkedIn", model.LinkedIn);
            updateMember.SetValue("skype", model.Skype);

            //Save the member
            Services.MemberService.Save(updateMember);

            //Return the view
            return PartialView("EditProfile", model);
        }

        /// <summary>
        /// Renders the Change Password View
        /// @Html.Action("RenderChangePassword","MemberProfile");
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult RenderChangePassword()
        {
            return PartialView("ChangePassword", new ChangePasswordViewModel());
        }

        /// <summary>
        /// Update the logged in members password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult HandleChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("ChangePassword", model);

            var membershipHelper = new Umbraco.Web.Security.MembershipHelper(UmbracoContext.Current);

            // Get the current member
            var member = Services.MemberService.GetById(membershipHelper.GetCurrentMemberId());

            //TODO-1: Validate their current password

            //Save the password for the member
            Services.MemberService.SavePassword(member, model.Password);

            //TODO-2: Notify the member that their password was just changed

            //Return the view
            return PartialView("ChangePassword", model);
        }

        public ActionResult RenderDeleteProfile()
        {
            return PartialView("DeleteProfile");
        }

        /// <summary>
        /// Deletes a member and all their associated data
        /// @Html.ActionLink("Delete Profile", "HandleDeleteProfile", "MemberEdit")
        /// </summary>
        /// <returns></returns>
        public ActionResult HandleDeleteProfile()
        {
            try
            {
                // Get the current member
                var membershipHelper = new Umbraco.Web.Security.MembershipHelper(UmbracoContext.Current);
                var member = Services.MemberService.GetById(membershipHelper.GetCurrentMemberId());

                //TODO-OPTIONAL:    Notify the member that their profile is being deleted in x days, giving them a couple of days to cancel this. 
                //                  -> Then delete the member with a scheduled task automatically after that period of time.

                // Delete the member
                Services.MemberService.Delete(member);
            }
            catch (Exception ex)
            {
                //TODO-1: Log the exception
                throw;
            }

            // Log the member out
            FormsAuthentication.SignOut();

            // Redirect home or to a custom page
            return Redirect("/");
        }

        #region REMOTE Validation

        public JsonResult CheckEmailIsUsed(string emailAddress)
        {
            var membershipHelper = new Umbraco.Web.Security.MembershipHelper(UmbracoContext.Current);

            //Get Current Member
            var member = membershipHelper.GetCurrentMember();
            if (member != null)
            {
                //if the email is the same as the one stored then it's OK
                if (member.GetPropertyValue<string>("Email") == emailAddress) return Json(true, JsonRequestBehavior.AllowGet);

                //Try and get member by email typed in
                var checkEmail = Services.MemberService.GetByEmail(emailAddress);
                if (checkEmail != null) return Json(String.Format("The email address '{0}' is already in use.", emailAddress), JsonRequestBehavior.AllowGet);

                return Json(true, JsonRequestBehavior.AllowGet);
            }

            // No member found
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckProfileURLAvailable(string profileURL)
        {
            var membershipHelper = new Umbraco.Web.Security.MembershipHelper(UmbracoContext.Current);

            //Get Current Member
            var member = membershipHelper.GetCurrentMember();
            if (member != null)
            {
                //If the Url is the same as the one currently stored for this user, return Ok
                if (member.GetPropertyValue<string>("profileURL") == profileURL) return Json(true, JsonRequestBehavior.AllowGet);

                //Check if an exisiting member is already using this Url:  
                //TODO-1: Maybe just exclude the current user in below search and delete the line above?
                var checkProfileURL = Services.MemberService.GetMembersByPropertyValue("profileUrl", profileURL).FirstOrDefault(); 

                //If an exisiting member was found, then return negative
                if (checkProfileURL != null) return Json(String.Format("The profile URL '{0}' is already in use.", profileURL), JsonRequestBehavior.AllowGet);

                //Otherwise everything is good
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            // No member found
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}