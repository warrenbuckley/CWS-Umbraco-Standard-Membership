﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace $rootnamespace$.Models
{
    public class ProfileViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int MemberID { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Remote("CheckEmailIsUsed", "MemberEdit", ErrorMessage = "The email address has already been registered")]
        public string EmailAddress { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string MemberType { get; set; }

        [Required]
        [Remote("CheckProfileURLAvailable", "MemberEdit", ErrorMessage = "The profile URL is already in use")]
        public string ProfileURL { get; set; }

        public string Description { get; set; }

        public string Twitter { get; set; }

        [Url(ErrorMessage = "This is not a valid URL")]
        public string LinkedIn { get; set; }

        [MinLength(1, ErrorMessage = "It's unlikely your Skype username is a single letter")]
        public string Skype { get; set; }
    }
}