CWS Umbraco Standard Membership
===========================
This is used to give an example standard membership to Umbraco V6+


Setup
===========================
Simply install the Nuget package to install the following:

* Code/EmailHelper.cs
* Controllers/SurfaceControllers/AuthSurfaceController.cs
* Models/AuthModel.cs
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


Umbraco Member Setup
===========================
Your member type in the Umbraco backoffice will need the following properties added to the member type:

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
</table>
