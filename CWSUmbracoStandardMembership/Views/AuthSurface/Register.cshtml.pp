@using $rootnamespace$.Controllers.SurfaceControllers
@using $rootnamespace$.Models
@model RegisterViewModel
@{
    Html.EnableClientValidation(true);
    Html.EnableUnobtrusiveJavaScript(true);
}
@using (Html.BeginUmbracoForm<AuthSurfaceController>("HandleRegister"))
{
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(true)

    <h3>Register</h3>

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
        @Html.LabelFor(model => model.Password)
        @Html.EditorFor(model => model.Password, new { htmlAttributes = new { @class = "form-control" } })
        @Html.ValidationMessageFor(model => model.Password, null, new { @class = "help-block" })
    </div>

    <div class="form-group">
        @Html.LabelFor(model => model.ConfirmPassword)
        @Html.EditorFor(model => model.ConfirmPassword, new { htmlAttributes = new { @class = "form-control" } })
        @Html.ValidationMessageFor(model => model.ConfirmPassword, null, new { @class = "help-block" })
    </div>

    <div class="form-group">
        @Html.CheckBoxFor(x => x.TermsAndConditions)
        @Html.LabelFor(x => x.TermsAndConditions)
        @Html.ValidationMessageFor(x => x.TermsAndConditions, null, new { @class = "help-block" })
    </div>

    <div class="form-group">
        <button type="submit" class="btn btn-primary">Register</button>
    </div>
}