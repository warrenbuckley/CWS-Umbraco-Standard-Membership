using System;
using System.Linq;
using System.Web.Mvc;
using CWSUmbracoStandardMembership.Models;
using Umbraco.Core.Models;
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

        /// <summary>
        /// This method uses the MemberTypeService to configure the defined memberType with all your custom properties, so that you don't have to add them manually
        /// You can extend or delete this for your own project
        /// @Html.ActionLink("Configure member properties", "ConfigureMemberProperties", "MemberProfile", new { memberTypeAlias = "Member" }, null)
        /// </summary>
        /// <returns></returns>
        public ActionResult ConfigureMemberProperties(string memberTypeAlias)
        {
            // Get the MemberTypeService and the MemberType
            var mts = Services.MemberTypeService;
            IMemberType memberType = mts.Get(memberTypeAlias);

            // Add the required properties:
            memberType.AddPropertyType(GetPropertyType("Joined Date", "joinedDate", PropertyEditor.Label));
            memberType.AddPropertyType(GetPropertyType("Host Name of Last Login", "hostNameOfLastLogin", PropertyEditor.Label));
            memberType.AddPropertyType(GetPropertyType("IP of Last Login", "iPOfLastLogin", PropertyEditor.Label));
            memberType.AddPropertyType(GetPropertyType("Number of Logins", "numberOfLogins", PropertyEditor.Label));
            memberType.AddPropertyType(GetPropertyType("Number of Profile Views", "numberOfProfileViews", PropertyEditor.Label));
            memberType.AddPropertyType(GetPropertyType("Last Logged In", "lastLoggedIn", PropertyEditor.Label));
            memberType.AddPropertyType(GetPropertyType("Reset GUID", "resetGUID", PropertyEditor.Label));
            memberType.AddPropertyType(GetPropertyType("Has Verified Email", "hasVerifiedEmail", PropertyEditor.Boolean));
            memberType.AddPropertyType(GetPropertyType("Email Verify GUID", "emailVerifyGUID", PropertyEditor.Label));
            memberType.AddPropertyType(GetPropertyType("Description", "description", PropertyEditor.Text));
            memberType.AddPropertyType(GetPropertyType("Profile URL", "profileURL", PropertyEditor.Label));

            // Add your custom properties:
            memberType.AddPropertyType(GetPropertyType("LinkedIn", "linkedIn", PropertyEditor.Text));
            memberType.AddPropertyType(GetPropertyType("Skype", "skype", PropertyEditor.Text));
            memberType.AddPropertyType(GetPropertyType("Twitter", "twitter", PropertyEditor.Text));

            // Update the membertype
            mts.Save(memberType);

            // Refresh the page
            return Redirect("/");
        }

        private PropertyType GetPropertyType(string name, string alias, PropertyEditor editor, bool isMandatory = false)
        {
            return new PropertyType(Services.DataTypeService.GetDataTypeDefinitionById((int)editor), alias)
            {
                Name = name,
                Mandatory = isMandatory
            };
        }

        // You can find the "ids" of datatypes, by going to Umbraco>Developer>Datatypes and notice the id in the url of the page
        private enum PropertyEditor : int
        {
            Text = -88,
            Label = -92,
            Numeric = -91,
            Datepicker = -41,
            Boolean = -49
        }
    }
}