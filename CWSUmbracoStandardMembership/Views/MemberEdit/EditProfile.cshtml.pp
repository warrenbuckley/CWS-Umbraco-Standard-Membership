@using $rootnamespace$.Controllers.SurfaceControllers
@using $rootnamespace$.Models
@model ProfileViewModel
@{
    Html.EnableClientValidation(true);
    Html.EnableUnobtrusiveJavaScript(true);
}
@using (Html.BeginUmbracoForm<MemberEditController>("HandleEditProfile", FormMethod.Post))
{
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(true)

    <h3>Edit Profile</h3>

    <div class="form-group">
        @Html.LabelFor(model => model.Name)
        @Html.EditorFor(model => model.Name, new { htmlAttributes = new { @class = "form-control" } })
        @Html.ValidationMessageFor(model => model.Name, null, new { @class = "help-block" })
    </div>

    <div class="form-group">
        @Html.LabelFor(model => model.EmailAddress)
        @Html.EditorFor(model => model.EmailAddress, new { htmlAttributes = new { @class = "form-control" } })
        @Html.ValidationMessageFor(model => model.EmailAddress, null, new { @class = "help-block" })
    </div>


    <div class="form-group">
        @Html.LabelFor(model => model.Description)
        @Html.EditorFor(model => model.Description, new { htmlAttributes = new { @class = "form-control" } })
        @Html.ValidationMessageFor(model => model.Description, null, new { @class = "help-block" })
    </div>

    <div class="form-group">
        @Html.LabelFor(model => model.ProfileURL)
        @Html.EditorFor(model => model.ProfileURL, new { htmlAttributes = new { @class = "form-control" } })
        @Html.ValidationMessageFor(model => model.ProfileURL, null, new { @class = "help-block" })
    </div>

    <div class="form-group">
        @Html.LabelFor(model => model.Twitter)
        @Html.EditorFor(model => model.Twitter, new { htmlAttributes = new { @class = "form-control" } })
        @Html.ValidationMessageFor(model => model.Twitter, null, new { @class = "help-block" })
    </div>

    <div class="form-group">
        @Html.LabelFor(model => model.LinkedIn)
        @Html.EditorFor(model => model.LinkedIn, new { htmlAttributes = new { @class = "form-control" } })
        @Html.ValidationMessageFor(model => model.LinkedIn, null, new { @class = "help-block" })
    </div>

    <div class="form-group">
        @Html.LabelFor(model => model.Skype)
        @Html.EditorFor(model => model.Skype, new { htmlAttributes = new { @class = "form-control" } })
        @Html.ValidationMessageFor(model => model.Skype, null, new { @class = "help-block" })
    </div>

    <div class="form-group">
        @Html.HiddenFor(model => model.MemberID)
        <button type="submit" class="btn btn-primary">Update</button>
    </div>
}