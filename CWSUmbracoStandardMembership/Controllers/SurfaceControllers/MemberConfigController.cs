using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Umbraco.Core.Models;

namespace CWSUmbracoStandardMembership.Controllers.SurfaceControllers
{
    public class MemberConfigController : SurfaceController
    {
        /// <summary>
        /// This method uses the MemberTypeService to configure the defined memberType with all your custom properties, so that you don't have to add them manually
        /// You can extend or delete this for your own project
        /// @Html.ActionLink("Configure member properties", "ConfigureMemberProperties", "MemberConfig", new { memberTypeAlias = "Member" }, null)
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