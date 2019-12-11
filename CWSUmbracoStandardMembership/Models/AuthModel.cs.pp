﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace $rootnamespace$.Models
{
    /// <summary>
    /// Register View Model
    /// </summary>
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Remote("CheckEmailIsUsed", "AuthSurface", ErrorMessage = "The email address has already been registered")]
        public string EmailAddress { get; set; }

        [UIHint("Password")]
        [Required(ErrorMessage = "Please enter your password")]
        [MinLength(10, ErrorMessage = "Password must be at least 10 characters long")]
        public string Password { get; set; }

        [UIHint("Password")]
        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Please enter your password")]
        [EqualTo("Password", ErrorMessage = "Your passwords do not match")]
        public string ConfirmPassword { get; set; }

        [DisplayName("I accept the Terms and Conditions")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Please accept the terms in order to register")]
        public bool TermsAndConditions { get; set; }
    }

    /// <summary>
    /// Login View Model
    /// </summary>
    public class LoginViewModel
    {
        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string EmailAddress { get; set; }

        [UIHint("Password")]
        [Required(ErrorMessage = "Please enter your password")]
        [MinLength(10, ErrorMessage = "Password must be at least 10 characters long")]
        public string Password { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }
    }

    //Forgotten Password View Model
    public class ForgottenPasswordViewModel
    {
        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string EmailAddress { get; set; }
    }


    //Reset Password View Model
    public class ResetPasswordViewModel
    {
        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string EmailAddress { get; set; }

        [UIHint("Password")]
        [Required(ErrorMessage = "Please enter your password")]
        [MinLength(10, ErrorMessage = "Password must be at least 10 characters long")]
        public string Password { get; set; }

        [UIHint("Password")]
        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Please enter your password")]
        [EqualTo("Password", ErrorMessage = "Your passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}