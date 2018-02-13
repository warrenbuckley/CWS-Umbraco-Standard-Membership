using System;
using System.Linq;
using System.Web.Mvc;
using CWSUmbracoStandardMembership.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web;

namespace CWSUmbracoStandardMembership.Controllers.SurfaceControllers
{
    public class ProfileSurfaceController : SurfaceController
    {
        /// <summary>
        /// Gets the logged in user, populates the model and returns it
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult RenderEditProfile()
        {
            var membershipHelper = new Umbraco.Web.Security.MembershipHelper(UmbracoContext.Current);

            //If user is logged in then let's create the model
            if (User?.Identity.IsAuthenticated == true)
            {
                ProfileViewModel profileModel = new ProfileViewModel();

                //Let's fill it up
                var currentMember = membershipHelper.GetCurrentMember();

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
            else
            {
                //They are not logged in, redirect to home
                return Redirect("/");
            }
        }

        /// <summary>
        /// Update the member with our data & save it down
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
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

        public ActionResult RenderMemberProfile(string profileURLtoCheck)
        {
            //Try and find member with the QueryString value ?profileURLtoCheck=warrenbuckley
            var findMember = Services.MemberService.GetMembersByPropertyValue("profileURL", profileURLtoCheck).FirstOrDefault();

            //Check if we found member
            if (findMember != null)
            {
                //Create a view model
                ViewProfileViewModel profile = new ViewProfileViewModel();

                //Increment profile view counter by one
                int noOfProfileViews = findMember.GetValue<int>("numberOfProfileViews");
                findMember.SetValue("numberOfProfileViews", noOfProfileViews + 1);

                //Save it down to the member
                Services.MemberService.Save(findMember);

                //Got the member lets bind the data to the view model
                profile.Name                    = findMember.Name;
                profile.MemberID                = findMember.Id;
                profile.EmailAddress            = findMember.Email;
                profile.MemberType              = string.Join(",", System.Web.Security.Roles.GetRolesForUser(findMember.Username));

                profile.Description             = findMember.GetValue<string>("description");

                profile.LinkedIn                = findMember.GetValue<string>("linkedIn");
                profile.Skype                   = findMember.GetValue<string>("skype");
                profile.Twitter                 = findMember.GetValue<string>("twitter");

                profile.NumberOfLogins          = findMember.GetValue<int>("numberOfLogins");
                profile.LastLoginDate           = DateTime.ParseExact(findMember.GetValue<string>("lastLoggedIn"), "dd/MM/yyyy @ HH:mm:ss", null);
                profile.NumberOfProfileViews    = findMember.GetValue<int>("numberOfProfileViews");
                
                return PartialView("ViewProfile", profile);
            }
            else
            {
                //Couldn't find the member return a 404
                return new HttpNotFoundResult("The member profile does not exist");
            }
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
                //TODO-1: Maybe just don't search for current user below and skip the line above?
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