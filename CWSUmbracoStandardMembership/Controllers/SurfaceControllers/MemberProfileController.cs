using System;
using System.Linq;
using System.Web.Mvc;
using CWSUmbracoStandardMembership.Models;
using Umbraco.Web.Mvc;

namespace CWSUmbracoStandardMembership.Controllers.SurfaceControllers
{
    public class MemberProfileController : SurfaceController
    {
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
    }
}