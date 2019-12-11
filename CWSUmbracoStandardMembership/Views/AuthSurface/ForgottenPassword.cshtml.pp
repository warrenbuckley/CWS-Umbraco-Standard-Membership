@using $rootnamespace$.Models
@using $rootnamespace$.Controllers.SurfaceControllers
@model ForgottenPasswordViewModel
@{
    Html.EnableClientValidation(true);
    Html.EnableUnobtrusiveJavaScript(true);
}
@using (Html.BeginUmbracoForm<AuthSurfaceController>("HandleForgottenPassword"))
{
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(true)

    <h3>Forgotten Password</h3>

    <div class="form-group">
        @Html.LabelFor(model => model.EmailAddress)
        @Html.EditorFor(model => model.EmailAddress, new { htmlAttributes = new { @class = "form-control" } })
        @Html.ValidationMessageFor(model => model.EmailAddress, null, new { @class = "help-block" })
    </div>
    <div class="form-group">
        <button type="submit" class="btn btn-primary">Remind me</button>
    </div>
}