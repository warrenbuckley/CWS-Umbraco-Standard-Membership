# Umbraco Standard Membership framework
This is starting point for building a membership-enabled website in Umbraco v7+.
The goal of this repository is to offer a standard framework, that is fully GDPR compliant, so that anyone can start building awesome Umbraco membership websites.

## Setup

Simply install the Nuget package to install the following:
* Code/EmailHelper.cs
* Code/Helpers.cs
* Code/UmbracoStartup.cs
* Controllers/SurfaceControllers/AuthSurfaceController.cs
* Controllers/SurfaceControllers/MemberConfigController.cs
* Controllers/SurfaceControllers/MemberEditController.cs
* Controllers/SurfaceControllers/MemberProfileController.cs
* Models/AuthModel.cs
* Models/ChangePasswordViewModel.cs
* Models/ProfileModel.cs
* Models/ViewProfileViewModel.cs
* Scripts/jquery.unobtrusive-ajax.js
* Scripts/jquery.unobtrusive-ajax.min.js
* Scripts/jquery.validate.js
* Scripts/jquery.validate.min.js
* Scripts/jquery.validate.unobtrusive.js
* Scripts/jquery.validate.unobtrusive.min.js
* Scripts/jquery.validate-vsdoc.js
* Scripts/jquery-1.8.2.intellisense.js
* Scripts/jquery-1.8.2.js
* Scripts/jquery-1.8.2.min.js
* Views/AuthSurface/ForgottenPassword.cshtml
* Views/AuthSurface/Login.cshtml
* Views/AuthSurface/Register.cshtml
* Views/AuthSurface/ResetPassword.cshtml
* Views/MemberEdit/ChangePassword.cshtml
* Views/MemberEdit/DeleteProfile.cshtml
* Views/MemberEdit/EditProfile.cshtml
* Views/MemberProfile/ViewProfile.cshtml

If you wish to use the ChangePassword functionality, you need to set the **allowManuallyChangingPassword** to true in your web.config, like so:

    <add name="UmbracoMembershipProvider" type="Umbraco.Web.Security.Providers.MembersMembershipProvider, Umbraco" minRequiredNonalphanumericCharacters="0" minRequiredPasswordLength="10" useLegacyEncoding="false" enablePasswordRetrieval="false" enablePasswordReset="false" requiresQuestionAndAnswer="false" defaultMemberTypeAlias="Member" passwordFormat="Hashed" allowManuallyChangingPassword="true" />

## Umbraco Member Setup
In order to use the code properly, your member type in Umbraco will need to support the new properties.

### Automagic setup
The code includes a **MemberConfigController** that can automagically set up the required properties for you. Include this in a view, click the action, and verify that the properties were added to the member type

    @Html.ActionLink("Configure member properties", "ConfigureMemberProperties", "MemberConfig", new { memberTypeAlias = "Member" }, null)
You can change the **memberTypeAlias** if you wish to configure another, or extend the method directly from the Controller.

### Manual setup

If you want to add the properties manually from the Umbraco backoffice, here are the required ones:

<table>
<thead>
  <tr>
		<th>Property Name</th>
		<th>Property Alias</th>
		<th>Property Type</th>
	</tr>
</thead>
<tbody>
<tr>
	<td>Joined Date</td>
	<td>joinedDate</td>
	<td>Label</td>
</tr>
<tr>
	<td>Host Name of Last Login</td>
	<td>hostNameOfLastLogin</td>
	<td>Label</td>
</tr>
<tr>
	<td>IP of Last Login</td>
	<td>iPOfLastLogin</td>
	<td>Label</td>
</tr>
<tr>
	<td>Number of Logins</td>
	<td>numberOfLogins</td>
	<td>Label</td>
</tr>
<tr>
	<td>Number of Profile Views</td>
	<td>numberOfProfileViews</td>
	<td>Label</td>
</tr>
<tr>
	<td>Last Logged In</td>
	<td>lastLoggedIn</td>
	<td>Label</td>
</tr>
<tr>
	<td>Reset GUID</td>
	<td>resetGUID</td>
	<td>Label</td>
</tr>
<tr>
	<td>Has Verified Email</td>
	<td>hasVerifiedEmail</td>
	<td>True/False</td>
</tr>
<tr>
	<td>Email Verify GUID</td>
	<td>emailVerifyGUID</td>
	<td>Label</td>
</tr>
<tr>
	<td>Description</td>
	<td>description</td>
	<td>Textbox</td>
</tr>
<tr>
	<td>Twitter</td>
	<td>twitter</td>
	<td>Textbox</td>
</tr>
<tr>
	<td>LinkedIn</td>
	<td>linkedIn</td>
	<td>Textbox</td>
</tr>
<tr>
	<td>Skype</td>
	<td>skype</td>
	<td>Textbox</td>
</tr>
<tr>
	<td>Profile URL</td>
	<td>profileURL</td>
	<td>Textbox</td>
</tr>
</table>

# Roadmap
Contributions are more than welcome. Here are the current items in the roadmap, feel free to add an issue if you have a suggestion.

- **[GDPR]** As a Member, I should be able to withdraw my consent (given at signup).
- **[GDPR]** As a Member, I should be able to download all my data in a readable format.
- **[DeleteProfile]** As a Member, I should receive an email notification when attempting to delete my profile, stating that my profile will be deleted in x days. 
- **[DeleteProfile]** As a Member, I should be able to cancel my profile deletion (via the link in the email notification)
- **[DeleteProfile]** Implement a scheduled task that deletes member profiles after x amount of days. 
- **[Project]** Add unit testing to the project, to ensure everything works and supply a template for extension
