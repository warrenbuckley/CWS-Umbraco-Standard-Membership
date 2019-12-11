@using $rootnamespace$.Models
@using $rootnamespace$.Controllers.SurfaceControllers
@model ResetPasswordViewModel
@{
    Html.EnableClientValidation(true);
    Html.EnableUnobtrusiveJavaScript(true);
}
@if (!ViewData.ModelState.IsValid)
{
    <h3>Errors</h3>
    foreach (ModelState modelState in ViewData.ModelState.Values)
    {
        var errors = modelState.Errors;

        if (errors.Any())
        {
            <ul>
                @foreach (ModelError error in errors)
                {
                    <li><em>@error.ErrorMessage</em></li>
                }
            </ul>
        }
    }
}
@using (Html.BeginUmbracoForm<AuthSurfaceController>("HandleResetPassword"))
{
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(true)

    <h3>Reset your Password</h3>

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
        <button type="submit" class="btn btn-primary">Reset Password</button>
    </div>
}